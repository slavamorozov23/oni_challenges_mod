using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200059E RID: 1438
public class BaggableCritterCapacityTracker : KMonoBehaviour, ISim1000ms, IUserControlledCapacity
{
	// Token: 0x1700012D RID: 301
	// (get) Token: 0x0600204A RID: 8266 RVA: 0x000BA25B File Offset: 0x000B845B
	// (set) Token: 0x0600204B RID: 8267 RVA: 0x000BA263 File Offset: 0x000B8463
	[Serialize]
	public int creatureLimit { get; set; } = 20;

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x0600204C RID: 8268 RVA: 0x000BA26C File Offset: 0x000B846C
	// (set) Token: 0x0600204D RID: 8269 RVA: 0x000BA274 File Offset: 0x000B8474
	public int storedCreatureCount { get; private set; }

	// Token: 0x0600204E RID: 8270 RVA: 0x000BA280 File Offset: 0x000B8480
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(this);
		this.cavityCell = Grid.OffsetCell(cell, this.cavityOffset);
		this.filter = base.GetComponent<TreeFilterable>();
		TreeFilterable treeFilterable = this.filter;
		treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.RefreshCreatureCount));
		base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
		if (this.requireLiquidOffset)
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add("BaggableCritterCapacityTracker.OnSpawn", base.gameObject, new Extents(this.cavityCell, new CellOffset[]
			{
				new CellOffset(0, 0)
			}), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
			this.OnLiquidChanged(null);
			return;
		}
		base.Subscribe(144050788, new Action<object>(this.RefreshCreatureCount));
	}

	// Token: 0x0600204F RID: 8271 RVA: 0x000BA370 File Offset: 0x000B8570
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (BaggableCritterCapacityTracker.capacityStatusItem == null)
		{
			BaggableCritterCapacityTracker.capacityStatusItem = new StatusItem("CritterCapacity", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			BaggableCritterCapacityTracker.capacityStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				IUserControlledCapacity userControlledCapacity = (IUserControlledCapacity)data;
				string newValue = Util.FormatWholeNumber(Mathf.Floor(userControlledCapacity.AmountStored));
				string newValue2 = Util.FormatWholeNumber(userControlledCapacity.UserMaxCapacity);
				str = str.Replace("{Stored}", newValue).Replace("{StoredUnits}", ((int)userControlledCapacity.AmountStored == 1) ? BUILDING.STATUSITEMS.CRITTERCAPACITY.UNIT : BUILDING.STATUSITEMS.CRITTERCAPACITY.UNITS).Replace("{Capacity}", newValue2).Replace("{CapacityUnits}", ((int)userControlledCapacity.UserMaxCapacity == 1) ? BUILDING.STATUSITEMS.CRITTERCAPACITY.UNIT : BUILDING.STATUSITEMS.CRITTERCAPACITY.UNITS);
				return str;
			};
		}
		this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, BaggableCritterCapacityTracker.capacityStatusItem, this);
	}

	// Token: 0x06002050 RID: 8272 RVA: 0x000BA3FC File Offset: 0x000B85FC
	protected override void OnCleanUp()
	{
		if (this.requireLiquidOffset)
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		}
		TreeFilterable treeFilterable = this.filter;
		treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.RefreshCreatureCount));
		base.Unsubscribe(144050788);
		base.OnCleanUp();
	}

	// Token: 0x06002051 RID: 8273 RVA: 0x000BA45C File Offset: 0x000B865C
	private void OnLiquidChanged(object data)
	{
		if (this.requireLiquidOffset)
		{
			bool flag = Grid.IsLiquid(this.cavityCell);
			if (flag)
			{
				this.RefreshCreatureCount(null);
			}
			this.operational.SetFlag(BaggableCritterCapacityTracker.isInLiquid, flag);
			this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NotSubmerged, !flag, this);
			this.selectable.ToggleStatusItem(BaggableCritterCapacityTracker.capacityStatusItem, flag, this);
		}
	}

	// Token: 0x06002052 RID: 8274 RVA: 0x000BA4CC File Offset: 0x000B86CC
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		BaggableCritterCapacityTracker component = gameObject.GetComponent<BaggableCritterCapacityTracker>();
		if (component == null)
		{
			return;
		}
		this.creatureLimit = component.creatureLimit;
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x000BA508 File Offset: 0x000B8708
	public void RefreshCreatureCount(object data = null)
	{
		int storedCreatureCount = this.storedCreatureCount;
		if (this.requireLiquidOffset)
		{
			this.storedCreatureCount = this.RefreshSwimmingCreatureCount();
		}
		else
		{
			this.storedCreatureCount = this.RefreshOtherCreatureCount();
		}
		if (this.onCountChanged != null && this.storedCreatureCount != storedCreatureCount)
		{
			this.onCountChanged();
		}
	}

	// Token: 0x06002054 RID: 8276 RVA: 0x000BA55C File Offset: 0x000B875C
	private int RefreshOtherCreatureCount()
	{
		int num = 0;
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.cavityCell);
		if (cavityForCell != null)
		{
			foreach (KPrefabID kprefabID in cavityForCell.creatures)
			{
				if (!kprefabID.HasTag(GameTags.Creatures.Bagged) && !kprefabID.HasTag(GameTags.Trapped) && (!this.filteredCount || this.filter.AcceptedTags.Contains(kprefabID.PrefabTag)))
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06002055 RID: 8277 RVA: 0x000BA604 File Offset: 0x000B8804
	private int RefreshSwimmingCreatureCount()
	{
		return FishOvercrowingManager.Instance.GetFishInPondCount(this.cavityCell, this.filter.AcceptedTags);
	}

	// Token: 0x06002056 RID: 8278 RVA: 0x000BA621 File Offset: 0x000B8821
	public void Sim1000ms(float dt)
	{
		this.RefreshCreatureCount(null);
	}

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x06002057 RID: 8279 RVA: 0x000BA62A File Offset: 0x000B882A
	// (set) Token: 0x06002058 RID: 8280 RVA: 0x000BA633 File Offset: 0x000B8833
	float IUserControlledCapacity.UserMaxCapacity
	{
		get
		{
			return (float)this.creatureLimit;
		}
		set
		{
			this.creatureLimit = Mathf.RoundToInt(value);
			if (this.onCountChanged != null)
			{
				this.onCountChanged();
			}
		}
	}

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06002059 RID: 8281 RVA: 0x000BA654 File Offset: 0x000B8854
	float IUserControlledCapacity.AmountStored
	{
		get
		{
			return (float)this.storedCreatureCount;
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x0600205A RID: 8282 RVA: 0x000BA65D File Offset: 0x000B885D
	float IUserControlledCapacity.MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x0600205B RID: 8283 RVA: 0x000BA664 File Offset: 0x000B8864
	float IUserControlledCapacity.MaxCapacity
	{
		get
		{
			return (float)this.maximumCreatures;
		}
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x0600205C RID: 8284 RVA: 0x000BA66D File Offset: 0x000B886D
	bool IUserControlledCapacity.WholeValues
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x0600205D RID: 8285 RVA: 0x000BA670 File Offset: 0x000B8870
	LocString IUserControlledCapacity.CapacityUnits
	{
		get
		{
			return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.UNITS_SUFFIX;
		}
	}

	// Token: 0x040012CB RID: 4811
	public int maximumCreatures = 40;

	// Token: 0x040012CC RID: 4812
	public bool requireLiquidOffset;

	// Token: 0x040012CD RID: 4813
	public CellOffset cavityOffset;

	// Token: 0x040012CE RID: 4814
	public bool filteredCount;

	// Token: 0x040012CF RID: 4815
	public System.Action onCountChanged;

	// Token: 0x040012D0 RID: 4816
	private int cavityCell;

	// Token: 0x040012D1 RID: 4817
	[MyCmpReq]
	private TreeFilterable filter;

	// Token: 0x040012D2 RID: 4818
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040012D3 RID: 4819
	private static readonly Operational.Flag isInLiquid = new Operational.Flag("isInLiquid", Operational.Flag.Type.Requirement);

	// Token: 0x040012D4 RID: 4820
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x040012D5 RID: 4821
	private static StatusItem capacityStatusItem;

	// Token: 0x040012D6 RID: 4822
	private HandleVector<int>.Handle partitionerEntry;
}
