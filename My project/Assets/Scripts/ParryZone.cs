using UnityEngine;

/// <summary>
/// A trigger collider sitting between the perimeter and the core.
/// When a projectile enters it AND the player has just pressed parry
/// (tracked via RhythmJudge), the projectile is deflected or destroyed.
///
/// If the projectile passes through WITHOUT a parry press, it continues
/// to the core — RhythmJudge.NotifyProjectileHitCore() is called there.
///
/// Attach to a ring/circle trigger around the core.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ParryZone : MonoBehaviour
{
    [SerializeField] private RhythmJudge rhythmJudge;
    [SerializeField] private ProjectilePool pool;

    // Track whether the player pressed parry this frame
    // PlayerInput sets this; ParryZone reads it on collision.
    public static bool ParryPressedThisFrame { get; set; }

    private void LateUpdate() //run AFTER each frame
    {
        // Reset value
        ParryPressedThisFrame = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<Projectile>(out var projectile)) return;

        if (ParryPressedThisFrame)
        {
            // if player intercepted then the grade is already handled by RhythmJudge via PlayerInput.HandleParryInput() called the same frame.
            pool.Return(projectile);
        }
        // else: projectile continues toward core naturally
    }
}
