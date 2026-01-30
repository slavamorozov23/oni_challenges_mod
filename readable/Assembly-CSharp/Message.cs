using System;
using KSerialization;

// Token: 0x02000D94 RID: 3476
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class Message : ISaveLoadable
{
	// Token: 0x06006C3B RID: 27707
	public abstract string GetTitle();

	// Token: 0x06006C3C RID: 27708
	public abstract string GetSound();

	// Token: 0x06006C3D RID: 27709
	public abstract string GetMessageBody();

	// Token: 0x06006C3E RID: 27710
	public abstract string GetTooltip();

	// Token: 0x06006C3F RID: 27711 RVA: 0x00290F39 File Offset: 0x0028F139
	public virtual bool ShowDialog()
	{
		return true;
	}

	// Token: 0x06006C40 RID: 27712 RVA: 0x00290F3C File Offset: 0x0028F13C
	public virtual void OnCleanUp()
	{
	}

	// Token: 0x06006C41 RID: 27713 RVA: 0x00290F3E File Offset: 0x0028F13E
	public virtual bool IsValid()
	{
		return true;
	}

	// Token: 0x06006C42 RID: 27714 RVA: 0x00290F41 File Offset: 0x0028F141
	public virtual bool PlayNotificationSound()
	{
		return true;
	}

	// Token: 0x06006C43 RID: 27715 RVA: 0x00290F44 File Offset: 0x0028F144
	public virtual void OnClick()
	{
	}

	// Token: 0x06006C44 RID: 27716 RVA: 0x00290F46 File Offset: 0x0028F146
	public virtual NotificationType GetMessageType()
	{
		return NotificationType.Messages;
	}

	// Token: 0x06006C45 RID: 27717 RVA: 0x00290F49 File Offset: 0x0028F149
	public virtual bool ShowDismissButton()
	{
		return true;
	}
}
