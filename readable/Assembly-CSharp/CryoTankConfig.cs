using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000403 RID: 1027
public class CryoTankConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600152B RID: 5419 RVA: 0x00079547 File Offset: 0x00077747
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600152C RID: 5420 RVA: 0x0007954E File Offset: 0x0007774E
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600152D RID: 5421 RVA: 0x00079554 File Offset: 0x00077754
	public GameObject CreatePrefab()
	{
		string id = "CryoTank";
		string name = STRINGS.BUILDINGS.PREFABS.CRYOTANK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.CRYOTANK.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("cryo_chamber_kanim"), "off", Grid.SceneLayer.Building, 2, 3, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.GetComponent<KAnimControllerBase>().SetFGLayer(Grid.SceneLayer.BuildingFront);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		CryoTank cryoTank = gameObject.AddOrGet<CryoTank>();
		cryoTank.overrideAnim = "anim_interacts_cryo_activation_kanim";
		cryoTank.dropOffset = new CellOffset(1, 0);
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("cryotank_warning", UI.USERMENUACTIONS.READLORE.SEARCH_CRYO_TANK, false));
		gameObject.AddOrGet<Demolishable>().allowDemolition = false;
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x0600152E RID: 5422 RVA: 0x0007964C File Offset: 0x0007784C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600152F RID: 5423 RVA: 0x0007964E File Offset: 0x0007784E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000CD2 RID: 3282
	public const string ID = "CryoTank";
}
