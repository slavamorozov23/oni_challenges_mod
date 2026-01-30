using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

// Token: 0x02000C28 RID: 3112
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/WorldInventory")]
public class WorldInventory : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170006DB RID: 1755
	// (get) Token: 0x06005E04 RID: 24068 RVA: 0x002205F7 File Offset: 0x0021E7F7
	public WorldContainer WorldContainer
	{
		get
		{
			if (this.m_worldContainer == null)
			{
				this.m_worldContainer = base.GetComponent<WorldContainer>();
			}
			return this.m_worldContainer;
		}
	}

	// Token: 0x170006DC RID: 1756
	// (get) Token: 0x06005E05 RID: 24069 RVA: 0x00220619 File Offset: 0x0021E819
	private MinionGroupProber Prober
	{
		get
		{
			return MinionGroupProber.Get();
		}
	}

	// Token: 0x170006DD RID: 1757
	// (get) Token: 0x06005E06 RID: 24070 RVA: 0x00220620 File Offset: 0x0021E820
	public bool HasValidCount
	{
		get
		{
			return this.hasValidCount;
		}
	}

	// Token: 0x170006DE RID: 1758
	// (get) Token: 0x06005E07 RID: 24071 RVA: 0x00220628 File Offset: 0x0021E828
	private int worldId
	{
		get
		{
			WorldContainer worldContainer = this.WorldContainer;
			if (!(worldContainer != null))
			{
				return -1;
			}
			return worldContainer.id;
		}
	}

	// Token: 0x06005E08 RID: 24072 RVA: 0x00220650 File Offset: 0x0021E850
	protected override void OnPrefabInit()
	{
		base.Subscribe(Game.Instance.gameObject, -1588644844, new Action<object>(this.OnAddedFetchable));
		base.Subscribe(Game.Instance.gameObject, -1491270284, new Action<object>(this.OnRemovedFetchable));
		base.Subscribe<WorldInventory>(631075836, WorldInventory.OnNewDayDelegate);
		this.m_worldContainer = base.GetComponent<WorldContainer>();
	}

	// Token: 0x06005E09 RID: 24073 RVA: 0x002206C0 File Offset: 0x0021E8C0
	protected override void OnCleanUp()
	{
		base.Unsubscribe(Game.Instance.gameObject, -1588644844, new Action<object>(this.OnAddedFetchable));
		base.Unsubscribe(Game.Instance.gameObject, -1491270284, new Action<object>(this.OnRemovedFetchable));
		base.OnCleanUp();
	}

	// Token: 0x06005E0A RID: 24074 RVA: 0x00220718 File Offset: 0x0021E918
	private void GenerateInventoryReport(object data)
	{
		int num = 0;
		int num2 = 0;
		foreach (Brain brain in Components.Brains.GetWorldItems(this.worldId, false))
		{
			CreatureBrain creatureBrain = brain as CreatureBrain;
			if (creatureBrain != null)
			{
				if (creatureBrain.HasTag(GameTags.Creatures.Wild))
				{
					num++;
					ReportManager.Instance.ReportValue(ReportManager.ReportType.WildCritters, 1f, creatureBrain.GetProperName(), creatureBrain.GetProperName());
				}
				else
				{
					num2++;
					ReportManager.Instance.ReportValue(ReportManager.ReportType.DomesticatedCritters, 1f, creatureBrain.GetProperName(), creatureBrain.GetProperName());
				}
			}
		}
		if (DlcManager.IsExpansion1Active())
		{
			WorldContainer component = base.GetComponent<WorldContainer>();
			if (component != null && component.IsModuleInterior)
			{
				Clustercraft clustercraft = component.GetComponent<ClusterGridEntity>() as Clustercraft;
				if (clustercraft != null && clustercraft.Status != Clustercraft.CraftStatus.Grounded)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.RocketsInFlight, 1f, clustercraft.Name, null);
					return;
				}
			}
		}
		else
		{
			foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
			{
				if (spacecraft.state != Spacecraft.MissionState.Grounded && spacecraft.state != Spacecraft.MissionState.Destroyed)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.RocketsInFlight, 1f, spacecraft.rocketName, null);
				}
			}
		}
	}

	// Token: 0x06005E0B RID: 24075 RVA: 0x002208A8 File Offset: 0x0021EAA8
	protected override void OnSpawn()
	{
		base.StartCoroutine(this.InitialRefresh());
	}

	// Token: 0x06005E0C RID: 24076 RVA: 0x002208B7 File Offset: 0x0021EAB7
	private IEnumerator InitialRefresh()
	{
		int num;
		for (int i = 0; i < 1; i = num)
		{
			yield return null;
			num = i + 1;
		}
		for (int j = 0; j < Components.Pickupables.Count; j++)
		{
			Pickupable pickupable = Components.Pickupables[j];
			if (pickupable != null)
			{
				ReachabilityMonitor.Instance smi = pickupable.GetSMI<ReachabilityMonitor.Instance>();
				if (smi != null)
				{
					smi.UpdateReachability();
				}
			}
		}
		yield break;
	}

	// Token: 0x06005E0D RID: 24077 RVA: 0x002208BF File Offset: 0x0021EABF
	public bool IsReachable(Pickupable pickupable)
	{
		return this.Prober.IsReachable(pickupable);
	}

	// Token: 0x06005E0E RID: 24078 RVA: 0x002208D0 File Offset: 0x0021EAD0
	public float GetTotalAmount(Tag tag, bool includeRelatedWorlds)
	{
		float result = 0f;
		this.accessibleAmounts.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x06005E0F RID: 24079 RVA: 0x002208F4 File Offset: 0x0021EAF4
	public ICollection<Pickupable> GetPickupables(Tag tag, bool includeRelatedWorlds = false)
	{
		if (!includeRelatedWorlds)
		{
			HashSet<Pickupable> result = null;
			this.Inventory.TryGetValue(tag, out result);
			return result;
		}
		return ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag);
	}

	// Token: 0x06005E10 RID: 24080 RVA: 0x00220920 File Offset: 0x0021EB20
	public List<Pickupable> CreatePickupablesList(Tag tag)
	{
		HashSet<Pickupable> hashSet = null;
		this.Inventory.TryGetValue(tag, out hashSet);
		if (hashSet == null)
		{
			return null;
		}
		return hashSet.ToList<Pickupable>();
	}

	// Token: 0x06005E11 RID: 24081 RVA: 0x0022094C File Offset: 0x0021EB4C
	public float GetAmount(Tag tag, bool includeRelatedWorlds)
	{
		float num;
		if (!includeRelatedWorlds)
		{
			num = this.GetTotalAmount(tag, includeRelatedWorlds);
			num -= MaterialNeeds.GetAmount(tag, this.worldId, includeRelatedWorlds);
		}
		else
		{
			num = ClusterUtil.GetAmountFromRelatedWorlds(this, tag);
		}
		return Mathf.Max(num, 0f);
	}

	// Token: 0x06005E12 RID: 24082 RVA: 0x00220994 File Offset: 0x0021EB94
	public int GetCountWithAdditionalTag(Tag tag, Tag additionalTag, bool includeRelatedWorlds = false)
	{
		ICollection<Pickupable> collection;
		if (!includeRelatedWorlds)
		{
			collection = this.GetPickupables(tag, false);
		}
		else
		{
			ICollection<Pickupable> pickupablesFromRelatedWorlds = ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag);
			collection = pickupablesFromRelatedWorlds;
		}
		ICollection<Pickupable> collection2 = collection;
		int num = 0;
		if (collection2 != null)
		{
			if (additionalTag.IsValid)
			{
				using (IEnumerator<Pickupable> enumerator = collection2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HasTag(additionalTag))
						{
							num++;
						}
					}
					return num;
				}
			}
			num = collection2.Count;
		}
		return num;
	}

	// Token: 0x06005E13 RID: 24083 RVA: 0x00220A10 File Offset: 0x0021EC10
	public float GetAmountWithoutTag(Tag tag, bool includeRelatedWorlds = false, Tag[] forbiddenTags = null)
	{
		if (forbiddenTags == null)
		{
			return this.GetAmount(tag, includeRelatedWorlds);
		}
		float num = 0f;
		ICollection<Pickupable> collection;
		if (!includeRelatedWorlds)
		{
			collection = this.GetPickupables(tag, false);
		}
		else
		{
			ICollection<Pickupable> pickupablesFromRelatedWorlds = ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag);
			collection = pickupablesFromRelatedWorlds;
		}
		ICollection<Pickupable> collection2 = collection;
		if (collection2 != null)
		{
			foreach (Pickupable pickupable in collection2)
			{
				if (pickupable != null && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate) && !pickupable.KPrefabID.HasAnyTags(forbiddenTags))
				{
					num += pickupable.TotalAmount;
				}
			}
		}
		return num;
	}

	// Token: 0x06005E14 RID: 24084 RVA: 0x00220AB8 File Offset: 0x0021ECB8
	private void Update()
	{
		int num = 0;
		Dictionary<Tag, HashSet<Pickupable>>.Enumerator enumerator = this.Inventory.GetEnumerator();
		int worldId = this.worldId;
		while (enumerator.MoveNext())
		{
			KeyValuePair<Tag, HashSet<Pickupable>> keyValuePair = enumerator.Current;
			if (num == this.accessibleUpdateIndex || this.firstUpdate)
			{
				Tag key = keyValuePair.Key;
				IEnumerable<Pickupable> value = keyValuePair.Value;
				float num2 = 0f;
				foreach (Pickupable pickupable in value)
				{
					if (pickupable != null && pickupable.GetMyWorldId() == worldId && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
					{
						num2 += pickupable.TotalAmount;
					}
				}
				if (!this.hasValidCount && this.accessibleUpdateIndex + 1 >= this.Inventory.Count)
				{
					this.hasValidCount = true;
					if (this.worldId == ClusterManager.Instance.activeWorldId)
					{
						this.hasValidCount = true;
						PinnedResourcesPanel.Instance.ClearExcessiveNewItems();
						PinnedResourcesPanel.Instance.Refresh();
					}
				}
				this.accessibleAmounts[key] = num2;
				this.accessibleUpdateIndex = (this.accessibleUpdateIndex + 1) % this.Inventory.Count;
				break;
			}
			num++;
		}
		this.firstUpdate = false;
	}

	// Token: 0x06005E15 RID: 24085 RVA: 0x00220C14 File Offset: 0x0021EE14
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
	}

	// Token: 0x06005E16 RID: 24086 RVA: 0x00220C1C File Offset: 0x0021EE1C
	private void OnAddedFetchable(object data)
	{
		GameObject gameObject = (GameObject)data;
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		if (component.HasAnyTags(WorldInventory.NonCritterEntitiesTags))
		{
			return;
		}
		Pickupable component2 = gameObject.GetComponent<Pickupable>();
		if (component2.GetMyWorldId() != this.worldId)
		{
			return;
		}
		Tag tag = component.PrefabID();
		if (!this.Inventory.ContainsKey(tag))
		{
			Tag categoryForEntity = DiscoveredResources.GetCategoryForEntity(component);
			DebugUtil.DevAssertArgs(categoryForEntity.IsValid, new object[]
			{
				component2.name,
				"was found by worldinventory but doesn't have a category! Add it to the element definition."
			});
			DiscoveredResources.Instance.Discover(tag, categoryForEntity);
		}
		HashSet<Pickupable> hashSet;
		if (!this.Inventory.TryGetValue(tag, out hashSet))
		{
			hashSet = new HashSet<Pickupable>();
			this.Inventory[tag] = hashSet;
		}
		hashSet.Add(component2);
		foreach (Tag key in component.Tags)
		{
			if (!this.Inventory.TryGetValue(key, out hashSet))
			{
				hashSet = new HashSet<Pickupable>();
				this.Inventory[key] = hashSet;
			}
			hashSet.Add(component2);
		}
	}

	// Token: 0x06005E17 RID: 24087 RVA: 0x00220D48 File Offset: 0x0021EF48
	private void OnRemovedFetchable(object data)
	{
		Pickupable component = ((GameObject)data).GetComponent<Pickupable>();
		KPrefabID kprefabID = component.KPrefabID;
		HashSet<Pickupable> hashSet;
		if (this.Inventory.TryGetValue(kprefabID.PrefabTag, out hashSet))
		{
			hashSet.Remove(component);
		}
		foreach (Tag key in kprefabID.Tags)
		{
			if (this.Inventory.TryGetValue(key, out hashSet))
			{
				hashSet.Remove(component);
			}
		}
	}

	// Token: 0x06005E18 RID: 24088 RVA: 0x00220DE0 File Offset: 0x0021EFE0
	public Dictionary<Tag, float> GetAccessibleAmounts()
	{
		return this.accessibleAmounts;
	}

	// Token: 0x04003E82 RID: 16002
	private WorldContainer m_worldContainer;

	// Token: 0x04003E83 RID: 16003
	[Serialize]
	public List<Tag> pinnedResources = new List<Tag>();

	// Token: 0x04003E84 RID: 16004
	[Serialize]
	public List<Tag> notifyResources = new List<Tag>();

	// Token: 0x04003E85 RID: 16005
	private Dictionary<Tag, HashSet<Pickupable>> Inventory = new Dictionary<Tag, HashSet<Pickupable>>();

	// Token: 0x04003E86 RID: 16006
	private Dictionary<Tag, float> accessibleAmounts = new Dictionary<Tag, float>();

	// Token: 0x04003E87 RID: 16007
	private bool hasValidCount;

	// Token: 0x04003E88 RID: 16008
	private static readonly EventSystem.IntraObjectHandler<WorldInventory> OnNewDayDelegate = new EventSystem.IntraObjectHandler<WorldInventory>(delegate(WorldInventory component, object data)
	{
		component.GenerateInventoryReport(data);
	});

	// Token: 0x04003E89 RID: 16009
	private int accessibleUpdateIndex;

	// Token: 0x04003E8A RID: 16010
	private bool firstUpdate = true;

	// Token: 0x04003E8B RID: 16011
	private static Tag[] NonCritterEntitiesTags = new Tag[]
	{
		GameTags.DupeBrain,
		GameTags.Robot
	};
}
