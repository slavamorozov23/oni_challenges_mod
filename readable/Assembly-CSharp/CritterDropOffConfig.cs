using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200006A RID: 106
public class CritterDropOffConfig : IBuildingConfig
{
	// Token: 0x060001FE RID: 510 RVA: 0x0000E58C File Offset: 0x0000C78C
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CritterDropOff", 1, 3, "relocator_dropoff_02_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.RAW_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort("CritterDropOffInput", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.CRITTERDROPOFF.LOGIC_INPUT.DESC, STRINGS.BUILDINGS.PREFABS.CRITTERDROPOFF.LOGIC_INPUT.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.CRITTERDROPOFF.LOGIC_INPUT.LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.AddSearchTerms(SEARCH_TERMS.CRITTER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.RANCHING);
		return buildingDef;
	}

	// Token: 0x060001FF RID: 511 RVA: 0x0000E650 File Offset: 0x0000C850
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.CodexCategories.CreatureRelocator, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = false;
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.BAGABLE_CREATURES;
		storage.workAnims = new HashedString[]
		{
			"place",
			"release"
		};
		storage.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_restrain_creature_kanim")
		};
		storage.workAnimPlayMode = KAnim.PlayMode.Once;
		storage.synchronizeAnims = false;
		storage.useGunForDelivery = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		go.AddOrGet<CreatureDeliveryPoint>();
		go.AddOrGet<BaggableCritterCapacityTracker>().filteredCount = true;
		go.AddOrGet<TreeFilterable>();
	}

	// Token: 0x06000200 RID: 512 RVA: 0x0000E710 File Offset: 0x0000C910
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400013B RID: 315
	public const string ID = "CritterDropOff";

	// Token: 0x0400013C RID: 316
	public const string INPUT_PORT = "CritterDropOffInput";
}
