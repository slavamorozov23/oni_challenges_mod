using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000247 RID: 583
public class GravitasBathroomStallConfig : IBuildingConfig
{
	// Token: 0x06000BCF RID: 3023 RVA: 0x00047F7C File Offset: 0x0004617C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GravitasBathroomStall";
		int width = 2;
		int height = 2;
		string anim = "gravitas_toilet_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER0, tier2, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x00047FE2 File Offset: 0x000461E2
	public override void DoPostConfigureComplete(GameObject go)
	{
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		BuildingTemplates.ExtendBuildingToGravitas(go);
		go.AddOrGet<Demolishable>();
		go.AddOrGetDef<GravitasBathroomStall.Def>();
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x00048014 File Offset: 0x00046214
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		Activatable activatable = go.AddOrGet<Activatable>();
		activatable.SetWorkTime(5f);
		activatable.SetButtonTextOverride(new ButtonMenuTextOverride
		{
			Text = UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.ACTIVATE_TOILET_BUTTON,
			ToolTip = UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.ACTIVATE_TOILET_BUTTON_TOOLTIP,
			CancelText = UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.ACTIVATE_TOILET_BUTTON_CANCEL,
			CancelToolTip = UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.ACTIVATE_TOILET_BUTTON_CANCEL_TOOLTIP
		});
		activatable.Required = true;
		activatable.synchronizeAnims = true;
		activatable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gravitas_toilet_kanim")
		};
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x000480A4 File Offset: 0x000462A4
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x000480BB File Offset: 0x000462BB
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400084B RID: 2123
	public const string ID = "GravitasBathroomStall";
}
