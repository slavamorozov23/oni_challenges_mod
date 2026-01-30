using System;
using UnityEngine;

// Token: 0x020007D5 RID: 2005
public class OrnamentReceptacle : SingleEntityReceptacle
{
	// Token: 0x17000354 RID: 852
	// (get) Token: 0x06003542 RID: 13634 RVA: 0x0012D0C0 File Offset: 0x0012B2C0
	public bool IsHoldingOrnament
	{
		get
		{
			return base.Occupant != null && base.Occupant.HasTag(GameTags.Ornament);
		}
	}

	// Token: 0x17000355 RID: 853
	// (get) Token: 0x06003543 RID: 13635 RVA: 0x0012D0E2 File Offset: 0x0012B2E2
	public bool IsOperational
	{
		get
		{
			return this.operational == null || this.operational.IsOperational;
		}
	}

	// Token: 0x06003544 RID: 13636 RVA: 0x0012D0FF File Offset: 0x0012B2FF
	protected override void OnPrefabInit()
	{
		this.ornamentDisabledStatusItem = Db.Get().BuildingStatusItems.OrnamentDisabled;
		this.noItemDisplayedStatusItem = Db.Get().BuildingStatusItems.PedestalNoItemDisplayed;
		base.OnPrefabInit();
	}

	// Token: 0x06003545 RID: 13637 RVA: 0x0012D134 File Offset: 0x0012B334
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.AddAdditionalCriteria((GameObject obj) => obj.HasTag(GameTags.PedestalDisplayable));
		if (base.occupyingObject == null && this.storage.MassStored() > 0f)
		{
			this.OnDepositObject(this.storage.items[0]);
			return;
		}
		this.RefreshDecorTag();
	}

	// Token: 0x06003546 RID: 13638 RVA: 0x0012D1AC File Offset: 0x0012B3AC
	protected override void ClearOccupant()
	{
		base.ClearOccupant();
		this.RefreshDecorTag();
		int cell = Grid.PosToCell(base.gameObject);
		Game.Instance.roomProber.TriggerBuildingChangedEvent(cell, base.gameObject);
	}

	// Token: 0x06003547 RID: 13639 RVA: 0x0012D1E8 File Offset: 0x0012B3E8
	protected override void OnDepositObject(GameObject depositedObject)
	{
		base.OnDepositObject(depositedObject);
		this.RefreshDecorTag();
		int cell = Grid.PosToCell(base.gameObject);
		Game.Instance.roomProber.TriggerBuildingChangedEvent(cell, base.gameObject);
	}

	// Token: 0x06003548 RID: 13640 RVA: 0x0012D224 File Offset: 0x0012B424
	protected override void OnOperationalChanged(object data)
	{
		base.OnOperationalChanged(data);
		this.RefreshDecorTag();
		int cell = Grid.PosToCell(base.gameObject);
		Game.Instance.roomProber.TriggerBuildingChangedEvent(cell, base.gameObject);
		base.UpdateStatusItem();
	}

	// Token: 0x06003549 RID: 13641 RVA: 0x0012D266 File Offset: 0x0012B466
	protected override void PositionOccupyingObject()
	{
		base.PositionOccupyingObject();
		this.refreshAnims = true;
	}

	// Token: 0x0600354A RID: 13642 RVA: 0x0012D278 File Offset: 0x0012B478
	public override void Render1000ms(float dt)
	{
		base.Render1000ms(dt);
		if (this.refreshAnims)
		{
			if (base.Occupant != null)
			{
				KBatchedAnimController component = base.occupyingObject.GetComponent<KBatchedAnimController>();
				component.enabled = false;
				component.enabled = true;
			}
			KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
			component2.enabled = false;
			component2.enabled = true;
			this.refreshAnims = false;
		}
	}

	// Token: 0x0600354B RID: 13643 RVA: 0x0012D2D4 File Offset: 0x0012B4D4
	protected override void UpdateStatusItem(KSelectable selectable)
	{
		base.UpdateStatusItem(selectable);
		if (this.operational != null && this.IsHoldingOrnament && !this.operational.IsOperational)
		{
			selectable.AddStatusItem(this.ornamentDisabledStatusItem, null);
		}
		else
		{
			selectable.RemoveStatusItem(this.ornamentDisabledStatusItem, false);
		}
		if (base.Occupant == null && (this.operational == null || this.operational.IsOperational))
		{
			selectable.AddStatusItem(this.noItemDisplayedStatusItem, null);
			return;
		}
		selectable.RemoveStatusItem(this.noItemDisplayedStatusItem, false);
	}

	// Token: 0x0600354C RID: 13644 RVA: 0x0012D378 File Offset: 0x0012B578
	public virtual void RefreshDecorTag()
	{
		KPrefabID component = base.gameObject.GetComponent<KPrefabID>();
		bool flag = component.HasTag(GameTags.Decoration);
		bool flag2 = base.Occupant != null && (this.operational == null || this.operational.IsOperational);
		if (flag2)
		{
			component.AddTag(GameTags.Decoration, false);
		}
		else
		{
			component.RemoveTag(GameTags.Decoration);
		}
		if (flag != flag2)
		{
			Game.Instance.roomProber.TriggerBuildingChangedEvent(Grid.PosToCell(base.gameObject), component);
		}
	}

	// Token: 0x0400203A RID: 8250
	protected StatusItem ornamentDisabledStatusItem;

	// Token: 0x0400203B RID: 8251
	protected StatusItem noItemDisplayedStatusItem;

	// Token: 0x0400203C RID: 8252
	private bool refreshAnims;
}
