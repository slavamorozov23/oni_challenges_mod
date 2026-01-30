using System;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x0200010A RID: 266
public class MoltStates : GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>
{
	// Token: 0x060004E2 RID: 1250 RVA: 0x00027674 File Offset: 0x00025874
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moltpre;
		GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.MOLTING.NAME;
		string tooltip = CREATURES.STATUSITEMS.MOLTING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.moltpre.Enter(new StateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State.Callback(MoltStates.Molt)).QueueAnim("lay_egg_pre", false, null).OnAnimQueueComplete(this.moltpst);
		this.moltpst.QueueAnim("lay_egg_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.ScalesGrown, false);
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x00027731 File Offset: 0x00025931
	private static void Molt(MoltStates.Instance smi)
	{
		smi.eggPos = smi.transform.GetPosition();
		smi.GetSMI<ScaleGrowthMonitor.Instance>().Shear();
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x00027750 File Offset: 0x00025950
	private static int GetMoveAsideCell(MoltStates.Instance smi)
	{
		int num = 1;
		if (GenericGameSettings.instance.acceleratedLifecycle)
		{
			num = 8;
		}
		int cell = Grid.PosToCell(smi);
		if (Grid.IsValidCell(cell))
		{
			int num2 = Grid.OffsetCell(cell, num, 0);
			if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
			{
				return num2;
			}
			int num3 = Grid.OffsetCell(cell, -num, 0);
			if (Grid.IsValidCell(num3))
			{
				return num3;
			}
		}
		return Grid.InvalidCell;
	}

	// Token: 0x0400038F RID: 911
	public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State moltpre;

	// Token: 0x04000390 RID: 912
	public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State moltpst;

	// Token: 0x04000391 RID: 913
	public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State behaviourcomplete;

	// Token: 0x0200117A RID: 4474
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200117B RID: 4475
	public new class Instance : GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.GameInstance
	{
		// Token: 0x060084A3 RID: 33955 RVA: 0x003455C7 File Offset: 0x003437C7
		public Instance(Chore<MoltStates.Instance> chore, MoltStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.ScalesGrown);
		}

		// Token: 0x040064CB RID: 25803
		public Vector3 eggPos;
	}
}
