using System;
using UnityEngine;


// Tracks time and fires a beat event at a given BPM.
// Kept separate from RhythmSpawner so AudioManager and VFX can also subscribe.

public class BeatClock : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float bpm = 120f;

    public float BeatInterval => 60f / bpm;
    public int CurrentBeat { get; private set; }

    //Fired every beat. Carries beat index
    public event Action<int> OnBeat;

    private float _accumulator;
    private bool _running;

    private void Update()
    {
        if (!_running) return;

        _accumulator += Time.deltaTime;

        if (_accumulator >= BeatInterval)
        {
            _accumulator -= BeatInterval;   // keep the remainder — no drift
            CurrentBeat++;
            OnBeat?.Invoke(CurrentBeat);
        }
    }

    public void StartClock()
    {
        _accumulator = 0f;
        CurrentBeat = 0;
        _running = true;
    }

    public void StopClock() => _running = false;
    public void PauseClock() => _running = false;
    public void ResumeClock() => _running = true;

    // Returns how many seconds the player is away from the nearest beat.
    // Used by RhythmJudge to grade parry timing.  
    public float DistanceToNearestBeat()
    {
        float half = BeatInterval * 0.5f;
        float pos = _accumulator % BeatInterval;
        return pos <= half ? pos : BeatInterval - pos;
    }
}