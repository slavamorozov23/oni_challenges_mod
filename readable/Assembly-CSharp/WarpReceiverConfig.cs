using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200045F RID: 1119
public class WarpReceiverConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001765 RID: 5989 RVA: 0x00084F43 File Offset: 0x00083143
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001766 RID: 5990 RVA: 0x00084F4A File Offset: 0x0008314A
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001767 RID: 5991 RVA: 0x00084F50 File Offset: 0x00083150
	public GameObject CreatePrefab()
	{
		string id = WarpReceiverConfig.ID;
		string name = STRINGS.BUILDINGS.PREFABS.WARPRECEIVER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.WARPRECEIVER.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("warp_portal_receiver_kanim"), "idle", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.AddTag(GameTags.NotRoomAssignable);
		gameObject.AddTag(GameTags.WarpTech);
		gameObject.AddTag(GameTags.Gravitas);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<Notifier>();
		gameObject.AddOrGet<WarpReceiver>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<Prioritizable>();
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("notes_AI", UI.USERMENUACTIONS.READLORE.SEARCH_TELEPORTER_RECEIVER, false));
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
		kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
		return gameObject;
	}

	// Token: 0x06001768 RID: 5992 RVA: 0x00085041 File Offset: 0x00083241
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<WarpReceiver>().workLayer = Grid.SceneLayer.Building;
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		inst.GetComponent<Deconstructable>();
	}

	// Token: 0x06001769 RID: 5993 RVA: 0x0008506C File Offset: 0x0008326C
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000DC5 RID: 3525
	public static string ID = "WarpReceiver";
}
