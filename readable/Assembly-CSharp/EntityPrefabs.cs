using System;
using UnityEngine;

// Token: 0x02000926 RID: 2342
[AddComponentMenu("KMonoBehaviour/scripts/EntityPrefabs")]
public class EntityPrefabs : KMonoBehaviour
{
	// Token: 0x170004AB RID: 1195
	// (get) Token: 0x06004186 RID: 16774 RVA: 0x00172045 File Offset: 0x00170245
	// (set) Token: 0x06004187 RID: 16775 RVA: 0x0017204C File Offset: 0x0017024C
	public static EntityPrefabs Instance { get; private set; }

	// Token: 0x06004188 RID: 16776 RVA: 0x00172054 File Offset: 0x00170254
	public static void DestroyInstance()
	{
		EntityPrefabs.Instance = null;
	}

	// Token: 0x06004189 RID: 16777 RVA: 0x0017205C File Offset: 0x0017025C
	protected override void OnPrefabInit()
	{
		EntityPrefabs.Instance = this;
	}

	// Token: 0x040028EB RID: 10475
	public GameObject SelectMarker;

	// Token: 0x040028EC RID: 10476
	public GameObject ForegroundLayer;
}
