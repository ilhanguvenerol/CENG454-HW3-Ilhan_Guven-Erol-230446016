using UnityEngine;

// at first i thought this interface was unnecessary, but in practice, when it will be implemented, projectiles will be abstracted from health components.
public interface IDamageable
{
    bool IsAlive { get; }

    void TakeDamage(int amount);
}
