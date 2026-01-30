using System;
using UnityEngine;

// Token: 0x020006B8 RID: 1720
public class DevToolWarmthZonesVisualizer : DevTool
{
	// Token: 0x06002A57 RID: 10839 RVA: 0x000F7DDC File Offset: 0x000F5FDC
	private void SetupColors()
	{
		if (this.colors == null)
		{
			this.colors = new Color[3];
			for (int i = 1; i <= 3; i++)
			{
				this.colors[i - 1] = this.CreateColorForWarmthValue(i);
			}
		}
	}

	// Token: 0x06002A58 RID: 10840 RVA: 0x000F7E20 File Offset: 0x000F6020
	private Color CreateColorForWarmthValue(int warmValue)
	{
		float b = (float)Mathf.Clamp(warmValue, 1, 3) / 3f;
		Color result = this.WARM_CELL_COLOR * b;
		result.a = this.WARM_CELL_COLOR.a;
		return result;
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x000F7E60 File Offset: 0x000F6060
	private Color GetBorderColor(int warmValue)
	{
		int num = Mathf.Clamp(warmValue, 0, 3);
		return this.colors[num];
	}

	// Token: 0x06002A5A RID: 10842 RVA: 0x000F7E84 File Offset: 0x000F6084
	private Color GetFillColor(int warmValue)
	{
		Color borderColor = this.GetBorderColor(warmValue);
		borderColor.a = 0.3f;
		return borderColor;
	}

	// Token: 0x06002A5B RID: 10843 RVA: 0x000F7EA8 File Offset: 0x000F60A8
	protected override void RenderTo(DevPanel panel)
	{
		this.SetupColors();
		foreach (int num in WarmthProvider.WarmCells.Keys)
		{
			if (Grid.IsValidCell(num) && WarmthProvider.IsWarmCell(num))
			{
				int warmthValue = WarmthProvider.GetWarmthValue(num);
				Option<ValueTuple<Vector2, Vector2>> screenRect = new DevToolEntityTarget.ForSimCell(num).GetScreenRect();
				string value = warmthValue.ToString();
				DevToolEntity.DrawScreenRect(screenRect.Unwrap(), value, this.GetBorderColor(warmthValue - 1), this.GetFillColor(warmthValue - 1), new Option<DevToolUtil.TextAlignment>(DevToolUtil.TextAlignment.Center));
			}
		}
	}

	// Token: 0x0400191F RID: 6431
	private const int MAX_COLOR_VARIANTS = 3;

	// Token: 0x04001920 RID: 6432
	private Color WARM_CELL_COLOR = Color.red;

	// Token: 0x04001921 RID: 6433
	private Color[] colors;
}
