using System;
using System.Collections.Generic;

// Token: 0x02000A16 RID: 2582
public class ColonyRationMonitor : GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance>
{
	// Token: 0x06004BC0 RID: 19392 RVA: 0x001B83E4 File Offset: 0x001B65E4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Update("UpdateOutOfRations", delegate(ColonyRationMonitor.Instance smi, float dt)
		{
			smi.UpdateIsOutOfRations();
		}, UpdateRate.SIM_200ms, false);
		this.satisfied.ParamTransition<bool>(this.isOutOfRations, this.outofrations, GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.IsTrue).TriggerOnEnter(GameHashes.ColonyHasRationsChanged, null);
		this.outofrations.ParamTransition<bool>(this.isOutOfRations, this.satisfied, GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.IsFalse).TriggerOnEnter(GameHashes.ColonyHasRationsChanged, null);
	}

	// Token: 0x04003231 RID: 12849
	public GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04003232 RID: 12850
	public GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.State outofrations;

	// Token: 0x04003233 RID: 12851
	private StateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.BoolParameter isOutOfRations = new StateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x02001AB4 RID: 6836
	public new class Instance : GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A6D5 RID: 42709 RVA: 0x003BAC57 File Offset: 0x003B8E57
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.UpdateIsOutOfRations();
		}

		// Token: 0x0600A6D6 RID: 42710 RVA: 0x003BAC68 File Offset: 0x003B8E68
		public void UpdateIsOutOfRations()
		{
			bool value = true;
			using (List<Edible>.Enumerator enumerator = Components.Edibles.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetComponent<Pickupable>().UnreservedFetchAmount > 0f)
					{
						value = false;
						break;
					}
				}
			}
			base.smi.sm.isOutOfRations.Set(value, base.smi, false);
		}

		// Token: 0x0600A6D7 RID: 42711 RVA: 0x003BACEC File Offset: 0x003B8EEC
		public bool IsOutOfRations()
		{
			return base.smi.sm.isOutOfRations.Get(base.smi);
		}
	}
}
