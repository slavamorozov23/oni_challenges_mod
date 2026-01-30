using System;
using UnityEngine;

// Token: 0x0200037A RID: 890
public class SlimeMoldConfig : IOreConfig
{
	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06001280 RID: 4736 RVA: 0x0006B930 File Offset: 0x00069B30
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.SlimeMold;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06001281 RID: 4737 RVA: 0x0006B937 File Offset: 0x00069B37
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ContaminatedOxygen;
		}
	}

	// Token: 0x06001282 RID: 4738 RVA: 0x0006B940 File Offset: 0x00069B40
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubble;
		sublimates.info = new Sublimates.Info(0.025f, 0.125f, 1.8f, 0f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
