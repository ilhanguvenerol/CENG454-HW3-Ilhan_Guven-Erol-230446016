using UnityEngine;

/// <summary>
/// Listens to BeatClock and spawns projectiles from perimeter spawn points,
/// assigning each one a movement strategy based on the current wave pattern.
/// </summary>
public class RhythmSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BeatClock beatClock;
    [SerializeField] private ProjectilePool pool;
    [SerializeField] private Transform coreTarget;

    [Header("Perimeter spawn points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Wave pattern")]
    [Tooltip("How many beats to skip between spawns (1 = every beat, 2 = every other, etc.)")] // a note to myself: give different patterns to different spawners. 3rd, 5th and 7th is not a bad idea
    [SerializeField] private int beatDivisor = 1;

    private void OnEnable()
    {
        beatClock.OnBeat += HandleBeat;
    }

    private void OnDisable()
    {
        // unsubscribing is critical for pool safety
        beatClock.OnBeat -= HandleBeat;
    }

    //Beat handler
    private void HandleBeat(int beatIndex)
    {
        if (beatIndex % beatDivisor != 0) return;

        foreach (Transform spawnPoint in spawnPoints)
            SpawnFrom(spawnPoint);
    }

    //Spawn logic
    private void SpawnFrom(Transform spawnPoint)
    {
        Projectile p = pool.Rent();
        p.transform.position = spawnPoint.position;

        IMovementStrategy strategy = PickStrategy(spawnPoint);
        p.Initialise(strategy, coreTarget.position);
    }


    // Selects a movement strategy per spawn point.
    private IMovementStrategy PickStrategy(Transform spawnPoint)
    {
        //alternate by spawn-point index. note to self: maybe add a randomizer later
        int idx = System.Array.IndexOf(spawnPoints, spawnPoint);
        return (idx % 3) switch
        {
            0 => new StraightStrategy(speed: 4f),
            1 => new SpiralStrategy(speed: 3f, angularSpeed: 90f),
            _ => new PulseStrategy(speed: 3.5f, pulseFrequency: beatClock.BeatInterval)
        };
    }
}