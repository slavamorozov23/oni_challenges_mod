using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200032F RID: 815
public class LargeImpactorCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060010D6 RID: 4310 RVA: 0x00064D96 File Offset: 0x00062F96
	public string[] GetRequiredDlcIds()
	{
		return new string[]
		{
			"DLC4_ID"
		};
	}

	// Token: 0x060010D7 RID: 4311 RVA: 0x00064DA6 File Offset: 0x00062FA6
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060010D8 RID: 4312 RVA: 0x00064DAC File Offset: 0x00062FAC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(LargeImpactorCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.ROCKCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		LargeComet largeComet = gameObject.AddOrGet<LargeComet>();
		largeComet.impactSound = "Meteor_Large_Impact";
		largeComet.flyingSoundID = 2;
		largeComet.additionalAnimFiles.Add(new KeyValuePair<string, string>("asteroid_wind_kanim", "wind_loop"));
		largeComet.additionalAnimFiles.Add(new KeyValuePair<string, string>("asteroid_flame_inner_kanim", "flame_loop"));
		largeComet.mainAnimFile = new KeyValuePair<string, string>("asteroid_001_kanim", "idle");
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Regolith, true);
		primaryElement.Temperature = 20000f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("asteroid_flame_outer_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "flame_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.animScale = 0.2f;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x00064EC8 File Offset: 0x000630C8
	public void OnPrefabInit(GameObject go)
	{
		LargeComet largeComet = go.AddOrGet<LargeComet>();
		largeComet.additionalAnimFiles.Add(new KeyValuePair<string, string>("asteroid_wind_kanim", "wind_loop"));
		largeComet.additionalAnimFiles.Add(new KeyValuePair<string, string>("asteroid_flame_inner_kanim", "flame_loop"));
		largeComet.mainAnimFile = new KeyValuePair<string, string>("asteroid_001_kanim", "idle");
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x00064F23 File Offset: 0x00063123
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000AC3 RID: 2755
	public static readonly string ID = "LargeImpactorComet";

	// Token: 0x04000AC4 RID: 2756
	private const SimHashes element = SimHashes.Regolith;

	// Token: 0x04000AC5 RID: 2757
	private const int ADDED_CELLS = 6;
}
