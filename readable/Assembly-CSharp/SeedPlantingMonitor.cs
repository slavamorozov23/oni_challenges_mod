using System;
using UnityEngine;

// Token: 0x02000112 RID: 274
public class SeedPlantingMonitor : GameStateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>
{
	// Token: 0x0600050E RID: 1294 RVA: 0x00028C0C File Offset: 0x00026E0C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToPlantSeed, new StateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>.Transition.ConditionCallback(SeedPlantingMonitor.ShouldSearchForSeeds), delegate(SeedPlantingMonitor.Instance smi)
		{
			smi.RefreshSearchTime();
		});
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x00028C5D File Offset: 0x00026E5D
	public static bool ShouldSearchForSeeds(SeedPlantingMonitor.Instance smi)
	{
		return Time.time >= smi.nextSearchTime;
	}

	// Token: 0x02001195 RID: 4501
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006510 RID: 25872
		public float searchMinInterval = 60f;

		// Token: 0x04006511 RID: 25873
		public float searchMaxInterval = 300f;
	}

	// Token: 0x02001196 RID: 4502
	public new class Instance : GameStateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>.GameInstance
	{
		// Token: 0x060084F0 RID: 34032 RVA: 0x0034619B File Offset: 0x0034439B
		public Instance(IStateMachineTarget master, SeedPlantingMonitor.Def def) : base(master, def)
		{
			this.RefreshSearchTime();
		}

		// Token: 0x060084F1 RID: 34033 RVA: 0x003461AB File Offset: 0x003443AB
		public void RefreshSearchTime()
		{
			this.nextSearchTime = Time.time + Mathf.Lerp(base.def.searchMinInterval, base.def.searchMaxInterval, UnityEngine.Random.value);
		}

		// Token: 0x04006512 RID: 25874
		public float nextSearchTime;
	}
}
