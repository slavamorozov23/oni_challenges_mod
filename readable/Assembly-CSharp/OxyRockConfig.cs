using System;
using UnityEngine;

// Token: 0x02000379 RID: 889
public class OxyRockConfig : IOreConfig
{
	// Token: 0x17000058 RID: 88
	// (get) Token: 0x0600127C RID: 4732 RVA: 0x0006B8C3 File Offset: 0x00069AC3
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.OxyRock;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x0600127D RID: 4733 RVA: 0x0006B8CA File Offset: 0x00069ACA
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.Oxygen;
		}
	}

	// Token: 0x0600127E RID: 4734 RVA: 0x0006B8D4 File Offset: 0x00069AD4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.OxygenEmissionBubbles;
		sublimates.info = new Sublimates.Info(0.010000001f, 0.0050000004f, 1.8f, 0.7f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
