using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200030D RID: 781
public class NuclearWasteCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001006 RID: 4102 RVA: 0x00061110 File Offset: 0x0005F310
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x00061117 File Offset: 0x0005F317
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x0006111C File Offset: 0x0005F31C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(NuclearWasteCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.NUCLEAR_WASTE.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(NuclearWasteCometConfig.MASS, NuclearWasteCometConfig.MASS);
		comet.EXHAUST_ELEMENT = SimHashes.Fallout;
		comet.EXHAUST_RATE = NuclearWasteCometConfig.MASS * 0.2f;
		comet.temperatureRange = new Vector2(473.15f, 573.15f);
		comet.entityDamage = 2;
		comet.totalTileDamage = 0.45f;
		comet.splashRadius = 0;
		comet.impactSound = "Meteor_Nuclear_Impact";
		comet.flyingSoundID = 3;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		comet.addTiles = 1;
		comet.diseaseIdx = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id);
		comet.addDiseaseCount = 1000000;
		comet.affectedByDifficulty = false;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Corium, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("nuclear_metldown_comet_fx_kanim")
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

	// Token: 0x06001009 RID: 4105 RVA: 0x000612CB File Offset: 0x0005F4CB
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600100A RID: 4106 RVA: 0x000612CD File Offset: 0x0005F4CD
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A6D RID: 2669
	public static string ID = "NuclearWasteComet";

	// Token: 0x04000A6E RID: 2670
	public static float MASS = 1f;
}
