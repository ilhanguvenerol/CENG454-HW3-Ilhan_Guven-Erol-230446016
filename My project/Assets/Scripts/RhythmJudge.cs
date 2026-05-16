using System;
using UnityEngine;

// THE JUDGEMENT FRAME IS UPON YOUR INPUT, LISTENER!
//      Grades the player's input against the current beat
// TOUCH THE BUTTON TO SAVE YOUR EARS
//      It's a second Observable that fires OnParry and OnMiss; so AudioManager, HUDController, VFXManager can react independently.
//
//      Attach to a persistent manager GameObject alongside BeatClock.
public class RhythmJudge : MonoBehaviour
{
    //Timing window
    [Header("Timing Windows")]
    [Tooltip("Within this distance from the beat = Perfect")]
    [SerializeField] private float perfectWindow = 0.06f;

    [Tooltip("Within this distance from the beat = Good")]
    [SerializeField] private float goodWindow    = 0.12f;

    // Outside goodWindow = Miss

    [Header("References")]
    [SerializeField] private BeatClock beatClock;

    //Observable events
    public event Action<ParryGrade> OnParry; //Fired on a successful parry.
    public event Action OnMiss; //Fired when the player misses (wrong timing or no input)
    public event Action OnProjectileHitCore; // Fired when a projectile reaches the core unblocked.

    //Combo tracking (observable state)
    public int  CurrentCombo    { get; private set; } //going to use current combo to activate a bonus, which will use decorator pattern.
    public int  BestCombo       { get; private set; }

    //Public methods will called by PlayerInput
    public ParryGrade EvaluateParry() //call on player input. returns the grade
    {
        float distance = beatClock.DistanceToNearestBeat();
        ParryGrade grade = Classify(distance);

        if (grade == ParryGrade.Miss)
        {
            RegisterMiss();
        }
        else
        {
            CurrentCombo++;
            BestCombo = Mathf.Max(BestCombo, CurrentCombo);
            OnParry?.Invoke(grade);
        }

        return grade;
    }

    public void NotifyProjectileHitCore() //call on projectile hit. break combo and notify subscribers
    {
        RegisterMiss();
        OnProjectileHitCore?.Invoke();
    }

    //Private methods
    private ParryGrade Classify(float distanceToBeat)
    {
        if (distanceToBeat <= perfectWindow) return ParryGrade.Perfect;
        if (distanceToBeat <= goodWindow)    return ParryGrade.Good;
        return ParryGrade.Miss;
    }

    private void RegisterMiss()
    {
        CurrentCombo = 0;
        OnMiss?.Invoke();
    }
}

public enum ParryGrade
{
    Perfect,
    Good,
    Miss
}