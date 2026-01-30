using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000349 RID: 841
public class StarmapHexCellInventoryConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001176 RID: 4470 RVA: 0x0006716E File Offset: 0x0006536E
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001177 RID: 4471 RVA: 0x00067175 File Offset: 0x00065375
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001178 RID: 4472 RVA: 0x00067178 File Offset: 0x00065378
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("StarmapHexCellInventory", UI.CLUSTERMAP.HEXCELL_INVENTORY.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<StarmapHexCellInventory>();
		gameObject.AddOrGet<StarmapHexCellInventoryVisuals>();
		gameObject.AddOrGet<InfoDescription>().description = UI.CLUSTERMAP.HEXCELL_INVENTORY.DESC;
		return gameObject;
	}

	// Token: 0x06001179 RID: 4473 RVA: 0x000671C4 File Offset: 0x000653C4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600117A RID: 4474 RVA: 0x000671C6 File Offset: 0x000653C6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000B07 RID: 2823
	public const string ID = "StarmapHexCellInventory";
}
