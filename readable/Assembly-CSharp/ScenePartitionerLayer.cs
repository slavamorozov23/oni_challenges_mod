using System;

// Token: 0x02000B31 RID: 2865
public class ScenePartitionerLayer
{
	// Token: 0x06005463 RID: 21603 RVA: 0x001ED39E File Offset: 0x001EB59E
	public ScenePartitionerLayer(HashedString name, int layer)
	{
		this.name = name;
		this.layer = layer;
	}

	// Token: 0x04003904 RID: 14596
	public HashedString name;

	// Token: 0x04003905 RID: 14597
	public int layer;

	// Token: 0x04003906 RID: 14598
	public Action<int, object> OnEvent;
}
