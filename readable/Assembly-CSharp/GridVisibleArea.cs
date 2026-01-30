using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000981 RID: 2433
public class GridVisibleArea
{
	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x060045B9 RID: 17849 RVA: 0x0019365B File Offset: 0x0019185B
	public GridArea CurrentArea
	{
		get
		{
			return this.VisibleAreas[0];
		}
	}

	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x060045BA RID: 17850 RVA: 0x00193669 File Offset: 0x00191869
	public GridArea PreviousArea
	{
		get
		{
			return this.VisibleAreas[1];
		}
	}

	// Token: 0x170004EC RID: 1260
	// (get) Token: 0x060045BB RID: 17851 RVA: 0x00193677 File Offset: 0x00191877
	public GridArea PreviousPreviousArea
	{
		get
		{
			return this.VisibleAreas[2];
		}
	}

	// Token: 0x170004ED RID: 1261
	// (get) Token: 0x060045BC RID: 17852 RVA: 0x00193685 File Offset: 0x00191885
	public GridArea CurrentAreaExtended
	{
		get
		{
			return this.VisibleAreasExtended[0];
		}
	}

	// Token: 0x170004EE RID: 1262
	// (get) Token: 0x060045BD RID: 17853 RVA: 0x00193693 File Offset: 0x00191893
	public GridArea PreviousAreaExtended
	{
		get
		{
			return this.VisibleAreasExtended[1];
		}
	}

	// Token: 0x170004EF RID: 1263
	// (get) Token: 0x060045BE RID: 17854 RVA: 0x001936A1 File Offset: 0x001918A1
	public GridArea PreviousPreviousAreaExtended
	{
		get
		{
			return this.VisibleAreasExtended[2];
		}
	}

	// Token: 0x060045BF RID: 17855 RVA: 0x001936AF File Offset: 0x001918AF
	public GridVisibleArea()
	{
	}

	// Token: 0x060045C0 RID: 17856 RVA: 0x001936DA File Offset: 0x001918DA
	public GridVisibleArea(int padding)
	{
		this._padding = padding;
	}

	// Token: 0x060045C1 RID: 17857 RVA: 0x0019370C File Offset: 0x0019190C
	public void Update()
	{
		if (!this.debugFreezeVisibleArea)
		{
			this.VisibleAreas[2] = this.VisibleAreas[1];
			this.VisibleAreas[1] = this.VisibleAreas[0];
			this.VisibleAreas[0] = GridVisibleArea.GetVisibleArea();
		}
		if (!this.debugFreezeVisibleAreasExtended)
		{
			this.VisibleAreasExtended[2] = this.VisibleAreasExtended[1];
			this.VisibleAreasExtended[1] = this.VisibleAreasExtended[0];
			this.VisibleAreasExtended[0] = GridVisibleArea.GetVisibleAreaExtended(this._padding);
		}
		foreach (GridVisibleArea.Callback callback in this.Callbacks)
		{
			callback.OnUpdate();
		}
	}

	// Token: 0x060045C2 RID: 17858 RVA: 0x001937FC File Offset: 0x001919FC
	public void AddCallback(string name, System.Action on_update)
	{
		GridVisibleArea.Callback item = new GridVisibleArea.Callback
		{
			Name = name,
			OnUpdate = on_update
		};
		this.Callbacks.Add(item);
	}

	// Token: 0x060045C3 RID: 17859 RVA: 0x00193830 File Offset: 0x00191A30
	public void Run(Action<int> in_view)
	{
		if (in_view != null)
		{
			this.CurrentArea.Run(in_view);
		}
	}

	// Token: 0x060045C4 RID: 17860 RVA: 0x00193850 File Offset: 0x00191A50
	public void RunExtended(Action<int> in_view)
	{
		if (in_view != null)
		{
			this.CurrentAreaExtended.Run(in_view);
		}
	}

	// Token: 0x060045C5 RID: 17861 RVA: 0x00193870 File Offset: 0x00191A70
	public void Run(Action<int> outside_view, Action<int> inside_view, Action<int> inside_view_second_time)
	{
		if (outside_view != null)
		{
			this.PreviousArea.RunOnDifference(this.CurrentArea, outside_view);
		}
		if (inside_view != null)
		{
			this.CurrentArea.RunOnDifference(this.PreviousArea, inside_view);
		}
		if (inside_view_second_time != null)
		{
			this.PreviousArea.RunOnDifference(this.PreviousPreviousArea, inside_view_second_time);
		}
	}

	// Token: 0x060045C6 RID: 17862 RVA: 0x001938C8 File Offset: 0x00191AC8
	public void RunExtended(Action<int> outside_view, Action<int> inside_view, Action<int> inside_view_second_time)
	{
		if (outside_view != null)
		{
			this.PreviousAreaExtended.RunOnDifference(this.CurrentAreaExtended, outside_view);
		}
		if (inside_view != null)
		{
			this.CurrentAreaExtended.RunOnDifference(this.PreviousAreaExtended, inside_view);
		}
		if (inside_view_second_time != null)
		{
			this.PreviousAreaExtended.RunOnDifference(this.PreviousPreviousAreaExtended, inside_view_second_time);
		}
	}

	// Token: 0x060045C7 RID: 17863 RVA: 0x00193920 File Offset: 0x00191B20
	public void RunIfVisible(int cell, Action<int> action)
	{
		this.CurrentArea.RunIfInside(cell, action);
	}

	// Token: 0x060045C8 RID: 17864 RVA: 0x00193940 File Offset: 0x00191B40
	public void RunIfVisibleExtended(int cell, Action<int> action)
	{
		this.CurrentAreaExtended.RunIfInside(cell, action);
	}

	// Token: 0x060045C9 RID: 17865 RVA: 0x0019395D File Offset: 0x00191B5D
	public static GridArea GetVisibleArea()
	{
		return GridVisibleArea.GetVisibleAreaExtended(0);
	}

	// Token: 0x060045CA RID: 17866 RVA: 0x00193968 File Offset: 0x00191B68
	public static GridArea GetVisibleAreaExtended(int padding)
	{
		GridArea result = default(GridArea);
		Camera mainCamera = Game.MainCamera;
		if (mainCamera != null)
		{
			Vector3 vector = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, mainCamera.transform.GetPosition().z));
			Vector3 vector2 = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, mainCamera.transform.GetPosition().z));
			vector.x += (float)padding;
			vector.y += (float)padding;
			vector2.x -= (float)padding;
			vector2.y -= (float)padding;
			if (CameraController.Instance != null)
			{
				Vector2I vector2I;
				Vector2I vector2I2;
				CameraController.Instance.GetWorldCamera(out vector2I, out vector2I2);
				result.SetExtents(Math.Max((int)(vector2.x - 0.5f), vector2I.x), Math.Max((int)(vector2.y - 0.5f), vector2I.y), Math.Min((int)(vector.x + 1.5f), vector2I2.x + vector2I.x), Math.Min((int)(vector.y + 1.5f), vector2I2.y + vector2I.y));
			}
			else
			{
				result.SetExtents(Math.Max((int)(vector2.x - 0.5f), 0), Math.Max((int)(vector2.y - 0.5f), 0), Math.Min((int)(vector.x + 1.5f), Grid.WidthInCells), Math.Min((int)(vector.y + 1.5f), Grid.HeightInCells));
			}
		}
		return result;
	}

	// Token: 0x04002EFD RID: 12029
	private GridArea[] VisibleAreas = new GridArea[3];

	// Token: 0x04002EFE RID: 12030
	private GridArea[] VisibleAreasExtended = new GridArea[3];

	// Token: 0x04002EFF RID: 12031
	private List<GridVisibleArea.Callback> Callbacks = new List<GridVisibleArea.Callback>();

	// Token: 0x04002F00 RID: 12032
	public bool debugFreezeVisibleArea;

	// Token: 0x04002F01 RID: 12033
	public bool debugFreezeVisibleAreasExtended;

	// Token: 0x04002F02 RID: 12034
	private readonly int _padding;

	// Token: 0x020019EA RID: 6634
	public struct Callback
	{
		// Token: 0x04007F89 RID: 32649
		public System.Action OnUpdate;

		// Token: 0x04007F8A RID: 32650
		public string Name;
	}
}
