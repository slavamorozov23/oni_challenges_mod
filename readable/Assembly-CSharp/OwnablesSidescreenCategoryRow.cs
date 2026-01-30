using System;

// Token: 0x02000E5D RID: 3677
public class OwnablesSidescreenCategoryRow : KMonoBehaviour
{
	// Token: 0x17000815 RID: 2069
	// (get) Token: 0x060074A6 RID: 29862 RVA: 0x002C80FC File Offset: 0x002C62FC
	private AssignableSlot[] slots
	{
		get
		{
			return this.data.slots;
		}
	}

	// Token: 0x060074A7 RID: 29863 RVA: 0x002C8109 File Offset: 0x002C6309
	public void SetCategoryData(OwnablesSidescreenCategoryRow.Data categoryData)
	{
		this.DeleteAllRows();
		this.data = categoryData;
		this.titleLabel.text = categoryData.name;
	}

	// Token: 0x060074A8 RID: 29864 RVA: 0x002C8129 File Offset: 0x002C6329
	public void SetOwner(Assignables owner)
	{
		this.owner = owner;
		if (owner != null)
		{
			this.RecreateAllItemRows();
			return;
		}
		this.DeleteAllRows();
	}

	// Token: 0x060074A9 RID: 29865 RVA: 0x002C8148 File Offset: 0x002C6348
	private void RecreateAllItemRows()
	{
		this.DeleteAllRows();
		this.itemRows = new OwnablesSidescreenItemRow[this.slots.Length];
		IAssignableIdentity component = this.owner.gameObject.GetComponent<IAssignableIdentity>();
		for (int i = 0; i < this.slots.Length; i++)
		{
			AssignableSlot slot = this.slots[i];
			this.itemRows[i] = this.CreateRow(slot, component);
		}
	}

	// Token: 0x060074AA RID: 29866 RVA: 0x002C81AC File Offset: 0x002C63AC
	private OwnablesSidescreenItemRow CreateRow(AssignableSlot slot, IAssignableIdentity ownerIdentity)
	{
		this.originalItemRow.gameObject.SetActive(false);
		OwnablesSidescreenItemRow component = Util.KInstantiateUI(this.originalItemRow.gameObject, this.originalItemRow.transform.parent.gameObject, false).GetComponent<OwnablesSidescreenItemRow>();
		component.OnSlotRowClicked = (Action<OwnablesSidescreenItemRow>)Delegate.Combine(component.OnSlotRowClicked, new Action<OwnablesSidescreenItemRow>(this.OnRowClicked));
		component.gameObject.SetActive(true);
		component.SetData(this.owner, slot, !this.data.IsSlotApplicable(ownerIdentity, slot));
		return component;
	}

	// Token: 0x060074AB RID: 29867 RVA: 0x002C8240 File Offset: 0x002C6440
	private void OnRowClicked(OwnablesSidescreenItemRow row)
	{
		Action<OwnablesSidescreenItemRow> onSlotRowClicked = this.OnSlotRowClicked;
		if (onSlotRowClicked == null)
		{
			return;
		}
		onSlotRowClicked(row);
	}

	// Token: 0x060074AC RID: 29868 RVA: 0x002C8254 File Offset: 0x002C6454
	private void DeleteAllRows()
	{
		this.originalItemRow.gameObject.SetActive(false);
		if (this.itemRows != null)
		{
			for (int i = 0; i < this.itemRows.Length; i++)
			{
				this.itemRows[i].ClearData();
				this.itemRows[i].DeleteObject();
			}
			this.itemRows = null;
		}
	}

	// Token: 0x060074AD RID: 29869 RVA: 0x002C82B0 File Offset: 0x002C64B0
	public void SetSelectedRow_VisualsOnly(AssignableSlotInstance slotInstance)
	{
		if (this.itemRows == null)
		{
			return;
		}
		for (int i = 0; i < this.itemRows.Length; i++)
		{
			OwnablesSidescreenItemRow ownablesSidescreenItemRow = this.itemRows[i];
			ownablesSidescreenItemRow.SetSelectedVisualState(ownablesSidescreenItemRow.SlotInstance == slotInstance);
		}
	}

	// Token: 0x040050A5 RID: 20645
	public Action<OwnablesSidescreenItemRow> OnSlotRowClicked;

	// Token: 0x040050A6 RID: 20646
	public LocText titleLabel;

	// Token: 0x040050A7 RID: 20647
	public OwnablesSidescreenItemRow originalItemRow;

	// Token: 0x040050A8 RID: 20648
	private Assignables owner;

	// Token: 0x040050A9 RID: 20649
	private OwnablesSidescreenCategoryRow.Data data;

	// Token: 0x040050AA RID: 20650
	private OwnablesSidescreenItemRow[] itemRows;

	// Token: 0x020020CF RID: 8399
	public struct AssignableSlotData
	{
		// Token: 0x0600BA63 RID: 47715 RVA: 0x003FB0CB File Offset: 0x003F92CB
		public AssignableSlotData(AssignableSlot slot, Func<IAssignableIdentity, bool> isApplicableCallback)
		{
			this.slot = slot;
			this.IsApplicableCallback = isApplicableCallback;
		}

		// Token: 0x04009738 RID: 38712
		public AssignableSlot slot;

		// Token: 0x04009739 RID: 38713
		public Func<IAssignableIdentity, bool> IsApplicableCallback;
	}

	// Token: 0x020020D0 RID: 8400
	public struct Data
	{
		// Token: 0x0600BA64 RID: 47716 RVA: 0x003FB0DC File Offset: 0x003F92DC
		public Data(string name, OwnablesSidescreenCategoryRow.AssignableSlotData[] slotsData)
		{
			this.name = name;
			this.slotsData = slotsData;
			this.slots = new AssignableSlot[slotsData.Length];
			for (int i = 0; i < slotsData.Length; i++)
			{
				this.slots[i] = slotsData[i].slot;
			}
		}

		// Token: 0x0600BA65 RID: 47717 RVA: 0x003FB128 File Offset: 0x003F9328
		public bool IsSlotApplicable(IAssignableIdentity identity, AssignableSlot slot)
		{
			for (int i = 0; i < this.slotsData.Length; i++)
			{
				OwnablesSidescreenCategoryRow.AssignableSlotData assignableSlotData = this.slotsData[i];
				if (assignableSlotData.slot == slot)
				{
					return assignableSlotData.IsApplicableCallback(identity);
				}
			}
			return false;
		}

		// Token: 0x0400973A RID: 38714
		public string name;

		// Token: 0x0400973B RID: 38715
		public AssignableSlot[] slots;

		// Token: 0x0400973C RID: 38716
		private OwnablesSidescreenCategoryRow.AssignableSlotData[] slotsData;
	}
}
