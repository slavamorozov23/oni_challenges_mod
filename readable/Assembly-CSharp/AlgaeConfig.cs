using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000373 RID: 883
public class AlgaeConfig : IOreConfig
{
	// Token: 0x17000051 RID: 81
	// (get) Token: 0x06001269 RID: 4713 RVA: 0x0006B4E9 File Offset: 0x000696E9
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.Algae;
		}
	}

	// Token: 0x0600126A RID: 4714 RVA: 0x0006B4F0 File Offset: 0x000696F0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateSolidOreEntity(this.ElementID, new List<Tag>
		{
			GameTags.Life
		});
	}
}
