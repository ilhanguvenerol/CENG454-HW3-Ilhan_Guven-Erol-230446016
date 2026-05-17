
//Basic parry that interacts directly to RhythmJudge.
public class BaseParryAction : IParryAction
{
    public ParryGrade Execute(RhythmJudge judge)
    {
        return judge.EvaluateParry();
    }
}
