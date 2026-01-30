using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000239 RID: 569
public class GeneShufflerConfig : IEntityConfig
{
	// Token: 0x06000B7E RID: 2942 RVA: 0x00044DDC File Offset: 0x00042FDC
	public GameObject CreatePrefab()
	{
		string id = "GeneShuffler";
		string name = STRINGS.BUILDINGS.PREFABS.GENESHUFFLER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.GENESHUFFLER.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("geneshuffler_kanim"), "on", Grid.SceneLayer.Building, 4, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		gameObject.AddTag(GameTags.NotRoomAssignable);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<Notifier>();
		gameObject.AddOrGet<GeneShuffler>();
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.NerualVacillator));
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<Ownable>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Demolishable>();
		Storage storage = gameObject.AddOrGet<Storage>();
		storage.dropOnLoad = true;
		ManualDeliveryKG manualDeliveryKG = gameObject.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.RequestedItemTag = new Tag("GeneShufflerRecharge");
		manualDeliveryKG.refillMass = 1f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.capacity = 1f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
		kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
		return gameObject;
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x00044F30 File Offset: 0x00043130
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<GeneShuffler>().workLayer = Grid.SceneLayer.Building;
		inst.GetComponent<Ownable>().slotID = Db.Get().AssignableSlots.GeneShuffler.Id;
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		inst.GetComponent<Deconstructable>();
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x00044F85 File Offset: 0x00043185
	public void OnSpawn(GameObject inst)
	{
	}
}
