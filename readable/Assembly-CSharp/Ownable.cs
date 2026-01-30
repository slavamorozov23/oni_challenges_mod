using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A85 RID: 2693
[SerializationConfig(MemberSerialization.OptIn)]
public class Ownable : Assignable, ISaveLoadable, IGameObjectEffectDescriptor
{
	// Token: 0x06004E41 RID: 20033 RVA: 0x001C77F8 File Offset: 0x001C59F8
	public override void Assign(IAssignableIdentity new_assignee)
	{
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (base.slot != null && new_assignee is MinionIdentity)
		{
			new_assignee = (new_assignee as MinionIdentity).assignableProxy.Get();
		}
		if (base.slot != null && new_assignee is StoredMinionIdentity)
		{
			new_assignee = (new_assignee as StoredMinionIdentity).assignableProxy.Get();
		}
		if (new_assignee is MinionAssignablesProxy)
		{
			AssignableSlotInstance slot = new_assignee.GetSoleOwner().GetComponent<Ownables>().GetSlot(base.slot);
			if (slot != null)
			{
				Assignable assignable = slot.assignable;
				if (assignable != null)
				{
					assignable.Unassign();
				}
			}
		}
		base.Assign(new_assignee);
	}

	// Token: 0x06004E42 RID: 20034 RVA: 0x001C7894 File Offset: 0x001C5A94
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateTint();
		this.UpdateStatusString();
		base.OnAssign += this.OnNewAssignment;
		if (this.assignee == null)
		{
			MinionStorage component = base.GetComponent<MinionStorage>();
			if (component)
			{
				List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
				if (storedMinionInfo.Count > 0)
				{
					Ref<KPrefabID> serializedMinion = storedMinionInfo[0].serializedMinion;
					if (serializedMinion != null && serializedMinion.GetId() != -1)
					{
						StoredMinionIdentity component2 = serializedMinion.Get().GetComponent<StoredMinionIdentity>();
						component2.ValidateProxy();
						this.Assign(component2);
					}
				}
			}
		}
	}

	// Token: 0x06004E43 RID: 20035 RVA: 0x001C791E File Offset: 0x001C5B1E
	private void OnNewAssignment(IAssignableIdentity assignables)
	{
		this.UpdateTint();
		this.UpdateStatusString();
	}

	// Token: 0x06004E44 RID: 20036 RVA: 0x001C792C File Offset: 0x001C5B2C
	private void UpdateTint()
	{
		if (this.tintWhenUnassigned)
		{
			KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
			if (component != null && component.HasBatchInstanceData)
			{
				component.TintColour = ((this.assignee == null) ? this.unownedTint : this.ownedTint);
				return;
			}
			KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
			if (component2 != null && component2.HasBatchInstanceData)
			{
				component2.TintColour = ((this.assignee == null) ? this.unownedTint : this.ownedTint);
			}
		}
	}

	// Token: 0x06004E45 RID: 20037 RVA: 0x001C79B4 File Offset: 0x001C5BB4
	private void UpdateStatusString()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		StatusItem status_item;
		if (this.assignee != null)
		{
			if (this.assignee is MinionIdentity)
			{
				status_item = Db.Get().BuildingStatusItems.AssignedTo;
			}
			else if (this.assignee is Room)
			{
				status_item = Db.Get().BuildingStatusItems.AssignedTo;
			}
			else
			{
				status_item = Db.Get().BuildingStatusItems.AssignedTo;
			}
		}
		else
		{
			status_item = Db.Get().BuildingStatusItems.Unassigned;
		}
		component.SetStatusItem(Db.Get().StatusItemCategories.Ownable, status_item, this);
	}

	// Token: 0x06004E46 RID: 20038 RVA: 0x001C7A54 File Offset: 0x001C5C54
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.ASSIGNEDDUPLICANT, UI.BUILDINGEFFECTS.TOOLTIPS.ASSIGNEDDUPLICANT, Descriptor.DescriptorType.Requirement);
		list.Add(item);
		return list;
	}

	// Token: 0x04003426 RID: 13350
	public bool tintWhenUnassigned = true;

	// Token: 0x04003427 RID: 13351
	private Color unownedTint = Color.gray;

	// Token: 0x04003428 RID: 13352
	private Color ownedTint = Color.white;
}
