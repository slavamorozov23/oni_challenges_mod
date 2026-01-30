using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000275 RID: 629
public class MachinePartsConfig : IEntityConfig
{
	// Token: 0x06000CC5 RID: 3269 RVA: 0x0004C734 File Offset: 0x0004A934
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity("MachineParts", ITEMS.INDUSTRIAL_PRODUCTS.MACHINE_PARTS.NAME, ITEMS.INDUSTRIAL_PRODUCTS.MACHINE_PARTS.DESC, 5f, true, Assets.GetAnim("buildingrelocate_kanim"), "idle", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true, 0, SimHashes.Creature, null);
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x0004C78E File Offset: 0x0004A98E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x0004C790 File Offset: 0x0004A990
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040008D8 RID: 2264
	public const string ID = "MachineParts";

	// Token: 0x040008D9 RID: 2265
	public static readonly Tag TAG = TagManager.Create("MachineParts");

	// Token: 0x040008DA RID: 2266
	public const float MASS = 5f;
}
