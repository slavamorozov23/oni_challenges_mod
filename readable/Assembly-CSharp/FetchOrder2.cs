using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000955 RID: 2389
public class FetchOrder2
{
	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x060042AF RID: 17071 RVA: 0x00179394 File Offset: 0x00177594
	// (set) Token: 0x060042B0 RID: 17072 RVA: 0x0017939C File Offset: 0x0017759C
	public float TotalAmount { get; set; }

	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x060042B1 RID: 17073 RVA: 0x001793A5 File Offset: 0x001775A5
	// (set) Token: 0x060042B2 RID: 17074 RVA: 0x001793AD File Offset: 0x001775AD
	public int PriorityMod { get; set; }

	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x060042B3 RID: 17075 RVA: 0x001793B6 File Offset: 0x001775B6
	// (set) Token: 0x060042B4 RID: 17076 RVA: 0x001793BE File Offset: 0x001775BE
	public HashSet<Tag> Tags { get; protected set; }

	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x060042B5 RID: 17077 RVA: 0x001793C7 File Offset: 0x001775C7
	// (set) Token: 0x060042B6 RID: 17078 RVA: 0x001793CF File Offset: 0x001775CF
	public FetchChore.MatchCriteria Criteria { get; protected set; }

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x060042B7 RID: 17079 RVA: 0x001793D8 File Offset: 0x001775D8
	// (set) Token: 0x060042B8 RID: 17080 RVA: 0x001793E0 File Offset: 0x001775E0
	public Tag RequiredTag { get; protected set; }

	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x060042B9 RID: 17081 RVA: 0x001793E9 File Offset: 0x001775E9
	// (set) Token: 0x060042BA RID: 17082 RVA: 0x001793F1 File Offset: 0x001775F1
	public Tag[] ForbiddenTags { get; protected set; }

	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x060042BB RID: 17083 RVA: 0x001793FA File Offset: 0x001775FA
	// (set) Token: 0x060042BC RID: 17084 RVA: 0x00179402 File Offset: 0x00177602
	public Storage Destination { get; set; }

	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x060042BD RID: 17085 RVA: 0x0017940B File Offset: 0x0017760B
	// (set) Token: 0x060042BE RID: 17086 RVA: 0x00179413 File Offset: 0x00177613
	private float UnfetchedAmount
	{
		get
		{
			return this._UnfetchedAmount;
		}
		set
		{
			this._UnfetchedAmount = value;
			this.Assert(this._UnfetchedAmount <= this.TotalAmount, "_UnfetchedAmount <= TotalAmount");
			this.Assert(this._UnfetchedAmount >= 0f, "_UnfetchedAmount >= 0");
		}
	}

	// Token: 0x060042BF RID: 17087 RVA: 0x00179454 File Offset: 0x00177654
	public FetchOrder2(ChoreType chore_type, HashSet<Tag> tags, FetchChore.MatchCriteria criteria, Tag required_tag, Tag[] forbidden_tags, Storage destination, float amount, Operational.State operationalRequirementDEPRECATED = Operational.State.None, int priorityMod = 0)
	{
		if (amount <= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("FetchOrder2 {0} is requesting {1} {2} to {3}", new object[]
				{
					chore_type.Id,
					tags,
					amount,
					(destination != null) ? destination.name : "to nowhere"
				})
			});
		}
		this.choreType = chore_type;
		this.Tags = tags;
		this.Criteria = criteria;
		this.RequiredTag = required_tag;
		this.ForbiddenTags = forbidden_tags;
		this.Destination = destination;
		this.TotalAmount = amount;
		this.UnfetchedAmount = amount;
		this.PriorityMod = priorityMod;
		this.operationalRequirement = operationalRequirementDEPRECATED;
	}

	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x060042C0 RID: 17088 RVA: 0x00179520 File Offset: 0x00177720
	public bool InProgress
	{
		get
		{
			bool result = false;
			using (List<FetchChore>.Enumerator enumerator = this.Chores.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.InProgress())
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
	}

	// Token: 0x060042C1 RID: 17089 RVA: 0x0017957C File Offset: 0x0017777C
	private void IssueTask()
	{
		if (this.UnfetchedAmount > 0f)
		{
			this.SetFetchTask(this.UnfetchedAmount);
			this.UnfetchedAmount = 0f;
		}
	}

	// Token: 0x060042C2 RID: 17090 RVA: 0x001795A4 File Offset: 0x001777A4
	public void SetPriorityMod(int priorityMod)
	{
		this.PriorityMod = priorityMod;
		for (int i = 0; i < this.Chores.Count; i++)
		{
			this.Chores[i].SetPriorityMod(this.PriorityMod);
		}
	}

	// Token: 0x060042C3 RID: 17091 RVA: 0x001795E8 File Offset: 0x001777E8
	private void SetFetchTask(float amount)
	{
		FetchChore fetchChore = new FetchChore(this.choreType, this.Destination, amount, this.Tags, this.Criteria, this.RequiredTag, this.ForbiddenTags, null, true, new Action<Chore>(this.OnFetchChoreComplete), new Action<Chore>(this.OnFetchChoreBegin), new Action<Chore>(this.OnFetchChoreEnd), this.operationalRequirement, this.PriorityMod);
		fetchChore.validateRequiredTagOnTagChange = this.validateRequiredTagOnTagChange;
		this.Chores.Add(fetchChore);
	}

	// Token: 0x060042C4 RID: 17092 RVA: 0x0017966C File Offset: 0x0017786C
	private void OnFetchChoreEnd(Chore chore)
	{
		FetchChore fetchChore = (FetchChore)chore;
		if (this.Chores.Contains(fetchChore))
		{
			this.UnfetchedAmount += fetchChore.amount;
			fetchChore.Cancel("FetchChore Redistribution");
			this.Chores.Remove(fetchChore);
			this.IssueTask();
		}
	}

	// Token: 0x060042C5 RID: 17093 RVA: 0x001796C0 File Offset: 0x001778C0
	private void OnFetchChoreComplete(Chore chore)
	{
		FetchChore fetchChore = (FetchChore)chore;
		this.Chores.Remove(fetchChore);
		if (this.Chores.Count == 0 && this.OnComplete != null)
		{
			this.OnComplete(this, fetchChore.fetchTarget);
		}
	}

	// Token: 0x060042C6 RID: 17094 RVA: 0x00179708 File Offset: 0x00177908
	private void OnFetchChoreBegin(Chore chore)
	{
		FetchChore fetchChore = (FetchChore)chore;
		this.UnfetchedAmount += fetchChore.originalAmount - fetchChore.amount;
		this.IssueTask();
		if (this.OnBegin != null)
		{
			this.OnBegin(this, fetchChore.fetchTarget);
		}
	}

	// Token: 0x060042C7 RID: 17095 RVA: 0x00179758 File Offset: 0x00177958
	public void Cancel(string reason)
	{
		while (this.Chores.Count > 0)
		{
			FetchChore fetchChore = this.Chores[0];
			fetchChore.Cancel(reason);
			this.Chores.Remove(fetchChore);
		}
	}

	// Token: 0x060042C8 RID: 17096 RVA: 0x00179796 File Offset: 0x00177996
	public void Suspend(string reason)
	{
		global::Debug.LogError("UNIMPLEMENTED!");
	}

	// Token: 0x060042C9 RID: 17097 RVA: 0x001797A2 File Offset: 0x001779A2
	public void Resume(string reason)
	{
		global::Debug.LogError("UNIMPLEMENTED!");
	}

	// Token: 0x060042CA RID: 17098 RVA: 0x001797B0 File Offset: 0x001779B0
	public void Submit(Action<FetchOrder2, Pickupable> on_complete, bool check_storage_contents, Action<FetchOrder2, Pickupable> on_begin = null)
	{
		this.OnComplete = on_complete;
		this.OnBegin = on_begin;
		this.checkStorageContents = check_storage_contents;
		if (check_storage_contents)
		{
			Pickupable arg = null;
			this.UnfetchedAmount = this.GetRemaining(out arg);
			if (this.UnfetchedAmount > this.Destination.storageFullMargin)
			{
				this.IssueTask();
				return;
			}
			if (this.OnComplete != null)
			{
				this.OnComplete(this, arg);
				return;
			}
		}
		else
		{
			this.IssueTask();
		}
	}

	// Token: 0x060042CB RID: 17099 RVA: 0x0017981C File Offset: 0x00177A1C
	public bool IsMaterialOnStorage(Storage storage, ref float amount, ref Pickupable out_item)
	{
		foreach (GameObject gameObject in this.Destination.items)
		{
			if (gameObject != null)
			{
				Pickupable component = gameObject.GetComponent<Pickupable>();
				if (component != null)
				{
					KPrefabID kprefabID = component.KPrefabID;
					foreach (Tag tag in this.Tags)
					{
						if (kprefabID.HasTag(tag))
						{
							amount = component.FetchTotalAmount;
							out_item = component;
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x060042CC RID: 17100 RVA: 0x001798E8 File Offset: 0x00177AE8
	public float AmountWaitingToFetch()
	{
		if (!this.checkStorageContents)
		{
			float num = this.UnfetchedAmount;
			for (int i = 0; i < this.Chores.Count; i++)
			{
				num += this.Chores[i].AmountWaitingToFetch();
			}
			return num;
		}
		Pickupable pickupable;
		return this.GetRemaining(out pickupable);
	}

	// Token: 0x060042CD RID: 17101 RVA: 0x00179938 File Offset: 0x00177B38
	public float GetRemaining(out Pickupable out_item)
	{
		float num = this.TotalAmount;
		float num2 = 0f;
		out_item = null;
		if (this.IsMaterialOnStorage(this.Destination, ref num2, ref out_item))
		{
			num = Math.Max(num - num2, 0f);
		}
		return num;
	}

	// Token: 0x060042CE RID: 17102 RVA: 0x00179978 File Offset: 0x00177B78
	public bool IsComplete()
	{
		for (int i = 0; i < this.Chores.Count; i++)
		{
			if (!this.Chores[i].isComplete)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060042CF RID: 17103 RVA: 0x001799B4 File Offset: 0x00177BB4
	private void Assert(bool condition, string message)
	{
		if (condition)
		{
			return;
		}
		string text = "FetchOrder error: " + message;
		if (this.Destination == null)
		{
			text += "\nDestination: None";
		}
		else
		{
			text = text + "\nDestination: " + this.Destination.name;
		}
		text = text + "\nTotal Amount: " + this.TotalAmount.ToString();
		text = text + "\nUnfetched Amount: " + this._UnfetchedAmount.ToString();
		global::Debug.LogError(text);
	}

	// Token: 0x040029F3 RID: 10739
	public Action<FetchOrder2, Pickupable> OnComplete;

	// Token: 0x040029F4 RID: 10740
	public Action<FetchOrder2, Pickupable> OnBegin;

	// Token: 0x040029F9 RID: 10745
	public bool validateRequiredTagOnTagChange;

	// Token: 0x040029FD RID: 10749
	public List<FetchChore> Chores = new List<FetchChore>();

	// Token: 0x040029FE RID: 10750
	private ChoreType choreType;

	// Token: 0x040029FF RID: 10751
	private float _UnfetchedAmount;

	// Token: 0x04002A00 RID: 10752
	private bool checkStorageContents;

	// Token: 0x04002A01 RID: 10753
	private Operational.State operationalRequirement = Operational.State.None;
}
