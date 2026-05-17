using UnityEngine;


// Subscriber reacts to CoreHealth and RhythmJudge events with sound.
public class AudioManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CoreHealth coreHealth;
    [SerializeField] private RhythmJudge rhythmJudge;

    [Header("Clips")]
    [SerializeField] private AudioClip clipCoreDamage;
    [SerializeField] private AudioClip clipPerfect;
    [SerializeField] private AudioClip clipGood;
    [SerializeField] private AudioClip clipMiss;
    [SerializeField] private AudioClip clipCoreDestroyed;

    private AudioSource _source;

    //Lifecycle
    private void Awake() => _source = GetComponent<AudioSource>();

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
    private void HandleCoreDamaged(int current, int max) => Play(clipCoreDamage);
    private void HandleCoreDestroyed() => Play(clipCoreDestroyed);
    private void HandleMiss() => Play(clipMiss);

    private void HandleParry(ParryGrade grade)
    {
        switch (grade)
        {
            case ParryGrade.Perfect: Play(clipPerfect); break;
            case ParryGrade.Good: Play(clipGood); break;
        }
    }

    private void Play(AudioClip clip)
    {
        if (clip != null) _source.PlayOneShot(clip);
    }
}