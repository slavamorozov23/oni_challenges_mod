using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200024F RID: 591
public class GravitasFlipPhoneConfig : IEntityConfig
{
	// Token: 0x06000C04 RID: 3076 RVA: 0x00048B84 File Offset: 0x00046D84
	public GameObject CreatePrefab()
	{
		string id = "GravitasFlipPhone";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFLIPPHONE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFLIPPHONE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_flip_phone_kanim"), "on", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Copper, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000C05 RID: 3077 RVA: 0x00048C17 File Offset: 0x00046E17
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000C06 RID: 3078 RVA: 0x00048C2E File Offset: 0x00046E2E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400085A RID: 2138
	public const string ID = "GravitasFlipPhone";
}
