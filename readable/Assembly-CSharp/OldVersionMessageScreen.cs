using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000DC8 RID: 3528
[AddComponentMenu("KMonoBehaviour/scripts/SplashMessageScreen")]
public class OldVersionMessageScreen : KModalScreen
{
	// Token: 0x06006E3D RID: 28221 RVA: 0x0029BBBC File Offset: 0x00299DBC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.forumButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/forums/topic/140474-previous-update-steam-branch-access/");
		};
		this.confirmButton.onClick += delegate()
		{
			base.gameObject.SetActive(false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot, STOP_MODE.ALLOWFADEOUT);
		};
		this.quitButton.onClick += delegate()
		{
			App.Quit();
		};
	}

	// Token: 0x06006E3E RID: 28222 RVA: 0x0029BC3C File Offset: 0x00299E3C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.messageContainer.sizeDelta = new Vector2(Mathf.Max(384f, (float)Screen.width * 0.25f), this.messageContainer.sizeDelta.y);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
	}

	// Token: 0x04004B4E RID: 19278
	public KButton forumButton;

	// Token: 0x04004B4F RID: 19279
	public KButton confirmButton;

	// Token: 0x04004B50 RID: 19280
	public KButton quitButton;

	// Token: 0x04004B51 RID: 19281
	public LocText bodyText;

	// Token: 0x04004B52 RID: 19282
	public bool previewInEditor;

	// Token: 0x04004B53 RID: 19283
	public RectTransform messageContainer;
}
