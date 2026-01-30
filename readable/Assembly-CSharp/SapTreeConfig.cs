using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001B5 RID: 437
public class SapTreeConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000888 RID: 2184 RVA: 0x00039A4A File Offset: 0x00037C4A
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x00039A51 File Offset: 0x00037C51
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x00039A54 File Offset: 0x00037C54
	public GameObject CreatePrefab()
	{
		string id = "SapTree";
		string name = STRINGS.CREATURES.SPECIES.SAPTREE.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SAPTREE.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = SapTreeConfig.POSITIVE_DECOR_EFFECT;
		KAnimFile anim = Assets.GetAnim("gravitas_sap_tree_kanim");
		string initialAnim = "idle";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingFront;
		int width = 5;
		int height = 5;
		EffectorValues decor = positive_DECOR_EFFECT;
		List<Tag> additionalTags = new List<Tag>
		{
			GameTags.Decoration
		};
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 293f);
		SapTree.Def def = gameObject.AddOrGetDef<SapTree.Def>();
		def.foodSenseArea = new Vector2I(5, 1);
		def.massEatRate = 0.05f;
		def.kcalorieToKGConversionRatio = 0.005f;
		def.stomachSize = 5f;
		def.oozeRate = 2f;
		def.oozeOffsets = new List<Vector3>
		{
			new Vector3(-2f, 2f),
			new Vector3(2f, 1f)
		};
		def.attackSenseArea = new Vector2I(5, 5);
		def.attackCooldown = 5f;
		gameObject.AddOrGet<Storage>();
		FactionAlignment factionAlignment = gameObject.AddOrGet<FactionAlignment>();
		factionAlignment.Alignment = FactionManager.FactionID.Hostile;
		factionAlignment.canBePlayerTargeted = false;
		gameObject.AddOrGet<RangedAttackable>();
		gameObject.AddWeapon(5f, 5f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.AreaOfEffect, 1, 2f);
		gameObject.AddOrGet<WiltCondition>();
		gameObject.AddOrGet<TemperatureVulnerable>().Configure(173.15f, 0f, 373.15f, 1023.15f);
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x00039BD8 File Offset: 0x00037DD8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x00039BDA File Offset: 0x00037DDA
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400066F RID: 1647
	public const string ID = "SapTree";

	// Token: 0x04000670 RID: 1648
	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER5;

	// Token: 0x04000671 RID: 1649
	private const int WIDTH = 5;

	// Token: 0x04000672 RID: 1650
	private const int HEIGHT = 5;

	// Token: 0x04000673 RID: 1651
	private const int ATTACK_RADIUS = 2;

	// Token: 0x04000674 RID: 1652
	public const float MASS_EAT_RATE = 0.05f;

	// Token: 0x04000675 RID: 1653
	public const float KCAL_TO_KG_RATIO = 0.005f;
}
