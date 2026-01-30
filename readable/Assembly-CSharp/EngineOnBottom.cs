using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000B3C RID: 2876
public class EngineOnBottom : SelectModuleCondition
{
	// Token: 0x060054BA RID: 21690 RVA: 0x001EEAF8 File Offset: 0x001ECCF8
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		if (existingModule == null || existingModule.GetComponent<LaunchPad>() != null)
		{
			return true;
		}
		if (selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule)
		{
			return existingModule.GetComponent<AttachableBuilding>().GetAttachedTo() == null;
		}
		return selectionContext == SelectModuleCondition.SelectionContext.AddModuleBelow && existingModule.GetComponent<AttachableBuilding>().GetAttachedTo() == null;
	}

	// Token: 0x060054BB RID: 21691 RVA: 0x001EEB55 File Offset: 0x001ECD55
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ENGINE_AT_BOTTOM.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ENGINE_AT_BOTTOM.FAILED;
	}
}
