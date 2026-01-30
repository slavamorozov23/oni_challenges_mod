using System;
using KSerialization;

// Token: 0x020005E5 RID: 1509
[SerializationConfig(MemberSerialization.OptIn)]
public class GasSource : SubstanceSource
{
	// Token: 0x060022F1 RID: 8945 RVA: 0x000CB638 File Offset: 0x000C9838
	protected override CellOffset[] GetOffsetGroup()
	{
		return OffsetGroups.LiquidSource;
	}

	// Token: 0x060022F2 RID: 8946 RVA: 0x000CB63F File Offset: 0x000C983F
	protected override IChunkManager GetChunkManager()
	{
		return GasSourceManager.Instance;
	}

	// Token: 0x060022F3 RID: 8947 RVA: 0x000CB646 File Offset: 0x000C9846
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}
}
