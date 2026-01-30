using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class ActiveParticleConsumer : GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>
{
	// Token: 0x060001B7 RID: 439 RVA: 0x0000BF0C File Offset: 0x0000A10C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.root.Enter(delegate(ActiveParticleConsumer.Instance smi)
		{
			smi.GetComponent<Operational>().SetFlag(ActiveParticleConsumer.canConsumeParticlesFlag, false);
		});
		this.inoperational.EventTransition(GameHashes.OnParticleStorageChanged, this.operational, new StateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.Transition.ConditionCallback(this.IsReady)).ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForHighEnergyParticles, null);
		this.operational.DefaultState(this.operational.waiting).EventTransition(GameHashes.OnParticleStorageChanged, this.inoperational, GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.Not(new StateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.Transition.ConditionCallback(this.IsReady))).ToggleOperationalFlag(ActiveParticleConsumer.canConsumeParticlesFlag);
		this.operational.waiting.EventTransition(GameHashes.ActiveChanged, this.operational.consuming, (ActiveParticleConsumer.Instance smi) => smi.GetComponent<Operational>().IsActive);
		this.operational.consuming.EventTransition(GameHashes.ActiveChanged, this.operational.waiting, (ActiveParticleConsumer.Instance smi) => !smi.GetComponent<Operational>().IsActive).Update(delegate(ActiveParticleConsumer.Instance smi, float dt)
		{
			smi.Update(dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x0000C06C File Offset: 0x0000A26C
	public bool IsReady(ActiveParticleConsumer.Instance smi)
	{
		return smi.storage.Particles >= smi.def.minParticlesForOperational;
	}

	// Token: 0x04000109 RID: 265
	public static readonly Operational.Flag canConsumeParticlesFlag = new Operational.Flag("canConsumeParticles", Operational.Flag.Type.Requirement);

	// Token: 0x0400010A RID: 266
	public GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State inoperational;

	// Token: 0x0400010B RID: 267
	public ActiveParticleConsumer.OperationalStates operational;

	// Token: 0x02001085 RID: 4229
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06008246 RID: 33350 RVA: 0x00341BD0 File Offset: 0x0033FDD0
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.BUILDINGEFFECTS.ACTIVE_PARTICLE_CONSUMPTION.Replace("{Rate}", GameUtil.GetFormattedHighEnergyParticles(this.activeConsumptionRate, GameUtil.TimeSlice.PerSecond, true)), UI.BUILDINGEFFECTS.TOOLTIPS.ACTIVE_PARTICLE_CONSUMPTION.Replace("{Rate}", GameUtil.GetFormattedHighEnergyParticles(this.activeConsumptionRate, GameUtil.TimeSlice.PerSecond, true)), Descriptor.DescriptorType.Requirement, false)
			};
		}

		// Token: 0x040062AA RID: 25258
		public float activeConsumptionRate = 1f;

		// Token: 0x040062AB RID: 25259
		public float minParticlesForOperational = 1f;

		// Token: 0x040062AC RID: 25260
		public string meterSymbolName;
	}

	// Token: 0x02001086 RID: 4230
	public class OperationalStates : GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State
	{
		// Token: 0x040062AD RID: 25261
		public GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State waiting;

		// Token: 0x040062AE RID: 25262
		public GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State consuming;
	}

	// Token: 0x02001087 RID: 4231
	public new class Instance : GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.GameInstance
	{
		// Token: 0x06008249 RID: 33353 RVA: 0x00341C4D File Offset: 0x0033FE4D
		public Instance(IStateMachineTarget master, ActiveParticleConsumer.Def def) : base(master, def)
		{
			this.storage = master.GetComponent<HighEnergyParticleStorage>();
		}

		// Token: 0x0600824A RID: 33354 RVA: 0x00341C63 File Offset: 0x0033FE63
		public void Update(float dt)
		{
			this.storage.ConsumeAndGet(dt * base.def.activeConsumptionRate);
		}

		// Token: 0x040062AF RID: 25263
		public bool ShowWorkingStatus;

		// Token: 0x040062B0 RID: 25264
		public HighEnergyParticleStorage storage;
	}
}
