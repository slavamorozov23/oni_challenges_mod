using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000126 RID: 294
[EntityConfigOrder(4)]
public class BabyDreckoPlasticConfig : IEntityConfig
{
	// Token: 0x06000585 RID: 1413 RVA: 0x0002B5B5 File Offset: 0x000297B5
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DreckoPlasticConfig.CreateDrecko("DreckoPlasticBaby", CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.BABY.NAME, CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.BABY.DESC, "baby_drecko_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "DreckoPlastic", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x0002B5F3 File Offset: 0x000297F3
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x0002B5F5 File Offset: 0x000297F5
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000416 RID: 1046
	public const string ID = "DreckoPlasticBaby";
}
