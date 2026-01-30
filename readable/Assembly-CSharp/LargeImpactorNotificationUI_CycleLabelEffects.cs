using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B88 RID: 2952
public class LargeImpactorNotificationUI_CycleLabelEffects : MonoBehaviour
{
	// Token: 0x06005819 RID: 22553 RVA: 0x0020052E File Offset: 0x001FE72E
	public void InitializeCycleLabelFocusMonitor()
	{
		this.AbortCycleLabelFocusMonitor();
		this.cycleLabelFocusCoroutine = base.StartCoroutine(this.CycleLabelFocusMonitor());
	}

	// Token: 0x0600581A RID: 22554 RVA: 0x00200548 File Offset: 0x001FE748
	public void AbortCycleLabelFocusMonitor()
	{
		if (this.cycleLabelFocusCoroutine != null)
		{
			base.StopCoroutine(this.cycleLabelFocusCoroutine);
			this.cycleLabelFocusCoroutine = null;
		}
	}

	// Token: 0x0600581B RID: 22555 RVA: 0x00200565 File Offset: 0x001FE765
	private IEnumerator CycleLabelFocusMonitor()
	{
		float previousVisibleValue = -1f;
		float visibleValue = 0f;
		for (;;)
		{
			visibleValue = Mathf.Clamp(visibleValue + Time.unscaledDeltaTime / (this.notificationTooltipComponent.isHovering ? this.cycleFocusSpeed : this.cycleUnfocusSpeed) * (float)(this.notificationTooltipComponent.isHovering ? 1 : -1), 0f, 1f);
			if (visibleValue != previousVisibleValue)
			{
				previousVisibleValue = visibleValue;
				this.cyclesLabelBackground.Opacity(visibleValue);
				this.numberOfCyclesLabel.Opacity(visibleValue);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04003B0D RID: 15117
	public ToolTip notificationTooltipComponent;

	// Token: 0x04003B0E RID: 15118
	public Image cyclesLabelBackground;

	// Token: 0x04003B0F RID: 15119
	public LocText numberOfCyclesLabel;

	// Token: 0x04003B10 RID: 15120
	private Coroutine cycleLabelFocusCoroutine;

	// Token: 0x04003B11 RID: 15121
	private float cycleFocusSpeed = 0.2f;

	// Token: 0x04003B12 RID: 15122
	private float cycleUnfocusSpeed = 0.4f;
}
