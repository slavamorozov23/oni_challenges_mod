using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000169 RID: 361
public class ShockwormConfig : IEntityConfig
{
	// Token: 0x060006F2 RID: 1778 RVA: 0x00031218 File Offset: 0x0002F418
	public GameObject CreatePrefab()
	{
		string id = "ShockWorm";
		string name = STRINGS.CREATURES.SPECIES.SHOCKWORM.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SHOCKWORM.DESC;
		float mass = 50f;
		EffectorValues tier = DECOR.BONUS.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("shockworm_kanim"), "idle", Grid.SceneLayer.Creatures, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		FactionManager.FactionID faction = FactionManager.FactionID.Hostile;
		string initialTraitID = null;
		string navGridName = "FlyerNavGrid1x2";
		NavType navType = NavType.Hover;
		int max_probing_radius = 32;
		float moveSpeed = 2f;
		string onDeathDropID = "Meat";
		float onDeathDropCount = 3f;
		bool drownVulnerable = true;
		bool entombVulnerable = true;
		float freezing_ = TUNING.CREATURES.TEMPERATURE.FREEZING_2;
		EntityTemplates.ExtendEntityToBasicCreature(gameObject, faction, initialTraitID, navGridName, navType, max_probing_radius, moveSpeed, onDeathDropID, onDeathDropCount, drownVulnerable, entombVulnerable, TUNING.CREATURES.TEMPERATURE.FREEZING_1, TUNING.CREATURES.TEMPERATURE.HOT_1, freezing_, TUNING.CREATURES.TEMPERATURE.HOT_2);
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddWeapon(3f, 6f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.AreaOfEffect, 10, 4f).AddEffect("WasAttacked", 1f);
		SoundEventVolumeCache.instance.AddVolume("shockworm_kanim", "Shockworm_attack_arc", NOISE_POLLUTION.CREATURES.TIER6);
		return gameObject;
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x000312FB File Offset: 0x0002F4FB
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x000312FD File Offset: 0x0002F4FD
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000548 RID: 1352
	public const string ID = "ShockWorm";
}
