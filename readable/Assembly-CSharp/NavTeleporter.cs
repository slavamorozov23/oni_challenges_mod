using System;
using System.Runtime.CompilerServices;
using TUNING;

// Token: 0x020007CC RID: 1996
public class NavTeleporter : KMonoBehaviour
{
	// Token: 0x060034D8 RID: 13528 RVA: 0x0012BAA5 File Offset: 0x00129CA5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<KPrefabID>().AddTag(GameTags.NavTeleporters, false);
		this.Register();
		this.cellChangeHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, NavTeleporter.OnCellChangedDispatcher, this, "NavTeleporterCellChanged");
	}

	// Token: 0x060034D9 RID: 13529 RVA: 0x0012BAE8 File Offset: 0x00129CE8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		int cell = this.GetCell();
		if (cell != Grid.InvalidCell)
		{
			Grid.HasNavTeleporter[cell] = false;
		}
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangeHandlerID);
		this.Deregister();
		Components.NavTeleporters.Remove(this);
	}

	// Token: 0x060034DA RID: 13530 RVA: 0x0012BB37 File Offset: 0x00129D37
	public void SetOverrideCell(int cell)
	{
		this.overrideCell = cell;
	}

	// Token: 0x060034DB RID: 13531 RVA: 0x0012BB40 File Offset: 0x00129D40
	public int GetCell()
	{
		if (this.overrideCell >= 0)
		{
			return this.overrideCell;
		}
		return Grid.OffsetCell(Grid.PosToCell(this), this.offset);
	}

	// Token: 0x060034DC RID: 13532 RVA: 0x0012BB64 File Offset: 0x00129D64
	public void TwoWayTarget(NavTeleporter nt)
	{
		if (this.target != null)
		{
			if (nt != null)
			{
				nt.SetTarget(null);
			}
			this.BreakLink();
		}
		this.target = nt;
		if (this.target != null)
		{
			this.SetLink();
			if (nt != null)
			{
				nt.SetTarget(this);
			}
		}
	}

	// Token: 0x060034DD RID: 13533 RVA: 0x0012BBC0 File Offset: 0x00129DC0
	public void EnableTwoWayTarget(bool enable)
	{
		if (enable)
		{
			this.target.SetLink();
			this.SetLink();
			return;
		}
		this.target.BreakLink();
		this.BreakLink();
	}

	// Token: 0x060034DE RID: 13534 RVA: 0x0012BBE8 File Offset: 0x00129DE8
	public void SetTarget(NavTeleporter nt)
	{
		if (this.target != null)
		{
			this.BreakLink();
		}
		this.target = nt;
		if (this.target != null)
		{
			this.SetLink();
		}
	}

	// Token: 0x060034DF RID: 13535 RVA: 0x0012BC1C File Offset: 0x00129E1C
	private void Register()
	{
		int cell = this.GetCell();
		if (!Grid.IsValidCell(cell))
		{
			this.lastRegisteredCell = Grid.InvalidCell;
			return;
		}
		Grid.HasNavTeleporter[cell] = true;
		Pathfinding.Instance.AddDirtyNavGridCell(cell);
		this.lastRegisteredCell = cell;
		if (this.target != null)
		{
			this.SetLink();
		}
	}

	// Token: 0x060034E0 RID: 13536 RVA: 0x0012BC78 File Offset: 0x00129E78
	private void SetLink()
	{
		int cell = this.target.GetCell();
		Pathfinding.Instance.GetNavGrid(DUPLICANTSTATS.STANDARD.BaseStats.NAV_GRID_NAME).teleportTransitions[this.lastRegisteredCell] = cell;
		Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
	}

	// Token: 0x060034E1 RID: 13537 RVA: 0x0012BCCC File Offset: 0x00129ECC
	public void Deregister()
	{
		if (this.lastRegisteredCell != Grid.InvalidCell)
		{
			this.BreakLink();
			Grid.HasNavTeleporter[this.lastRegisteredCell] = false;
			Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
			this.lastRegisteredCell = Grid.InvalidCell;
		}
	}

	// Token: 0x060034E2 RID: 13538 RVA: 0x0012BD18 File Offset: 0x00129F18
	private void BreakLink()
	{
		Pathfinding.Instance.GetNavGrid(DUPLICANTSTATS.STANDARD.BaseStats.NAV_GRID_NAME).teleportTransitions.Remove(this.lastRegisteredCell);
		Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
	}

	// Token: 0x060034E3 RID: 13539 RVA: 0x0012BD54 File Offset: 0x00129F54
	private void OnCellChanged()
	{
		this.Deregister();
		this.Register();
		if (this.target != null)
		{
			NavTeleporter component = this.target.GetComponent<NavTeleporter>();
			if (component != null)
			{
				component.SetTarget(this);
			}
		}
	}

	// Token: 0x04001FF8 RID: 8184
	private NavTeleporter target;

	// Token: 0x04001FF9 RID: 8185
	private int lastRegisteredCell = Grid.InvalidCell;

	// Token: 0x04001FFA RID: 8186
	public CellOffset offset;

	// Token: 0x04001FFB RID: 8187
	private int overrideCell = -1;

	// Token: 0x04001FFC RID: 8188
	private ulong cellChangeHandlerID;

	// Token: 0x04001FFD RID: 8189
	private static readonly Action<object> OnCellChangedDispatcher = delegate(object obj)
	{
		Unsafe.As<NavTeleporter>(obj).OnCellChanged();
	};
}
