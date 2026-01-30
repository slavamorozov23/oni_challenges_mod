using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003C2 RID: 962
public class PropGravitasJar2Config : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060013CC RID: 5068 RVA: 0x0007133E File Offset: 0x0006F53E
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060013CD RID: 5069 RVA: 0x00071345 File Offset: 0x0006F545
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060013CE RID: 5070 RVA: 0x00071348 File Offset: 0x0006F548
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasJar2";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASJAR2.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASJAR2.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_jar2_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextDimensionalLore));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060013CF RID: 5071 RVA: 0x000713ED File Offset: 0x0006F5ED
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060013D0 RID: 5072 RVA: 0x00071404 File Offset: 0x0006F604
	public void OnSpawn(GameObject inst)
	{
	}
}
