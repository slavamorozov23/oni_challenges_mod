using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class MooConfig : IEntityConfig
{
	// Token: 0x0600063C RID: 1596 RVA: 0x0002E79C File Offset: 0x0002C99C
	public static GameObject CreateMoo(string id, string name, string desc, string anim_file, List<BeckoningMonitor.SongChance> initialSongChances, bool is_baby)
	{
		GameObject gameObject = BaseMooConfig.BaseMoo(id, name, CREATURES.SPECIES.MOO.DESC, "MooBaseTrait", anim_file, initialSongChances, is_baby, null);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, MooTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("MooBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MooTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -MooTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 50f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, MooTuning.STANDARD_LIFESPAN, name, false, false, true));
		BaseMooConfig.SetupBaseDiet(gameObject, MooConfig.POOP_ELEMENT);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x0002E8C2 File Offset: 0x0002CAC2
	public GameObject CreatePrefab()
	{
		return MooConfig.CreateMoo("Moo", CREATURES.SPECIES.MOO.NAME, CREATURES.SPECIES.MOO.DESC, "gassy_moo_kanim", MooTuning.BaseSongChances, false);
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x0002E8ED File Offset: 0x0002CAED
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x0002E8EF File Offset: 0x0002CAEF
	public void OnSpawn(GameObject inst)
	{
		BaseMooConfig.OnSpawn(inst);
	}

	// Token: 0x040004B5 RID: 1205
	public const string ID = "Moo";

	// Token: 0x040004B6 RID: 1206
	public const string BASE_TRAIT_ID = "MooBaseTrait";

	// Token: 0x040004B7 RID: 1207
	public const SimHashes CONSUME_ELEMENT = SimHashes.Carbon;

	// Token: 0x040004B8 RID: 1208
	public static Tag POOP_ELEMENT = SimHashes.Methane.CreateTag();
}
