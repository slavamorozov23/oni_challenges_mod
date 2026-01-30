using System;
using STRINGS;

// Token: 0x02000E52 RID: 3666
public interface IEmptyableCargo
{
	// Token: 0x06007437 RID: 29751
	bool CanEmptyCargo();

	// Token: 0x06007438 RID: 29752
	void EmptyCargo();

	// Token: 0x170007FC RID: 2044
	// (get) Token: 0x06007439 RID: 29753
	IStateMachineTarget master { get; }

	// Token: 0x170007FD RID: 2045
	// (get) Token: 0x0600743A RID: 29754
	bool CanAutoDeploy { get; }

	// Token: 0x170007FE RID: 2046
	// (get) Token: 0x0600743B RID: 29755
	// (set) Token: 0x0600743C RID: 29756
	bool AutoDeploy { get; set; }

	// Token: 0x170007FF RID: 2047
	// (get) Token: 0x0600743D RID: 29757
	bool ChooseDuplicant { get; }

	// Token: 0x17000800 RID: 2048
	// (get) Token: 0x0600743E RID: 29758
	bool ModuleDeployed { get; }

	// Token: 0x17000801 RID: 2049
	// (get) Token: 0x0600743F RID: 29759
	// (set) Token: 0x06007440 RID: 29760
	MinionIdentity ChosenDuplicant { get; set; }

	// Token: 0x17000802 RID: 2050
	// (get) Token: 0x06007441 RID: 29761 RVA: 0x002C60B6 File Offset: 0x002C42B6
	bool CanTargetClusterGridEntities
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000803 RID: 2051
	// (get) Token: 0x06007442 RID: 29762 RVA: 0x002C60B9 File Offset: 0x002C42B9
	string GetButtonText
	{
		get
		{
			return UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.DEPLOY_BUTTON;
		}
	}

	// Token: 0x17000804 RID: 2052
	// (get) Token: 0x06007443 RID: 29763 RVA: 0x002C60C5 File Offset: 0x002C42C5
	string GetButtonToolip
	{
		get
		{
			return UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.DEPLOY_BUTTON_TOOLTIP;
		}
	}
}
