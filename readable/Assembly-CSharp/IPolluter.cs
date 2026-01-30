using System;
using UnityEngine;

// Token: 0x02000A6E RID: 2670
public interface IPolluter
{
	// Token: 0x06004D89 RID: 19849
	int GetRadius();

	// Token: 0x06004D8A RID: 19850
	int GetNoise();

	// Token: 0x06004D8B RID: 19851
	GameObject GetGameObject();

	// Token: 0x06004D8C RID: 19852
	void SetAttributes(Vector2 pos, int dB, GameObject go, string name = null);

	// Token: 0x06004D8D RID: 19853
	string GetName();

	// Token: 0x06004D8E RID: 19854
	Vector2 GetPosition();

	// Token: 0x06004D8F RID: 19855
	void Clear();

	// Token: 0x06004D90 RID: 19856
	void SetSplat(NoiseSplat splat);
}
