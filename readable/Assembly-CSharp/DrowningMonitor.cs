using System;
using System.Runtime.CompilerServices;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000899 RID: 2201
[AddComponentMenu("KMonoBehaviour/scripts/DrowningMonitor")]
public class DrowningMonitor : KMonoBehaviour, IWiltCause, ISlicedSim1000ms
{
	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x06003C8A RID: 15498 RVA: 0x001528E3 File Offset: 0x00150AE3
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

	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x06003C8B RID: 15499 RVA: 0x00152905 File Offset: 0x00150B05
	public bool Drowning
	{
		get
		{
			return this.drowning;
		}
	}

	// Token: 0x06003C8C RID: 15500 RVA: 0x00152910 File Offset: 0x00150B10
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.timeToDrown = 75f;
		if (DrowningMonitor.drowningEffect == null)
		{
			DrowningMonitor.drowningEffect = new Effect("Drowning", CREATURES.STATUSITEMS.DROWNING.NAME, CREATURES.STATUSITEMS.DROWNING.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
			DrowningMonitor.drowningEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -100f, CREATURES.STATUSITEMS.DROWNING.NAME, false, false, true));
		}
		if (DrowningMonitor.saturatedEffect == null)
		{
			DrowningMonitor.saturatedEffect = new Effect("Saturated", CREATURES.STATUSITEMS.SATURATED.NAME, CREATURES.STATUSITEMS.SATURATED.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
			DrowningMonitor.saturatedEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -100f, CREATURES.STATUSITEMS.SATURATED.NAME, false, false, true));
		}
	}

	// Token: 0x06003C8D RID: 15501 RVA: 0x00152A20 File Offset: 0x00150C20
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SlicedUpdaterSim1000ms<DrowningMonitor>.instance.RegisterUpdate1000ms(this);
		this.OnMove();
		this.CheckDrowning(null);
		this.cellChangedHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, DrowningMonitor.OnMoveDispatcher, this, "DrowningMonitor.OnSpawn");
	}

	// Token: 0x06003C8E RID: 15502 RVA: 0x00152A6C File Offset: 0x00150C6C
	private void OnMove()
	{
		if (this.partitionerEntry.IsValid())
		{
			Extents ext = this.occupyArea.GetExtents();
			GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, ext);
		}
		else
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add("DrowningMonitor.OnSpawn", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
		}
		this.CheckDrowning(null);
	}

	// Token: 0x06003C8F RID: 15503 RVA: 0x00152AE8 File Offset: 0x00150CE8
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		SlicedUpdaterSim1000ms<DrowningMonitor>.instance.UnregisterUpdate1000ms(this);
		base.OnCleanUp();
	}

	// Token: 0x06003C90 RID: 15504 RVA: 0x00152B1C File Offset: 0x00150D1C
	private void CheckDrowning(object data = null)
	{
		if (this.drowned)
		{
			return;
		}
		int cell = Grid.PosToCell(base.gameObject.transform.GetPosition());
		if (!this.IsCellSafe(cell))
		{
			if (!this.drowning)
			{
				this.drowning = true;
				base.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Drowning, false);
				base.Trigger(1949704522, null);
			}
			if (this.timeToDrown <= 0f && this.canDrownToDeath)
			{
				DeathMonitor.Instance smi = this.GetSMI<DeathMonitor.Instance>();
				if (smi != null)
				{
					smi.Kill(Db.Get().Deaths.Drowned);
				}
				base.Trigger(-750750377, null);
				this.drowned = true;
			}
		}
		else if (this.drowning)
		{
			this.drowning = false;
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.Drowning);
			base.Trigger(99949694, null);
		}
		if (this.livesUnderWater)
		{
			this.saturatedStatusGuid = this.selectable.ToggleStatusItem(Db.Get().CreatureStatusItems.Saturated, this.saturatedStatusGuid, this.drowning, this);
		}
		else
		{
			this.drowningStatusGuid = this.selectable.ToggleStatusItem(Db.Get().CreatureStatusItems.Drowning, this.drowningStatusGuid, this.drowning, this);
		}
		if (this.effects != null)
		{
			if (this.drowning)
			{
				if (this.livesUnderWater)
				{
					this.effects.Add(DrowningMonitor.saturatedEffect, false);
					return;
				}
				this.effects.Add(DrowningMonitor.drowningEffect, false);
				return;
			}
			else
			{
				if (this.livesUnderWater)
				{
					this.effects.Remove(DrowningMonitor.saturatedEffect);
					return;
				}
				this.effects.Remove(DrowningMonitor.drowningEffect);
			}
		}
	}

	// Token: 0x06003C91 RID: 15505 RVA: 0x00152CC2 File Offset: 0x00150EC2
	private static bool CellSafeTest(int testCell, object data)
	{
		return !Grid.IsNavigatableLiquid(testCell);
	}

	// Token: 0x06003C92 RID: 15506 RVA: 0x00152CCD File Offset: 0x00150ECD
	public bool IsCellSafe(int cell)
	{
		return this.occupyArea.TestArea(cell, this, DrowningMonitor.CellSafeTestDelegate);
	}

	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x06003C93 RID: 15507 RVA: 0x00152CE1 File Offset: 0x00150EE1
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Drowning
			};
		}
	}

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x06003C94 RID: 15508 RVA: 0x00152CED File Offset: 0x00150EED
	public string WiltStateString
	{
		get
		{
			if (this.livesUnderWater)
			{
				return "    • " + CREATURES.STATUSITEMS.SATURATED.NAME;
			}
			return "    • " + CREATURES.STATUSITEMS.DROWNING.NAME;
		}
	}

	// Token: 0x06003C95 RID: 15509 RVA: 0x00152D20 File Offset: 0x00150F20
	private void OnLiquidChanged(object data)
	{
		this.CheckDrowning(null);
	}

	// Token: 0x06003C96 RID: 15510 RVA: 0x00152D2C File Offset: 0x00150F2C
	public void SlicedSim1000ms(float dt)
	{
		this.CheckDrowning(null);
		if (this.drowning)
		{
			if (!this.drowned)
			{
				this.timeToDrown -= dt;
				if (this.timeToDrown <= 0f)
				{
					this.CheckDrowning(null);
					return;
				}
			}
		}
		else
		{
			this.timeToDrown += dt * 5f;
			this.timeToDrown = Mathf.Clamp(this.timeToDrown, 0f, 75f);
		}
	}

	// Token: 0x04002554 RID: 9556
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002555 RID: 9557
	[MyCmpGet]
	private Effects effects;

	// Token: 0x04002556 RID: 9558
	private OccupyArea _occupyArea;

	// Token: 0x04002557 RID: 9559
	[Serialize]
	[SerializeField]
	private float timeToDrown;

	// Token: 0x04002558 RID: 9560
	[Serialize]
	private bool drowned;

	// Token: 0x04002559 RID: 9561
	private bool drowning;

	// Token: 0x0400255A RID: 9562
	protected const float MaxDrownTime = 75f;

	// Token: 0x0400255B RID: 9563
	protected const float RegenRate = 5f;

	// Token: 0x0400255C RID: 9564
	protected const float CellLiquidThreshold = 0.95f;

	// Token: 0x0400255D RID: 9565
	public bool canDrownToDeath = true;

	// Token: 0x0400255E RID: 9566
	public bool livesUnderWater;

	// Token: 0x0400255F RID: 9567
	private Guid drowningStatusGuid;

	// Token: 0x04002560 RID: 9568
	private Guid saturatedStatusGuid;

	// Token: 0x04002561 RID: 9569
	private Extents extents;

	// Token: 0x04002562 RID: 9570
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04002563 RID: 9571
	public static Effect drowningEffect;

	// Token: 0x04002564 RID: 9572
	public static Effect saturatedEffect;

	// Token: 0x04002565 RID: 9573
	private ulong cellChangedHandlerID;

	// Token: 0x04002566 RID: 9574
	private static readonly Action<object> OnMoveDispatcher = delegate(object obj)
	{
		Unsafe.As<DrowningMonitor>(obj).OnMove();
	};

	// Token: 0x04002567 RID: 9575
	private static readonly Func<int, object, bool> CellSafeTestDelegate = (int testCell, object data) => DrowningMonitor.CellSafeTest(testCell, data);
}
