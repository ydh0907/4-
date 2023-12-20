using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkDamageble : MonoBehaviour, IDamageble
{
    private PlayerDamageble PlayerDamageble;

    [field: SerializeField] public int CurrentHealth { get; set; }
    [field: SerializeField] public int MaxHealth { get; set; }

    private void Awake()
    {
        PlayerDamageble = GetComponentInParent<PlayerDamageble>();
    }

    private void Start()
    {
        MaxHealth = 1000;
        CurrentHealth = MaxHealth;
    }

    public void Damage(int damageAmount, Vector3 hitDirection)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 200)
        {
            Die();
        }
    }

    public void StartRush()
    {
         StartCoroutine(nameof(RushDamage));
        // CurrentHealth--;
    }

    public void Die()
    {
        PlayerDamageble.Die();
    }

    IEnumerator RushDamage()
    {
        CurrentHealth--;

        if (CurrentHealth <= 200)
        {
            Die();
        }
        yield return null;
    }
}
