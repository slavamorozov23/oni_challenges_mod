using System;

// Token: 0x02000D13 RID: 3347
public class GameOverScreen : KModalScreen
{
	// Token: 0x06006799 RID: 26521 RVA: 0x00270EF2 File Offset: 0x0026F0F2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Init();
	}

	// Token: 0x0600679A RID: 26522 RVA: 0x00270F00 File Offset: 0x0026F100
	private void Init()
	{
		if (this.QuitButton)
		{
			this.QuitButton.onClick += delegate()
			{
				this.Quit();
			};
		}
		if (this.DismissButton)
		{
			this.DismissButton.onClick += delegate()
			{
				this.Dismiss();
			};
		}
	}

	// Token: 0x0600679B RID: 26523 RVA: 0x00270F55 File Offset: 0x0026F155
	private void Quit()
	{
		PauseScreen.TriggerQuitGame();
	}

	// Token: 0x0600679C RID: 26524 RVA: 0x00270F5C File Offset: 0x0026F15C
	private void Dismiss()
	{
		this.Show(false);
	}

	// Token: 0x040046F7 RID: 18167
	public KButton DismissButton;

	// Token: 0x040046F8 RID: 18168
	public KButton QuitButton;
}
