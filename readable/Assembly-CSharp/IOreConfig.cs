using System;
using UnityEngine;

// Token: 0x02000376 RID: 886
public interface IOreConfig
{
	// Token: 0x17000056 RID: 86
	// (get) Token: 0x06001274 RID: 4724
	SimHashes ElementID { get; }

	// Token: 0x06001275 RID: 4725
	GameObject CreatePrefab();
}
