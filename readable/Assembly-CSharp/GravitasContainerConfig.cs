using System;
using TUNING;
using UnityEngine;

// Token: 0x0200024A RID: 586
public class GravitasContainerConfig : IBuildingConfig
{
	// Token: 0x06000BDD RID: 3037 RVA: 0x00048258 File Offset: 0x00046458
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GravitasContainer";
		int width = 2;
		int height = 2;
		string anim = "gravitas_container_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.ShowInBuildMenu = false;
		buildingDef.Entombable = false;
		buildingDef.Floodable = false;
		buildingDef.Invincible = true;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x000482C5 File Offset: 0x000464C5
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.Gravitas);
		go.AddOrGet<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.Building;
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x000482E8 File Offset: 0x000464E8
	public override void DoPostConfigureComplete(GameObject go)
	{
		PajamaDispenser pajamaDispenser = go.AddComponent<PajamaDispenser>();
		pajamaDispenser.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gravitas_container_kanim")
		};
		pajamaDispenser.SetWorkTime(30f);
		go.AddOrGet<Demolishable>();
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
	}

	// Token: 0x0400084C RID: 2124
	public const string ID = "GravitasContainer";

	// Token: 0x0400084D RID: 2125
	private const float WORK_TIME = 1.5f;
}
