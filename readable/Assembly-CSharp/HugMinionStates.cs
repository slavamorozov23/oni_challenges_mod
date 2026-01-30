using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000101 RID: 257
public class HugMinionStates : GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>
{
	// Token: 0x060004B8 RID: 1208 RVA: 0x00026680 File Offset: 0x00024880
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moving;
		this.moving.MoveTo(new Func<HugMinionStates.Instance, int>(HugMinionStates.FindFlopLocation), this.waiting, this.behaviourcomplete, false);
		GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.State state = this.waiting.Enter(delegate(HugMinionStates.Instance smi)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
		}).ParamTransition<float>(this.timeout, this.behaviourcomplete, (HugMinionStates.Instance smi, float p) => p > 60f && !smi.GetSMI<HugMonitor.Instance>().IsHugging()).Update(delegate(HugMinionStates.Instance smi, float dt)
		{
			smi.sm.timeout.Delta(dt, smi);
		}, UpdateRate.SIM_200ms, false).PlayAnim("waiting_pre").QueueAnim("waiting_loop", true, null);
		string name = CREATURES.STATUSITEMS.HUGMINIONWAITING.NAME;
		string tooltip = CREATURES.STATUSITEMS.HUGMINIONWAITING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsAHug, false);
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x000267A8 File Offset: 0x000249A8
	private static int FindFlopLocation(HugMinionStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		FloorCellQuery floorCellQuery = PathFinderQueries.floorCellQuery.Reset(1, 1);
		component.RunQuery(floorCellQuery);
		if (floorCellQuery.result_cells.Count > 0)
		{
			smi.targetFlopCell = floorCellQuery.result_cells[UnityEngine.Random.Range(0, floorCellQuery.result_cells.Count)];
		}
		else
		{
			smi.targetFlopCell = Grid.InvalidCell;
		}
		return smi.targetFlopCell;
	}

	// Token: 0x04000379 RID: 889
	public GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.ApproachSubState<EggIncubator> moving;

	// Token: 0x0400037A RID: 890
	public GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.State waiting;

	// Token: 0x0400037B RID: 891
	public GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.State behaviourcomplete;

	// Token: 0x0400037C RID: 892
	public StateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.FloatParameter timeout;

	// Token: 0x02001164 RID: 4452
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001165 RID: 4453
	public new class Instance : GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.GameInstance
	{
		// Token: 0x0600846E RID: 33902 RVA: 0x00344FC1 File Offset: 0x003431C1
		public Instance(Chore<HugMinionStates.Instance> chore, HugMinionStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsAHug);
		}

		// Token: 0x04006494 RID: 25748
		public int targetFlopCell;
	}
}
