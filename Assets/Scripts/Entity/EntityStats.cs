using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [HideInInspector]
    public int currentHealth { get; set; }

    public Stat maxHealth;

    public Stat maxSpeed;

    public Stat armor;

    public Stat damage;

    public virtual void TakeDamage(int damage)
    {
        // ouch
    }

    public virtual void Die()
    {
        // Die
    }

}
