using System;
using System.Collections.Generic;
using Database;
using TUNING;
using UnityEngine;

// Token: 0x020006D5 RID: 1749
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ArtifactFinder")]
public class ArtifactFinder : KMonoBehaviour
{
	// Token: 0x06002ACD RID: 10957 RVA: 0x000FAAFC File Offset: 0x000F8CFC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ArtifactFinder>(-887025858, ArtifactFinder.OnLandDelegate);
	}

	// Token: 0x06002ACE RID: 10958 RVA: 0x000FAB18 File Offset: 0x000F8D18
	public ArtifactTier GetArtifactDropTier(StoredMinionIdentity minionID, SpaceDestination destination)
	{
		ArtifactDropRate artifactDropTable = destination.GetDestinationType().artifactDropTable;
		bool flag = minionID.traitIDs.Contains("Archaeologist");
		if (artifactDropTable != null)
		{
			float num = artifactDropTable.totalWeight;
			if (flag)
			{
				num -= artifactDropTable.GetTierWeight(DECOR.SPACEARTIFACT.TIER_NONE);
			}
			float num2 = UnityEngine.Random.value * num;
			foreach (global::Tuple<ArtifactTier, float> tuple in artifactDropTable.rates)
			{
				if (!flag || (flag && tuple.first != DECOR.SPACEARTIFACT.TIER_NONE))
				{
					num2 -= tuple.second;
				}
				if (num2 <= 0f)
				{
					return tuple.first;
				}
			}
		}
		return DECOR.SPACEARTIFACT.TIER0;
	}

	// Token: 0x06002ACF RID: 10959 RVA: 0x000FABE4 File Offset: 0x000F8DE4
	public List<string> GetArtifactsOfTier(ArtifactTier tier)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<ArtifactType, List<string>> keyValuePair in ArtifactConfig.artifactItems)
		{
			foreach (string text in keyValuePair.Value)
			{
				GameObject prefab = Assets.GetPrefab(text.ToTag());
				ArtifactTier artifactTier = prefab.GetComponent<SpaceArtifact>().GetArtifactTier();
				if (Game.IsCorrectDlcActiveForCurrentSave(prefab.GetComponent<KPrefabID>()) && artifactTier == tier)
				{
					list.Add(text);
				}
			}
		}
		return list;
	}

	// Token: 0x06002AD0 RID: 10960 RVA: 0x000FACA8 File Offset: 0x000F8EA8
	public string SearchForArtifact(StoredMinionIdentity minionID, SpaceDestination destination)
	{
		ArtifactTier artifactDropTier = this.GetArtifactDropTier(minionID, destination);
		if (artifactDropTier == DECOR.SPACEARTIFACT.TIER_NONE)
		{
			return null;
		}
		List<string> artifactsOfTier = this.GetArtifactsOfTier(artifactDropTier);
		return artifactsOfTier[UnityEngine.Random.Range(0, artifactsOfTier.Count)];
	}

	// Token: 0x06002AD1 RID: 10961 RVA: 0x000FACE4 File Offset: 0x000F8EE4
	public void OnLand(object data)
	{
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(SpacecraftManager.instance.GetSpacecraftID(base.GetComponent<RocketModule>().conditionManager.GetComponent<ILaunchableRocket>()));
		foreach (MinionStorage.Info info in this.minionStorage.GetStoredMinionInfo())
		{
			StoredMinionIdentity minionID = info.serializedMinion.Get<StoredMinionIdentity>();
			string text = this.SearchForArtifact(minionID, spacecraftDestination);
			if (text != null)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(text.ToTag()), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0).SetActive(true);
			}
		}
	}

	// Token: 0x0400198A RID: 6538
	public const string ID = "ArtifactFinder";

	// Token: 0x0400198B RID: 6539
	[MyCmpReq]
	private MinionStorage minionStorage;

	// Token: 0x0400198C RID: 6540
	private static readonly EventSystem.IntraObjectHandler<ArtifactFinder> OnLandDelegate = new EventSystem.IntraObjectHandler<ArtifactFinder>(delegate(ArtifactFinder component, object data)
	{
		component.OnLand(data);
	});
}
