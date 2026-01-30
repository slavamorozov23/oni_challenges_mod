using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003B7 RID: 951
public class PropGravitasCreaturePosterConfig : IEntityConfig
{
	// Token: 0x06001394 RID: 5012 RVA: 0x00070A18 File Offset: 0x0006EC18
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasCreaturePoster";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCREATUREPOSTER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCREATUREPOSTER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_poster_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("storytrait_crittermanipulator_workiversary", UI.USERMENUACTIONS.READLORE.SEARCH_PROPGRAVITASCREATUREPOSTER, false));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001395 RID: 5013 RVA: 0x00070AC6 File Offset: 0x0006ECC6
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001396 RID: 5014 RVA: 0x00070ADD File Offset: 0x0006ECDD
	public void OnSpawn(GameObject inst)
	{
	}
}
