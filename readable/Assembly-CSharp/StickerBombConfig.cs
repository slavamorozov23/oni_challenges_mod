using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200043D RID: 1085
public class StickerBombConfig : IEntityConfig
{
	// Token: 0x06001688 RID: 5768 RVA: 0x000802A8 File Offset: 0x0007E4A8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity("StickerBomb", STRINGS.BUILDINGS.PREFABS.STICKERBOMB.NAME, STRINGS.BUILDINGS.PREFABS.STICKERBOMB.DESC, 1f, true, Assets.GetAnim("sticker_a_kanim"), "off", Grid.SceneLayer.Backwall, SimHashes.Creature, null, 293f);
		EntityTemplates.AddCollision(gameObject, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f);
		gameObject.AddOrGet<StickerBomb>();
		return gameObject;
	}

	// Token: 0x06001689 RID: 5769 RVA: 0x00080312 File Offset: 0x0007E512
	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<OccupyArea>().SetCellOffsets(new CellOffset[1]);
		inst.AddComponent<Modifiers>();
		inst.AddOrGet<DecorProvider>().SetValues(DECOR.BONUS.TIER2);
	}

	// Token: 0x0600168A RID: 5770 RVA: 0x0008033C File Offset: 0x0007E53C
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000D61 RID: 3425
	public const string ID = "StickerBomb";
}
