using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000B41 RID: 2881
public class LimitOneRoboPilotModule : SelectModuleCondition
{
	// Token: 0x060054C9 RID: 21705 RVA: 0x001EF01C File Offset: 0x001ED21C
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		if (existingModule == null)
		{
			return true;
		}
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(existingModule.GetComponent<AttachableBuilding>()))
		{
			if (selectionContext != SelectModuleCondition.SelectionContext.ReplaceModule || !(gameObject == existingModule.gameObject))
			{
				if (gameObject.GetComponent<RoboPilotModule>() != null)
				{
					return false;
				}
				if (gameObject.GetComponent<BuildingUnderConstruction>() != null && gameObject.GetComponent<BuildingUnderConstruction>().Def.BuildingComplete.GetComponent<RoboPilotModule>() != null)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060054CA RID: 21706 RVA: 0x001EF0D0 File Offset: 0x001ED2D0
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ONE_ROBOPILOT_PER_ROCKET.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ONE_ROBOPILOT_PER_ROCKET.FAILED;
	}
}
