using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000304 RID: 772
public class CarePackageConfig : IEntityConfig
{
	// Token: 0x06000FDA RID: 4058 RVA: 0x00060704 File Offset: 0x0005E904
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity(CarePackageConfig.ID, ITEMS.CARGO_CAPSULE.NAME, ITEMS.CARGO_CAPSULE.DESC, 1f, true, Assets.GetAnim("portal_carepackage_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, false, 0, SimHashes.Creature, null);
	}

	// Token: 0x06000FDB RID: 4059 RVA: 0x0006075E File Offset: 0x0005E95E
	public void OnPrefabInit(GameObject go)
	{
		go.AddOrGet<CarePackage>();
	}

	// Token: 0x06000FDC RID: 4060 RVA: 0x00060767 File Offset: 0x0005E967
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A63 RID: 2659
	public static readonly string ID = "CarePackage";
}
