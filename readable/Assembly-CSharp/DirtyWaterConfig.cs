using System;
using UnityEngine;

// Token: 0x02000375 RID: 885
public class DirtyWaterConfig : IOreConfig
{
	// Token: 0x17000054 RID: 84
	// (get) Token: 0x06001270 RID: 4720 RVA: 0x0006B580 File Offset: 0x00069780
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.DirtyWater;
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x06001271 RID: 4721 RVA: 0x0006B587 File Offset: 0x00069787
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ContaminatedOxygen;
		}
	}

	// Token: 0x06001272 RID: 4722 RVA: 0x0006B590 File Offset: 0x00069790
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLiquidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubbleWater;
		sublimates.info = new Sublimates.Info(4.0000006E-05f, 0.025f, 1.8f, 1f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
