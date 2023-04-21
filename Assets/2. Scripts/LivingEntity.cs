using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float initHealth = 100f; // ���� ü��
    public float health { get; protected set; } // ���� ü��

    public bool dead { get; protected set; } // ��� ����
    public event Action onDeath; // ��� �� �ߵ��� �̺�Ʈ
    
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
