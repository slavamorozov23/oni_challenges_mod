using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class BeeConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600039D RID: 925 RVA: 0x0001FCD8 File Offset: 0x0001DED8
	public static GameObject CreateBee(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseBeeConfig.BaseBee(id, name, desc, anim_file, "BeeBaseTrait", DECOR.BONUS.TIER4, is_baby, null);
		Trait trait = Db.Get().CreateTrait("BeeBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 5f, name, false, false, true));
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x0600039E RID: 926 RVA: 0x0001FD75 File Offset: 0x0001DF75
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600039F RID: 927 RVA: 0x0001FD7C File Offset: 0x0001DF7C
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x0001FD7F File Offset: 0x0001DF7F
	public GameObject CreatePrefab()
	{
		return BeeConfig.CreateBee("Bee", STRINGS.CREATURES.SPECIES.BEE.NAME, STRINGS.CREATURES.SPECIES.BEE.DESC, "bee_kanim", false);
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x0001FDA5 File Offset: 0x0001DFA5
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x0001FDA7 File Offset: 0x0001DFA7
	public void OnSpawn(GameObject inst)
	{
		BaseBeeConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x040002D9 RID: 729
	public const string ID = "Bee";

	// Token: 0x040002DA RID: 730
	public const string BASE_TRAIT_ID = "BeeBaseTrait";
}
