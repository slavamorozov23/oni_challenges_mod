using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B87 RID: 2951
public class LargeImpactorNotificationUI_Clock : KMonoBehaviour, ISim4000ms
{
	// Token: 0x0600580A RID: 22538 RVA: 0x002002F8 File Offset: 0x001FE4F8
	protected override void OnSpawn()
	{
		this.color_circleFillOriginalColor = this.TimerOutCircleFill.color;
		this.color_needleTrailBgOriginalColor = this.NeedleTrailBg.color;
		this.color_timerOutCircleBGOriginalColor = this.TimerOutCircleBG.color;
		this.softRed = new Color(1f, 0f, 0f, this.color_needleTrailBgOriginalColor.a);
		GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewCycleReached));
		this.UpdateSmallNeedlePosition();
		this.InitializeAnimationCoroutine();
		this.hasSpawned = true;
	}

	// Token: 0x0600580B RID: 22539 RVA: 0x0020038C File Offset: 0x001FE58C
	private void OnNewCycleReached(object data)
	{
		this.PlayReminderAnimation();
	}

	// Token: 0x0600580C RID: 22540 RVA: 0x00200394 File Offset: 0x001FE594
	public void SetLargeImpactorTime(float normalizedValue)
	{
		this.lastLargeImpactorTime = normalizedValue;
		this.SetNeedleRotation(this.LargeNeedle.rectTransform, 1f - this.lastLargeImpactorTime);
		if (!this.hasPlayedEntryAnimation)
		{
			return;
		}
		this.TimerOutCircleFill.fillAmount = this.lastLargeImpactorTime;
	}

	// Token: 0x0600580D RID: 22541 RVA: 0x002003D4 File Offset: 0x001FE5D4
	private void SetNeedleRotation(RectTransform needle, float normalizedTime)
	{
		needle.localRotation = Quaternion.Euler(0f, 0f, -360f * normalizedTime);
		if (needle.gameObject == this.LargeNeedle.gameObject)
		{
			this.NeedleTrailBg.fillAmount = normalizedTime;
		}
	}

	// Token: 0x0600580E RID: 22542 RVA: 0x00200421 File Offset: 0x001FE621
	public void Sim4000ms(float dt)
	{
		this.UpdateSmallNeedlePosition();
	}

	// Token: 0x0600580F RID: 22543 RVA: 0x0020042C File Offset: 0x001FE62C
	private void UpdateSmallNeedlePosition()
	{
		float currentCycleAsPercentage = GameClock.Instance.GetCurrentCycleAsPercentage();
		this.SetNeedleRotation(this.SmallNeedlePivot, currentCycleAsPercentage);
	}

	// Token: 0x06005810 RID: 22544 RVA: 0x00200451 File Offset: 0x001FE651
	private void InitializeAnimationCoroutine()
	{
		this.AbortCoroutine();
		this.animationCoroutine = base.StartCoroutine(this.AnimationCoroutineLogic());
	}

	// Token: 0x06005811 RID: 22545 RVA: 0x0020046B File Offset: 0x001FE66B
	private void AbortCoroutine()
	{
		if (this.animationCoroutine != null)
		{
			base.StopAllCoroutines();
		}
		this.animationCoroutine = null;
	}

	// Token: 0x06005812 RID: 22546 RVA: 0x00200482 File Offset: 0x001FE682
	public void PlayReminderAnimation()
	{
		this.reminderAnimationTimer = 0f;
	}

	// Token: 0x06005813 RID: 22547 RVA: 0x0020048F File Offset: 0x001FE68F
	private IEnumerator AnimationCoroutineLogic()
	{
		if (!this.hasPlayedEntryAnimation)
		{
			GameClock.Instance.GetCurrentCycleAsPercentage();
			float num = this.entryAnimationDuration / 600f;
			yield return this.Interpolate(delegate(float n)
			{
				this.TimerOutCircleFill.fillAmount = n * this.lastLargeImpactorTime;
			}, this.entryAnimationDuration, null);
			this.hasPlayedEntryAnimation = true;
		}
		for (;;)
		{
			if (this.reminderAnimationTimer < 0f)
			{
				yield return null;
			}
			if (this.reminderAnimationTimer >= 0f && this.reminderAnimationTimer < this.reminderAnimationDuration)
			{
				float t = Mathf.Abs(Mathf.Sin(this.reminderAnimationTimer / this.reminderAnimationDuration * 3.1415927f * 16f));
				this.TimerOutCircleBG.color = Color.Lerp(this.color_timerOutCircleBGOriginalColor, Color.red, t);
				this.NeedleTrailBg.color = Color.Lerp(this.color_needleTrailBgOriginalColor, this.softRed, t);
				this.reminderAnimationTimer += Time.deltaTime;
				yield return null;
			}
			if (this.reminderAnimationTimer >= this.reminderAnimationDuration)
			{
				this.TimerOutCircleBG.color = this.color_timerOutCircleBGOriginalColor;
				this.NeedleTrailBg.color = this.color_needleTrailBgOriginalColor;
				this.reminderAnimationTimer = -1f;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06005814 RID: 22548 RVA: 0x0020049E File Offset: 0x001FE69E
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (this.hasSpawned)
		{
			this.InitializeAnimationCoroutine();
		}
	}

	// Token: 0x06005815 RID: 22549 RVA: 0x002004B4 File Offset: 0x001FE6B4
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.AbortCoroutine();
	}

	// Token: 0x06005816 RID: 22550 RVA: 0x002004C2 File Offset: 0x001FE6C2
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameClock.Instance.Unsubscribe(631075836, new Action<object>(this.OnNewCycleReached));
	}

	// Token: 0x04003AFC RID: 15100
	public KImage LargeNeedle;

	// Token: 0x04003AFD RID: 15101
	public RectTransform SmallNeedlePivot;

	// Token: 0x04003AFE RID: 15102
	public KImage NeedleTrailBg;

	// Token: 0x04003AFF RID: 15103
	public Image TimerOutCircleFill;

	// Token: 0x04003B00 RID: 15104
	public Image TimerOutCircleBG;

	// Token: 0x04003B01 RID: 15105
	private Color color_circleFillOriginalColor;

	// Token: 0x04003B02 RID: 15106
	private Color color_needleTrailBgOriginalColor;

	// Token: 0x04003B03 RID: 15107
	private Color color_timerOutCircleBGOriginalColor;

	// Token: 0x04003B04 RID: 15108
	private Color softRed;

	// Token: 0x04003B05 RID: 15109
	private Coroutine animationCoroutine;

	// Token: 0x04003B06 RID: 15110
	private bool hasSpawned;

	// Token: 0x04003B07 RID: 15111
	private float entryAnimationDuration = 1f;

	// Token: 0x04003B08 RID: 15112
	private float reminderAnimationDuration = 16f;

	// Token: 0x04003B09 RID: 15113
	private bool hasPlayedEntryAnimation;

	// Token: 0x04003B0A RID: 15114
	private float reminderAnimationTimer = -1f;

	// Token: 0x04003B0B RID: 15115
	private float lastLargeImpactorTime = -1f;

	// Token: 0x04003B0C RID: 15116
	private const int reminderSetting_BlinkTimes = 16;
}
