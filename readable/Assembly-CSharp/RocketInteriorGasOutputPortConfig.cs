using System;
using TUNING;
using UnityEngine;

// Token: 0x020003F3 RID: 1011
public class RocketInteriorGasOutputPortConfig : IBuildingConfig
{
	// Token: 0x060014D3 RID: 5331 RVA: 0x00077B4E File Offset: 0x00075D4E
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014D4 RID: 5332 RVA: 0x00077B58 File Offset: 0x00075D58
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketInteriorGasOutputPort";
		int width = 1;
		int height = 1;
		string anim = "rocket_interior_port_gas_out_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.DefaultAnimState = "gas_out";
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

	// Token: 0x060014D5 RID: 5333 RVA: 0x00077BF8 File Offset: 0x00075DF8
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.gasOutputPort;
	}

	// Token: 0x060014D6 RID: 5334 RVA: 0x00077C0C File Offset: 0x00075E0C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<SimCellOccupier>().notifyOnMelt = true;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		go.AddComponent<RocketConduitReceiver>().conduitPortInfo = this.gasOutputPort;
	}

	// Token: 0x060014D7 RID: 5335 RVA: 0x00077C60 File Offset: 0x00075E60
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

	// Token: 0x060014D8 RID: 5336 RVA: 0x00077CCE File Offset: 0x00075ECE
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPort(go);
	}

	// Token: 0x060014D9 RID: 5337 RVA: 0x00077CE6 File Offset: 0x00075EE6
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPort(go);
	}

	// Token: 0x04000CA7 RID: 3239
	public const string ID = "RocketInteriorGasOutputPort";

	// Token: 0x04000CA8 RID: 3240
	private ConduitPortInfo gasOutputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(0, 0));
}
