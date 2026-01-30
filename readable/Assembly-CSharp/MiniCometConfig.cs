using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000337 RID: 823
public class MiniCometConfig : IEntityConfig
{
	// Token: 0x06001103 RID: 4355 RVA: 0x0006553C File Offset: 0x0006373C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MiniCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.MINICOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		MiniComet miniComet = gameObject.AddOrGet<MiniComet>();
		Sim.PhysicsData defaultValues = ElementLoader.FindElementByHash(SimHashes.Regolith).defaultValues;
		miniComet.impactSound = "MeteorDamage_Rock";
		miniComet.flyingSoundID = 2;
		miniComet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		gameObject.AddOrGet<PrimaryElement>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_sand_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.AddTag(GameTags.Comet);
		gameObject.AddTag(GameTags.HideFromSpawnTool);
		gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
		return gameObject;
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x0006562A File Offset: 0x0006382A
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001105 RID: 4357 RVA: 0x0006562C File Offset: 0x0006382C
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000AE7 RID: 2791
	public static readonly string ID = "MiniComet";

	// Token: 0x04000AE8 RID: 2792
	private const SimHashes element = SimHashes.Regolith;

	// Token: 0x04000AE9 RID: 2793
	private const int ADDED_CELLS = 6;
}
