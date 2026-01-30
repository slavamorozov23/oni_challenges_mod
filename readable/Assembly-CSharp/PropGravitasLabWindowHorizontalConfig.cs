using System;
using TUNING;
using UnityEngine;

// Token: 0x020003C6 RID: 966
public class PropGravitasLabWindowHorizontalConfig : IBuildingConfig
{
	// Token: 0x060013E0 RID: 5088 RVA: 0x000716E5 File Offset: 0x0006F8E5
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060013E1 RID: 5089 RVA: 0x000716EC File Offset: 0x0006F8EC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PropGravitasLabWindowHorizontal";
		int width = 3;
		int height = 2;
		string anim = "gravitas_lab_window_horizontal_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier_TINY = BUILDINGS.CONSTRUCTION_MASS_KG.TIER_TINY;
		string[] glasses = MATERIALS.GLASSES;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier_TINY, glasses, melting_point, build_location_rule, DECOR.BONUS.TIER0, none, 0.2f);
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

	// Token: 0x060013E2 RID: 5090 RVA: 0x00071780 File Offset: 0x0006F980
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
		go.AddComponent<ZoneTile>();
		go.GetComponent<PrimaryElement>().SetElement(SimHashes.Glass, true);
		go.GetComponent<PrimaryElement>().Temperature = 273f;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas, false);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x060013E3 RID: 5091 RVA: 0x000717E7 File Offset: 0x0006F9E7
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000C1A RID: 3098
	public const string ID = "PropGravitasLabWindowHorizontal";
}
