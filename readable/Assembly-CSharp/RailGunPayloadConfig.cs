using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020003D9 RID: 985
public class RailGunPayloadConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001439 RID: 5177 RVA: 0x00072DF8 File Offset: 0x00070FF8
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600143A RID: 5178 RVA: 0x00072DFF File Offset: 0x00070FFF
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600143B RID: 5179 RVA: 0x00072E04 File Offset: 0x00071004
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("RailGunPayload", ITEMS.RAILGUNPAYLOAD.NAME, ITEMS.RAILGUNPAYLOAD.DESC, 200f, true, Assets.GetAnim("railgun_capsule_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 1f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IgnoreMaterialCategory,
			GameTags.Experimental,
			GameTags.PedestalDisplayable
		});
		gameObject.AddOrGetDef<RailGunPayload.Def>().attractToBeacons = true;
		gameObject.AddComponent<LoopingSounds>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(gameObject, false);
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.allowItemRemoval = false;
		storage.capacityKg = 200f;
		DropAllWorkable dropAllWorkable = gameObject.AddOrGet<DropAllWorkable>();
		dropAllWorkable.dropWorkTime = 30f;
		dropAllWorkable.choreTypeID = Db.Get().ChoreTypes.Fetch.Id;
		dropAllWorkable.ConfigureMultitoolContext("build", EffectConfigs.BuildSplashId);
		ClusterDestinationSelector clusterDestinationSelector = gameObject.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.assignable = false;
		clusterDestinationSelector.shouldPointTowardsPath = true;
		clusterDestinationSelector.requireAsteroidDestination = true;
		BallisticClusterGridEntity ballisticClusterGridEntity = gameObject.AddOrGet<BallisticClusterGridEntity>();
		ballisticClusterGridEntity.clusterAnimName = "payload01_kanim";
		ballisticClusterGridEntity.isWorldEntity = true;
		ballisticClusterGridEntity.nameKey = new StringKey("STRINGS.ITEMS.RAILGUNPAYLOAD.NAME");
		gameObject.AddOrGet<ClusterTraveler>();
		return gameObject;
	}

	// Token: 0x0600143C RID: 5180 RVA: 0x00072F53 File Offset: 0x00071153
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600143D RID: 5181 RVA: 0x00072F55 File Offset: 0x00071155
	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<Pickupable>().MinTakeAmount = true;
	}

	// Token: 0x04000C3A RID: 3130
	public const string ID = "RailGunPayload";

	// Token: 0x04000C3B RID: 3131
	public const float MASS = 200f;

	// Token: 0x04000C3C RID: 3132
	public const int LANDING_EDGE_PADDING = 3;
}
