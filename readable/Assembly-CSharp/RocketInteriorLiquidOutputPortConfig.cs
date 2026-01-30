using System;
using TUNING;
using UnityEngine;

// Token: 0x020003F7 RID: 1015
public class RocketInteriorLiquidOutputPortConfig : IBuildingConfig
{
	// Token: 0x060014ED RID: 5357 RVA: 0x00078239 File Offset: 0x00076439
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014EE RID: 5358 RVA: 0x00078240 File Offset: 0x00076440
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketInteriorLiquidOutputPort";
		int width = 1;
		int height = 1;
		string anim = "rocket_interior_port_liquid_out_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.DefaultAnimState = "liquid_out";
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.Replaceable = false;
		buildingDef.Invincible = true;
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x060014EF RID: 5359 RVA: 0x000782E0 File Offset: 0x000764E0
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.liquidOutputPort;
	}

	// Token: 0x060014F0 RID: 5360 RVA: 0x000782F4 File Offset: 0x000764F4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<SimCellOccupier>().notifyOnMelt = true;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		go.AddComponent<RocketConduitReceiver>().conduitPortInfo = this.liquidOutputPort;
	}

	// Token: 0x060014F1 RID: 5361 RVA: 0x00078348 File Offset: 0x00076548
	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Bunker, false);
		component.AddTag(GameTags.FloorTiles, false);
		component.AddTag(GameTags.NoRocketRefund, false);
		go.AddOrGetDef<MakeBaseSolid.Def>().solidOffsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		go.AddOrGet<BuildingCellVisualizer>();
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
	}

	// Token: 0x060014F2 RID: 5362 RVA: 0x000783B6 File Offset: 0x000765B6
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPort(go);
	}

	// Token: 0x060014F3 RID: 5363 RVA: 0x000783CE File Offset: 0x000765CE
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPort(go);
	}

	// Token: 0x04000CB1 RID: 3249
	public const string ID = "RocketInteriorLiquidOutputPort";

	// Token: 0x04000CB2 RID: 3250
	private ConduitPortInfo liquidOutputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 0));
}
