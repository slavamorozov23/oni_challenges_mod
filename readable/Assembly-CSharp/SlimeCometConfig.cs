using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000313 RID: 787
public class SlimeCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001028 RID: 4136 RVA: 0x00061AA1 File Offset: 0x0005FCA1
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001029 RID: 4137 RVA: 0x00061AA8 File Offset: 0x0005FCA8
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600102A RID: 4138 RVA: 0x00061AAC File Offset: 0x0005FCAC
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.SlimeMold).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(SlimeCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.SLIMECOMET.NAME, "meteor_slime_kanim", SimHashes.SlimeMold, new Vector2(mass * 0.8f * 2f, mass * 1.2f * 2f), new Vector2(310.15f, 323.15f), "Meteor_slimeball_Impact", 7, SimHashes.ContaminatedOxygen, SpawnFXHashes.MeteorImpactSlime, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.explosionOreCount = new Vector2I(1, 2);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.addTiles = 2;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 2;
		component.diseaseIdx = Db.Get().Diseases.GetIndex("SlimeLung");
		component.addDiseaseCount = (int)(component.EXHAUST_RATE * 100000f);
		return gameObject;
	}

	// Token: 0x0600102B RID: 4139 RVA: 0x00061BAF File Offset: 0x0005FDAF
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600102C RID: 4140 RVA: 0x00061BB4 File Offset: 0x0005FDB4
	public void OnSpawn(GameObject go)
	{
		go.GetComponent<PrimaryElement>().AddDisease(Db.Get().Diseases.GetIndex("SlimeLung"), (int)(UnityEngine.Random.Range(0.9f, 1.2f) * 50f * 100000f), "Meteor");
	}

	// Token: 0x04000A76 RID: 2678
	public static string ID = "SlimeComet";

	// Token: 0x04000A77 RID: 2679
	public const int ADDED_CELLS = 2;

	// Token: 0x04000A78 RID: 2680
	private const SimHashes element = SimHashes.SlimeMold;
}
