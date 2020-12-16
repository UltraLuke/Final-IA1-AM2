using UnityEngine;
public interface IMelee
{
    Component MeleeSettings(float damage, float rate, float distance);
    void GetMeleeData(out float meleeDamage, out float meleeRate, out float meleeDistance);
}
