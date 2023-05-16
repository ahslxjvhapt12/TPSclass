using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;

public class PlayerHealth : LivingEntity
{
    public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        Debug.Log(1);
        CameraAction.Instance.ShakeCam(6, 1);
        base.OnDamage(damage, hitPosition, hitNormal);
        UpdateUI();
        StartCoroutine(ShowBloodEffect(hitPosition, hitNormal));
    }

    private IEnumerator ShowBloodEffect(Vector3 hitPosition, Vector3 hitNormal)
    {
        EffectManager.Instance.PlayHitEffect(hitPosition, hitNormal, transform, EffectManager.EffectType.Flesh);

        yield return new WaitForSeconds(1f);
    }

    public override void Die()
    {

    }

    public override void RestoreHealth(float newHealth)
    {

        if (dead == true) return;

        health += newHealth;
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!dead)
        {
            IItem item = other.GetComponent<IItem>();

            item?.Use(gameObject);

        }
    }

    private void UpdateUI()
    {
        UIManager.Instance.UpdateHealthText(dead ? 0f : health);
    }
}
