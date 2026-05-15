using UnityEngine;

// Reads player input and forwards parry attempts to RhythmJudge. No game logic lives here.
// Attach to the Player GameObject.
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private RhythmJudge rhythmJudge;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            HandleParryInput();
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