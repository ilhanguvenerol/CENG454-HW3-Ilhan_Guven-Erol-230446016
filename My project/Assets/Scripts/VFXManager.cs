using UnityEngine;

// Observes CoreHealth and RhythmJudge events and plays visual effects accordingly
public class VFXManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CoreHealth coreHealth;
    [SerializeField] private RhythmJudge rhythmJudge;
    [SerializeField] private Transform coreTransform;

    [Header("Prefabs")]
    [SerializeField] private ParticleSystem fxCoreDamage;
    [SerializeField] private ParticleSystem fxPerfect;
    [SerializeField] private ParticleSystem fxGood;
    [SerializeField] private ParticleSystem fxMiss;
    [SerializeField] private ParticleSystem fxCoreDestroyed;

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

    //Handlers
    private void HandleCoreDamaged(int current, int max) => Play(fxCoreDamage, coreTransform.position);
    private void HandleCoreDestroyed() => Play(fxCoreDestroyed, coreTransform.position);
    private void HandleMiss() => Play(fxMiss, coreTransform.position);

    private void HandleParry(ParryGrade grade)
    {
        switch (grade)
        {
            case ParryGrade.Perfect: Play(fxPerfect, coreTransform.position); break;
            case ParryGrade.Good: Play(fxGood, coreTransform.position); break;
        }
    }

    //VFX helper
    private void Play(ParticleSystem fx, Vector3 position)
    {
        if (fx == null) return;
        fx.transform.position = position;
        fx.Play();
    }
}