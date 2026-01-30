using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C78 RID: 3192
public class StoryMessageScreen : KScreen
{
	// Token: 0x17000709 RID: 1801
	// (set) Token: 0x0600616C RID: 24940 RVA: 0x0023ED4B File Offset: 0x0023CF4B
	public string title
	{
		set
		{
			this.titleLabel.SetText(value);
		}
	}

	// Token: 0x1700070A RID: 1802
	// (set) Token: 0x0600616D RID: 24941 RVA: 0x0023ED59 File Offset: 0x0023CF59
	public string body
	{
		set
		{
			this.bodyLabel.SetText(value);
		}
	}

	// Token: 0x0600616E RID: 24942 RVA: 0x0023ED67 File Offset: 0x0023CF67
	public override float GetSortKey()
	{
		return 8f;
	}

	// Token: 0x0600616F RID: 24943 RVA: 0x0023ED6E File Offset: 0x0023CF6E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		StoryMessageScreen.HideInterface(true);
		CameraController.Instance.FadeOut(0.5f, 1f, null);
	}

	// Token: 0x06006170 RID: 24944 RVA: 0x0023ED91 File Offset: 0x0023CF91
	private IEnumerator ExpandPanel()
	{
		this.content.gameObject.SetActive(true);
		yield return SequenceUtil.WaitForSecondsRealtime(0.25f);
		float height = 0f;
		while (height < 299f)
		{
			height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, 300f, Time.unscaledDeltaTime * 15f);
			this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
			yield return 0;
		}
		CameraController.Instance.FadeOut(0.5f, 1f, null);
		yield return null;
		yield break;
	}

	// Token: 0x06006171 RID: 24945 RVA: 0x0023EDA0 File Offset: 0x0023CFA0
	private IEnumerator CollapsePanel()
	{
		float height = 300f;
		while (height > 0f)
		{
			height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, -1f, Time.unscaledDeltaTime * 15f);
			this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
			yield return 0;
		}
		this.content.gameObject.SetActive(false);
		if (this.OnClose != null)
		{
			this.OnClose();
			this.OnClose = null;
		}
		this.Deactivate();
		yield return null;
		yield break;
	}

	// Token: 0x06006172 RID: 24946 RVA: 0x0023EDB0 File Offset: 0x0023CFB0
	public static void HideInterface(bool hide)
	{
		SelectTool.Instance.Select(null, true);
		NotificationScreen.Instance.Show(!hide);
		OverlayMenu.Instance.Show(!hide);
		if (PlanScreen.Instance != null)
		{
			PlanScreen.Instance.Show(!hide);
		}
		if (BuildMenu.Instance != null)
		{
			BuildMenu.Instance.Show(!hide);
		}
		ManagementMenu.Instance.Show(!hide);
		ToolMenu.Instance.Show(!hide);
		ToolMenu.Instance.PriorityScreen.Show(!hide);
		ColonyDiagnosticScreen.Instance.Show(!hide);
		PinnedResourcesPanel.Instance.Show(!hide);
		TopLeftControlScreen.Instance.Show(!hide);
		if (WorldSelector.Instance != null)
		{
			WorldSelector.Instance.Show(!hide);
		}
		global::DateTime.Instance.Show(!hide);
		if (BuildWatermark.Instance != null)
		{
			BuildWatermark.Instance.Show(!hide);
		}
		PopFXManager.Instance.Show(!hide);
	}

	// Token: 0x06006173 RID: 24947 RVA: 0x0023EEC8 File Offset: 0x0023D0C8
	public void Update()
	{
		if (!this.startFade)
		{
			return;
		}
		Color color = this.bg.color;
		color.a -= 0.01f;
		if (color.a <= 0f)
		{
			color.a = 0f;
		}
		this.bg.color = color;
	}

	// Token: 0x06006174 RID: 24948 RVA: 0x0023EF20 File Offset: 0x0023D120
	protected override void OnActivate()
	{
		base.OnActivate();
		SelectTool.Instance.Select(null, false);
		this.button.onClick += delegate()
		{
			base.StartCoroutine(this.CollapsePanel());
		};
		this.dialog.GetComponent<KScreen>().Show(false);
		this.startFade = false;
		CameraController.Instance.DisableUserCameraControl = true;
		KFMOD.PlayUISound(this.dialogSound);
		this.dialog.GetComponent<KScreen>().Activate();
		this.dialog.GetComponent<KScreen>().SetShouldFadeIn(true);
		this.dialog.GetComponent<KScreen>().Show(true);
		MusicManager.instance.PlaySong("Music_Victory_01_Message", false);
		base.StartCoroutine(this.ExpandPanel());
	}

	// Token: 0x06006175 RID: 24949 RVA: 0x0023EFD4 File Offset: 0x0023D1D4
	protected override void OnDeactivate()
	{
		base.IsActive();
		base.OnDeactivate();
		MusicManager.instance.StopSong("Music_Victory_01_Message", true, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (this.restoreInterfaceOnClose)
		{
			CameraController.Instance.DisableUserCameraControl = false;
			CameraController.Instance.FadeIn(0f, 1f, null);
			StoryMessageScreen.HideInterface(false);
		}
	}

	// Token: 0x06006176 RID: 24950 RVA: 0x0023F02D File Offset: 0x0023D22D
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			base.StartCoroutine(this.CollapsePanel());
		}
		e.Consumed = true;
	}

	// Token: 0x06006177 RID: 24951 RVA: 0x0023F04C File Offset: 0x0023D24C
	public override void OnKeyUp(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x0400412F RID: 16687
	private const float ALPHA_SPEED = 0.01f;

	// Token: 0x04004130 RID: 16688
	[SerializeField]
	private Image bg;

	// Token: 0x04004131 RID: 16689
	[SerializeField]
	private GameObject dialog;

	// Token: 0x04004132 RID: 16690
	[SerializeField]
	private KButton button;

	// Token: 0x04004133 RID: 16691
	[SerializeField]
	private EventReference dialogSound;

	// Token: 0x04004134 RID: 16692
	[SerializeField]
	private LocText titleLabel;

	// Token: 0x04004135 RID: 16693
	[SerializeField]
	private LocText bodyLabel;

	// Token: 0x04004136 RID: 16694
	private const float expandedHeight = 300f;

	// Token: 0x04004137 RID: 16695
	[SerializeField]
	private GameObject content;

	// Token: 0x04004138 RID: 16696
	public bool restoreInterfaceOnClose = true;

	// Token: 0x04004139 RID: 16697
	public System.Action OnClose;

	// Token: 0x0400413A RID: 16698
	private bool startFade;
}
