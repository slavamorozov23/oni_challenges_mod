using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000465 RID: 1125
public class WireBridgeHighWattageConfig : IBuildingConfig
{
	// Token: 0x0600178D RID: 6029 RVA: 0x00085C8F File Offset: 0x00083E8F
	protected virtual string GetID()
	{
		return "WireBridgeHighWattage";
	}

	// Token: 0x0600178E RID: 6030 RVA: 0x00085C98 File Offset: 0x00083E98
	public override BuildingDef CreateBuildingDef()
	{
		string id = this.GetID();
		int width = 1;
		int height = 1;
		string anim = "heavywatttile_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.HighWattBridgeTile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER5, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Overheatable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.SceneLayer = Grid.SceneLayer.WireBridgesFront;
		buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireBridgeHighWattage");
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.WIRE);
		return buildingDef;
	}

	// Token: 0x0600178F RID: 6031 RVA: 0x00085D98 File Offset: 0x00083F98
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.doReplaceElement = true;
		simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT_MODIFIERS.PENALTY_3;
		simCellOccupier.notifyOnMelt = true;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		go.AddOrGet<TileTemperature>();
	}

	// Token: 0x06001790 RID: 6032 RVA: 0x00085DF1 File Offset: 0x00083FF1
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06001791 RID: 6033 RVA: 0x00085E0F File Offset: 0x0008400F
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06001792 RID: 6034 RVA: 0x00085E2C File Offset: 0x0008402C
	public override void DoPostConfigureComplete(GameObject go)
	{
		this.AddNetworkLink(go).visualizeOnly = false;
		go.GetComponent<KPrefabID>().AddTag(GameTags.WireBridges, false);
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06001793 RID: 6035 RVA: 0x00085E53 File Offset: 0x00084053
	protected virtual WireUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		WireUtilityNetworkLink wireUtilityNetworkLink = go.AddOrGet<WireUtilityNetworkLink>();
		wireUtilityNetworkLink.maxWattageRating = Wire.WattageRating.Max20000;
		wireUtilityNetworkLink.link1 = new CellOffset(-1, 0);
		wireUtilityNetworkLink.link2 = new CellOffset(1, 0);
		return wireUtilityNetworkLink;
	}

	// Token: 0x04000DDB RID: 3547
	public const string ID = "WireBridgeHighWattage";
}
