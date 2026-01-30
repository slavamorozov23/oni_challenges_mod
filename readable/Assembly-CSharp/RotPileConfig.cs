using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001FC RID: 508
public class RotPileConfig : IEntityConfig
{
	// Token: 0x06000A44 RID: 2628 RVA: 0x000405DC File Offset: 0x0003E7DC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(RotPileConfig.ID, STRINGS.ITEMS.FOOD.ROTPILE.NAME, STRINGS.ITEMS.FOOD.ROTPILE.DESC, 1f, false, Assets.GetAnim("rotfood_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Organics, false);
		component.AddTag(GameTags.Compostable, false);
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<OccupyArea>();
		gameObject.AddOrGet<Modifiers>();
		gameObject.AddOrGet<RotPile>();
		gameObject.AddComponent<DecorProvider>().SetValues(DECOR.PENALTY.TIER2);
		return gameObject;
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x0004067F File Offset: 0x0003E87F
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<DecorProvider>().overrideName = STRINGS.ITEMS.FOOD.ROTPILE.NAME;
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x00040696 File Offset: 0x0003E896
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000758 RID: 1880
	public static string ID = "RotPile";
}
