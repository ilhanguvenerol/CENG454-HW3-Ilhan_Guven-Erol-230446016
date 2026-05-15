using UnityEngine;

// Pooled projectile. Holds an IMovementStrategy assigned at rent time.
// deals damage and returns itself to the pool on collision 
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour, IPoolable
{
    // Set by ProjectilePool immediately after Instantiate
    public ProjectilePool Pool { get; set; }

    [SerializeField] private int damage = 1;

    private IMovementStrategy _strategy;
    private bool _active;

    public void Initialise(IMovementStrategy strategy, Vector2 targetPosition) //Called by RhythmSpawner after Rent()
    {
        _strategy = strategy;
        _strategy.Init(transform, targetPosition);
        _active = true;
    }

    //Lifecycle
    private void Update()
    {
        if (!_active || _strategy == null) return;

        bool reached = _strategy.Tick(transform, Time.deltaTime);
        if (reached) ReturnToPool();   // missed — no collision, projectile arrived
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_active) return;

        if (other.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeDamage(damage);
            ReturnToPool();
        }
    }

    //IPoolable
    public void OnRent()
    {
        _active = true;
    }

    public void OnReturn()
    {
        ResetState();
    }

    public void ResetState()
    {
        // Unsubscribe from events
        _strategy?.Reset();
        _strategy = null;
        _active = false;
    }

    private void ReturnToPool() => Pool.Return(this);
}