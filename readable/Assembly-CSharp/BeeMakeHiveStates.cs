using System;
using UnityEngine;

// Token: 0x020000DC RID: 220
public class BeeMakeHiveStates : GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>
{
	// Token: 0x060003FB RID: 1019 RVA: 0x000218B4 File Offset: 0x0001FAB4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.findBuildLocation;
		this.root.DoNothing();
		this.findBuildLocation.Enter(delegate(BeeMakeHiveStates.Instance smi)
		{
			this.FindBuildLocation(smi);
			if (smi.targetBuildCell != Grid.InvalidCell)
			{
				smi.GoTo(this.moveToBuildLocation);
				return;
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.moveToBuildLocation.MoveTo((BeeMakeHiveStates.Instance smi) => smi.targetBuildCell, this.doBuild, this.behaviourcomplete, false);
		this.doBuild.PlayAnim("hive_grow_pre").EventHandler(GameHashes.AnimQueueComplete, delegate(BeeMakeHiveStates.Instance smi)
		{
			if (smi.gameObject.GetComponent<Bee>().FindHiveInRoom() == null)
			{
				smi.builtHome = true;
				smi.BuildHome();
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToMakeHome, false).Exit(delegate(BeeMakeHiveStates.Instance smi)
		{
			if (smi.builtHome)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			}
		});
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x00021984 File Offset: 0x0001FB84
	private void FindBuildLocation(BeeMakeHiveStates.Instance smi)
	{
		smi.targetBuildCell = Grid.InvalidCell;
		GameObject prefab = Assets.GetPrefab("BeeHive".ToTag());
		BuildingPlacementQuery buildingPlacementQuery = PathFinderQueries.buildingPlacementQuery.Reset(1, prefab);
		smi.GetComponent<Navigator>().RunQuery(buildingPlacementQuery);
		if (buildingPlacementQuery.result_cells.Count > 0)
		{
			smi.targetBuildCell = buildingPlacementQuery.result_cells[UnityEngine.Random.Range(0, buildingPlacementQuery.result_cells.Count)];
		}
	}

	// Token: 0x04000303 RID: 771
	public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State findBuildLocation;

	// Token: 0x04000304 RID: 772
	public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State moveToBuildLocation;

	// Token: 0x04000305 RID: 773
	public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State doBuild;

	// Token: 0x04000306 RID: 774
	public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State behaviourcomplete;

	// Token: 0x020010E8 RID: 4328
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020010E9 RID: 4329
	public new class Instance : GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.GameInstance
	{
		// Token: 0x06008341 RID: 33601 RVA: 0x0034328A File Offset: 0x0034148A
		public Instance(Chore<BeeMakeHiveStates.Instance> chore, BeeMakeHiveStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToMakeHome);
		}

		// Token: 0x06008342 RID: 33602 RVA: 0x003432B0 File Offset: 0x003414B0
		public void BuildHome()
		{
			Vector3 position = Grid.CellToPos(this.targetBuildCell, CellAlignment.Bottom, Grid.SceneLayer.Creatures);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("BeeHive".ToTag()), position, Quaternion.identity, null, null, true, 0);
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.ElementID = SimHashes.Creature;
			component.Temperature = base.gameObject.GetComponent<PrimaryElement>().Temperature;
			gameObject.SetActive(true);
			gameObject.GetSMI<BeeHive.StatesInstance>().SetUpNewHive();
		}

		// Token: 0x0400638D RID: 25485
		public int targetBuildCell;

		// Token: 0x0400638E RID: 25486
		public bool builtHome;
	}
}
