using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000314 RID: 788
public class SnowballCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600102F RID: 4143 RVA: 0x00061C1A File Offset: 0x0005FE1A
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x00061C21 File Offset: 0x0005FE21
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001031 RID: 4145 RVA: 0x00061C24 File Offset: 0x0005FE24
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(SnowballCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.SNOWBALLCOMET.NAME, "meteor_snow_kanim", SimHashes.Snow, new Vector2(3f, 20f), new Vector2(253.15f, 263.15f), "Meteor_snowball_Impact", 5, SimHashes.Void, SpawnFXHashes.None, 0.3f);
		Comet component = gameObject.GetComponent<Comet>();
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.splashRadius = 1;
		component.addTiles = 3;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 2;
		return gameObject;
	}

	// Token: 0x06001032 RID: 4146 RVA: 0x00061CB1 File Offset: 0x0005FEB1
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001033 RID: 4147 RVA: 0x00061CB3 File Offset: 0x0005FEB3
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A79 RID: 2681
	public static string ID = "SnowballComet";
}
