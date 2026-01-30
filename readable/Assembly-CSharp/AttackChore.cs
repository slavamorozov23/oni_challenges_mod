using System;
using TUNING;
using UnityEngine;

// Token: 0x02000491 RID: 1169
public class AttackChore : Chore<AttackChore.StatesInstance>
{
	// Token: 0x060018D6 RID: 6358 RVA: 0x00089E29 File Offset: 0x00088029
	protected override void OnStateMachineStop(string reason, StateMachine.Status status)
	{
		this.CleanUpMultitool();
		base.OnStateMachineStop(reason, status);
	}

	// Token: 0x060018D7 RID: 6359 RVA: 0x00089E3C File Offset: 0x0008803C
	public AttackChore(IStateMachineTarget target, GameObject enemy) : base(Db.Get().ChoreTypes.Attack, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new AttackChore.StatesInstance(this);
		base.smi.sm.attackTarget.Set(enemy, base.smi, false);
		Game.Instance.Trigger(1980521255, enemy);
		base.SetPrioritizable(enemy.GetComponent<Prioritizable>());
	}

	// Token: 0x060018D8 RID: 6360 RVA: 0x00089EB8 File Offset: 0x000880B8
	public string GetHitAnim()
	{
		Workable component = base.smi.sm.attackTarget.Get(base.smi).gameObject.GetComponent<Workable>();
		if (component)
		{
			return MultitoolController.GetAnimationStrings(component, this.gameObject.GetComponent<WorkerBase>(), "hit")[1].Replace("_loop", "");
		}
		return "hit";
	}

	// Token: 0x060018D9 RID: 6361 RVA: 0x00089F20 File Offset: 0x00088120
	public void OnTargetMoved(object data)
	{
		int num = Grid.PosToCell(base.smi.master.gameObject);
		if (base.smi.sm.attackTarget.Get(base.smi) == null)
		{
			this.CleanUpMultitool();
			return;
		}
		if (base.smi.GetCurrentState() == base.smi.sm.attack)
		{
			int num2 = Grid.PosToCell(base.smi.sm.attackTarget.Get(base.smi).gameObject);
			IApproachable component = base.smi.sm.attackTarget.Get(base.smi).gameObject.GetComponent<IApproachable>();
			if (component != null)
			{
				CellOffset[] offsets = component.GetOffsets();
				if (num == num2 || !Grid.IsCellOffsetOf(num, num2, offsets))
				{
					if (this.multiTool != null)
					{
						this.CleanUpMultitool();
					}
					base.smi.GoTo(base.smi.sm.approachtarget);
				}
			}
			else
			{
				global::Debug.Log("has no approachable");
			}
		}
		if (this.multiTool != null)
		{
			this.multiTool.UpdateHitEffectTarget();
		}
	}

	// Token: 0x060018DA RID: 6362 RVA: 0x0008A03B File Offset: 0x0008823B
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.attacker.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x0008A06C File Offset: 0x0008826C
	protected override void End(string reason)
	{
		this.CleanUpMultitool();
		if (!base.smi.sm.attackTarget.IsNull(base.smi))
		{
			GameObject gameObject = base.smi.sm.attackTarget.Get(base.smi);
			Prioritizable component = gameObject.GetComponent<Prioritizable>();
			if (component != null && component.IsPrioritizable())
			{
				Prioritizable.RemoveRef(gameObject);
			}
		}
		base.End(reason);
	}

	// Token: 0x060018DC RID: 6364 RVA: 0x0008A0DD File Offset: 0x000882DD
	public void OnTargetDestroyed(object data)
	{
		this.Fail("target destroyed");
	}

	// Token: 0x060018DD RID: 6365 RVA: 0x0008A0EA File Offset: 0x000882EA
	private void CleanUpMultitool()
	{
		if (base.smi.master.multiTool != null)
		{
			this.multiTool.DestroyHitEffect();
			this.multiTool.StopSM("attack complete");
			this.multiTool = null;
		}
	}

	// Token: 0x04000E59 RID: 3673
	private MultitoolController.Instance multiTool;

	// Token: 0x020012B5 RID: 4789
	public class StatesInstance : GameStateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.GameInstance
	{
		// Token: 0x06008923 RID: 35107 RVA: 0x00350385 File Offset: 0x0034E585
		public StatesInstance(AttackChore master) : base(master)
		{
		}
	}

	// Token: 0x020012B6 RID: 4790
	public class States : GameStateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore>
	{
		// Token: 0x06008924 RID: 35108 RVA: 0x00350390 File Offset: 0x0034E590
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approachtarget;
			this.root.ToggleStatusItem(Db.Get().DuplicantStatusItems.Fighting, (AttackChore.StatesInstance smi) => smi.master.gameObject).EventHandler(GameHashes.TargetLost, delegate(AttackChore.StatesInstance smi)
			{
				smi.master.Fail("target lost");
			}).Enter(delegate(AttackChore.StatesInstance smi)
			{
				smi.master.GetComponent<Weapon>().Configure(DUPLICANTSTATS.STANDARD.Combat.BasicWeapon.MIN_DAMAGE_PER_HIT, DUPLICANTSTATS.STANDARD.Combat.BasicWeapon.MAX_DAMAGE_PER_HIT, DUPLICANTSTATS.STANDARD.Combat.BasicWeapon.DAMAGE_TYPE, DUPLICANTSTATS.STANDARD.Combat.BasicWeapon.TARGET_TYPE, DUPLICANTSTATS.STANDARD.Combat.BasicWeapon.MAX_HITS, DUPLICANTSTATS.STANDARD.Combat.BasicWeapon.AREA_OF_EFFECT_RADIUS);
			});
			this.approachtarget.InitializeStates(this.attacker, this.attackTarget, this.attack, null, BaseMinionConfig.ATTACK_OFFSETS, NavigationTactics.Range_3_ProhibitOverlap).Enter(delegate(AttackChore.StatesInstance smi)
			{
				smi.master.CleanUpMultitool();
				smi.master.Trigger(1039067354, this.attackTarget.Get(smi));
				Health component = this.attackTarget.Get(smi).GetComponent<Health>();
				if (component == null || component.IsDefeated())
				{
					smi.StopSM("target defeated approachtarget");
				}
			});
			this.attack.Target(this.attacker).Enter(delegate(AttackChore.StatesInstance smi)
			{
				this.attackTarget.Get(smi).Subscribe(1088554450, new Action<object>(smi.master.OnTargetMoved));
				if (this.attackTarget != null && smi.master.multiTool == null)
				{
					smi.master.multiTool = new MultitoolController.Instance(this.attackTarget.Get(smi).GetComponent<Workable>(), smi.master.GetComponent<WorkerBase>(), "attack", Assets.GetPrefab(EffectConfigs.AttackSplashId));
					smi.master.multiTool.StartSM();
				}
				this.attackTarget.Get(smi).Subscribe(1969584890, new Action<object>(smi.master.OnTargetDestroyed));
				smi.ScheduleGoTo(1f / DUPLICANTSTATS.STANDARD.Combat.BasicWeapon.ATTACKS_PER_SECOND, this.success);
			}).Update(delegate(AttackChore.StatesInstance smi, float dt)
			{
				if (smi.master.multiTool != null)
				{
					smi.master.multiTool.UpdateHitEffectTarget();
				}
			}, UpdateRate.SIM_200ms, false).Exit(delegate(AttackChore.StatesInstance smi)
			{
				if (this.attackTarget.Get(smi) != null)
				{
					this.attackTarget.Get(smi).Unsubscribe(1088554450, new Action<object>(smi.master.OnTargetMoved));
				}
			});
			this.success.Enter("finishAttack", delegate(AttackChore.StatesInstance smi)
			{
				if (this.attackTarget.Get(smi) != null)
				{
					Transform transform = this.attackTarget.Get(smi).transform;
					Weapon component = this.attacker.Get(smi).gameObject.GetComponent<Weapon>();
					if (!(component != null))
					{
						smi.master.CleanUpMultitool();
						smi.StopSM("no weapon");
						return;
					}
					component.AttackTarget(transform.gameObject);
					Health component2 = this.attackTarget.Get(smi).GetComponent<Health>();
					if (component2 != null)
					{
						if (!component2.IsDefeated())
						{
							smi.GoTo(this.attack);
							return;
						}
						smi.master.CleanUpMultitool();
						smi.StopSM("target defeated success");
						return;
					}
				}
				else
				{
					smi.master.CleanUpMultitool();
					smi.StopSM("no target");
				}
			}).ReturnSuccess();
		}

		// Token: 0x040068A3 RID: 26787
		public StateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.TargetParameter attackTarget;

		// Token: 0x040068A4 RID: 26788
		public StateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.TargetParameter attacker;

		// Token: 0x040068A5 RID: 26789
		public GameStateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.ApproachSubState<RangedAttackable> approachtarget;

		// Token: 0x040068A6 RID: 26790
		public GameStateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.State attack;

		// Token: 0x040068A7 RID: 26791
		public GameStateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.State success;
	}
}
