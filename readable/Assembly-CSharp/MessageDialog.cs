using System;

// Token: 0x02000D95 RID: 3477
public abstract class MessageDialog : KMonoBehaviour
{
	// Token: 0x170007B0 RID: 1968
	// (get) Token: 0x06006C47 RID: 27719 RVA: 0x00290F54 File Offset: 0x0028F154
	public virtual bool CanDontShowAgain
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06006C48 RID: 27720
	public abstract bool CanDisplay(Message message);

	// Token: 0x06006C49 RID: 27721
	public abstract void SetMessage(Message message);

	// Token: 0x06006C4A RID: 27722
	public abstract void OnClickAction();

	// Token: 0x06006C4B RID: 27723 RVA: 0x00290F57 File Offset: 0x0028F157
	public virtual void OnDontShowAgain()
	{
	}
}
