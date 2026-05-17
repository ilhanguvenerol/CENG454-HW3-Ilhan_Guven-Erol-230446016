using UnityEngine;

// Reads player input and forwards parry attempts to RhythmJudge. No game logic lives here.
// Attach to the Player GameObject.
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private RhythmJudge rhythmJudge;

    //plain base parry, replaced by decorators
    private IParryAction _parryAction;

    private void Awake()
    {
        _parryAction = new BaseParryAction();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            HandleParryInput();
    }

    //wrap the current action in a decorator
    public void WrapParry(System.Func<IParryAction, IParryAction> decorator)
    {
        _parryAction = decorator(_parryAction);
    }

    private void HandleParryInput()
    {
        ParryGrade grade = rhythmJudge.EvaluateParry();

        switch (grade)
        {
            case ParryGrade.Perfect:
            case ParryGrade.Good:
                break;
            case ParryGrade.Miss:
                break;
        }
    }
}