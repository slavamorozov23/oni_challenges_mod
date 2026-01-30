using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200015C RID: 348
[EntityConfigOrder(4)]
public class BabyPuftAlphaConfig : IEntityConfig
{
	// Token: 0x060006A6 RID: 1702 RVA: 0x0002FDD3 File Offset: 0x0002DFD3
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftAlphaConfig.CreatePuftAlpha("PuftAlphaBaby", CREATURES.SPECIES.PUFT.VARIANT_ALPHA.BABY.NAME, CREATURES.SPECIES.PUFT.VARIANT_ALPHA.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PuftAlpha", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x0002FE11 File Offset: 0x0002E011
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x0002FE13 File Offset: 0x0002E013
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x04000507 RID: 1287
	public const string ID = "PuftAlphaBaby";
}
