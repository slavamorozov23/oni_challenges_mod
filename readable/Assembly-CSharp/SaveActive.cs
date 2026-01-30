using System;

// Token: 0x02000DFB RID: 3579
public class SaveActive : KScreen
{
	// Token: 0x0600715A RID: 29018 RVA: 0x002B515A File Offset: 0x002B335A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.SetAutoSaveCallbacks(new Game.SavingPreCB(this.ActiveateSaveIndicator), new Game.SavingActiveCB(this.SetActiveSaveIndicator), new Game.SavingPostCB(this.DeactivateSaveIndicator));
	}

	// Token: 0x0600715B RID: 29019 RVA: 0x002B5190 File Offset: 0x002B3390
	private void DoCallBack(HashedString name)
	{
		this.controller.onAnimComplete -= this.DoCallBack;
		this.readyForSaveCallback();
		this.readyForSaveCallback = null;
	}

	// Token: 0x0600715C RID: 29020 RVA: 0x002B51BB File Offset: 0x002B33BB
	private void ActiveateSaveIndicator(Game.CansaveCB cb)
	{
		this.readyForSaveCallback = cb;
		this.controller.onAnimComplete += this.DoCallBack;
		this.controller.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0600715D RID: 29021 RVA: 0x002B51FB File Offset: 0x002B33FB
	private void SetActiveSaveIndicator()
	{
		this.controller.Play("working_loop", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0600715E RID: 29022 RVA: 0x002B521D File Offset: 0x002B341D
	private void DeactivateSaveIndicator()
	{
		this.controller.Play("working_pst", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0600715F RID: 29023 RVA: 0x002B523F File Offset: 0x002B343F
	public override void OnKeyDown(KButtonEvent e)
	{
	}

	// Token: 0x04004E40 RID: 20032
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x04004E41 RID: 20033
	private Game.CansaveCB readyForSaveCallback;
}
