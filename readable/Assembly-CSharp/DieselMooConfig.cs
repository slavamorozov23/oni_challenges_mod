using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200011E RID: 286
[EntityConfigOrder(2)]
public class DieselMooConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000551 RID: 1361 RVA: 0x0002A73F File Offset: 0x0002893F
	public string[] GetRequiredDlcIds()
	{
		return null;
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x0002A742 File Offset: 0x00028942
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x0002A748 File Offset: 0x00028948
	public static GameObject CreateMoo(string id, string name, string desc, string anim_file, List<BeckoningMonitor.SongChance> initialSongChances, bool is_baby)
	{
		GameObject gameObject = BaseMooConfig.BaseMoo(id, name, CREATURES.SPECIES.DIESELMOO.DESC, "DieselMooBaseTrait", anim_file, initialSongChances, is_baby, "die_");
		EntityTemplates.ExtendEntityToWildCreature(gameObject, MooTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("DieselMooBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MooTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -MooTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 50f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, MooTuning.STANDARD_LIFESPAN, name, false, false, true));
		BaseMooConfig.SetupBaseDiet(gameObject, DieselMooConfig.POOP_ELEMENT);
		gameObject.AddOrGetDef<BeckoningMonitor.Def>().effectId = "HuskyMooFed";
		MilkProductionMonitor.Def def = gameObject.AddOrGetDef<MilkProductionMonitor.Def>();
		def.effectId = "HuskyMooWellFed";
		def.element = DieselMooConfig.MILK_ELEMENT;
		def.Capacity = 800f;
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x0002A8A6 File Offset: 0x00028AA6
	public GameObject CreatePrefab()
	{
		return DieselMooConfig.CreateMoo("DieselMoo", CREATURES.SPECIES.DIESELMOO.NAME, CREATURES.SPECIES.DIESELMOO.DESC, "gassy_moo_kanim", MooTuning.DieselSongChances, false);
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x0002A8D1 File Offset: 0x00028AD1
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x0002A8D3 File Offset: 0x00028AD3
	public void OnSpawn(GameObject inst)
	{
		BaseMooConfig.OnSpawn(inst);
	}

	// Token: 0x040003DC RID: 988
	public const string ID = "DieselMoo";

	// Token: 0x040003DD RID: 989
	public const string BASE_TRAIT_ID = "DieselMooBaseTrait";

	// Token: 0x040003DE RID: 990
	public static Tag POOP_ELEMENT = SimHashes.CarbonDioxide.CreateTag();

	// Token: 0x040003DF RID: 991
	public static SimHashes MILK_ELEMENT = SimHashes.RefinedLipid;
}
