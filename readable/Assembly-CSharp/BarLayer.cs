using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D19 RID: 3353
public class BarLayer : GraphLayer
{
	// Token: 0x1700078C RID: 1932
	// (get) Token: 0x060067D0 RID: 26576 RVA: 0x0027284F File Offset: 0x00270A4F
	public int bar_count
	{
		get
		{
			return this.bars.Count;
		}
	}

	// Token: 0x060067D1 RID: 26577 RVA: 0x0027285C File Offset: 0x00270A5C
	public void NewBar(int[] values, float x_position, string ID = "")
	{
		GameObject gameObject = Util.KInstantiateUI(this.prefab_bar, this.bar_container, true);
		if (ID == "")
		{
			ID = this.bars.Count.ToString();
		}
		gameObject.name = string.Format("bar_{0}", ID);
		GraphedBar component = gameObject.GetComponent<GraphedBar>();
		component.SetFormat(this.bar_formats[this.bars.Count % this.bar_formats.Length]);
		int[] array = new int[values.Length];
		for (int i = 0; i < values.Length; i++)
		{
			array[i] = (int)(base.graph.rectTransform().rect.height * base.graph.GetRelativeSize(new Vector2(0f, (float)values[i])).y);
		}
		component.SetValues(array, base.graph.GetRelativePosition(new Vector2(x_position, 0f)).x);
		this.bars.Add(component);
	}

	// Token: 0x060067D2 RID: 26578 RVA: 0x0027295C File Offset: 0x00270B5C
	public void ClearBars()
	{
		foreach (GraphedBar graphedBar in this.bars)
		{
			if (graphedBar != null && graphedBar.gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(graphedBar.gameObject);
			}
		}
		this.bars.Clear();
	}

	// Token: 0x04004738 RID: 18232
	public GameObject bar_container;

	// Token: 0x04004739 RID: 18233
	public GameObject prefab_bar;

	// Token: 0x0400473A RID: 18234
	public GraphedBarFormatting[] bar_formats;

	// Token: 0x0400473B RID: 18235
	private List<GraphedBar> bars = new List<GraphedBar>();
}
