using System;
using TUNING;
using UnityEngine;

// Token: 0x02000413 RID: 1043
public class ShowerConfig : IBuildingConfig
{
	// Token: 0x0600157F RID: 5503 RVA: 0x0007ADF8 File Offset: 0x00078FF8
	public override BuildingDef CreateBuildingDef()
	{
		string id = ShowerConfig.ID;
		int width = 2;
		int height = 4;
		string anim = "shower_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, tier2, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 1);
		return buildingDef;
	}

	// Token: 0x06001580 RID: 5504 RVA: 0x0007AE90 File Offset: 0x00079090
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WashStation, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.AdvancedWashStation, false);
		Shower shower = go.AddOrGet<Shower>();
		shower.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_shower_kanim")
		};
		shower.workTime = 15f;
		shower.outputTargetElement = SimHashes.DirtyWater;
		shower.fractionalDiseaseRemoval = 0.95f;
		shower.absoluteDiseaseRemoval = -2000;
		shower.workLayer = Grid.SceneLayer.BuildingFront;
		shower.trackUses = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		conduitConsumer.capacityKG = 5f;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.invertElementFilter = true;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.Water
		};
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(new Tag("Water"), 1f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(1f, SimHashes.DirtyWater, 0f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true)
		};
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 10f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		go.AddOrGet<RequireOutputs>().ignoreFullPipe = true;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06001581 RID: 5505 RVA: 0x0007B01C File Offset: 0x0007921C
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000CDD RID: 3293
	public static string ID = "Shower";
}
