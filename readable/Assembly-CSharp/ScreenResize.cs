using System;
using UnityEngine;

// Token: 0x02000AF2 RID: 2802
public class ScreenResize : MonoBehaviour
{
	// Token: 0x0600516C RID: 20844 RVA: 0x001D83C0 File Offset: 0x001D65C0
	private void Awake()
	{
		ScreenResize.Instance = this;
		this.isFullscreen = Screen.fullScreen;
		this.OnResize = (System.Action)Delegate.Combine(this.OnResize, new System.Action(this.SaveResolutionToPrefs));
	}

	// Token: 0x0600516D RID: 20845 RVA: 0x001D83F8 File Offset: 0x001D65F8
	private void LateUpdate()
	{
		if (Screen.width != this.Width || Screen.height != this.Height || this.isFullscreen != Screen.fullScreen)
		{
			this.Width = Screen.width;
			this.Height = Screen.height;
			this.isFullscreen = Screen.fullScreen;
			this.TriggerResize();
		}
	}

	// Token: 0x0600516E RID: 20846 RVA: 0x001D8453 File Offset: 0x001D6653
	public void TriggerResize()
	{
		if (this.OnResize != null)
		{
			this.OnResize();
		}
	}

	// Token: 0x0600516F RID: 20847 RVA: 0x001D8468 File Offset: 0x001D6668
	private void SaveResolutionToPrefs()
	{
		GraphicsOptionsScreen.OnResize();
	}

	// Token: 0x0400370C RID: 14092
	public System.Action OnResize;

	// Token: 0x0400370D RID: 14093
	public static ScreenResize Instance;

	// Token: 0x0400370E RID: 14094
	private int Width;

	// Token: 0x0400370F RID: 14095
	private int Height;

	// Token: 0x04003710 RID: 14096
	private bool isFullscreen;
}
