using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003B6 RID: 950
public class PropGravitasCeilingRobotConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600138E RID: 5006 RVA: 0x00070958 File Offset: 0x0006EB58
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600138F RID: 5007 RVA: 0x0007095F File Offset: 0x0006EB5F
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001390 RID: 5008 RVA: 0x00070964 File Offset: 0x0006EB64
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasCeilingRobot";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCEILINGROBOT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCEILINGROBOT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_ceiling_robot_kanim"), "off", Grid.SceneLayer.Building, 2, 4, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001391 RID: 5009 RVA: 0x000709F7 File Offset: 0x0006EBF7
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001392 RID: 5010 RVA: 0x00070A0E File Offset: 0x0006EC0E
	public void OnSpawn(GameObject inst)
	{
	}
}
