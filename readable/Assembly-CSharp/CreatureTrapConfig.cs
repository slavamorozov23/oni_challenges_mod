using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000067 RID: 103
public class CreatureTrapConfig : IBuildingConfig
{
	// Token: 0x060001EF RID: 495 RVA: 0x0000E178 File Offset: 0x0000C378
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CreatureTrap", 2, 1, "creaturetrap_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.PLASTICS, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Deprecated = true;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Floodable = false;
		return buildingDef;
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x0000E1DC File Offset: 0x0000C3DC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = true;
		storage.SetDefaultStoredItemModifiers(CreatureTrapConfig.StoredItemModifiers);
		storage.sendOnStoreOnSpawn = true;
		TrapTrigger trapTrigger = go.AddOrGet<TrapTrigger>();
		trapTrigger.trappableCreatures = new Tag[]
		{
			GameTags.Creatures.Walker,
			GameTags.Creatures.Hoverer
		};
		trapTrigger.trappedOffset = new Vector2(0.5f, 0f);
		go.AddOrGet<Trap>();
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0000E24E File Offset: 0x0000C44E
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000136 RID: 310
	public const string ID = "CreatureTrap";

	// Token: 0x04000137 RID: 311
	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>();
}
