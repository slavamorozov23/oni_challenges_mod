using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000BFF RID: 3071
[SerializationConfig(MemberSerialization.OptOut)]
[AddComponentMenu("KMonoBehaviour/scripts/UnstableGround")]
public class UnstableGround : KMonoBehaviour
{
	// Token: 0x04003D7B RID: 15739
	public SimHashes element;

	// Token: 0x04003D7C RID: 15740
	public float mass;

	// Token: 0x04003D7D RID: 15741
	public float temperature;

	// Token: 0x04003D7E RID: 15742
	public byte diseaseIdx;

	// Token: 0x04003D7F RID: 15743
	public int diseaseCount;
}
