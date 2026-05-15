using System.Collections.Generic;
using UnityEngine;

// Generic object pool for Projectile GameObjects.
// RhythmSpawner calls Rent(), Projectile calls Return() on itself when done.
public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private Projectile prefab;
    [SerializeField] private int initialSize = 20;

    private readonly Queue<Projectile> _pool = new();

    //Lifecycle
    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
            _pool.Enqueue(CreateNew());
    }

    //Get a projectile from the pool, expand if empty
    public Projectile Rent()
    {
        Projectile p = _pool.Count > 0 ? _pool.Dequeue() : CreateNew();
        p.gameObject.SetActive(true);
        p.OnRent();
        return p;
    }

    //Return a projectile to the pool
    public void Return(Projectile p)
    {
        p.OnReturn(); // unsubscribes from events, resets state
        p.gameObject.SetActive(false);
        _pool.Enqueue(p);
    }

    //Private
    private Projectile CreateNew()
    {
        Projectile p = Instantiate(prefab, transform);
        p.Pool = this;
        p.gameObject.SetActive(false);
        return p;
    }
}

