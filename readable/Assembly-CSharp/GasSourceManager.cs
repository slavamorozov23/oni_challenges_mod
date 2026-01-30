using System;
using UnityEngine;

// Token: 0x020005E6 RID: 1510
[AddComponentMenu("KMonoBehaviour/scripts/GasSourceManager")]
public class GasSourceManager : KMonoBehaviour, IChunkManager
{
	// Token: 0x060022F5 RID: 8949 RVA: 0x000CB656 File Offset: 0x000C9856
	protected override void OnPrefabInit()
	{
		GasSourceManager.Instance = this;
	}

	// Token: 0x060022F6 RID: 8950 RVA: 0x000CB65E File Offset: 0x000C985E
	public SubstanceChunk CreateChunk(SimHashes element_id, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return this.CreateChunk(ElementLoader.FindElementByHash(element_id), mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x060022F7 RID: 8951 RVA: 0x000CB674 File Offset: 0x000C9874
	public SubstanceChunk CreateChunk(Element element, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return GeneratedOre.CreateChunk(element, mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x0400147E RID: 5246
	public static GasSourceManager Instance;
}
