using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000218 RID: 536
public class FoodSplatConfig : IEntityConfig
{
	// Token: 0x06000AD2 RID: 2770 RVA: 0x00041A18 File Offset: 0x0003FC18
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateBasicEntity("FoodSplat", STRINGS.ITEMS.FOOD.FOODSPLAT.NAME, STRINGS.ITEMS.FOOD.FOODSPLAT.DESC, 1f, true, Assets.GetAnim("sticker_a_kanim"), "idle_sticker_a", Grid.SceneLayer.Backwall, SimHashes.Creature, null, 293f);
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x00041A69 File Offset: 0x0003FC69
	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<OccupyArea>().SetCellOffsets(new CellOffset[1]);
		inst.AddComponent<Modifiers>();
		inst.AddOrGet<KSelectable>();
		inst.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER2);
		inst.AddOrGetDef<Splat.Def>();
		inst.AddOrGet<SplatWorkable>();
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x00041AA8 File Offset: 0x0003FCA8
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007A9 RID: 1961
	public const string ID = "FoodSplat";
}
