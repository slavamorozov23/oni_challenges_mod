using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public class FlopStates : GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>
{
	// Token: 0x06000494 RID: 1172 RVA: 0x00025940 File Offset: 0x00023B40
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.flop_pre;
		GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.FLOPPING.NAME;
		string tooltip = CREATURES.STATUSITEMS.FLOPPING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Exclamation;
		NotificationType notification_type = NotificationType.Bad;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.flop_pre.Enter(new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State.Callback(FlopStates.ChooseDirection)).Transition(this.flop_cycle, new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback(FlopStates.ShouldFlop), UpdateRate.SIM_200ms).Transition(this.pst, GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Not(new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback(FlopStates.ShouldFlop)), UpdateRate.SIM_200ms);
		this.flop_cycle.PlayAnim("flop_loop", KAnim.PlayMode.Once).Transition(this.pst, new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback(FlopStates.IsSubstantialLiquid), UpdateRate.SIM_200ms).Update("Flop", new Action<FlopStates.Instance, float>(FlopStates.FlopForward), UpdateRate.SIM_33ms, false).OnAnimQueueComplete(this.flop_pre);
		this.pst.QueueAnim("flop_loop", true, null).BehaviourComplete(GameTags.Creatures.Flopping, false);
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x00025A58 File Offset: 0x00023C58
	public static bool ShouldFlop(FlopStates.Instance smi)
	{
		int num = Grid.CellBelow(Grid.PosToCell(smi.transform.GetPosition()));
		return Grid.IsValidCell(num) && Grid.Solid[num];
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x00025A90 File Offset: 0x00023C90
	public static void ChooseDirection(FlopStates.Instance smi)
	{
		int cell = Grid.PosToCell(smi.transform.GetPosition());
		if (FlopStates.SearchForLiquid(cell, 1))
		{
			smi.currentDir = 1f;
			return;
		}
		if (FlopStates.SearchForLiquid(cell, -1))
		{
			smi.currentDir = -1f;
			return;
		}
		if (UnityEngine.Random.value > 0.5f)
		{
			smi.currentDir = 1f;
			return;
		}
		smi.currentDir = -1f;
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x00025AFC File Offset: 0x00023CFC
	private static bool SearchForLiquid(int cell, int delta_x)
	{
		while (Grid.IsValidCell(cell))
		{
			if (Grid.IsSubstantialLiquid(cell, 0.35f))
			{
				return true;
			}
			if (Grid.Solid[cell])
			{
				return false;
			}
			if (Grid.CritterImpassable[cell])
			{
				return false;
			}
			int num = Grid.CellBelow(cell);
			if (Grid.IsValidCell(num) && Grid.Solid[num])
			{
				cell += delta_x;
			}
			else
			{
				cell = num;
			}
		}
		return false;
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00025B68 File Offset: 0x00023D68
	public static void FlopForward(FlopStates.Instance smi, float dt)
	{
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		int currentFrame = component.currentFrame;
		if (component.IsVisible() && (currentFrame < 23 || currentFrame > 36))
		{
			return;
		}
		Vector3 position = smi.transform.GetPosition();
		Vector3 vector = position;
		vector.x = position.x + smi.currentDir * dt * 1f;
		int new_cell = Grid.PosToCell(vector);
		if (FlopStates.CanFlopForward(smi, new_cell))
		{
			smi.transform.SetPosition(vector);
			return;
		}
		smi.currentDir = -smi.currentDir;
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x00025BEC File Offset: 0x00023DEC
	private static bool CanFlopForward(FlopStates.Instance smi, int new_cell)
	{
		if (!Grid.IsValidCell(new_cell) || Grid.Solid[new_cell] || Grid.CritterImpassable[new_cell])
		{
			return false;
		}
		CellOffset[] occupiedCellsOffsets = smi.GetComponent<OccupyArea>().OccupiedCellsOffsets;
		for (int i = 0; i < occupiedCellsOffsets.Length; i++)
		{
			int num = Grid.OffsetCell(new_cell, occupiedCellsOffsets[i]);
			if (!Grid.IsValidCell(num) || Grid.Solid[num] || Grid.CritterImpassable[num])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x00025C6A File Offset: 0x00023E6A
	public static bool IsSubstantialLiquid(FlopStates.Instance smi)
	{
		return Grid.IsSubstantialLiquid(Grid.PosToCell(smi.transform.GetPosition()), 0.35f);
	}

	// Token: 0x04000368 RID: 872
	private GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State flop_pre;

	// Token: 0x04000369 RID: 873
	private GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State flop_cycle;

	// Token: 0x0400036A RID: 874
	private GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State pst;

	// Token: 0x02001148 RID: 4424
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001149 RID: 4425
	public new class Instance : GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.GameInstance
	{
		// Token: 0x0600842E RID: 33838 RVA: 0x003448BA File Offset: 0x00342ABA
		public Instance(Chore<FlopStates.Instance> chore, FlopStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Flopping);
		}

		// Token: 0x0400645D RID: 25693
		public float currentDir = 1f;
	}
}
