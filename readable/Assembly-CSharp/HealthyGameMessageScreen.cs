using System;
using UnityEngine;

// Token: 0x02000D23 RID: 3363
[AddComponentMenu("KMonoBehaviour/scripts/HealthyGameMessageScreen")]
public class HealthyGameMessageScreen : KMonoBehaviour
{
	// Token: 0x0600680A RID: 26634 RVA: 0x0027487B File Offset: 0x00272A7B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.confirmButton.onClick += delegate()
		{
			this.PlayIntroShort();
		};
		this.confirmButton.gameObject.SetActive(false);
	}

	// Token: 0x0600680B RID: 26635 RVA: 0x002748AC File Offset: 0x00272AAC
	private void PlayIntroShort()
	{
		string @string = KPlayerPrefs.GetString("PlayShortOnLaunch", "");
		if (!string.IsNullOrEmpty(MainMenu.Instance.IntroShortName) && @string != MainMenu.Instance.IntroShortName)
		{
			VideoScreen component = KScreenManager.AddChild(FrontEndManager.Instance.gameObject, ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
			component.PlayVideo(Assets.GetVideo(MainMenu.Instance.IntroShortName), false, AudioMixerSnapshots.Get().MainMenuVideoPlayingSnapshot, false, true);
			component.OnStop = (System.Action)Delegate.Combine(component.OnStop, new System.Action(delegate()
			{
				KPlayerPrefs.SetString("PlayShortOnLaunch", MainMenu.Instance.IntroShortName);
				if (base.gameObject != null)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}));
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600680C RID: 26636 RVA: 0x0027495E File Offset: 0x00272B5E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600680D RID: 26637 RVA: 0x00274974 File Offset: 0x00272B74
	private void Update()
	{
		if (!DistributionPlatform.Inst.IsDLCStatusReady())
		{
			return;
		}
		if (this.isFirstUpdate)
		{
			this.isFirstUpdate = false;
			this.spawnTime = Time.unscaledTime;
			return;
		}
		float num = Mathf.Min(Time.unscaledDeltaTime, 0.033333335f);
		float num2 = Time.unscaledTime - this.spawnTime;
		if (num2 < this.totalTime - this.fadeTime)
		{
			this.canvasGroup.alpha = this.canvasGroup.alpha + num * (1f / this.fadeTime);
			return;
		}
		if (num2 >= this.totalTime + 0.75f)
		{
			this.canvasGroup.alpha = 1f;
			this.confirmButton.gameObject.SetActive(true);
			return;
		}
		if (num2 >= this.totalTime - this.fadeTime)
		{
			this.canvasGroup.alpha = this.canvasGroup.alpha - num * (1f / this.fadeTime);
		}
	}

	// Token: 0x04004779 RID: 18297
	public KButton confirmButton;

	// Token: 0x0400477A RID: 18298
	public CanvasGroup canvasGroup;

	// Token: 0x0400477B RID: 18299
	private float spawnTime;

	// Token: 0x0400477C RID: 18300
	private float totalTime = 10f;

	// Token: 0x0400477D RID: 18301
	private float fadeTime = 1.5f;

	// Token: 0x0400477E RID: 18302
	private bool isFirstUpdate = true;
}
