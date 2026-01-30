using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000923 RID: 2339
public interface IMultiEntityConfig
{
	// Token: 0x06004178 RID: 16760
	List<GameObject> CreatePrefabs();

	// Token: 0x06004179 RID: 16761
	void OnPrefabInit(GameObject inst);

	// Token: 0x0600417A RID: 16762
	void OnSpawn(GameObject inst);
}
