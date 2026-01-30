using System;

// Token: 0x0200051C RID: 1308
public class AssignableReachabilitySensor : Sensor
{
	// Token: 0x06001C4E RID: 7246 RVA: 0x0009BAC0 File Offset: 0x00099CC0
	public AssignableReachabilitySensor(Sensors sensors) : base(sensors)
	{
		MinionAssignablesProxy minionAssignablesProxy = base.gameObject.GetComponent<MinionIdentity>().assignableProxy.Get();
		minionAssignablesProxy.ConfigureAssignableSlots();
		Assignables[] components = minionAssignablesProxy.GetComponents<Assignables>();
		if (components.Length == 0)
		{
			Debug.LogError(base.gameObject.GetProperName() + ": No 'Assignables' components found for AssignableReachabilitySensor");
		}
		int num = 0;
		foreach (Assignables assignables in components)
		{
			num += assignables.Slots.Count;
		}
		this.slots = new AssignableReachabilitySensor.SlotEntry[num];
		int num2 = 0;
		foreach (Assignables assignables2 in components)
		{
			for (int k = 0; k < assignables2.Slots.Count; k++)
			{
				this.slots[num2++].slot = assignables2.Slots[k];
			}
		}
		this.navigator = base.GetComponent<Navigator>();
	}

	// Token: 0x06001C4F RID: 7247 RVA: 0x0009BBA8 File Offset: 0x00099DA8
	public bool IsReachable(AssignableSlot slot)
	{
		for (int i = 0; i < this.slots.Length; i++)
		{
			if (this.slots[i].slot.slot == slot)
			{
				return this.slots[i].isReachable;
			}
		}
		Debug.LogError("Could not find slot: " + ((slot != null) ? slot.ToString() : null));
		return false;
	}

	// Token: 0x06001C50 RID: 7248 RVA: 0x0009BC10 File Offset: 0x00099E10
	public override void Update()
	{
		for (int i = 0; i < this.slots.Length; i++)
		{
			AssignableReachabilitySensor.SlotEntry slotEntry = this.slots[i];
			AssignableSlotInstance slot = slotEntry.slot;
			if (slot.IsAssigned())
			{
				bool flag = slot.assignable.GetNavigationCost(this.navigator) != -1;
				Operational component = slot.assignable.GetComponent<Operational>();
				if (component != null)
				{
					flag = (flag && component.IsOperational);
				}
				if (flag != slotEntry.isReachable)
				{
					slotEntry.isReachable = flag;
					this.slots[i] = slotEntry;
					base.Trigger(334784980, slotEntry);
				}
			}
			else if (slotEntry.isReachable)
			{
				slotEntry.isReachable = false;
				this.slots[i] = slotEntry;
				base.Trigger(334784980, slotEntry);
			}
		}
	}

	// Token: 0x040010A8 RID: 4264
	private AssignableReachabilitySensor.SlotEntry[] slots;

	// Token: 0x040010A9 RID: 4265
	private Navigator navigator;

	// Token: 0x020013A7 RID: 5031
	private struct SlotEntry
	{
		// Token: 0x04006C12 RID: 27666
		public AssignableSlotInstance slot;

		// Token: 0x04006C13 RID: 27667
		public bool isReachable;
	}
}
