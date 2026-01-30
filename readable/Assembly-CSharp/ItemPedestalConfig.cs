using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200027E RID: 638
public class ItemPedestalConfig : IBuildingConfig
{
	// Token: 0x06000CF9 RID: 3321 RVA: 0x0004D120 File Offset: 0x0004B320
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ItemPedestal";
		int width = 1;
		int height = 2;
		string anim = "pedestal_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.DefaultAnimState = "pedestal";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "small";
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x06000CFA RID: 3322 RVA: 0x0004D1B0 File Offset: 0x0004B3B0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>(new Storage.StoredItemModifier[]
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Preserve
		}));
		Prioritizable.AddRef(go);
		OrnamentReceptacle ornamentReceptacle = go.AddOrGet<OrnamentReceptacle>();
		ornamentReceptacle.AddDepositTag(GameTags.Ornament);
		ornamentReceptacle.AddDepositTag(GameTags.Suit);
		ornamentReceptacle.AddDepositTag(GameTags.Clothes);
		ornamentReceptacle.AddDepositTag(GameTags.Egg);
		ornamentReceptacle.AddDepositTag(GameTags.Seed);
		ornamentReceptacle.AddDepositTag(GameTags.Edible);
		ornamentReceptacle.AddDepositTag(GameTags.BionicUpgrade);
		ornamentReceptacle.AddDepositTag(GameTags.Solid);
		ornamentReceptacle.AddDepositTag(GameTags.Liquid);
		ornamentReceptacle.AddDepositTag(GameTags.Gas);
		ornamentReceptacle.AddDepositTag(GameTags.PedestalDisplayable);
		ornamentReceptacle.occupyingObjectRelativePosition = new Vector3(0f, 1.2f, -1f);
		go.AddOrGet<DecorProvider>();
		go.AddOrGet<ItemPedestal>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
		go.GetComponent<KPrefabID>().AddTag(GameTags.OrnamentDisplayer, false);
	}

	// Token: 0x06000CFB RID: 3323 RVA: 0x0004D2A9 File Offset: 0x0004B4A9
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040008ED RID: 2285
	public const string ID = "ItemPedestal";
}
