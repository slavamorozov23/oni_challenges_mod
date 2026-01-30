using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000318 RID: 792
public class LightDustCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600104B RID: 4171 RVA: 0x000621A3 File Offset: 0x000603A3
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600104C RID: 4172 RVA: 0x000621AA File Offset: 0x000603AA
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600104D RID: 4173 RVA: 0x000621B0 File Offset: 0x000603B0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(LightDustCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.LIGHTDUSTCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(10f, 14f);
		comet.temperatureRange = new Vector2(223.15f, 253.15f);
		comet.explosionTemperatureRange = comet.temperatureRange;
		comet.explosionOreCount = new Vector2I(1, 2);
		comet.explosionSpeedRange = new Vector2(4f, 7f);
		comet.entityDamage = 0;
		comet.totalTileDamage = 0f;
		comet.splashRadius = 0;
		comet.impactSound = "Meteor_dust_light_Impact";
		comet.flyingSoundID = 0;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactLightDust;
		comet.EXHAUST_ELEMENT = SimHashes.Void;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Regolith, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_dust_kanim")
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

	// Token: 0x0600104E RID: 4174 RVA: 0x00062335 File Offset: 0x00060535
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600104F RID: 4175 RVA: 0x00062337 File Offset: 0x00060537
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A7F RID: 2687
	public static string ID = "LightDustComet";
}
