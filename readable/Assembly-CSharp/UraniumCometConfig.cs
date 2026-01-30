using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000312 RID: 786
public class UraniumCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001021 RID: 4129 RVA: 0x000619C2 File Offset: 0x0005FBC2
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001022 RID: 4130 RVA: 0x000619C9 File Offset: 0x0005FBC9
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001023 RID: 4131 RVA: 0x000619CC File Offset: 0x0005FBCC
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.UraniumOre).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(UraniumCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.URANIUMORECOMET.NAME, "meteor_uranium_kanim", SimHashes.UraniumOre, new Vector2(mass * 0.8f * 6f, mass * 1.2f * 6f), new Vector2(323.15f, 403.15f), "Meteor_Nuclear_Impact", 3, SimHashes.CarbonDioxide, SpawnFXHashes.MeteorImpactUranium, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(1, 2);
		component.entityDamage = 15;
		component.totalTileDamage = 0f;
		component.addTiles = 6;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 1;
		return gameObject;
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x00061A89 File Offset: 0x0005FC89
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001025 RID: 4133 RVA: 0x00061A8B File Offset: 0x0005FC8B
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A73 RID: 2675
	public static readonly string ID = "UraniumComet";

	// Token: 0x04000A74 RID: 2676
	private const SimHashes element = SimHashes.UraniumOre;

	// Token: 0x04000A75 RID: 2677
	private const int ADDED_CELLS = 6;
}
