using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A52 RID: 2642
public class ThreatMonitor : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>
{
	// Token: 0x06004CE3 RID: 19683 RVA: 0x001BF730 File Offset: 0x001BD930
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.safe;
		this.root.EventHandler(GameHashes.SafeFromThreats, delegate(ThreatMonitor.Instance smi, object d)
		{
			smi.OnSafe(d);
		}).EventHandler(GameHashes.Attacked, delegate(ThreatMonitor.Instance smi, object d)
		{
			smi.OnAttacked(d);
		}).EventHandler(GameHashes.ObjectDestroyed, delegate(ThreatMonitor.Instance smi, object d)
		{
			smi.Cleanup(d);
		});
		this.safe.Enter(delegate(ThreatMonitor.Instance smi)
		{
			smi.revengeThreat.Clear();
		}).Enter(new StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State.Callback(ThreatMonitor.SeekThreats)).EventHandler(GameHashes.FactionChanged, new StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State.Callback(ThreatMonitor.SeekThreats));
		this.safe.passive.DoNothing();
		this.safe.seeking.PreBrainUpdate(delegate(ThreatMonitor.Instance smi)
		{
			smi.RefreshThreat(null);
		});
		this.threatened.duplicant.Transition(this.safe, GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Not(new StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Transition.ConditionCallback(ThreatMonitor.DupeHasValidTarget)), UpdateRate.SIM_200ms);
		this.threatened.duplicant.ShouldFight.ToggleChore(new Func<ThreatMonitor.Instance, Chore>(this.CreateAttackChore), this.safe).Update("DupeUpdateTarget", new Action<ThreatMonitor.Instance, float>(ThreatMonitor.DupeUpdateTarget), UpdateRate.SIM_200ms, false);
		this.threatened.duplicant.ShoudFlee.ToggleChore(new Func<ThreatMonitor.Instance, Chore>(this.CreateFleeChore), this.safe);
		this.threatened.creature.ToggleBehaviour(GameTags.Creatures.Flee, (ThreatMonitor.Instance smi) => !smi.WillFight(), delegate(ThreatMonitor.Instance smi)
		{
			smi.GoTo(this.safe);
		}).ToggleBehaviour(GameTags.Creatures.Attack, (ThreatMonitor.Instance smi) => smi.WillFight(), delegate(ThreatMonitor.Instance smi)
		{
			smi.GoTo(this.safe);
		}).Update("CritterCalmUpdate", new Action<ThreatMonitor.Instance, float>(ThreatMonitor.CritterCalmUpdate), UpdateRate.SIM_200ms, false).PreBrainUpdate(new Action<ThreatMonitor.Instance>(ThreatMonitor.CritterUpdateThreats));
	}

	// Token: 0x06004CE4 RID: 19684 RVA: 0x001BF98C File Offset: 0x001BDB8C
	private static void SeekThreats(ThreatMonitor.Instance smi)
	{
		Faction faction = FactionManager.Instance.GetFaction(smi.alignment.Alignment);
		if (smi.IAmADuplicant || faction.CanAttack)
		{
			smi.GoTo(smi.sm.safe.seeking);
			return;
		}
		smi.GoTo(smi.sm.safe.passive);
	}

	// Token: 0x06004CE5 RID: 19685 RVA: 0x001BF9EC File Offset: 0x001BDBEC
	private static bool DupeHasValidTarget(ThreatMonitor.Instance smi)
	{
		bool result = false;
		if (smi.MainThreat != null && smi.MainThreat.GetComponent<FactionAlignment>().IsPlayerTargeted())
		{
			IApproachable component = smi.MainThreat.GetComponent<RangedAttackable>();
			if (component != null)
			{
				result = (smi.navigator.GetNavigationCost(component) != -1);
			}
		}
		return result;
	}

	// Token: 0x06004CE6 RID: 19686 RVA: 0x001BFA3E File Offset: 0x001BDC3E
	private static void DupeUpdateTarget(ThreatMonitor.Instance smi, float dt)
	{
		if (!ThreatMonitor.DupeHasValidTarget(smi))
		{
			smi.Trigger(2144432245, null);
		}
	}

	// Token: 0x06004CE7 RID: 19687 RVA: 0x001BFA54 File Offset: 0x001BDC54
	private static void CritterCalmUpdate(ThreatMonitor.Instance smi, float dt)
	{
		if (smi.isMasterNull)
		{
			return;
		}
		if (smi.revengeThreat.target != null && smi.revengeThreat.Calm(dt, smi.alignment))
		{
			smi.Trigger(-21431934, null);
		}
	}

	// Token: 0x06004CE8 RID: 19688 RVA: 0x001BFA92 File Offset: 0x001BDC92
	private static void CritterUpdateThreats(ThreatMonitor.Instance smi)
	{
		if (smi.isMasterNull)
		{
			return;
		}
		if (!smi.CheckForThreats() && !ThreatMonitor.IsInSafeState(smi))
		{
			smi.GoTo(smi.sm.safe);
		}
	}

	// Token: 0x06004CE9 RID: 19689 RVA: 0x001BFABE File Offset: 0x001BDCBE
	private static bool IsInSafeState(ThreatMonitor.Instance smi)
	{
		return smi.GetCurrentState() == smi.sm.safe.passive || smi.GetCurrentState() == smi.sm.safe.seeking;
	}

	// Token: 0x06004CEA RID: 19690 RVA: 0x001BFAF2 File Offset: 0x001BDCF2
	private Chore CreateAttackChore(ThreatMonitor.Instance smi)
	{
		return new AttackChore(smi.master, smi.MainThreat);
	}

	// Token: 0x06004CEB RID: 19691 RVA: 0x001BFB05 File Offset: 0x001BDD05
	private Chore CreateFleeChore(ThreatMonitor.Instance smi)
	{
		return new FleeChore(smi.master, smi.MainThreat);
	}

	// Token: 0x04003341 RID: 13121
	public ThreatMonitor.SafeStates safe;

	// Token: 0x04003342 RID: 13122
	public ThreatMonitor.ThreatenedStates threatened;

	// Token: 0x02001B57 RID: 6999
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04008496 RID: 33942
		public Health.HealthState fleethresholdState = Health.HealthState.Injured;

		// Token: 0x04008497 RID: 33943
		public Tag[] friendlyCreatureTags;

		// Token: 0x04008498 RID: 33944
		public int maxSearchEntities = 50;

		// Token: 0x04008499 RID: 33945
		public int maxSearchDistance = 20;

		// Token: 0x0400849A RID: 33946
		public CellOffset[] offsets = OffsetGroups.Use;
	}

	// Token: 0x02001B58 RID: 7000
	public class SafeStates : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
	{
		// Token: 0x0400849B RID: 33947
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State passive;

		// Token: 0x0400849C RID: 33948
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State seeking;
	}

	// Token: 0x02001B59 RID: 7001
	public class ThreatenedStates : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
	{
		// Token: 0x0400849D RID: 33949
		public ThreatMonitor.ThreatenedDuplicantStates duplicant;

		// Token: 0x0400849E RID: 33950
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State creature;
	}

	// Token: 0x02001B5A RID: 7002
	public class ThreatenedDuplicantStates : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
	{
		// Token: 0x0400849F RID: 33951
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State ShoudFlee;

		// Token: 0x040084A0 RID: 33952
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State ShouldFight;
	}

	// Token: 0x02001B5B RID: 7003
	public struct Grudge
	{
		// Token: 0x0600A984 RID: 43396 RVA: 0x003C1840 File Offset: 0x003BFA40
		public void Reset(FactionAlignment revengeTarget)
		{
			this.target = revengeTarget;
			float num = 10f;
			this.grudgeTime = num;
		}

		// Token: 0x0600A985 RID: 43397 RVA: 0x003C1864 File Offset: 0x003BFA64
		public bool Calm(float dt, FactionAlignment self)
		{
			if (this.grudgeTime <= 0f)
			{
				return true;
			}
			this.grudgeTime = Mathf.Max(0f, this.grudgeTime - dt);
			if (this.grudgeTime == 0f)
			{
				if (FactionManager.Instance.GetDisposition(self.Alignment, this.target.Alignment) != FactionManager.Disposition.Attack)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, UI.GAMEOBJECTEFFECTS.FORGAVEATTACKER, self.transform, 2f, true);
				}
				this.Clear();
				return true;
			}
			return false;
		}

		// Token: 0x0600A986 RID: 43398 RVA: 0x003C18F7 File Offset: 0x003BFAF7
		public void Clear()
		{
			this.grudgeTime = 0f;
			this.target = null;
		}

		// Token: 0x0600A987 RID: 43399 RVA: 0x003C190C File Offset: 0x003BFB0C
		public bool IsValidRevengeTarget(bool isDuplicant)
		{
			return this.target != null && this.target.IsAlignmentActive() && (this.target.health == null || !this.target.health.IsDefeated()) && (!isDuplicant || !this.target.IsPlayerTargeted());
		}

		// Token: 0x040084A1 RID: 33953
		public FactionAlignment target;

		// Token: 0x040084A2 RID: 33954
		public float grudgeTime;
	}

	// Token: 0x02001B5C RID: 7004
	public new class Instance : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.GameInstance
	{
		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x0600A988 RID: 43400 RVA: 0x003C196E File Offset: 0x003BFB6E
		public GameObject MainThreat
		{
			get
			{
				return this.mainThreat;
			}
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x0600A989 RID: 43401 RVA: 0x003C1976 File Offset: 0x003BFB76
		public bool IAmADuplicant
		{
			get
			{
				return this.alignment.Alignment == FactionManager.FactionID.Duplicant;
			}
		}

		// Token: 0x0600A98A RID: 43402 RVA: 0x003C1988 File Offset: 0x003BFB88
		public Instance(IStateMachineTarget master, ThreatMonitor.Def def) : base(master, def)
		{
			this.alignment = master.GetComponent<FactionAlignment>();
			this.navigator = master.GetComponent<Navigator>();
			this.choreDriver = master.GetComponent<ChoreDriver>();
			this.health = master.GetComponent<Health>();
			this.choreConsumer = master.GetComponent<ChoreConsumer>();
			this.refreshThreatDelegate = new Action<object>(this.RefreshThreat);
		}

		// Token: 0x0600A98B RID: 43403 RVA: 0x003C19F6 File Offset: 0x003BFBF6
		public void ClearMainThreat()
		{
			this.SetMainThreat(null);
		}

		// Token: 0x0600A98C RID: 43404 RVA: 0x003C1A00 File Offset: 0x003BFC00
		public void SetMainThreat(GameObject threat)
		{
			if (threat == this.mainThreat)
			{
				return;
			}
			if (this.mainThreat != null)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
				if (threat == null)
				{
					base.Trigger(2144432245, null);
				}
			}
			if (this.mainThreat != null)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
			}
			this.mainThreat = threat;
			if (this.mainThreat != null)
			{
				this.mainThreatFaction = this.mainThreat.GetComponent<FactionAlignment>().Alignment;
				this.mainThreat.Subscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Subscribe(1969584890, this.refreshThreatDelegate);
			}
		}

		// Token: 0x0600A98D RID: 43405 RVA: 0x003C1AFE File Offset: 0x003BFCFE
		public bool HasThreat()
		{
			return this.MainThreat != null;
		}

		// Token: 0x0600A98E RID: 43406 RVA: 0x003C1B0C File Offset: 0x003BFD0C
		public void OnSafe(object data)
		{
			if (this.revengeThreat.target != null)
			{
				if (!this.revengeThreat.target.GetComponent<FactionAlignment>().IsAlignmentActive())
				{
					this.revengeThreat.Clear();
				}
				this.ClearMainThreat();
			}
		}

		// Token: 0x0600A98F RID: 43407 RVA: 0x003C1B4C File Offset: 0x003BFD4C
		public void OnAttacked(object data)
		{
			FactionAlignment factionAlignment = (FactionAlignment)data;
			this.revengeThreat.Reset(factionAlignment);
			Game.BrainScheduler.PrioritizeBrain(base.GetComponent<Brain>());
			if (this.mainThreat == null)
			{
				this.SetMainThreat(factionAlignment.gameObject);
				this.GoToThreatened();
			}
			else if (!this.WillFight())
			{
				this.GoToThreatened();
			}
			if (factionAlignment.GetComponent<Bee>())
			{
				Chore chore = (this.choreDriver != null) ? this.choreDriver.GetCurrentChore() : null;
				if (chore != null && chore.gameObject.GetComponent<HiveWorkableEmpty>() != null)
				{
					chore.gameObject.GetComponent<HiveWorkableEmpty>().wasStung = true;
				}
			}
		}

		// Token: 0x0600A990 RID: 43408 RVA: 0x003C1C00 File Offset: 0x003BFE00
		public bool WillFight()
		{
			if (this.choreConsumer != null)
			{
				if (!this.choreConsumer.IsPermittedByUser(Db.Get().ChoreGroups.Combat))
				{
					return false;
				}
				if (!this.choreConsumer.IsPermittedByTraits(Db.Get().ChoreGroups.Combat))
				{
					return false;
				}
			}
			return (this.IAmADuplicant || base.smi.mainThreatFaction != FactionManager.FactionID.Predator) && this.health.State < base.smi.def.fleethresholdState;
		}

		// Token: 0x0600A991 RID: 43409 RVA: 0x003C1C94 File Offset: 0x003BFE94
		private void GotoThreatResponse()
		{
			Chore currentChore = base.smi.master.GetComponent<ChoreDriver>().GetCurrentChore();
			if (this.WillFight() && this.mainThreat.GetComponent<FactionAlignment>().IsPlayerTargeted())
			{
				base.smi.GoTo(base.smi.sm.threatened.duplicant.ShouldFight);
				return;
			}
			if (currentChore != null && currentChore.target != null && currentChore.target != base.master && currentChore.target.GetComponent<Pickupable>() != null)
			{
				return;
			}
			base.smi.GoTo(base.smi.sm.threatened.duplicant.ShoudFlee);
		}

		// Token: 0x0600A992 RID: 43410 RVA: 0x003C1D49 File Offset: 0x003BFF49
		public void GoToThreatened()
		{
			if (this.IAmADuplicant)
			{
				this.GotoThreatResponse();
				return;
			}
			base.smi.GoTo(base.sm.threatened.creature);
		}

		// Token: 0x0600A993 RID: 43411 RVA: 0x003C1D75 File Offset: 0x003BFF75
		public void Cleanup(object data)
		{
			if (this.mainThreat)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
			}
		}

		// Token: 0x0600A994 RID: 43412 RVA: 0x003C1DB0 File Offset: 0x003BFFB0
		public void RefreshThreat(object data)
		{
			if (!base.IsRunning())
			{
				return;
			}
			if (base.smi.CheckForThreats())
			{
				this.GoToThreatened();
				return;
			}
			if (!ThreatMonitor.IsInSafeState(base.smi))
			{
				base.Trigger(-21431934, null);
				base.smi.GoTo(base.sm.safe);
			}
		}

		// Token: 0x0600A995 RID: 43413 RVA: 0x003C1E0C File Offset: 0x003C000C
		public bool CheckForThreats()
		{
			if (base.isMasterNull)
			{
				return false;
			}
			GameObject x;
			if (this.revengeThreat.IsValidRevengeTarget(this.IAmADuplicant))
			{
				x = this.revengeThreat.target.gameObject;
			}
			else if (this.IAmADuplicant)
			{
				x = this.FindThreatDuplicant();
			}
			else
			{
				x = this.FindThreatOther();
			}
			this.SetMainThreat(x);
			return x != null;
		}

		// Token: 0x0600A996 RID: 43414 RVA: 0x003C1E70 File Offset: 0x003C0070
		private GameObject FindThreatDuplicant()
		{
			this.threats.Clear();
			if (this.WillFight())
			{
				foreach (object obj in Components.PlayerTargeted)
				{
					FactionAlignment factionAlignment = (FactionAlignment)obj;
					if (!factionAlignment.IsNullOrDestroyed() && factionAlignment.IsPlayerTargeted() && !factionAlignment.health.IsDefeated() && this.navigator.CanReach(factionAlignment.attackable.GetCell(), base.smi.def.offsets))
					{
						this.threats.Add(factionAlignment);
					}
				}
			}
			return this.PickBestTarget(this.threats);
		}

		// Token: 0x0600A997 RID: 43415 RVA: 0x003C1F34 File Offset: 0x003C0134
		private GameObject FindThreatOther()
		{
			this.threats.Clear();
			this.GatherThreats();
			return this.PickBestTarget(this.threats);
		}

		// Token: 0x0600A998 RID: 43416 RVA: 0x003C1F53 File Offset: 0x003C0153
		private static Util.IterationInstruction collectFactionAlignments(object obj, List<FactionAlignment> alignments)
		{
			alignments.Add(obj as FactionAlignment);
			return Util.IterationInstruction.Continue;
		}

		// Token: 0x0600A999 RID: 43417 RVA: 0x003C1F64 File Offset: 0x003C0164
		private void GatherThreats()
		{
			ListPool<FactionAlignment, ThreatMonitor>.PooledList pooledList = ListPool<FactionAlignment, ThreatMonitor>.Allocate();
			Extents extents = new Extents(Grid.PosToCell(base.gameObject), base.def.maxSearchDistance);
			GameScenePartitioner.Instance.VisitEntries<ListPool<FactionAlignment, ThreatMonitor>.PooledList>(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.attackableEntitiesLayer, new Func<object, ListPool<FactionAlignment, ThreatMonitor>.PooledList, Util.IterationInstruction>(ThreatMonitor.Instance.collectFactionAlignments), pooledList);
			int count = pooledList.Count;
			int num = Mathf.Min(count, base.def.maxSearchEntities);
			for (int i = 0; i < num; i++)
			{
				if (this.currentUpdateIndex >= count)
				{
					this.currentUpdateIndex = 0;
				}
				FactionAlignment factionAlignment = pooledList[this.currentUpdateIndex];
				this.currentUpdateIndex++;
				if (!(factionAlignment.transform == null) && !(factionAlignment == this.alignment) && (base.def.friendlyCreatureTags == null || !factionAlignment.kprefabID.HasAnyTags(base.def.friendlyCreatureTags)) && factionAlignment.IsAlignmentActive() && FactionManager.Instance.GetDisposition(this.alignment.Alignment, factionAlignment.Alignment) == FactionManager.Disposition.Attack && this.navigator.CanReach(factionAlignment.attackable.GetCell(), base.smi.def.offsets))
				{
					this.threats.Add(factionAlignment);
				}
			}
			pooledList.Recycle();
		}

		// Token: 0x0600A99A RID: 43418 RVA: 0x003C20D8 File Offset: 0x003C02D8
		public GameObject PickBestTarget(List<FactionAlignment> threats)
		{
			float num = 1f;
			Vector2 a = base.gameObject.transform.GetPosition();
			GameObject result = null;
			float num2 = float.PositiveInfinity;
			for (int i = threats.Count - 1; i >= 0; i--)
			{
				FactionAlignment factionAlignment = threats[i];
				float num3 = Vector2.Distance(a, factionAlignment.transform.GetPosition()) / num;
				if (num3 < num2)
				{
					num2 = num3;
					result = factionAlignment.gameObject;
				}
			}
			return result;
		}

		// Token: 0x040084A3 RID: 33955
		public FactionAlignment alignment;

		// Token: 0x040084A4 RID: 33956
		public Navigator navigator;

		// Token: 0x040084A5 RID: 33957
		public ChoreDriver choreDriver;

		// Token: 0x040084A6 RID: 33958
		private Health health;

		// Token: 0x040084A7 RID: 33959
		private ChoreConsumer choreConsumer;

		// Token: 0x040084A8 RID: 33960
		public ThreatMonitor.Grudge revengeThreat;

		// Token: 0x040084A9 RID: 33961
		public int currentUpdateIndex;

		// Token: 0x040084AA RID: 33962
		private GameObject mainThreat;

		// Token: 0x040084AB RID: 33963
		private FactionManager.FactionID mainThreatFaction;

		// Token: 0x040084AC RID: 33964
		private List<FactionAlignment> threats = new List<FactionAlignment>();

		// Token: 0x040084AD RID: 33965
		private Action<object> refreshThreatDelegate;
	}
}
