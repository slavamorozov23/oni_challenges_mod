using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A14 RID: 2580
public class CaloriesConsumedSecondaryExcretionMonitor : GameStateMachine<CaloriesConsumedSecondaryExcretionMonitor, CaloriesConsumedSecondaryExcretionMonitor.Instance>, IGameObjectEffectDescriptor
{
	// Token: 0x06004BB0 RID: 19376 RVA: 0x001B7EA4 File Offset: 0x001B60A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.idle.PlayAnim("idle_loop", KAnim.PlayMode.Loop).Enter(delegate(CaloriesConsumedSecondaryExcretionMonitor.Instance smi)
		{
			this.handle = smi.gameObject.Subscribe(-2038961714, new Action<object>(smi.OnCaloriesConsumed));
		}).Exit(delegate(CaloriesConsumedSecondaryExcretionMonitor.Instance smi)
		{
			smi.gameObject.Unsubscribe(this.handle);
		});
		this.schedule_fart.ScheduleGoTo((CaloriesConsumedSecondaryExcretionMonitor.Instance smi) => UnityEngine.Random.Range(3f, 6f), this.needs_to_fart);
		this.needs_to_fart.Enter(new StateMachine<CaloriesConsumedSecondaryExcretionMonitor, CaloriesConsumedSecondaryExcretionMonitor.Instance, IStateMachineTarget, object>.State.Callback(CaloriesConsumedSecondaryExcretionMonitor.CreateChore)).ToggleUrge(Db.Get().Urges.Fart).EventHandler(GameHashes.BeginChore, delegate(CaloriesConsumedSecondaryExcretionMonitor.Instance smi, object o)
		{
			smi.OnStartChore(o);
		});
	}

	// Token: 0x06004BB1 RID: 19377 RVA: 0x001B7F7C File Offset: 0x001B617C
	public static void CreateChore(CaloriesConsumedSecondaryExcretionMonitor.Instance smi)
	{
		CreatureCalorieMonitor.CaloriesConsumedEvent consumptionData = smi.consumptionData;
		new FartChore(smi.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.Fart, consumptionData.calories * 0.001f * smi.sm.kgProducedPerKcalConsumed, smi.sm.producedElement, byte.MaxValue, 0, smi.sm.overpressureThreshold);
	}

	// Token: 0x06004BB2 RID: 19378 RVA: 0x001B7FE0 File Offset: 0x001B61E0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.BUILDINGEFFECTS.DIET_ADDITIONAL_PRODUCED.Replace("{Items}", ElementLoader.GetElement(this.producedElement.CreateTag()).name), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_ADDITIONAL_PRODUCED.Replace("{Items}", ElementLoader.GetElement(this.producedElement.CreateTag()).name), Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x04003226 RID: 12838
	public GameStateMachine<CaloriesConsumedSecondaryExcretionMonitor, CaloriesConsumedSecondaryExcretionMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04003227 RID: 12839
	public GameStateMachine<CaloriesConsumedSecondaryExcretionMonitor, CaloriesConsumedSecondaryExcretionMonitor.Instance, IStateMachineTarget, object>.State schedule_fart;

	// Token: 0x04003228 RID: 12840
	public GameStateMachine<CaloriesConsumedSecondaryExcretionMonitor, CaloriesConsumedSecondaryExcretionMonitor.Instance, IStateMachineTarget, object>.State needs_to_fart;

	// Token: 0x04003229 RID: 12841
	public SimHashes producedElement;

	// Token: 0x0400322A RID: 12842
	public float kgProducedPerKcalConsumed = 1f;

	// Token: 0x0400322B RID: 12843
	private float overpressureThreshold = 2f;

	// Token: 0x0400322C RID: 12844
	private int handle;

	// Token: 0x02001AAE RID: 6830
	public new class Instance : GameStateMachine<CaloriesConsumedSecondaryExcretionMonitor, CaloriesConsumedSecondaryExcretionMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A6C1 RID: 42689 RVA: 0x003BAA65 File Offset: 0x003B8C65
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600A6C2 RID: 42690 RVA: 0x003BAA6E File Offset: 0x003B8C6E
		public void OnStartChore(object o)
		{
			if (((Chore)o).SatisfiesUrge(Db.Get().Urges.Fart))
			{
				this.GoTo(base.sm.idle);
			}
		}

		// Token: 0x0600A6C3 RID: 42691 RVA: 0x003BAA9D File Offset: 0x003B8C9D
		public void OnCaloriesConsumed(object data)
		{
			base.smi.consumptionData = ((Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>)data).value;
			base.smi.GoTo(base.smi.sm.schedule_fart);
		}

		// Token: 0x0400827A RID: 33402
		public CreatureCalorieMonitor.CaloriesConsumedEvent consumptionData;
	}
}
