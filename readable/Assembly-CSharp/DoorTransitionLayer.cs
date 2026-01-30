using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000668 RID: 1640
public class DoorTransitionLayer : TransitionDriver.InterruptOverrideLayer
{
	// Token: 0x060027B4 RID: 10164 RVA: 0x000E3CEC File Offset: 0x000E1EEC
	public DoorTransitionLayer(Navigator navigator) : base(navigator)
	{
		KBoxCollider2D component = navigator.GetComponent<KBoxCollider2D>();
		this.checkCellAbove = (component != null && component.size.y > 1f);
	}

	// Token: 0x060027B5 RID: 10165 RVA: 0x000E3D38 File Offset: 0x000E1F38
	private bool AreAllDoorsOpen()
	{
		foreach (INavDoor navDoor in this.doors)
		{
			if (navDoor != null && !navDoor.IsOpen())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060027B6 RID: 10166 RVA: 0x000E3D98 File Offset: 0x000E1F98
	protected override bool IsOverrideComplete()
	{
		return base.IsOverrideComplete() && this.AreAllDoorsOpen();
	}

	// Token: 0x060027B7 RID: 10167 RVA: 0x000E3DAC File Offset: 0x000E1FAC
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		if (this.doors.Count > 0)
		{
			return;
		}
		int cell = Grid.PosToCell(navigator);
		int cell2 = Grid.OffsetCell(cell, transition.x, transition.y);
		this.AddDoor(cell2);
		if (navigator.CurrentNavType != NavType.Tube && this.checkCellAbove)
		{
			this.AddDoor(Grid.CellAbove(cell2));
		}
		for (int i = 0; i < transition.navGridTransition.voidOffsets.Length; i++)
		{
			int cell3 = Grid.OffsetCell(cell, transition.navGridTransition.voidOffsets[i]);
			this.AddDoor(cell3);
		}
		if (this.doors.Count == 0)
		{
			return;
		}
		if (!this.AreAllDoorsOpen())
		{
			base.BeginTransition(navigator, transition);
			transition.anim = navigator.NavGrid.GetIdleAnim(navigator.CurrentNavType);
			transition.start = this.originalTransition.start;
			transition.end = this.originalTransition.start;
		}
		foreach (INavDoor navDoor in this.doors)
		{
			navDoor.Open();
		}
	}

	// Token: 0x060027B8 RID: 10168 RVA: 0x000E3ED8 File Offset: 0x000E20D8
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		if (this.doors.Count == 0)
		{
			return;
		}
		foreach (INavDoor navDoor in this.doors)
		{
			if (!navDoor.IsNullOrDestroyed())
			{
				navDoor.Close();
			}
		}
		this.doors.Clear();
	}

	// Token: 0x060027B9 RID: 10169 RVA: 0x000E3F54 File Offset: 0x000E2154
	private void AddDoor(int cell)
	{
		INavDoor door = this.GetDoor(cell);
		if (!door.IsNullOrDestroyed() && !this.doors.Contains(door))
		{
			this.doors.Add(door);
		}
	}

	// Token: 0x060027BA RID: 10170 RVA: 0x000E3F8C File Offset: 0x000E218C
	private INavDoor GetDoor(int cell)
	{
		if (!Grid.HasDoor[cell])
		{
			return null;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			INavDoor navDoor = gameObject.GetComponent<INavDoor>();
			if (navDoor == null)
			{
				navDoor = gameObject.GetSMI<INavDoor>();
			}
			if (navDoor != null && navDoor.isSpawned)
			{
				return navDoor;
			}
		}
		return null;
	}

	// Token: 0x04001756 RID: 5974
	private List<INavDoor> doors = new List<INavDoor>();

	// Token: 0x04001757 RID: 5975
	private bool checkCellAbove;
}
