using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003FE RID: 1022
public class SaunaConfig : IBuildingConfig
{
	// Token: 0x06001513 RID: 5395 RVA: 0x00078D4C File Offset: 0x00076F4C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Sauna";
		int width = 3;
		int height = 3;
		string anim = "sauna_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] construction_mass = new float[]
		{
			100f,
			100f
		};
		string[] construction_materials = new string[]
		{
			"Metal",
			"BuildingWood"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 2);
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.STEAM);
		return buildingDef;
	}

	// Token: 0x06001514 RID: 5396 RVA: 0x00078E4C File Offset: 0x0007704C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
		conduitConsumer.capacityKG = 50f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.alwaysConsume = true;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.Water
		};
		go.AddOrGet<SaunaWorkable>().basePriority = RELAXATION.PRIORITY.TIER3;
		Sauna sauna = go.AddOrGet<Sauna>();
		sauna.steamPerUseKG = 25f;
		sauna.waterOutputTemp = 353.15f;
		sauna.specificEffect = "Sauna";
		sauna.trackingEffect = "RecentlySauna";
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06001515 RID: 5397 RVA: 0x00078F42 File Offset: 0x00077142
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<RequireInputs>().requireConduitHasMass = false;
	}

	// Token: 0x04000CC7 RID: 3271
	public const string ID = "Sauna";

	// Token: 0x04000CC8 RID: 3272
	public const string COLD_IMMUNITY_EFFECT_NAME = "WarmTouch";

	// Token: 0x04000CC9 RID: 3273
	public const float COLD_IMMUNITY_DURATION = 1800f;

	// Token: 0x04000CCA RID: 3274
	private const float STEAM_PER_USE_KG = 25f;

	// Token: 0x04000CCB RID: 3275
	private const float WATER_OUTPUT_TEMP = 353.15f;
}
