using System;
using UnityEngine;

// Token: 0x0200034C RID: 844
public class TallowConfig : IOreConfig
{
	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06001193 RID: 4499 RVA: 0x0006785D File Offset: 0x00065A5D
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.Tallow;
		}
	}

	// Token: 0x06001194 RID: 4500 RVA: 0x00067864 File Offset: 0x00065A64
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
	}

	// Token: 0x04000B12 RID: 2834
	public const string ID = "Tallow";

	// Token: 0x04000B13 RID: 2835
	public static readonly Tag TAG = TagManager.Create("Tallow");
}
