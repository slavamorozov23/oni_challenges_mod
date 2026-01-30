using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D1E RID: 3358
public class LineLayer : GraphLayer
{
	// Token: 0x060067E1 RID: 26593 RVA: 0x00272E7A File Offset: 0x0027107A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.InitAreaTexture();
		this.rectTransform = base.gameObject.GetComponent<RectTransform>();
	}

	// Token: 0x060067E2 RID: 26594 RVA: 0x00272E9C File Offset: 0x0027109C
	private void InitAreaTexture()
	{
		if (this.areaTexture != null)
		{
			return;
		}
		this.areaTexture = new Texture2D(96, 32);
		this.areaFill.sprite = Sprite.Create(this.areaTexture, new Rect(0f, 0f, 96f, 32f), new Vector2(0.5f, 0.5f), 100f);
		this.areaTexture.filterMode = FilterMode.Point;
	}

	// Token: 0x060067E3 RID: 26595 RVA: 0x00272F18 File Offset: 0x00271118
	public virtual GraphedLine NewLine(global::Tuple<float, float>[] points, string ID = "")
	{
		Vector2[] array = new Vector2[points.Length];
		for (int i = 0; i < points.Length; i++)
		{
			array[i] = new Vector2(points[i].first, points[i].second);
		}
		if (this.fillAreaUnderLine)
		{
			Vector2 vector = this.CalculateMin(points);
			Vector2 vector2 = this.CalculateMax(points) - vector;
			for (int j = 0; j < 96; j++)
			{
				float num = vector.x + vector2.x * ((float)j / 96f);
				if (points.Length > 1)
				{
					int num2 = 1;
					for (int k = 1; k < points.Length; k++)
					{
						if (points[k].first >= num)
						{
							num2 = k;
							break;
						}
					}
					Vector2 vector3 = new Vector2(points[num2].first - points[num2 - 1].first, points[num2].second - points[num2 - 1].second);
					float num3 = (num - points[num2 - 1].first) / vector3.x;
					bool flag = false;
					int num4 = -1;
					for (int l = 31; l >= 0; l--)
					{
						if (!flag && vector.y + vector2.y * ((float)l / 32f) < points[num2 - 1].second + vector3.y * num3)
						{
							flag = true;
							num4 = l;
						}
						Color32 color = flag ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255f * Mathf.Lerp(1f, this.fillAlphaMin, Mathf.Clamp((float)(num4 - l) / this.fillFadePixels, 0f, 1f)))) : Color.clear;
						LineLayer.s_pixelBuffer[l * 96 + j] = color;
					}
				}
			}
			this.InitAreaTexture();
			this.areaTexture.SetPixels32(LineLayer.s_pixelBuffer);
			this.areaTexture.Apply();
			this.areaFill.color = this.line_formatting[0].color;
		}
		return this.NewLine(array, ID);
	}

	// Token: 0x060067E4 RID: 26596 RVA: 0x0027312C File Offset: 0x0027132C
	private GraphedLine FindLine(string ID)
	{
		string text = string.Format("line_{0}", ID);
		foreach (GraphedLine graphedLine in this.lines)
		{
			if (graphedLine.name == text)
			{
				return graphedLine.GetComponent<GraphedLine>();
			}
		}
		GameObject gameObject = Util.KInstantiateUI(this.prefab_line, this.line_container, true);
		gameObject.name = text;
		GraphedLine component = gameObject.GetComponent<GraphedLine>();
		this.lines.Add(component);
		return component;
	}

	// Token: 0x060067E5 RID: 26597 RVA: 0x002731CC File Offset: 0x002713CC
	public virtual void RefreshLine(global::Tuple<float, float>[] data, string ID)
	{
		this.FillArea(data);
		Vector2[] array2;
		if (data.Length > this.compressDataToPointCount)
		{
			Vector2[] array = new Vector2[this.compressDataToPointCount];
			if (this.compressType == LineLayer.DataScalingType.DropValues)
			{
				float num = (float)(data.Length - this.compressDataToPointCount + 1);
				float num2 = (float)data.Length / num;
				int num3 = 0;
				float num4 = 0f;
				for (int i = 0; i < data.Length; i++)
				{
					num4 += 1f;
					if (num4 >= num2)
					{
						num4 -= num2;
					}
					else
					{
						array[num3] = new Vector2(data[i].first, data[i].second);
						num3++;
					}
				}
				if (array[this.compressDataToPointCount - 1] == Vector2.zero)
				{
					array[this.compressDataToPointCount - 1] = array[this.compressDataToPointCount - 2];
				}
			}
			else
			{
				int num5 = data.Length / this.compressDataToPointCount;
				for (int j = 0; j < this.compressDataToPointCount; j++)
				{
					if (j > 0)
					{
						float num6 = 0f;
						LineLayer.DataScalingType dataScalingType = this.compressType;
						if (dataScalingType != LineLayer.DataScalingType.Average)
						{
							if (dataScalingType == LineLayer.DataScalingType.Max)
							{
								for (int k = 0; k < num5; k++)
								{
									num6 = Mathf.Max(num6, data[j * num5 - k].second);
								}
							}
						}
						else
						{
							for (int l = 0; l < num5; l++)
							{
								num6 += data[j * num5 - l].second;
							}
							num6 /= (float)num5;
						}
						array[j] = new Vector2(data[j * num5].first, num6);
					}
				}
			}
			array2 = array;
		}
		else
		{
			array2 = new Vector2[data.Length];
			for (int m = 0; m < data.Length; m++)
			{
				array2[m] = new Vector2(data[m].first, data[m].second);
			}
		}
		GraphedLine graphedLine = this.FindLine(ID);
		graphedLine.SetPoints(array2);
		graphedLine.line_renderer.color = this.line_formatting[this.lines.Count % this.line_formatting.Length].color;
		graphedLine.line_renderer.LineThickness = (float)this.line_formatting[this.lines.Count % this.line_formatting.Length].thickness;
	}

	// Token: 0x060067E6 RID: 26598 RVA: 0x00273418 File Offset: 0x00271618
	private void FillArea(global::Tuple<float, float>[] points)
	{
		if (this.fillAreaUnderLine)
		{
			Vector2 vector;
			Vector2 a;
			this.CalculateMinMax(points, out vector, out a);
			Vector2 vector2 = a - vector;
			Color32 color = new Color32(0, 0, 0, 0);
			Vector2 vector3 = default(Vector2);
			for (int i = 0; i < 96; i++)
			{
				float num = vector.x + vector2.x * ((float)i / 96f);
				if (points.Length > 1)
				{
					int num2 = 1;
					for (int j = 1; j < points.Length; j++)
					{
						if (points[j].first >= num)
						{
							num2 = j;
							break;
						}
					}
					vector3.x = points[num2].first - points[num2 - 1].first;
					vector3.y = points[num2].second - points[num2 - 1].second;
					float num3 = (num - points[num2 - 1].first) / vector3.x;
					bool flag = false;
					int num4 = -1;
					for (int k = 31; k >= 0; k--)
					{
						if (!flag && vector.y + vector2.y * ((float)k / 32f) < points[num2 - 1].second + vector3.y * num3)
						{
							flag = true;
							num4 = k;
						}
						Color32 color2 = flag ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255f * Mathf.Lerp(1f, this.fillAlphaMin, Mathf.Clamp((float)(num4 - k) / this.fillFadePixels, 0f, 1f)))) : color;
						LineLayer.s_pixelBuffer[k * 96 + i] = color2;
					}
				}
			}
			this.areaTexture.SetPixels32(LineLayer.s_pixelBuffer);
			this.areaTexture.Apply();
			this.areaFill.color = this.line_formatting[0].color;
		}
	}

	// Token: 0x060067E7 RID: 26599 RVA: 0x002735FC File Offset: 0x002717FC
	private void CalculateMinMax(global::Tuple<float, float>[] points, out Vector2 min, out Vector2 max)
	{
		max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
		min = new Vector2(float.PositiveInfinity, 0f);
		for (int i = 0; i < points.Length; i++)
		{
			max = new Vector2(Mathf.Max(points[i].first, max.x), Mathf.Max(points[i].second, max.y));
			min = new Vector2(Mathf.Min(points[i].first, min.x), Mathf.Min(points[i].second, min.y));
		}
	}

	// Token: 0x060067E8 RID: 26600 RVA: 0x002736A4 File Offset: 0x002718A4
	protected Vector2 CalculateMax(global::Tuple<float, float>[] points)
	{
		Vector2 vector = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
		for (int i = 0; i < points.Length; i++)
		{
			vector = new Vector2(Mathf.Max(points[i].first, vector.x), Mathf.Max(points[i].second, vector.y));
		}
		return vector;
	}

	// Token: 0x060067E9 RID: 26601 RVA: 0x00273700 File Offset: 0x00271900
	protected Vector2 CalculateMin(global::Tuple<float, float>[] points)
	{
		Vector2 vector = new Vector2(float.PositiveInfinity, 0f);
		for (int i = 0; i < points.Length; i++)
		{
			vector = new Vector2(Mathf.Min(points[i].first, vector.x), Mathf.Min(points[i].second, vector.y));
		}
		return vector;
	}

	// Token: 0x060067EA RID: 26602 RVA: 0x0027375C File Offset: 0x0027195C
	public GraphedLine NewLine(Vector2[] points, string ID = "")
	{
		GameObject gameObject = Util.KInstantiateUI(this.prefab_line, this.line_container, true);
		if (ID == "")
		{
			ID = this.lines.Count.ToString();
		}
		gameObject.name = string.Format("line_{0}", ID);
		GraphedLine component = gameObject.GetComponent<GraphedLine>();
		if (points.Length > this.compressDataToPointCount)
		{
			Vector2[] array = new Vector2[this.compressDataToPointCount];
			if (this.compressType == LineLayer.DataScalingType.DropValues)
			{
				float num = (float)(points.Length - this.compressDataToPointCount + 1);
				float num2 = (float)points.Length / num;
				int num3 = 0;
				float num4 = 0f;
				for (int i = 0; i < points.Length; i++)
				{
					num4 += 1f;
					if (num4 >= num2)
					{
						num4 -= num2;
					}
					else
					{
						array[num3] = points[i];
						num3++;
					}
				}
				if (array[this.compressDataToPointCount - 1] == Vector2.zero)
				{
					array[this.compressDataToPointCount - 1] = array[this.compressDataToPointCount - 2];
				}
			}
			else
			{
				int num5 = points.Length / this.compressDataToPointCount;
				for (int j = 0; j < this.compressDataToPointCount; j++)
				{
					if (j > 0)
					{
						float num6 = 0f;
						LineLayer.DataScalingType dataScalingType = this.compressType;
						if (dataScalingType != LineLayer.DataScalingType.Average)
						{
							if (dataScalingType == LineLayer.DataScalingType.Max)
							{
								for (int k = 0; k < num5; k++)
								{
									num6 = Mathf.Max(num6, points[j * num5 - k].y);
								}
							}
						}
						else
						{
							for (int l = 0; l < num5; l++)
							{
								num6 += points[j * num5 - l].y;
							}
							num6 /= (float)num5;
						}
						array[j] = new Vector2(points[j * num5].x, num6);
					}
				}
			}
			points = array;
		}
		component.SetPoints(points);
		component.line_renderer.color = this.line_formatting[this.lines.Count % this.line_formatting.Length].color;
		component.line_renderer.LineThickness = (float)this.line_formatting[this.lines.Count % this.line_formatting.Length].thickness;
		this.lines.Add(component);
		return component;
	}

	// Token: 0x060067EB RID: 26603 RVA: 0x002739B8 File Offset: 0x00271BB8
	public void ClearLines()
	{
		foreach (GraphedLine graphedLine in this.lines)
		{
			if (graphedLine != null && graphedLine.gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(graphedLine.gameObject);
			}
		}
		this.lines.Clear();
	}

	// Token: 0x060067EC RID: 26604 RVA: 0x00273A34 File Offset: 0x00271C34
	private void Update()
	{
		if (!RectTransformUtility.RectangleContainsScreenPoint(this.rectTransform, Input.mousePosition))
		{
			for (int i = 0; i < this.lines.Count; i++)
			{
				this.lines[i].HidePointHighlight();
			}
			return;
		}
		Vector2 vector = Vector2.zero;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, Input.mousePosition, null, out vector);
		vector += this.rectTransform.sizeDelta / 2f;
		for (int j = 0; j < this.lines.Count; j++)
		{
			if (this.lines[j].PointCount != 0)
			{
				Vector2 closestDataToPointOnXAxis = this.lines[j].GetClosestDataToPointOnXAxis(vector);
				if (!float.IsInfinity(closestDataToPointOnXAxis.x) && !float.IsNaN(closestDataToPointOnXAxis.x) && !float.IsInfinity(closestDataToPointOnXAxis.y) && !float.IsNaN(closestDataToPointOnXAxis.y))
				{
					this.lines[j].SetPointHighlight(closestDataToPointOnXAxis);
				}
				else
				{
					this.lines[j].HidePointHighlight();
				}
			}
		}
	}

	// Token: 0x04004747 RID: 18247
	private const int WIDTH = 96;

	// Token: 0x04004748 RID: 18248
	private const int HEIGHT = 32;

	// Token: 0x04004749 RID: 18249
	private static Color32[] s_pixelBuffer = new Color32[3072];

	// Token: 0x0400474A RID: 18250
	[Header("Lines")]
	public LineLayer.LineFormat[] line_formatting;

	// Token: 0x0400474B RID: 18251
	public Image areaFill;

	// Token: 0x0400474C RID: 18252
	public GameObject prefab_line;

	// Token: 0x0400474D RID: 18253
	public GameObject line_container;

	// Token: 0x0400474E RID: 18254
	private List<GraphedLine> lines = new List<GraphedLine>();

	// Token: 0x0400474F RID: 18255
	protected float fillAlphaMin = 0.33f;

	// Token: 0x04004750 RID: 18256
	protected float fillFadePixels = 15f;

	// Token: 0x04004751 RID: 18257
	public bool fillAreaUnderLine;

	// Token: 0x04004752 RID: 18258
	private Texture2D areaTexture;

	// Token: 0x04004753 RID: 18259
	private int compressDataToPointCount = 256;

	// Token: 0x04004754 RID: 18260
	private LineLayer.DataScalingType compressType = LineLayer.DataScalingType.DropValues;

	// Token: 0x04004755 RID: 18261
	private RectTransform rectTransform;

	// Token: 0x02001F4F RID: 8015
	[Serializable]
	public struct LineFormat
	{
		// Token: 0x04009231 RID: 37425
		public Color color;

		// Token: 0x04009232 RID: 37426
		public int thickness;
	}

	// Token: 0x02001F50 RID: 8016
	public enum DataScalingType
	{
		// Token: 0x04009234 RID: 37428
		Average,
		// Token: 0x04009235 RID: 37429
		Max,
		// Token: 0x04009236 RID: 37430
		DropValues
	}
}
