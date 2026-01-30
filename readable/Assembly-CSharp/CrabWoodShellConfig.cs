using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000270 RID: 624
public class CrabWoodShellConfig : IEntityConfig
{
	// Token: 0x06000CAD RID: 3245 RVA: 0x0004C444 File Offset: 0x0004A644
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("CrabWoodShell", ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.VARIANT_WOOD.NAME, ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.VARIANT_WOOD.DESC, 1f, false, Assets.GetAnim("woodcrabshell_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.Organics,
			GameTags.MoltShell
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>();
		gameObject.AddComponent<EntitySizeVisualizer>().TierSetType = OreSizeVisualizerComponents.TiersSetType.WoodPokeShells;
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000CAE RID: 3246 RVA: 0x0004C4D9 File Offset: 0x0004A6D9
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
					component.Mass = component.Units * 100f;
				}
				KPrefabID component2 = inst.GetComponent<KPrefabID>();
				if (component2 != null)
				{
					component2.RemoveTag(GameTags.IndustrialIngredient);
				}
			}
		};
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x0004C505 File Offset: 0x0004A705
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040008C6 RID: 2246
	public const string ID = "CrabWoodShell";

	// Token: 0x040008C7 RID: 2247
	public static readonly Tag TAG = TagManager.Create("CrabWoodShell");

	// Token: 0x040008C8 RID: 2248
	public const float ADULT_MASS = 100f;

	// Token: 0x040008C9 RID: 2249
	public const string symbolPrefix = "wood_";
}
