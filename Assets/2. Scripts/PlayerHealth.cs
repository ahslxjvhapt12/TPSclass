using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    
    
    public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        Debug.Log(1);
        CameraAction.Instance.ShakeCam(6, 1);
        base.OnDamage(damage, hitPosition, hitNormal);
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
}
