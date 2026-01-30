using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class ElectrolyzerConfig : IBuildingConfig
{
	// Token: 0x0600027A RID: 634 RVA: 0x000112BC File Offset: 0x0000F4BC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Electrolyzer";
		int width = 2;
		int height = 2;
		string anim = "electrolyzer_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.ViewMode = OverlayModes.Oxygen.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 1));
		buildingDef.AddSearchTerms(SEARCH_TERMS.OXYGEN);
		return buildingDef;
	}

	// Token: 0x0600027B RID: 635 RVA: 0x00011384 File Offset: 0x0000F584
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		CellOffset cellOffset = new CellOffset(0, 1);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		Electrolyzer electrolyzer = go.AddOrGet<Electrolyzer>();
		electrolyzer.maxMass = 1.8f;
		electrolyzer.hasMeter = true;
		electrolyzer.emissionOffset = cellOffset;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 2f;
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Insulate
		});
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(new Tag("Water"), 1f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.888f, SimHashes.Oxygen, 343.15f, false, false, (float)cellOffset.x, (float)cellOffset.y, 1f, byte.MaxValue, 0, true),
			new ElementConverter.OutputElement(0.11199999f, SimHashes.Hydrogen, 343.15f, false, false, (float)cellOffset.x, (float)cellOffset.y, 1f, byte.MaxValue, 0, true)
		};
		Prioritizable.AddRef(go);
	}

	// Token: 0x0600027C RID: 636 RVA: 0x000114DB File Offset: 0x0000F6DB
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x0400018B RID: 395
	public const string ID = "Electrolyzer";

	// Token: 0x0400018C RID: 396
	public const float WATER2OXYGEN_RATIO = 0.888f;

	// Token: 0x0400018D RID: 397
	public const float OXYGEN_TEMPERATURE = 343.15f;
}
