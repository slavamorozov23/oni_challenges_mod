using System;
using KSerialization;

// Token: 0x02000D9F RID: 3487
public abstract class TargetMessage : Message
{
	// Token: 0x06006C94 RID: 27796 RVA: 0x00291B6E File Offset: 0x0028FD6E
	protected TargetMessage()
	{
	}

	// Token: 0x06006C95 RID: 27797 RVA: 0x00291B76 File Offset: 0x0028FD76
	public TargetMessage(KPrefabID prefab_id)
	{
		this.target = new MessageTarget(prefab_id);
	}

	// Token: 0x06006C96 RID: 27798 RVA: 0x00291B8A File Offset: 0x0028FD8A
	public MessageTarget GetTarget()
	{
		return this.target;
	}

	// Token: 0x06006C97 RID: 27799 RVA: 0x00291B92 File Offset: 0x0028FD92
	public override void OnCleanUp()
	{
		this.target.OnCleanUp();
	}

	// Token: 0x04004A4B RID: 19019
	[Serialize]
	private MessageTarget target;
}
