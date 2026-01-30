using System;
using UnityEngine;

// Token: 0x02000D3A RID: 3386
public class KModalButtonMenu : KButtonMenu
{
	// Token: 0x060068BC RID: 26812 RVA: 0x0027ADD9 File Offset: 0x00278FD9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.modalBackground = KModalScreen.MakeScreenModal(this);
	}

	// Token: 0x060068BD RID: 26813 RVA: 0x0027ADED File Offset: 0x00278FED
	protected override void OnCmpEnable()
	{
		KModalScreen.ResizeBackground(this.modalBackground);
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
	}

	// Token: 0x060068BE RID: 26814 RVA: 0x0027AE20 File Offset: 0x00279020
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.childDialog == null)
		{
			base.Trigger(476357528, null);
		}
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Remove(instance.OnResize, new System.Action(this.OnResize));
	}

	// Token: 0x060068BF RID: 26815 RVA: 0x0027AE73 File Offset: 0x00279073
	private void OnResize()
	{
		KModalScreen.ResizeBackground(this.modalBackground);
	}

	// Token: 0x060068C0 RID: 26816 RVA: 0x0027AE80 File Offset: 0x00279080
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x060068C1 RID: 26817 RVA: 0x0027AE83 File Offset: 0x00279083
	public override float GetSortKey()
	{
		return 100f;
	}

	// Token: 0x060068C2 RID: 26818 RVA: 0x0027AE8C File Offset: 0x0027908C
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (SpeedControlScreen.Instance != null)
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
		if (CameraController.Instance != null)
		{
			CameraController.Instance.DisableUserCameraControl = show;
		}
	}

	// Token: 0x060068C3 RID: 26819 RVA: 0x0027AEFB File Offset: 0x002790FB
	public override void OnKeyDown(KButtonEvent e)
	{
		base.OnKeyDown(e);
		e.Consumed = true;
	}

	// Token: 0x060068C4 RID: 26820 RVA: 0x0027AF0B File Offset: 0x0027910B
	public override void OnKeyUp(KButtonEvent e)
	{
		base.OnKeyUp(e);
		e.Consumed = true;
	}

	// Token: 0x060068C5 RID: 26821 RVA: 0x0027AF1B File Offset: 0x0027911B
	public void SetBackgroundActive(bool active)
	{
	}

	// Token: 0x060068C6 RID: 26822 RVA: 0x0027AF20 File Offset: 0x00279120
	protected GameObject ActivateChildScreen(GameObject screenPrefab)
	{
		GameObject gameObject = Util.KInstantiateUI(screenPrefab, base.transform.parent.gameObject, false);
		this.childDialog = gameObject;
		gameObject.Subscribe(476357528, new Action<object>(this.Unhide));
		this.Hide();
		return gameObject;
	}

	// Token: 0x060068C7 RID: 26823 RVA: 0x0027AF6B File Offset: 0x0027916B
	private void Hide()
	{
		this.panelRoot.rectTransform().localScale = Vector3.zero;
	}

	// Token: 0x060068C8 RID: 26824 RVA: 0x0027AF82 File Offset: 0x00279182
	private void Unhide(object data = null)
	{
		this.panelRoot.rectTransform().localScale = Vector3.one;
		this.childDialog.Unsubscribe(476357528, new Action<object>(this.Unhide));
		this.childDialog = null;
	}

	// Token: 0x040047FE RID: 18430
	private bool shown;

	// Token: 0x040047FF RID: 18431
	[SerializeField]
	private GameObject panelRoot;

	// Token: 0x04004800 RID: 18432
	private GameObject childDialog;

	// Token: 0x04004801 RID: 18433
	private RectTransform modalBackground;
}
