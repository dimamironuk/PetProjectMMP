using UnityEngine;
using Zenject;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private int cardsToDrawPerRound = 4;
    [SerializeField] private TurnTimer turnTimer;

    [Inject] private IDeck deck;
    [Inject] private HandManager hand;
    [Inject] private ITokenPool tokens;
    [Inject] private IBoard board;

    private Coroutine drawRoutine;

    private void Awake()
    {
        if (turnTimer == null)
            turnTimer = FindObjectOfType<TurnTimer>();
    }

    private void OnEnable()
    {
        if (turnTimer != null)
            turnTimer.OnTimerExpired += StartNewRound;
    }

    private void OnDisable()
    {
        if (turnTimer != null)
            turnTimer.OnTimerExpired -= StartNewRound;

        if (drawRoutine != null)
        {
            StopCoroutine(drawRoutine);
            drawRoutine = null;
        }
    }

    private void Start() => StartNewRound();

    public void StartNewRound()
    {
        tokens.StartRound();

        foreach (var unit in board.Characters)
            unit.OnRoundStart();

        if (drawRoutine != null)
            StopCoroutine(drawRoutine);

        drawRoutine = StartCoroutine(hand.DrawRoutine(deck, cardsToDrawPerRound));
        turnTimer?.Restart();
    }
}
