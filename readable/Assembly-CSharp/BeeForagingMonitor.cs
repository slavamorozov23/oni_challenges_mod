using System;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class BeeForagingMonitor : GameStateMachine<BeeForagingMonitor, BeeForagingMonitor.Instance, IStateMachineTarget, BeeForagingMonitor.Def>
{
	// Token: 0x060003F8 RID: 1016 RVA: 0x000217F8 File Offset: 0x0001F9F8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToForage, new StateMachine<BeeForagingMonitor, BeeForagingMonitor.Instance, IStateMachineTarget, BeeForagingMonitor.Def>.Transition.ConditionCallback(BeeForagingMonitor.ShouldForage), delegate(BeeForagingMonitor.Instance smi)
		{
			smi.RefreshSearchTime();
		});
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x0002184C File Offset: 0x0001FA4C
	public static bool ShouldForage(BeeForagingMonitor.Instance smi)
	{
		bool flag = GameClock.Instance.GetTimeInCycles() >= smi.nextSearchTime;
		KPrefabID kprefabID = smi.master.GetComponent<Bee>().FindHiveInRoom();
		if (kprefabID != null)
		{
			BeehiveCalorieMonitor.Instance smi2 = kprefabID.GetSMI<BeehiveCalorieMonitor.Instance>();
			if (smi2 == null || !smi2.IsHungry())
			{
				flag = false;
			}
		}
		return flag && kprefabID != null;
	}

	// Token: 0x020010E5 RID: 4325
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006388 RID: 25480
		public float searchMinInterval = 0.25f;

		// Token: 0x04006389 RID: 25481
		public float searchMaxInterval = 0.3f;
	}

	// Token: 0x020010E6 RID: 4326
	public new class Instance : GameStateMachine<BeeForagingMonitor, BeeForagingMonitor.Instance, IStateMachineTarget, BeeForagingMonitor.Def>.GameInstance
	{
		// Token: 0x0600833B RID: 33595 RVA: 0x00343223 File Offset: 0x00341423
		public Instance(IStateMachineTarget master, BeeForagingMonitor.Def def) : base(master, def)
		{
			this.RefreshSearchTime();
		}

		// Token: 0x0600833C RID: 33596 RVA: 0x00343233 File Offset: 0x00341433
		public void RefreshSearchTime()
		{
			this.nextSearchTime = GameClock.Instance.GetTimeInCycles() + Mathf.Lerp(base.def.searchMinInterval, base.def.searchMaxInterval, UnityEngine.Random.value);
		}

		// Token: 0x0400638A RID: 25482
		public float nextSearchTime;
	}
}
