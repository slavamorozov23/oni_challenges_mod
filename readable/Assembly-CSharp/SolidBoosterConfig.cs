using System;
using TUNING;
using UnityEngine;

// Token: 0x0200041E RID: 1054
public class SolidBoosterConfig : IBuildingConfig
{
	// Token: 0x060015C1 RID: 5569 RVA: 0x0007C6A3 File Offset: 0x0007A8A3
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060015C2 RID: 5570 RVA: 0x0007C6AC File Offset: 0x0007A8AC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidBooster";
		int width = 7;
		int height = 5;
		string anim = "rocket_solid_booster_kanim";
		int hitpoints = 1000;
		float construction_time = 480f;
		float[] engine_MASS_SMALL = BUILDINGS.ROCKETRY_MASS_KG.ENGINE_MASS_SMALL;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, engine_MASS_SMALL, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.Invincible = true;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		return buildingDef;
	}

	// Token: 0x060015C3 RID: 5571 RVA: 0x0007C764 File Offset: 0x0007A964
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
		};
	}

	// Token: 0x060015C4 RID: 5572 RVA: 0x0007C7C8 File Offset: 0x0007A9C8
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x060015C5 RID: 5573 RVA: 0x0007C7CA File Offset: 0x0007A9CA
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x060015C6 RID: 5574 RVA: 0x0007C7CC File Offset: 0x0007A9CC
	public override void DoPostConfigureComplete(GameObject go)
	{
		SolidBooster solidBooster = go.AddOrGet<SolidBooster>();
		solidBooster.mainEngine = false;
		solidBooster.efficiency = ROCKETRY.ENGINE_EFFICIENCY.BOOSTER;
		solidBooster.fuelTag = ElementLoader.FindElementByHash(SimHashes.Iron).tag;
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.capacityKg = 800f;
		solidBooster.fuelStorage = storage;
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = solidBooster.fuelTag;
		manualDeliveryKG.refillMass = storage.capacityKg / 2f;
		manualDeliveryKG.capacity = storage.capacityKg / 2f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		ManualDeliveryKG manualDeliveryKG2 = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG2.SetStorage(storage);
		manualDeliveryKG2.RequestedItemTag = ElementLoader.FindElementByHash(SimHashes.OxyRock).tag;
		manualDeliveryKG2.refillMass = storage.capacityKg / 2f;
		manualDeliveryKG2.capacity = storage.capacityKg / 2f;
		manualDeliveryKG2.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_solid_booster_bg_kanim", false);
	}

	// Token: 0x04000CFC RID: 3324
	public const string ID = "SolidBooster";

	// Token: 0x04000CFD RID: 3325
	public const float capacity = 400f;
}
