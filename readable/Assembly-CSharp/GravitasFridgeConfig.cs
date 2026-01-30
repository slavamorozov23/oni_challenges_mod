using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000250 RID: 592
public class GravitasFridgeConfig : IEntityConfig
{
	// Token: 0x06000C08 RID: 3080 RVA: 0x00048C38 File Offset: 0x00046E38
	public GameObject CreatePrefab()
	{
		string id = "GravitasFridge";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFRIDGE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFRIDGE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_fridge_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, PermittedRotations.Unrotatable, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntryThenNext("story_trait_hijackheadquarters_initial", UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS.SEARCH4, new Action<InfoDialogScreen>(LoreBearerUtil.UnlockNextEmail), true));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x00048CF4 File Offset: 0x00046EF4
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x00048D0B File Offset: 0x00046F0B
	public void OnSpawn(GameObject inst)
	{
	}
}
