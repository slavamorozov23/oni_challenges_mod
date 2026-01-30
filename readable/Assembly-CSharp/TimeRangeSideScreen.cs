using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E8A RID: 3722
public class TimeRangeSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x0600768A RID: 30346 RVA: 0x002D34FC File Offset: 0x002D16FC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.labelHeaderStart.text = UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.ON;
		this.labelHeaderDuration.text = UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.DURATION;
	}

	// Token: 0x0600768B RID: 30347 RVA: 0x002D352E File Offset: 0x002D172E
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicTimeOfDaySensor>() != null;
	}

	// Token: 0x0600768C RID: 30348 RVA: 0x002D353C File Offset: 0x002D173C
	public override void SetTarget(GameObject target)
	{
		this.imageActiveZone.color = GlobalAssets.Instance.colorSet.logicOnSidescreen;
		this.imageInactiveZone.color = GlobalAssets.Instance.colorSet.logicOffSidescreen;
		base.SetTarget(target);
		this.targetTimedSwitch = target.GetComponent<LogicTimeOfDaySensor>();
		this.duration.onValueChanged.RemoveAllListeners();
		this.startTime.onValueChanged.RemoveAllListeners();
		this.startTime.value = this.targetTimedSwitch.startTime;
		this.duration.value = this.targetTimedSwitch.duration;
		this.ChangeSetting();
		this.startTime.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
		this.duration.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
	}

	// Token: 0x0600768D RID: 30349 RVA: 0x002D3624 File Offset: 0x002D1824
	private void ChangeSetting()
	{
		this.targetTimedSwitch.startTime = this.startTime.value;
		this.targetTimedSwitch.duration = this.duration.value;
		this.imageActiveZone.rectTransform.rotation = Quaternion.identity;
		this.imageActiveZone.rectTransform.Rotate(0f, 0f, this.NormalizedValueToDegrees(this.startTime.value));
		this.imageActiveZone.fillAmount = this.duration.value;
		this.labelValueStart.text = GameUtil.GetFormattedPercent(this.targetTimedSwitch.startTime * 100f, GameUtil.TimeSlice.None);
		this.labelValueDuration.text = GameUtil.GetFormattedPercent(this.targetTimedSwitch.duration * 100f, GameUtil.TimeSlice.None);
		this.endIndicator.rotation = Quaternion.identity;
		this.endIndicator.Rotate(0f, 0f, this.NormalizedValueToDegrees(this.startTime.value + this.duration.value));
		this.startTime.SetTooltipText(string.Format(UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.ON_TOOLTIP, GameUtil.GetFormattedPercent(this.targetTimedSwitch.startTime * 100f, GameUtil.TimeSlice.None)));
		this.duration.SetTooltipText(string.Format(UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.DURATION_TOOLTIP, GameUtil.GetFormattedPercent(this.targetTimedSwitch.duration * 100f, GameUtil.TimeSlice.None)));
	}

	// Token: 0x0600768E RID: 30350 RVA: 0x002D379B File Offset: 0x002D199B
	public void Render200ms(float dt)
	{
		this.currentTimeMarker.rotation = Quaternion.identity;
		this.currentTimeMarker.Rotate(0f, 0f, this.NormalizedValueToDegrees(GameClock.Instance.GetCurrentCycleAsPercentage()));
	}

	// Token: 0x0600768F RID: 30351 RVA: 0x002D37D2 File Offset: 0x002D19D2
	private float NormalizedValueToDegrees(float value)
	{
		return 360f * value;
	}

	// Token: 0x06007690 RID: 30352 RVA: 0x002D37DB File Offset: 0x002D19DB
	private float SecondsToDegrees(float seconds)
	{
		return 360f * (seconds / 600f);
	}

	// Token: 0x06007691 RID: 30353 RVA: 0x002D37EA File Offset: 0x002D19EA
	private float DegreesToNormalizedValue(float degrees)
	{
		return degrees / 360f;
	}

	// Token: 0x04005203 RID: 20995
	public Image imageInactiveZone;

	// Token: 0x04005204 RID: 20996
	public Image imageActiveZone;

	// Token: 0x04005205 RID: 20997
	private LogicTimeOfDaySensor targetTimedSwitch;

	// Token: 0x04005206 RID: 20998
	public KSlider startTime;

	// Token: 0x04005207 RID: 20999
	public KSlider duration;

	// Token: 0x04005208 RID: 21000
	public RectTransform endIndicator;

	// Token: 0x04005209 RID: 21001
	public LocText labelHeaderStart;

	// Token: 0x0400520A RID: 21002
	public LocText labelHeaderDuration;

	// Token: 0x0400520B RID: 21003
	public LocText labelValueStart;

	// Token: 0x0400520C RID: 21004
	public LocText labelValueDuration;

	// Token: 0x0400520D RID: 21005
	public RectTransform currentTimeMarker;
}
