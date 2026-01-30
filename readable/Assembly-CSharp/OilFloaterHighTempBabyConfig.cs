using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000151 RID: 337
[EntityConfigOrder(4)]
public class OilFloaterHighTempBabyConfig : IEntityConfig
{
	// Token: 0x0600066F RID: 1647 RVA: 0x0002F32C File Offset: 0x0002D52C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterHighTempConfig.CreateOilFloater("OilfloaterHighTempBaby", CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.BABY.NAME, CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.BABY.DESC, "baby_oilfloater_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "OilfloaterHighTemp", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x0002F36A File Offset: 0x0002D56A
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x0002F36C File Offset: 0x0002D56C
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004E1 RID: 1249
	public const string ID = "OilfloaterHighTempBaby";
}
