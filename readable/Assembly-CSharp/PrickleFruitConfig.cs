using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001F8 RID: 504
public class PrickleFruitConfig : IEntityConfig
{
	// Token: 0x06000A31 RID: 2609 RVA: 0x000402C8 File Offset: 0x0003E4C8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(PrickleFruitConfig.ID, STRINGS.ITEMS.FOOD.PRICKLEFRUIT.NAME, STRINGS.ITEMS.FOOD.PRICKLEFRUIT.DESC, 1f, false, Assets.GetAnim("bristleberry_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.PRICKLEFRUIT);
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x0004032C File Offset: 0x0003E52C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x0004032E File Offset: 0x0003E52E
	public void OnSpawn(GameObject inst)
	{
		inst.Subscribe(-10536414, PrickleFruitConfig.OnEatCompleteDelegate);
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x00040344 File Offset: 0x0003E544
	private static void OnEatComplete(Edible edible)
	{
		if (edible != null)
		{
			int num = 0;
			float unitsConsumed = edible.unitsConsumed;
			int num2 = Mathf.FloorToInt(unitsConsumed);
			float num3 = unitsConsumed % 1f;
			if (UnityEngine.Random.value < num3)
			{
				num2++;
			}
			for (int i = 0; i < num2; i++)
			{
				if (UnityEngine.Random.value < PrickleFruitConfig.SEEDS_PER_FRUIT_CHANCE)
				{
					num++;
				}
			}
			if (num > 0)
			{
				Vector3 vector = edible.transform.GetPosition() + new Vector3(0f, 0.05f, 0f);
				vector = Grid.CellToPosCCC(Grid.PosToCell(vector), Grid.SceneLayer.Ore);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("PrickleFlowerSeed")), vector, Grid.SceneLayer.Ore, null, 0);
				PrimaryElement component = edible.GetComponent<PrimaryElement>();
				PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
				component2.Temperature = component.Temperature;
				component2.Units = (float)num;
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x0400074E RID: 1870
	public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

	// Token: 0x0400074F RID: 1871
	public static string ID = "PrickleFruit";

	// Token: 0x04000750 RID: 1872
	private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		PrickleFruitConfig.OnEatComplete(component);
	});
}
