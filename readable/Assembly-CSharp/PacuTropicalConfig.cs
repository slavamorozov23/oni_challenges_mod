using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000157 RID: 343
[EntityConfigOrder(2)]
public class PacuTropicalConfig : IEntityConfig
{
	// Token: 0x06000689 RID: 1673 RVA: 0x0002F75C File Offset: 0x0002D95C
	public static GameObject CreatePacu(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToWildCreature(BasePacuConfig.CreatePrefab(id, "PacuTropicalBaseTrait", name, desc, anim_file, is_baby, "trp_", 303.15f, 353.15f, 283.15f, 373.15f), PacuTuning.PEN_SIZE_PER_CREATURE, false);
		gameObject.AddOrGet<DecorProvider>().SetValues(PacuTropicalConfig.DECOR);
		return gameObject;
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x0002F7B0 File Offset: 0x0002D9B0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(EntityTemplates.ExtendEntityToWildCreature(PacuTropicalConfig.CreatePacu("PacuTropical", STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.DESC, "pacu_kanim", false), PacuTuning.PEN_SIZE_PER_CREATURE, false), this as IHasDlcRestrictions, "PacuTropicalEgg", STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.EGG_NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuTropicalBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_TROPICAL, 502, false, true, 0.75f, false);
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x0002F83B File Offset: 0x0002DA3B
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x0002F83D File Offset: 0x0002DA3D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004F1 RID: 1265
	public const string ID = "PacuTropical";

	// Token: 0x040004F2 RID: 1266
	public const string BASE_TRAIT_ID = "PacuTropicalBaseTrait";

	// Token: 0x040004F3 RID: 1267
	public const string EGG_ID = "PacuTropicalEgg";

	// Token: 0x040004F4 RID: 1268
	public static readonly EffectorValues DECOR = TUNING.BUILDINGS.DECOR.BONUS.TIER4;

	// Token: 0x040004F5 RID: 1269
	public const int EGG_SORT_ORDER = 502;
}
