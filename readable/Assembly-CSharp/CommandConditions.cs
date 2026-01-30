using System;

// Token: 0x02000B9F RID: 2975
public class CommandConditions : KMonoBehaviour
{
	// Token: 0x04003B9D RID: 15261
	public ConditionDestinationReachable reachable;

	// Token: 0x04003B9E RID: 15262
	public CargoBayIsEmpty cargoEmpty;

	// Token: 0x04003B9F RID: 15263
	public ConditionHasMinimumMass destHasResources;

	// Token: 0x04003BA0 RID: 15264
	public ConditionAllModulesComplete allModulesComplete;

	// Token: 0x04003BA1 RID: 15265
	public ConditionHasEngine hasEngine;

	// Token: 0x04003BA2 RID: 15266
	public ConditionHasNosecone hasNosecone;

	// Token: 0x04003BA3 RID: 15267
	public ConditionOnLaunchPad onLaunchPad;

	// Token: 0x04003BA4 RID: 15268
	public ConditionFlightPathIsClear flightPathIsClear;
}
