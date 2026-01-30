using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000308 RID: 776
public class IronCometConfig : IEntityConfig
{
	// Token: 0x06000FEB RID: 4075 RVA: 0x00060AA4 File Offset: 0x0005ECA4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(IronCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.IRONCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(3f, 20f);
		comet.temperatureRange = new Vector2(323.15f, 423.15f);
		comet.explosionOreCount = new Vector2I(2, 4);
		comet.entityDamage = 15;
		comet.totalTileDamage = 0.5f;
		comet.splashRadius = 1;
		comet.impactSound = "Meteor_Medium_Impact";
		comet.flyingSoundID = 1;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactMetal;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Iron, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_metal_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}

	// Token: 0x06000FEC RID: 4076 RVA: 0x00060BFE File Offset: 0x0005EDFE
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000FED RID: 4077 RVA: 0x00060C00 File Offset: 0x0005EE00
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A68 RID: 2664
	public static string ID = "IronComet";
}
