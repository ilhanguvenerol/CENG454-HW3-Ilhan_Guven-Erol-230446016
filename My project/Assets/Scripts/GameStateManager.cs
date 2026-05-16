using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Observes CoreHealth only. Transitions between game states
// Fires its own OnGameOver / OnVictory events for any further subscribers.
public class GameStateManager : MonoBehaviour
{
    public enum State { Playing, Victory, GameOver }

    [Header("References")]
    [SerializeField] private CoreHealth coreHealth;
    [SerializeField] private BeatClock beatClock;

    [Header("Win condition")]
    [Tooltip("Survive this many seconds to win")]
    [SerializeField] private float survivalDuration = 120f;

    [Header("Transition")]
    [SerializeField] private float delayBeforeReload = 2.5f;
    [SerializeField] private string gameOverScene = "GameOver";
    [SerializeField] private string victoryScene = "Victory";

    // UI Observable: can subscribe cinematics, but honestly i don't see it happening i have less than 24 hours left :)
    public event System.Action<State> OnStateChanged;

    public State CurrentState { get; private set; } = State.Playing;

    private float _elapsed;

    //Lifecycle
    private void OnEnable()
    {
        coreHealth.OnDestroyed += HandleCoreDestroyed;
        beatClock.StartClock();
    }

    private void OnDisable()
    {
        coreHealth.OnDestroyed -= HandleCoreDestroyed;
    }

    private void Update()
    {
        if (CurrentState != State.Playing) return;

        _elapsed += Time.deltaTime;

        if (_elapsed >= survivalDuration)
            Transition(State.Victory);
    }

    //Handlers
    private void HandleCoreDestroyed() => Transition(State.GameOver);

    //State machine
    private void Transition(State next)
    {
        if (CurrentState != State.Playing) return;

        CurrentState = next;
        beatClock.StopClock();
        OnStateChanged?.Invoke(next);

        StartCoroutine(LoadSceneDelayed(
            next == State.Victory ? victoryScene : gameOverScene));
    }

    private IEnumerator LoadSceneDelayed(string sceneName)
    {
        yield return new WaitForSeconds(delayBeforeReload);
        SceneManager.LoadScene(sceneName);
    }
}