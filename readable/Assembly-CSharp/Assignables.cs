using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006DE RID: 1758
[AddComponentMenu("KMonoBehaviour/scripts/Assignables")]
public class Assignables : KMonoBehaviour
{
	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06002B42 RID: 11074 RVA: 0x000FCBA8 File Offset: 0x000FADA8
	public List<AssignableSlotInstance> Slots
	{
		get
		{
			return this.slots;
		}
	}

	// Token: 0x06002B43 RID: 11075 RVA: 0x000FCBB0 File Offset: 0x000FADB0
	protected IAssignableIdentity GetAssignableIdentity()
	{
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		if (component != null)
		{
			return component.assignableProxy.Get();
		}
		return base.GetComponent<MinionAssignablesProxy>();
	}

	// Token: 0x06002B44 RID: 11076 RVA: 0x000FCBDF File Offset: 0x000FADDF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameUtil.SubscribeToTags<Assignables>(this, Assignables.OnDeadTagAddedDelegate, true);
	}

	// Token: 0x06002B45 RID: 11077 RVA: 0x000FCBF4 File Offset: 0x000FADF4
	private void OnDeath(object data)
	{
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			assignableSlotInstance.Unassign(true);
		}
	}

	// Token: 0x06002B46 RID: 11078 RVA: 0x000FCC48 File Offset: 0x000FAE48
	public void Add(AssignableSlotInstance slot_instance)
	{
		this.slots.Add(slot_instance);
	}

	// Token: 0x06002B47 RID: 11079 RVA: 0x000FCC58 File Offset: 0x000FAE58
	public Assignable GetAssignable(AssignableSlot slot)
	{
		AssignableSlotInstance slot2 = this.GetSlot(slot);
		if (slot2 == null)
		{
			return null;
		}
		return slot2.assignable;
	}

	// Token: 0x06002B48 RID: 11080 RVA: 0x000FCC78 File Offset: 0x000FAE78
	public AssignableSlotInstance GetSlot(AssignableSlot slot)
	{
		global::Debug.Assert(this.slots.Count > 0, "GetSlot called with no slots configured");
		if (slot == null)
		{
			return null;
		}
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			if (assignableSlotInstance.slot == slot)
			{
				return assignableSlotInstance;
			}
		}
		return null;
	}

	// Token: 0x06002B49 RID: 11081 RVA: 0x000FCCF4 File Offset: 0x000FAEF4
	public AssignableSlotInstance[] GetSlots(AssignableSlot slot)
	{
		global::Debug.Assert(this.slots.Count > 0, "GetSlot called with no slots configured");
		if (slot == null)
		{
			return null;
		}
		List<AssignableSlotInstance> list = this.slots.FindAll((AssignableSlotInstance s) => s.slot == slot);
		if (list != null && list.Count > 0)
		{
			return list.ToArray();
		}
		return null;
	}

	// Token: 0x06002B4A RID: 11082 RVA: 0x000FCD5C File Offset: 0x000FAF5C
	public Assignable AutoAssignSlot(AssignableSlot slot)
	{
		Assignable assignable = this.GetAssignable(slot);
		if (assignable != null)
		{
			return assignable;
		}
		GameObject targetGameObject = base.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		if (targetGameObject == null)
		{
			global::Debug.LogWarning("AutoAssignSlot failed, proxy game object was null.");
			return null;
		}
		Navigator component = targetGameObject.GetComponent<Navigator>();
		IAssignableIdentity assignableIdentity = this.GetAssignableIdentity();
		int num = int.MaxValue;
		foreach (Assignable assignable2 in Game.Instance.assignmentManager)
		{
			if (!(assignable2 == null) && !assignable2.IsAssigned() && assignable2.slot == slot && assignable2.CanAutoAssignTo(assignableIdentity))
			{
				int navigationCost = assignable2.GetNavigationCost(component);
				if (navigationCost != -1 && navigationCost < num)
				{
					num = navigationCost;
					assignable = assignable2;
				}
			}
		}
		if (assignable != null)
		{
			assignable.Assign(assignableIdentity);
		}
		return assignable;
	}

	// Token: 0x06002B4B RID: 11083 RVA: 0x000FCE4C File Offset: 0x000FB04C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			assignableSlotInstance.Unassign(true);
		}
	}

	// Token: 0x040019DE RID: 6622
	protected List<AssignableSlotInstance> slots = new List<AssignableSlotInstance>();

	// Token: 0x040019DF RID: 6623
	private static readonly EventSystem.IntraObjectHandler<Assignables> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<Assignables>(GameTags.Dead, delegate(Assignables component, object data)
	{
		component.OnDeath(data);
	});
}
