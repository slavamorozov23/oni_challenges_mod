using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020005C6 RID: 1478
public class PollinateMonitor : GameStateMachine<PollinateMonitor, PollinateMonitor.Instance, IStateMachineTarget, PollinateMonitor.Def>
{
	// Token: 0x060021E6 RID: 8678 RVA: 0x000C4E78 File Offset: 0x000C3078
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.lookingForPlant;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.lookingForPlant.PreBrainUpdate(new Action<PollinateMonitor.Instance>(PollinateMonitor.FindPollinateTarget)).ToggleBehaviour(GameTags.Creatures.WantsToPollinate, (PollinateMonitor.Instance smi) => smi.IsValidTarget(), delegate(PollinateMonitor.Instance smi)
		{
			smi.GoTo(this.satisfied);
		});
		this.satisfied.Enter(delegate(PollinateMonitor.Instance smi)
		{
			this.remainingSecondsForEffect.Set(ButterflyTuning.SEARCH_COOLDOWN, smi, false);
		}).ScheduleGoTo((PollinateMonitor.Instance smi) => this.remainingSecondsForEffect.Get(smi), this.lookingForPlant);
	}

	// Token: 0x060021E7 RID: 8679 RVA: 0x000C4F10 File Offset: 0x000C3110
	private static void FindPollinateTarget(PollinateMonitor.Instance smi)
	{
		if (smi.IsValidTarget())
		{
			return;
		}
		KPrefabID kprefabID = smi.def.PlantSeeker.Scan(Grid.PosToXY(smi.transform.GetPosition()), smi.navigator);
		GameObject gameObject = (kprefabID != null) ? kprefabID.gameObject : null;
		if (gameObject != smi.target)
		{
			if (gameObject == null)
			{
				smi.target = null;
				smi.targetCell = -1;
			}
			else
			{
				smi.target = gameObject;
				smi.targetCell = Grid.PosToCell(smi.target);
			}
			smi.Trigger(-255880159, null);
		}
	}

	// Token: 0x040013C5 RID: 5061
	public static Tag ID = new Tag("PollinateMonitor");

	// Token: 0x040013C6 RID: 5062
	public GameStateMachine<PollinateMonitor, PollinateMonitor.Instance, IStateMachineTarget, PollinateMonitor.Def>.State lookingForPlant;

	// Token: 0x040013C7 RID: 5063
	public GameStateMachine<PollinateMonitor, PollinateMonitor.Instance, IStateMachineTarget, PollinateMonitor.Def>.State satisfied;

	// Token: 0x040013C8 RID: 5064
	private StateMachine<PollinateMonitor, PollinateMonitor.Instance, IStateMachineTarget, PollinateMonitor.Def>.FloatParameter remainingSecondsForEffect;

	// Token: 0x02001486 RID: 5254
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x0600901C RID: 36892 RVA: 0x0036D9EC File Offset: 0x0036BBEC
		public Navigator.Scanner<KPrefabID> PlantSeeker
		{
			get
			{
				if (this.plantSeeker == null)
				{
					this.plantSeeker = new Navigator.Scanner<KPrefabID>(this.radius, GameScenePartitioner.Instance.plants, new Func<KPrefabID, bool>(PollinateMonitor.Def.IsHarvestablePlant));
					this.plantSeeker.SetEarlyOutThreshold(5);
				}
				return this.plantSeeker;
			}
		}

		// Token: 0x0600901D RID: 36893 RVA: 0x0036DA3C File Offset: 0x0036BC3C
		private static bool IsHarvestablePlant(KPrefabID plant)
		{
			if (plant == null)
			{
				return false;
			}
			if (plant.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				return false;
			}
			if (plant.HasTag("ButterflyPlant"))
			{
				return false;
			}
			if (!plant.HasTag(GameTags.GrowingPlant))
			{
				return false;
			}
			if (plant.HasTag(GameTags.FullyGrown))
			{
				return false;
			}
			Effects component = plant.GetComponent<Effects>();
			if (component == null)
			{
				return false;
			}
			for (int i = 0; i < PollinationMonitor.PollinationEffects.Length; i++)
			{
				HashedString effect_id = PollinationMonitor.PollinationEffects[i];
				if (component.HasEffect(effect_id))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04006EDF RID: 28383
		public int radius = 10;

		// Token: 0x04006EE0 RID: 28384
		private Navigator.Scanner<KPrefabID> plantSeeker;
	}

	// Token: 0x02001487 RID: 5255
	public new class Instance : GameStateMachine<PollinateMonitor, PollinateMonitor.Instance, IStateMachineTarget, PollinateMonitor.Def>.GameInstance, IApproachableBehaviour, ICreatureMonitor
	{
		// Token: 0x0600901F RID: 36895 RVA: 0x0036DAE1 File Offset: 0x0036BCE1
		public Instance(IStateMachineTarget master, PollinateMonitor.Def def) : base(master, def)
		{
			this.navigator = master.GetComponent<Navigator>();
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06009020 RID: 36896 RVA: 0x0036DAF7 File Offset: 0x0036BCF7
		public Tag Id
		{
			get
			{
				return PollinateMonitor.ID;
			}
		}

		// Token: 0x06009021 RID: 36897 RVA: 0x0036DAFE File Offset: 0x0036BCFE
		public bool IsValidTarget()
		{
			return !this.target.IsNullOrDestroyed() && this.navigator.GetNavigationCost(this.targetCell) != -1;
		}

		// Token: 0x06009022 RID: 36898 RVA: 0x0036DB26 File Offset: 0x0036BD26
		public GameObject GetTarget()
		{
			return this.target;
		}

		// Token: 0x06009023 RID: 36899 RVA: 0x0036DB2E File Offset: 0x0036BD2E
		public StatusItem GetApproachStatusItem()
		{
			return Db.Get().CreatureStatusItems.TravelingToPollinate;
		}

		// Token: 0x06009024 RID: 36900 RVA: 0x0036DB3F File Offset: 0x0036BD3F
		public StatusItem GetBehaviourStatusItem()
		{
			return Db.Get().CreatureStatusItems.Pollinating;
		}

		// Token: 0x06009025 RID: 36901 RVA: 0x0036DB50 File Offset: 0x0036BD50
		public void OnSuccess()
		{
			Effects component = this.target.GetComponent<Effects>();
			if (component != null)
			{
				component.Add(Db.Get().effects.Get("ButterflyPollinated"), true);
			}
			this.target = null;
		}

		// Token: 0x04006EE1 RID: 28385
		public GameObject target;

		// Token: 0x04006EE2 RID: 28386
		public int targetCell;

		// Token: 0x04006EE3 RID: 28387
		public Navigator navigator;
	}
}
