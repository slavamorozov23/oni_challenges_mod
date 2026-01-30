using System;

// Token: 0x02000727 RID: 1831
public interface ISecondaryInput
{
	// Token: 0x06002DFD RID: 11773
	bool HasSecondaryConduitType(ConduitType type);

	// Token: 0x06002DFE RID: 11774
	CellOffset GetSecondaryConduitOffset(ConduitType type);
}
