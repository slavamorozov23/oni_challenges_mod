using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020000FA RID: 250
public class HiveEatingStates : GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>
{
	// Token: 0x0600049F RID: 1183 RVA: 0x00025D64 File Offset: 0x00023F64
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.eating;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State state = this.eating;
		string name = CREATURES.STATUSITEMS.HIVE_DIGESTING.NAME;
		string tooltip = CREATURES.STATUSITEMS.HIVE_DIGESTING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).DefaultState(this.eating.pre).Enter(delegate(HiveEatingStates.Instance smi)
		{
			smi.TurnOn();
		}).Exit(delegate(HiveEatingStates.Instance smi)
		{
			smi.TurnOff();
		});
		this.eating.pre.PlayAnim("eating_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.eating.loop);
		this.eating.loop.PlayAnim("eating_loop", KAnim.PlayMode.Loop).Update(delegate(HiveEatingStates.Instance smi, float dt)
		{
			smi.EatOreFromStorage(smi, dt);
		}, UpdateRate.SIM_4000ms, false).EventTransition(GameHashes.OnStorageChange, this.eating.pst, (HiveEatingStates.Instance smi) => !smi.storage.FindFirst(smi.def.consumedOre));
		this.eating.pst.PlayAnim("eating_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToEat, false);
	}

	// Token: 0x0400036E RID: 878
	public HiveEatingStates.EatingStates eating;

	// Token: 0x0400036F RID: 879
	public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State behaviourcomplete;

	// Token: 0x0200114D RID: 4429
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06008435 RID: 33845 RVA: 0x00344984 File Offset: 0x00342B84
		public Def(Tag consumedOre)
		{
			this.consumedOre = consumedOre;
		}

		// Token: 0x04006460 RID: 25696
		public Tag consumedOre;
	}

	// Token: 0x0200114E RID: 4430
	public class EatingStates : GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State
	{
		// Token: 0x04006461 RID: 25697
		public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State pre;

		// Token: 0x04006462 RID: 25698
		public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State loop;

		// Token: 0x04006463 RID: 25699
		public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State pst;
	}

	// Token: 0x0200114F RID: 4431
	public new class Instance : GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.GameInstance
	{
		// Token: 0x06008437 RID: 33847 RVA: 0x0034499B File Offset: 0x00342B9B
		public Instance(Chore<HiveEatingStates.Instance> chore, HiveEatingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToEat);
		}

		// Token: 0x06008438 RID: 33848 RVA: 0x003449BF File Offset: 0x00342BBF
		public void TurnOn()
		{
			this.emitter.emitRads = 600f * this.emitter.emitRate;
			this.emitter.Refresh();
		}

		// Token: 0x06008439 RID: 33849 RVA: 0x003449E8 File Offset: 0x00342BE8
		public void TurnOff()
		{
			this.emitter.emitRads = 0f;
			this.emitter.Refresh();
		}

		// Token: 0x0600843A RID: 33850 RVA: 0x00344A08 File Offset: 0x00342C08
		public void EatOreFromStorage(HiveEatingStates.Instance smi, float dt)
		{
			GameObject gameObject = smi.storage.FindFirst(smi.def.consumedOre);
			if (!gameObject)
			{
				return;
			}
			float num = 0.25f;
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component == null)
			{
				return;
			}
			PrimaryElement component2 = component.GetComponent<PrimaryElement>();
			if (component2 == null)
			{
				return;
			}
			Diet.Info dietInfo = smi.GetSMI<BeehiveCalorieMonitor.Instance>().stomach.diet.GetDietInfo(component.PrefabTag);
			if (dietInfo == null)
			{
				return;
			}
			AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(smi.gameObject);
			float calories = amountInstance.GetMax() - amountInstance.value;
			float num2 = dietInfo.ConvertCaloriesToConsumptionMass(calories);
			float num3 = num * dt;
			if (num2 < num3)
			{
				num3 = num2;
			}
			num3 = Mathf.Min(num3, component2.Mass);
			component2.Mass -= num3;
			Pickupable component3 = component2.GetComponent<Pickupable>();
			if (component3.storage != null)
			{
				component3.storage.Trigger(-1452790913, smi.gameObject);
				component3.storage.Trigger(-1697596308, smi.gameObject);
			}
			float calories2 = dietInfo.ConvertConsumptionMassToCalories(num3);
			smi.gameObject.BoxingTrigger(-2038961714, new CreatureCalorieMonitor.CaloriesConsumedEvent
			{
				tag = component.PrefabTag,
				calories = calories2
			});
		}

		// Token: 0x04006464 RID: 25700
		[MyCmpReq]
		public Storage storage;

		// Token: 0x04006465 RID: 25701
		[MyCmpReq]
		private RadiationEmitter emitter;
	}
}
