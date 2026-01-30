using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000244 RID: 580
public class GraveConfig : IBuildingConfig
{
	// Token: 0x06000BC1 RID: 3009 RVA: 0x00047C18 File Offset: 0x00045E18
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Grave";
		int width = 1;
		int height = 2;
		string anim = "gravestone_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		return buildingDef;
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x00047C84 File Offset: 0x00045E84
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GraveConfig.STORAGE_OVERRIDE_ANIM_FILES = new KAnimFile[]
		{
			Assets.GetAnim("anim_bury_dupe_kanim")
		};
		GraveStorage graveStorage = go.AddOrGet<GraveStorage>();
		graveStorage.showInUI = true;
		graveStorage.SetDefaultStoredItemModifiers(GraveConfig.StorageModifiers);
		graveStorage.overrideAnims = GraveConfig.STORAGE_OVERRIDE_ANIM_FILES;
		graveStorage.workAnims = GraveConfig.STORAGE_WORK_ANIMS;
		graveStorage.workingPstComplete = new HashedString[]
		{
			GraveConfig.STORAGE_PST_ANIM
		};
		graveStorage.synchronizeAnims = false;
		graveStorage.useGunForDelivery = false;
		graveStorage.workAnimPlayMode = KAnim.PlayMode.Once;
		go.AddOrGet<Grave>();
		Prioritizable.AddRef(go);
		go.GetComponent<KPrefabID>().prefabInitFn += this.OnInit;
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x00047D30 File Offset: 0x00045F30
	private void OnInit(GameObject go)
	{
		GraveStorage graveStorage = go.AddOrGet<GraveStorage>();
		KAnimFile[] value = new KAnimFile[]
		{
			Assets.GetAnim("anim_bury_dupe_kanim")
		};
		graveStorage.workerTypeOverrideAnims.Add(MinionConfig.ID, value);
		graveStorage.workerTypeOverrideAnims.Add(BionicMinionConfig.ID, new KAnimFile[]
		{
			Assets.GetAnim("anim_bionic_bury_dupe_kanim")
		});
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x00047DA0 File Offset: 0x00045FA0
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000843 RID: 2115
	public const string ID = "Grave";

	// Token: 0x04000844 RID: 2116
	public const string AnimFile = "gravestone_kanim";

	// Token: 0x04000845 RID: 2117
	private static KAnimFile[] STORAGE_OVERRIDE_ANIM_FILES;

	// Token: 0x04000846 RID: 2118
	private static readonly HashedString[] STORAGE_WORK_ANIMS = new HashedString[]
	{
		"working_pre"
	};

	// Token: 0x04000847 RID: 2119
	private static readonly HashedString STORAGE_PST_ANIM = HashedString.Invalid;

	// Token: 0x04000848 RID: 2120
	private static readonly List<Storage.StoredItemModifier> StorageModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve
	};
}
