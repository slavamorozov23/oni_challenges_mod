using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200031B RID: 795
public class OxyliteCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001060 RID: 4192 RVA: 0x000624C3 File Offset: 0x000606C3
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001061 RID: 4193 RVA: 0x000624CA File Offset: 0x000606CA
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001062 RID: 4194 RVA: 0x000624D0 File Offset: 0x000606D0
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.OxyRock).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(OxyliteCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.OXYLITECOMET.NAME, "meteor_oxylite_kanim", SimHashes.OxyRock, new Vector2(mass * 0.8f * 6f, mass * 1.2f * 6f), new Vector2(310.15f, 323.15f), "Meteor_dust_heavy_Impact", 0, SimHashes.Oxygen, SpawnFXHashes.MeteorImpactIce, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.addTiles = 6;
		component.addTilesMinHeight = 2;
		component.addTilesMaxHeight = 8;
		return gameObject;
	}

	// Token: 0x06001063 RID: 4195 RVA: 0x0006257F File Offset: 0x0006077F
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001064 RID: 4196 RVA: 0x00062581 File Offset: 0x00060781
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A82 RID: 2690
	public static string ID = "OxyliteComet";

	// Token: 0x04000A83 RID: 2691
	private const int ADDED_CELLS = 6;
}
