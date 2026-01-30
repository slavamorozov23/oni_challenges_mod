using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A5 RID: 933
public class PropDlc2Display1Config : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001346 RID: 4934 RVA: 0x0006F870 File Offset: 0x0006DA70
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06001347 RID: 4935 RVA: 0x0006F877 File Offset: 0x0006DA77
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001348 RID: 4936 RVA: 0x0006F87C File Offset: 0x0006DA7C
	public GameObject CreatePrefab()
	{
		string id = "PropDlc2Display1";
		string name = STRINGS.BUILDINGS.PREFABS.PROPDLC2DISPLAY1.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPDLC2DISPLAY1.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_display_showroom_kanim"), "off", Grid.SceneLayer.Building, 1, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextEmail));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001349 RID: 4937 RVA: 0x0006F921 File Offset: 0x0006DB21
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x0600134A RID: 4938 RVA: 0x0006F938 File Offset: 0x0006DB38
	public void OnSpawn(GameObject inst)
	{
	}
}
