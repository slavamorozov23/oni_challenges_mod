using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000107 RID: 263
public class InhaleStates : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>
{
	// Token: 0x060004D4 RID: 1236 RVA: 0x000270C4 File Offset: 0x000252C4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingtoeat;
		this.root.Enter("SetTarget", delegate(InhaleStates.Instance smi)
		{
			this.targetCell.Set(smi.monitor.targetCell, smi, false);
		});
		this.goingtoeat.MoveTo((InhaleStates.Instance smi) => this.targetCell.Get(smi), this.inhaling, null, false).ToggleMainStatusItem(new Func<InhaleStates.Instance, StatusItem>(InhaleStates.GetMovingStatusItem), null);
		GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State state = this.inhaling.DefaultState(this.inhaling.inhale);
		string name = CREATURES.STATUSITEMS.INHALING.NAME;
		string tooltip = CREATURES.STATUSITEMS.INHALING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.inhaling.inhale.PlayAnim((InhaleStates.Instance smi) => smi.def.inhaleAnimPre, KAnim.PlayMode.Once).QueueAnim((InhaleStates.Instance smi) => smi.def.inhaleAnimLoop, true, null).Enter("ComputeInhaleAmount", delegate(InhaleStates.Instance smi)
		{
			smi.ComputeInhaleAmounts();
		}).Update("Consume", delegate(InhaleStates.Instance smi, float dt)
		{
			smi.monitor.Consume(dt * smi.consumptionMult);
		}, UpdateRate.SIM_200ms, false).EventTransition(GameHashes.ElementNoLongerAvailable, this.inhaling.pst, null).Enter("StartInhaleSound", delegate(InhaleStates.Instance smi)
		{
			smi.StartInhaleSound();
		}).Exit("StopInhaleSound", delegate(InhaleStates.Instance smi)
		{
			smi.StopInhaleSound();
		}).ScheduleGoTo((InhaleStates.Instance smi) => smi.inhaleTime, this.inhaling.pst);
		this.inhaling.pst.Transition(this.inhaling.full, (InhaleStates.Instance smi) => smi.def.alwaysPlayPstAnim || InhaleStates.IsFull(smi), UpdateRate.SIM_200ms).Transition(this.behaviourcomplete, GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Not(new StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Transition.ConditionCallback(InhaleStates.IsFull)), UpdateRate.SIM_200ms);
		this.inhaling.full.QueueAnim((InhaleStates.Instance smi) => smi.def.inhaleAnimPst, false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete((InhaleStates.Instance smi) => smi.def.behaviourTag, false);
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x00027390 File Offset: 0x00025590
	private static StatusItem GetMovingStatusItem(InhaleStates.Instance smi)
	{
		if (smi.def.useStorage)
		{
			return smi.def.storageStatusItem;
		}
		return Db.Get().CreatureStatusItems.LookingForFood;
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x000273BC File Offset: 0x000255BC
	private static bool IsFull(InhaleStates.Instance smi)
	{
		if (smi.def.useStorage)
		{
			if (smi.storage != null)
			{
				return smi.storage.IsFull();
			}
		}
		else
		{
			CreatureCalorieMonitor.Instance smi2 = smi.GetSMI<CreatureCalorieMonitor.Instance>();
			if (smi2 != null)
			{
				return smi2.stomach.GetFullness() >= 1f;
			}
		}
		return false;
	}

	// Token: 0x04000385 RID: 901
	public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State goingtoeat;

	// Token: 0x04000386 RID: 902
	public InhaleStates.InhalingStates inhaling;

	// Token: 0x04000387 RID: 903
	public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State behaviourcomplete;

	// Token: 0x04000388 RID: 904
	public StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.IntParameter targetCell;

	// Token: 0x02001171 RID: 4465
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040064AC RID: 25772
		public string inhaleSound;

		// Token: 0x040064AD RID: 25773
		public float inhaleTime = 3f;

		// Token: 0x040064AE RID: 25774
		public Tag behaviourTag = GameTags.Creatures.WantsToEat;

		// Token: 0x040064AF RID: 25775
		public bool useStorage;

		// Token: 0x040064B0 RID: 25776
		public string inhaleAnimPre = "inhale_pre";

		// Token: 0x040064B1 RID: 25777
		public string inhaleAnimLoop = "inhale_loop";

		// Token: 0x040064B2 RID: 25778
		public string inhaleAnimPst = "inhale_pst";

		// Token: 0x040064B3 RID: 25779
		public bool alwaysPlayPstAnim;

		// Token: 0x040064B4 RID: 25780
		public StatusItem storageStatusItem = Db.Get().CreatureStatusItems.LookingForGas;
	}

	// Token: 0x02001172 RID: 4466
	public new class Instance : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.GameInstance
	{
		// Token: 0x0600848A RID: 33930 RVA: 0x003452FB File Offset: 0x003434FB
		public Instance(Chore<InhaleStates.Instance> chore, InhaleStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, def.behaviourTag);
			this.inhaleSound = GlobalAssets.GetSound(def.inhaleSound, false);
		}

		// Token: 0x0600848B RID: 33931 RVA: 0x00345334 File Offset: 0x00343534
		public void StartInhaleSound()
		{
			LoopingSounds component = base.GetComponent<LoopingSounds>();
			if (component != null && base.smi.inhaleSound != null)
			{
				component.StartSound(base.smi.inhaleSound);
			}
		}

		// Token: 0x0600848C RID: 33932 RVA: 0x00345370 File Offset: 0x00343570
		public void StopInhaleSound()
		{
			LoopingSounds component = base.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(base.smi.inhaleSound);
			}
		}

		// Token: 0x0600848D RID: 33933 RVA: 0x003453A0 File Offset: 0x003435A0
		public void ComputeInhaleAmounts()
		{
			float num = base.def.inhaleTime;
			this.inhaleTime = num;
			this.consumptionMult = 1f;
			if (!base.def.useStorage && this.monitor.def.diet != null)
			{
				Diet.Info dietInfo = base.smi.monitor.def.diet.GetDietInfo(base.smi.monitor.GetTargetElement().tag);
				if (dietInfo != null)
				{
					CreatureCalorieMonitor.Instance smi = base.smi.gameObject.GetSMI<CreatureCalorieMonitor.Instance>();
					float num2 = Mathf.Clamp01(smi.GetCalories0to1() / smi.HungryRatio);
					float num3 = 1f - num2;
					float consumptionRate = base.smi.monitor.def.consumptionRate;
					float num4 = dietInfo.ConvertConsumptionMassToCalories(consumptionRate);
					float num5 = num * num4 + 0.8f * smi.calories.GetMax() * num3 * num3 * num3;
					float num6 = num5 / num4;
					if (num6 > 5f * num)
					{
						this.inhaleTime = 5f * num;
						this.consumptionMult = num5 / (this.inhaleTime * num4);
						return;
					}
					this.inhaleTime = num6;
				}
			}
		}

		// Token: 0x040064B5 RID: 25781
		public string inhaleSound;

		// Token: 0x040064B6 RID: 25782
		public float inhaleTime;

		// Token: 0x040064B7 RID: 25783
		public float consumptionMult;

		// Token: 0x040064B8 RID: 25784
		[MySmiGet]
		public GasAndLiquidConsumerMonitor.Instance monitor;

		// Token: 0x040064B9 RID: 25785
		[MyCmpGet]
		public Storage storage;
	}

	// Token: 0x02001173 RID: 4467
	public class InhalingStates : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State
	{
		// Token: 0x040064BA RID: 25786
		public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State inhale;

		// Token: 0x040064BB RID: 25787
		public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State pst;

		// Token: 0x040064BC RID: 25788
		public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State full;
	}
}
