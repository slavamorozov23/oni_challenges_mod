using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000DE RID: 222
public class BeeSleepStates : GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>
{
	// Token: 0x06000404 RID: 1028 RVA: 0x00021B64 File Offset: 0x0001FD64
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.findSleepLocation;
		GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.SLEEPING.NAME;
		string tooltip = CREATURES.STATUSITEMS.SLEEPING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.findSleepLocation.Enter(delegate(BeeSleepStates.Instance smi)
		{
			BeeSleepStates.FindSleepLocation(smi);
			if (smi.targetSleepCell != Grid.InvalidCell)
			{
				smi.GoTo(this.moveToSleepLocation);
				return;
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.moveToSleepLocation.MoveTo((BeeSleepStates.Instance smi) => smi.targetSleepCell, this.sleep.pre, this.behaviourcomplete, false);
		this.sleep.Enter("EnableGravity", delegate(BeeSleepStates.Instance smi)
		{
			GameComps.Gravities.Add(smi.gameObject, Vector2.zero, delegate(Transform transform)
			{
				if (GameComps.Gravities.Has(smi.gameObject))
				{
					GameComps.Gravities.Remove(smi.gameObject);
				}
			});
		}).TriggerOnEnter(GameHashes.SleepStarted, null).TriggerOnExit(GameHashes.SleepFinished, null).Transition(this.sleep.pst, new StateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.Transition.ConditionCallback(BeeSleepStates.ShouldWakeUp), UpdateRate.SIM_1000ms);
		this.sleep.pre.QueueAnim("sleep_pre", false, null).OnAnimQueueComplete(this.sleep.loop);
		this.sleep.loop.Enter(delegate(BeeSleepStates.Instance smi)
		{
			smi.GetComponent<LoopingSounds>().PauseSound(GlobalAssets.GetSound("Bee_wings_LP", false), true);
		}).QueueAnim("sleep_loop", true, null).Exit(delegate(BeeSleepStates.Instance smi)
		{
			smi.GetComponent<LoopingSounds>().PauseSound(GlobalAssets.GetSound("Bee_wings_LP", false), false);
		});
		this.sleep.pst.QueueAnim("sleep_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.BeeWantsToSleep, false);
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x00021D38 File Offset: 0x0001FF38
	private static void FindSleepLocation(BeeSleepStates.Instance smi)
	{
		smi.targetSleepCell = Grid.InvalidCell;
		FloorCellQuery floorCellQuery = PathFinderQueries.floorCellQuery.Reset(1, 0);
		smi.GetComponent<Navigator>().RunQuery(floorCellQuery);
		if (floorCellQuery.result_cells.Count > 0)
		{
			smi.targetSleepCell = floorCellQuery.result_cells[UnityEngine.Random.Range(0, floorCellQuery.result_cells.Count)];
		}
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x00021D99 File Offset: 0x0001FF99
	public static bool ShouldWakeUp(BeeSleepStates.Instance smi)
	{
		return smi.GetSMI<BeeSleepMonitor.Instance>().CO2Exposure <= 0f;
	}

	// Token: 0x04000309 RID: 777
	public BeeSleepStates.SleepStates sleep;

	// Token: 0x0400030A RID: 778
	public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State findSleepLocation;

	// Token: 0x0400030B RID: 779
	public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State moveToSleepLocation;

	// Token: 0x0400030C RID: 780
	public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State behaviourcomplete;

	// Token: 0x020010EE RID: 4334
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020010EF RID: 4335
	public new class Instance : GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.GameInstance
	{
		// Token: 0x06008350 RID: 33616 RVA: 0x003433AE File Offset: 0x003415AE
		public Instance(Chore<BeeSleepStates.Instance> chore, BeeSleepStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.BeeWantsToSleep);
		}

		// Token: 0x04006397 RID: 25495
		public int targetSleepCell;

		// Token: 0x04006398 RID: 25496
		public float co2Exposure;
	}

	// Token: 0x020010F0 RID: 4336
	public class SleepStates : GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State
	{
		// Token: 0x04006399 RID: 25497
		public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State pre;

		// Token: 0x0400639A RID: 25498
		public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State loop;

		// Token: 0x0400639B RID: 25499
		public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State pst;
	}
}
