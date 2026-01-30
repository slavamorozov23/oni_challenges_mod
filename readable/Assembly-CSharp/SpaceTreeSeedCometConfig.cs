using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000315 RID: 789
public class SpaceTreeSeedCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001036 RID: 4150 RVA: 0x00061CC9 File Offset: 0x0005FEC9
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06001037 RID: 4151 RVA: 0x00061CD0 File Offset: 0x0005FED0
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001038 RID: 4152 RVA: 0x00061CD4 File Offset: 0x0005FED4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(SpaceTreeSeedCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.SPACETREESEEDCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		SpaceTreeSeededComet spaceTreeSeededComet = gameObject.AddOrGet<SpaceTreeSeededComet>();
		spaceTreeSeededComet.massRange = new Vector2(50f, 100f);
		spaceTreeSeededComet.temperatureRange = new Vector2(253.15f, 263.15f);
		spaceTreeSeededComet.explosionTemperatureRange = spaceTreeSeededComet.temperatureRange;
		spaceTreeSeededComet.impactSound = "Meteor_copper_Impact";
		spaceTreeSeededComet.flyingSoundID = 5;
		spaceTreeSeededComet.EXHAUST_ELEMENT = SimHashes.Void;
		spaceTreeSeededComet.explosionEffectHash = SpawnFXHashes.None;
		spaceTreeSeededComet.entityDamage = 0;
		spaceTreeSeededComet.totalTileDamage = 0f;
		spaceTreeSeededComet.splashRadius = 1;
		spaceTreeSeededComet.addTiles = 3;
		spaceTreeSeededComet.addTilesMinHeight = 1;
		spaceTreeSeededComet.addTilesMaxHeight = 2;
		spaceTreeSeededComet.lootOnDestroyedByMissile = new string[]
		{
			"SpaceTreeSeed"
		};
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Snow, true);
		primaryElement.Temperature = (spaceTreeSeededComet.temperatureRange.x + spaceTreeSeededComet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_bonbon_snow_kanim")
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

	// Token: 0x06001039 RID: 4153 RVA: 0x00061E5C File Offset: 0x0006005C
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x00061E5E File Offset: 0x0006005E
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A7A RID: 2682
	public static string ID = "SpaceTreeSeedComet";
}
