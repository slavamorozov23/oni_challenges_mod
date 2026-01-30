using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200036E RID: 878
public class OilChangerConfig : IBuildingConfig
{
	// Token: 0x0600124E RID: 4686 RVA: 0x0006AAED File Offset: 0x00068CED
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x0600124F RID: 4687 RVA: 0x0006AAF4 File Offset: 0x00068CF4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OilChanger";
		int width = 3;
		int height = 3;
		string anim = "oilchange_station_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.Overheatable = false;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.UtilityInputOffset = new CellOffset(1, 2);
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
		buildingDef.AddSearchTerms(SEARCH_TERMS.BIONIC);
		buildingDef.AddSearchTerms(SEARCH_TERMS.MEDICINE);
		return buildingDef;
	}

	// Token: 0x06001250 RID: 4688 RVA: 0x0006ABC8 File Offset: 0x00068DC8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.BionicUpkeepType, false);
		go.GetComponent<KPrefabID>().AddTag(GameTags.CodexCategories.BionicBuilding, false);
		Storage storage = go.AddComponent<Storage>();
		storage.capacityKg = this.OIL_CAPACITY;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		OilChangerWorkableUse oilChangerWorkableUse = go.AddOrGet<OilChangerWorkableUse>();
		oilChangerWorkableUse.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_oilchange_kanim")
		};
		oilChangerWorkableUse.resetProgressOnStop = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = GameTags.LubricatingOil;
		conduitConsumer.capacityKG = this.OIL_CAPACITY;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		go.AddOrGetDef<OilChanger.Def>();
	}

	// Token: 0x06001251 RID: 4689 RVA: 0x0006AC77 File Offset: 0x00068E77
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000BA1 RID: 2977
	public const string ID = "OilChanger";

	// Token: 0x04000BA2 RID: 2978
	public float OIL_CAPACITY = 400f;
}
