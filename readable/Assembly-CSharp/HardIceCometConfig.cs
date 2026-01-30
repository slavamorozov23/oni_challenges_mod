using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000317 RID: 791
public class HardIceCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001044 RID: 4164 RVA: 0x00062009 File Offset: 0x00060209
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001045 RID: 4165 RVA: 0x00062010 File Offset: 0x00060210
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001046 RID: 4166 RVA: 0x00062014 File Offset: 0x00060214
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(HardIceCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.HARDICECOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		float mass = ElementLoader.FindElementByHash(SimHashes.CrushedIce).defaultValues.mass;
		comet.massRange = new Vector2(mass * 0.8f * 6f, mass * 1.2f * 6f);
		comet.temperatureRange = new Vector2(173.15f, 248.15f);
		comet.explosionTemperatureRange = comet.temperatureRange;
		comet.addTiles = 6;
		comet.addTilesMinHeight = 2;
		comet.addTilesMaxHeight = 8;
		comet.entityDamage = 0;
		comet.totalTileDamage = 0f;
		comet.splashRadius = 1;
		comet.impactSound = "Meteor_ice_Impact";
		comet.flyingSoundID = 6;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactIce;
		comet.EXHAUST_ELEMENT = SimHashes.Oxygen;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.CrushedIce, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_ice_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}

	// Token: 0x06001047 RID: 4167 RVA: 0x0006218B File Offset: 0x0006038B
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001048 RID: 4168 RVA: 0x0006218D File Offset: 0x0006038D
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A7C RID: 2684
	public static readonly string ID = "HardIceComet";

	// Token: 0x04000A7D RID: 2685
	private const SimHashes element = SimHashes.CrushedIce;

	// Token: 0x04000A7E RID: 2686
	private const int ADDED_CELLS = 6;
}
