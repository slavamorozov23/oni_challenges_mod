using System;
using UnityEngine;

// Token: 0x02000D69 RID: 3433
[AddComponentMenu("KMonoBehaviour/scripts/MainMenuIntroShort")]
public class MainMenuIntroShort : KMonoBehaviour
{
	// Token: 0x06006A73 RID: 27251 RVA: 0x00284530 File Offset: 0x00282730
	protected override void OnSpawn()
	{
		base.OnSpawn();
		string @string = KPlayerPrefs.GetString("PlayShortOnLaunch", "");
		if (!string.IsNullOrEmpty(MainMenu.Instance.IntroShortName) && @string != MainMenu.Instance.IntroShortName)
		{
			VideoScreen component = KScreenManager.AddChild(FrontEndManager.Instance.gameObject, ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
			component.PlayVideo(Assets.GetVideo(MainMenu.Instance.IntroShortName), false, AudioMixerSnapshots.Get().MainMenuVideoPlayingSnapshot, false, true);
			component.OnStop = (System.Action)Delegate.Combine(component.OnStop, new System.Action(delegate()
			{
				KPlayerPrefs.SetString("PlayShortOnLaunch", MainMenu.Instance.IntroShortName);
				base.gameObject.SetActive(false);
			}));
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400493B RID: 18747
	[SerializeField]
	private bool alwaysPlay;
}
