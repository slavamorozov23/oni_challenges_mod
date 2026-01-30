using System;
using Database;
using UnityEngine;

// Token: 0x02000D4B RID: 3403
public interface IKleiPermitDioramaVisTarget
{
	// Token: 0x06006976 RID: 26998
	GameObject GetGameObject();

	// Token: 0x06006977 RID: 26999
	void ConfigureSetup();

	// Token: 0x06006978 RID: 27000
	void ConfigureWith(PermitResource permit);
}
