using System;

// Token: 0x02000DB9 RID: 3513
public abstract class NewGameFlowScreen : KModalScreen
{
	// Token: 0x1400002C RID: 44
	// (add) Token: 0x06006DBB RID: 28091 RVA: 0x002997B0 File Offset: 0x002979B0
	// (remove) Token: 0x06006DBC RID: 28092 RVA: 0x002997E8 File Offset: 0x002979E8
	public event System.Action OnNavigateForward;

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x06006DBD RID: 28093 RVA: 0x00299820 File Offset: 0x00297A20
	// (remove) Token: 0x06006DBE RID: 28094 RVA: 0x00299858 File Offset: 0x00297A58
	public event System.Action OnNavigateBackward;

	// Token: 0x06006DBF RID: 28095 RVA: 0x0029988D File Offset: 0x00297A8D
	protected void NavigateBackward()
	{
		this.OnNavigateBackward();
	}

	// Token: 0x06006DC0 RID: 28096 RVA: 0x0029989A File Offset: 0x00297A9A
	protected void NavigateForward()
	{
		this.OnNavigateForward();
	}

	// Token: 0x06006DC1 RID: 28097 RVA: 0x002998A7 File Offset: 0x00297AA7
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (e.TryConsume(global::Action.MouseRight))
		{
			this.NavigateBackward();
		}
		base.OnKeyDown(e);
	}
}
