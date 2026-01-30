using System;
using UnityEngine;

// Token: 0x020006DD RID: 1757
public abstract class AssignableSlotInstance
{
	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06002B39 RID: 11065 RVA: 0x000FCAC2 File Offset: 0x000FACC2
	// (set) Token: 0x06002B3A RID: 11066 RVA: 0x000FCACA File Offset: 0x000FACCA
	public Assignables assignables { get; private set; }

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06002B3B RID: 11067 RVA: 0x000FCAD3 File Offset: 0x000FACD3
	public GameObject gameObject
	{
		get
		{
			return this.assignables.gameObject;
		}
	}

	// Token: 0x06002B3C RID: 11068 RVA: 0x000FCAE0 File Offset: 0x000FACE0
	public AssignableSlotInstance(Assignables assignables, AssignableSlot slot) : this(slot.Id, assignables, slot)
	{
	}

	// Token: 0x06002B3D RID: 11069 RVA: 0x000FCAF0 File Offset: 0x000FACF0
	public AssignableSlotInstance(string id, Assignables assignables, AssignableSlot slot)
	{
		this.ID = id;
		this.slot = slot;
		this.assignables = assignables;
	}

	// Token: 0x06002B3E RID: 11070 RVA: 0x000FCB0D File Offset: 0x000FAD0D
	public void Assign(Assignable assignable)
	{
		if (this.assignable == assignable)
		{
			return;
		}
		this.Unassign(false);
		this.assignable = assignable;
		this.assignables.Trigger(-1585839766, this);
	}

	// Token: 0x06002B3F RID: 11071 RVA: 0x000FCB40 File Offset: 0x000FAD40
	public virtual void Unassign(bool trigger_event = true)
	{
		if (this.unassigning)
		{
			return;
		}
		if (this.IsAssigned())
		{
			this.unassigning = true;
			this.assignable.Unassign();
			if (trigger_event)
			{
				this.assignables.Trigger(-1585839766, this);
			}
			this.assignable = null;
			this.unassigning = false;
		}
	}

	// Token: 0x06002B40 RID: 11072 RVA: 0x000FCB92 File Offset: 0x000FAD92
	public bool IsAssigned()
	{
		return this.assignable != null;
	}

	// Token: 0x06002B41 RID: 11073 RVA: 0x000FCBA0 File Offset: 0x000FADA0
	public bool IsUnassigning()
	{
		return this.unassigning;
	}

	// Token: 0x040019D9 RID: 6617
	public string ID;

	// Token: 0x040019DA RID: 6618
	public AssignableSlot slot;

	// Token: 0x040019DB RID: 6619
	public Assignable assignable;

	// Token: 0x040019DD RID: 6621
	private bool unassigning;
}
