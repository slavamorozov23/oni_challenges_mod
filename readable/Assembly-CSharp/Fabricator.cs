using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000754 RID: 1876
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Fabricator")]
public class Fabricator : KMonoBehaviour
{
	// Token: 0x06002F77 RID: 12151 RVA: 0x00112304 File Offset: 0x00110504
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}
}
