using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200089B RID: 2203
public class EggProtectionMonitor : GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>
{
	// Token: 0x06003C9A RID: 15514 RVA: 0x00152DE8 File Offset: 0x00150FE8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.find_egg;
		this.find_egg.BatchUpdate(new UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.BatchUpdateDelegate(EggProtectionMonitor.Instance.FindEggToGuard), UpdateRate.SIM_200ms).ParamTransition<bool>(this.hasEggToGuard, this.guard.safe, GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.IsTrue);
		this.guard.Enter(delegate(EggProtectionMonitor.Instance smi)
		{
			smi.gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim("pincher_build_kanim"), smi.def.animPrefix, "_heat", 0);
			smi.gameObject.AddOrGet<FactionAlignment>().SwitchAlignment(FactionManager.FactionID.Hostile);
		}).Exit(delegate(EggProtectionMonitor.Instance smi)
		{
			if (!smi.def.animPrefix.IsNullOrWhiteSpace())
			{
				smi.gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim("pincher_build_kanim"), smi.def.animPrefix, null, 0);
			}
			else
			{
				smi.gameObject.AddOrGet<SymbolOverrideController>().RemoveBuildOverride(Assets.GetAnim("pincher_build_kanim").GetData(), 0);
			}
			smi.gameObject.AddOrGet<FactionAlignment>().SwitchAlignment(FactionManager.FactionID.Pest);
		}).Update("CanProtectEgg", delegate(EggProtectionMonitor.Instance smi, float dt)
		{
			smi.CanProtectEgg();
		}, UpdateRate.SIM_1000ms, true).ParamTransition<bool>(this.hasEggToGuard, this.find_egg, GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.IsFalse);
		this.guard.safe.Enter(delegate(EggProtectionMonitor.Instance smi)
		{
			smi.RefreshThreat(null);
		}).Update("EggProtectionMonitor.safe", delegate(EggProtectionMonitor.Instance smi, float dt)
		{
			smi.RefreshThreat(null);
		}, UpdateRate.SIM_200ms, true).ToggleStatusItem(CREATURES.STATUSITEMS.PROTECTINGENTITY.NAME, CREATURES.STATUSITEMS.PROTECTINGENTITY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, null);
		this.guard.threatened.ToggleBehaviour(GameTags.Creatures.Defend, (EggProtectionMonitor.Instance smi) => smi.threatMonitor.HasThreat(), delegate(EggProtectionMonitor.Instance smi)
		{
			smi.GoTo(this.guard.safe);
		}).Update("Threatened", new Action<EggProtectionMonitor.Instance, float>(EggProtectionMonitor.CritterUpdateThreats), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x06003C9B RID: 15515 RVA: 0x00152FA7 File Offset: 0x001511A7
	private static void CritterUpdateThreats(EggProtectionMonitor.Instance smi, float dt)
	{
		if (smi.isMasterNull)
		{
			return;
		}
		if (!smi.threatMonitor.HasThreat())
		{
			smi.GoTo(smi.sm.guard.safe);
		}
	}

	// Token: 0x04002568 RID: 9576
	private const string BUILD = "pincher_build_kanim";

	// Token: 0x04002569 RID: 9577
	public StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.BoolParameter hasEggToGuard;

	// Token: 0x0400256A RID: 9578
	public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State find_egg;

	// Token: 0x0400256B RID: 9579
	public EggProtectionMonitor.GuardEggStates guard;

	// Token: 0x0200187C RID: 6268
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007B03 RID: 31491
		public Tag[] allyTags;

		// Token: 0x04007B04 RID: 31492
		public string animPrefix;
	}

	// Token: 0x0200187D RID: 6269
	public class GuardEggStates : GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State
	{
		// Token: 0x04007B05 RID: 31493
		public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State safe;

		// Token: 0x04007B06 RID: 31494
		public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State threatened;
	}

	// Token: 0x0200187E RID: 6270
	public new class Instance : GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.GameInstance
	{
		// Token: 0x06009F11 RID: 40721 RVA: 0x003A4CD6 File Offset: 0x003A2ED6
		public Instance(IStateMachineTarget master, EggProtectionMonitor.Def def) : base(master, def)
		{
			this.navigator = master.GetComponent<Navigator>();
			this.refreshThreatDelegate = new Action<object>(this.RefreshThreat);
		}

		// Token: 0x06009F12 RID: 40722 RVA: 0x003A4D00 File Offset: 0x003A2F00
		public void CanProtectEgg()
		{
			bool flag = true;
			if (this.eggToProtect == null)
			{
				flag = false;
			}
			if (flag)
			{
				int num = 150;
				int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(this.eggToProtect));
				if (navigationCost == -1 || navigationCost >= num)
				{
					flag = false;
				}
			}
			if (!flag)
			{
				this.SetEggToGuard(null);
			}
		}

		// Token: 0x06009F13 RID: 40723 RVA: 0x003A4D54 File Offset: 0x003A2F54
		public static void FindEggToGuard(List<UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry> instances, float time_delta)
		{
			ListPool<KPrefabID, EggProtectionMonitor>.PooledList pooledList = ListPool<KPrefabID, EggProtectionMonitor>.Allocate();
			pooledList.Capacity = Mathf.Max(pooledList.Capacity, Components.IncubationMonitors.Count);
			foreach (object obj in Components.IncubationMonitors)
			{
				IncubationMonitor.Instance instance = (IncubationMonitor.Instance)obj;
				pooledList.Add(instance.gameObject.GetComponent<KPrefabID>());
			}
			ListPool<EggProtectionMonitor.Instance.Egg, EggProtectionMonitor>.PooledList pooledList2 = ListPool<EggProtectionMonitor.Instance.Egg, EggProtectionMonitor>.Allocate();
			EggProtectionMonitor.Instance.find_eggs_job.Reset(pooledList);
			for (int i = 0; i < pooledList.Count; i += 256)
			{
				EggProtectionMonitor.Instance.find_eggs_job.Add(new EggProtectionMonitor.Instance.FindEggsTask(i, Mathf.Min(i + 256, pooledList.Count)));
			}
			GlobalJobManager.Run(EggProtectionMonitor.Instance.find_eggs_job);
			for (int num = 0; num != EggProtectionMonitor.Instance.find_eggs_job.Count; num++)
			{
				EggProtectionMonitor.Instance.find_eggs_job.GetWorkItem(num).Finish(pooledList, pooledList2);
			}
			pooledList.Recycle();
			EggProtectionMonitor.Instance.find_eggs_job.Reset(null);
			ListPool<UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry, EggProtectionMonitor>.PooledList pooledList3 = ListPool<UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry, EggProtectionMonitor>.Allocate();
			pooledList3.AddRange(instances);
			foreach (UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry entry in pooledList3)
			{
				GameObject eggToGuard = null;
				int num2 = 100;
				foreach (EggProtectionMonitor.Instance.Egg egg in pooledList2)
				{
					int navigationCost = entry.data.navigator.GetNavigationCost(egg.cell);
					if (navigationCost != -1 && navigationCost < num2)
					{
						eggToGuard = egg.game_object;
						num2 = navigationCost;
					}
				}
				entry.data.SetEggToGuard(eggToGuard);
			}
			pooledList3.Recycle();
			pooledList2.Recycle();
		}

		// Token: 0x06009F14 RID: 40724 RVA: 0x003A4F48 File Offset: 0x003A3148
		public void SetEggToGuard(GameObject egg)
		{
			this.eggToProtect = egg;
			base.sm.hasEggToGuard.Set(egg != null, base.smi, false);
		}

		// Token: 0x06009F15 RID: 40725 RVA: 0x003A4F70 File Offset: 0x003A3170
		public void GoToThreatened()
		{
			base.smi.GoTo(base.sm.guard.threatened);
		}

		// Token: 0x06009F16 RID: 40726 RVA: 0x003A4F90 File Offset: 0x003A3190
		public void RefreshThreat(object data)
		{
			if (!base.IsRunning() || this.eggToProtect == null)
			{
				return;
			}
			if (base.smi.threatMonitor.HasThreat())
			{
				this.GoToThreatened();
				return;
			}
			if (base.smi.GetCurrentState() != base.sm.guard.safe)
			{
				base.Trigger(-21431934, null);
				base.smi.GoTo(base.sm.guard.safe);
			}
		}

		// Token: 0x04007B07 RID: 31495
		[MySmiReq]
		public ThreatMonitor.Instance threatMonitor;

		// Token: 0x04007B08 RID: 31496
		public GameObject eggToProtect;

		// Token: 0x04007B09 RID: 31497
		private Navigator navigator;

		// Token: 0x04007B0A RID: 31498
		private Action<object> refreshThreatDelegate;

		// Token: 0x04007B0B RID: 31499
		private static WorkItemCollection<EggProtectionMonitor.Instance.FindEggsTask, List<KPrefabID>> find_eggs_job = new WorkItemCollection<EggProtectionMonitor.Instance.FindEggsTask, List<KPrefabID>>();

		// Token: 0x02002986 RID: 10630
		private struct Egg
		{
			// Token: 0x0400B7A1 RID: 47009
			public GameObject game_object;

			// Token: 0x0400B7A2 RID: 47010
			public int cell;
		}

		// Token: 0x02002987 RID: 10631
		private struct FindEggsTask : IWorkItem<List<KPrefabID>>
		{
			// Token: 0x0600D157 RID: 53591 RVA: 0x00437C5F File Offset: 0x00435E5F
			public FindEggsTask(int start, int end)
			{
				this.start = start;
				this.end = end;
				this.eggs = ListPool<int, EggProtectionMonitor>.Allocate();
			}

			// Token: 0x0600D158 RID: 53592 RVA: 0x00437C7C File Offset: 0x00435E7C
			public void Run(List<KPrefabID> prefab_ids, int threadIndex)
			{
				for (int num = this.start; num != this.end; num++)
				{
					if (EggProtectionMonitor.Instance.FindEggsTask.EGG_TAG.Contains(prefab_ids[num].PrefabTag))
					{
						this.eggs.Add(num);
					}
				}
			}

			// Token: 0x0600D159 RID: 53593 RVA: 0x00437CC4 File Offset: 0x00435EC4
			public void Finish(List<KPrefabID> prefab_ids, List<EggProtectionMonitor.Instance.Egg> eggs)
			{
				foreach (int index in this.eggs)
				{
					GameObject gameObject = prefab_ids[index].gameObject;
					eggs.Add(new EggProtectionMonitor.Instance.Egg
					{
						game_object = gameObject,
						cell = Grid.PosToCell(gameObject)
					});
				}
				this.eggs.Recycle();
			}

			// Token: 0x0400B7A3 RID: 47011
			private static readonly List<Tag> EGG_TAG = new List<Tag>
			{
				"CrabEgg".ToTag(),
				"CrabWoodEgg".ToTag(),
				"CrabFreshWaterEgg".ToTag()
			};

			// Token: 0x0400B7A4 RID: 47012
			private ListPool<int, EggProtectionMonitor>.PooledList eggs;

			// Token: 0x0400B7A5 RID: 47013
			private int start;

			// Token: 0x0400B7A6 RID: 47014
			private int end;
		}
	}
}
