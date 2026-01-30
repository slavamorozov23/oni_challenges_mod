using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200014F RID: 335
[EntityConfigOrder(4)]
public class OilFloaterDecorBabyConfig : IEntityConfig
{
	// Token: 0x06000665 RID: 1637 RVA: 0x0002F0D6 File Offset: 0x0002D2D6
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterDecorConfig.CreateOilFloater("OilfloaterDecorBaby", CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.BABY.NAME, CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.BABY.DESC, "baby_oilfloater_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "OilfloaterDecor", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x0002F114 File Offset: 0x0002D314
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x0002F116 File Offset: 0x0002D316
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004D7 RID: 1239
	public const string ID = "OilfloaterDecorBaby";
}
