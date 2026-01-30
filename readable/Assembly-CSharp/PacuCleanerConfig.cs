using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000152 RID: 338
[EntityConfigOrder(2)]
public class PacuCleanerConfig : IEntityConfig
{
	// Token: 0x06000673 RID: 1651 RVA: 0x0002F378 File Offset: 0x0002D578
	public static GameObject CreatePacu(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BasePacuConfig.CreatePrefab(id, "PacuCleanerBaseTrait", name, desc, anim_file, is_baby, "glp_", 243.15f, 278.15f, 223.15f, 298.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, PacuTuning.PEN_SIZE_PER_CREATURE, false);
		if (!is_baby)
		{
			Storage storage = gameObject.AddComponent<Storage>();
			storage.capacityKg = 10f;
			storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
			PassiveElementConsumer passiveElementConsumer = gameObject.AddOrGet<PassiveElementConsumer>();
			passiveElementConsumer.elementToConsume = SimHashes.DirtyWater;
			passiveElementConsumer.consumptionRate = 0.2f;
			passiveElementConsumer.capacityKG = 10f;
			passiveElementConsumer.consumptionRadius = 3;
			passiveElementConsumer.showInStatusPanel = true;
			passiveElementConsumer.sampleCellOffset = new Vector3(0f, 0f, 0f);
			passiveElementConsumer.isRequired = false;
			passiveElementConsumer.storeOnConsume = true;
			passiveElementConsumer.showDescriptor = false;
			gameObject.AddOrGet<UpdateElementConsumerPosition>();
			BubbleSpawner bubbleSpawner = gameObject.AddComponent<BubbleSpawner>();
			bubbleSpawner.element = SimHashes.Water;
			bubbleSpawner.emitMass = 2f;
			bubbleSpawner.emitVariance = 0.5f;
			bubbleSpawner.initialVelocity = new Vector2f(0, 1);
			ElementConverter elementConverter = gameObject.AddOrGet<ElementConverter>();
			elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(SimHashes.DirtyWater.CreateTag(), 0.2f, true)
			};
			elementConverter.outputElements = new ElementConverter.OutputElement[]
			{
				new ElementConverter.OutputElement(0.2f, SimHashes.Water, 0f, true, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true)
			};
		}
		return gameObject;
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0002F4F0 File Offset: 0x0002D6F0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(EntityTemplates.ExtendEntityToWildCreature(PacuCleanerConfig.CreatePacu("PacuCleaner", STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.DESC, "pacu_kanim", false), PacuTuning.PEN_SIZE_PER_CREATURE, false), this as IHasDlcRestrictions, "PacuCleanerEgg", STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.EGG_NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuCleanerBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_CLEANER, 501, false, true, 0.75f, false);
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x0002F57B File Offset: 0x0002D77B
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x0002F580 File Offset: 0x0002D780
	public void OnSpawn(GameObject inst)
	{
		ElementConsumer component = inst.GetComponent<ElementConsumer>();
		if (component != null)
		{
			component.EnableConsumption(true);
		}
	}

	// Token: 0x040004E2 RID: 1250
	public const string ID = "PacuCleaner";

	// Token: 0x040004E3 RID: 1251
	public const string BASE_TRAIT_ID = "PacuCleanerBaseTrait";

	// Token: 0x040004E4 RID: 1252
	public const string EGG_ID = "PacuCleanerEgg";

	// Token: 0x040004E5 RID: 1253
	public const float POLLUTED_WATER_CONVERTED_PER_CYCLE = 120f;

	// Token: 0x040004E6 RID: 1254
	public const SimHashes INPUT_ELEMENT = SimHashes.DirtyWater;

	// Token: 0x040004E7 RID: 1255
	public const SimHashes OUTPUT_ELEMENT = SimHashes.Water;

	// Token: 0x040004E8 RID: 1256
	public static readonly EffectorValues DECOR = TUNING.BUILDINGS.DECOR.BONUS.TIER4;

	// Token: 0x040004E9 RID: 1257
	public const int EGG_SORT_ORDER = 501;
}
