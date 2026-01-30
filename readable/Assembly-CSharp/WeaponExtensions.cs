using System;
using UnityEngine;

// Token: 0x02000868 RID: 2152
public static class WeaponExtensions
{
	// Token: 0x06003B0F RID: 15119 RVA: 0x001495EC File Offset: 0x001477EC
	public static Weapon AddWeapon(this GameObject prefab, float base_damage_min, float base_damage_max, AttackProperties.DamageType attackType = AttackProperties.DamageType.Standard, AttackProperties.TargetType targetType = AttackProperties.TargetType.Single, int maxHits = 1, float aoeRadius = 0f)
	{
		Weapon weapon = prefab.AddOrGet<Weapon>();
		weapon.Configure(base_damage_min, base_damage_max, attackType, targetType, maxHits, aoeRadius);
		return weapon;
	}
}
