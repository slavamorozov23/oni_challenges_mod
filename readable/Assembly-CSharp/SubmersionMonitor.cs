using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using STRINGS;
using UnityEngine;

// Token: 0x020008BD RID: 2237
[AddComponentMenu("KMonoBehaviour/scripts/SubmersionMonitor")]
public class SubmersionMonitor : KMonoBehaviour, IGameObjectEffectDescriptor, IWiltCause, ISim1000ms
{
	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x06003DA1 RID: 15777 RVA: 0x00158033 File Offset: 0x00156233
	public bool Dry
	{
		get
		{
			return this.dry;
		}
	}

	// Token: 0x06003DA2 RID: 15778 RVA: 0x0015803B File Offset: 0x0015623B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnMove();
		this.CheckDry();
		this.cellChangeMonitorHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, SubmersionMonitor.OnMoveDispatcher, this, "SubmersionMonitor.OnSpawn");
	}

	// Token: 0x06003DA3 RID: 15779 RVA: 0x00158070 File Offset: 0x00156270
	private void OnMove()
	{
		this.position = Grid.PosToCell(base.gameObject);
		if (this.partitionerEntry.IsValid())
		{
			GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, this.position);
		}
		else
		{
			Vector2I vector2I = Grid.PosToXY(base.transform.GetPosition());
			Extents extents = new Extents(vector2I.x, vector2I.y, 1, 2);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("DrowningMonitor.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
		}
		this.CheckDry();
	}

	// Token: 0x06003DA4 RID: 15780 RVA: 0x00158111 File Offset: 0x00156311
	private void OnDrawGizmosSelected()
	{
	}

	// Token: 0x06003DA5 RID: 15781 RVA: 0x00158113 File Offset: 0x00156313
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangeMonitorHandlerID);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06003DA6 RID: 15782 RVA: 0x0015813B File Offset: 0x0015633B
	public void Configure(float _maxStamina, float _staminaRegenRate, float _cellLiquidThreshold = 0.95f)
	{
		this.cellLiquidThreshold = _cellLiquidThreshold;
	}

	// Token: 0x06003DA7 RID: 15783 RVA: 0x00158144 File Offset: 0x00156344
	public void Sim1000ms(float dt)
	{
		this.CheckDry();
	}

	// Token: 0x06003DA8 RID: 15784 RVA: 0x0015814C File Offset: 0x0015634C
	private void CheckDry()
	{
		if (!this.IsCellSafe())
		{
			if (!this.dry)
			{
				this.dry = true;
				base.Trigger(-2057657673, null);
				return;
			}
		}
		else if (this.dry)
		{
			this.dry = false;
			base.Trigger(1555379996, null);
		}
	}

	// Token: 0x06003DA9 RID: 15785 RVA: 0x00158198 File Offset: 0x00156398
	public bool IsCellSafe()
	{
		int cell = Grid.PosToCell(base.gameObject);
		return Grid.IsValidCell(cell) && Grid.IsSubstantialLiquid(cell, this.cellLiquidThreshold);
	}

	// Token: 0x06003DAA RID: 15786 RVA: 0x001581CC File Offset: 0x001563CC
	private void OnLiquidChanged(object data)
	{
		this.CheckDry();
	}

	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x06003DAB RID: 15787 RVA: 0x001581D4 File Offset: 0x001563D4
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.DryingOut
			};
		}
	}

	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x06003DAC RID: 15788 RVA: 0x001581E0 File Offset: 0x001563E0
	public string WiltStateString
	{
		get
		{
			if (this.Dry)
			{
				return Db.Get().CreatureStatusItems.DryingOut.resolveStringCallback(CREATURES.STATUSITEMS.DRYINGOUT.NAME, this);
			}
			return "";
		}
	}

	// Token: 0x06003DAD RID: 15789 RVA: 0x00158214 File Offset: 0x00156414
	public void SetIncapacitated(bool state)
	{
	}

	// Token: 0x06003DAE RID: 15790 RVA: 0x00158216 File Offset: 0x00156416
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_SUBMERSION, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_SUBMERSION, Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x0400260B RID: 9739
	private int position;

	// Token: 0x0400260C RID: 9740
	private bool dry;

	// Token: 0x0400260D RID: 9741
	protected float cellLiquidThreshold = 0.2f;

	// Token: 0x0400260E RID: 9742
	private Extents extents;

	// Token: 0x0400260F RID: 9743
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04002610 RID: 9744
	private ulong cellChangeMonitorHandlerID;

	// Token: 0x04002611 RID: 9745
	private static readonly Action<object> OnMoveDispatcher = delegate(object obj)
	{
		Unsafe.As<SubmersionMonitor>(obj).OnMove();
	};
}
