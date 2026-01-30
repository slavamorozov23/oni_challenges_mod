using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000B82 RID: 2946
public class LargeImpactorConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060057CB RID: 22475 RVA: 0x001FEE7F File Offset: 0x001FD07F
	public string[] GetRequiredDlcIds()
	{
		return new string[]
		{
			"EXPANSION1_ID",
			"DLC4_ID"
		};
	}

	// Token: 0x060057CC RID: 22476 RVA: 0x001FEE97 File Offset: 0x001FD097
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060057CD RID: 22477 RVA: 0x001FEE9C File Offset: 0x001FD09C
	GameObject IEntityConfig.CreatePrefab()
	{
		GameObject gameObject = LargeImpactorVanillaConfig.ConfigCommon("LargeImpactor", this.NAME);
		gameObject.AddOrGet<InfoDescription>().description = this.DESC;
		ClusterDestinationSelector clusterDestinationSelector = gameObject.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.assignable = false;
		clusterDestinationSelector.canNavigateFogOfWar = true;
		clusterDestinationSelector.requireAsteroidDestination = true;
		clusterDestinationSelector.requireLaunchPadOnAsteroidDestination = false;
		clusterDestinationSelector.dodgesHiddenAsteroids = true;
		ClusterMapMeteorShowerVisualizer clusterMapMeteorShowerVisualizer = gameObject.AddOrGet<ClusterMapMeteorShowerVisualizer>();
		clusterMapMeteorShowerVisualizer.p_name = this.NAME;
		clusterMapMeteorShowerVisualizer.clusterAnimName = "shower_cluster_demolior_kanim";
		clusterMapMeteorShowerVisualizer.revealed = true;
		clusterMapMeteorShowerVisualizer.forceRevealed = true;
		clusterMapMeteorShowerVisualizer.isWorldEntity = true;
		ClusterTraveler clusterTraveler = gameObject.AddOrGet<ClusterTraveler>();
		clusterTraveler.revealsFogOfWarAsItTravels = true;
		clusterTraveler.peekRadius = 0;
		clusterTraveler.quickTravelToAsteroidIfInOrbit = false;
		ClusterMapLargeImpactor.Def def = gameObject.AddOrGetDef<ClusterMapLargeImpactor.Def>();
		def.name = this.NAME;
		def.description = this.DESC;
		def.eventID = "LargeImpactor";
		return gameObject;
	}

	// Token: 0x060057CE RID: 22478 RVA: 0x001FEF65 File Offset: 0x001FD165
	void IEntityConfig.OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060057CF RID: 22479 RVA: 0x001FEF67 File Offset: 0x001FD167
	void IEntityConfig.OnSpawn(GameObject inst)
	{
		inst.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.ImpactorStatus, inst.GetComponent<ClusterTraveler>());
		LargeImpactorVanillaConfig.SpawnCommon(inst);
	}

	// Token: 0x04003ADD RID: 15069
	public const string ID = "LargeImpactor";

	// Token: 0x04003ADE RID: 15070
	public string NAME = UI.SPACEDESTINATIONS.CLUSTERMAPMETEORS.LARGEIMACTOR.NAME;

	// Token: 0x04003ADF RID: 15071
	public string DESC = UI.SPACEDESTINATIONS.CLUSTERMAPMETEORS.LARGEIMACTOR.DESCRIPTION;

	// Token: 0x04003AE0 RID: 15072
	public const string ANIMFILE = "shower_cluster_demolior_kanim";

	// Token: 0x04003AE1 RID: 15073
	public const int HEALTH = 1000;
}
