using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200030B RID: 779
public class FullereneCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000FFA RID: 4090 RVA: 0x00060EFE File Offset: 0x0005F0FE
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000FFB RID: 4091 RVA: 0x00060F05 File Offset: 0x0005F105
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000FFC RID: 4092 RVA: 0x00060F08 File Offset: 0x0005F108
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(FullereneCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.FULLERENECOMET.NAME, "meteor_fullerene_kanim", SimHashes.Fullerene, new Vector2(3f, 20f), new Vector2(323.15f, 423.15f), "Meteor_Medium_Impact", 1, SimHashes.CarbonDioxide, SpawnFXHashes.MeteorImpactMetal, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(2, 4);
		component.entityDamage = 15;
		component.totalTileDamage = 0.5f;
		component.affectedByDifficulty = false;
		return gameObject;
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x00060F92 File Offset: 0x0005F192
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000FFE RID: 4094 RVA: 0x00060F94 File Offset: 0x0005F194
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A6B RID: 2667
	public static readonly string ID = "FullereneComet";
}
