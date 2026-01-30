using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200031F RID: 799
public class DisposableElectrobankConfig : IMultiEntityConfig
{
	// Token: 0x0600107A RID: 4218 RVA: 0x00062724 File Offset: 0x00060924
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		if (!DlcManager.IsContentSubscribed("DLC3_ID"))
		{
			return list;
		}
		list.Add(this.CreateDisposableElectrobank("DisposableElectrobank_RawMetal", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_METAL_ORE.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_METAL_ORE.DESC, 20f, SimHashes.Cuprite, "electrobank_popcan_kanim", DlcManager.DLC3, null, "object"));
		if (DlcManager.IsExpansion1Active())
		{
			GameObject gameObject = this.CreateDisposableElectrobank("DisposableElectrobank_UraniumOre", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_URANIUM_ORE.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_URANIUM_ORE.DESC, 10f, SimHashes.UraniumOre, "electrobank_uranium_kanim", DlcManager.EXPANSION1.Append(DlcManager.DLC3), null, "object");
			RadiationEmitter radiationEmitter = gameObject.AddOrGet<RadiationEmitter>();
			radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
			radiationEmitter.radiusProportionalToRads = false;
			radiationEmitter.emitRadiusX = 5;
			radiationEmitter.emitRadiusY = radiationEmitter.emitRadiusX;
			radiationEmitter.emitRads = 60f;
			radiationEmitter.emissionOffset = new Vector3(0f, 0f, 0f);
			list.Add(gameObject);
			gameObject.GetComponent<Electrobank>().radioactivityTuning = radiationEmitter.emitRads;
		}
		list.RemoveAll((GameObject t) => t == null);
		return list;
	}

	// Token: 0x0600107B RID: 4219 RVA: 0x0006284C File Offset: 0x00060A4C
	private GameObject CreateDisposableElectrobank(string id, LocString name, LocString description, float mass, SimHashes element, string animName, string[] requiredDlcIDs = null, string[] forbiddenDlcIds = null, string initialAnim = "object")
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(id, name, description, mass, true, Assets.GetAnim(animName), initialAnim, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.ChargedPortableBattery,
			GameTags.PedestalDisplayable,
			GameTags.DisposablePortableBattery
		});
		if (!Assets.IsTagCountable(GameTags.ChargedPortableBattery))
		{
			Assets.AddCountableTag(GameTags.ChargedPortableBattery);
		}
		gameObject.AddComponent<Electrobank>();
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.requiredDlcIds = requiredDlcIDs;
		component.forbiddenDlcIds = forbiddenDlcIds;
		return gameObject;
	}

	// Token: 0x0600107C RID: 4220 RVA: 0x0006290A File Offset: 0x00060B0A
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600107D RID: 4221 RVA: 0x0006290C File Offset: 0x00060B0C
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000A88 RID: 2696
	public const string ID = "DisposableElectrobank_";

	// Token: 0x04000A89 RID: 2697
	public const float MASS = 20f;

	// Token: 0x04000A8A RID: 2698
	public static Dictionary<Tag, ComplexRecipe> recipes = new Dictionary<Tag, ComplexRecipe>();

	// Token: 0x04000A8B RID: 2699
	public const string ID_METAL_ORE = "DisposableElectrobank_RawMetal";

	// Token: 0x04000A8C RID: 2700
	public const string ID_URANIUM_ORE = "DisposableElectrobank_UraniumOre";
}
