using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000155 RID: 341
[EntityConfigOrder(1)]
public class PacuConfig : IEntityConfig
{
	// Token: 0x06000680 RID: 1664 RVA: 0x0002F62C File Offset: 0x0002D82C
	public static GameObject CreatePacu(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToWildCreature(BasePacuConfig.CreatePrefab(id, "PacuBaseTrait", name, desc, anim_file, is_baby, null, 273.15f, 333.15f, 253.15f, 373.15f), PacuTuning.PEN_SIZE_PER_CREATURE, false);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x0002F674 File Offset: 0x0002D874
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(PacuConfig.CreatePacu("Pacu", CREATURES.SPECIES.PACU.NAME, CREATURES.SPECIES.PACU.DESC, "pacu_kanim", false), this as IHasDlcRestrictions, "PacuEgg", CREATURES.SPECIES.PACU.EGG_NAME, CREATURES.SPECIES.PACU.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_BASE, 500, false, true, 0.75f, false);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x0002F6FF File Offset: 0x0002D8FF
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.AddOrGet<LoopingSounds>();
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x0002F708 File Offset: 0x0002D908
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004EC RID: 1260
	public const string ID = "Pacu";

	// Token: 0x040004ED RID: 1261
	public const string BASE_TRAIT_ID = "PacuBaseTrait";

	// Token: 0x040004EE RID: 1262
	public const string EGG_ID = "PacuEgg";

	// Token: 0x040004EF RID: 1263
	public const int EGG_SORT_ORDER = 500;
}
