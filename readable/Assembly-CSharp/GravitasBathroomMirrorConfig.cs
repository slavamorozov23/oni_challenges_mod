using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000245 RID: 581
public class GravitasBathroomMirrorConfig : IEntityConfig
{
	// Token: 0x06000BC7 RID: 3015 RVA: 0x00047DEC File Offset: 0x00045FEC
	public GameObject CreatePrefab()
	{
		string id = "GravitasBathroomMirror";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASBATHROOMMIRROR.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASBATHROOMMIRROR.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_bathroom_mirror_kanim"), "on", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Glass, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntryThenNext("story_trait_hijackheadquarters_mirror", UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS.SEARCH6, new Action<InfoDialogScreen>(LoreBearerUtil.UnlockNextJournalEntry), true));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x00047EA6 File Offset: 0x000460A6
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x00047EBD File Offset: 0x000460BD
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000849 RID: 2121
	public const string ID = "GravitasBathroomMirror";
}
