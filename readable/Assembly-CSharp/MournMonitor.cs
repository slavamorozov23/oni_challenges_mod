using System;
using Klei.AI;

// Token: 0x02000A36 RID: 2614
public class MournMonitor : GameStateMachine<MournMonitor, MournMonitor.Instance>
{
	// Token: 0x06004C48 RID: 19528 RVA: 0x001BB684 File Offset: 0x001B9884
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.EffectAdded, new GameStateMachine<MournMonitor, MournMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.OnEffectAdded)).Enter(delegate(MournMonitor.Instance smi)
		{
			if (this.ShouldMourn(smi))
			{
				smi.GoTo(this.needsToMourn);
			}
		});
		this.needsToMourn.ToggleChore((MournMonitor.Instance smi) => new MournChore(smi.master), this.idle);
	}

	// Token: 0x06004C49 RID: 19529 RVA: 0x001BB6F8 File Offset: 0x001B98F8
	private bool ShouldMourn(MournMonitor.Instance smi)
	{
		Effect effect = Db.Get().effects.Get("Mourning");
		return smi.master.GetComponent<Effects>().HasEffect(effect);
	}

	// Token: 0x06004C4A RID: 19530 RVA: 0x001BB72B File Offset: 0x001B992B
	private void OnEffectAdded(MournMonitor.Instance smi, object data)
	{
		if (this.ShouldMourn(smi))
		{
			smi.GoTo(this.needsToMourn);
		}
	}

	// Token: 0x040032AC RID: 12972
	private GameStateMachine<MournMonitor, MournMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x040032AD RID: 12973
	private GameStateMachine<MournMonitor, MournMonitor.Instance, IStateMachineTarget, object>.State needsToMourn;

	// Token: 0x02001B0A RID: 6922
	public new class Instance : GameStateMachine<MournMonitor, MournMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A832 RID: 43058 RVA: 0x003BE8E8 File Offset: 0x003BCAE8
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
