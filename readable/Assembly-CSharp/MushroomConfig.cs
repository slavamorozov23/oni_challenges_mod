using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001F0 RID: 496
public class MushroomConfig : IEntityConfig
{
	// Token: 0x06000A08 RID: 2568 RVA: 0x0003FDDC File Offset: 0x0003DFDC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(MushroomConfig.ID, STRINGS.ITEMS.FOOD.MUSHROOM.NAME, STRINGS.ITEMS.FOOD.MUSHROOM.DESC, 1f, false, Assets.GetAnim("funguscap_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.77f, 0.48f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.MUSHROOM);
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x0003FE40 File Offset: 0x0003E040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x0003FE42 File Offset: 0x0003E042
	public void OnSpawn(GameObject inst)
	{
		inst.Subscribe(-10536414, MushroomConfig.OnEatCompleteDelegate);
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x0003FE58 File Offset: 0x0003E058
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
				if (UnityEngine.Random.value < MushroomConfig.SEEDS_PER_FRUIT_CHANCE)
				{
					num++;
				}
			}
			if (num > 0)
			{
				Vector3 vector = edible.transform.GetPosition() + new Vector3(0f, 0.05f, 0f);
				vector = Grid.CellToPosCCC(Grid.PosToCell(vector), Grid.SceneLayer.Ore);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("MushroomSeed")), vector, Grid.SceneLayer.Ore, null, 0);
				PrimaryElement component = edible.GetComponent<PrimaryElement>();
				PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
				component2.Temperature = component.Temperature;
				component2.Units = (float)num;
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x0400073C RID: 1852
	public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

	// Token: 0x0400073D RID: 1853
	public static string ID = "Mushroom";

	// Token: 0x0400073E RID: 1854
	private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		MushroomConfig.OnEatComplete(component);
	});
}
