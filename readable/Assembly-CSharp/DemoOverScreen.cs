using System;

// Token: 0x02000CF3 RID: 3315
public class DemoOverScreen : KModalScreen
{
	// Token: 0x06006674 RID: 26228 RVA: 0x002692D6 File Offset: 0x002674D6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Init();
		PlayerController.Instance.ActivateTool(SelectTool.Instance);
		SelectTool.Instance.Select(null, false);
	}

	// Token: 0x06006675 RID: 26229 RVA: 0x002692FF File Offset: 0x002674FF
	private void Init()
	{
		this.QuitButton.onClick += delegate()
		{
			this.Quit();
		};
	}

	// Token: 0x06006676 RID: 26230 RVA: 0x00269318 File Offset: 0x00267518
	private void Quit()
	{
		PauseScreen.TriggerQuitGame();
	}

	// Token: 0x040045F5 RID: 17909
	public KButton QuitButton;
}
