using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A3 RID: 931
public class PropClothesHanger : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600133C RID: 4924 RVA: 0x0006F650 File Offset: 0x0006D850
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x0006F657 File Offset: 0x0006D857
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x0006F65C File Offset: 0x0006D85C
	public GameObject CreatePrefab()
	{
		string id = "PropClothesHanger";
		string name = STRINGS.BUILDINGS.PREFABS.PROPCLOTHESHANGER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPCLOTHESHANGER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("unlock_clothing_kanim"), "on", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas,
			GameTags.RoomProberBuilding
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Cinnabar, true);
		component.Temperature = 294.15f;
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		SetLocker setLocker = gameObject.AddOrGet<SetLocker>();
		setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
		setLocker.dropOffset = new Vector2I(0, 1);
		setLocker.dropOnDeconstruct = true;
		gameObject.AddOrGet<Deconstructable>().audioSize = "small";
		return gameObject;
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x0006F754 File Offset: 0x0006D954
	public void OnPrefabInit(GameObject inst)
	{
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = new string[][]
		{
			new string[]
			{
				"Warm_Vest"
			}
		};
		component.ChooseContents();
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x0006F78B File Offset: 0x0006D98B
	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<Deconstructable>().SetWorkTime(5f);
	}
}
