using System;
using UnityEngine;

// Token: 0x0200072C RID: 1836
[AddComponentMenu("KMonoBehaviour/scripts/ConduitSecondaryOutput")]
public class ConduitSecondaryOutput : KMonoBehaviour, ISecondaryOutput
{
	// Token: 0x06002E23 RID: 11811 RVA: 0x0010B7C0 File Offset: 0x001099C0
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06002E24 RID: 11812 RVA: 0x0010B7D0 File Offset: 0x001099D0
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == this.portInfo.conduitType)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04001B68 RID: 7016
	[SerializeField]
	public ConduitPortInfo portInfo;
}
