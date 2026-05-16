using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Observes CoreHealth and RhythmJudge events and updates all UI elements in response
public class HUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CoreHealth coreHealth;
    [SerializeField] private RhythmJudge rhythmJudge;

    [Header("HP Bar")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Image hpFill;
    [SerializeField] private Color colorHealthy = Color.green;
    [SerializeField] private Color colorCritical = Color.red;

    [Header("Combo")]
    [SerializeField] private TextMeshProUGUI comboText;

    [Header("Grade Flash")]
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private float gradeDisplaySeconds = 0.6f;

    [Header("Screen Flash")]
    [SerializeField] private Image screenFlash;
    [SerializeField] private Color damageFlashColor = new Color(1f, 0f, 0f, 0.25f);
    [SerializeField] private float flashDuration = 0.15f;

    private Coroutine _gradeRoutine;
    private Coroutine _flashRoutine;

    //Lifecycle
    private void OnEnable()
    {
        coreHealth.OnDamaged += HandleCoreDamaged;
        coreHealth.OnDestroyed += HandleCoreDestroyed;
        rhythmJudge.OnParry += HandleParry;
        rhythmJudge.OnMiss += HandleMiss;
    }

    private void OnDisable()
    {
        coreHealth.OnDamaged -= HandleCoreDamaged;
        coreHealth.OnDestroyed -= HandleCoreDestroyed;
        rhythmJudge.OnParry -= HandleParry;
        rhythmJudge.OnMiss -= HandleMiss;
    }

    private void Start()
    {
        // start full health
        if (hpSlider != null)
        {
            hpSlider.minValue = 0;
            hpSlider.maxValue = 1;
            hpSlider.value = 1;
        }

        SetGradeVisible(false);
        SetFlashVisible(false);
        UpdateCombo(0);
    }

    //Handlers
    private void HandleCoreDamaged(int current, int max)
    {
        float ratio = (float)current / max;

        if (hpSlider != null) hpSlider.value = ratio;
        if (hpFill != null) hpFill.color = Color.Lerp(colorCritical, colorHealthy, ratio);

        TriggerFlash(damageFlashColor);
    }

    private void HandleCoreDestroyed()
    {
        TriggerFlash(Color.red);
        ShowGrade("CORE LOST", Color.red);
    }

    private void HandleParry(ParryGrade grade)
    {
        UpdateCombo(rhythmJudge.CurrentCombo);

        switch (grade)
        {
            case ParryGrade.Perfect:
                ShowGrade("PERFECT", Color.cyan);
                break;
            case ParryGrade.Good:
                ShowGrade("GOOD", Color.yellow);
                break;
        }
    }

    private void HandleMiss()
    {
        UpdateCombo(0);
        ShowGrade("MISS", Color.red);
    }

    //UI helpers
    private void UpdateCombo(int combo)
    {
        if (comboText == null) return;
        comboText.text = combo > 1 ? $"x{combo}" : string.Empty;
    }

    private void ShowGrade(string label, Color color)
    {
        if (gradeText == null) return;

        if (_gradeRoutine != null) StopCoroutine(_gradeRoutine);
        _gradeRoutine = StartCoroutine(GradeFlashRoutine(label, color));
    }

    private void TriggerFlash(Color color)
    {
        if (screenFlash == null) return;

        if (_flashRoutine != null) StopCoroutine(_flashRoutine);
        _flashRoutine = StartCoroutine(ScreenFlashRoutine(color));
    }

    private IEnumerator GradeFlashRoutine(string label, Color color)
    {
        gradeText.text = label;
        gradeText.color = color;
        SetGradeVisible(true);

        yield return new WaitForSeconds(gradeDisplaySeconds);

        SetGradeVisible(false);
        _gradeRoutine = null;
    }

    private IEnumerator ScreenFlashRoutine(Color color)
    {
        screenFlash.color = color;
        SetFlashVisible(true);

        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(color.a, 0f, elapsed / flashDuration);
            screenFlash.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        SetFlashVisible(false);
        _flashRoutine = null;
    }

    private void SetGradeVisible(bool visible)
    {
        if (gradeText != null) gradeText.gameObject.SetActive(visible);
    }

    private void SetFlashVisible(bool visible)
    {
        if (screenFlash != null) screenFlash.gameObject.SetActive(visible);
    }
}