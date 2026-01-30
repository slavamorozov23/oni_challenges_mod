using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C87 RID: 3207
public class DetailsPanelDrawer
{
	// Token: 0x06006244 RID: 25156 RVA: 0x00244E68 File Offset: 0x00243068
	public DetailsPanelDrawer(GameObject label_prefab, GameObject parent)
	{
		this.parent = parent;
		this.labelPrefab = label_prefab;
		this.stringformatter = new UIStringFormatter();
		this.floatFormatter = new UIFloatFormatter();
	}

	// Token: 0x06006245 RID: 25157 RVA: 0x00244EA0 File Offset: 0x002430A0
	public DetailsPanelDrawer NewLabel(string text)
	{
		DetailsPanelDrawer.Label label = default(DetailsPanelDrawer.Label);
		if (this.activeLabelCount >= this.labels.Count)
		{
			label.text = Util.KInstantiate(this.labelPrefab, this.parent, null).GetComponent<LocText>();
			label.tooltip = label.text.GetComponent<ToolTip>();
			label.text.transform.localScale = new Vector3(1f, 1f, 1f);
			this.labels.Add(label);
		}
		else
		{
			label = this.labels[this.activeLabelCount];
		}
		this.activeLabelCount++;
		label.text.text = text;
		label.tooltip.toolTip = "";
		label.tooltip.OnToolTip = null;
		label.text.gameObject.SetActive(true);
		return this;
	}

	// Token: 0x06006246 RID: 25158 RVA: 0x00244F84 File Offset: 0x00243184
	public DetailsPanelDrawer BeginDrawing()
	{
		return this;
	}

	// Token: 0x06006247 RID: 25159 RVA: 0x00244F87 File Offset: 0x00243187
	public DetailsPanelDrawer EndDrawing()
	{
		return this;
	}

	// Token: 0x040042D0 RID: 17104
	private List<DetailsPanelDrawer.Label> labels = new List<DetailsPanelDrawer.Label>();

	// Token: 0x040042D1 RID: 17105
	private int activeLabelCount;

	// Token: 0x040042D2 RID: 17106
	private UIStringFormatter stringformatter;

	// Token: 0x040042D3 RID: 17107
	private UIFloatFormatter floatFormatter;

	// Token: 0x040042D4 RID: 17108
	private GameObject parent;

	// Token: 0x040042D5 RID: 17109
	private GameObject labelPrefab;

	// Token: 0x02001EB8 RID: 7864
	private struct Label
	{
		// Token: 0x04009075 RID: 36981
		public LocText text;

		// Token: 0x04009076 RID: 36982
		public ToolTip tooltip;
	}
}
