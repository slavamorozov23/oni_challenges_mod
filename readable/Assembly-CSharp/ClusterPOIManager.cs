using System;
using System.Collections.Generic;
using KSerialization;
using ProcGenGame;
using UnityEngine;

// Token: 0x0200084E RID: 2126
[SerializationConfig(MemberSerialization.OptIn)]
public class ClusterPOIManager : KMonoBehaviour
{
	// Token: 0x06003A5A RID: 14938 RVA: 0x0014615F File Offset: 0x0014435F
	private ClusterFogOfWarManager.Instance GetFOWManager()
	{
		if (this.m_fowManager == null)
		{
			this.m_fowManager = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
		}
		return this.m_fowManager;
	}

	// Token: 0x06003A5B RID: 14939 RVA: 0x0014617F File Offset: 0x0014437F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			UIScheduler.Instance.ScheduleNextFrame("UpgradeOldSaves", delegate(object o)
			{
				this.UpgradeOldSaves();
			}, null, null);
		}
	}

	// Token: 0x06003A5C RID: 14940 RVA: 0x001461AC File Offset: 0x001443AC
	public void RegisterTemporalTear(TemporalTear temporalTear)
	{
		this.m_temporalTear.Set(temporalTear);
	}

	// Token: 0x06003A5D RID: 14941 RVA: 0x001461BA File Offset: 0x001443BA
	public bool HasTemporalTear()
	{
		return this.m_temporalTear.Get() != null;
	}

	// Token: 0x06003A5E RID: 14942 RVA: 0x001461CD File Offset: 0x001443CD
	public TemporalTear GetTemporalTear()
	{
		return this.m_temporalTear.Get();
	}

	// Token: 0x06003A5F RID: 14943 RVA: 0x001461DC File Offset: 0x001443DC
	private void UpgradeOldSaves()
	{
		bool flag = false;
		foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in ClusterGrid.Instance.cellContents)
		{
			foreach (ClusterGridEntity clusterGridEntity in keyValuePair.Value)
			{
				if (clusterGridEntity.GetComponent<HarvestablePOIClusterGridEntity>() || clusterGridEntity.GetComponent<ArtifactPOIClusterGridEntity>())
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (!flag)
		{
			ClusterManager.Instance.GetClusterPOIManager().SpawnSpacePOIsInLegacySave();
		}
	}

	// Token: 0x06003A60 RID: 14944 RVA: 0x001462A4 File Offset: 0x001444A4
	public void SpawnSpacePOIsInLegacySave()
	{
		Dictionary<int[], string[]> dictionary = new Dictionary<int[], string[]>();
		dictionary.Add(new int[]
		{
			2,
			3
		}, new string[]
		{
			"HarvestableSpacePOI_SandyOreField"
		});
		dictionary.Add(new int[]
		{
			5,
			7
		}, new string[]
		{
			"HarvestableSpacePOI_OrganicMassField"
		});
		dictionary.Add(new int[]
		{
			8,
			11
		}, new string[]
		{
			"HarvestableSpacePOI_GildedAsteroidField",
			"HarvestableSpacePOI_GlimmeringAsteroidField",
			"HarvestableSpacePOI_HeliumCloud",
			"HarvestableSpacePOI_OilyAsteroidField",
			"HarvestableSpacePOI_FrozenOreField"
		});
		dictionary.Add(new int[]
		{
			10,
			11
		}, new string[]
		{
			"HarvestableSpacePOI_RadioactiveGasCloud",
			"HarvestableSpacePOI_RadioactiveAsteroidField"
		});
		dictionary.Add(new int[]
		{
			5,
			7
		}, new string[]
		{
			"HarvestableSpacePOI_RockyAsteroidField",
			"HarvestableSpacePOI_InterstellarIceField",
			"HarvestableSpacePOI_InterstellarOcean",
			"HarvestableSpacePOI_SandyOreField",
			"HarvestableSpacePOI_SwampyOreField"
		});
		dictionary.Add(new int[]
		{
			7,
			11
		}, new string[]
		{
			"HarvestableSpacePOI_MetallicAsteroidField",
			"HarvestableSpacePOI_SatelliteField",
			"HarvestableSpacePOI_ChlorineCloud",
			"HarvestableSpacePOI_OxidizedAsteroidField",
			"HarvestableSpacePOI_OxygenRichAsteroidField",
			"HarvestableSpacePOI_GildedAsteroidField",
			"HarvestableSpacePOI_HeliumCloud",
			"HarvestableSpacePOI_OilyAsteroidField",
			"HarvestableSpacePOI_FrozenOreField",
			"HarvestableSpacePOI_RadioactiveAsteroidField"
		});
		List<AxialI> list = new List<AxialI>();
		string[] array;
		foreach (KeyValuePair<int[], string[]> keyValuePair in dictionary)
		{
			int[] key = keyValuePair.Key;
			string[] value = keyValuePair.Value;
			int minRadius = Mathf.Min(key[0], ClusterGrid.Instance.numRings - 1);
			int maxRadius = Mathf.Min(key[1], ClusterGrid.Instance.numRings - 1);
			List<AxialI> rings = AxialUtil.GetRings(AxialI.ZERO, minRadius, maxRadius);
			List<AxialI> list2 = new List<AxialI>();
			foreach (AxialI axialI in rings)
			{
				ClusterGrid instance = ClusterGrid.Instance;
				Dictionary<AxialI, List<ClusterGridEntity>> cellContents = ClusterGrid.Instance.cellContents;
				List<ClusterGridEntity> list3 = ClusterGrid.Instance.cellContents[axialI];
				if (ClusterGrid.Instance.cellContents[axialI].Count == 0 && ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(axialI, EntityLayer.Asteroid) == null)
				{
					list2.Add(axialI);
				}
			}
			array = value;
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(array[i]), null, null);
				AxialI axialI2 = list2[UnityEngine.Random.Range(0, list2.Count - 1)];
				list2.Remove(axialI2);
				list.Add(axialI2);
				gameObject.GetComponent<ClusterGridEntity>().Location = axialI2;
				gameObject.SetActive(true);
			}
		}
		string[] array2 = new string[]
		{
			"ArtifactSpacePOI_GravitasSpaceStation1",
			"ArtifactSpacePOI_GravitasSpaceStation4",
			"ArtifactSpacePOI_GravitasSpaceStation5",
			"ArtifactSpacePOI_GravitasSpaceStation6",
			"ArtifactSpacePOI_GravitasSpaceStation8",
			"ArtifactSpacePOI_RussellsTeapot"
		};
		int minRadius2 = Mathf.Min(2, ClusterGrid.Instance.numRings - 1);
		int maxRadius2 = Mathf.Min(11, ClusterGrid.Instance.numRings - 1);
		List<AxialI> rings2 = AxialUtil.GetRings(AxialI.ZERO, minRadius2, maxRadius2);
		List<AxialI> list4 = new List<AxialI>();
		foreach (AxialI axialI3 in rings2)
		{
			if (ClusterGrid.Instance.cellContents[axialI3].Count == 0 && ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(axialI3, EntityLayer.Asteroid) == null && !list.Contains(axialI3))
			{
				list4.Add(axialI3);
			}
		}
		array = array2;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab(array[i]), null, null);
			AxialI axialI4 = list4[UnityEngine.Random.Range(0, list4.Count - 1)];
			list4.Remove(axialI4);
			HarvestablePOIClusterGridEntity component = gameObject2.GetComponent<HarvestablePOIClusterGridEntity>();
			if (component != null)
			{
				component.Init(axialI4);
			}
			ArtifactPOIClusterGridEntity component2 = gameObject2.GetComponent<ArtifactPOIClusterGridEntity>();
			if (component2 != null)
			{
				component2.Init(axialI4);
			}
			gameObject2.SetActive(true);
		}
	}

	// Token: 0x06003A61 RID: 14945 RVA: 0x00146748 File Offset: 0x00144948
	public void PopulatePOIsFromWorldGen(Cluster clusterLayout)
	{
		foreach (KeyValuePair<AxialI, string> keyValuePair in clusterLayout.poiPlacements)
		{
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(keyValuePair.Value), null, null);
			gameObject.GetComponent<ClusterGridEntity>().Location = keyValuePair.Key;
			gameObject.SetActive(true);
		}
	}

	// Token: 0x06003A62 RID: 14946 RVA: 0x001467C4 File Offset: 0x001449C4
	public void RevealTemporalTear()
	{
		if (this.m_temporalTear.Get() == null)
		{
			global::Debug.LogWarning("This cluster has no temporal tear, but has the poi mechanism to reveal it");
			return;
		}
		AxialI location = this.m_temporalTear.Get().Location;
		this.GetFOWManager().RevealLocation(location, 1, 2);
	}

	// Token: 0x06003A63 RID: 14947 RVA: 0x0014680E File Offset: 0x00144A0E
	public bool IsTemporalTearRevealed()
	{
		if (this.m_temporalTear.Get() == null)
		{
			global::Debug.LogWarning("This cluster has no temporal tear, but has the poi mechanism to reveal it");
			return false;
		}
		return this.GetFOWManager().IsLocationRevealed(this.m_temporalTear.Get().Location);
	}

	// Token: 0x06003A64 RID: 14948 RVA: 0x0014684C File Offset: 0x00144A4C
	public void OpenTemporalTear(int openerWorldId)
	{
		if (this.m_temporalTear.Get() == null)
		{
			global::Debug.LogWarning("This cluster has no temporal tear, but has the poi mechanism to open it");
			return;
		}
		if (!this.m_temporalTear.Get().IsOpen())
		{
			this.m_temporalTear.Get().Open();
			ClusterManager.Instance.GetWorld(openerWorldId).GetSMI<GameplaySeasonManager.Instance>().StartNewSeason(Db.Get().GameplaySeasons.TemporalTearMeteorShowers);
		}
	}

	// Token: 0x06003A65 RID: 14949 RVA: 0x001468BD File Offset: 0x00144ABD
	public bool HasTemporalTearConsumedCraft()
	{
		return !(this.m_temporalTear.Get() == null) && this.m_temporalTear.Get().HasConsumedCraft();
	}

	// Token: 0x06003A66 RID: 14950 RVA: 0x001468E4 File Offset: 0x00144AE4
	public bool IsTemporalTearOpen()
	{
		return !(this.m_temporalTear.Get() == null) && this.m_temporalTear.Get().IsOpen();
	}

	// Token: 0x0400238D RID: 9101
	[Serialize]
	private List<Ref<ResearchDestination>> m_researchDestinations = new List<Ref<ResearchDestination>>();

	// Token: 0x0400238E RID: 9102
	[Serialize]
	private Ref<TemporalTear> m_temporalTear = new Ref<TemporalTear>();

	// Token: 0x0400238F RID: 9103
	private ClusterFogOfWarManager.Instance m_fowManager;
}
