using System;
using UnityEngine;

// Token: 0x02000648 RID: 1608
public interface IChunkManager
{
	// Token: 0x0600272D RID: 10029
	SubstanceChunk CreateChunk(Element element, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position);

	// Token: 0x0600272E RID: 10030
	SubstanceChunk CreateChunk(SimHashes element_id, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position);
}
