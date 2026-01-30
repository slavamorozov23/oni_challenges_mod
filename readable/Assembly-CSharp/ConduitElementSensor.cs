using System;
using KSerialization;

// Token: 0x02000735 RID: 1845
[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitElementSensor : ConduitSensor
{
	// Token: 0x06002E75 RID: 11893 RVA: 0x0010C68E File Offset: 0x0010A88E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.filterable.onFilterChanged += this.OnFilterChanged;
		this.OnFilterChanged(this.filterable.SelectedTag);
	}

	// Token: 0x06002E76 RID: 11894 RVA: 0x0010C6C0 File Offset: 0x0010A8C0
	private void OnFilterChanged(Tag tag)
	{
		if (!tag.IsValid)
		{
			return;
		}
		bool on = tag == GameTags.Void;
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on, null);
	}

	// Token: 0x06002E77 RID: 11895 RVA: 0x0010C700 File Offset: 0x0010A900
	protected override void ConduitUpdate(float dt)
	{
		Tag a;
		bool flag;
		this.GetContentsElement(out a, out flag);
		if (!base.IsSwitchedOn)
		{
			if (a == this.filterable.SelectedTag && flag)
			{
				this.Toggle();
				return;
			}
		}
		else if (a != this.filterable.SelectedTag || !flag)
		{
			this.Toggle();
		}
	}

	// Token: 0x06002E78 RID: 11896 RVA: 0x0010C758 File Offset: 0x0010A958
	private void GetContentsElement(out Tag element, out bool hasMass)
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
			element = contents.element.CreateTag();
			hasMass = (contents.mass > 0f);
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents2.pickupableHandle);
		KPrefabID kprefabID = (pickupable != null) ? pickupable.GetComponent<KPrefabID>() : null;
		if (kprefabID != null && pickupable.PrimaryElement.Mass > 0f)
		{
			element = kprefabID.PrefabTag;
			hasMass = true;
			return;
		}
		element = GameTags.Void;
		hasMass = false;
	}

	// Token: 0x04001B88 RID: 7048
	[MyCmpGet]
	private Filterable filterable;
}
