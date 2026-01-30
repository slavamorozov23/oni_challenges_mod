using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000279 RID: 633
public class ResearchDatabankConfig : IEntityConfig
{
	// Token: 0x06000CDF RID: 3295 RVA: 0x0004CA98 File Offset: 0x0004AC98
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("ResearchDatabank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.DESC, 1f, true, Assets.GetAnim("floppy_disc_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.TechComponents,
			GameTags.Experimental,
			GameTags.PedestalDisplayable
		});
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			gameObject.AddTag(GameTags.HideFromSpawnTool);
		}
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = (float)ROCKETRY.DESTINATION_RESEARCH.BASIC;
		return gameObject;
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x0004CB3C File Offset: 0x0004AD3C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CE1 RID: 3297 RVA: 0x0004CB40 File Offset: 0x0004AD40
	public void OnSpawn(GameObject inst)
	{
		if (Game.IsDlcActiveForCurrentSave("DLC2_ID") && SaveLoader.Instance.ClusterLayout != null && SaveLoader.Instance.ClusterLayout.clusterTags.Contains("CeresCluster"))
		{
			inst.AddOrGet<KBatchedAnimController>().SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("floppy_disc_ceres_kanim")
			});
		}
	}

	// Token: 0x040008E2 RID: 2274
	public const string ID = "ResearchDatabank";

	// Token: 0x040008E3 RID: 2275
	public static readonly Tag TAG = TagManager.Create("ResearchDatabank");

	// Token: 0x040008E4 RID: 2276
	public const float MASS = 1f;
}
