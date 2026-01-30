using System;
using TUNING;
using UnityEngine;

// Token: 0x020003C5 RID: 965
public class PropGravitasLabWindowConfig : IBuildingConfig
{
	// Token: 0x060013DC RID: 5084 RVA: 0x000715E0 File Offset: 0x0006F7E0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PropGravitasLabWindow";
		int width = 2;
		int height = 3;
		string anim = "gravitas_lab_window_kanim";
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

	// Token: 0x060013DD RID: 5085 RVA: 0x00071674 File Offset: 0x0006F874
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
		go.AddComponent<ZoneTile>();
		go.GetComponent<PrimaryElement>().SetElement(SimHashes.Glass, true);
		go.GetComponent<PrimaryElement>().Temperature = 273f;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas, false);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x060013DE RID: 5086 RVA: 0x000716DB File Offset: 0x0006F8DB
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000C19 RID: 3097
	public const string ID = "PropGravitasLabWindow";
}
