using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000B86 RID: 2950
public class LargeImpactorNotificationUI : KMonoBehaviour, ISim200ms
{
	// Token: 0x060057FE RID: 22526 RVA: 0x001FFECC File Offset: 0x001FE0CC
	protected override void OnSpawn()
	{
		GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance(Db.Get().GameplayEvents.LargeImpactor.Id, -1);
		LargeImpactorEvent.StatesInstance statesInstance = (LargeImpactorEvent.StatesInstance)gameplayEventInstance.smi;
		this.rangeVisualizer = statesInstance.impactorInstance.GetComponent<LargeImpactorVisualizer>();
		this.asteroidBackground = statesInstance.impactorInstance.GetComponent<ParallaxBackgroundObject>();
		this.statusMonitor = statesInstance.impactorInstance.GetSMI<LargeImpactorStatus.Instance>();
		LargeImpactorStatus.Instance instance = this.statusMonitor;
		instance.OnDamaged = (Action<int>)Delegate.Combine(instance.OnDamaged, new Action<int>(this.OnAsteroidDamaged));
		Game.Instance.Subscribe(445618876, new Action<object>(this.OnScreenResolutionChanged));
		Game.Instance.Subscribe(-810220474, new Action<object>(this.OnScreenResolutionChanged));
		this.cyclesLabelEffects.InitializeCycleLabelFocusMonitor();
		this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.ToggleVisibility));
		this.toggle.SetIsOnWithoutNotify(this.rangeVisualizer != null && this.rangeVisualizer.Visible);
		this.toggle.offEffectDuration = this.rangeVisualizer.FoldEffectDuration;
		LargeImpactorCrashStamp component = statesInstance.impactorInstance.GetComponent<LargeImpactorCrashStamp>();
		this.midSkyCell = Grid.FindMidSkyCellAlignedWithCellInWorld(Grid.XYToCell(component.stampLocation.x, component.stampLocation.y), gameplayEventInstance.worldId);
		this.RefreshTogglePositionInRangeVisualizer();
		this.RefreshValues();
	}

	// Token: 0x060057FF RID: 22527 RVA: 0x00200044 File Offset: 0x001FE244
	private void OnScreenResolutionChanged(object data)
	{
		this.RefreshTogglePositionInRangeVisualizer();
	}

	// Token: 0x06005800 RID: 22528 RVA: 0x0020004C File Offset: 0x001FE24C
	private void RefreshTogglePositionInRangeVisualizer()
	{
		if (this.rangeVisualizer != null)
		{
			RectTransform rectTransform = this.toggle.rectTransform();
			Vector3 worldPoint = rectTransform.TransformPoint(rectTransform.rect.center);
			Vector2 vector = RectTransformUtility.WorldToScreenPoint(null, worldPoint);
			Vector2 screenSpaceNotificationTogglePosition = new Vector2(vector.x / (float)Screen.width, vector.y / (float)Screen.height);
			this.rangeVisualizer.ScreenSpaceNotificationTogglePosition = screenSpaceNotificationTogglePosition;
		}
	}

	// Token: 0x06005801 RID: 22529 RVA: 0x002000C0 File Offset: 0x001FE2C0
	public void Sim200ms(float dt)
	{
		this.RefreshValues();
	}

	// Token: 0x06005802 RID: 22530 RVA: 0x002000C8 File Offset: 0x001FE2C8
	public void RefreshValues()
	{
		float fillAmount = (float)this.statusMonitor.Health / (float)this.statusMonitor.def.MAX_HEALTH;
		float largeImpactorTime = this.statusMonitor.TimeRemainingBeforeCollision / LargeImpactorEvent.GetImpactTime();
		this.healthbar.fillAmount = fillAmount;
		this.clock.SetLargeImpactorTime(largeImpactorTime);
		string[] array = GameUtil.GetFormattedCycles(this.statusMonitor.TimeRemainingBeforeCollision, "F1", false).Split(' ', StringSplitOptions.None);
		this.numberOfCyclesLabel.SetText(array[0]);
		if (this.rangeVisualizer != null && this.toggle.isOn != this.rangeVisualizer.Visible)
		{
			this.toggle.isOn = this.rangeVisualizer.Visible;
		}
	}

	// Token: 0x06005803 RID: 22531 RVA: 0x00200187 File Offset: 0x001FE387
	private void OnAsteroidDamaged(int newHealth)
	{
		this.hitEffects.PlayHitEffect();
		KFMOD.PlayUISound(GlobalAssets.GetSound("Notification_Imperative_hit", false));
		this.RefreshValues();
	}

	// Token: 0x06005804 RID: 22532 RVA: 0x002001AA File Offset: 0x001FE3AA
	public void ToggleVisibility(bool shouldBeVisible)
	{
		if (this.rangeVisualizer != null)
		{
			KFMOD.PlayUISound(GlobalAssets.GetSound(shouldBeVisible ? "HUD_Demolior_LandingZone_toggle_on" : "HUD_Demolior_LandingZone_toggle_off", false));
			this.RefreshTogglePositionInRangeVisualizer();
			this.rangeVisualizer.SetFoldedState(!shouldBeVisible);
		}
	}

	// Token: 0x06005805 RID: 22533 RVA: 0x002001EC File Offset: 0x001FE3EC
	public void OnPlayerClickedNotification()
	{
		GameUtil.FocusCamera(this.midSkyCell, true);
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click_Open ", false));
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Demolior_Click_focus", false));
		if (this.asteroidBackground != null)
		{
			this.asteroidBackground.PlayPlayerClickFeedback();
		}
	}

	// Token: 0x06005806 RID: 22534 RVA: 0x00200240 File Offset: 0x001FE440
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.cyclesLabelEffects.AbortCycleLabelFocusMonitor();
		if (this.statusMonitor != null)
		{
			LargeImpactorStatus.Instance instance = this.statusMonitor;
			instance.OnDamaged = (Action<int>)Delegate.Remove(instance.OnDamaged, new Action<int>(this.OnAsteroidDamaged));
		}
		Game.Instance.Unsubscribe(445618876, new Action<object>(this.OnScreenResolutionChanged));
		Game.Instance.Unsubscribe(-810220474, new Action<object>(this.OnScreenResolutionChanged));
	}

	// Token: 0x06005807 RID: 22535 RVA: 0x002002C3 File Offset: 0x001FE4C3
	protected override void OnCmpEnable()
	{
		if (base.isSpawned)
		{
			this.cyclesLabelEffects.InitializeCycleLabelFocusMonitor();
		}
	}

	// Token: 0x06005808 RID: 22536 RVA: 0x002002D8 File Offset: 0x001FE4D8
	protected override void OnCmpDisable()
	{
		this.cyclesLabelEffects.AbortCycleLabelFocusMonitor();
	}

	// Token: 0x04003AED RID: 15085
	public Image healthbar;

	// Token: 0x04003AEE RID: 15086
	public LargeImpactorNotificationUI_Clock clock;

	// Token: 0x04003AEF RID: 15087
	public KToggleSlider toggle;

	// Token: 0x04003AF0 RID: 15088
	public LargeImpactorUINotificationHitEffects hitEffects;

	// Token: 0x04003AF1 RID: 15089
	public LargeImpactorNotificationUI_CycleLabelEffects cyclesLabelEffects;

	// Token: 0x04003AF2 RID: 15090
	public LocText numberOfCyclesLabel;

	// Token: 0x04003AF3 RID: 15091
	private LargeImpactorStatus.Instance statusMonitor;

	// Token: 0x04003AF4 RID: 15092
	private LargeImpactorVisualizer rangeVisualizer;

	// Token: 0x04003AF5 RID: 15093
	private ParallaxBackgroundObject asteroidBackground;

	// Token: 0x04003AF6 RID: 15094
	private int midSkyCell = Grid.InvalidCell;

	// Token: 0x04003AF7 RID: 15095
	private const string Hit_SFX = "Notification_Imperative_hit";

	// Token: 0x04003AF8 RID: 15096
	private const string Click_SFX = "HUD_Click_Open ";

	// Token: 0x04003AF9 RID: 15097
	private const string Focus_SFX = "HUD_Demolior_Click_focus";

	// Token: 0x04003AFA RID: 15098
	private const string ToggleOff_SFX = "HUD_Demolior_LandingZone_toggle_off";

	// Token: 0x04003AFB RID: 15099
	private const string ToggleOn_SFX = "HUD_Demolior_LandingZone_toggle_on";
}
