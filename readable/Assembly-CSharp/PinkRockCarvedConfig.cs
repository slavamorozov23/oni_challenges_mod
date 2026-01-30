using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000341 RID: 833
public class PinkRockCarvedConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600113A RID: 4410 RVA: 0x000665A9 File Offset: 0x000647A9
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x0600113B RID: 4411 RVA: 0x000665B0 File Offset: 0x000647B0
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600113C RID: 4412 RVA: 0x000665B4 File Offset: 0x000647B4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PinkRockCarved", STRINGS.CREATURES.SPECIES.PINKROCKCARVED.NAME, STRINGS.CREATURES.SPECIES.PINKROCKCARVED.DESC, 1f, true, Assets.GetAnim("pinkrock_decor_kanim"), "idle", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.CIRCLE, 0.5f, 0.5f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.RareMaterials,
			GameTags.MiscPickupable,
			GameTags.PedestalDisplayable,
			GameTags.Experimental,
			GameTags.Ornament
		});
		gameObject.AddOrGet<OccupyArea>();
		DecorProvider decorProvider = gameObject.AddOrGet<DecorProvider>();
		decorProvider.SetValues(TUNING.BUILDINGS.DECOR.BONUS.TIER1);
		decorProvider.overrideName = gameObject.GetProperName();
		Light2D light2D = gameObject.AddOrGet<Light2D>();
		light2D.overlayColour = LIGHT2D.PINKROCK_COLOR;
		light2D.Color = LIGHT2D.PINKROCK_COLOR;
		light2D.Range = 3f;
		light2D.Angle = 0f;
		light2D.Direction = LIGHT2D.PINKROCK_DIRECTION;
		light2D.Offset = LIGHT2D.PINKROCK_OFFSET;
		light2D.shape = global::LightShape.Circle;
		light2D.drawOverlay = true;
		light2D.disableOnStore = true;
		gameObject.GetComponent<KCircleCollider2D>().offset = new Vector2(0f, 0.25f);
		gameObject.AddOrGet<CodexEntryRedirector>().CodexID = "REQUIREMENTCLASSORNAMENT";
		return gameObject;
	}

	// Token: 0x0600113D RID: 4413 RVA: 0x000666F4 File Offset: 0x000648F4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600113E RID: 4414 RVA: 0x000666F6 File Offset: 0x000648F6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AFB RID: 2811
	public const string ID = "PinkRockCarved";
}
