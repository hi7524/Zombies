using UnityEngine;

public interface IDamagable
{
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
    // hitPoint: 맞은 지점
    // normal: 맞은 지점의 normal (이펙트 재생 위함)
}
