using System;

// Token: 0x02000D27 RID: 3367
public class Hud : KScreen
{
	// Token: 0x06006816 RID: 26646 RVA: 0x00274BE1 File Offset: 0x00272DE1
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Help))
		{
			GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ControlsScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
		}
	}
}
