using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float initHealth = 100f; // 시작 체력
    public float health { get; protected set; } // 현재 체력

    public bool dead { get; protected set; } // 사망 상태
    public event Action onDeath; // 사망 시 발동할 이벤트
    
    protected virtual void OnEnable()
    {
        health = initHealth;
        dead = false;
    }

    public virtual void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        health -= damage;
        if (health <= 0 && dead == false)
        {
            Die();
        }
    }

    public virtual void RestoreHealth(float newHealth)
    {
        if (dead == true) return;

        health += newHealth;
    }

    public virtual void Die()
    {
        onDeath?.Invoke();
        dead = true;
    }
}
