using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000253 RID: 595
public class GravitasPedestalConfig : IBuildingConfig
{
	// Token: 0x06000C14 RID: 3092 RVA: 0x00048E50 File Offset: 0x00047050
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x00048E58 File Offset: 0x00047058
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GravitasPedestal";
		int width = 1;
		int height = 2;
		string anim = "gravitas_pedestal_nice_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.DefaultAnimState = "pedestal_nice";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "small";
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x00048EE0 File Offset: 0x000470E0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>(new Storage.StoredItemModifier[]
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Preserve
		}));
		Prioritizable.AddRef(go);
		SingleEntityReceptacle singleEntityReceptacle = go.AddOrGet<SingleEntityReceptacle>();
		singleEntityReceptacle.AddDepositTag(GameTags.PedestalDisplayable);
		singleEntityReceptacle.occupyingObjectRelativePosition = new Vector3(0f, 1.2f, -1f);
		go.AddOrGet<DecorProvider>();
		go.AddOrGet<ItemPedestal>();
		go.AddOrGet<PedestalArtifactSpawner>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x00048F61 File Offset: 0x00047161
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400085D RID: 2141
	public const string ID = "GravitasPedestal";
}
