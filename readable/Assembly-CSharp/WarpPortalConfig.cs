using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200045E RID: 1118
public class WarpPortalConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600175F RID: 5983 RVA: 0x00084DD8 File Offset: 0x00082FD8
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001760 RID: 5984 RVA: 0x00084DDF File Offset: 0x00082FDF
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001761 RID: 5985 RVA: 0x00084DE4 File Offset: 0x00082FE4
	public GameObject CreatePrefab()
	{
		string id = "WarpPortal";
		string name = STRINGS.BUILDINGS.PREFABS.WARPPORTAL.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.WARPPORTAL.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("warp_portal_sender_kanim"), "idle", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.AddTag(GameTags.NotRoomAssignable);
		gameObject.AddTag(GameTags.WarpTech);
		gameObject.AddTag(GameTags.Gravitas);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<Notifier>();
		gameObject.AddOrGet<WarpPortal>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<Ownable>().tintWhenUnassigned = false;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("notes_teleportation", UI.USERMENUACTIONS.READLORE.SEARCH_TELEPORTER_SENDER, false));
		gameObject.AddOrGet<Prioritizable>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
		kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
		return gameObject;
	}

	// Token: 0x06001762 RID: 5986 RVA: 0x00084EE4 File Offset: 0x000830E4
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<WarpPortal>().workLayer = Grid.SceneLayer.Building;
		inst.GetComponent<Ownable>().slotID = Db.Get().AssignableSlots.WarpPortal.Id;
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		inst.GetComponent<Deconstructable>();
	}

	// Token: 0x06001763 RID: 5987 RVA: 0x00084F39 File Offset: 0x00083139
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000DC4 RID: 3524
	public const string ID = "WarpPortal";
}
