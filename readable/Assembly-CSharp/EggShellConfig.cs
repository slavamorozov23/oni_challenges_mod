using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000272 RID: 626
public class EggShellConfig : IEntityConfig
{
	// Token: 0x06000CB7 RID: 3255 RVA: 0x0004C5C8 File Offset: 0x0004A7C8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("EggShell", ITEMS.INDUSTRIAL_PRODUCTS.EGG_SHELL.NAME, ITEMS.INDUSTRIAL_PRODUCTS.EGG_SHELL.DESC, 1f, false, Assets.GetAnim("eggshells_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Organics, false);
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>();
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000CB8 RID: 3256 RVA: 0x0004C648 File Offset: 0x0004A848
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CB9 RID: 3257 RVA: 0x0004C64A File Offset: 0x0004A84A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040008D0 RID: 2256
	public const string ID = "EggShell";

	// Token: 0x040008D1 RID: 2257
	public static readonly Tag TAG = TagManager.Create("EggShell");

	// Token: 0x040008D2 RID: 2258
	public const float EGG_TO_SHELL_RATIO = 0.5f;
}
