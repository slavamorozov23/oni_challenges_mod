using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003CA RID: 970
public class PropGravitasToolShelfConfig : IEntityConfig
{
	// Token: 0x060013F5 RID: 5109 RVA: 0x00071A28 File Offset: 0x0006FC28
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasToolShelf";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASTOOLSHELF.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASTOOLSHELF.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_toolshelf_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060013F6 RID: 5110 RVA: 0x00071ABB File Offset: 0x0006FCBB
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060013F7 RID: 5111 RVA: 0x00071AD2 File Offset: 0x0006FCD2
	public void OnSpawn(GameObject inst)
	{
	}
}
