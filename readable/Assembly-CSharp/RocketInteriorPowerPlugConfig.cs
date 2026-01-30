using System;
using TUNING;
using UnityEngine;

// Token: 0x020003F8 RID: 1016
public class RocketInteriorPowerPlugConfig : IBuildingConfig
{
	// Token: 0x060014F5 RID: 5365 RVA: 0x00078400 File Offset: 0x00076600
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014F6 RID: 5366 RVA: 0x00078408 File Offset: 0x00076608
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketInteriorPowerPlug";
		int width = 1;
		int height = 1;
		string anim = "rocket_floor_plug_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnRocketEnvelope;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.RequiresPowerOutput = true;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "RocketInteriorPowerPlug");
		return buildingDef;
	}

	// Token: 0x060014F7 RID: 5367 RVA: 0x000784BC File Offset: 0x000766BC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding, false);
		go.AddComponent<RequireInputs>();
	}

	// Token: 0x060014F8 RID: 5368 RVA: 0x000784DE File Offset: 0x000766DE
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<OperationalController.Def>();
		go.AddOrGet<WireUtilitySemiVirtualNetworkLink>().link1 = new CellOffset(0, 0);
	}

	// Token: 0x04000CB3 RID: 3251
	public const string ID = "RocketInteriorPowerPlug";
}
