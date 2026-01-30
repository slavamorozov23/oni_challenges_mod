using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200064E RID: 1614
public class TrackerTool : KMonoBehaviour
{
	// Token: 0x0600274C RID: 10060 RVA: 0x000E1D28 File Offset: 0x000DFF28
	protected override void OnSpawn()
	{
		TrackerTool.Instance = this;
		base.OnSpawn();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			this.AddNewWorldTrackers(worldContainer.id);
		}
		foreach (object obj in Components.LiveMinionIdentities)
		{
			this.AddMinionTrackers((MinionIdentity)obj);
		}
		Components.LiveMinionIdentities.OnAdd += this.AddMinionTrackers;
		ClusterManager.Instance.Subscribe(-1280433810, new Action<object>(this.Refresh));
		ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.RemoveWorld));
	}

	// Token: 0x0600274D RID: 10061 RVA: 0x000E1E28 File Offset: 0x000E0028
	protected override void OnForcedCleanUp()
	{
		TrackerTool.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600274E RID: 10062 RVA: 0x000E1E38 File Offset: 0x000E0038
	private void AddMinionTrackers(MinionIdentity identity)
	{
		this.minionTrackers.Add(identity, new List<MinionTracker>());
		identity.Subscribe(1969584890, delegate(object data)
		{
			this.minionTrackers.Remove(identity);
		});
	}

	// Token: 0x0600274F RID: 10063 RVA: 0x000E1E8C File Offset: 0x000E008C
	private void Refresh(object data)
	{
		int value = ((Boxed<int>)data).value;
		this.AddNewWorldTrackers(value);
	}

	// Token: 0x06002750 RID: 10064 RVA: 0x000E1EAC File Offset: 0x000E00AC
	private void RemoveWorld(object data)
	{
		int world_id = ((Boxed<int>)data).value;
		this.worldTrackers.RemoveAll((WorldTracker match) => match.WorldID == world_id);
	}

	// Token: 0x06002751 RID: 10065 RVA: 0x000E1EE8 File Offset: 0x000E00E8
	public bool IsRocketInterior(int worldID)
	{
		return ClusterManager.Instance.GetWorld(worldID).IsModuleInterior;
	}

	// Token: 0x06002752 RID: 10066 RVA: 0x000E1EFC File Offset: 0x000E00FC
	private void AddNewWorldTrackers(int worldID)
	{
		this.worldTrackers.Add(new StressTracker(worldID));
		this.worldTrackers.Add(new KCalTracker(worldID));
		this.worldTrackers.Add(new IdleTracker(worldID));
		this.worldTrackers.Add(new BreathabilityTracker(worldID));
		this.worldTrackers.Add(new PowerUseTracker(worldID));
		this.worldTrackers.Add(new BatteryTracker(worldID));
		this.worldTrackers.Add(new CropTracker(worldID));
		this.worldTrackers.Add(new WorkingToiletTracker(worldID));
		this.worldTrackers.Add(new RadiationTracker(worldID));
		if (Game.IsDlcActiveForCurrentSave("DLC3_ID"))
		{
			this.worldTrackers.Add(new ElectrobankJoulesTracker(worldID));
		}
		if (ClusterManager.Instance.GetWorld(worldID).IsModuleInterior)
		{
			this.worldTrackers.Add(new RocketFuelTracker(worldID));
			this.worldTrackers.Add(new RocketOxidizerTracker(worldID));
		}
		for (int i = 0; i < Db.Get().ChoreGroups.Count; i++)
		{
			this.worldTrackers.Add(new WorkTimeTracker(worldID, Db.Get().ChoreGroups[i]));
			this.worldTrackers.Add(new ChoreCountTracker(worldID, Db.Get().ChoreGroups[i]));
		}
		this.worldTrackers.Add(new AllChoresCountTracker(worldID));
		this.worldTrackers.Add(new AllWorkTimeTracker(worldID));
		foreach (Tag tag in GameTags.CalorieCategories)
		{
			this.worldTrackers.Add(new ResourceTracker(worldID, tag));
			foreach (GameObject gameObject in Assets.GetPrefabsWithTag(tag))
			{
				this.AddResourceTracker(worldID, gameObject.GetComponent<KPrefabID>().PrefabTag);
			}
		}
		foreach (Tag tag2 in GameTags.UnitCategories)
		{
			this.worldTrackers.Add(new ResourceTracker(worldID, tag2));
			foreach (GameObject gameObject2 in Assets.GetPrefabsWithTag(tag2))
			{
				this.AddResourceTracker(worldID, gameObject2.GetComponent<KPrefabID>().PrefabTag);
			}
		}
		foreach (Tag tag3 in GameTags.MaterialCategories)
		{
			this.worldTrackers.Add(new ResourceTracker(worldID, tag3));
			foreach (GameObject gameObject3 in Assets.GetPrefabsWithTag(tag3))
			{
				this.AddResourceTracker(worldID, gameObject3.GetComponent<KPrefabID>().PrefabTag);
			}
		}
		foreach (Tag tag4 in GameTags.OtherEntityTags)
		{
			this.worldTrackers.Add(new ResourceTracker(worldID, tag4));
			foreach (GameObject gameObject4 in Assets.GetPrefabsWithTag(tag4))
			{
				this.AddResourceTracker(worldID, gameObject4.GetComponent<KPrefabID>().PrefabTag);
			}
		}
		foreach (GameObject gameObject5 in Assets.GetPrefabsWithTag(GameTags.CookingIngredient))
		{
			this.AddResourceTracker(worldID, gameObject5.GetComponent<KPrefabID>().PrefabTag);
		}
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			this.AddResourceTracker(worldID, foodInfo.Id);
		}
		foreach (Element element in ElementLoader.elements)
		{
			this.AddResourceTracker(worldID, element.tag);
		}
	}

	// Token: 0x06002753 RID: 10067 RVA: 0x000E23C0 File Offset: 0x000E05C0
	private void AddResourceTracker(int worldID, Tag tag)
	{
		if (this.worldTrackers.Find((WorldTracker match) => match is ResourceTracker && ((ResourceTracker)match).WorldID == worldID && ((ResourceTracker)match).tag == tag) != null)
		{
			return;
		}
		this.worldTrackers.Add(new ResourceTracker(worldID, tag));
	}

	// Token: 0x06002754 RID: 10068 RVA: 0x000E2418 File Offset: 0x000E0618
	public ResourceTracker GetResourceStatistic(int worldID, Tag tag)
	{
		return (ResourceTracker)this.worldTrackers.Find((WorldTracker match) => match is ResourceTracker && ((ResourceTracker)match).WorldID == worldID && ((ResourceTracker)match).tag == tag);
	}

	// Token: 0x06002755 RID: 10069 RVA: 0x000E2458 File Offset: 0x000E0658
	public WorldTracker GetWorldTracker<T>(int worldID) where T : WorldTracker
	{
		return (T)((object)this.worldTrackers.Find((WorldTracker match) => match is T && ((T)((object)match)).WorldID == worldID));
	}

	// Token: 0x06002756 RID: 10070 RVA: 0x000E2494 File Offset: 0x000E0694
	public ChoreCountTracker GetChoreGroupTracker(int worldID, ChoreGroup choreGroup)
	{
		return (ChoreCountTracker)this.worldTrackers.Find((WorldTracker match) => match is ChoreCountTracker && ((ChoreCountTracker)match).WorldID == worldID && ((ChoreCountTracker)match).choreGroup == choreGroup);
	}

	// Token: 0x06002757 RID: 10071 RVA: 0x000E24D4 File Offset: 0x000E06D4
	public WorkTimeTracker GetWorkTimeTracker(int worldID, ChoreGroup choreGroup)
	{
		return (WorkTimeTracker)this.worldTrackers.Find((WorldTracker match) => match is WorkTimeTracker && ((WorkTimeTracker)match).WorldID == worldID && ((WorkTimeTracker)match).choreGroup == choreGroup);
	}

	// Token: 0x06002758 RID: 10072 RVA: 0x000E2511 File Offset: 0x000E0711
	public MinionTracker GetMinionTracker<T>(MinionIdentity identity) where T : MinionTracker
	{
		return (T)((object)this.minionTrackers[identity].Find((MinionTracker match) => match is T));
	}

	// Token: 0x06002759 RID: 10073 RVA: 0x000E2550 File Offset: 0x000E0750
	public void Update()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		if (!this.trackerActive)
		{
			return;
		}
		if (this.minionTrackers.Count > 0)
		{
			this.updatingMinionTracker++;
			if (this.updatingMinionTracker >= this.minionTrackers.Count)
			{
				this.updatingMinionTracker = 0;
			}
			KeyValuePair<MinionIdentity, List<MinionTracker>> keyValuePair = this.minionTrackers.ElementAt(this.updatingMinionTracker);
			for (int i = 0; i < keyValuePair.Value.Count; i++)
			{
				keyValuePair.Value[i].UpdateData();
			}
		}
		if (this.worldTrackers.Count > 0)
		{
			for (int j = 0; j < this.numUpdatesPerFrame; j++)
			{
				this.updatingWorldTracker++;
				if (this.updatingWorldTracker >= this.worldTrackers.Count)
				{
					this.updatingWorldTracker = 0;
				}
				this.worldTrackers[this.updatingWorldTracker].UpdateData();
			}
		}
	}

	// Token: 0x04001737 RID: 5943
	public static TrackerTool Instance;

	// Token: 0x04001738 RID: 5944
	private List<WorldTracker> worldTrackers = new List<WorldTracker>();

	// Token: 0x04001739 RID: 5945
	private Dictionary<MinionIdentity, List<MinionTracker>> minionTrackers = new Dictionary<MinionIdentity, List<MinionTracker>>();

	// Token: 0x0400173A RID: 5946
	private int updatingWorldTracker;

	// Token: 0x0400173B RID: 5947
	private int updatingMinionTracker;

	// Token: 0x0400173C RID: 5948
	public bool trackerActive = true;

	// Token: 0x0400173D RID: 5949
	private int numUpdatesPerFrame = 50;
}
