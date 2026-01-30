using System;
using TUNING;
using UnityEngine;

// Token: 0x020002F2 RID: 754
public class MilkFatSeparatorConfig : IBuildingConfig
{
	// Token: 0x06000F56 RID: 3926 RVA: 0x00059518 File Offset: 0x00057718
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MilkFatSeparator";
		int width = 4;
		int height = 4;
		string anim = "milk_separator_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.SelfHeatKilowattsWhenActive = 8f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x000595D6 File Offset: 0x000577D6
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06000F58 RID: 3928 RVA: 0x000595D8 File Offset: 0x000577D8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = false;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		go.AddOrGet<Operational>();
		go.AddOrGet<EmptyMilkSeparatorWorkable>();
		go.AddOrGetDef<MilkSeparator.Def>().MILK_FAT_CAPACITY = 15f;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(new Tag("Milk"), 1f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.089999996f, SimHashes.MilkFat, 0f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true),
			new ElementConverter.OutputElement(0.80999994f, SimHashes.Brine, 0f, false, true, 0f, 0.5f, 0f, byte.MaxValue, 0, true),
			new ElementConverter.OutputElement(0.100000024f, SimHashes.CarbonDioxide, 348.15f, false, false, 1f, 3f, 0f, byte.MaxValue, 0, true)
		};
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityKG = 4f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Milk).tag;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.invertElementFilter = true;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.Milk
		};
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000F59 RID: 3929 RVA: 0x00059778 File Offset: 0x00057978
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x0005977A File Offset: 0x0005797A
	public override void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x04000A01 RID: 2561
	public const string ID = "MilkFatSeparator";

	// Token: 0x04000A02 RID: 2562
	public const float INPUT_RATE = 1f;

	// Token: 0x04000A03 RID: 2563
	public const float MILK_STORED_CAPACITY = 4f;

	// Token: 0x04000A04 RID: 2564
	public const float MILK_FAT_CAPACITY = 15f;

	// Token: 0x04000A05 RID: 2565
	public const float EFFICIENCY = 0.9f;

	// Token: 0x04000A06 RID: 2566
	public const float MILKFAT_PERCENT = 0.1f;

	// Token: 0x04000A07 RID: 2567
	private const float MILK_TO_FAT_OUTPUT_RATE = 0.089999996f;

	// Token: 0x04000A08 RID: 2568
	private const float MILK_TO_BRINE_WATER_OUTPUT_RATE = 0.80999994f;

	// Token: 0x04000A09 RID: 2569
	private const float MILK_TO_CO2_RATE = 0.100000024f;
}
