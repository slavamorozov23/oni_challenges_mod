using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200026F RID: 623
public class CrabShellConfig : IEntityConfig
{
	// Token: 0x06000CA8 RID: 3240 RVA: 0x0004C368 File Offset: 0x0004A568
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("CrabShell", ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.NAME, ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.DESC, 1f, false, Assets.GetAnim("crabshell_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.Organics,
			GameTags.MoltShell
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>();
		gameObject.AddComponent<EntitySizeVisualizer>().TierSetType = OreSizeVisualizerComponents.TiersSetType.PokeShells;
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x0004C3FD File Offset: 0x0004A5FD
	public void OnPrefabInit(GameObject inst)
	{
		inst2.GetComponent<Compostable>().OnDeserializeCb = delegate(KMonoBehaviour inst)
		{
			if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 36))
			{
				inst.GetComponent<PrimaryElement>();
				PrimaryElement component = inst.GetComponent<PrimaryElement>();
				if (component != null)
				{
					component.MassPerUnit = 1f;
					component.Mass = component.Units * 10f;
				}
				KPrefabID component2 = inst.GetComponent<KPrefabID>();
				if (component2 != null)
				{
					component2.RemoveTag(GameTags.IndustrialIngredient);
				}
			}
		};
	}

	// Token: 0x06000CAA RID: 3242 RVA: 0x0004C429 File Offset: 0x0004A629
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040008C3 RID: 2243
	public const string ID = "CrabShell";

	// Token: 0x040008C4 RID: 2244
	public static readonly Tag TAG = TagManager.Create("CrabShell");

	// Token: 0x040008C5 RID: 2245
	public const float ADULT_MASS = 10f;
}
