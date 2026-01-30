using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200028C RID: 652
public class LandingPodConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000D3F RID: 3391 RVA: 0x0004E952 File Offset: 0x0004CB52
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x0004E959 File Offset: 0x0004CB59
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x0004E95C File Offset: 0x0004CB5C
	public GameObject CreatePrefab()
	{
		string id = "LandingPod";
		string name = STRINGS.BUILDINGS.PREFABS.LANDING_POD.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.LANDING_POD.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("rocket_puft_pod_kanim"), "grounded", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<PodLander>();
		gameObject.AddOrGet<MinionStorage>();
		return gameObject;
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x0004E9CB File Offset: 0x0004CBCB
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000D43 RID: 3395 RVA: 0x0004E9E2 File Offset: 0x0004CBE2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400090B RID: 2315
	public const string ID = "LandingPod";
}
