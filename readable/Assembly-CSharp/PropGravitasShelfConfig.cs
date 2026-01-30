using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003C8 RID: 968
public class PropGravitasShelfConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060013EB RID: 5099 RVA: 0x000718B6 File Offset: 0x0006FAB6
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060013EC RID: 5100 RVA: 0x000718BD File Offset: 0x0006FABD
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060013ED RID: 5101 RVA: 0x000718C0 File Offset: 0x0006FAC0
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasShelf";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASSHELF.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASSHELF.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_shelf_kanim"), "off", Grid.SceneLayer.Building, 2, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060013EE RID: 5102 RVA: 0x00071953 File Offset: 0x0006FB53
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060013EF RID: 5103 RVA: 0x0007196A File Offset: 0x0006FB6A
	public void OnSpawn(GameObject inst)
	{
	}
}
