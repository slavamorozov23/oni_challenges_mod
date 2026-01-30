using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public class CarrotConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000987 RID: 2439 RVA: 0x0003F09C File Offset: 0x0003D29C
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x0003F0A3 File Offset: 0x0003D2A3
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x0003F0A8 File Offset: 0x0003D2A8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(CarrotConfig.ID, STRINGS.ITEMS.FOOD.CARROT.NAME, STRINGS.ITEMS.FOOD.CARROT.DESC, 1f, false, Assets.GetAnim("purplerootVegetable_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.CARROT);
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x0003F10C File Offset: 0x0003D30C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x0003F10E File Offset: 0x0003D30E
	public void OnSpawn(GameObject inst)
	{
		inst.Subscribe(-10536414, CarrotConfig.OnEatCompleteDelegate);
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x0003F124 File Offset: 0x0003D324
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
				if (UnityEngine.Random.value < CarrotConfig.SEEDS_PER_FRUIT_CHANCE)
				{
					num++;
				}
			}
			if (num > 0)
			{
				Vector3 vector = edible.transform.GetPosition() + new Vector3(0f, 0.05f, 0f);
				vector = Grid.CellToPosCCC(Grid.PosToCell(vector), Grid.SceneLayer.Ore);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("CarrotPlantSeed")), vector, Grid.SceneLayer.Ore, null, 0);
				PrimaryElement component = edible.GetComponent<PrimaryElement>();
				PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
				component2.Temperature = component.Temperature;
				component2.Units = (float)num;
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x0400070A RID: 1802
	public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

	// Token: 0x0400070B RID: 1803
	public static string ID = "Carrot";

	// Token: 0x0400070C RID: 1804
	private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		CarrotConfig.OnEatComplete(component);
	});
}
