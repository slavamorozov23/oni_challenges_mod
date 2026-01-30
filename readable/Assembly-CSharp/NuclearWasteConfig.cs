using System;
using UnityEngine;

// Token: 0x02000378 RID: 888
public class NuclearWasteConfig : IOreConfig
{
	// Token: 0x17000057 RID: 87
	// (get) Token: 0x06001279 RID: 4729 RVA: 0x0006B856 File Offset: 0x00069A56
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.NuclearWaste;
		}
	}

	// Token: 0x0600127A RID: 4730 RVA: 0x0006B860 File Offset: 0x00069A60
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLiquidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.decayStorage = true;
		sublimates.spawnFXHash = SpawnFXHashes.NuclearWasteDrip;
		sublimates.info = new Sublimates.Info(0.066f, 6.6f, 1000f, 0f, this.ElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
