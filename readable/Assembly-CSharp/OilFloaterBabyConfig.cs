using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200014D RID: 333
[EntityConfigOrder(3)]
public class OilFloaterBabyConfig : IEntityConfig
{
	// Token: 0x0600065B RID: 1627 RVA: 0x0002EE82 File Offset: 0x0002D082
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterConfig.CreateOilFloater("OilfloaterBaby", CREATURES.SPECIES.OILFLOATER.BABY.NAME, CREATURES.SPECIES.OILFLOATER.BABY.DESC, "baby_oilfloater_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Oilfloater", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x0002EEC0 File Offset: 0x0002D0C0
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x0002EEC2 File Offset: 0x0002D0C2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004CF RID: 1231
	public const string ID = "OilfloaterBaby";
}
