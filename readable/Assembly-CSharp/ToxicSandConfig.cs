using System;
using UnityEngine;

// Token: 0x0200037B RID: 891
public class ToxicSandConfig : IOreConfig
{
	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06001284 RID: 4740 RVA: 0x0006B99C File Offset: 0x00069B9C
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.ToxicSand;
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06001285 RID: 4741 RVA: 0x0006B9A3 File Offset: 0x00069BA3
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ContaminatedOxygen;
		}
	}

	// Token: 0x06001286 RID: 4742 RVA: 0x0006B9AC File Offset: 0x00069BAC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubble;
		sublimates.info = new Sublimates.Info(2.0000001E-05f, 0.05f, 1.8f, 0.5f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
