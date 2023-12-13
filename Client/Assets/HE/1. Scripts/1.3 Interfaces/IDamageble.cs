using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }

    public void Damage(int damageAmount, Vector3 hitDirection);
    public void Die();
}
