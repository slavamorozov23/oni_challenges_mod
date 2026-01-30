using System;
using UnityEngine;

// Token: 0x02000A83 RID: 2691
public struct OreSizeVisualizerData
{
	// Token: 0x06004E37 RID: 20023 RVA: 0x001C72A4 File Offset: 0x001C54A4
	public OreSizeVisualizerData(GameObject go)
	{
		this.primaryElement = go.GetComponent<PrimaryElement>();
		this.tierSetType = OreSizeVisualizerComponents.TiersSetType.Ores;
		this.absorbHandle = -1;
		this.splitFromChunkHandle = -1;
	}

	// Token: 0x0400341C RID: 13340
	public PrimaryElement primaryElement;

	// Token: 0x0400341D RID: 13341
	public OreSizeVisualizerComponents.TiersSetType tierSetType;

	// Token: 0x0400341E RID: 13342
	public int absorbHandle;

	// Token: 0x0400341F RID: 13343
	public int splitFromChunkHandle;
}
