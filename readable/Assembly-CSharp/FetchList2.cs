using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000952 RID: 2386
public class FetchList2 : IFetchList
{
	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x06004282 RID: 17026 RVA: 0x00177CAC File Offset: 0x00175EAC
	// (set) Token: 0x06004283 RID: 17027 RVA: 0x00177CB4 File Offset: 0x00175EB4
	public bool ShowStatusItem
	{
		get
		{
			return this.bShowStatusItem;
		}
		set
		{
			this.bShowStatusItem = value;
		}
	}

	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x06004284 RID: 17028 RVA: 0x00177CBD File Offset: 0x00175EBD
	public bool IsComplete
	{
		get
		{
			return this.FetchOrders.Count == 0;
		}
	}

	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x06004285 RID: 17029 RVA: 0x00177CD0 File Offset: 0x00175ED0
	public bool InProgress
	{
		get
		{
			if (this.FetchOrders.Count < 0)
			{
				return false;
			}
			bool result = false;
			using (List<FetchOrder2>.Enumerator enumerator = this.FetchOrders.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.InProgress)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
	}

	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x06004286 RID: 17030 RVA: 0x00177D3C File Offset: 0x00175F3C
	// (set) Token: 0x06004287 RID: 17031 RVA: 0x00177D44 File Offset: 0x00175F44
	public Storage Destination { get; private set; }

	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x06004288 RID: 17032 RVA: 0x00177D4D File Offset: 0x00175F4D
	// (set) Token: 0x06004289 RID: 17033 RVA: 0x00177D55 File Offset: 0x00175F55
	public int PriorityMod { get; private set; }

	// Token: 0x0600428A RID: 17034 RVA: 0x00177D60 File Offset: 0x00175F60
	public FetchList2(Storage destination, ChoreType chore_type)
	{
		this.Destination = destination;
		this.choreType = chore_type;
	}

	// Token: 0x0600428B RID: 17035 RVA: 0x00177DCC File Offset: 0x00175FCC
	public void SetPriorityMod(int priorityMod)
	{
		this.PriorityMod = priorityMod;
		for (int i = 0; i < this.FetchOrders.Count; i++)
		{
			this.FetchOrders[i].SetPriorityMod(this.PriorityMod);
		}
	}

	// Token: 0x0600428C RID: 17036 RVA: 0x00177E10 File Offset: 0x00176010
	public void Add(HashSet<Tag> tags, Tag requiredTag, Tag[] forbidden_tags = null, float amount = 1f, Operational.State operationalRequirementDEPRECATED = Operational.State.None)
	{
		foreach (Tag key in tags)
		{
			if (!this.MinimumAmount.ContainsKey(key))
			{
				this.MinimumAmount[key] = amount;
			}
		}
		FetchOrder2 item = new FetchOrder2(this.choreType, tags, FetchChore.MatchCriteria.MatchID, requiredTag, forbidden_tags, this.Destination, amount, operationalRequirementDEPRECATED, this.PriorityMod);
		this.FetchOrders.Add(item);
	}

	// Token: 0x0600428D RID: 17037 RVA: 0x00177EA0 File Offset: 0x001760A0
	public void Add(HashSet<Tag> tags, Tag[] forbidden_tags = null, float amount = 1f, Operational.State operationalRequirementDEPRECATED = Operational.State.None)
	{
		foreach (Tag key in tags)
		{
			if (!this.MinimumAmount.ContainsKey(key))
			{
				this.MinimumAmount[key] = amount;
			}
		}
		FetchOrder2 item = new FetchOrder2(this.choreType, tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, forbidden_tags, this.Destination, amount, operationalRequirementDEPRECATED, this.PriorityMod);
		this.FetchOrders.Add(item);
	}

	// Token: 0x0600428E RID: 17038 RVA: 0x00177F34 File Offset: 0x00176134
	public void Add(Tag tag, Tag[] forbidden_tags = null, float amount = 1f, Operational.State operationalRequirementDEPRECATED = Operational.State.None)
	{
		amount = FetchChore.GetMinimumFetchAmount(tag, amount);
		if (!this.MinimumAmount.ContainsKey(tag))
		{
			this.MinimumAmount[tag] = amount;
		}
		FetchOrder2 item = new FetchOrder2(this.choreType, new HashSet<Tag>
		{
			tag
		}, FetchChore.MatchCriteria.MatchTags, Tag.Invalid, forbidden_tags, this.Destination, amount, operationalRequirementDEPRECATED, this.PriorityMod);
		this.FetchOrders.Add(item);
	}

	// Token: 0x0600428F RID: 17039 RVA: 0x00177FA0 File Offset: 0x001761A0
	public float GetMinimumAmount(Tag tag)
	{
		float result = 0f;
		this.MinimumAmount.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x06004290 RID: 17040 RVA: 0x00177FC3 File Offset: 0x001761C3
	private void OnFetchOrderComplete(FetchOrder2 fetch_order, Pickupable fetched_item)
	{
		this.FetchOrders.Remove(fetch_order);
		if (this.FetchOrders.Count == 0)
		{
			if (this.OnComplete != null)
			{
				this.OnComplete();
			}
			FetchListStatusItemUpdater.instance.RemoveFetchList(this);
			this.ClearStatus();
		}
	}

	// Token: 0x06004291 RID: 17041 RVA: 0x00178004 File Offset: 0x00176204
	public void Cancel(string reason)
	{
		FetchListStatusItemUpdater.instance.RemoveFetchList(this);
		this.ClearStatus();
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			fetchOrder.Cancel(reason);
		}
	}

	// Token: 0x06004292 RID: 17042 RVA: 0x00178068 File Offset: 0x00176268
	public void UpdateRemaining()
	{
		this.Remaining.Clear();
		for (int i = 0; i < this.FetchOrders.Count; i++)
		{
			FetchOrder2 fetchOrder = this.FetchOrders[i];
			foreach (Tag key in fetchOrder.Tags)
			{
				float num = 0f;
				this.Remaining.TryGetValue(key, out num);
				this.Remaining[key] = num + fetchOrder.AmountWaitingToFetch();
			}
		}
	}

	// Token: 0x06004293 RID: 17043 RVA: 0x00178110 File Offset: 0x00176310
	public Dictionary<Tag, float> GetRemaining()
	{
		return this.Remaining;
	}

	// Token: 0x06004294 RID: 17044 RVA: 0x00178118 File Offset: 0x00176318
	public Dictionary<Tag, float> GetRemainingMinimum()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			foreach (Tag key in fetchOrder.Tags)
			{
				dictionary[key] = this.MinimumAmount[key];
			}
		}
		foreach (GameObject gameObject in this.Destination.items)
		{
			if (gameObject != null)
			{
				Pickupable component = gameObject.GetComponent<Pickupable>();
				if (component != null)
				{
					KPrefabID kprefabID = component.KPrefabID;
					if (dictionary.ContainsKey(kprefabID.PrefabTag))
					{
						dictionary[kprefabID.PrefabTag] = Math.Max(dictionary[kprefabID.PrefabTag] - component.FetchTotalAmount, 0f);
					}
					foreach (Tag key2 in kprefabID.Tags)
					{
						if (dictionary.ContainsKey(key2))
						{
							dictionary[key2] = Math.Max(dictionary[key2] - component.FetchTotalAmount, 0f);
						}
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06004295 RID: 17045 RVA: 0x001782D0 File Offset: 0x001764D0
	public void Suspend(string reason)
	{
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			fetchOrder.Suspend(reason);
		}
	}

	// Token: 0x06004296 RID: 17046 RVA: 0x00178324 File Offset: 0x00176524
	public void Resume(string reason)
	{
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			fetchOrder.Resume(reason);
		}
	}

	// Token: 0x06004297 RID: 17047 RVA: 0x00178378 File Offset: 0x00176578
	public void Submit(System.Action on_complete, bool check_storage_contents)
	{
		this.OnComplete = on_complete;
		foreach (FetchOrder2 fetchOrder in this.FetchOrders.GetRange(0, this.FetchOrders.Count))
		{
			fetchOrder.Submit(new Action<FetchOrder2, Pickupable>(this.OnFetchOrderComplete), check_storage_contents, null);
		}
		if (!this.IsComplete && this.ShowStatusItem)
		{
			FetchListStatusItemUpdater.instance.AddFetchList(this);
		}
	}

	// Token: 0x06004298 RID: 17048 RVA: 0x0017840C File Offset: 0x0017660C
	private void ClearStatus()
	{
		if (this.Destination != null)
		{
			KSelectable component = this.Destination.GetComponent<KSelectable>();
			if (component != null)
			{
				this.waitingForMaterialsHandle = component.RemoveStatusItem(this.waitingForMaterialsHandle, false);
				this.materialsUnavailableHandle = component.RemoveStatusItem(this.materialsUnavailableHandle, false);
				this.materialsUnavailableForRefillHandle = component.RemoveStatusItem(this.materialsUnavailableForRefillHandle, false);
			}
		}
	}

	// Token: 0x06004299 RID: 17049 RVA: 0x00178478 File Offset: 0x00176678
	public void UpdateStatusItem(MaterialsStatusItem status_item, ref Guid handle, bool should_add)
	{
		bool flag = handle != Guid.Empty;
		if (should_add != flag)
		{
			if (should_add)
			{
				KSelectable component = this.Destination.GetComponent<KSelectable>();
				if (component != null)
				{
					handle = component.AddStatusItem(status_item, this);
					GameScheduler.Instance.Schedule("Digging Tutorial", 2f, delegate(object obj)
					{
						Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Digging, true);
					}, null, null);
					return;
				}
			}
			else
			{
				KSelectable component2 = this.Destination.GetComponent<KSelectable>();
				if (component2 != null)
				{
					handle = component2.RemoveStatusItem(handle, false);
				}
			}
		}
	}

	// Token: 0x040029E0 RID: 10720
	private System.Action OnComplete;

	// Token: 0x040029E3 RID: 10723
	private ChoreType choreType;

	// Token: 0x040029E4 RID: 10724
	public Guid waitingForMaterialsHandle = Guid.Empty;

	// Token: 0x040029E5 RID: 10725
	public Guid materialsUnavailableForRefillHandle = Guid.Empty;

	// Token: 0x040029E6 RID: 10726
	public Guid materialsUnavailableHandle = Guid.Empty;

	// Token: 0x040029E7 RID: 10727
	public Dictionary<Tag, float> MinimumAmount = new Dictionary<Tag, float>();

	// Token: 0x040029E8 RID: 10728
	public List<FetchOrder2> FetchOrders = new List<FetchOrder2>();

	// Token: 0x040029E9 RID: 10729
	private Dictionary<Tag, float> Remaining = new Dictionary<Tag, float>();

	// Token: 0x040029EA RID: 10730
	private bool bShowStatusItem = true;
}
