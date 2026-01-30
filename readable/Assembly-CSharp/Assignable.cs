using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x020006DB RID: 1755
public abstract class Assignable : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x17000208 RID: 520
	// (get) Token: 0x06002B1B RID: 11035 RVA: 0x000FC253 File Offset: 0x000FA453
	public AssignableSlot slot
	{
		get
		{
			if (this._slot == null)
			{
				this._slot = Db.Get().AssignableSlots.Get(this.slotID);
			}
			return this._slot;
		}
	}

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x06002B1C RID: 11036 RVA: 0x000FC27E File Offset: 0x000FA47E
	public bool CanBeAssigned
	{
		get
		{
			return this.canBeAssigned;
		}
	}

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06002B1D RID: 11037 RVA: 0x000FC288 File Offset: 0x000FA488
	// (remove) Token: 0x06002B1E RID: 11038 RVA: 0x000FC2C0 File Offset: 0x000FA4C0
	public event Action<IAssignableIdentity> OnAssign;

	// Token: 0x06002B1F RID: 11039 RVA: 0x000FC2F5 File Offset: 0x000FA4F5
	[OnDeserialized]
	internal void OnDeserialized()
	{
	}

	// Token: 0x06002B20 RID: 11040 RVA: 0x000FC2F8 File Offset: 0x000FA4F8
	private void RestoreAssignee()
	{
		IAssignableIdentity savedAssignee = this.GetSavedAssignee();
		if (savedAssignee != null)
		{
			AssignableSlotInstance savedSlotInstance = this.GetSavedSlotInstance(savedAssignee);
			this.Assign(savedAssignee, savedSlotInstance);
		}
	}

	// Token: 0x06002B21 RID: 11041 RVA: 0x000FC320 File Offset: 0x000FA520
	private AssignableSlotInstance GetSavedSlotInstance(IAssignableIdentity savedAsignee)
	{
		if ((savedAsignee != null && savedAsignee is MinionIdentity) || savedAsignee is StoredMinionIdentity || savedAsignee is MinionAssignablesProxy)
		{
			Ownables soleOwner = savedAsignee.GetSoleOwner();
			if (soleOwner != null)
			{
				AssignableSlotInstance[] slots = soleOwner.GetSlots(this.slot);
				if (slots != null)
				{
					AssignableSlotInstance assignableSlotInstance = slots.FindFirst((AssignableSlotInstance i) => i.ID == this.assignee_slotInstanceID);
					if (assignableSlotInstance != null)
					{
						return assignableSlotInstance;
					}
				}
			}
			Equipment component = soleOwner.GetComponent<Equipment>();
			if (component != null)
			{
				AssignableSlotInstance[] slots2 = component.GetSlots(this.slot);
				if (slots2 != null)
				{
					AssignableSlotInstance assignableSlotInstance2 = slots2.FindFirst((AssignableSlotInstance i) => i.ID == this.assignee_slotInstanceID);
					if (assignableSlotInstance2 != null)
					{
						return assignableSlotInstance2;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x000FC3C0 File Offset: 0x000FA5C0
	private IAssignableIdentity GetSavedAssignee()
	{
		if (this.assignee_identityRef.Get() != null)
		{
			return this.assignee_identityRef.Get().GetComponent<IAssignableIdentity>();
		}
		if (!string.IsNullOrEmpty(this.assignee_groupID))
		{
			return Game.Instance.assignmentManager.assignment_groups[this.assignee_groupID];
		}
		return null;
	}

	// Token: 0x06002B23 RID: 11043 RVA: 0x000FC41C File Offset: 0x000FA61C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreAssignee();
		Components.AssignableItems.Add(this);
		Game.Instance.assignmentManager.Add(this);
		if (this.assignee == null && this.canBePublic)
		{
			this.Assign(Game.Instance.assignmentManager.assignment_groups["public"]);
		}
		this.assignmentPreconditions.Add(delegate(MinionAssignablesProxy proxy)
		{
			GameObject targetGameObject = proxy.GetTargetGameObject();
			return targetGameObject.GetComponent<KMonoBehaviour>().GetMyWorldId() == this.GetMyWorldId() || targetGameObject.IsMyParentWorld(base.gameObject);
		});
		this.autoassignmentPreconditions.Add(delegate(MinionAssignablesProxy proxy)
		{
			Operational component = base.GetComponent<Operational>();
			return !(component != null) || component.IsOperational;
		});
	}

	// Token: 0x06002B24 RID: 11044 RVA: 0x000FC4AD File Offset: 0x000FA6AD
	protected override void OnCleanUp()
	{
		this.Unassign();
		Components.AssignableItems.Remove(this);
		Game.Instance.assignmentManager.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002B25 RID: 11045 RVA: 0x000FC4D8 File Offset: 0x000FA6D8
	public bool CanAutoAssignTo(IAssignableIdentity identity)
	{
		MinionAssignablesProxy minionAssignablesProxy = identity as MinionAssignablesProxy;
		if (minionAssignablesProxy == null)
		{
			return true;
		}
		if (!this.CanAssignTo(minionAssignablesProxy))
		{
			return false;
		}
		using (List<Func<MinionAssignablesProxy, bool>>.Enumerator enumerator = this.autoassignmentPreconditions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current(minionAssignablesProxy))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06002B26 RID: 11046 RVA: 0x000FC550 File Offset: 0x000FA750
	public bool CanAssignTo(IAssignableIdentity identity)
	{
		MinionAssignablesProxy minionAssignablesProxy = identity as MinionAssignablesProxy;
		if (minionAssignablesProxy == null)
		{
			return true;
		}
		using (List<Func<MinionAssignablesProxy, bool>>.Enumerator enumerator = this.assignmentPreconditions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current(minionAssignablesProxy))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06002B27 RID: 11047 RVA: 0x000FC5BC File Offset: 0x000FA7BC
	public bool IsAssigned()
	{
		return this.assignee != null;
	}

	// Token: 0x06002B28 RID: 11048 RVA: 0x000FC5C8 File Offset: 0x000FA7C8
	public bool IsAssignedTo(IAssignableIdentity identity)
	{
		global::Debug.Assert(identity != null, "IsAssignedTo identity is null");
		Ownables soleOwner = identity.GetSoleOwner();
		global::Debug.Assert(soleOwner != null, "IsAssignedTo identity sole owner is null");
		if (this.assignee != null)
		{
			foreach (Ownables ownables in this.assignee.GetOwners())
			{
				global::Debug.Assert(ownables, "Assignable owners list contained null");
				if (ownables.gameObject == soleOwner.gameObject)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x06002B29 RID: 11049 RVA: 0x000FC670 File Offset: 0x000FA870
	public virtual void Assign(IAssignableIdentity new_assignee)
	{
		this.Assign(new_assignee, null);
	}

	// Token: 0x06002B2A RID: 11050 RVA: 0x000FC67C File Offset: 0x000FA87C
	public virtual void Assign(IAssignableIdentity new_assignee, AssignableSlotInstance specificSlotInstance)
	{
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (new_assignee is KMonoBehaviour)
		{
			if (!this.CanAssignTo(new_assignee))
			{
				return;
			}
			this.assignee_identityRef.Set((KMonoBehaviour)new_assignee);
			this.assignee_groupID = "";
		}
		else if (new_assignee is AssignmentGroup)
		{
			this.assignee_identityRef.Set(null);
			this.assignee_groupID = ((AssignmentGroup)new_assignee).id;
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.Assigned, false);
		this.assignee = new_assignee;
		this.assignee_slotInstanceID = null;
		if (this.slot != null && (new_assignee is MinionIdentity || new_assignee is StoredMinionIdentity || new_assignee is MinionAssignablesProxy))
		{
			if (specificSlotInstance == null)
			{
				Ownables soleOwner = new_assignee.GetSoleOwner();
				if (soleOwner != null)
				{
					AssignableSlotInstance slot = soleOwner.GetSlot(this.slot);
					if (slot != null)
					{
						this.assignee_slotInstanceID = slot.ID;
						slot.Assign(this);
					}
				}
				Equipment component = soleOwner.GetComponent<Equipment>();
				if (component != null)
				{
					AssignableSlotInstance slot2 = component.GetSlot(this.slot);
					if (slot2 != null)
					{
						this.assignee_slotInstanceID = slot2.ID;
						slot2.Assign(this);
					}
				}
			}
			else
			{
				this.assignee_slotInstanceID = specificSlotInstance.ID;
				specificSlotInstance.Assign(this);
			}
		}
		if (this.OnAssign != null)
		{
			this.OnAssign(new_assignee);
		}
		base.Trigger(684616645, new_assignee);
	}

	// Token: 0x06002B2B RID: 11051 RVA: 0x000FC7C8 File Offset: 0x000FA9C8
	public virtual void Unassign()
	{
		if (this.assignee == null)
		{
			return;
		}
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.Assigned);
		if (this.slot != null)
		{
			Ownables soleOwner = this.assignee.GetSoleOwner();
			if (soleOwner)
			{
				AssignableSlotInstance[] slots = soleOwner.GetSlots(this.slot);
				AssignableSlotInstance assignableSlotInstance = (slots == null) ? null : slots.FindFirst((AssignableSlotInstance s) => s.assignable == this);
				if (assignableSlotInstance != null)
				{
					assignableSlotInstance.Unassign(true);
				}
				Equipment component = soleOwner.GetComponent<Equipment>();
				if (component != null)
				{
					AssignableSlotInstance[] slots2 = component.GetSlots(this.slot);
					assignableSlotInstance = ((slots2 == null) ? null : slots2.FindFirst((AssignableSlotInstance s) => s.assignable == this));
					if (assignableSlotInstance != null)
					{
						assignableSlotInstance.Unassign(true);
					}
				}
			}
		}
		this.assignee = null;
		if (this.canBePublic)
		{
			this.Assign(Game.Instance.assignmentManager.assignment_groups["public"]);
		}
		this.assignee_slotInstanceID = null;
		this.assignee_identityRef.Set(null);
		this.assignee_groupID = "";
		if (this.OnAssign != null)
		{
			this.OnAssign(null);
		}
		base.Trigger(684616645, null);
	}

	// Token: 0x06002B2C RID: 11052 RVA: 0x000FC8EC File Offset: 0x000FAAEC
	public void SetCanBeAssigned(bool state)
	{
		this.canBeAssigned = state;
	}

	// Token: 0x06002B2D RID: 11053 RVA: 0x000FC8F5 File Offset: 0x000FAAF5
	public void AddAssignPrecondition(Func<MinionAssignablesProxy, bool> precondition)
	{
		this.assignmentPreconditions.Add(precondition);
	}

	// Token: 0x06002B2E RID: 11054 RVA: 0x000FC903 File Offset: 0x000FAB03
	public void AddAutoassignPrecondition(Func<MinionAssignablesProxy, bool> precondition)
	{
		this.autoassignmentPreconditions.Add(precondition);
	}

	// Token: 0x06002B2F RID: 11055 RVA: 0x000FC914 File Offset: 0x000FAB14
	public int GetNavigationCost(Navigator navigator)
	{
		int num = -1;
		int cell = Grid.PosToCell(this);
		IApproachable component = base.GetComponent<IApproachable>();
		CellOffset[] array = (component != null) ? component.GetOffsets() : new CellOffset[1];
		DebugUtil.DevAssert(navigator != null, "Navigator is mysteriously null", null);
		if (navigator == null)
		{
			return -1;
		}
		foreach (CellOffset offset in array)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			int navigationCost = navigator.GetNavigationCost(cell2);
			if (navigationCost != -1 && (num == -1 || navigationCost < num))
			{
				num = navigationCost;
			}
		}
		return num;
	}

	// Token: 0x040019C9 RID: 6601
	public string slotID;

	// Token: 0x040019CA RID: 6602
	private AssignableSlot _slot;

	// Token: 0x040019CB RID: 6603
	public IAssignableIdentity assignee;

	// Token: 0x040019CC RID: 6604
	[Serialize]
	protected Ref<KMonoBehaviour> assignee_identityRef = new Ref<KMonoBehaviour>();

	// Token: 0x040019CD RID: 6605
	[Serialize]
	protected string assignee_slotInstanceID;

	// Token: 0x040019CE RID: 6606
	[Serialize]
	private string assignee_groupID = "";

	// Token: 0x040019CF RID: 6607
	public AssignableSlot[] subSlots;

	// Token: 0x040019D0 RID: 6608
	public bool canBePublic;

	// Token: 0x040019D1 RID: 6609
	[Serialize]
	private bool canBeAssigned = true;

	// Token: 0x040019D2 RID: 6610
	public int priority;

	// Token: 0x040019D3 RID: 6611
	private List<Func<MinionAssignablesProxy, bool>> autoassignmentPreconditions = new List<Func<MinionAssignablesProxy, bool>>();

	// Token: 0x040019D4 RID: 6612
	private List<Func<MinionAssignablesProxy, bool>> assignmentPreconditions = new List<Func<MinionAssignablesProxy, bool>>();

	// Token: 0x040019D5 RID: 6613
	public Func<Assignables, string> customAssignmentUITooltipFunc;

	// Token: 0x040019D6 RID: 6614
	public Func<Assignables, string> customAssignablesUITooltipFunc;
}
