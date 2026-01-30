using System;
using KSerialization;
using STRINGS;

// Token: 0x0200089F RID: 2207
public class EntombVulnerable : KMonoBehaviour, IWiltCause
{
	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x06003CB5 RID: 15541 RVA: 0x0015378A File Offset: 0x0015198A
	private OccupyArea occupyArea
	{
		get
		{
			if (this._occupyArea == null)
			{
				this._occupyArea = base.GetComponent<OccupyArea>();
			}
			return this._occupyArea;
		}
	}

	// Token: 0x17000426 RID: 1062
	// (get) Token: 0x06003CB6 RID: 15542 RVA: 0x001537AC File Offset: 0x001519AC
	public bool GetEntombed
	{
		get
		{
			return this.isEntombed;
		}
	}

	// Token: 0x06003CB7 RID: 15543 RVA: 0x001537B4 File Offset: 0x001519B4
	public void SetStatusItem(StatusItem si)
	{
		bool flag = this.showStatusItemOnEntombed;
		this.SetShowStatusItemOnEntombed(false);
		this.EntombedStatusItem = si;
		this.SetShowStatusItemOnEntombed(flag);
	}

	// Token: 0x06003CB8 RID: 15544 RVA: 0x001537E0 File Offset: 0x001519E0
	public void SetShowStatusItemOnEntombed(bool val)
	{
		this.showStatusItemOnEntombed = val;
		if (this.isEntombed && this.EntombedStatusItem != null)
		{
			if (this.showStatusItemOnEntombed)
			{
				this.selectable.AddStatusItem(this.EntombedStatusItem, null);
				return;
			}
			this.selectable.RemoveStatusItem(this.EntombedStatusItem, false);
		}
	}

	// Token: 0x17000427 RID: 1063
	// (get) Token: 0x06003CB9 RID: 15545 RVA: 0x00153833 File Offset: 0x00151A33
	public string WiltStateString
	{
		get
		{
			return Db.Get().CreatureStatusItems.Entombed.resolveStringCallback(CREATURES.STATUSITEMS.ENTOMBED.LINE_ITEM, base.gameObject);
		}
	}

	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x06003CBA RID: 15546 RVA: 0x0015385E File Offset: 0x00151A5E
	public WiltCondition.Condition[] Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Entombed
			};
		}
	}

	// Token: 0x06003CBB RID: 15547 RVA: 0x0015386C File Offset: 0x00151A6C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.EntombedStatusItem == null)
		{
			this.EntombedStatusItem = this.DefaultEntombedStatusItem;
		}
		this.partitionerEntry = GameScenePartitioner.Instance.Add("EntombVulnerable", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.CheckEntombed();
		if (this.isEntombed)
		{
			base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
			base.Trigger(-1089732772, BoxedBools.True);
		}
	}

	// Token: 0x06003CBC RID: 15548 RVA: 0x001538FE File Offset: 0x00151AFE
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06003CBD RID: 15549 RVA: 0x00153916 File Offset: 0x00151B16
	private void OnSolidChanged(object data)
	{
		this.CheckEntombed();
	}

	// Token: 0x06003CBE RID: 15550 RVA: 0x00153920 File Offset: 0x00151B20
	private void CheckEntombed()
	{
		int cell = Grid.PosToCell(base.gameObject.transform.GetPosition());
		if (!Grid.IsValidCell(cell))
		{
			return;
		}
		if (!this.IsCellSafe(cell))
		{
			if (!this.isEntombed)
			{
				this.isEntombed = true;
				if (this.showStatusItemOnEntombed)
				{
					this.selectable.AddStatusItem(this.EntombedStatusItem, base.gameObject);
				}
				base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
				base.Trigger(-1089732772, BoxedBools.True);
			}
		}
		else if (this.isEntombed)
		{
			this.isEntombed = false;
			this.selectable.RemoveStatusItem(this.EntombedStatusItem, false);
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
			base.Trigger(-1089732772, BoxedBools.False);
		}
		if (this.operational != null)
		{
			this.operational.SetFlag(EntombVulnerable.notEntombedFlag, !this.isEntombed);
		}
	}

	// Token: 0x06003CBF RID: 15551 RVA: 0x00153A13 File Offset: 0x00151C13
	public bool IsCellSafe(int cell)
	{
		return this.occupyArea.TestArea(cell, null, EntombVulnerable.IsCellSafeCBDelegate);
	}

	// Token: 0x06003CC0 RID: 15552 RVA: 0x00153A27 File Offset: 0x00151C27
	private static bool IsCellSafeCB(int cell, object data)
	{
		return Grid.IsValidCell(cell) && !Grid.Solid[cell];
	}

	// Token: 0x0400257E RID: 9598
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x0400257F RID: 9599
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002580 RID: 9600
	private OccupyArea _occupyArea;

	// Token: 0x04002581 RID: 9601
	[Serialize]
	private bool isEntombed;

	// Token: 0x04002582 RID: 9602
	private StatusItem DefaultEntombedStatusItem = Db.Get().CreatureStatusItems.Entombed;

	// Token: 0x04002583 RID: 9603
	[NonSerialized]
	private StatusItem EntombedStatusItem;

	// Token: 0x04002584 RID: 9604
	private bool showStatusItemOnEntombed = true;

	// Token: 0x04002585 RID: 9605
	public static readonly Operational.Flag notEntombedFlag = new Operational.Flag("not_entombed", Operational.Flag.Type.Functional);

	// Token: 0x04002586 RID: 9606
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04002587 RID: 9607
	private static readonly Func<int, object, bool> IsCellSafeCBDelegate = (int cell, object data) => EntombVulnerable.IsCellSafeCB(cell, data);
}
