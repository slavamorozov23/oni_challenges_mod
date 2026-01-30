using System;
using System.Collections.Generic;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000320 RID: 800
public class EggConfig
{
	// Token: 0x06001080 RID: 4224 RVA: 0x00062924 File Offset: 0x00060B24
	[Obsolete("Mod compatibility: Use CreateEgg with requiredDlcIds and forbiddenDlcIds")]
	public static GameObject CreateEgg(string id, string name, string desc, Tag creature_id, string anim, float mass, int egg_sort_order, float base_incubation_rate)
	{
		return EggConfig.CreateEgg(id, name, desc, creature_id, anim, mass, egg_sort_order, base_incubation_rate, null, null);
	}

	// Token: 0x06001081 RID: 4225 RVA: 0x00062944 File Offset: 0x00060B44
	[Obsolete("Mod compatibility: Use CreateEgg with requiredDlcIds and forbiddenDlcIds")]
	public static GameObject CreateEgg(string id, string name, string desc, Tag creature_id, string anim, float mass, int egg_sort_order, float base_incubation_rate, string[] dlcIds)
	{
		string[] requiredDlcIds;
		string[] forbiddenDlcIds;
		DlcManager.ConvertAvailableToRequireAndForbidden(dlcIds, out requiredDlcIds, out forbiddenDlcIds);
		return EggConfig.CreateEgg(id, name, desc, creature_id, anim, mass, egg_sort_order, base_incubation_rate, requiredDlcIds, forbiddenDlcIds);
	}

	// Token: 0x06001082 RID: 4226 RVA: 0x00062970 File Offset: 0x00060B70
	public static GameObject CreateEgg(string id, string name, string desc, Tag creature_id, string anim, float mass, int egg_sort_order, float base_incubation_rate, string[] requiredDlcIds, string[] forbiddenDlcIds)
	{
		return EggConfig.CreateEgg(id, name, desc, creature_id, anim, mass, egg_sort_order, base_incubation_rate, requiredDlcIds, forbiddenDlcIds, false);
	}

	// Token: 0x06001083 RID: 4227 RVA: 0x00062994 File Offset: 0x00060B94
	public static GameObject CreateEgg(string id, string name, string desc, Tag creature_id, string anim, float mass, int egg_sort_order, float base_incubation_rate, string[] requiredDlcIds, string[] forbiddenDlcIds, bool preventEggDrops)
	{
		return EggConfig.CreateEgg(id, name, desc, creature_id, anim, mass, egg_sort_order, base_incubation_rate, requiredDlcIds, forbiddenDlcIds, preventEggDrops, mass);
	}

	// Token: 0x06001084 RID: 4228 RVA: 0x000629BC File Offset: 0x00060BBC
	public static GameObject CreateEgg(string id, string name, string desc, Tag creature_id, string anim, float mass, int egg_sort_order, float base_incubation_rate, string[] requiredDlcIds, string[] forbiddenDlcIds, bool preventEggDrops, float eggMassToDrop)
	{
		return EggConfig.CreateEgg(id, name, desc, creature_id, anim, mass, egg_sort_order, base_incubation_rate, requiredDlcIds, forbiddenDlcIds, preventEggDrops, eggMassToDrop, true);
	}

	// Token: 0x06001085 RID: 4229 RVA: 0x000629E4 File Offset: 0x00060BE4
	public static GameObject CreateEgg(string id, string name, string desc, Tag creature_id, string anim, float mass, int egg_sort_order, float base_incubation_rate, string[] requiredDlcIds, string[] forbiddenDlcIds, bool preventEggDrops, float eggMassToDrop, bool allowCrackerRecipeCreation = true)
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(id, name, desc, mass, true, Assets.GetAnim(anim), "idle", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.8f, true, 0, SimHashes.Creature, null);
		gameObject.AddOrGet<KBoxCollider2D>().offset = new Vector2f(0f, 0.36f);
		gameObject.AddOrGet<Pickupable>().sortOrder = SORTORDER.EGGS + egg_sort_order;
		gameObject.AddOrGet<Effects>();
		KPrefabID kprefabID = gameObject.AddOrGet<KPrefabID>();
		kprefabID.AddTag(GameTags.Egg, false);
		kprefabID.AddTag(GameTags.IncubatableEgg, false);
		kprefabID.AddTag(GameTags.PedestalDisplayable, false);
		kprefabID.requiredDlcIds = requiredDlcIds;
		kprefabID.forbiddenDlcIds = forbiddenDlcIds;
		IncubationMonitor.Def def = gameObject.AddOrGetDef<IncubationMonitor.Def>();
		def.preventEggDrops = preventEggDrops;
		def.spawnedCreature = creature_id;
		def.baseIncubationRate = base_incubation_rate;
		gameObject.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
		UnityEngine.Object.Destroy(gameObject.GetComponent<EntitySplitter>());
		Assets.AddPrefab(gameObject.GetComponent<KPrefabID>());
		EggCrackerConfig.RegisterEgg(id, name, desc, eggMassToDrop, requiredDlcIds, forbiddenDlcIds, EggConfig.CUSTOM_EGG_OUTPUTS.ContainsKey(creature_id) ? EggConfig.CUSTOM_EGG_OUTPUTS[creature_id].ToArray() : null, allowCrackerRecipeCreation);
		return gameObject;
	}

	// Token: 0x04000A8D RID: 2701
	public static Dictionary<Tag, List<global::Tuple<Tag, float>>> CUSTOM_EGG_OUTPUTS = new Dictionary<Tag, List<global::Tuple<Tag, float>>>();
}
