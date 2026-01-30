using System;
using UnityEngine;

// Token: 0x020002FD RID: 765
public class AsteroidConfig : IEntityConfig
{
	// Token: 0x06000F91 RID: 3985 RVA: 0x0005BBC4 File Offset: 0x00059DC4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("Asteroid", "Asteroid", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<WorldInventory>();
		gameObject.AddOrGet<WorldContainer>();
		gameObject.AddOrGet<AsteroidGridEntity>();
		gameObject.AddOrGet<OrbitalMechanics>();
		gameObject.AddOrGetDef<GameplaySeasonManager.Def>();
		gameObject.AddOrGetDef<AlertStateManager.Def>();
		return gameObject;
	}

	// Token: 0x06000F92 RID: 3986 RVA: 0x0005BC12 File Offset: 0x00059E12
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F93 RID: 3987 RVA: 0x0005BC14 File Offset: 0x00059E14
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000A35 RID: 2613
	public const string ID = "Asteroid";
}
