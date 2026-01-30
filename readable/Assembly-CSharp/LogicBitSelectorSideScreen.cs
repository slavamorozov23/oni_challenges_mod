using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000E4A RID: 3658
public class LogicBitSelectorSideScreen : SideScreenContent, IRenderEveryTick
{
	// Token: 0x060073FA RID: 29690 RVA: 0x002C47F7 File Offset: 0x002C29F7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.activeColor = GlobalAssets.Instance.colorSet.logicOnText;
		this.inactiveColor = GlobalAssets.Instance.colorSet.logicOffText;
	}

	// Token: 0x060073FB RID: 29691 RVA: 0x002C4833 File Offset: 0x002C2A33
	public void SelectToggle(int bit)
	{
		this.target.SetBitSelection(bit);
		this.target.UpdateVisuals();
		this.RefreshToggles();
	}

	// Token: 0x060073FC RID: 29692 RVA: 0x002C4854 File Offset: 0x002C2A54
	private void RefreshToggles()
	{
		for (int i = 0; i < this.target.GetBitDepth(); i++)
		{
			int n = i;
			if (!this.toggles_by_int.ContainsKey(i))
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowPrefab.transform.parent.gameObject, true);
				gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("bitName").SetText(string.Format(UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.BIT, i + 1));
				gameObject.GetComponent<HierarchyReferences>().GetReference<KImage>("stateIcon").color = (this.target.IsBitActive(i) ? this.activeColor : this.inactiveColor);
				gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("stateText").SetText(this.target.IsBitActive(i) ? UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_ACTIVE : UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_INACTIVE);
				MultiToggle component = gameObject.GetComponent<MultiToggle>();
				this.toggles_by_int.Add(i, component);
			}
			this.toggles_by_int[i].onClick = delegate()
			{
				this.SelectToggle(n);
			};
		}
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.toggles_by_int)
		{
			if (this.target.GetBitSelection() == keyValuePair.Key)
			{
				keyValuePair.Value.ChangeState(0);
			}
			else
			{
				keyValuePair.Value.ChangeState(1);
			}
		}
	}

	// Token: 0x060073FD RID: 29693 RVA: 0x002C49F4 File Offset: 0x002C2BF4
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<ILogicRibbonBitSelector>() != null;
	}

	// Token: 0x060073FE RID: 29694 RVA: 0x002C4A00 File Offset: 0x002C2C00
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<ILogicRibbonBitSelector>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received is not an ILogicRibbonBitSelector");
			return;
		}
		this.titleKey = this.target.SideScreenTitle;
		this.readerDescriptionContainer.SetActive(this.target.SideScreenDisplayReaderDescription());
		this.writerDescriptionContainer.SetActive(this.target.SideScreenDisplayWriterDescription());
		this.RefreshToggles();
		this.UpdateInputOutputDisplay();
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.toggles_by_int)
		{
			this.UpdateStateVisuals(keyValuePair.Key);
		}
	}

	// Token: 0x060073FF RID: 29695 RVA: 0x002C4AD8 File Offset: 0x002C2CD8
	public void RenderEveryTick(float dt)
	{
		if (this.target.Equals(null))
		{
			return;
		}
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.toggles_by_int)
		{
			this.UpdateStateVisuals(keyValuePair.Key);
		}
		this.UpdateInputOutputDisplay();
	}

	// Token: 0x06007400 RID: 29696 RVA: 0x002C4B48 File Offset: 0x002C2D48
	private void UpdateInputOutputDisplay()
	{
		if (this.target.SideScreenDisplayReaderDescription())
		{
			this.outputDisplayIcon.color = ((this.target.GetOutputValue() > 0) ? this.activeColor : this.inactiveColor);
		}
		if (this.target.SideScreenDisplayWriterDescription())
		{
			this.inputDisplayIcon.color = ((this.target.GetInputValue() > 0) ? this.activeColor : this.inactiveColor);
		}
	}

	// Token: 0x06007401 RID: 29697 RVA: 0x002C4BC0 File Offset: 0x002C2DC0
	private void UpdateStateVisuals(int bit)
	{
		MultiToggle multiToggle = this.toggles_by_int[bit];
		multiToggle.gameObject.GetComponent<HierarchyReferences>().GetReference<KImage>("stateIcon").color = (this.target.IsBitActive(bit) ? this.activeColor : this.inactiveColor);
		multiToggle.gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("stateText").SetText(this.target.IsBitActive(bit) ? UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_ACTIVE : UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_INACTIVE);
	}

	// Token: 0x04005039 RID: 20537
	private ILogicRibbonBitSelector target;

	// Token: 0x0400503A RID: 20538
	public GameObject rowPrefab;

	// Token: 0x0400503B RID: 20539
	public KImage inputDisplayIcon;

	// Token: 0x0400503C RID: 20540
	public KImage outputDisplayIcon;

	// Token: 0x0400503D RID: 20541
	public GameObject readerDescriptionContainer;

	// Token: 0x0400503E RID: 20542
	public GameObject writerDescriptionContainer;

	// Token: 0x0400503F RID: 20543
	[NonSerialized]
	public Dictionary<int, MultiToggle> toggles_by_int = new Dictionary<int, MultiToggle>();

	// Token: 0x04005040 RID: 20544
	private Color activeColor;

	// Token: 0x04005041 RID: 20545
	private Color inactiveColor;
}
