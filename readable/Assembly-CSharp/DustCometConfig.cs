using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200030C RID: 780
public class DustCometConfig : IEntityConfig
{
	// Token: 0x06001001 RID: 4097 RVA: 0x00060FAC File Offset: 0x0005F1AC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(DustCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.DUSTCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(0.2f, 0.5f);
		comet.temperatureRange = new Vector2(223.15f, 253.15f);
		comet.entityDamage = 2;
		comet.totalTileDamage = 0.15f;
		comet.splashRadius = 0;
		comet.impactSound = "Meteor_dust_heavy_Impact";
		comet.flyingSoundID = 0;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Regolith, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_sand_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}

	// Token: 0x06001002 RID: 4098 RVA: 0x000610F8 File Offset: 0x0005F2F8
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001003 RID: 4099 RVA: 0x000610FA File Offset: 0x0005F2FA
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A6C RID: 2668
	public static string ID = "DustComet";
}
