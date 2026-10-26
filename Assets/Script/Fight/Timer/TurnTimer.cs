using System;
using TMPro;
using UnityEngine;

public class TurnTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI minText;
    [SerializeField] private TextMeshProUGUI secText;
    [SerializeField] private float defaultDuration = 120f;
    [SerializeField] private bool autoStart;
    [SerializeField] private bool useUnscaledTime;

    public event Action OnTimerExpired;

    public bool IsRunning { get; private set; }
    public float RemainingTime { get; private set; }

    private Action onComplete;
    private int lastDisplayedSecond = -1;

    private void Start()
    {
        if (autoStart) StartTimer(defaultDuration);
    }

    private void Update()
    {
        if (!IsRunning) return;

        RemainingTime -= useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

        if (RemainingTime <= 0f)
        {
            Expire();
            return;
        }

        UpdateDisplay(RemainingTime);
    }

    private void OnDestroy()
    {
        StopTimer();
        OnTimerExpired = null;
    }

    public void Restart() => StartTimer(defaultDuration);

    public void StartTimer(float durationInSeconds, Action onCompleteCallback = null)
    {
        onComplete = onCompleteCallback;
        RemainingTime = Mathf.Max(0f, durationInSeconds);
        lastDisplayedSecond = -1;

        if (RemainingTime <= 0f)
        {
            Expire();
            return;
        }

        IsRunning = true;
        UpdateDisplay(RemainingTime);
    }

    public void StopTimer()
    {
        IsRunning = false;
        onComplete = null;
    }

    private void Expire()
    {
        RemainingTime = 0f;
        IsRunning = false;
        UpdateDisplay(0f);

        Action callback = onComplete;
        onComplete = null;

        OnTimerExpired?.Invoke();
        callback?.Invoke();
    }

    private void UpdateDisplay(float timeToDisplay)
    {
        int totalSeconds = Mathf.CeilToInt(timeToDisplay);
        if (totalSeconds == lastDisplayedSecond) return;
        lastDisplayedSecond = totalSeconds;

        if (minText != null) minText.text = (totalSeconds / 60).ToString("00");
        if (secText != null) secText.text = (totalSeconds % 60).ToString("00");
    }
}