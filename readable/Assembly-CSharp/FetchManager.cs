using System;
using System.Collections.Generic;
using System.Diagnostics;
using FoodRehydrator;
using UnityEngine;

// Token: 0x02000954 RID: 2388
[AddComponentMenu("KMonoBehaviour/scripts/FetchManager")]
public class FetchManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x060042A0 RID: 17056 RVA: 0x00178B7B File Offset: 0x00176D7B
	private static int QuantizeRotValue(float rot_value)
	{
		return (int)(4f * rot_value);
	}

	// Token: 0x060042A1 RID: 17057 RVA: 0x00178B88 File Offset: 0x00176D88
	public HandleVector<int>.Handle Add(Pickupable pickupable)
	{
		Tag tag = pickupable.KPrefabID.PrefabID();
		FetchManager.FetchablesByPrefabId fetchablesByPrefabId = null;
		if (!this.prefabIdToFetchables.TryGetValue(tag, out fetchablesByPrefabId))
		{
			fetchablesByPrefabId = new FetchManager.FetchablesByPrefabId(tag);
			this.prefabIdToFetchables[tag] = fetchablesByPrefabId;
		}
		return fetchablesByPrefabId.AddPickupable(pickupable);
	}

	// Token: 0x060042A2 RID: 17058 RVA: 0x00178BD0 File Offset: 0x00176DD0
	public void Remove(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle)
	{
		FetchManager.FetchablesByPrefabId fetchablesByPrefabId;
		if (this.prefabIdToFetchables.TryGetValue(prefab_tag, out fetchablesByPrefabId))
		{
			fetchablesByPrefabId.RemovePickupable(fetchable_handle);
		}
	}

	// Token: 0x060042A3 RID: 17059 RVA: 0x00178BF4 File Offset: 0x00176DF4
	public void UpdateStorage(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle, Storage storage)
	{
		FetchManager.FetchablesByPrefabId fetchablesByPrefabId;
		if (this.prefabIdToFetchables.TryGetValue(prefab_tag, out fetchablesByPrefabId))
		{
			fetchablesByPrefabId.UpdateStorage(fetchable_handle, storage);
		}
	}

	// Token: 0x060042A4 RID: 17060 RVA: 0x00178C19 File Offset: 0x00176E19
	public void UpdateTags(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle)
	{
		this.prefabIdToFetchables[prefab_tag].UpdateTags(fetchable_handle);
	}

	// Token: 0x060042A5 RID: 17061 RVA: 0x00178C30 File Offset: 0x00176E30
	public void Sim1000ms(float dt)
	{
		foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> keyValuePair in this.prefabIdToFetchables)
		{
			keyValuePair.Value.Sim1000ms(dt);
		}
	}

	// Token: 0x060042A6 RID: 17062 RVA: 0x00178C8C File Offset: 0x00176E8C
	public void UpdatePickups(Navigator navigator, WorkerBase worker)
	{
		this.updateOffsetTables.Reset(null);
		this.updatePickupsWorkItems.Reset(null);
		foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> keyValuePair in this.prefabIdToFetchables)
		{
			FetchManager.FetchablesByPrefabId value = keyValuePair.Value;
			this.updateOffsetTables.Add(new FetchManager.UpdateOffsetTables(value));
			this.updatePickupsWorkItems.Add(new FetchManager.UpdatePickupWorkItem
			{
				fetchablesByPrefabId = value,
				navigator = navigator,
				worker = worker.GetComponent<KPrefabID>().InstanceID
			});
		}
		GlobalJobManager.Run(this.updateOffsetTables);
		for (int i = 0; i < this.updateOffsetTables.Count; i++)
		{
			this.updateOffsetTables.GetWorkItem(i).Finish();
		}
		OffsetTracker.isExecutingWithinJob = true;
		GlobalJobManager.Run(this.updatePickupsWorkItems);
		OffsetTracker.isExecutingWithinJob = false;
		this.pickups.Clear();
		foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> keyValuePair2 in this.prefabIdToFetchables)
		{
			this.pickups.AddRange(keyValuePair2.Value.finalPickups);
		}
		this.pickups.Sort(FetchManager.PickupComparerNoPriority.CompareInst);
	}

	// Token: 0x060042A7 RID: 17063 RVA: 0x00178E00 File Offset: 0x00177000
	public static bool IsFetchablePickup(Pickupable pickup, FetchChore chore, Storage destination)
	{
		KPrefabID kprefabID = pickup.KPrefabID;
		Storage storage = pickup.storage;
		if (pickup.UnreservedFetchAmount <= 0f)
		{
			return false;
		}
		if (pickup.PrimaryElement.MassPerUnit > 1f && pickup.PrimaryElement.MassPerUnit > chore.originalAmount)
		{
			return false;
		}
		if (kprefabID == null)
		{
			return false;
		}
		if (!pickup.isChoreAllowedToPickup(chore.choreType))
		{
			return false;
		}
		if (chore.criteria == FetchChore.MatchCriteria.MatchID && !chore.tags.Contains(kprefabID.PrefabTag))
		{
			return false;
		}
		if (chore.criteria == FetchChore.MatchCriteria.MatchTags && !kprefabID.HasTag(chore.tagsFirst))
		{
			return false;
		}
		if (chore.requiredTag.IsValid && !kprefabID.HasTag(chore.requiredTag))
		{
			return false;
		}
		if (kprefabID.HasAnyTags(chore.forbiddenTags))
		{
			return false;
		}
		if (kprefabID.HasTag(GameTags.MarkedForMove))
		{
			return false;
		}
		if (storage != null)
		{
			if (!storage.ignoreSourcePriority && destination.ShouldOnlyTransferFromLowerPriority && destination.masterPriority <= storage.masterPriority)
			{
				return false;
			}
			if (destination.storageNetworkID != -1 && destination.storageNetworkID == storage.storageNetworkID)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060042A8 RID: 17064 RVA: 0x00178F28 File Offset: 0x00177128
	public static Pickupable FindFetchTarget(List<Pickupable> pickupables, Storage destination, FetchChore chore)
	{
		foreach (Pickupable pickupable in pickupables)
		{
			if (FetchManager.IsFetchablePickup(pickupable, chore, destination))
			{
				return pickupable;
			}
		}
		return null;
	}

	// Token: 0x060042A9 RID: 17065 RVA: 0x00178F80 File Offset: 0x00177180
	public Pickupable FindFetchTarget(Storage destination, FetchChore chore)
	{
		foreach (FetchManager.Pickup pickup in this.pickups)
		{
			if (FetchManager.IsFetchablePickup(pickup.pickupable, chore, destination))
			{
				return pickup.pickupable;
			}
		}
		return null;
	}

	// Token: 0x060042AA RID: 17066 RVA: 0x00178FE8 File Offset: 0x001771E8
	public static bool IsFetchablePickup_Exclude(KPrefabID pickup_id, Storage source, float pickup_unreserved_amount, HashSet<Tag> exclude_tags, Tag required_tag, Storage destination)
	{
		return FetchManager.IsFetchablePickup_Exclude(pickup_id, source, pickup_unreserved_amount, exclude_tags, new Tag[]
		{
			required_tag
		}, destination);
	}

	// Token: 0x060042AB RID: 17067 RVA: 0x00179004 File Offset: 0x00177204
	public static bool IsFetchablePickup_Exclude(KPrefabID pickup_id, Storage source, float pickup_unreserved_amount, HashSet<Tag> exclude_tags, Tag[] required_tags, Storage destination)
	{
		if (pickup_unreserved_amount <= 0f)
		{
			return false;
		}
		if (pickup_id == null)
		{
			return false;
		}
		if (exclude_tags.Contains(pickup_id.PrefabTag))
		{
			return false;
		}
		if (!pickup_id.HasAllTags(required_tags))
		{
			return false;
		}
		if (source != null)
		{
			if (!source.ignoreSourcePriority && destination.ShouldOnlyTransferFromLowerPriority && destination.masterPriority <= source.masterPriority)
			{
				return false;
			}
			if (destination.storageNetworkID != -1 && destination.storageNetworkID == source.storageNetworkID)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060042AC RID: 17068 RVA: 0x0017908E File Offset: 0x0017728E
	public Pickupable FindEdibleFetchTarget(Storage destination, HashSet<Tag> exclude_tags, Tag required_tag)
	{
		return this.FindEdibleFetchTarget(destination, exclude_tags, new Tag[]
		{
			required_tag
		});
	}

	// Token: 0x060042AD RID: 17069 RVA: 0x001790A8 File Offset: 0x001772A8
	public Pickupable FindEdibleFetchTarget(Storage destination, HashSet<Tag> exclude_tags, Tag[] required_tags)
	{
		FetchManager.Pickup pickup = new FetchManager.Pickup
		{
			PathCost = ushort.MaxValue,
			foodQuality = int.MinValue
		};
		int num = int.MaxValue;
		foreach (FetchManager.Pickup pickup2 in this.pickups)
		{
			Pickupable pickupable = pickup2.pickupable;
			if (FetchManager.IsFetchablePickup_Exclude(pickupable.KPrefabID, pickupable.storage, pickupable.UnreservedFetchAmount, exclude_tags, required_tags, destination))
			{
				int num2 = (int)pickup2.PathCost + (5 - pickup2.foodQuality) * 50;
				if (num2 < num)
				{
					pickup = pickup2;
					num = num2;
				}
			}
		}
		Navigator component = destination.GetComponent<Navigator>();
		if (component != null)
		{
			foreach (object obj in Components.FoodRehydrators)
			{
				GameObject gameObject = (GameObject)obj;
				int cell = Grid.PosToCell(gameObject);
				int navigationCost = component.GetNavigationCost(cell);
				if (navigationCost != -1 && num > navigationCost + 50 + 5)
				{
					AccessabilityManager accessabilityManager = (gameObject != null) ? gameObject.GetComponent<AccessabilityManager>() : null;
					if (accessabilityManager != null && accessabilityManager.CanAccess(destination.gameObject))
					{
						foreach (GameObject gameObject2 in gameObject.GetComponent<Storage>().items)
						{
							Storage storage = (gameObject2 != null) ? gameObject2.GetComponent<Storage>() : null;
							if (storage != null && !storage.IsEmpty())
							{
								Edible component2 = storage.items[0].GetComponent<Edible>();
								Pickupable component3 = component2.GetComponent<Pickupable>();
								if (FetchManager.IsFetchablePickup_Exclude(component3.KPrefabID, component3.storage, component3.UnreservedFetchAmount, exclude_tags, required_tags, destination))
								{
									int num3 = navigationCost + (5 - component2.FoodInfo.Quality + 1) * 50 + 5;
									if (num3 < num)
									{
										pickup.pickupable = component3;
										pickup.foodQuality = component2.FoodInfo.Quality;
										pickup.tagBitsHash = component2.PrefabID().GetHashCode();
										num = num3;
									}
								}
							}
						}
					}
				}
			}
		}
		return pickup.pickupable;
	}

	// Token: 0x040029EF RID: 10735
	private List<FetchManager.Pickup> pickups = new List<FetchManager.Pickup>();

	// Token: 0x040029F0 RID: 10736
	public Dictionary<Tag, FetchManager.FetchablesByPrefabId> prefabIdToFetchables = new Dictionary<Tag, FetchManager.FetchablesByPrefabId>();

	// Token: 0x040029F1 RID: 10737
	private WorkItemCollection<FetchManager.UpdateOffsetTables, object> updateOffsetTables = new WorkItemCollection<FetchManager.UpdateOffsetTables, object>();

	// Token: 0x040029F2 RID: 10738
	private WorkItemCollection<FetchManager.UpdatePickupWorkItem, object> updatePickupsWorkItems = new WorkItemCollection<FetchManager.UpdatePickupWorkItem, object>();

	// Token: 0x0200193F RID: 6463
	public struct Fetchable
	{
		// Token: 0x04007D45 RID: 32069
		public Pickupable pickupable;

		// Token: 0x04007D46 RID: 32070
		public int tagBitsHash;

		// Token: 0x04007D47 RID: 32071
		public int masterPriority;

		// Token: 0x04007D48 RID: 32072
		public int freshness;

		// Token: 0x04007D49 RID: 32073
		public int foodQuality;
	}

	// Token: 0x02001940 RID: 6464
	[DebuggerDisplay("{pickupable.name}")]
	public struct Pickup
	{
		// Token: 0x04007D4A RID: 32074
		public Pickupable pickupable;

		// Token: 0x04007D4B RID: 32075
		public int tagBitsHash;

		// Token: 0x04007D4C RID: 32076
		public ushort PathCost;

		// Token: 0x04007D4D RID: 32077
		public int masterPriority;

		// Token: 0x04007D4E RID: 32078
		public int freshness;

		// Token: 0x04007D4F RID: 32079
		public int foodQuality;
	}

	// Token: 0x02001941 RID: 6465
	private static class PickupComparerIncludingPriority
	{
		// Token: 0x0600A1B7 RID: 41399 RVA: 0x003AC5F0 File Offset: 0x003AA7F0
		private static int Compare(FetchManager.Pickup a, FetchManager.Pickup b)
		{
			int num = a.tagBitsHash.CompareTo(b.tagBitsHash);
			if (num != 0)
			{
				return num;
			}
			num = b.masterPriority.CompareTo(a.masterPriority);
			if (num != 0)
			{
				return num;
			}
			num = a.PathCost.CompareTo(b.PathCost);
			if (num != 0)
			{
				return num;
			}
			num = b.foodQuality.CompareTo(a.foodQuality);
			if (num != 0)
			{
				return num;
			}
			return b.freshness.CompareTo(a.freshness);
		}

		// Token: 0x04007D50 RID: 32080
		public static Comparison<FetchManager.Pickup> CompareInst = new Comparison<FetchManager.Pickup>(FetchManager.PickupComparerIncludingPriority.Compare);
	}

	// Token: 0x02001942 RID: 6466
	private static class PickupComparerNoPriority
	{
		// Token: 0x0600A1B9 RID: 41401 RVA: 0x003AC684 File Offset: 0x003AA884
		public static int Compare(FetchManager.Pickup a, FetchManager.Pickup b)
		{
			int num = a.PathCost.CompareTo(b.PathCost);
			if (num != 0)
			{
				return num;
			}
			num = b.foodQuality.CompareTo(a.foodQuality);
			if (num != 0)
			{
				return num;
			}
			return b.freshness.CompareTo(a.freshness);
		}

		// Token: 0x04007D51 RID: 32081
		public static Comparison<FetchManager.Pickup> CompareInst = new Comparison<FetchManager.Pickup>(FetchManager.PickupComparerNoPriority.Compare);
	}

	// Token: 0x02001943 RID: 6467
	public class FetchablesByPrefabId
	{
		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x0600A1BB RID: 41403 RVA: 0x003AC6E8 File Offset: 0x003AA8E8
		// (set) Token: 0x0600A1BC RID: 41404 RVA: 0x003AC6F0 File Offset: 0x003AA8F0
		public Tag prefabId { get; private set; }

		// Token: 0x0600A1BD RID: 41405 RVA: 0x003AC6FC File Offset: 0x003AA8FC
		public FetchablesByPrefabId(Tag prefab_id)
		{
			this.prefabId = prefab_id;
			this.fetchables = new KCompactedVector<FetchManager.Fetchable>(0);
			this.rotUpdaters = new Dictionary<HandleVector<int>.Handle, Rottable.Instance>();
			this.finalPickups = new List<FetchManager.Pickup>();
		}

		// Token: 0x0600A1BE RID: 41406 RVA: 0x003AC75C File Offset: 0x003AA95C
		public HandleVector<int>.Handle AddPickupable(Pickupable pickupable)
		{
			int foodQuality = 5;
			Edible component = pickupable.GetComponent<Edible>();
			if (component != null)
			{
				foodQuality = component.GetQuality();
			}
			int masterPriority = 0;
			if (pickupable.storage != null)
			{
				Prioritizable prioritizable = pickupable.storage.prioritizable;
				if (prioritizable != null)
				{
					masterPriority = prioritizable.GetMasterPriority().priority_value;
				}
			}
			Rottable.Instance smi = pickupable.GetSMI<Rottable.Instance>();
			int freshness = 0;
			if (!smi.IsNullOrStopped())
			{
				freshness = FetchManager.QuantizeRotValue(smi.RotValue);
			}
			KPrefabID kprefabID = pickupable.KPrefabID;
			HandleVector<int>.Handle handle = this.fetchables.Allocate(new FetchManager.Fetchable
			{
				pickupable = pickupable,
				foodQuality = foodQuality,
				freshness = freshness,
				masterPriority = masterPriority,
				tagBitsHash = kprefabID.GetTagsHash()
			});
			if (!smi.IsNullOrStopped())
			{
				this.rotUpdaters[handle] = smi;
			}
			return handle;
		}

		// Token: 0x0600A1BF RID: 41407 RVA: 0x003AC83F File Offset: 0x003AAA3F
		public void RemovePickupable(HandleVector<int>.Handle fetchable_handle)
		{
			this.fetchables.Free(fetchable_handle);
			this.rotUpdaters.Remove(fetchable_handle);
		}

		// Token: 0x0600A1C0 RID: 41408 RVA: 0x003AC85C File Offset: 0x003AAA5C
		public void UpdatePickups(Navigator worker_navigator, int worker)
		{
			this.GatherPickupablesWhichCanBePickedUp(worker);
			this.GatherReachablePickups(worker_navigator);
			this.finalPickups.Sort(FetchManager.PickupComparerIncludingPriority.CompareInst);
			if (this.finalPickups.Count > 0)
			{
				FetchManager.Pickup pickup = this.finalPickups[0];
				int num = pickup.tagBitsHash;
				int num2 = this.finalPickups.Count;
				int num3 = 0;
				for (int i = 1; i < this.finalPickups.Count; i++)
				{
					bool flag = false;
					FetchManager.Pickup pickup2 = this.finalPickups[i];
					int tagBitsHash = pickup2.tagBitsHash;
					if (pickup.masterPriority == pickup2.masterPriority && tagBitsHash == num)
					{
						flag = true;
					}
					if (flag)
					{
						num2--;
					}
					else
					{
						num3++;
						pickup = pickup2;
						num = tagBitsHash;
						if (i > num3)
						{
							this.finalPickups[num3] = pickup2;
						}
					}
				}
				this.finalPickups.RemoveRange(num2, this.finalPickups.Count - num2);
			}
		}

		// Token: 0x0600A1C1 RID: 41409 RVA: 0x003AC948 File Offset: 0x003AAB48
		private void GatherPickupablesWhichCanBePickedUp(int worker)
		{
			this.pickupsWhichCanBePickedUp.Clear();
			foreach (FetchManager.Fetchable fetchable in this.fetchables.GetDataList())
			{
				Pickupable pickupable = fetchable.pickupable;
				if (pickupable.CouldBePickedUpByMinion(worker))
				{
					this.pickupsWhichCanBePickedUp.Add(new FetchManager.Pickup
					{
						pickupable = pickupable,
						tagBitsHash = fetchable.tagBitsHash,
						PathCost = ushort.MaxValue,
						masterPriority = fetchable.masterPriority,
						freshness = fetchable.freshness,
						foodQuality = fetchable.foodQuality
					});
				}
			}
		}

		// Token: 0x0600A1C2 RID: 41410 RVA: 0x003ACA10 File Offset: 0x003AAC10
		public void UpdateOffsetTables()
		{
			foreach (FetchManager.Fetchable fetchable in this.fetchables.GetDataList())
			{
				fetchable.pickupable.GetOffsets(fetchable.pickupable.cachedCell);
			}
		}

		// Token: 0x0600A1C3 RID: 41411 RVA: 0x003ACA78 File Offset: 0x003AAC78
		private void GatherReachablePickups(Navigator navigator)
		{
			this.cellCosts.Clear();
			this.finalPickups.Clear();
			foreach (FetchManager.Pickup pickup in this.pickupsWhichCanBePickedUp)
			{
				Pickupable pickupable = pickup.pickupable;
				int num = -1;
				if (!this.cellCosts.TryGetValue(pickupable.cachedCell, out num))
				{
					num = pickupable.GetNavigationCost(navigator, pickupable.cachedCell);
					this.cellCosts[pickupable.cachedCell] = num;
				}
				if (num != -1)
				{
					this.finalPickups.Add(new FetchManager.Pickup
					{
						pickupable = pickupable,
						tagBitsHash = pickup.tagBitsHash,
						PathCost = (ushort)num,
						masterPriority = pickup.masterPriority,
						freshness = pickup.freshness,
						foodQuality = pickup.foodQuality
					});
				}
			}
		}

		// Token: 0x0600A1C4 RID: 41412 RVA: 0x003ACB7C File Offset: 0x003AAD7C
		public void UpdateStorage(HandleVector<int>.Handle fetchable_handle, Storage storage)
		{
			FetchManager.Fetchable data = this.fetchables.GetData(fetchable_handle);
			int masterPriority = 0;
			Pickupable pickupable = data.pickupable;
			if (pickupable.storage != null)
			{
				Prioritizable prioritizable = pickupable.storage.prioritizable;
				if (prioritizable != null)
				{
					masterPriority = prioritizable.GetMasterPriority().priority_value;
				}
			}
			data.masterPriority = masterPriority;
			this.fetchables.SetData(fetchable_handle, data);
		}

		// Token: 0x0600A1C5 RID: 41413 RVA: 0x003ACBE8 File Offset: 0x003AADE8
		public void UpdateTags(HandleVector<int>.Handle fetchable_handle)
		{
			FetchManager.Fetchable data = this.fetchables.GetData(fetchable_handle);
			data.tagBitsHash = data.pickupable.KPrefabID.GetTagsHash();
			this.fetchables.SetData(fetchable_handle, data);
		}

		// Token: 0x0600A1C6 RID: 41414 RVA: 0x003ACC28 File Offset: 0x003AAE28
		public void Sim1000ms(float dt)
		{
			foreach (KeyValuePair<HandleVector<int>.Handle, Rottable.Instance> keyValuePair in this.rotUpdaters)
			{
				HandleVector<int>.Handle key = keyValuePair.Key;
				Rottable.Instance value = keyValuePair.Value;
				FetchManager.Fetchable data = this.fetchables.GetData(key);
				data.freshness = FetchManager.QuantizeRotValue(value.RotValue);
				this.fetchables.SetData(key, data);
			}
		}

		// Token: 0x04007D52 RID: 32082
		public KCompactedVector<FetchManager.Fetchable> fetchables;

		// Token: 0x04007D53 RID: 32083
		public List<FetchManager.Pickup> finalPickups = new List<FetchManager.Pickup>();

		// Token: 0x04007D54 RID: 32084
		private Dictionary<HandleVector<int>.Handle, Rottable.Instance> rotUpdaters;

		// Token: 0x04007D55 RID: 32085
		private List<FetchManager.Pickup> pickupsWhichCanBePickedUp = new List<FetchManager.Pickup>();

		// Token: 0x04007D56 RID: 32086
		private Dictionary<int, int> cellCosts = new Dictionary<int, int>();
	}

	// Token: 0x02001944 RID: 6468
	private struct UpdateOffsetTables : IWorkItem<object>
	{
		// Token: 0x0600A1C7 RID: 41415 RVA: 0x003ACCB4 File Offset: 0x003AAEB4
		public UpdateOffsetTables(FetchManager.FetchablesByPrefabId fetchables)
		{
			this.data = fetchables;
			this.failed = ListPool<Pickupable, FetchManager.UpdateOffsetTables>.Allocate();
		}

		// Token: 0x0600A1C8 RID: 41416 RVA: 0x003ACCC8 File Offset: 0x003AAEC8
		public void Run(object _, int threadIndex)
		{
			if (Game.IsOnMainThread())
			{
				this.data.UpdateOffsetTables();
				return;
			}
			foreach (FetchManager.Fetchable fetchable in this.data.fetchables.GetDataList())
			{
				if (!fetchable.pickupable.ValidateOffsets(fetchable.pickupable.cachedCell))
				{
					this.failed.Add(fetchable.pickupable);
				}
			}
		}

		// Token: 0x0600A1C9 RID: 41417 RVA: 0x003ACD5C File Offset: 0x003AAF5C
		public void Finish()
		{
			foreach (Pickupable pickupable in this.failed)
			{
				pickupable.GetOffsets(pickupable.cachedCell);
			}
			this.failed.Recycle();
		}

		// Token: 0x04007D58 RID: 32088
		public FetchManager.FetchablesByPrefabId data;

		// Token: 0x04007D59 RID: 32089
		private ListPool<Pickupable, FetchManager.UpdateOffsetTables>.PooledList failed;
	}

	// Token: 0x02001945 RID: 6469
	private struct UpdatePickupWorkItem : IWorkItem<object>
	{
		// Token: 0x0600A1CA RID: 41418 RVA: 0x003ACDC0 File Offset: 0x003AAFC0
		public void Run(object shared_data, int threadIndex)
		{
			this.fetchablesByPrefabId.UpdatePickups(this.navigator, this.worker);
		}

		// Token: 0x04007D5A RID: 32090
		public FetchManager.FetchablesByPrefabId fetchablesByPrefabId;

		// Token: 0x04007D5B RID: 32091
		public Navigator navigator;

		// Token: 0x04007D5C RID: 32092
		public int worker;
	}
}
