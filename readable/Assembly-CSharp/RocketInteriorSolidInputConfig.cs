using System;
using TUNING;
using UnityEngine;

// Token: 0x020003F9 RID: 1017
public class RocketInteriorSolidInputConfig : IBuildingConfig
{
	// Token: 0x060014FA RID: 5370 RVA: 0x00078501 File Offset: 0x00076701
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014FB RID: 5371 RVA: 0x00078508 File Offset: 0x00076708
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketInteriorSolidInput";
		int width = 1;
		int height = 1;
		string anim = "rocket_floor_plug_solid_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnRocketEnvelope;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.InputConduitType = ConduitType.Solid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "RocketInteriorSolidInput");
		return buildingDef;
	}

	// Token: 0x060014FC RID: 5372 RVA: 0x000785C9 File Offset: 0x000767C9
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding, false);
		go.AddComponent<RequireInputs>();
	}

	// Token: 0x060014FD RID: 5373 RVA: 0x000785EC File Offset: 0x000767EC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<ActiveController.Def>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 20f;
		RocketConduitStorageAccess rocketConduitStorageAccess = go.AddOrGet<RocketConduitStorageAccess>();
		rocketConduitStorageAccess.storage = storage;
		rocketConduitStorageAccess.cargoType = CargoBay.CargoType.Solids;
		rocketConduitStorageAccess.targetLevel = 0f;
		SolidConduitConsumer solidConduitConsumer = go.AddOrGet<SolidConduitConsumer>();
		solidConduitConsumer.alwaysConsume = true;
		solidConduitConsumer.capacityKG = storage.capacityKg;
	}

	// Token: 0x04000CB4 RID: 3252
	private const ConduitType CONDUIT_TYPE = ConduitType.Solid;

	// Token: 0x04000CB5 RID: 3253
	private const CargoBay.CargoType CARGO_TYPE = CargoBay.CargoType.Solids;

	// Token: 0x04000CB6 RID: 3254
	public const string ID = "RocketInteriorSolidInput";
}
