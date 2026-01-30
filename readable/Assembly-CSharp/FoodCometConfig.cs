using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200030F RID: 783
public class FoodCometConfig : IEntityConfig
{
	// Token: 0x06001012 RID: 4114 RVA: 0x000614A4 File Offset: 0x0005F6A4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(FoodCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.FOODCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(0.2f, 0.5f);
		comet.temperatureRange = new Vector2(298.15f, 303.15f);
		comet.entityDamage = 0;
		comet.totalTileDamage = 0f;
		comet.splashRadius = 0;
		comet.impactSound = "Meteor_dust_heavy_Impact";
		comet.flyingSoundID = 0;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		comet.canHitDuplicants = true;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Creature, true);
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
		comet.EXHAUST_ELEMENT = SimHashes.Void;
		gameObject.AddTag(GameTags.Comet);
		gameObject.AddTag(GameTags.DeprecatedContent);
		return gameObject;
	}

	// Token: 0x06001013 RID: 4115 RVA: 0x00061610 File Offset: 0x0005F810
	public void OnPrefabInit(GameObject go)
	{
		Comet component = go.GetComponent<Comet>();
		component.OnImpact = (System.Action)Delegate.Combine(component.OnImpact, new System.Action(delegate()
		{
			int i = 10;
			while (i > 0)
			{
				i--;
				Vector3 vector = go.transform.position + new Vector3((float)UnityEngine.Random.Range(-2, 3), (float)UnityEngine.Random.Range(-2, 3), 0f);
				if (!Grid.Solid[Grid.PosToCell(vector)])
				{
					GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("FoodSplat"), vector);
					gameObject.SetActive(true);
					gameObject.transform.Rotate(0f, 0f, (float)UnityEngine.Random.Range(-90, 90));
					i = 0;
				}
			}
		}));
	}

	// Token: 0x06001014 RID: 4116 RVA: 0x00061656 File Offset: 0x0005F856
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A70 RID: 2672
	public static string ID = "FoodComet";
}
