using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000B40 RID: 2880
public class NoFreeRocketInterior : SelectModuleCondition
{
	// Token: 0x060054C6 RID: 21702 RVA: 0x001EEF94 File Offset: 0x001ED194
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		int num = 0;
		using (List<WorldContainer>.Enumerator enumerator = ClusterManager.Instance.WorldContainers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsModuleInterior)
				{
					num++;
				}
			}
		}
		return num < ClusterManager.MAX_ROCKET_INTERIOR_COUNT;
	}

	// Token: 0x060054C7 RID: 21703 RVA: 0x001EEFF8 File Offset: 0x001ED1F8
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.PASSENGER_MODULE_AVAILABLE.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.PASSENGER_MODULE_AVAILABLE.FAILED;
	}
}
