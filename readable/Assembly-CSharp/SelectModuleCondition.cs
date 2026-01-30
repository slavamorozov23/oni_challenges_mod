using System;
using UnityEngine;

// Token: 0x02000B37 RID: 2871
public abstract class SelectModuleCondition
{
	// Token: 0x060054A8 RID: 21672
	public abstract bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext);

	// Token: 0x060054A9 RID: 21673
	public abstract string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart);

	// Token: 0x060054AA RID: 21674 RVA: 0x001EE7F9 File Offset: 0x001EC9F9
	public virtual bool IgnoreInSanboxMode()
	{
		return false;
	}

	// Token: 0x02001CA2 RID: 7330
	public enum SelectionContext
	{
		// Token: 0x040088AD RID: 34989
		AddModuleAbove,
		// Token: 0x040088AE RID: 34990
		AddModuleBelow,
		// Token: 0x040088AF RID: 34991
		ReplaceModule
	}
}
