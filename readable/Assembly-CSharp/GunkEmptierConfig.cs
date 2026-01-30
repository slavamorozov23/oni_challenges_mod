using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000259 RID: 601
public class GunkEmptierConfig : IBuildingConfig
{
	// Token: 0x06000C2D RID: 3117 RVA: 0x00049334 File Offset: 0x00047534
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x0004933C File Offset: 0x0004753C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GunkEmptier";
		int width = 3;
		int height = 3;
		string anim = "gunkdump_station_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = new CellOffset(-1, 0);
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TOILET);
		buildingDef.AddSearchTerms(SEARCH_TERMS.BIONIC);
		return buildingDef;
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x000493F0 File Offset: 0x000475F0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.CodexCategories.BionicBuilding, false);
		component.AddTag(RoomConstraints.ConstraintTags.ToiletType, false);
		component.AddTag(RoomConstraints.ConstraintTags.FlushToiletType, false);
		Prioritizable.AddRef(go);
		Storage storage = go.AddComponent<Storage>();
		storage.capacityKg = GunkEmptierConfig.STORAGE_CAPACITY;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		go.AddOrGet<GunkEmptierWorkable>();
		go.AddOrGetDef<GunkEmptier.Def>();
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.LiquidGunk
		};
		Ownable ownable = go.AddOrGet<Ownable>();
		ownable.slotID = Db.Get().AssignableSlots.Toilet.Id;
		ownable.canBePublic = true;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x000494A2 File Offset: 0x000476A2
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000860 RID: 2144
	public const string ID = "GunkEmptier";

	// Token: 0x04000861 RID: 2145
	private static float STORAGE_CAPACITY = GunkMonitor.GUNK_CAPACITY * 1.5f;
}
