using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000319 RID: 793
public class AlgaeCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001052 RID: 4178 RVA: 0x0006234D File Offset: 0x0006054D
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001053 RID: 4179 RVA: 0x00062354 File Offset: 0x00060554
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001054 RID: 4180 RVA: 0x00062358 File Offset: 0x00060558
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(AlgaeCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.ALGAECOMET.NAME, "meteor_algae_kanim", SimHashes.Algae, new Vector2(3f, 20f), new Vector2(310.15f, 323.15f), "Meteor_algae_Impact", 7, SimHashes.Void, SpawnFXHashes.MeteorImpactAlgae, 0.3f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(2, 4);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		return gameObject;
	}

	// Token: 0x06001055 RID: 4181 RVA: 0x000623EF File Offset: 0x000605EF
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001056 RID: 4182 RVA: 0x000623F1 File Offset: 0x000605F1
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A80 RID: 2688
	public static string ID = "AlgaeComet";
}
