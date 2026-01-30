using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000343 RID: 835
public class PlantFiberConfig : IEntityConfig
{
	// Token: 0x06001146 RID: 4422 RVA: 0x00066834 File Offset: 0x00064A34
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PlantFiber", ITEMS.INDUSTRIAL_PRODUCTS.PLANT_FIBER.NAME, ITEMS.INDUSTRIAL_PRODUCTS.PLANT_FIBER.DESC, 1f, false, Assets.GetAnim("plant_matter_kanim"), "idle1", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialProduct,
			GameTags.PedestalDisplayable
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddComponent<EntitySizeVisualizer>().TierSetType = OreSizeVisualizerComponents.TiersSetType.PlantFiber;
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06001147 RID: 4423 RVA: 0x000668C2 File Offset: 0x00064AC2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001148 RID: 4424 RVA: 0x000668C4 File Offset: 0x00064AC4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AFD RID: 2813
	public const string ID = "PlantFiber";
}
