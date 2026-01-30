using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CFA RID: 3322
public class DetailCollapsableLabel : MonoBehaviour
{
	// Token: 0x1700077E RID: 1918
	// (get) Token: 0x060066B0 RID: 26288 RVA: 0x0026AE8A File Offset: 0x0026908A
	public bool IsExpanded
	{
		get
		{
			return this.toggle.isOn;
		}
	}

	// Token: 0x1700077F RID: 1919
	// (get) Token: 0x060066B1 RID: 26289 RVA: 0x0026AE97 File Offset: 0x00269097
	public object Data
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x060066B2 RID: 26290 RVA: 0x0026AE9F File Offset: 0x0026909F
	private void OnDisable()
	{
		this.MarkAllRowsUnused();
		this.RefreshRowVisibilityState();
		this.toggle.SetIsOnWithoutNotify(false);
		this.RefreshArrowIcon();
	}

	// Token: 0x060066B3 RID: 26291 RVA: 0x0026AEBF File Offset: 0x002690BF
	public void SetData(object data)
	{
		this.data = data;
	}

	// Token: 0x060066B4 RID: 26292 RVA: 0x0026AEC8 File Offset: 0x002690C8
	public void ClearToggleCallbacks()
	{
		this.toggle.ClearOnValueChanged();
		this.toggle.onValueChanged += this.OnToggleChanged;
		this.OnExpanded = null;
		this.OnCollapsed = null;
	}

	// Token: 0x060066B5 RID: 26293 RVA: 0x0026AEFC File Offset: 0x002690FC
	public void MarkAllRowsUnused()
	{
		this.lastKnownRowAvailable = 0;
		foreach (DetailCollapsableLabel.ContentRow contentRow in this.contentRows)
		{
			contentRow.inUse = false;
		}
	}

	// Token: 0x060066B6 RID: 26294 RVA: 0x0026AF54 File Offset: 0x00269154
	public void RefreshRowVisibilityState()
	{
		foreach (DetailCollapsableLabel.ContentRow contentRow in this.contentRows)
		{
			if (contentRow.label.gameObject.activeInHierarchy != contentRow.inUse)
			{
				contentRow.label.gameObject.SetActive(contentRow.inUse);
			}
		}
	}

	// Token: 0x060066B7 RID: 26295 RVA: 0x0026AFD4 File Offset: 0x002691D4
	public DetailLabelWithButton AddOrGetAvailableContentRow()
	{
		DetailCollapsableLabel.ContentRow contentRow = (this.lastKnownRowAvailable < this.contentRows.Count && !this.contentRows[this.lastKnownRowAvailable].inUse) ? this.contentRows[this.lastKnownRowAvailable] : null;
		int siblingIndex = base.transform.GetSiblingIndex();
		if (contentRow == null)
		{
			contentRow = new DetailCollapsableLabel.ContentRow
			{
				label = Util.KInstantiateUI(this.contentRowPrefab, base.transform.parent.gameObject, false).GetComponent<DetailLabelWithButton>()
			};
			this.contentRows.Add(contentRow);
		}
		contentRow.inUse = true;
		contentRow.label.transform.SetSiblingIndex(siblingIndex + 1);
		this.lastKnownRowAvailable++;
		return contentRow.label;
	}

	// Token: 0x060066B8 RID: 26296 RVA: 0x0026B097 File Offset: 0x00269297
	private void OnToggleChanged(bool expanded)
	{
		this.RefreshArrowIcon();
		if (expanded)
		{
			if (this.OnExpanded != null)
			{
				this.OnExpanded(this);
				return;
			}
		}
		else if (this.OnCollapsed != null)
		{
			this.OnCollapsed(this);
		}
	}

	// Token: 0x060066B9 RID: 26297 RVA: 0x0026B0CB File Offset: 0x002692CB
	public void ManualTriggerOnExpanded()
	{
		if (this.OnExpanded != null)
		{
			this.OnExpanded(this);
		}
	}

	// Token: 0x060066BA RID: 26298 RVA: 0x0026B0E1 File Offset: 0x002692E1
	private void RefreshArrowIcon()
	{
		this.arrowImage.sprite = (this.toggle.isOn ? this.unfoldedIcon : this.collapsedIcon);
		this.arrowImage.enabled = false;
		this.arrowImage.enabled = true;
	}

	// Token: 0x0400462F RID: 17967
	public Image arrowImage;

	// Token: 0x04004630 RID: 17968
	public LocText nameLabel;

	// Token: 0x04004631 RID: 17969
	public LocText valueLabel;

	// Token: 0x04004632 RID: 17970
	public ToolTip toolTip;

	// Token: 0x04004633 RID: 17971
	public KToggle toggle;

	// Token: 0x04004634 RID: 17972
	[SerializeField]
	private GameObject contentRowPrefab;

	// Token: 0x04004635 RID: 17973
	[Header("Icons")]
	public Sprite collapsedIcon;

	// Token: 0x04004636 RID: 17974
	public Sprite unfoldedIcon;

	// Token: 0x04004637 RID: 17975
	public Action<DetailCollapsableLabel> OnExpanded;

	// Token: 0x04004638 RID: 17976
	public Action<DetailCollapsableLabel> OnCollapsed;

	// Token: 0x04004639 RID: 17977
	private int lastKnownRowAvailable;

	// Token: 0x0400463A RID: 17978
	public List<DetailCollapsableLabel.ContentRow> contentRows = new List<DetailCollapsableLabel.ContentRow>();

	// Token: 0x0400463B RID: 17979
	private object data;

	// Token: 0x02001F27 RID: 7975
	public class ContentRow
	{
		// Token: 0x040091AB RID: 37291
		public bool inUse;

		// Token: 0x040091AC RID: 37292
		public DetailLabelWithButton label;
	}
}
