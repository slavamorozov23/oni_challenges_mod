using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000145 RID: 325
[EntityConfigOrder(3)]
public class BabyMoleConfig : IEntityConfig
{
	// Token: 0x0600062D RID: 1581 RVA: 0x0002E359 File Offset: 0x0002C559
	public GameObject CreatePrefab()
	{
		GameObject gameObject = MoleConfig.CreateMole("MoleBaby", CREATURES.SPECIES.MOLE.BABY.NAME, CREATURES.SPECIES.MOLE.BABY.DESC, "baby_driller_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Mole", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x0002E397 File Offset: 0x0002C597
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x0002E399 File Offset: 0x0002C599
	public void OnSpawn(GameObject inst)
	{
		MoleConfig.SetSpawnNavType(inst);
	}

	// Token: 0x040004A6 RID: 1190
	public const string ID = "MoleBaby";
}
