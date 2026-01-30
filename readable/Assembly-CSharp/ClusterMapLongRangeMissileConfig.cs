using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class ClusterMapLongRangeMissileConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600015F RID: 351 RVA: 0x0000A94B File Offset: 0x00008B4B
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000A952 File Offset: 0x00008B52
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000161 RID: 353 RVA: 0x0000A958 File Offset: 0x00008B58
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity("ClusterMapLongRangeMissile", ITEMS.MISSILE_LONGRANGE.NAME, ITEMS.MISSILE_LONGRANGE.DESC, 2000f, true, Assets.GetAnim("longrange_missile_clustermap_kanim"), "idle_loop", Grid.SceneLayer.Front, SimHashes.Creature, new List<Tag>
		{
			GameTags.IgnoreMaterialCategory,
			GameTags.Experimental
		}, 293f);
		gameObject.AddOrGetDef<ClusterMapLongRangeMissile.Def>();
		gameObject.AddComponent<LoopingSounds>();
		gameObject.AddOrGet<KSelectable>().IsSelectable = true;
		ClusterMapLongRangeMissileGridEntity clusterMapLongRangeMissileGridEntity = gameObject.AddOrGet<ClusterMapLongRangeMissileGridEntity>();
		clusterMapLongRangeMissileGridEntity.clusterAnimName = "longrange_missile_clustermap_kanim";
		clusterMapLongRangeMissileGridEntity.isWorldEntity = false;
		clusterMapLongRangeMissileGridEntity.nameKey = new StringKey("STRINGS.ITEMS.MISSILE_LONGRANGE.NAME");
		clusterMapLongRangeMissileGridEntity.keepRotationWhenSpacingOutInHex = true;
		ClusterDestinationSelector clusterDestinationSelector = gameObject.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.canNavigateFogOfWar = false;
		clusterDestinationSelector.dodgesHiddenAsteroids = true;
		clusterDestinationSelector.requireAsteroidDestination = false;
		clusterDestinationSelector.requireLaunchPadOnAsteroidDestination = false;
		clusterDestinationSelector.assignable = false;
		clusterDestinationSelector.shouldPointTowardsPath = true;
		gameObject.AddOrGet<ClusterTraveler>();
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000162 RID: 354 RVA: 0x0000AA49 File Offset: 0x00008C49
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000163 RID: 355 RVA: 0x0000AA4B File Offset: 0x00008C4B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040000DB RID: 219
	public const string ID = "ClusterMapLongRangeMissile";

	// Token: 0x040000DC RID: 220
	public const float MASS = 2000f;

	// Token: 0x040000DD RID: 221
	public const float STARMAP_SPEED = 10f;
}
