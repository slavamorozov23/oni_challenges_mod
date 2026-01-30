using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003C1 RID: 961
public class PropGravitasJar1Config : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060013C6 RID: 5062 RVA: 0x0007126C File Offset: 0x0006F46C
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060013C7 RID: 5063 RVA: 0x00071273 File Offset: 0x0006F473
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060013C8 RID: 5064 RVA: 0x00071278 File Offset: 0x0006F478
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasJar1";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASJAR1.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASJAR1.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_jar1_kanim"), "off", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextDimensionalLore));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060013C9 RID: 5065 RVA: 0x0007131D File Offset: 0x0006F51D
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x00071334 File Offset: 0x0006F534
	public void OnSpawn(GameObject inst)
	{
	}
}
