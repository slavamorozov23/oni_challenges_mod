using System;

// Token: 0x02000C07 RID: 3079
public class UtilityNetwork
{
	// Token: 0x06005C87 RID: 23687 RVA: 0x00218448 File Offset: 0x00216648
	public virtual void AddItem(object item)
	{
	}

	// Token: 0x06005C88 RID: 23688 RVA: 0x0021844A File Offset: 0x0021664A
	public virtual void RemoveItem(object item)
	{
	}

	// Token: 0x06005C89 RID: 23689 RVA: 0x0021844C File Offset: 0x0021664C
	public virtual void ConnectItem(object item)
	{
	}

	// Token: 0x06005C8A RID: 23690 RVA: 0x0021844E File Offset: 0x0021664E
	public virtual void DisconnectItem(object item)
	{
	}

	// Token: 0x06005C8B RID: 23691 RVA: 0x00218450 File Offset: 0x00216650
	public virtual void Reset(UtilityNetworkGridNode[] grid)
	{
	}

	// Token: 0x04003DA3 RID: 15779
	public int id;

	// Token: 0x04003DA4 RID: 15780
	public ConduitType conduitType;
}
