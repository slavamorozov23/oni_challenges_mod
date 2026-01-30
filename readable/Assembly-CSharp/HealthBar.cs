using System;
using UnityEngine;

// Token: 0x02000D22 RID: 3362
public class HealthBar : ProgressBar
{
	// Token: 0x1700078F RID: 1935
	// (get) Token: 0x06006802 RID: 26626 RVA: 0x00274724 File Offset: 0x00272924
	private bool ShouldShow
	{
		get
		{
			return this.showTimer > 0f || base.PercentFull < this.alwaysShowThreshold;
		}
	}

	// Token: 0x06006803 RID: 26627 RVA: 0x00274743 File Offset: 0x00272943
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.barColor = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
		base.gameObject.SetActive(this.ShouldShow);
	}

	// Token: 0x06006804 RID: 26628 RVA: 0x00274771 File Offset: 0x00272971
	public void OnChange()
	{
		base.enabled = true;
		this.showTimer = this.maxShowTime;
	}

	// Token: 0x06006805 RID: 26629 RVA: 0x00274788 File Offset: 0x00272988
	public override void Update()
	{
		base.Update();
		if (Time.timeScale > 0f)
		{
			this.showTimer = Mathf.Max(0f, this.showTimer - Time.unscaledDeltaTime);
		}
		if (!this.ShouldShow)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06006806 RID: 26630 RVA: 0x002747D7 File Offset: 0x002729D7
	private void OnBecameInvisible()
	{
		base.enabled = false;
	}

	// Token: 0x06006807 RID: 26631 RVA: 0x002747E0 File Offset: 0x002729E0
	private void OnBecameVisible()
	{
		base.enabled = true;
	}

	// Token: 0x06006808 RID: 26632 RVA: 0x002747EC File Offset: 0x002729EC
	public override void OnOverlayChanged(object data = null)
	{
		if (!this.autoHide)
		{
			return;
		}
		if ((HashedString)data == OverlayModes.None.ID)
		{
			if (!base.gameObject.activeSelf && this.ShouldShow)
			{
				base.enabled = true;
				base.gameObject.SetActive(true);
				return;
			}
		}
		else if (base.gameObject.activeSelf)
		{
			base.enabled = false;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04004776 RID: 18294
	private float showTimer;

	// Token: 0x04004777 RID: 18295
	private float maxShowTime = 10f;

	// Token: 0x04004778 RID: 18296
	private float alwaysShowThreshold = 0.8f;
}
