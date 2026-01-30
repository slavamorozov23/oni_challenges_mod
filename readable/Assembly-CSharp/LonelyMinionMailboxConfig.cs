using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020002D2 RID: 722
public class LonelyMinionMailboxConfig : IBuildingConfig
{
	// Token: 0x06000EAC RID: 3756 RVA: 0x000556A4 File Offset: 0x000538A4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LonelyMailBox";
		int width = 2;
		int height = 2;
		string anim = "parcel_delivery_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingBack;
		buildingDef.DefaultAnimState = "idle";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.ViewMode = OverlayModes.None.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		return buildingDef;
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x00055734 File Offset: 0x00053934
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		SingleEntityReceptacle singleEntityReceptacle = go.AddComponent<SingleEntityReceptacle>();
		singleEntityReceptacle.AddDepositTag(GameTags.Edible);
		singleEntityReceptacle.enabled = false;
		go.AddComponent<LonelyMinionMailbox>();
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = false;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Preserve
		});
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x00055796 File Offset: 0x00053996
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400098B RID: 2443
	public const string ID = "LonelyMailBox";

	// Token: 0x0400098C RID: 2444
	public static readonly HashedString IdHash = "LonelyMailBox";
}
