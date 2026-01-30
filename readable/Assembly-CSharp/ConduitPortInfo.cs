using System;

// Token: 0x0200072A RID: 1834
[Serializable]
public class ConduitPortInfo
{
	// Token: 0x06002E1F RID: 11807 RVA: 0x0010B771 File Offset: 0x00109971
	public ConduitPortInfo(ConduitType type, CellOffset offset)
	{
		this.conduitType = type;
		this.offset = offset;
	}

	// Token: 0x04001B65 RID: 7013
	public ConduitType conduitType;

	// Token: 0x04001B66 RID: 7014
	public CellOffset offset;
}
