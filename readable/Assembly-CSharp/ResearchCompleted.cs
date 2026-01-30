using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000B38 RID: 2872
public class ResearchCompleted : SelectModuleCondition
{
	// Token: 0x060054AC RID: 21676 RVA: 0x001EE804 File Offset: 0x001ECA04
	public override bool IgnoreInSanboxMode()
	{
		return true;
	}

	// Token: 0x060054AD RID: 21677 RVA: 0x001EE808 File Offset: 0x001ECA08
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		if (existingModule == null)
		{
			return true;
		}
		TechItem techItem = Db.Get().TechItems.TryGet(selectedPart.PrefabID);
		return DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || techItem == null || techItem.IsComplete();
	}

	// Token: 0x060054AE RID: 21678 RVA: 0x001EE854 File Offset: 0x001ECA54
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.RESEARCHED.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.RESEARCHED.FAILED;
	}
}
