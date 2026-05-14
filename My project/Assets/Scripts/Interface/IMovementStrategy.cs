using UnityEngine;

//Base interface for strategy pattern
public interface IMovementStrategy
{
    // Called once when the projectile is activated
    // to cache the target position
    void Init(Transform projectile, Vector2 targetPosition);

    // Call every frame from Projectile.Update()
    // Move the projectile, return true when it reaches the target
    bool Tick(Transform projectile, float deltaTime);

    //Reset internal state
    void Reset();
}

