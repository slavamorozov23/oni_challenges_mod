using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000342 RID: 834
public class PinkRockConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001140 RID: 4416 RVA: 0x00066700 File Offset: 0x00064900
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06001141 RID: 4417 RVA: 0x00066707 File Offset: 0x00064907
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001142 RID: 4418 RVA: 0x0006670C File Offset: 0x0006490C
	public GameObject CreatePrefab()
	{
		string id = this.ID;
		string name = STRINGS.CREATURES.SPECIES.PINKROCK.NAME;
		string desc = STRINGS.CREATURES.SPECIES.PINKROCK.DESC;
		float mass = 25f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("pinkrock_kanim"), "idle", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Experimental
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 235.15f;
		gameObject.AddOrGet<Carvable>().dropItemPrefabId = "PinkRockCarved";
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		Light2D light2D = gameObject.AddOrGet<Light2D>();
		light2D.overlayColour = LIGHT2D.PINKROCK_COLOR;
		light2D.Color = LIGHT2D.PINKROCK_COLOR;
		light2D.Range = 2f;
		light2D.Angle = 0f;
		light2D.Direction = LIGHT2D.PINKROCK_DIRECTION;
		light2D.Offset = LIGHT2D.PINKROCK_OFFSET;
		light2D.shape = global::LightShape.Circle;
		light2D.drawOverlay = true;
		return gameObject;
	}

	// Token: 0x06001143 RID: 4419 RVA: 0x0006681C File Offset: 0x00064A1C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001144 RID: 4420 RVA: 0x0006681E File Offset: 0x00064A1E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AFC RID: 2812
	public string ID = "PinkRock";
}
