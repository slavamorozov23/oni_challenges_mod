using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200031C RID: 796
public class BleachStoneCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001067 RID: 4199 RVA: 0x00062597 File Offset: 0x00060797
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001068 RID: 4200 RVA: 0x0006259E File Offset: 0x0006079E
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001069 RID: 4201 RVA: 0x000625A4 File Offset: 0x000607A4
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.OxyRock).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(BleachStoneCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.BLEACHSTONECOMET.NAME, "meteor_bleachstone_kanim", SimHashes.BleachStone, new Vector2(mass * 0.8f * 1f, mass * 1.2f * 1f), new Vector2(310.15f, 323.15f), "Meteor_dust_heavy_Impact", 1, SimHashes.ChlorineGas, SpawnFXHashes.MeteorImpactIce, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(2, 4);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.addTiles = 1;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 1;
		return gameObject;
	}

	// Token: 0x0600106A RID: 4202 RVA: 0x00062675 File Offset: 0x00060875
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x00062677 File Offset: 0x00060877
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A84 RID: 2692
	public static string ID = "BleachStoneComet";

	// Token: 0x04000A85 RID: 2693
	private const int ADDED_CELLS = 1;
}
