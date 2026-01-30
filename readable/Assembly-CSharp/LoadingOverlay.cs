using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000C62 RID: 3170
public class LoadingOverlay : KModalScreen
{
	// Token: 0x06006082 RID: 24706 RVA: 0x002373EE File Offset: 0x002355EE
	protected override void OnPrefabInit()
	{
		this.pause = false;
		this.fadeIn = false;
		base.OnPrefabInit();
	}

	// Token: 0x06006083 RID: 24707 RVA: 0x00237404 File Offset: 0x00235604
	private void Update()
	{
		if (!this.loadNextFrame && this.showLoad)
		{
			this.loadNextFrame = true;
			this.showLoad = false;
			return;
		}
		if (this.loadNextFrame)
		{
			this.loadNextFrame = false;
			this.loadCb();
		}
	}

	// Token: 0x06006084 RID: 24708 RVA: 0x0023743F File Offset: 0x0023563F
	public static void DestroyInstance()
	{
		LoadingOverlay.instance = null;
	}

	// Token: 0x06006085 RID: 24709 RVA: 0x00237448 File Offset: 0x00235648
	public static void Load(System.Action cb)
	{
		GameObject gameObject = GameObject.Find("/SceneInitializerFE/FrontEndManager");
		if (LoadingOverlay.instance == null)
		{
			LoadingOverlay.instance = Util.KInstantiateUI<LoadingOverlay>(ScreenPrefabs.Instance.loadingOverlay.gameObject, (GameScreenManager.Instance == null) ? gameObject : GameScreenManager.Instance.ssOverlayCanvas, false);
			LoadingOverlay.instance.GetComponentInChildren<LocText>().SetText(UI.FRONTEND.LOADING);
		}
		if (GameScreenManager.Instance != null)
		{
			LoadingOverlay.instance.transform.SetParent(GameScreenManager.Instance.ssOverlayCanvas.transform);
			LoadingOverlay.instance.transform.SetSiblingIndex(GameScreenManager.Instance.ssOverlayCanvas.transform.childCount - 1);
		}
		else
		{
			LoadingOverlay.instance.transform.SetParent(gameObject.transform);
			LoadingOverlay.instance.transform.SetSiblingIndex(gameObject.transform.childCount - 1);
			if (MainMenu.Instance != null)
			{
				MainMenu.Instance.StopAmbience();
			}
		}
		LoadingOverlay.instance.loadCb = cb;
		LoadingOverlay.instance.showLoad = true;
		LoadingOverlay.instance.Activate();
	}

	// Token: 0x06006086 RID: 24710 RVA: 0x00237574 File Offset: 0x00235774
	public static void Clear()
	{
		if (LoadingOverlay.instance != null)
		{
			LoadingOverlay.instance.Deactivate();
		}
	}

	// Token: 0x04004071 RID: 16497
	private bool loadNextFrame;

	// Token: 0x04004072 RID: 16498
	private bool showLoad;

	// Token: 0x04004073 RID: 16499
	private System.Action loadCb;

	// Token: 0x04004074 RID: 16500
	private static LoadingOverlay instance;
}
