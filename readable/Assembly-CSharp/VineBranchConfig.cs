using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C5 RID: 453
public class VineBranchConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600092C RID: 2348 RVA: 0x0003DAAA File Offset: 0x0003BCAA
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x0003DAB1 File Offset: 0x0003BCB1
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x0003DAB4 File Offset: 0x0003BCB4
	public GameObject CreatePrefab()
	{
		string id = "VineBranch";
		string name = STRINGS.CREATURES.SPECIES.VINEBRANCH.NAME;
		string desc = STRINGS.CREATURES.SPECIES.VINEBRANCH.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER0;
		KAnimFile anim = Assets.GetAnim("vine_kanim");
		string initialAnim = "line_idle";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingFront;
		int width = 1;
		int height = 1;
		EffectorValues decor = tier;
		List<Tag> additionalTags = new List<Tag>
		{
			GameTags.HideFromSpawnTool,
			GameTags.HideFromCodex,
			GameTags.PlantBranch,
			GameTags.ExcludeFromTemplate
		};
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 308.15f);
		string text = "VineBranchOriginal";
		bool should_grow_old = false;
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 273.15f, 298.15f, 318.15f, 378.15f, null, false, 0f, 0.15f, null, true, true, false, should_grow_old, 2400f, 0f, 2200f, text, STRINGS.CREATURES.SPECIES.VINEBRANCH.NAME);
		gameObject.AddOrGet<HarvestDesignatable>();
		gameObject.AddOrGet<CodexEntryRedirector>().CodexID = "VineMother";
		gameObject.AddOrGet<PlantFiberProducer>().amount = 6f;
		gameObject.AddOrGet<UprootedMonitor>();
		Crop.CropVal cropVal = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == VineFruitConfig.ID);
		gameObject.AddOrGet<Crop>().Configure(cropVal);
		Modifiers component = gameObject.GetComponent<Modifiers>();
		if (gameObject.GetComponent<Traits>() == null)
		{
			gameObject.AddOrGet<Traits>();
			component.initialTraits.Add(text);
		}
		component.initialAmounts.Add(Db.Get().Amounts.Maturity.Id);
		component.initialAmounts.Add(Db.Get().Amounts.Maturity2.Id);
		component.initialAttributes.Add(Db.Get().PlantAttributes.YieldAmount.Id);
		Trait trait = Db.Get().traits.Get(component.initialTraits[0]);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute.Id, 3f, STRINGS.CREATURES.SPECIES.VINEBRANCH.NAME, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Maturity2.maxAttribute.Id, 3f, STRINGS.CREATURES.SPECIES.VINEBRANCH.NAME, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().PlantAttributes.YieldAmount.Id, (float)cropVal.numProduced, STRINGS.CREATURES.SPECIES.VINEBRANCH.NAME, false, false, true));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, "VineBranch");
		gameObject.AddOrGetDef<VineBranch.Def>().BRANCH_PREFAB_NAME = "VineBranch";
		gameObject.AddOrGet<Harvestable>();
		gameObject.AddOrGet<HarvestDesignatable>();
		WiltCondition wiltCondition = gameObject.AddOrGet<WiltCondition>();
		wiltCondition.WiltDelay = 0f;
		wiltCondition.RecoveryDelay = 0f;
		SeedProducer seedProducer = gameObject.AddOrGet<SeedProducer>();
		seedProducer.Configure("VineMotherSeed", SeedProducer.ProductionType.HarvestOnly, 1);
		seedProducer.seedDropChanceMultiplier = 0.16666667f;
		return gameObject;
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x0003DDAB File Offset: 0x0003BFAB
	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<UprootedMonitor>().monitorCells = new CellOffset[0];
		inst.AddOrGet<HarvestDesignatable>().iconOffset = new Vector2(0f, Grid.CellSizeInMeters * 0.75f);
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x0003DDDE File Offset: 0x0003BFDE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006D7 RID: 1751
	public const string ID = "VineBranch";

	// Token: 0x040006D8 RID: 1752
	public const float GROWING_DURATION_CYCLES = 3f;

	// Token: 0x040006D9 RID: 1753
	public const float GROWING_DURATION = 1800f;

	// Token: 0x040006DA RID: 1754
	public const float FRUIT_GROWING_DURATION = 1800f;

	// Token: 0x040006DB RID: 1755
	public const int FRUIT_COUNT_PER_HARVEST = 1;

	// Token: 0x040006DC RID: 1756
	public const float PLANT_FIBER_PRODUCED_PER_CYCLE = 6f;
}
