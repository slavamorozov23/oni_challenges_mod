using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003BA RID: 954
public class PropGravitasDeskPodiumDLC4Config : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060013A2 RID: 5026 RVA: 0x00070C6E File Offset: 0x0006EE6E
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x00070C75 File Offset: 0x0006EE75
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060013A4 RID: 5028 RVA: 0x00070C78 File Offset: 0x0006EE78
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasDeskPodiumDLC4";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDESKPODIUM.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDESKPODIUM.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_desk_podium_kanim"), "off", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new string[]
		{
			"dlc4surfacepoi"
		});
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060013A5 RID: 5029 RVA: 0x00070D21 File Offset: 0x0006EF21
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060013A6 RID: 5030 RVA: 0x00070D38 File Offset: 0x0006EF38
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000C17 RID: 3095
	public static string ID = "PropGravitasDeskPodiumDLC4";
}
