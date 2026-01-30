using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200031A RID: 794
public class PhosphoricCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001059 RID: 4185 RVA: 0x00062407 File Offset: 0x00060607
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x0006240E File Offset: 0x0006060E
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x00062414 File Offset: 0x00060614
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(PhosphoricCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.PHOSPHORICCOMET.NAME, "meteor_phosphoric_kanim", SimHashes.Phosphorite, new Vector2(3f, 20f), new Vector2(310.15f, 323.15f), "Meteor_dust_heavy_Impact", 0, SimHashes.Void, SpawnFXHashes.MeteorImpactPhosphoric, 0.3f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(1, 2);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		return gameObject;
	}

	// Token: 0x0600105C RID: 4188 RVA: 0x000624AB File Offset: 0x000606AB
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600105D RID: 4189 RVA: 0x000624AD File Offset: 0x000606AD
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A81 RID: 2689
	public static string ID = "PhosphoricComet";
}
