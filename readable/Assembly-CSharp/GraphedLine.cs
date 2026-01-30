using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

// Token: 0x02000D1D RID: 3357
[AddComponentMenu("KMonoBehaviour/scripts/GraphedLine")]
[Serializable]
public class GraphedLine : KMonoBehaviour
{
	// Token: 0x1700078E RID: 1934
	// (get) Token: 0x060067DA RID: 26586 RVA: 0x00272B6D File Offset: 0x00270D6D
	public int PointCount
	{
		get
		{
			return this.points.Length;
		}
	}

	// Token: 0x060067DB RID: 26587 RVA: 0x00272B77 File Offset: 0x00270D77
	public void SetPoints(Vector2[] points)
	{
		this.points = points;
		this.UpdatePoints();
	}

	// Token: 0x060067DC RID: 26588 RVA: 0x00272B88 File Offset: 0x00270D88
	private void UpdatePoints()
	{
		Vector2[] array = new Vector2[this.points.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.layer.graph.GetRelativePosition(this.points[i]);
		}
		this.line_renderer.Points = array;
	}

	// Token: 0x060067DD RID: 26589 RVA: 0x00272BE0 File Offset: 0x00270DE0
	public Vector2 GetClosestDataToPointOnXAxis(Vector2 toPoint)
	{
		float num = toPoint.x / this.layer.graph.rectTransform().sizeDelta.x;
		float num2 = this.layer.graph.axis_x.min_value + this.layer.graph.axis_x.range * num;
		Vector2 vector = Vector2.zero;
		foreach (Vector2 vector2 in this.points)
		{
			if (Mathf.Abs(vector2.x - num2) < Mathf.Abs(vector.x - num2))
			{
				vector = vector2;
			}
		}
		return vector;
	}

	// Token: 0x060067DE RID: 26590 RVA: 0x00272C87 File Offset: 0x00270E87
	public void HidePointHighlight()
	{
		if (this.highlightPoint != null)
		{
			this.highlightPoint.SetActive(false);
		}
	}

	// Token: 0x060067DF RID: 26591 RVA: 0x00272CA4 File Offset: 0x00270EA4
	public void SetPointHighlight(Vector2 point)
	{
		if (this.highlightPoint == null)
		{
			return;
		}
		this.highlightPoint.SetActive(true);
		Vector2 relativePosition = this.layer.graph.GetRelativePosition(point);
		this.highlightPoint.rectTransform().SetLocalPosition(new Vector2(relativePosition.x * this.layer.graph.rectTransform().sizeDelta.x - this.layer.graph.rectTransform().sizeDelta.x / 2f, relativePosition.y * this.layer.graph.rectTransform().sizeDelta.y - this.layer.graph.rectTransform().sizeDelta.y / 2f));
		ToolTip component = this.layer.graph.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		component.tooltipPositionOffset = new Vector2(this.highlightPoint.rectTransform().localPosition.x, this.layer.graph.rectTransform().rect.height / 2f - 12f);
		component.SetSimpleTooltip(string.Concat(new string[]
		{
			this.layer.graph.axis_x.name,
			" ",
			point.x.ToString(),
			", ",
			Mathf.RoundToInt(point.y).ToString(),
			" ",
			this.layer.graph.axis_y.name
		}));
		ToolTipScreen.Instance.SetToolTip(component);
	}

	// Token: 0x04004743 RID: 18243
	public UILineRenderer line_renderer;

	// Token: 0x04004744 RID: 18244
	public LineLayer layer;

	// Token: 0x04004745 RID: 18245
	private Vector2[] points = new Vector2[0];

	// Token: 0x04004746 RID: 18246
	[SerializeField]
	private GameObject highlightPoint;
}
