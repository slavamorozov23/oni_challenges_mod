using System;
using TUNING;
using UnityEngine;

// Token: 0x020003F1 RID: 1009
public class RocketInteriorGasInputPortConfig : IBuildingConfig
{
	// Token: 0x060014C6 RID: 5318 RVA: 0x000777B1 File Offset: 0x000759B1
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014C7 RID: 5319 RVA: 0x000777B8 File Offset: 0x000759B8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketInteriorGasInputPort";
		int width = 1;
		int height = 1;
		string anim = "rocket_interior_port_gas_in_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.DefaultAnimState = "gas_in";
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

	// Token: 0x060014C8 RID: 5320 RVA: 0x00077858 File Offset: 0x00075A58
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.gasInputPort;
	}

	// Token: 0x060014C9 RID: 5321 RVA: 0x0007786C File Offset: 0x00075A6C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<SimCellOccupier>().notifyOnMelt = true;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		Storage storage = go.AddComponent<Storage>();
		storage.showInUI = false;
		storage.capacityKg = 1f;
		RocketConduitSender rocketConduitSender = go.AddComponent<RocketConduitSender>();
		rocketConduitSender.conduitStorage = storage;
		rocketConduitSender.conduitPortInfo = this.gasInputPort;
		AutoStorageDropper.Def def = go.AddOrGetDef<AutoStorageDropper.Def>();
		def.elementFilter = new SimHashes[]
		{
			SimHashes.Unobtanium
		};
		def.dropOffset = new CellOffset(0, -1);
	}

	// Token: 0x060014CA RID: 5322 RVA: 0x00077908 File Offset: 0x00075B08
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

	// Token: 0x060014CB RID: 5323 RVA: 0x00077976 File Offset: 0x00075B76
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPort(go);
	}

	// Token: 0x060014CC RID: 5324 RVA: 0x0007798E File Offset: 0x00075B8E
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPort(go);
	}

	// Token: 0x04000CA2 RID: 3234
	public const string ID = "RocketInteriorGasInputPort";

	// Token: 0x04000CA3 RID: 3235
	private ConduitPortInfo gasInputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(0, 0));
}
