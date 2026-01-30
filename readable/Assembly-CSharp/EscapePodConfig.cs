using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class EscapePodConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060002E7 RID: 743 RVA: 0x000151B6 File Offset: 0x000133B6
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x000151BD File Offset: 0x000133BD
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x000151C0 File Offset: 0x000133C0
	public GameObject CreatePrefab()
	{
		string id = "EscapePod";
		string name = STRINGS.BUILDINGS.PREFABS.ESCAPEPOD.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.ESCAPEPOD.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("escape_pod_kanim"), "grounded", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.RoomProberBuilding
		}, 293f);
		gameObject.AddOrGet<KBatchedAnimController>().fgLayer = Grid.SceneLayer.BuildingFront;
		TravellingCargoLander.Def def = gameObject.AddOrGetDef<TravellingCargoLander.Def>();
		def.landerWidth = 1;
		def.landingSpeed = 15f;
		def.deployOnLanding = true;
		CargoDropperMinion.Def def2 = gameObject.AddOrGetDef<CargoDropperMinion.Def>();
		def2.kAnimName = "anim_interacts_escape_pod_kanim";
		def2.animName = "deploying";
		def2.animLayer = Grid.SceneLayer.BuildingUse;
		def2.notifyOnJettison = true;
		BallisticClusterGridEntity ballisticClusterGridEntity = gameObject.AddOrGet<BallisticClusterGridEntity>();
		ballisticClusterGridEntity.clusterAnimName = "escape_pod01_kanim";
		ballisticClusterGridEntity.isWorldEntity = true;
		ballisticClusterGridEntity.nameKey = new StringKey("STRINGS.BUILDINGS.PREFABS.ESCAPEPOD.NAME");
		ClusterDestinationSelector clusterDestinationSelector = gameObject.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.assignable = false;
		clusterDestinationSelector.shouldPointTowardsPath = true;
		clusterDestinationSelector.requireAsteroidDestination = true;
		clusterDestinationSelector.canNavigateFogOfWar = true;
		gameObject.AddOrGet<ClusterTraveler>();
		gameObject.AddOrGet<MinionStorage>();
		gameObject.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(gameObject);
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<Deconstructable>().audioSize = "large";
		return gameObject;
	}

	// Token: 0x060002EA RID: 746 RVA: 0x000152FF File Offset: 0x000134FF
	public void OnPrefabInit(GameObject inst)
	{
		OccupyArea component = inst.GetComponent<OccupyArea>();
		component.ApplyToCells = false;
		component.objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060002EB RID: 747 RVA: 0x0001531D File Offset: 0x0001351D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040001AF RID: 431
	public const string ID = "EscapePod";

	// Token: 0x040001B0 RID: 432
	public const float MASS = 100f;
}
