using System;

// Token: 0x02000C18 RID: 3096
public class VisibleAreaUpdater
{
	// Token: 0x06005D1D RID: 23837 RVA: 0x0021B4BA File Offset: 0x002196BA
	public VisibleAreaUpdater(Action<int> outside_view_first_time_cb, Action<int> inside_view_first_time_cb, string name)
	{
		this.OutsideViewFirstTimeCallback = outside_view_first_time_cb;
		this.InsideViewFirstTimeCallback = inside_view_first_time_cb;
		this.UpdateCallback = new Action<int>(this.InternalUpdateCell);
		this.Name = name;
	}

	// Token: 0x06005D1E RID: 23838 RVA: 0x0021B4E9 File Offset: 0x002196E9
	public void Update()
	{
		if (CameraController.Instance != null && this.VisibleArea == null)
		{
			this.VisibleArea = CameraController.Instance.VisibleArea;
			this.VisibleArea.Run(this.InsideViewFirstTimeCallback);
		}
	}

	// Token: 0x06005D1F RID: 23839 RVA: 0x0021B521 File Offset: 0x00219721
	private void InternalUpdateCell(int cell)
	{
		this.OutsideViewFirstTimeCallback(cell);
		this.InsideViewFirstTimeCallback(cell);
	}

	// Token: 0x06005D20 RID: 23840 RVA: 0x0021B53B File Offset: 0x0021973B
	public void UpdateCell(int cell)
	{
		if (this.VisibleArea != null)
		{
			this.VisibleArea.RunIfVisible(cell, this.UpdateCallback);
		}
	}

	// Token: 0x04003E00 RID: 15872
	private GridVisibleArea VisibleArea;

	// Token: 0x04003E01 RID: 15873
	private Action<int> OutsideViewFirstTimeCallback;

	// Token: 0x04003E02 RID: 15874
	private Action<int> InsideViewFirstTimeCallback;

	// Token: 0x04003E03 RID: 15875
	private Action<int> UpdateCallback;

	// Token: 0x04003E04 RID: 15876
	private string Name;
}
