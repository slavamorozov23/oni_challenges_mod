using System;
using UnityEngine;

// Token: 0x02000305 RID: 773
public class ClustercraftConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000FDF RID: 4063 RVA: 0x0006077D File Offset: 0x0005E97D
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000FE0 RID: 4064 RVA: 0x00060784 File Offset: 0x0005E984
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000FE1 RID: 4065 RVA: 0x00060788 File Offset: 0x0005E988
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("Clustercraft", "Clustercraft", true);
		SaveLoadRoot saveLoadRoot = gameObject.AddOrGet<SaveLoadRoot>();
		saveLoadRoot.DeclareOptionalComponent<WorldInventory>();
		saveLoadRoot.DeclareOptionalComponent<WorldContainer>();
		saveLoadRoot.DeclareOptionalComponent<OrbitalMechanics>();
		gameObject.AddOrGet<Clustercraft>();
		gameObject.AddOrGet<CraftModuleInterface>();
		gameObject.AddOrGet<UserNameable>();
		RocketClusterDestinationSelector rocketClusterDestinationSelector = gameObject.AddOrGet<RocketClusterDestinationSelector>();
		rocketClusterDestinationSelector.requireLaunchPadOnAsteroidDestination = true;
		rocketClusterDestinationSelector.assignable = true;
		rocketClusterDestinationSelector.shouldPointTowardsPath = true;
		gameObject.AddOrGet<ClusterTraveler>().stopAndNotifyWhenPathChanges = true;
		gameObject.AddOrGetDef<AlertStateManager.Def>();
		gameObject.AddOrGet<Notifier>();
		gameObject.AddOrGetDef<RocketSelfDestructMonitor.Def>();
		return gameObject;
	}

	// Token: 0x06000FE2 RID: 4066 RVA: 0x0006080C File Offset: 0x0005EA0C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000FE3 RID: 4067 RVA: 0x0006080E File Offset: 0x0005EA0E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000A64 RID: 2660
	public const string ID = "Clustercraft";
}
