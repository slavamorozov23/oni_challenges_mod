using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000307 RID: 775
public class RockCometConfig : IEntityConfig
{
	// Token: 0x06000FE6 RID: 4070 RVA: 0x00060928 File Offset: 0x0005EB28
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(RockCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.ROCKCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		float mass = ElementLoader.FindElementByHash(SimHashes.Regolith).defaultValues.mass;
		comet.massRange = new Vector2(mass * 0.8f * 6f, mass * 1.2f * 6f);
		comet.temperatureRange = new Vector2(323.15f, 423.15f);
		comet.addTiles = 6;
		comet.addTilesMinHeight = 2;
		comet.addTilesMaxHeight = 8;
		comet.entityDamage = 20;
		comet.totalTileDamage = 0f;
		comet.splashRadius = 1;
		comet.impactSound = "Meteor_Large_Impact";
		comet.flyingSoundID = 2;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDirt;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Regolith, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_rock_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}

	// Token: 0x06000FE7 RID: 4071 RVA: 0x00060A89 File Offset: 0x0005EC89
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000FE8 RID: 4072 RVA: 0x00060A8B File Offset: 0x0005EC8B
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A65 RID: 2661
	public static readonly string ID = "RockComet";

	// Token: 0x04000A66 RID: 2662
	private const SimHashes element = SimHashes.Regolith;

	// Token: 0x04000A67 RID: 2663
	private const int ADDED_CELLS = 6;
}
