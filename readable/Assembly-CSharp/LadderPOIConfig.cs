using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200028A RID: 650
public class LadderPOIConfig : IEntityConfig
{
	// Token: 0x06000D32 RID: 3378 RVA: 0x0004E69C File Offset: 0x0004C89C
	public GameObject CreatePrefab()
	{
		int num = 1;
		int num2 = 1;
		string id = "PropLadder";
		string name = STRINGS.BUILDINGS.PREFABS.PROPLADDER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPLADDER.DESC;
		float mass = 50f;
		int width = num;
		int height = num2;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("ladder_poi_kanim"), "off", Grid.SceneLayer.Building, width, height, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Polypropylene, true);
		component.Temperature = 294.15f;
		Ladder ladder = gameObject.AddOrGet<Ladder>();
		ladder.upwardsMovementSpeedMultiplier = 1.5f;
		ladder.downwardsMovementSpeedMultiplier = 1.5f;
		gameObject.AddOrGet<AnimTileable>();
		UnityEngine.Object.DestroyImmediate(gameObject.AddOrGet<OccupyArea>());
		OccupyArea occupyArea = gameObject.AddOrGet<OccupyArea>();
		occupyArea.SetCellOffsets(EntityTemplates.GenerateOffsets(num, num2));
		occupyArea.objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x0004E790 File Offset: 0x0004C990
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x0004E792 File Offset: 0x0004C992
	public void OnSpawn(GameObject inst)
	{
	}
}
