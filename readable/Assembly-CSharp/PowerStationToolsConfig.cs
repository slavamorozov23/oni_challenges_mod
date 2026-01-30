using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000277 RID: 631
public class PowerStationToolsConfig : IEntityConfig
{
	// Token: 0x06000CD1 RID: 3281 RVA: 0x0004C8C8 File Offset: 0x0004AAC8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PowerStationTools", ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME, ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.DESC, 5f, true, Assets.GetAnim("kit_electrician_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialProduct,
			GameTags.MiscPickupable,
			GameTags.PedestalDisplayable
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x0004C94E File Offset: 0x0004AB4E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x0004C950 File Offset: 0x0004AB50
	public void OnSpawn(GameObject inst)
	{
		PrimaryElement component = inst.GetComponent<PrimaryElement>();
		if (component.MassPerUnit > 1f && Math.Abs(component.Units - Mathf.Floor(component.Units)) > Mathf.Epsilon)
		{
			float mass = Mathf.Ceil(component.Mass / component.MassPerUnit) * component.MassPerUnit;
			component.Mass = mass;
		}
	}

	// Token: 0x040008DE RID: 2270
	public const string ID = "PowerStationTools";

	// Token: 0x040008DF RID: 2271
	public static readonly Tag tag = TagManager.Create("PowerStationTools");

	// Token: 0x040008E0 RID: 2272
	public const float MASS = 5f;
}
