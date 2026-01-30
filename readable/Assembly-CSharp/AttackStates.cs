using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000D7 RID: 215
public class AttackStates : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>
{
	// Token: 0x060003D0 RID: 976 RVA: 0x00020664 File Offset: 0x0001E864
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.waitBeforeAttack;
		this.root.Enter("SetTarget", delegate(AttackStates.Instance smi)
		{
			this.target.Set(smi.GetSMI<ThreatMonitor.Instance>().MainThreat, smi, false);
			this.cellOffsets = smi.def.cellOffsets;
		});
		this.waitBeforeAttack.ScheduleGoTo((AttackStates.Instance smi) => UnityEngine.Random.Range(0f, 4f), this.approach);
		GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State state = this.approach.InitializeStates(this.masterTarget, this.target, this.attack, null, this.cellOffsets, null);
		string name = CREATURES.STATUSITEMS.ATTACK_APPROACH.NAME;
		string tooltip = CREATURES.STATUSITEMS.ATTACK_APPROACH.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State state2 = this.attack.DefaultState(this.attack.pre);
		string name2 = CREATURES.STATUSITEMS.ATTACK.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.ATTACK.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main);
		this.attack.pre.PlayAnim((AttackStates.Instance smi) => smi.def.preAnim, KAnim.PlayMode.Once).Exit(delegate(AttackStates.Instance smi)
		{
			smi.GetComponent<Weapon>().AttackTarget(this.target.Get(smi));
		}).OnAnimQueueComplete(this.attack.pst);
		this.attack.pst.PlayAnim((AttackStates.Instance smi) => smi.def.pstAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Attack, false);
	}

	// Token: 0x040002F2 RID: 754
	public StateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.TargetParameter target;

	// Token: 0x040002F3 RID: 755
	public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.ApproachSubState<AttackableBase> approach;

	// Token: 0x040002F4 RID: 756
	public CellOffset[] cellOffsets;

	// Token: 0x040002F5 RID: 757
	public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State waitBeforeAttack;

	// Token: 0x040002F6 RID: 758
	public AttackStates.AttackingStates attack;

	// Token: 0x040002F7 RID: 759
	public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State behaviourcomplete;

	// Token: 0x020010D3 RID: 4307
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600831D RID: 33565 RVA: 0x00342DB4 File Offset: 0x00340FB4
		public Def(string pre_anim = "eat_pre", string pst_anim = "eat_pst", CellOffset[] cell_offsets = null)
		{
			this.preAnim = pre_anim;
			this.pstAnim = pst_anim;
			if (cell_offsets != null)
			{
				this.cellOffsets = cell_offsets;
			}
		}

		// Token: 0x0400635A RID: 25434
		public string preAnim;

		// Token: 0x0400635B RID: 25435
		public string pstAnim;

		// Token: 0x0400635C RID: 25436
		public CellOffset[] cellOffsets = new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(1, 0),
			new CellOffset(-1, 0),
			new CellOffset(1, 1),
			new CellOffset(-1, 1)
		};
	}

	// Token: 0x020010D4 RID: 4308
	public class AttackingStates : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State
	{
		// Token: 0x0400635D RID: 25437
		public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State pre;

		// Token: 0x0400635E RID: 25438
		public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State pst;
	}

	// Token: 0x020010D5 RID: 4309
	public new class Instance : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.GameInstance
	{
		// Token: 0x0600831F RID: 33567 RVA: 0x00342E39 File Offset: 0x00341039
		public Instance(Chore<AttackStates.Instance> chore, AttackStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Attack);
		}
	}
}
