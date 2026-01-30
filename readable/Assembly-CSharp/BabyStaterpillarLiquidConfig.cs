using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000173 RID: 371
[EntityConfigOrder(4)]
public class BabyStaterpillarLiquidConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600072E RID: 1838 RVA: 0x00032185 File Offset: 0x00030385
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0003218C File Offset: 0x0003038C
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x0003218F File Offset: 0x0003038F
	public GameObject CreatePrefab()
	{
		GameObject gameObject = StaterpillarLiquidConfig.CreateStaterpillarLiquid("StaterpillarLiquidBaby", CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.BABY.NAME, CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.BABY.DESC, "baby_caterpillar_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "StaterpillarLiquid", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x000321CD File Offset: 0x000303CD
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("electric_bolt_c_bloom", false);
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x000321E5 File Offset: 0x000303E5
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400057F RID: 1407
	public const string ID = "StaterpillarLiquidBaby";
}
