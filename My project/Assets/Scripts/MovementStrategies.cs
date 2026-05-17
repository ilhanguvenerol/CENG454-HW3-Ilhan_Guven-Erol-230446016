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

//SpiralStrategy: Spirals inward while advancing toward the target
public class SpiralStrategy : IMovementStrategy
{
    private readonly float _speed;
    private readonly float _angularSpeed; // degrees per second

    private Vector2 _targetPos;
    private float _angle;           // current orbit angle in degrees
    private float _radius;          // current distance to target

    public SpiralStrategy(float speed, float angularSpeed)
    {
        _speed = speed;
        _angularSpeed = angularSpeed;
    }

    public void Init(Transform projectile, Vector2 targetPosition)
    {
        _targetPos = targetPosition;
        Vector2 offset = (Vector2)projectile.position - targetPosition;
        _radius = offset.magnitude;
        _angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    }

    public bool Tick(Transform projectile, float deltaTime)
    {
        _radius -= _speed * deltaTime;
        _angle += _angularSpeed * deltaTime;

        if (_radius <= 0f) return true;

        float rad = _angle * Mathf.Deg2Rad;
        projectile.position = _targetPos + new Vector2(
            Mathf.Cos(rad) * _radius,
            Mathf.Sin(rad) * _radius);

        return false;
    }

    public void Reset()
    {
        _radius = 0f;
        _angle = 0f;
    }
}

public class PulseStrategy : IMovementStrategy
{
    private readonly float _baseSpeed;
    private readonly float _pulseFrequency; // should match BeatClock.BeatInterval

    private Vector2 _direction;
    private Vector2 _targetPos;
    private float _elapsed;

    public PulseStrategy(float speed, float pulseFrequency)
    {
        _baseSpeed = speed;
        _pulseFrequency = pulseFrequency;
    }

    public void Init(Transform projectile, Vector2 targetPosition)
    {
        _targetPos = targetPosition;
        _direction = (targetPosition - (Vector2)projectile.position).normalized;
        _elapsed = 0f;
    }

    public bool Tick(Transform projectile, float deltaTime)
    {
        _elapsed += deltaTime;

        // Speed oscillates between 0.2x and 1.8x base speed on the beat frequency
        float t = Mathf.Sin((_elapsed / _pulseFrequency) * Mathf.PI);
        float currentSpeed = _baseSpeed * (0.2f + 1.6f * Mathf.Abs(t));

        projectile.Translate(_direction * (currentSpeed * deltaTime));

        return Vector2.Dot(
            _targetPos - (Vector2)projectile.position,
            _direction) <= 0f;
    }

    public void Reset() => _elapsed = 0f;
}


