using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000147 RID: 327
[EntityConfigOrder(4)]
public class BabyMoleDelicacyConfig : IEntityConfig
{
	// Token: 0x06000638 RID: 1592 RVA: 0x0002E74A File Offset: 0x0002C94A
	public GameObject CreatePrefab()
	{
		GameObject gameObject = MoleDelicacyConfig.CreateMole("MoleDelicacyBaby", CREATURES.SPECIES.MOLE.VARIANT_DELICACY.BABY.NAME, CREATURES.SPECIES.MOLE.VARIANT_DELICACY.BABY.DESC, "baby_driller_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "MoleDelicacy", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x0002E788 File Offset: 0x0002C988
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x0002E78A File Offset: 0x0002C98A
	public void OnSpawn(GameObject inst)
	{
		MoleConfig.SetSpawnNavType(inst);
	}

	// Token: 0x040004B4 RID: 1204
	public const string ID = "MoleDelicacyBaby";
}
