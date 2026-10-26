using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class NetworkLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private int arenaBuildIndex = 1;
    [SerializeField] private int maxPlayers = 2;
    [SerializeField] private float minWaitSeconds = 6f;
    [SerializeField] private float maxWaitSeconds = 12f;

    public event Action<string> StatusChanged;
    public event Action<bool> SearchStateChanged;

    private NetworkRunner runner;
    private GameObject runnerObject;
    private bool searching;
    private bool connected;
    private bool arenaLoading;
    private bool restarting;

    private void Awake() => DontDestroyOnLoad(gameObject);

    public async void FindMatch()
    {
        if (searching) return;
        searching = true;
        arenaLoading = false;
        SearchStateChanged?.Invoke(true);

        while (searching)
        {
            if (!await Connect())
            {
                if (this == null) return;
                EndSearch();
                break;
            }

            if (this == null) return;

            if (!searching)
            {
                await ShutdownRunner();
                if (this == null) return;
                EndSearch();
                break;
            }

            if (runner == null)
            {
                EndSearch();
                break;
            }

            if (!runner.IsServer)
            {
                SetStatus("Opponent found, loading...");
                break;
            }

            SetStatus("Searching for opponent...");
            float wait = UnityEngine.Random.Range(minWaitSeconds, maxWaitSeconds);
            for (float t = 0f; t < wait && searching && !arenaLoading; t += Time.unscaledDeltaTime)
            {
                if (this == null) return;
                await Task.Yield();
            }

            if (this == null) return;
            if (!searching || arenaLoading) break;

            SetStatus("Continuing search...");
            await ShutdownRunner();
            if (this == null) return;
        }
    }

    public async void CancelSearch()
    {
        if (!searching) return;
        searching = false;
        SetStatus("Search cancelled");

        if (connected)
            await ShutdownRunner();

        if (this == null) return;
        EndSearch();
    }

    private void OnDestroy()
    {
        searching = false;
        StatusChanged = null;
        SearchStateChanged = null;
        ResetRunner();
    }

    private async Task<bool> Connect()
    {
        connected = false;

        runnerObject = new GameObject("NetworkRunner");
        runnerObject.transform.SetParent(transform);
        runner = runnerObject.AddComponent<NetworkRunner>();
        runner.AddCallbacks(this);
        var sceneManager = runnerObject.AddComponent<NetworkSceneManagerDefault>();

        SetStatus("Connecting...");
        var result = await runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.AutoHostOrClient,
            PlayerCount = maxPlayers,
            SceneManager = sceneManager
        });

        if (!result.Ok)
        {
            SetStatus($"Error: {result.ShutdownReason}");
            ResetRunner();
            return false;
        }

        connected = true;
        return true;
    }

    private async Task ShutdownRunner()
    {
        if (runner == null) return;
        restarting = true;
        await runner.Shutdown();
        ResetRunner();
        restarting = false;
    }

    private void ResetRunner()
    {
        if (runnerObject != null) Destroy(runnerObject);
        runner = null;
        runnerObject = null;
        connected = false;
    }

    private void EndSearch()
    {
        searching = false;
        SearchStateChanged?.Invoke(false);
    }

    private void SetStatus(string text)
    {
        Debug.Log($"[Network] {text}");
        StatusChanged?.Invoke(text);
    }

    public void OnPlayerJoined(NetworkRunner r, PlayerRef player)
    {
        if (!r.IsServer || arenaLoading) return;

        if (r.ActivePlayers.Count() >= maxPlayers)
        {
            arenaLoading = true;
            r.SessionInfo.IsOpen = false;
            SetStatus("Opponent found!");
            r.LoadScene(SceneRef.FromIndex(arenaBuildIndex));
        }
    }

    public void OnPlayerLeft(NetworkRunner r, PlayerRef player)
        => Debug.Log($"Player {player} left");

    public void OnShutdown(NetworkRunner r, ShutdownReason reason)
    {
        if (restarting) return;
        bool wasSearching = searching;
        ResetRunner();
        EndSearch();
        if (wasSearching) SetStatus($"Disconnected: {reason}");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ReadOnlySpan<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
}