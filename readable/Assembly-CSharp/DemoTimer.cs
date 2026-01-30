using System;
using Klei;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CF4 RID: 3316
public class DemoTimer : MonoBehaviour
{
	// Token: 0x06006679 RID: 26233 RVA: 0x0026932F File Offset: 0x0026752F
	public static void DestroyInstance()
	{
		DemoTimer.Instance = null;
	}

	// Token: 0x0600667A RID: 26234 RVA: 0x00269338 File Offset: 0x00267538
	private void Start()
	{
		DemoTimer.Instance = this;
		if (GenericGameSettings.instance != null)
		{
			if (GenericGameSettings.instance.demoMode)
			{
				this.duration = (float)GenericGameSettings.instance.demoTime;
				this.labelText.gameObject.SetActive(GenericGameSettings.instance.showDemoTimer);
				this.clockImage.gameObject.SetActive(GenericGameSettings.instance.showDemoTimer);
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
		this.duration = (float)GenericGameSettings.instance.demoTime;
		this.fadeOutScreen = Util.KInstantiateUI(this.Prefab_FadeOutScreen, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false);
		Image component = this.fadeOutScreen.GetComponent<Image>();
		component.raycastTarget = false;
		this.fadeOutColor = component.color;
		this.fadeOutColor.a = 0f;
		this.fadeOutScreen.GetComponent<Image>().color = this.fadeOutColor;
	}

	// Token: 0x0600667B RID: 26235 RVA: 0x00269438 File Offset: 0x00267638
	private void Update()
	{
		if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.BackQuote))
		{
			this.CountdownActive = !this.CountdownActive;
			this.UpdateLabel();
		}
		if (this.demoOver || !this.CountdownActive)
		{
			return;
		}
		if (this.beginTime == -1f)
		{
			this.beginTime = Time.unscaledTime;
		}
		this.elapsed = Mathf.Clamp(0f, Time.unscaledTime - this.beginTime, this.duration);
		if (this.elapsed + 5f >= this.duration)
		{
			float f = (this.duration - this.elapsed) / 5f;
			this.fadeOutColor.a = Mathf.Min(1f, 1f - Mathf.Sqrt(f));
			this.fadeOutScreen.GetComponent<Image>().color = this.fadeOutColor;
		}
		if (this.elapsed >= this.duration)
		{
			this.EndDemo();
		}
		this.UpdateLabel();
	}

	// Token: 0x0600667C RID: 26236 RVA: 0x00269540 File Offset: 0x00267740
	private void UpdateLabel()
	{
		int num = Mathf.RoundToInt(this.duration - this.elapsed);
		int num2 = Mathf.FloorToInt((float)(num / 60));
		int num3 = num % 60;
		this.labelText.text = string.Concat(new string[]
		{
			UI.DEMOOVERSCREEN.TIMEREMAINING,
			" ",
			num2.ToString("00"),
			":",
			num3.ToString("00")
		});
		if (!this.CountdownActive)
		{
			this.labelText.text = UI.DEMOOVERSCREEN.TIMERINACTIVE;
		}
	}

	// Token: 0x0600667D RID: 26237 RVA: 0x002695DC File Offset: 0x002677DC
	public void EndDemo()
	{
		if (this.demoOver)
		{
			return;
		}
		this.demoOver = true;
		Util.KInstantiateUI(this.Prefab_DemoOverScreen, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false).GetComponent<DemoOverScreen>().Show(true);
	}

	// Token: 0x040045F6 RID: 17910
	public static DemoTimer Instance;

	// Token: 0x040045F7 RID: 17911
	public LocText labelText;

	// Token: 0x040045F8 RID: 17912
	public Image clockImage;

	// Token: 0x040045F9 RID: 17913
	public GameObject Prefab_DemoOverScreen;

	// Token: 0x040045FA RID: 17914
	public GameObject Prefab_FadeOutScreen;

	// Token: 0x040045FB RID: 17915
	private float duration;

	// Token: 0x040045FC RID: 17916
	private float elapsed;

	// Token: 0x040045FD RID: 17917
	private bool demoOver;

	// Token: 0x040045FE RID: 17918
	private float beginTime = -1f;

	// Token: 0x040045FF RID: 17919
	public bool CountdownActive;

	// Token: 0x04004600 RID: 17920
	private GameObject fadeOutScreen;

	// Token: 0x04004601 RID: 17921
	private Color fadeOutColor;
}
