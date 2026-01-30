using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000171 RID: 369
[EntityConfigOrder(4)]
public class BabyStaterpillarGasConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000720 RID: 1824 RVA: 0x00031DB1 File Offset: 0x0002FFB1
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x00031DB8 File Offset: 0x0002FFB8
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x00031DBB File Offset: 0x0002FFBB
	public GameObject CreatePrefab()
	{
		GameObject gameObject = StaterpillarGasConfig.CreateStaterpillarGas("StaterpillarGasBaby", CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.BABY.NAME, CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.BABY.DESC, "baby_caterpillar_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "StaterpillarGas", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x00031DF9 File Offset: 0x0002FFF9
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("electric_bolt_c_bloom", false);
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x00031E11 File Offset: 0x00030011
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000573 RID: 1395
	public const string ID = "StaterpillarGasBaby";
}
