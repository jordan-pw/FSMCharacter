using System.Collections;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [HideInInspector]
    public float stamina;

    public Stat maxStamina;
    public FloatStat staminaRechargeRate;
    public FloatStat staminaRechargeDelay;

    private bool canRecharge;

    private void Awake()
    {
        canRecharge = true;
        stamina = maxStamina.BaseValue;
    }

    private void Update()
    {
        Debug.Log(stamina);
        if (stamina <= 0 && canRecharge)
        {
            canRecharge = false;
            StartCoroutine(DelayStaminaRoutine());
        }
        else if ((stamina < maxStamina.BaseValue) && (stamina > 0))
        {
            stamina += staminaRechargeRate.BaseValue * Time.deltaTime;
        }
        stamina = Mathf.Min(stamina, maxStamina.BaseValue);
    }

    private IEnumerator DelayStaminaRoutine()
    {
        stamina = 0;
        yield return new WaitForSeconds(staminaRechargeDelay.BaseValue);
        canRecharge = true;
        stamina += 1;
    }
}
