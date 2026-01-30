using System;
using UnityEngine;

// Token: 0x02000922 RID: 2338
public interface IEntityConfig
{
	// Token: 0x06004174 RID: 16756
	GameObject CreatePrefab();

	// Token: 0x06004175 RID: 16757
	void OnPrefabInit(GameObject inst);

	// Token: 0x06004176 RID: 16758
	void OnSpawn(GameObject inst);

	// Token: 0x06004177 RID: 16759 RVA: 0x00171C11 File Offset: 0x0016FE11
	[Obsolete("Use IHasDlcRestrictions instead")]
	string[] GetDlcIds()
	{
		return null;
	}
}
