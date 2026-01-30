using System;
using UnityEngine;

// Token: 0x02000374 RID: 884
public class BleachStoneConfig : IOreConfig
{
	// Token: 0x17000052 RID: 82
	// (get) Token: 0x0600126C RID: 4716 RVA: 0x0006B515 File Offset: 0x00069715
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.BleachStone;
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x0600126D RID: 4717 RVA: 0x0006B51C File Offset: 0x0006971C
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ChlorineGas;
		}
	}

	// Token: 0x0600126E RID: 4718 RVA: 0x0006B524 File Offset: 0x00069724
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.BleachStoneEmissionBubbles;
		sublimates.info = new Sublimates.Info(0.00020000001f, 0.0025000002f, 1.8f, 0.5f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
