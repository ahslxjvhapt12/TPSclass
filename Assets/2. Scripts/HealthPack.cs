using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour, IItem
{
    public float newHealth = 30;

    public void Use(GameObject target)
    {
        Debug.Log("use");
        LivingEntity health = target.GetComponent<LivingEntity>();

        if(health != null)
        {
            health.RestoreHealth(newHealth);
        }

        Destroy(gameObject);
    }
}
