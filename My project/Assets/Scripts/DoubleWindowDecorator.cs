using UnityEngine;


//temporarily double the perfect and good timing windows on RhythmJudge
public class DoubleWindowDecorator : ParryDecorator
{
    private readonly float _duration;
    private float _elapsed;
    private bool _active = true;

    // Cached originals so i can restore them later
    private float _originalPerfect;
    private float _originalGood;

    public DoubleWindowDecorator(IParryAction inner, float duration) : base(inner)
    {
        _duration = duration;
    }

    public override ParryGrade Execute(RhythmJudge judge)
    {
        // Tick duration on each parry attempt (frame-independent alternative:
        // tick in a MonoBehaviour Update and call Expire() when done)
        _elapsed += Time.deltaTime;

        if (!_active || _elapsed >= _duration)
        {
            Expire(judge);
            return _inner.Execute(judge);   // pass through without widening
        }

        // Widen, evaluate, restore so no change is permanent
        Widen(judge);
        ParryGrade grade = _inner.Execute(judge);
        Restore(judge);

        return grade;
    }

    //Helpers
    private void Widen(RhythmJudge judge)
    {
        _originalPerfect = judge.PerfectWindow;
        _originalGood = judge.GoodWindow;

        judge.PerfectWindow = _originalPerfect * 2f;
        judge.GoodWindow = _originalGood * 2f;
    }

    private void Restore(RhythmJudge judge)
    {
        judge.PerfectWindow = _originalPerfect;
        judge.GoodWindow = _originalGood;
    }

    private void Expire(RhythmJudge judge)
    {
        if (!_active) return;
        _active = false;
        Restore(judge); //failsafe for incorrect calculation
    }

    public bool IsExpired => !_active || _elapsed >= _duration;
}