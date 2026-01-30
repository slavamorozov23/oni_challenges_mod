using System;
using UnityEngine;

// Token: 0x02000A75 RID: 2677
public class ObjectLayerListItem
{
	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x06004DCF RID: 19919 RVA: 0x001C3EA2 File Offset: 0x001C20A2
	// (set) Token: 0x06004DD0 RID: 19920 RVA: 0x001C3EAA File Offset: 0x001C20AA
	public ObjectLayerListItem previousItem { get; private set; }

	// Token: 0x17000550 RID: 1360
	// (get) Token: 0x06004DD1 RID: 19921 RVA: 0x001C3EB3 File Offset: 0x001C20B3
	// (set) Token: 0x06004DD2 RID: 19922 RVA: 0x001C3EBB File Offset: 0x001C20BB
	public ObjectLayerListItem nextItem { get; private set; }

	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x06004DD3 RID: 19923 RVA: 0x001C3EC4 File Offset: 0x001C20C4
	// (set) Token: 0x06004DD4 RID: 19924 RVA: 0x001C3ECC File Offset: 0x001C20CC
	public GameObject gameObject { get; private set; }

	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x06004DD5 RID: 19925 RVA: 0x001C3ED5 File Offset: 0x001C20D5
	// (set) Token: 0x06004DD6 RID: 19926 RVA: 0x001C3EDD File Offset: 0x001C20DD
	public Pickupable pickupable { get; private set; }

	// Token: 0x06004DD7 RID: 19927 RVA: 0x001C3EE6 File Offset: 0x001C20E6
	public ObjectLayerListItem(GameObject gameObject, Pickupable pickupable, ObjectLayer layer, int new_cell)
	{
		this.gameObject = gameObject;
		this.pickupable = pickupable;
		this.layer = layer;
		this.Refresh(new_cell);
	}

	// Token: 0x06004DD8 RID: 19928 RVA: 0x001C3F17 File Offset: 0x001C2117
	public void Clear()
	{
		this.Refresh(Grid.InvalidCell);
	}

	// Token: 0x06004DD9 RID: 19929 RVA: 0x001C3F28 File Offset: 0x001C2128
	public bool Refresh(int new_cell)
	{
		if (this.cell != new_cell)
		{
			if (this.cell != Grid.InvalidCell && Grid.Objects[this.cell, (int)this.layer] == this.gameObject)
			{
				GameObject value = null;
				if (this.nextItem != null && this.nextItem.gameObject != null)
				{
					value = this.nextItem.gameObject;
				}
				Grid.Objects[this.cell, (int)this.layer] = value;
			}
			if (this.previousItem != null)
			{
				this.previousItem.nextItem = this.nextItem;
			}
			if (this.nextItem != null)
			{
				this.nextItem.previousItem = this.previousItem;
			}
			this.previousItem = null;
			this.nextItem = null;
			this.cell = new_cell;
			if (this.cell != Grid.InvalidCell)
			{
				GameObject gameObject = Grid.Objects[this.cell, (int)this.layer];
				if (gameObject != null && gameObject != this.gameObject)
				{
					ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
					this.nextItem = objectLayerListItem;
					objectLayerListItem.previousItem = this;
				}
				Grid.Objects[this.cell, (int)this.layer] = this.gameObject;
			}
			return true;
		}
		return false;
	}

	// Token: 0x06004DDA RID: 19930 RVA: 0x001C406C File Offset: 0x001C226C
	public bool Update(int cell)
	{
		return this.Refresh(cell);
	}

	// Token: 0x040033D8 RID: 13272
	private int cell = Grid.InvalidCell;

	// Token: 0x040033D9 RID: 13273
	private ObjectLayer layer;
}
