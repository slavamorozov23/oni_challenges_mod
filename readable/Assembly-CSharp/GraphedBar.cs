using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D1A RID: 3354
[AddComponentMenu("KMonoBehaviour/scripts/GraphedBar")]
[Serializable]
public class GraphedBar : KMonoBehaviour
{
	// Token: 0x060067D4 RID: 26580 RVA: 0x002729EB File Offset: 0x00270BEB
	public void SetFormat(GraphedBarFormatting format)
	{
		this.format = format;
	}

	// Token: 0x060067D5 RID: 26581 RVA: 0x002729F4 File Offset: 0x00270BF4
	public void SetValues(int[] values, float x_position)
	{
		this.ClearValues();
		base.gameObject.rectTransform().anchorMin = new Vector2(x_position, 0f);
		base.gameObject.rectTransform().anchorMax = new Vector2(x_position, 1f);
		base.gameObject.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)this.format.width);
		for (int i = 0; i < values.Length; i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.prefab_segment, this.segments_container, true);
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.preferredHeight = (float)values[i];
			component.minWidth = (float)this.format.width;
			gameObject.GetComponent<Image>().color = this.format.colors[i % this.format.colors.Length];
			this.segments.Add(gameObject);
		}
	}

	// Token: 0x060067D6 RID: 26582 RVA: 0x00272AD4 File Offset: 0x00270CD4
	public void ClearValues()
	{
		foreach (GameObject obj in this.segments)
		{
			UnityEngine.Object.DestroyImmediate(obj);
		}
		this.segments.Clear();
	}

	// Token: 0x0400473C RID: 18236
	public GameObject segments_container;

	// Token: 0x0400473D RID: 18237
	public GameObject prefab_segment;

	// Token: 0x0400473E RID: 18238
	private List<GameObject> segments = new List<GameObject>();

	// Token: 0x0400473F RID: 18239
	private GraphedBarFormatting format;
}
