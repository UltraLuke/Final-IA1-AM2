using UnityEngine;
public interface IShooter
{
    Component ShootSettings(float damage, float rate, float distance);
    void GetShootData(out float shootDamage, out float shootRate, out float shootDistance);
}
