using System;
using UnityEngine;

// Token: 0x020005FB RID: 1531
[AddComponentMenu("KMonoBehaviour/scripts/LiquidSourceManager")]
public class LiquidSourceManager : KMonoBehaviour, IChunkManager
{
	// Token: 0x0600238E RID: 9102 RVA: 0x000CD393 File Offset: 0x000CB593
	protected override void OnPrefabInit()
	{
		LiquidSourceManager.Instance = this;
	}

	// Token: 0x0600238F RID: 9103 RVA: 0x000CD39B File Offset: 0x000CB59B
	public SubstanceChunk CreateChunk(SimHashes element_id, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return this.CreateChunk(ElementLoader.FindElementByHash(element_id), mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x06002390 RID: 9104 RVA: 0x000CD3B1 File Offset: 0x000CB5B1
	public SubstanceChunk CreateChunk(Element element, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return GeneratedOre.CreateChunk(element, mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x040014C0 RID: 5312
	public static LiquidSourceManager Instance;
}
