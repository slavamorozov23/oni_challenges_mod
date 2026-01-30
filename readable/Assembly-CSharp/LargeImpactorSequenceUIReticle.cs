using System;
using System.Collections;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B89 RID: 2953
public class LargeImpactorSequenceUIReticle : KMonoBehaviour
{
	// Token: 0x0600581D RID: 22557 RVA: 0x00200592 File Offset: 0x001FE792
	protected override void OnPrefabInit()
	{
		this.transform = (base.transform as RectTransform);
		this.bgOriginalColor = this.bg.color;
		this.calculatingImpactLabelOriginalColor = this.calculateImpactLabel.color;
		base.OnPrefabInit();
	}

	// Token: 0x0600581E RID: 22558 RVA: 0x002005CD File Offset: 0x001FE7CD
	protected override void OnSpawn()
	{
		this.ResetGraphics();
	}

	// Token: 0x0600581F RID: 22559 RVA: 0x002005D5 File Offset: 0x001FE7D5
	public void Run(System.Action onPhase1Completed = null, System.Action onComplete = null)
	{
		this.SetVisibility(true);
		this.AbortCoroutine();
		this.ResetGraphics();
		this.onPhase1Completed = onPhase1Completed;
		this.onComplete = onComplete;
		this.InitializeAndRunCoroutine();
	}

	// Token: 0x06005820 RID: 22560 RVA: 0x002005FE File Offset: 0x001FE7FE
	public void Hide()
	{
		this.AbortCoroutine();
		this.ResetGraphics();
		this.SetVisibility(false);
	}

	// Token: 0x06005821 RID: 22561 RVA: 0x00200613 File Offset: 0x001FE813
	private void SetVisibility(bool visible)
	{
		this.isVisible = visible;
		this.label.enabled = visible;
		this.bg.enabled = visible;
		this.border.enabled = visible;
	}

	// Token: 0x06005822 RID: 22562 RVA: 0x00200640 File Offset: 0x001FE840
	private void InitializeAndRunCoroutine()
	{
		this.coroutine = base.StartCoroutine(this.EnterSequence());
	}

	// Token: 0x06005823 RID: 22563 RVA: 0x00200654 File Offset: 0x001FE854
	private void AbortCoroutine()
	{
		base.StopAllCoroutines();
		this.coroutine = null;
	}

	// Token: 0x06005824 RID: 22564 RVA: 0x00200663 File Offset: 0x001FE863
	public void SetTarget(LargeImpactorStatus.Instance largeImpactorStatus)
	{
		this.largeImpactorStatus = largeImpactorStatus;
		this.loopingSounds = largeImpactorStatus.GetComponent<LoopingSounds>();
	}

	// Token: 0x06005825 RID: 22565 RVA: 0x00200678 File Offset: 0x001FE878
	private void ResetGraphics()
	{
		this.label.SetText("");
		this.border.Opacity(0f);
		this.bg.color = this.bgOriginalColor;
		this.bg.Opacity(0f);
		this.sidePanelIcon.Opacity(0f);
		this.sidePanelTitleLabel.SetText("");
		this.calculateImpactLabel.SetText("");
		this.sidePanelDescriptionLabel.SetText("");
		this.calculateImpactLabel.color = this.calculatingImpactLabelOriginalColor;
	}

	// Token: 0x06005826 RID: 22566 RVA: 0x00200718 File Offset: 0x001FE918
	private void PlayLoopingSound(string soundName)
	{
		string sound = GlobalAssets.GetSound(soundName, false);
		this.loopingSounds.StartSound(sound, false, false, false);
	}

	// Token: 0x06005827 RID: 22567 RVA: 0x0020073D File Offset: 0x001FE93D
	private IEnumerator EnterSequence()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Imperative_analysis_start", false));
		yield return this.Interpolate(delegate(float n)
		{
			this.transform.sizeDelta = Vector2.Lerp(this.initialSize * 2f, this.initialSize, n);
			this.border.Opacity(n);
		}, 0.4f, null);
		if (this.bg.color != this.border.color)
		{
			this.bg.color = this.border.color;
		}
		yield return this.Interpolate(delegate(float n)
		{
			this.bg.Opacity(Mathf.Abs(Mathf.Sin(n * 3.1415927f * 3f)));
		}, 0.4f, delegate()
		{
			this.bg.color = this.bgOriginalColor;
		});
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Imperative_bracket_open_first", false));
		yield return this.Interpolate(delegate(float n)
		{
			this.bg.Opacity(n * this.bgOriginalColor.a);
			this.transform.sizeDelta = Vector2.Lerp(this.initialSize, this.zoomedOutSize, n);
		}, 0.8f, null);
		this.PlayLoopingSound("HUD_Imperative_Text_typing_header");
		string titleText = MISC.NOTIFICATIONS.LARGEIMPACTORREVEALSEQUENCE.RETICLE.LARGE_IMPACTOR_NAME;
		yield return this.Interpolate(delegate(float n)
		{
			SequenceTools.TextWriter(this.label, titleText, n);
		}, 1f, null);
		this.loopingSounds.StopSound(GlobalAssets.GetSound("HUD_Imperative_Text_typing_header", false));
		yield return null;
		this.PlayLoopingSound("HUD_Imperative_Text_typing_header");
		this.sidePanelIcon.color = Color.white;
		string sidePanelTitleText = MISC.NOTIFICATIONS.LARGEIMPACTORREVEALSEQUENCE.RETICLE.SIDE_PANEL_TITLE;
		yield return this.Interpolate(delegate(float n)
		{
			this.sidePanelIcon.Opacity(n);
			this.sidePanelIcon.transform.localRotation = Quaternion.Euler(0f, Mathf.Lerp(90f, 0f, n), 0f);
			SequenceTools.TextWriter(this.sidePanelTitleLabel, sidePanelTitleText, n);
		}, 0.5f, null);
		this.loopingSounds.StopSound(GlobalAssets.GetSound("HUD_Imperative_Text_typing_header", false));
		this.PlayLoopingSound("HUD_Imperative_Text_typing_body");
		string sidePanelDescriptionText = GameUtil.SafeStringFormat(MISC.NOTIFICATIONS.LARGEIMPACTORREVEALSEQUENCE.RETICLE.SIDE_PANEL_DESCRIPTION, new object[]
		{
			GameUtil.GetFormattedCycles(this.largeImpactorStatus.TimeRemainingBeforeCollision, "F1", false).Split(' ', StringSplitOptions.None)[0]
		});
		yield return this.Interpolate(delegate(float n)
		{
			SequenceTools.TextWriter(this.sidePanelDescriptionLabel, sidePanelDescriptionText, n);
		}, 1.5f, null);
		this.loopingSounds.StopSound(GlobalAssets.GetSound("HUD_Imperative_Text_typing_body", false));
		yield return new WaitForSecondsRealtime(2f);
		if (this.onPhase1Completed != null)
		{
			this.onPhase1Completed();
			this.onPhase1Completed = null;
		}
		yield return this.Interpolate(delegate(float n)
		{
			SequenceTools.TextEraser(this.label, titleText, n);
			SequenceTools.TextEraser(this.sidePanelTitleLabel, sidePanelTitleText, n);
			SequenceTools.TextEraser(this.sidePanelDescriptionLabel, sidePanelDescriptionText, n);
			this.sidePanelIcon.color = Color.Lerp(Color.white, Color.red, n);
			this.sidePanelIcon.Opacity(1f - n);
		}, 0.5f, null);
		Color bgColor = this.bg.color;
		Color targetBgColor = Color.Lerp(this.bgOriginalColor, Color.black, 0.5f);
		targetBgColor.a = this.bgOriginalColor.a * 0.8f;
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Imperative_bracket_open_second", false));
		yield return this.Interpolate(delegate(float n)
		{
			this.bg.color = Color.Lerp(bgColor, targetBgColor, n);
			this.transform.sizeDelta = Vector2.Lerp(this.zoomedOutSize, this.calculatingImpactSize, n);
		}, 0.8f, null);
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Imperative_calculating_beep", false));
		Coroutine flashLabelCoroutine = null;
		this.Interpolate(delegate(float n)
		{
			this.calculateImpactLabel.color = Color.Lerp(Color.white, Color.red, Mathf.Abs(Mathf.Sin(n * 3.1415927f * 999f)));
		}, 999f, out flashLabelCoroutine, null);
		yield return this.Interpolate(delegate(float n)
		{
			SequenceTools.TextWriter(this.calculateImpactLabel, MISC.NOTIFICATIONS.LARGEIMPACTORREVEALSEQUENCE.RETICLE.CALCULATING_IMPACT_ZONE_TEXT, n);
		}, 0.5f, null);
		yield return new WaitForSecondsRealtime(3.5f);
		base.StopCoroutine(flashLabelCoroutine);
		flashLabelCoroutine = null;
		Color impactLabelColor = this.calculateImpactLabel.color;
		yield return this.Interpolate(delegate(float n)
		{
			float num = Mathf.Sqrt(1f - n);
			float t = Mathf.Sqrt(n);
			this.bg.Opacity(this.bgOriginalColor.a * num);
			this.border.Opacity(num);
			this.calculateImpactLabel.Opacity(impactLabelColor.a * num);
			this.transform.sizeDelta = Vector2.Lerp(this.calculatingImpactSize, this.calculatingImpactSize * 1.3f, t);
		}, 0.8f, null);
		if (this.onComplete != null)
		{
			this.onComplete();
			this.onComplete = null;
		}
		yield break;
	}

	// Token: 0x06005828 RID: 22568 RVA: 0x0020074C File Offset: 0x001FE94C
	protected override void OnCmpDisable()
	{
		this.AbortCoroutine();
	}

	// Token: 0x06005829 RID: 22569 RVA: 0x00200754 File Offset: 0x001FE954
	protected override void OnCmpEnable()
	{
		if (this.isVisible)
		{
			this.AbortCoroutine();
			this.ResetGraphics();
			this.InitializeAndRunCoroutine();
		}
	}

	// Token: 0x04003B13 RID: 15123
	private const float reticleEnterDuration = 0.4f;

	// Token: 0x04003B14 RID: 15124
	private const float flashDuration = 0.4f;

	// Token: 0x04003B15 RID: 15125
	private const int flashTimes = 3;

	// Token: 0x04003B16 RID: 15126
	private const float reticleZoomOutDuration = 0.8f;

	// Token: 0x04003B17 RID: 15127
	private const float labelRevealDuration = 1f;

	// Token: 0x04003B18 RID: 15128
	private const float sidePanel_TitleRevealDuration = 0.5f;

	// Token: 0x04003B19 RID: 15129
	private const float sidePanel_DescriptionRevealDuration = 1.5f;

	// Token: 0x04003B1A RID: 15130
	private const float exitToCalculationDuration = 0.5f;

	// Token: 0x04003B1B RID: 15131
	private const float expandReticleHorizontallyDuration = 0.8f;

	// Token: 0x04003B1C RID: 15132
	private const float calculateImpactZoneTextRevealDuration = 0.5f;

	// Token: 0x04003B1D RID: 15133
	private const float exitDuration = 0.8f;

	// Token: 0x04003B1E RID: 15134
	public const float RevealPOI_LandingZone_Duration = 3.5f;

	// Token: 0x04003B1F RID: 15135
	private const string Sound_LockTarget = "HUD_Imperative_analysis_start";

	// Token: 0x04003B20 RID: 15136
	private const string Sound_BracketSquareExpand = "HUD_Imperative_bracket_open_first";

	// Token: 0x04003B21 RID: 15137
	private const string Sound_BracketExpandsForCalculatingLandingZone = "HUD_Imperative_bracket_open_second";

	// Token: 0x04003B22 RID: 15138
	private const string Sound_CalculatingLandingZoneTextAppears = "HUD_Imperative_calculating_beep";

	// Token: 0x04003B23 RID: 15139
	private const string Sound_TypeHeader = "HUD_Imperative_Text_typing_header";

	// Token: 0x04003B24 RID: 15140
	private const string Sound_TypeBody = "HUD_Imperative_Text_typing_body";

	// Token: 0x04003B25 RID: 15141
	public Vector2 initialSize = new Vector2(100f, 100f);

	// Token: 0x04003B26 RID: 15142
	public Vector2 zoomedOutSize = new Vector2(180f, 180f);

	// Token: 0x04003B27 RID: 15143
	public Vector2 calculatingImpactSize = new Vector2(500f, 120f);

	// Token: 0x04003B28 RID: 15144
	[Space]
	public LocText label;

	// Token: 0x04003B29 RID: 15145
	public LocText sidePanelTitleLabel;

	// Token: 0x04003B2A RID: 15146
	public LocText sidePanelDescriptionLabel;

	// Token: 0x04003B2B RID: 15147
	public LocText calculateImpactLabel;

	// Token: 0x04003B2C RID: 15148
	public Image bg;

	// Token: 0x04003B2D RID: 15149
	public Image border;

	// Token: 0x04003B2E RID: 15150
	public Image sidePanelIcon;

	// Token: 0x04003B2F RID: 15151
	private new RectTransform transform;

	// Token: 0x04003B30 RID: 15152
	private LargeImpactorStatus.Instance largeImpactorStatus;

	// Token: 0x04003B31 RID: 15153
	private LoopingSounds loopingSounds;

	// Token: 0x04003B32 RID: 15154
	private bool isVisible;

	// Token: 0x04003B33 RID: 15155
	private Color bgOriginalColor;

	// Token: 0x04003B34 RID: 15156
	private Color calculatingImpactLabelOriginalColor;

	// Token: 0x04003B35 RID: 15157
	private System.Action onPhase1Completed;

	// Token: 0x04003B36 RID: 15158
	private System.Action onComplete;

	// Token: 0x04003B37 RID: 15159
	private Coroutine coroutine;
}
