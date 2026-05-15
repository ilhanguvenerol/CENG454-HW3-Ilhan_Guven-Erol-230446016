using UnityEngine;
using System;

public class CoreHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 10;

    //Observable events (Observer pattern)
    public event Action<int, int> OnDamaged; //when core takes damage. (currentHP, maxHP)
    public event Action OnDestroyed; //when health reaches zero

    //IDamageable
    public bool IsAlive => CurrentHealth > 0;

    public int CurrentHealth { get; private set; }

    private void Awake() => CurrentHealth = maxHealth;

    public void TakeDamage(int amount)
    {
        if (!IsAlive) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        OnDamaged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth == 0)
            OnDestroyed?.Invoke();
    }
}

