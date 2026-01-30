using System;
using UnityEngine;

// Token: 0x02000E63 RID: 3683
public class ProgressBarSideScreen : SideScreenContent, IRender1000ms
{
	// Token: 0x060074F3 RID: 29939 RVA: 0x002CA43B File Offset: 0x002C863B
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060074F4 RID: 29940 RVA: 0x002CA443 File Offset: 0x002C8643
	public override int GetSideScreenSortOrder()
	{
		return -10;
	}

	// Token: 0x060074F5 RID: 29941 RVA: 0x002CA447 File Offset: 0x002C8647
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IProgressBarSideScreen>() != null;
	}

	// Token: 0x060074F6 RID: 29942 RVA: 0x002CA452 File Offset: 0x002C8652
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetObject = target.GetComponent<IProgressBarSideScreen>();
		this.RefreshBar();
	}

	// Token: 0x060074F7 RID: 29943 RVA: 0x002CA470 File Offset: 0x002C8670
	private void RefreshBar()
	{
		this.progressBar.SetMaxValue(this.targetObject.GetProgressBarMaxValue());
		this.progressBar.SetFillPercentage(this.targetObject.GetProgressBarFillPercentage());
		this.progressBar.label.SetText(this.targetObject.GetProgressBarLabel());
		this.label.SetText(this.targetObject.GetProgressBarTitleLabel());
		this.progressBar.GetComponentInChildren<ToolTip>().SetSimpleTooltip(this.targetObject.GetProgressBarTooltip());
	}

	// Token: 0x060074F8 RID: 29944 RVA: 0x002CA4F5 File Offset: 0x002C86F5
	public void Render1000ms(float dt)
	{
		this.RefreshBar();
	}

	// Token: 0x040050E3 RID: 20707
	public LocText label;

	// Token: 0x040050E4 RID: 20708
	public GenericUIProgressBar progressBar;

	// Token: 0x040050E5 RID: 20709
	public IProgressBarSideScreen targetObject;
}
