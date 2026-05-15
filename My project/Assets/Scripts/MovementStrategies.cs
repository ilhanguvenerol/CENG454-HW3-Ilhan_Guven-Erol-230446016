using UnityEngine;

//StraightStrategy: Move directly toward the target
public class StraightStrategy : IMovementStrategy
{
    private readonly float _speed;
    private Vector2 _direction;
    private Vector2 _targetPos;

    public StraightStrategy(float speed) => _speed = speed;

    public void Init(Transform projectile, Vector2 targetPosition)
    {
        _targetPos = targetPosition;
        _direction = (targetPosition - (Vector2)projectile.position).normalized;
    }

    public bool Tick(Transform projectile, float deltaTime)
    {
        projectile.Translate(_direction * (_speed * deltaTime));

        // Reached if we overshot the target
        return Vector2.Dot(
            _targetPos - (Vector2)projectile.position,
            _direction) <= 0f;
    }

    public void Reset() { }
}
