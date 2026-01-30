using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000EA0 RID: 3744
[AddComponentMenu("KMonoBehaviour/scripts/SplashMessageScreen")]
public class SplashMessageScreen : KMonoBehaviour
{
	// Token: 0x060077C4 RID: 30660 RVA: 0x002DECFC File Offset: 0x002DCEFC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.forumButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/forums/forum/118-oxygen-not-included/");
		};
		this.confirmButton.onClick += delegate()
		{
			base.gameObject.SetActive(false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot, STOP_MODE.ALLOWFADEOUT);
		};
		this.bodyText.text = UI.DEVELOPMENTBUILDS.ALPHA.LOADING.BODY;
	}

	// Token: 0x060077C5 RID: 30661 RVA: 0x002DED65 File Offset: 0x002DCF65
	private void OnEnable()
	{
		this.confirmButton.GetComponent<LayoutElement>();
		this.confirmButton.GetComponentInChildren<LocText>();
	}

	// Token: 0x060077C6 RID: 30662 RVA: 0x002DED7F File Offset: 0x002DCF7F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!DlcManager.IsExpansion1Active())
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
	}

	// Token: 0x04005342 RID: 21314
	public KButton forumButton;

	// Token: 0x04005343 RID: 21315
	public KButton confirmButton;

	// Token: 0x04005344 RID: 21316
	public LocText bodyText;

	// Token: 0x04005345 RID: 21317
	public bool previewInEditor;
}
