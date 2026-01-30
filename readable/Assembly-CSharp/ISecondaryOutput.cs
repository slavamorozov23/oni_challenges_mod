using System;

// Token: 0x02000726 RID: 1830
public interface ISecondaryOutput
{
	// Token: 0x06002DFB RID: 11771
	bool HasSecondaryConduitType(ConduitType type);

	// Token: 0x06002DFC RID: 11772
	CellOffset GetSecondaryConduitOffset(ConduitType type);
}
