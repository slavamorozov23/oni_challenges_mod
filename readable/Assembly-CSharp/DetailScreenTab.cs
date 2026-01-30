using System;
using UnityEngine;

// Token: 0x02000CFF RID: 3327
public abstract class DetailScreenTab : TargetPanel
{
	// Token: 0x060066D2 RID: 26322
	public abstract override bool IsValidForTarget(GameObject target);

	// Token: 0x060066D3 RID: 26323 RVA: 0x0026B874 File Offset: 0x00269A74
	protected override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
	}

	// Token: 0x060066D4 RID: 26324 RVA: 0x0026B880 File Offset: 0x00269A80
	protected CollapsibleDetailContentPanel CreateCollapsableSection(string title = null)
	{
		CollapsibleDetailContentPanel collapsibleDetailContentPanel = Util.KInstantiateUI<CollapsibleDetailContentPanel>(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject, false);
		if (!string.IsNullOrEmpty(title))
		{
			collapsibleDetailContentPanel.SetTitle(title);
		}
		return collapsibleDetailContentPanel;
	}

	// Token: 0x060066D5 RID: 26325 RVA: 0x0026B8B4 File Offset: 0x00269AB4
	private void Update()
	{
		this.Refresh(false);
	}

	// Token: 0x060066D6 RID: 26326 RVA: 0x0026B8BD File Offset: 0x00269ABD
	protected virtual void Refresh(bool force = false)
	{
	}
}
