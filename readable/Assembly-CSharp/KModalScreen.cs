using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D3B RID: 3387
public class KModalScreen : KScreen
{
	// Token: 0x060068CA RID: 26826 RVA: 0x0027AFC4 File Offset: 0x002791C4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.backgroundRectTransform = KModalScreen.MakeScreenModal(this);
	}

	// Token: 0x060068CB RID: 26827 RVA: 0x0027AFD8 File Offset: 0x002791D8
	public static RectTransform MakeScreenModal(KScreen screen)
	{
		screen.ConsumeMouseScroll = true;
		screen.activateOnSpawn = true;
		GameObject gameObject = new GameObject("background");
		gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
		gameObject.AddComponent<CanvasRenderer>();
		Image image = gameObject.AddComponent<Image>();
		image.color = new Color32(0, 0, 0, 160);
		image.raycastTarget = true;
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.SetParent(screen.transform);
		KModalScreen.ResizeBackground(component);
		return component;
	}

	// Token: 0x060068CC RID: 26828 RVA: 0x0027B04C File Offset: 0x0027924C
	public static void ResizeBackground(RectTransform rectTransform)
	{
		rectTransform.SetAsFirstSibling();
		rectTransform.SetLocalPosition(Vector3.zero);
		rectTransform.localScale = Vector3.one;
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
	}

	// Token: 0x060068CD RID: 26829 RVA: 0x0027B0B8 File Offset: 0x002792B8
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (CameraController.Instance != null)
		{
			CameraController.Instance.DisableUserCameraControl = true;
		}
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		}
	}

	// Token: 0x060068CE RID: 26830 RVA: 0x0027B118 File Offset: 0x00279318
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (CameraController.Instance != null)
		{
			CameraController.Instance.DisableUserCameraControl = false;
		}
		base.Trigger(476357528, null);
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Remove(instance.OnResize, new System.Action(this.OnResize));
		}
	}

	// Token: 0x060068CF RID: 26831 RVA: 0x0027B182 File Offset: 0x00279382
	private void OnResize()
	{
		KModalScreen.ResizeBackground(this.backgroundRectTransform);
	}

	// Token: 0x060068D0 RID: 26832 RVA: 0x0027B18F File Offset: 0x0027938F
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x060068D1 RID: 26833 RVA: 0x0027B192 File Offset: 0x00279392
	public override float GetSortKey()
	{
		return 100f;
	}

	// Token: 0x060068D2 RID: 26834 RVA: 0x0027B199 File Offset: 0x00279399
	protected override void OnActivate()
	{
		this.OnShow(true);
	}

	// Token: 0x060068D3 RID: 26835 RVA: 0x0027B1A2 File Offset: 0x002793A2
	protected override void OnDeactivate()
	{
		this.OnShow(false);
	}

	// Token: 0x060068D4 RID: 26836 RVA: 0x0027B1AC File Offset: 0x002793AC
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (this.pause && SpeedControlScreen.Instance != null)
		{
			if (show && !this.shown)
			{
				SpeedControlScreen.Instance.Pause(false, false);
			}
			else if (!show && this.shown)
			{
				SpeedControlScreen.Instance.Unpause(false);
			}
			this.shown = show;
		}
	}

	// Token: 0x060068D5 RID: 26837 RVA: 0x0027B20C File Offset: 0x0027940C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (Game.Instance != null && (e.TryConsume(global::Action.TogglePause) || e.TryConsume(global::Action.CycleSpeed)))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		if (!e.Consumed && (e.TryConsume(global::Action.Escape) || (e.TryConsume(global::Action.MouseRight) && this.canBackoutWithRightClick)))
		{
			this.Deactivate();
		}
		base.OnKeyDown(e);
		e.Consumed = true;
	}

	// Token: 0x060068D6 RID: 26838 RVA: 0x0027B289 File Offset: 0x00279489
	public override void OnKeyUp(KButtonEvent e)
	{
		base.OnKeyUp(e);
		e.Consumed = true;
	}

	// Token: 0x04004802 RID: 18434
	private bool shown;

	// Token: 0x04004803 RID: 18435
	public bool pause = true;

	// Token: 0x04004804 RID: 18436
	[Tooltip("Only used for main menu")]
	public bool canBackoutWithRightClick;

	// Token: 0x04004805 RID: 18437
	private RectTransform backgroundRectTransform;
}
