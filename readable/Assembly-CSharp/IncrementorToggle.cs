using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000D2B RID: 3371
public class IncrementorToggle : MultiToggle
{
	// Token: 0x06006821 RID: 26657 RVA: 0x00274CC4 File Offset: 0x00272EC4
	protected override void Update()
	{
		if (this.clickHeldDown)
		{
			this.totalHeldTime += Time.unscaledDeltaTime;
			if (this.timeToNextIncrement <= 0f)
			{
				this.PlayClickSound();
				this.onClick();
				this.timeToNextIncrement = Mathf.Lerp(this.timeBetweenIncrementsMax, this.timeBetweenIncrementsMin, this.totalHeldTime / 2.5f);
				return;
			}
			this.timeToNextIncrement -= Time.unscaledDeltaTime;
		}
	}

	// Token: 0x06006822 RID: 26658 RVA: 0x00274D40 File Offset: 0x00272F40
	private void PlayClickSound()
	{
		if (this.play_sound_on_click)
		{
			if (this.states[this.state].on_click_override_sound_path == "")
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
				return;
			}
			KFMOD.PlayUISound(GlobalAssets.GetSound(this.states[this.state].on_click_override_sound_path, false));
		}
	}

	// Token: 0x06006823 RID: 26659 RVA: 0x00274DA9 File Offset: 0x00272FA9
	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		this.timeToNextIncrement = this.timeBetweenIncrementsMax;
	}

	// Token: 0x06006824 RID: 26660 RVA: 0x00274DC0 File Offset: 0x00272FC0
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (!this.clickHeldDown)
		{
			this.clickHeldDown = true;
			this.PlayClickSound();
			if (this.onClick != null)
			{
				this.onClick();
			}
		}
		if (this.states.Length - 1 < this.state)
		{
			global::Debug.LogWarning("Multi toggle has too few / no states");
		}
		base.RefreshHoverColor();
	}

	// Token: 0x06006825 RID: 26661 RVA: 0x00274E17 File Offset: 0x00273017
	public override void OnPointerClick(PointerEventData eventData)
	{
		base.RefreshHoverColor();
	}

	// Token: 0x04004784 RID: 18308
	private float timeBetweenIncrementsMin = 0.033f;

	// Token: 0x04004785 RID: 18309
	private float timeBetweenIncrementsMax = 0.25f;

	// Token: 0x04004786 RID: 18310
	private const float incrementAccelerationScale = 2.5f;

	// Token: 0x04004787 RID: 18311
	private float timeToNextIncrement;
}
