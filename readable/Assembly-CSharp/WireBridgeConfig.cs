using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000464 RID: 1124
public class WireBridgeConfig : IBuildingConfig
{
	// Token: 0x06001785 RID: 6021 RVA: 0x00085B15 File Offset: 0x00083D15
	protected virtual string GetID()
	{
		return "WireBridge";
	}

	// Token: 0x06001786 RID: 6022 RVA: 0x00085B1C File Offset: 0x00083D1C
	public override BuildingDef CreateBuildingDef()
	{
		string id = this.GetID();
		int width = 3;
		int height = 1;
		string anim = "utilityelectricbridge_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.WireBridge;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.ObjectLayer = ObjectLayer.WireConnectors;
		buildingDef.SceneLayer = Grid.SceneLayer.WireBridges;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireBridge");
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.WIRE);
		return buildingDef;
	}

	// Token: 0x06001787 RID: 6023 RVA: 0x00085C05 File Offset: 0x00083E05
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
	}

	// Token: 0x06001788 RID: 6024 RVA: 0x00085C0D File Offset: 0x00083E0D
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06001789 RID: 6025 RVA: 0x00085C2B File Offset: 0x00083E2B
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x0600178A RID: 6026 RVA: 0x00085C48 File Offset: 0x00083E48
	public override void DoPostConfigureComplete(GameObject go)
	{
		this.AddNetworkLink(go).visualizeOnly = false;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x0600178B RID: 6027 RVA: 0x00085C5E File Offset: 0x00083E5E
	protected virtual WireUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		WireUtilityNetworkLink wireUtilityNetworkLink = go.AddOrGet<WireUtilityNetworkLink>();
		wireUtilityNetworkLink.maxWattageRating = Wire.WattageRating.Max1000;
		wireUtilityNetworkLink.link1 = new CellOffset(-1, 0);
		wireUtilityNetworkLink.link2 = new CellOffset(1, 0);
		return wireUtilityNetworkLink;
	}

	// Token: 0x04000DDA RID: 3546
	public const string ID = "WireBridge";
}
