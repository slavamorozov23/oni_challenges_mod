using System;
using UnityEngine;

// Token: 0x02000895 RID: 2197
public class CropTendingMonitor : GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>
{
	// Token: 0x06003C79 RID: 15481 RVA: 0x001524B5 File Offset: 0x001506B5
	private bool InterestedInTendingCrops(CropTendingMonitor.Instance smi)
	{
		return !smi.HasTag(GameTags.Creatures.Hungry) || UnityEngine.Random.value <= smi.def.unsatisfiedTendChance;
	}

	// Token: 0x06003C7A RID: 15482 RVA: 0x001524DC File Offset: 0x001506DC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cooldown;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.cooldown.ParamTransition<float>(this.cooldownTimer, this.lookingForCrop, (CropTendingMonitor.Instance smi, float p) => this.cooldownTimer.Get(smi) <= 0f && this.InterestedInTendingCrops(smi)).ParamTransition<float>(this.cooldownTimer, this.reset, (CropTendingMonitor.Instance smi, float p) => this.cooldownTimer.Get(smi) <= 0f && !this.InterestedInTendingCrops(smi)).Update(delegate(CropTendingMonitor.Instance smi, float dt)
		{
			this.cooldownTimer.Delta(-dt, smi);
		}, UpdateRate.SIM_1000ms, false);
		this.lookingForCrop.ToggleBehaviour(GameTags.Creatures.WantsToTendCrops, (CropTendingMonitor.Instance smi) => true, delegate(CropTendingMonitor.Instance smi)
		{
			smi.GoTo(this.reset);
		});
		this.reset.Exit(delegate(CropTendingMonitor.Instance smi)
		{
			this.cooldownTimer.Set(600f / smi.def.numCropsTendedPerCycle, smi, false);
		}).GoTo(this.cooldown);
	}

	// Token: 0x04002549 RID: 9545
	private StateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.FloatParameter cooldownTimer;

	// Token: 0x0400254A RID: 9546
	private GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.State cooldown;

	// Token: 0x0400254B RID: 9547
	private GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.State lookingForCrop;

	// Token: 0x0400254C RID: 9548
	private GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.State reset;

	// Token: 0x0200186F RID: 6255
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007AEB RID: 31467
		public float numCropsTendedPerCycle = 8f;

		// Token: 0x04007AEC RID: 31468
		public float unsatisfiedTendChance = 0.5f;
	}

	// Token: 0x02001870 RID: 6256
	public new class Instance : GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.GameInstance
	{
		// Token: 0x06009EE6 RID: 40678 RVA: 0x003A463C File Offset: 0x003A283C
		public Instance(IStateMachineTarget master, CropTendingMonitor.Def def) : base(master, def)
		{
		}
	}
}
