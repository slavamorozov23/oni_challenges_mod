using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000CEF RID: 3311
public class DateTime : KScreen
{
	// Token: 0x0600662D RID: 26157 RVA: 0x00267105 File Offset: 0x00265305
	public static void DestroyInstance()
	{
		global::DateTime.Instance = null;
	}

	// Token: 0x0600662E RID: 26158 RVA: 0x0026710D File Offset: 0x0026530D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::DateTime.Instance = this;
		this.milestoneEffect.gameObject.SetActive(false);
	}

	// Token: 0x0600662F RID: 26159 RVA: 0x0026712C File Offset: 0x0026532C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tooltip.OnComplexToolTip = new ToolTip.ComplexTooltipDelegate(this.BuildTooltip);
		Game.Instance.Subscribe(2070437606, new Action<object>(this.OnMilestoneDayReached));
		Game.Instance.Subscribe(-720092972, new Action<object>(this.OnMilestoneDayApproaching));
	}

	// Token: 0x06006630 RID: 26160 RVA: 0x00267190 File Offset: 0x00265390
	private List<global::Tuple<string, TextStyleSetting>> BuildTooltip()
	{
		List<global::Tuple<string, TextStyleSetting>> colonyToolTip = SaveGame.Instance.GetColonyToolTip();
		if (TimeOfDay.IsMilestoneApproaching)
		{
			colonyToolTip.Add(new global::Tuple<string, TextStyleSetting>(" ", null));
			colonyToolTip.Add(new global::Tuple<string, TextStyleSetting>(UI.ASTEROIDCLOCK.MILESTONE_TITLE.text, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			colonyToolTip.Add(new global::Tuple<string, TextStyleSetting>(UI.ASTEROIDCLOCK.MILESTONE_DESCRIPTION.text.Replace("{0}", (GameClock.Instance.GetCycle() + 2).ToString()), ToolTipScreen.Instance.defaultTooltipBodyStyle));
		}
		return colonyToolTip;
	}

	// Token: 0x06006631 RID: 26161 RVA: 0x0026721D File Offset: 0x0026541D
	private void Update()
	{
		if (GameClock.Instance != null && this.displayedDayCount != GameUtil.GetCurrentCycle())
		{
			this.text.text = this.Days();
			this.displayedDayCount = GameUtil.GetCurrentCycle();
		}
	}

	// Token: 0x06006632 RID: 26162 RVA: 0x00267255 File Offset: 0x00265455
	private void OnMilestoneDayApproaching(object data)
	{
		int value = ((Boxed<int>)data).value;
		this.milestoneEffect.gameObject.SetActive(true);
		this.milestoneEffect.Play("100fx_pre", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06006633 RID: 26163 RVA: 0x00267294 File Offset: 0x00265494
	private void OnMilestoneDayReached(object data)
	{
		int value = ((Boxed<int>)data).value;
		this.milestoneEffect.gameObject.SetActive(true);
		this.milestoneEffect.Play("100fx", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06006634 RID: 26164 RVA: 0x002672D4 File Offset: 0x002654D4
	private string Days()
	{
		return GameUtil.GetCurrentCycle().ToString();
	}

	// Token: 0x040045B5 RID: 17845
	public static global::DateTime Instance;

	// Token: 0x040045B6 RID: 17846
	private const string MILESTONE_ANTICIPATION_ANIMATION_NAME = "100fx_pre";

	// Token: 0x040045B7 RID: 17847
	private const string MILESTONE_ANIMATION_NAME = "100fx";

	// Token: 0x040045B8 RID: 17848
	public LocText day;

	// Token: 0x040045B9 RID: 17849
	private int displayedDayCount = -1;

	// Token: 0x040045BA RID: 17850
	[SerializeField]
	private KBatchedAnimController milestoneEffect;

	// Token: 0x040045BB RID: 17851
	[SerializeField]
	private LocText text;

	// Token: 0x040045BC RID: 17852
	[SerializeField]
	private ToolTip tooltip;

	// Token: 0x040045BD RID: 17853
	[SerializeField]
	private TextStyleSetting tooltipstyle_Days;

	// Token: 0x040045BE RID: 17854
	[SerializeField]
	private TextStyleSetting tooltipstyle_Playtime;

	// Token: 0x040045BF RID: 17855
	[SerializeField]
	public KToggle scheduleToggle;
}
