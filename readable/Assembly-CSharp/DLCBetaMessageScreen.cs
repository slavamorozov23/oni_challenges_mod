using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000CED RID: 3309
public class DLCBetaMessageScreen : KModalScreen
{
	// Token: 0x06006625 RID: 26149 RVA: 0x00266F2C File Offset: 0x0026512C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
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

	// Token: 0x06006626 RID: 26150 RVA: 0x00266F80 File Offset: 0x00265180
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!this.betaIsLive || (Application.isEditor && this.skipInEditor) || !DlcManager.IsContentSubscribed("DLC4_ID"))
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
	}

	// Token: 0x06006627 RID: 26151 RVA: 0x00266FD7 File Offset: 0x002651D7
	private void Update()
	{
		this.logo.rectTransform().localPosition = new Vector3(0f, Mathf.Sin(Time.realtimeSinceStartup) * 7.5f);
	}

	// Token: 0x040045AD RID: 17837
	public RectTransform logo;

	// Token: 0x040045AE RID: 17838
	public KButton confirmButton;

	// Token: 0x040045AF RID: 17839
	public KButton quitButton;

	// Token: 0x040045B0 RID: 17840
	public LocText bodyText;

	// Token: 0x040045B1 RID: 17841
	public RectTransform messageContainer;

	// Token: 0x040045B2 RID: 17842
	private bool betaIsLive;

	// Token: 0x040045B3 RID: 17843
	private bool skipInEditor;
}
