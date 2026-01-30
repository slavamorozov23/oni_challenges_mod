using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200006F RID: 111
public class DebrisPayloadConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000216 RID: 534 RVA: 0x0000EDF8 File Offset: 0x0000CFF8
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0000EDFF File Offset: 0x0000CFFF
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0000EE04 File Offset: 0x0000D004
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("DebrisPayload", ITEMS.DEBRISPAYLOAD.NAME, ITEMS.DEBRISPAYLOAD.DESC, 100f, true, Assets.GetAnim("rocket_debris_combined_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IgnoreMaterialCategory,
			GameTags.Experimental
		});
		RailGunPayload.Def def = gameObject.AddOrGetDef<RailGunPayload.Def>();
		def.attractToBeacons = false;
		def.clusterAnimSymbolSwapTarget = "debris1";
		def.randomClusterSymbolSwaps = new List<string>
		{
			"debris1",
			"debris2",
			"debris3"
		};
		def.worldAnimSymbolSwapTarget = "debris";
		def.randomWorldSymbolSwaps = new List<string>
		{
			"debris",
			"2_debris",
			"3_debris"
		};
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		gameObject.AddOrGet<LoopingSounds>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(gameObject, false);
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.allowItemRemoval = false;
		storage.capacityKg = 5000f;
		DropAllWorkable dropAllWorkable = gameObject.AddOrGet<DropAllWorkable>();
		dropAllWorkable.dropWorkTime = 30f;
		dropAllWorkable.choreTypeID = Db.Get().ChoreTypes.Fetch.Id;
		dropAllWorkable.ConfigureMultitoolContext("build", EffectConfigs.BuildSplashId);
		ClusterDestinationSelector clusterDestinationSelector = gameObject.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.assignable = false;
		clusterDestinationSelector.shouldPointTowardsPath = true;
		clusterDestinationSelector.requireAsteroidDestination = true;
		clusterDestinationSelector.canNavigateFogOfWar = true;
		BallisticClusterGridEntity ballisticClusterGridEntity = gameObject.AddOrGet<BallisticClusterGridEntity>();
		ballisticClusterGridEntity.clusterAnimName = "rocket_debris_kanim";
		ballisticClusterGridEntity.isWorldEntity = true;
		ballisticClusterGridEntity.nameKey = new StringKey("STRINGS.ITEMS.DEBRISPAYLOAD.NAME");
		gameObject.AddOrGet<ClusterTraveler>();
		return gameObject;
	}

	// Token: 0x06000219 RID: 537 RVA: 0x0000EFC4 File Offset: 0x0000D1C4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600021A RID: 538 RVA: 0x0000EFC6 File Offset: 0x0000D1C6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000154 RID: 340
	public const string ID = "DebrisPayload";

	// Token: 0x04000155 RID: 341
	public const float MASS = 100f;

	// Token: 0x04000156 RID: 342
	public const float MAX_STORAGE_KG_MASS = 5000f;

	// Token: 0x04000157 RID: 343
	public const float STARMAP_SPEED = 10f;
}
