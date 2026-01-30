using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000BBB RID: 3003
public class CargoBayIsEmpty : ProcessCondition
{
	// Token: 0x06005A2D RID: 23085 RVA: 0x0020B9E4 File Offset: 0x00209BE4
	public CargoBayIsEmpty(CommandModule module)
	{
		this.commandModule = module;
	}

	// Token: 0x06005A2E RID: 23086 RVA: 0x0020B9F4 File Offset: 0x00209BF4
	public override ProcessCondition.Status EvaluateCondition()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			CargoBay component = gameObject.GetComponent<CargoBay>();
			if (component != null && component.storage.MassStored() != 0f)
			{
				return ProcessCondition.Status.Failure;
			}
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06005A2F RID: 23087 RVA: 0x0020BA74 File Offset: 0x00209C74
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return UI.STARMAP.CARGOEMPTY.NAME;
	}

	// Token: 0x06005A30 RID: 23088 RVA: 0x0020BA80 File Offset: 0x00209C80
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		return UI.STARMAP.CARGOEMPTY.TOOLTIP;
	}

	// Token: 0x06005A31 RID: 23089 RVA: 0x0020BA8C File Offset: 0x00209C8C
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C55 RID: 15445
	private CommandModule commandModule;
}
