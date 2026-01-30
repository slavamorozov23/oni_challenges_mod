using System;
using UnityEngine;

// Token: 0x0200072B RID: 1835
[AddComponentMenu("KMonoBehaviour/scripts/ConduitSecondaryInput")]
public class ConduitSecondaryInput : KMonoBehaviour, ISecondaryInput
{
	// Token: 0x06002E20 RID: 11808 RVA: 0x0010B787 File Offset: 0x00109987
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06002E21 RID: 11809 RVA: 0x0010B797 File Offset: 0x00109997
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.portInfo.conduitType == type)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04001B67 RID: 7015
	[SerializeField]
	public ConduitPortInfo portInfo;
}
