using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CF9 RID: 3321
[AddComponentMenu("KMonoBehaviour/scripts/CollapsibleDetailContentPanel")]
public class CollapsibleDetailContentPanel : KMonoBehaviour
{
	// Token: 0x060066A5 RID: 26277 RVA: 0x0026A7C8 File Offset: 0x002689C8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.collapseButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.ToggleOpen));
		this.ArrowIcon.SetActive();
		this.log = new LoggerFSS("detailpanel", 35);
		this.labels = new Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabel>>();
		this.buttonLabels = new Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabelWithButton>>();
		this.collapsableButtonLabels = new Dictionary<string, CollapsibleDetailContentPanel.Label<DetailCollapsableLabel>>();
		this.Commit();
	}

	// Token: 0x060066A6 RID: 26278 RVA: 0x0026A846 File Offset: 0x00268A46
	public void SetTitle(string title)
	{
		this.HeaderLabel.text = title;
	}

	// Token: 0x060066A7 RID: 26279 RVA: 0x0026A854 File Offset: 0x00268A54
	public void Commit()
	{
		int num = 0;
		foreach (CollapsibleDetailContentPanel.Label<DetailLabel> label in this.labels.Values)
		{
			if (label.used)
			{
				num++;
				if (!label.obj.gameObject.activeSelf)
				{
					label.obj.gameObject.SetActive(true);
				}
			}
			else if (!label.used && label.obj.gameObject.activeSelf)
			{
				label.obj.gameObject.SetActive(false);
			}
			label.used = false;
		}
		foreach (CollapsibleDetailContentPanel.Label<DetailLabelWithButton> label2 in this.buttonLabels.Values)
		{
			if (label2.used)
			{
				num++;
				if (!label2.obj.gameObject.activeSelf)
				{
					label2.obj.gameObject.SetActive(true);
				}
			}
			else if (!label2.used && label2.obj.gameObject.activeSelf)
			{
				label2.obj.gameObject.SetActive(false);
			}
			label2.used = false;
		}
		foreach (CollapsibleDetailContentPanel.Label<DetailCollapsableLabel> label3 in this.collapsableButtonLabels.Values)
		{
			if (label3.used)
			{
				num++;
				if (!label3.obj.gameObject.activeSelf)
				{
					label3.obj.gameObject.SetActive(true);
				}
			}
			else if (!label3.used && label3.obj.gameObject.activeSelf)
			{
				label3.obj.gameObject.SetActive(false);
			}
			label3.used = false;
		}
		if (base.gameObject.activeSelf && num == 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (!base.gameObject.activeSelf && num > 0)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x060066A8 RID: 26280 RVA: 0x0026AA9C File Offset: 0x00268C9C
	public void SetLabel(string id, string text, string tooltip)
	{
		CollapsibleDetailContentPanel.Label<DetailLabel> label;
		if (!this.labels.TryGetValue(id, out label))
		{
			label = new CollapsibleDetailContentPanel.Label<DetailLabel>
			{
				used = true,
				obj = Util.KInstantiateUI(this.labelTemplate.gameObject, this.Content.gameObject, false).GetComponent<DetailLabel>()
			};
			label.obj.gameObject.name = id;
			this.labels[id] = label;
		}
		label.obj.label.AllowLinks = true;
		label.obj.label.text = text;
		label.obj.toolTip.toolTip = tooltip;
		label.used = true;
	}

	// Token: 0x060066A9 RID: 26281 RVA: 0x0026AB45 File Offset: 0x00268D45
	public DetailLabelWithButton SetLabelWithButton(string id, string text, string tooltip, System.Action buttonCb)
	{
		return this.SetLabelWithButton(id, text, null, null, tooltip, buttonCb);
	}

	// Token: 0x060066AA RID: 26282 RVA: 0x0026AB54 File Offset: 0x00268D54
	public DetailLabelWithButton SetLabelWithButton(string id, string mainText, string secondaryText, string thirdText, string tooltip, System.Action buttonCb)
	{
		CollapsibleDetailContentPanel.Label<DetailLabelWithButton> label;
		if (!this.buttonLabels.TryGetValue(id, out label))
		{
			label = new CollapsibleDetailContentPanel.Label<DetailLabelWithButton>
			{
				used = true,
				obj = Util.KInstantiateUI(this.labelWithActionButtonTemplate.gameObject, this.Content.gameObject, false).GetComponent<DetailLabelWithButton>()
			};
			label.obj.gameObject.name = id;
			this.buttonLabels[id] = label;
		}
		label.obj.label.AllowLinks = false;
		label.obj.label.raycastTarget = false;
		label.obj.label.text = mainText;
		label.obj.label2.AllowLinks = false;
		label.obj.label2.raycastTarget = false;
		label.obj.label2.text = secondaryText;
		label.obj.label3.AllowLinks = false;
		label.obj.label3.raycastTarget = false;
		label.obj.label3.text = thirdText;
		label.obj.RefreshLabelsVisibility();
		label.obj.toolTip.toolTip = tooltip;
		label.obj.button.ClearOnClick();
		label.obj.button.onClick += buttonCb;
		label.used = true;
		return label.obj;
	}

	// Token: 0x060066AB RID: 26283 RVA: 0x0026ACAC File Offset: 0x00268EAC
	public DetailCollapsableLabel SetCollapsableLabel(string id, string text, string valueText, string tooltip, object data, Action<DetailCollapsableLabel> onExpanded, Action<DetailCollapsableLabel> onCollapsed)
	{
		CollapsibleDetailContentPanel.Label<DetailCollapsableLabel> label;
		if (!this.collapsableButtonLabels.TryGetValue(id, out label))
		{
			label = new CollapsibleDetailContentPanel.Label<DetailCollapsableLabel>
			{
				used = true,
				obj = Util.KInstantiateUI(this.labelWithCollapsableToggleTemplate.gameObject, this.Content.gameObject, false).GetComponent<DetailCollapsableLabel>()
			};
			label.obj.gameObject.name = id;
			this.collapsableButtonLabels[id] = label;
		}
		label.obj.nameLabel.AllowLinks = false;
		label.obj.nameLabel.raycastTarget = false;
		label.obj.nameLabel.SetText(text);
		label.obj.valueLabel.SetText(valueText);
		label.obj.toolTip.toolTip = tooltip;
		label.obj.ClearToggleCallbacks();
		DetailCollapsableLabel obj = label.obj;
		obj.OnCollapsed = (Action<DetailCollapsableLabel>)Delegate.Combine(obj.OnCollapsed, onCollapsed);
		DetailCollapsableLabel obj2 = label.obj;
		obj2.OnExpanded = (Action<DetailCollapsableLabel>)Delegate.Combine(obj2.OnExpanded, onExpanded);
		label.used = true;
		label.obj.SetData(data);
		if (label.obj.IsExpanded)
		{
			label.obj.ManualTriggerOnExpanded();
		}
		return label.obj;
	}

	// Token: 0x060066AC RID: 26284 RVA: 0x0026ADE8 File Offset: 0x00268FE8
	private void ToggleOpen()
	{
		bool flag = this.scalerMask.gameObject.activeSelf;
		flag = !flag;
		this.scalerMask.gameObject.SetActive(flag);
		if (flag)
		{
			this.ArrowIcon.SetActive();
			this.ForceLocTextsMeshRebuild();
			return;
		}
		this.ArrowIcon.SetInactive();
	}

	// Token: 0x060066AD RID: 26285 RVA: 0x0026AE3C File Offset: 0x0026903C
	public void ForceLocTextsMeshRebuild()
	{
		LocText[] componentsInChildren = base.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].ForceMeshUpdate();
		}
	}

	// Token: 0x060066AE RID: 26286 RVA: 0x0026AE66 File Offset: 0x00269066
	public void SetActive(bool active)
	{
		if (base.gameObject.activeSelf != active)
		{
			base.gameObject.SetActive(active);
		}
	}

	// Token: 0x04004623 RID: 17955
	public ImageToggleState ArrowIcon;

	// Token: 0x04004624 RID: 17956
	public LocText HeaderLabel;

	// Token: 0x04004625 RID: 17957
	public MultiToggle collapseButton;

	// Token: 0x04004626 RID: 17958
	public Transform Content;

	// Token: 0x04004627 RID: 17959
	public ScalerMask scalerMask;

	// Token: 0x04004628 RID: 17960
	[Space(10f)]
	public DetailLabel labelTemplate;

	// Token: 0x04004629 RID: 17961
	public DetailLabelWithButton labelWithActionButtonTemplate;

	// Token: 0x0400462A RID: 17962
	public DetailCollapsableLabel labelWithCollapsableToggleTemplate;

	// Token: 0x0400462B RID: 17963
	private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabel>> labels;

	// Token: 0x0400462C RID: 17964
	private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabelWithButton>> buttonLabels;

	// Token: 0x0400462D RID: 17965
	private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailCollapsableLabel>> collapsableButtonLabels;

	// Token: 0x0400462E RID: 17966
	private LoggerFSS log;

	// Token: 0x02001F26 RID: 7974
	private class Label<T>
	{
		// Token: 0x040091A9 RID: 37289
		public T obj;

		// Token: 0x040091AA RID: 37290
		public bool used;
	}
}
