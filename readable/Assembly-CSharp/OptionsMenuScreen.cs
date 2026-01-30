using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000DCA RID: 3530
public class OptionsMenuScreen : KModalButtonMenu
{
	// Token: 0x06006E49 RID: 28233 RVA: 0x0029BEC0 File Offset: 0x0029A0C0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.keepMenuOpen = true;
		this.buttons = new List<KButtonMenu.ButtonInfo>
		{
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.GRAPHICS, global::Action.NumActions, new UnityAction(this.OnGraphicsOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.AUDIO, global::Action.NumActions, new UnityAction(this.OnAudioOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.GAME, global::Action.NumActions, new UnityAction(this.OnGameOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.METRICS, global::Action.NumActions, new UnityAction(this.OnMetrics), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.FEEDBACK, global::Action.NumActions, new UnityAction(this.OnFeedback), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.CREDITS, global::Action.NumActions, new UnityAction(this.OnCredits), null, null)
		};
		this.closeButton.onClick += this.Deactivate;
		this.backButton.onClick += this.Deactivate;
	}

	// Token: 0x06006E4A RID: 28234 RVA: 0x0029C005 File Offset: 0x0029A205
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.title.SetText(UI.FRONTEND.OPTIONS_SCREEN.TITLE);
		this.backButton.transform.SetAsLastSibling();
	}

	// Token: 0x06006E4B RID: 28235 RVA: 0x0029C034 File Offset: 0x0029A234
	protected override void OnActivate()
	{
		base.OnActivate();
		foreach (GameObject gameObject in this.buttonObjects)
		{
		}
	}

	// Token: 0x06006E4C RID: 28236 RVA: 0x0029C060 File Offset: 0x0029A260
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006E4D RID: 28237 RVA: 0x0029C082 File Offset: 0x0029A282
	private void OnGraphicsOptions()
	{
		base.ActivateChildScreen(this.graphicsOptionsScreenPrefab.gameObject);
	}

	// Token: 0x06006E4E RID: 28238 RVA: 0x0029C096 File Offset: 0x0029A296
	private void OnAudioOptions()
	{
		base.ActivateChildScreen(this.audioOptionsScreenPrefab.gameObject);
	}

	// Token: 0x06006E4F RID: 28239 RVA: 0x0029C0AA File Offset: 0x0029A2AA
	private void OnGameOptions()
	{
		base.ActivateChildScreen(this.gameOptionsScreenPrefab.gameObject);
	}

	// Token: 0x06006E50 RID: 28240 RVA: 0x0029C0BE File Offset: 0x0029A2BE
	private void OnMetrics()
	{
		base.ActivateChildScreen(this.metricsScreenPrefab.gameObject);
	}

	// Token: 0x06006E51 RID: 28241 RVA: 0x0029C0D2 File Offset: 0x0029A2D2
	public void ShowMetricsScreen()
	{
		base.ActivateChildScreen(this.metricsScreenPrefab.gameObject);
	}

	// Token: 0x06006E52 RID: 28242 RVA: 0x0029C0E6 File Offset: 0x0029A2E6
	private void OnFeedback()
	{
		base.ActivateChildScreen(this.feedbackScreenPrefab.gameObject);
	}

	// Token: 0x06006E53 RID: 28243 RVA: 0x0029C0FA File Offset: 0x0029A2FA
	private void OnCredits()
	{
		base.ActivateChildScreen(this.creditsScreenPrefab.gameObject);
	}

	// Token: 0x06006E54 RID: 28244 RVA: 0x0029C10E File Offset: 0x0029A30E
	private void Update()
	{
		global::Debug.developerConsoleVisible = false;
	}

	// Token: 0x04004B58 RID: 19288
	[SerializeField]
	private GameOptionsScreen gameOptionsScreenPrefab;

	// Token: 0x04004B59 RID: 19289
	[SerializeField]
	private AudioOptionsScreen audioOptionsScreenPrefab;

	// Token: 0x04004B5A RID: 19290
	[SerializeField]
	private GraphicsOptionsScreen graphicsOptionsScreenPrefab;

	// Token: 0x04004B5B RID: 19291
	[SerializeField]
	private CreditsScreen creditsScreenPrefab;

	// Token: 0x04004B5C RID: 19292
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004B5D RID: 19293
	[SerializeField]
	private MetricsOptionsScreen metricsScreenPrefab;

	// Token: 0x04004B5E RID: 19294
	[SerializeField]
	private FeedbackScreen feedbackScreenPrefab;

	// Token: 0x04004B5F RID: 19295
	[SerializeField]
	private LocText title;

	// Token: 0x04004B60 RID: 19296
	[SerializeField]
	private KButton backButton;
}
