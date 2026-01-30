using System;
using UnityEngine;

// Token: 0x02000713 RID: 1811
public interface IKComponentManager
{
	// Token: 0x06002D29 RID: 11561
	HandleVector<int>.Handle Add(GameObject go);

	// Token: 0x06002D2A RID: 11562
	void Remove(GameObject go);
}
