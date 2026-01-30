using System;
using STRINGS;

// Token: 0x02000D8D RID: 3469
public class CodexUnlockedMessage : Message
{
	// Token: 0x06006C07 RID: 27655 RVA: 0x00290B8A File Offset: 0x0028ED8A
	public CodexUnlockedMessage()
	{
	}

	// Token: 0x06006C08 RID: 27656 RVA: 0x00290B92 File Offset: 0x0028ED92
	public CodexUnlockedMessage(string lock_id, string unlock_message)
	{
		this.lockId = lock_id;
		this.unlockMessage = unlock_message;
	}

	// Token: 0x06006C09 RID: 27657 RVA: 0x00290BA8 File Offset: 0x0028EDA8
	public string GetLockId()
	{
		return this.lockId;
	}

	// Token: 0x06006C0A RID: 27658 RVA: 0x00290BB0 File Offset: 0x0028EDB0
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x06006C0B RID: 27659 RVA: 0x00290BB7 File Offset: 0x0028EDB7
	public override string GetMessageBody()
	{
		return UI.CODEX.CODEX_DISCOVERED_MESSAGE.BODY.Replace("{codex}", this.unlockMessage);
	}

	// Token: 0x06006C0C RID: 27660 RVA: 0x00290BCE File Offset: 0x0028EDCE
	public override string GetTitle()
	{
		return UI.CODEX.CODEX_DISCOVERED_MESSAGE.TITLE;
	}

	// Token: 0x06006C0D RID: 27661 RVA: 0x00290BDA File Offset: 0x0028EDDA
	public override string GetTooltip()
	{
		return this.GetMessageBody();
	}

	// Token: 0x06006C0E RID: 27662 RVA: 0x00290BE2 File Offset: 0x0028EDE2
	public override bool IsValid()
	{
		return true;
	}

	// Token: 0x04004A20 RID: 18976
	private string unlockMessage;

	// Token: 0x04004A21 RID: 18977
	private string lockId;
}
