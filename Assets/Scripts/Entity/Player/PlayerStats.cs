using UnityEngine;

public class PlayerStats : EntityStats
{
    public Stat sprintSpeed;
    public Stat crouchSpeed;
    public Stat dashSpeed;

    public Stat maxAcceleration;
    public Stat maxAirAcceleration;

    public Stat dashStaminaCost;
    public FloatStat dashDuration;

    public Stat maxJumps;
    public Stat jumpHeight;

    private void Awake()
    {
        currentHealth = maxHealth.BaseValue;
    }

    public override void TakeDamage(int damage)
    {
        damage -= armor.BaseValue;
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        // Die
    }
}
