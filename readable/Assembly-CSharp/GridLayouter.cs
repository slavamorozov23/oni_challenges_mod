using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D20 RID: 3360
public class GridLayouter
{
	// Token: 0x060067F6 RID: 26614 RVA: 0x00274028 File Offset: 0x00272228
	[Conditional("UNITY_EDITOR")]
	private void ValidateImportantFieldsAreSet()
	{
		global::Debug.Assert(this.minCellSize >= 0f, string.Format("[{0} Error] Minimum cell size is invalid. Given: {1}", "GridLayouter", this.minCellSize));
		global::Debug.Assert(this.maxCellSize >= 0f, string.Format("[{0} Error] Maximum cell size is invalid. Given: {1}", "GridLayouter", this.maxCellSize));
		global::Debug.Assert(this.targetGridLayouts != null, string.Format("[{0} Error] Target grid layout is invalid. Given: {1}", "GridLayouter", this.targetGridLayouts));
	}

	// Token: 0x060067F7 RID: 26615 RVA: 0x002740B8 File Offset: 0x002722B8
	public void CheckIfShouldResizeGrid()
	{
		Vector2 lhs = new Vector2((float)Screen.width, (float)Screen.height);
		if (lhs != this.oldScreenSize)
		{
			this.RequestGridResize();
		}
		this.oldScreenSize = lhs;
		float @float = KPlayerPrefs.GetFloat(KCanvasScaler.UIScalePrefKey);
		if (@float != this.oldScreenScale)
		{
			this.RequestGridResize();
		}
		this.oldScreenScale = @float;
		this.ResizeGridIfRequested();
	}

	// Token: 0x060067F8 RID: 26616 RVA: 0x0027411A File Offset: 0x0027231A
	public void RequestGridResize()
	{
		this.framesLeftToResizeGrid = 3;
	}

	// Token: 0x060067F9 RID: 26617 RVA: 0x00274123 File Offset: 0x00272323
	private void ResizeGridIfRequested()
	{
		if (this.framesLeftToResizeGrid > 0)
		{
			this.ImmediateSizeGridToScreenResolution();
			this.framesLeftToResizeGrid--;
			if (this.framesLeftToResizeGrid == 0 && this.OnSizeGridComplete != null)
			{
				this.OnSizeGridComplete();
			}
		}
	}

	// Token: 0x060067FA RID: 26618 RVA: 0x00274160 File Offset: 0x00272360
	public void ImmediateSizeGridToScreenResolution()
	{
		foreach (GridLayoutGroup gridLayoutGroup in this.targetGridLayouts)
		{
			float workingWidth = ((this.overrideParentForSizeReference != null) ? this.overrideParentForSizeReference.rect.size.x : gridLayoutGroup.transform.parent.rectTransform().rect.size.x) - (float)gridLayoutGroup.padding.left - (float)gridLayoutGroup.padding.right;
			float x = gridLayoutGroup.spacing.x;
			int num = GridLayouter.<ImmediateSizeGridToScreenResolution>g__GetCellCountToFit|12_1(this.maxCellSize, x, workingWidth) + 1;
			float num2;
			for (num2 = GridLayouter.<ImmediateSizeGridToScreenResolution>g__GetCellSize|12_0(workingWidth, x, num); num2 < this.minCellSize; num2 = Mathf.Min(this.maxCellSize, GridLayouter.<ImmediateSizeGridToScreenResolution>g__GetCellSize|12_0(workingWidth, x, num)))
			{
				num--;
				if (num <= 0)
				{
					num = 1;
					num2 = this.minCellSize;
					break;
				}
			}
			gridLayoutGroup.childAlignment = ((num == 1) ? TextAnchor.UpperCenter : TextAnchor.UpperLeft);
			gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			gridLayoutGroup.constraintCount = num;
			gridLayoutGroup.cellSize = Vector2.one * num2;
		}
	}

	// Token: 0x060067FC RID: 26620 RVA: 0x002742D6 File Offset: 0x002724D6
	[CompilerGenerated]
	internal static float <ImmediateSizeGridToScreenResolution>g__GetCellSize|12_0(float workingWidth, float spacingSize, int count)
	{
		return (workingWidth - (spacingSize * (float)count - 1f)) / (float)count;
	}

	// Token: 0x060067FD RID: 26621 RVA: 0x002742E8 File Offset: 0x002724E8
	[CompilerGenerated]
	internal static int <ImmediateSizeGridToScreenResolution>g__GetCellCountToFit|12_1(float cellSize, float spacingSize, float workingWidth)
	{
		int num = 0;
		for (float num2 = cellSize; num2 < workingWidth; num2 += cellSize + spacingSize)
		{
			num++;
		}
		return num;
	}

	// Token: 0x0400475B RID: 18267
	public float minCellSize = -1f;

	// Token: 0x0400475C RID: 18268
	public float maxCellSize = -1f;

	// Token: 0x0400475D RID: 18269
	public List<GridLayoutGroup> targetGridLayouts;

	// Token: 0x0400475E RID: 18270
	public RectTransform overrideParentForSizeReference;

	// Token: 0x0400475F RID: 18271
	public System.Action OnSizeGridComplete;

	// Token: 0x04004760 RID: 18272
	private Vector2 oldScreenSize;

	// Token: 0x04004761 RID: 18273
	private float oldScreenScale;

	// Token: 0x04004762 RID: 18274
	private int framesLeftToResizeGrid;
}
