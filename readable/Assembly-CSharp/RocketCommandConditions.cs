using System;

// Token: 0x02000BA0 RID: 2976
public class RocketCommandConditions : CommandConditions
{
	// Token: 0x060058F1 RID: 22769 RVA: 0x002043F0 File Offset: 0x002025F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		RocketModule component = base.GetComponent<RocketModule>();
		this.reachable = (ConditionDestinationReachable)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionDestinationReachable(base.GetComponent<RocketModule>()));
		this.allModulesComplete = (ConditionAllModulesComplete)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionAllModulesComplete(base.GetComponent<ILaunchableRocket>()));
		if (base.GetComponent<ILaunchableRocket>().registerType == LaunchableRocketRegisterType.Spacecraft)
		{
			this.destHasResources = (ConditionHasMinimumMass)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionHasMinimumMass(base.GetComponent<CommandModule>()));
			this.hasAstronaut = (ConditionHasAstronaut)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionHasAstronaut(base.GetComponent<CommandModule>()));
			this.hasSuit = (ConditionHasAtmoSuit)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionHasAtmoSuit(base.GetComponent<CommandModule>()));
			this.cargoEmpty = (CargoBayIsEmpty)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new CargoBayIsEmpty(base.GetComponent<CommandModule>()));
		}
		else if (base.GetComponent<ILaunchableRocket>().registerType == LaunchableRocketRegisterType.Clustercraft)
		{
			this.hasEngine = (ConditionHasEngine)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionHasEngine(base.GetComponent<ILaunchableRocket>()));
			this.hasNosecone = (ConditionHasNosecone)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionHasNosecone(base.GetComponent<LaunchableRocketCluster>()));
			this.hasControlStation = (ConditionHasControlStation)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionHasControlStation(base.GetComponent<RocketModuleCluster>()));
			this.pilotOnBoard = (ConditionPilotOnBoard)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketBoard, new ConditionPilotOnBoard(base.GetComponent<PassengerRocketModule>()));
			this.passengersOnBoard = (ConditionPassengersOnBoard)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketBoard, new ConditionPassengersOnBoard(base.GetComponent<PassengerRocketModule>()));
			this.noExtraPassengers = (ConditionNoExtraPassengers)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketBoard, new ConditionNoExtraPassengers(base.GetComponent<PassengerRocketModule>()));
			this.onLaunchPad = (ConditionOnLaunchPad)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionOnLaunchPad(base.GetComponent<RocketModuleCluster>().CraftInterface));
		}
		int bufferWidth = 1;
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			bufferWidth = 0;
		}
		this.flightPathIsClear = (ConditionFlightPathIsClear)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketFlight, new ConditionFlightPathIsClear(base.gameObject, bufferWidth));
	}

	// Token: 0x04003BA5 RID: 15269
	public ConditionHasAstronaut hasAstronaut;

	// Token: 0x04003BA6 RID: 15270
	public ConditionPilotOnBoard pilotOnBoard;

	// Token: 0x04003BA7 RID: 15271
	public ConditionPassengersOnBoard passengersOnBoard;

	// Token: 0x04003BA8 RID: 15272
	public ConditionNoExtraPassengers noExtraPassengers;

	// Token: 0x04003BA9 RID: 15273
	public ConditionHasAtmoSuit hasSuit;

	// Token: 0x04003BAA RID: 15274
	public ConditionHasControlStation hasControlStation;
}
