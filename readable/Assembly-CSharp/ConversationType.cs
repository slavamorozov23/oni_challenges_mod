using System;
using UnityEngine;

// Token: 0x0200087C RID: 2172
public class ConversationType
{
	// Token: 0x06003BCB RID: 15307 RVA: 0x0014EC6B File Offset: 0x0014CE6B
	public virtual void NewTarget(MinionIdentity speaker)
	{
	}

	// Token: 0x06003BCC RID: 15308 RVA: 0x0014EC6D File Offset: 0x0014CE6D
	public virtual Conversation.Topic GetNextTopic(MinionIdentity speaker, Conversation.Topic lastTopic)
	{
		return null;
	}

	// Token: 0x06003BCD RID: 15309 RVA: 0x0014EC70 File Offset: 0x0014CE70
	public virtual Sprite GetSprite(string topic)
	{
		return null;
	}

	// Token: 0x040024F9 RID: 9465
	public string id;

	// Token: 0x040024FA RID: 9466
	public string target;
}
