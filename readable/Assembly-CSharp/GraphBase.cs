using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

// Token: 0x02000D17 RID: 3351
[AddComponentMenu("KMonoBehaviour/scripts/GraphBase")]
public class GraphBase : KMonoBehaviour
{
	// Token: 0x060067C6 RID: 26566 RVA: 0x00272340 File Offset: 0x00270540
	public Vector2 GetRelativePosition(Vector2 absolute_point)
	{
		Vector2 zero = Vector2.zero;
		float num = Mathf.Max(1f, this.axis_x.max_value - this.axis_x.min_value);
		float num2 = absolute_point.x - this.axis_x.min_value;
		zero.x = num2 / num;
		float num3 = Mathf.Max(1f, this.axis_y.max_value - this.axis_y.min_value);
		float num4 = absolute_point.y - this.axis_y.min_value;
		zero.y = num4 / num3;
		return zero;
	}

	// Token: 0x060067C7 RID: 26567 RVA: 0x002723D4 File Offset: 0x002705D4
	public Vector2 GetRelativeSize(Vector2 absolute_size)
	{
		return this.GetRelativePosition(absolute_size);
	}

	// Token: 0x060067C8 RID: 26568 RVA: 0x002723DD File Offset: 0x002705DD
	public void ClearGuides()
	{
		this.ClearVerticalGuides();
		this.ClearHorizontalGuides();
	}

	// Token: 0x060067C9 RID: 26569 RVA: 0x002723EC File Offset: 0x002705EC
	public void ClearHorizontalGuides()
	{
		foreach (GameObject gameObject in this.horizontalGuides)
		{
			if (gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
		}
		this.horizontalGuides.Clear();
	}

	// Token: 0x060067CA RID: 26570 RVA: 0x00272454 File Offset: 0x00270654
	public void ClearVerticalGuides()
	{
		foreach (GameObject gameObject in this.verticalGuides)
		{
			if (gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
		}
		this.verticalGuides.Clear();
	}

	// Token: 0x060067CB RID: 26571 RVA: 0x002724BC File Offset: 0x002706BC
	public void RefreshGuides()
	{
		this.ClearGuides();
		this.RefreshHorizontalGuides();
		this.RefreshVerticalGuides();
	}

	// Token: 0x060067CC RID: 26572 RVA: 0x002724D0 File Offset: 0x002706D0
	public void RefreshHorizontalGuides()
	{
		if (this.prefab_guide_x != null)
		{
			GameObject gameObject = Util.KInstantiateUI(this.prefab_guide_x, this.guides_x, true);
			gameObject.name = "guides_horizontal";
			Vector2[] array = new Vector2[2 * (int)(this.axis_y.range / this.axis_y.guide_frequency)];
			for (int i = 0; i < array.Length; i += 2)
			{
				Vector2 absolute_point = new Vector2(this.axis_x.min_value, (float)i * (this.axis_y.guide_frequency / 2f));
				array[i] = this.GetRelativePosition(absolute_point);
				Vector2 absolute_point2 = new Vector2(this.axis_x.max_value, (float)i * (this.axis_y.guide_frequency / 2f));
				array[i + 1] = this.GetRelativePosition(absolute_point2);
				if (this.prefab_guide_horizontal_label != null)
				{
					GameObject gameObject2 = Util.KInstantiateUI(this.prefab_guide_horizontal_label, gameObject, true);
					gameObject2.GetComponent<LocText>().alignment = TextAlignmentOptions.MidlineLeft;
					gameObject2.GetComponent<LocText>().text = ((int)this.axis_y.guide_frequency * (i / 2)).ToString();
					gameObject2.rectTransform().SetLocalPosition(new Vector2(8f, (float)i * (base.gameObject.rectTransform().rect.height / (float)array.Length)) - base.gameObject.rectTransform().rect.size / 2f);
				}
			}
			gameObject.GetComponent<UILineRenderer>().Points = array;
			this.horizontalGuides.Add(gameObject);
		}
	}

	// Token: 0x060067CD RID: 26573 RVA: 0x00272678 File Offset: 0x00270878
	public void RefreshVerticalGuides()
	{
		if (this.prefab_guide_y != null)
		{
			GameObject gameObject = Util.KInstantiateUI(this.prefab_guide_y, this.guides_y, true);
			gameObject.name = "guides_vertical";
			Vector2[] array = new Vector2[2 * (int)(this.axis_x.range / this.axis_x.guide_frequency)];
			for (int i = 0; i < array.Length; i += 2)
			{
				Vector2 absolute_point = new Vector2((float)i * (this.axis_x.guide_frequency / 2f), this.axis_y.min_value);
				array[i] = this.GetRelativePosition(absolute_point);
				Vector2 absolute_point2 = new Vector2((float)i * (this.axis_x.guide_frequency / 2f), this.axis_y.max_value);
				array[i + 1] = this.GetRelativePosition(absolute_point2);
				if (this.prefab_guide_vertical_label != null)
				{
					GameObject gameObject2 = Util.KInstantiateUI(this.prefab_guide_vertical_label, gameObject, true);
					gameObject2.GetComponent<LocText>().alignment = TextAlignmentOptions.Bottom;
					gameObject2.GetComponent<LocText>().text = ((int)this.axis_x.guide_frequency * (i / 2)).ToString();
					gameObject2.rectTransform().SetLocalPosition(new Vector2((float)i * (base.gameObject.rectTransform().rect.width / (float)array.Length), 4f) - base.gameObject.rectTransform().rect.size / 2f);
				}
			}
			gameObject.GetComponent<UILineRenderer>().Points = array;
			this.verticalGuides.Add(gameObject);
		}
	}

	// Token: 0x04004725 RID: 18213
	[Header("Axis")]
	public GraphAxis axis_x;

	// Token: 0x04004726 RID: 18214
	public GraphAxis axis_y;

	// Token: 0x04004727 RID: 18215
	[Header("References")]
	public GameObject prefab_guide_x;

	// Token: 0x04004728 RID: 18216
	public GameObject prefab_guide_y;

	// Token: 0x04004729 RID: 18217
	public GameObject prefab_guide_horizontal_label;

	// Token: 0x0400472A RID: 18218
	public GameObject prefab_guide_vertical_label;

	// Token: 0x0400472B RID: 18219
	public GameObject guides_x;

	// Token: 0x0400472C RID: 18220
	public GameObject guides_y;

	// Token: 0x0400472D RID: 18221
	public LocText label_title;

	// Token: 0x0400472E RID: 18222
	public LocText label_x;

	// Token: 0x0400472F RID: 18223
	public LocText label_y;

	// Token: 0x04004730 RID: 18224
	public string graphName;

	// Token: 0x04004731 RID: 18225
	protected List<GameObject> horizontalGuides = new List<GameObject>();

	// Token: 0x04004732 RID: 18226
	protected List<GameObject> verticalGuides = new List<GameObject>();

	// Token: 0x04004733 RID: 18227
	private const int points_per_guide_line = 2;
}
