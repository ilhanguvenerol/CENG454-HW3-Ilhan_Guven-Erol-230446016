
//Abstract decorator base
public abstract class ParryDecorator : IParryAction
{
    protected readonly IParryAction _inner;

    protected ParryDecorator(IParryAction inner)
    {
        _inner = inner;
    }

    // Default: pure pass-through. Concrete decorators override this.
    public virtual ParryGrade Execute(RhythmJudge judge)
    {
        return _inner.Execute(judge);
    }
}