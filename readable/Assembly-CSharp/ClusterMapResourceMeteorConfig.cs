using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000B72 RID: 2930
public class ClusterMapResourceMeteorConfig : IMultiEntityConfig
{
	// Token: 0x060056BF RID: 22207 RVA: 0x001F923A File Offset: 0x001F743A
	public static string GetFullID(string id)
	{
		return "ClusterMapResourceMeteor_" + id;
	}

	// Token: 0x060056C0 RID: 22208 RVA: 0x001F9247 File Offset: 0x001F7447
	public static string GetReverseFullID(string fullID)
	{
		return fullID.Replace("ClusterMapResourceMeteor_", "");
	}

	// Token: 0x060056C1 RID: 22209 RVA: 0x001F925C File Offset: 0x001F745C
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		if (!DlcManager.IsExpansion1Active())
		{
			return list;
		}
		list.Add(ClusterMapResourceMeteorConfig.CreateClusterResourceMeteor("Copper", "ClusterCopperMeteor", UI.SPACEDESTINATIONS.CLUSTERMAPMETEORS.COPPER.NAME, UI.SPACEDESTINATIONS.CLUSTERMAPMETEORS.COPPER.DESCRIPTION, "shower_cluster_copper_kanim", "idle_loop", "ui", DlcManager.EXPANSION1, null, SimHashes.Unobtanium));
		list.Add(ClusterMapResourceMeteorConfig.CreateClusterResourceMeteor("Iron", "ClusterIronMeteor", UI.SPACEDESTINATIONS.CLUSTERMAPMETEORS.IRON.NAME, UI.SPACEDESTINATIONS.CLUSTERMAPMETEORS.IRON.DESCRIPTION, "shower_cluster_iron_kanim", "idle_loop", "ui", DlcManager.EXPANSION1, null, SimHashes.Unobtanium));
		list.RemoveAll((GameObject x) => x == null);
		return list;
	}

	// Token: 0x060056C2 RID: 22210 RVA: 0x001F9328 File Offset: 0x001F7528
	public static GameObject CreateClusterResourceMeteor(string id, string meteorEventID, string name, string desc, string animFile, string initial_anim = "idle_loop", string ui_anim = "ui", string[] requiredDlcIds = null, string[] forbiddenDlcIds = null, SimHashes element = SimHashes.Unobtanium)
	{
		if (!DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
		{
			return null;
		}
		GameObject gameObject = EntityTemplates.CreateLooseEntity(ClusterMapResourceMeteorConfig.GetFullID(id), name, desc, 2000f, true, Assets.GetAnim(animFile), initial_anim, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, false, SORTORDER.KEEPSAKES, element, new List<Tag>());
		gameObject.AddOrGet<KSelectable>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.GetComponent<KBatchedAnimController>().initialMode = KAnim.PlayMode.Loop;
		ClusterDestinationSelector clusterDestinationSelector = gameObject.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.assignable = false;
		clusterDestinationSelector.canNavigateFogOfWar = true;
		clusterDestinationSelector.requireAsteroidDestination = true;
		clusterDestinationSelector.requireLaunchPadOnAsteroidDestination = false;
		clusterDestinationSelector.dodgesHiddenAsteroids = true;
		ClusterMapMeteorShowerVisualizer clusterMapMeteorShowerVisualizer = gameObject.AddOrGet<ClusterMapMeteorShowerVisualizer>();
		clusterMapMeteorShowerVisualizer.p_name = name;
		clusterMapMeteorShowerVisualizer.clusterAnimName = animFile;
		ClusterTraveler clusterTraveler = gameObject.AddOrGet<ClusterTraveler>();
		clusterTraveler.revealsFogOfWarAsItTravels = false;
		clusterTraveler.quickTravelToAsteroidIfInOrbit = false;
		ClusterMapMeteorShower.Def def = gameObject.AddOrGetDef<ClusterMapMeteorShower.Def>();
		def.name = name;
		def.description = desc;
		def.name_Hidden = UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.UNIDENTIFIED.NAME;
		def.description_Hidden = UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.UNIDENTIFIED.DESCRIPTION;
		def.eventID = meteorEventID;
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.requiredDlcIds = requiredDlcIds;
		component.forbiddenDlcIds = forbiddenDlcIds;
		return gameObject;
	}

	// Token: 0x060056C3 RID: 22211 RVA: 0x001F9435 File Offset: 0x001F7635
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060056C4 RID: 22212 RVA: 0x001F9437 File Offset: 0x001F7637
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04003A89 RID: 14985
	public const string IDENTIFY_AUDIO_NAME = "ClusterMapMeteor_Reveal";

	// Token: 0x04003A8A RID: 14986
	public const string ID_SIGNATURE = "ClusterMapResourceMeteor_";
}
