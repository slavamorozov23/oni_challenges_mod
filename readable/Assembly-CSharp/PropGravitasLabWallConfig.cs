using System;
using TUNING;
using UnityEngine;

// Token: 0x020003C4 RID: 964
public class PropGravitasLabWallConfig : IBuildingConfig
{
	// Token: 0x060013D8 RID: 5080 RVA: 0x000714D4 File Offset: 0x0006F6D4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PropGravitasLabWall";
		int width = 2;
		int height = 3;
		string anim = "gravitas_lab_wall_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.PermittedRotations = PermittedRotations.R90;
		buildingDef.Entombable = false;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.DefaultAnimState = "on";
		buildingDef.ObjectLayer = ObjectLayer.Backwall;
		buildingDef.SceneLayer = Grid.SceneLayer.Backwall;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x060013D9 RID: 5081 RVA: 0x0007156C File Offset: 0x0006F76C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
		go.AddComponent<ZoneTile>();
		go.GetComponent<PrimaryElement>().SetElement(SimHashes.Glass, true);
		go.GetComponent<PrimaryElement>().Temperature = 273f;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas, false);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x060013DA RID: 5082 RVA: 0x000715D3 File Offset: 0x0006F7D3
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000C18 RID: 3096
	public const string ID = "PropGravitasLabWall";
}
