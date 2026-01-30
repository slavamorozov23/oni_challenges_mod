using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000316 RID: 790
public class IridiumCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600103D RID: 4157 RVA: 0x00061E74 File Offset: 0x00060074
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x0600103E RID: 4158 RVA: 0x00061E7B File Offset: 0x0006007B
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600103F RID: 4159 RVA: 0x00061E80 File Offset: 0x00060080
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(IridiumCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.IRIDIUMCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(10f, 100f);
		comet.temperatureRange = new Vector2(473.15f, 548.15f);
		comet.explosionTemperatureRange = comet.temperatureRange;
		comet.explosionOreCount = new Vector2I(2, 4);
		comet.impactSound = "Meteor_copper_Impact";
		comet.flyingSoundID = 1;
		comet.EXHAUST_ELEMENT = SimHashes.CarbonDioxide;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactMetal;
		comet.entityDamage = 15;
		comet.totalTileDamage = 0.5f;
		comet.splashRadius = 1;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Iridium, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_iridium_kanim")
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

	// Token: 0x06001040 RID: 4160 RVA: 0x00061FF1 File Offset: 0x000601F1
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001041 RID: 4161 RVA: 0x00061FF3 File Offset: 0x000601F3
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A7B RID: 2683
	public static string ID = "IridiumComet";
}
