using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020005C4 RID: 1476
public class MosquitoHungerMonitor : StateMachineComponent<MosquitoHungerMonitor.Instance>
{
	// Token: 0x060021D6 RID: 8662 RVA: 0x000C4B1F File Offset: 0x000C2D1F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x000C4B27 File Offset: 0x000C2D27
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060021D8 RID: 8664 RVA: 0x000C4B3A File Offset: 0x000C2D3A
	private static void ClearTarget(MosquitoHungerMonitor.Instance smi)
	{
		smi.sm.victim.Set(null, smi);
	}

	// Token: 0x060021D9 RID: 8665 RVA: 0x000C4B4E File Offset: 0x000C2D4E
	public static bool IsFed(MosquitoHungerMonitor.Instance smi)
	{
		return smi.IsFed;
	}

	// Token: 0x060021DA RID: 8666 RVA: 0x000C4B56 File Offset: 0x000C2D56
	public static bool HasValidVictim(MosquitoHungerMonitor.Instance smi)
	{
		return MosquitoHungerMonitor.HasValidVictim(smi, smi.Victim);
	}

	// Token: 0x060021DB RID: 8667 RVA: 0x000C4B64 File Offset: 0x000C2D64
	public static bool HasValidVictim(MosquitoHungerMonitor.Instance smi, GameObject victimParam)
	{
		return victimParam != null && !MosquitoHungerMonitor.IsVictimForbidden(smi, victimParam.GetComponent<KPrefabID>(), true);
	}

	// Token: 0x060021DC RID: 8668 RVA: 0x000C4B84 File Offset: 0x000C2D84
	public static void LookForVictim(MosquitoHungerMonitor.Instance smi)
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(smi));
		if (cavityForCell == null)
		{
			return;
		}
		int myWorldId = smi.GetMyWorldId();
		List<KPrefabID> list = new List<KPrefabID>();
		if (smi.master.CanBiteMinions)
		{
			List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(myWorldId, false);
			for (int i = 0; i < worldItems.Count; i++)
			{
				KPrefabID component = worldItems[i].GetComponent<KPrefabID>();
				if (!MosquitoHungerMonitor.IsVictimForbidden(smi, component, true))
				{
					list.Add(component);
				}
			}
		}
		for (int j = 0; j < cavityForCell.creatures.Count; j++)
		{
			KPrefabID kprefabID = cavityForCell.creatures[j];
			if (kprefabID.HasAnyTags(smi.master.AllowedTargetTags) && !MosquitoHungerMonitor.IsVictimForbidden(smi, kprefabID, false))
			{
				list.Add(kprefabID);
			}
		}
		KPrefabID value = (list.Count > 0) ? list.GetRandom<KPrefabID>() : null;
		smi.sm.victim.Set(value, smi);
	}

	// Token: 0x060021DD RID: 8669 RVA: 0x000C4C84 File Offset: 0x000C2E84
	private static bool IsVictimForbidden(MosquitoHungerMonitor.Instance smi, KPrefabID victim, bool mustBeInSameCavity = false)
	{
		int cell = Grid.PosToCell(victim);
		if (mustBeInSameCavity)
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(smi));
			if (Game.Instance.roomProber.GetCavityForCell(cell) != cavityForCell)
			{
				return true;
			}
		}
		if (victim.HasAnyTags(smi.master.ForbiddenTargetTags))
		{
			return true;
		}
		Effects component = victim.GetComponent<Effects>();
		if (component.HasEffect("DupeMosquitoBite") || component.HasEffect("CritterMosquitoBite") || component.HasEffect("DupeMosquitoBiteSuppressed") || component.HasEffect("CritterMosquitoBiteSuppressed"))
		{
			return true;
		}
		OccupyArea component2 = victim.GetComponent<OccupyArea>();
		return !smi.navigator.CanReach(cell, component2.OccupiedCellsOffsets);
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x000C4D38 File Offset: 0x000C2F38
	public static void InitiatePokeBehaviour(MosquitoHungerMonitor.Instance smi)
	{
		PokeMonitor.Instance smi2 = smi.GetSMI<PokeMonitor.Instance>();
		CellOffset[] array = smi.Victim.GetComponent<OccupyArea>().OccupiedCellsOffsets;
		for (int i = 0; i < 1; i++)
		{
			array = array.Expand();
		}
		smi2.InitiatePoke(smi.Victim, array);
	}

	// Token: 0x060021DF RID: 8671 RVA: 0x000C4D80 File Offset: 0x000C2F80
	public static void AbortPokeBehaviour(MosquitoHungerMonitor.Instance smi)
	{
		PokeMonitor.Instance smi2 = smi.GetSMI<PokeMonitor.Instance>();
		if (smi2 != null)
		{
			smi2.AbortPoke();
		}
	}

	// Token: 0x060021E0 RID: 8672 RVA: 0x000C4DA0 File Offset: 0x000C2FA0
	public static void OnVictimPoked(MosquitoHungerMonitor.Instance smi, object victimOBJ)
	{
		if (victimOBJ == null)
		{
			return;
		}
		GameObject gameObject = (GameObject)victimOBJ;
		Effects component = gameObject.GetComponent<Effects>();
		bool flag = gameObject.HasTag(GameTags.BaseMinion);
		bool flag2 = false;
		foreach (string effect_id in MosquitoHungerMonitor.ImmunityEffectNames)
		{
			flag2 = (flag2 || component.HasEffect(effect_id));
		}
		if (flag)
		{
			component.Add(flag2 ? "DupeMosquitoBiteSuppressed" : "DupeMosquitoBite", true);
		}
		else
		{
			component.Add(flag2 ? "CritterMosquitoBiteSuppressed" : "CritterMosquitoBite", true);
		}
		smi.ApplyFedEffect();
	}

	// Token: 0x040013BB RID: 5051
	public const string DupeMosquitoBiteEffectName = "DupeMosquitoBite";

	// Token: 0x040013BC RID: 5052
	public const string CritterMosquitoBiteEffectName = "CritterMosquitoBite";

	// Token: 0x040013BD RID: 5053
	public const string Dupe_SUPPRESSED_MosquitoBiteEffectName = "DupeMosquitoBiteSuppressed";

	// Token: 0x040013BE RID: 5054
	public const string Critter_SUPPRESSED_MosquitoBiteEffectName = "CritterMosquitoBiteSuppressed";

	// Token: 0x040013BF RID: 5055
	public const string MosquitoFedEffectName = "MosquitoFed";

	// Token: 0x040013C0 RID: 5056
	public const int ReachabilityPadding = 1;

	// Token: 0x040013C1 RID: 5057
	public bool CanBiteMinions = true;

	// Token: 0x040013C2 RID: 5058
	public List<Tag> AllowedTargetTags;

	// Token: 0x040013C3 RID: 5059
	public List<Tag> ForbiddenTargetTags;

	// Token: 0x040013C4 RID: 5060
	public static string[] ImmunityEffectNames = new string[]
	{
		"HistamineSuppression"
	};

	// Token: 0x02001482 RID: 5250
	public class States : GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor>
	{
		// Token: 0x0600900D RID: 36877 RVA: 0x0036D674 File Offset: 0x0036B874
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Never;
			default_state = this.satisfied;
			this.satisfied.EventTransition(GameHashes.EffectRemoved, this.hungry, GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.Not(new StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.Transition.ConditionCallback(MosquitoHungerMonitor.IsFed))).Enter(new StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State.Callback(MosquitoHungerMonitor.ClearTarget));
			this.hungry.EventTransition(GameHashes.EffectAdded, this.satisfied, new StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.Transition.ConditionCallback(MosquitoHungerMonitor.IsFed)).DefaultState(this.hungry.lookingForVictim);
			this.hungry.lookingForVictim.ToggleStatusItem(CREATURES.STATUSITEMS.HUNGRY.NAME, CREATURES.STATUSITEMS.HUNGRY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, null).ParamTransition<GameObject>(this.victim, this.hungry.chaseVictim, GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.IsNotNull).PreBrainUpdate(new Action<MosquitoHungerMonitor.Instance>(MosquitoHungerMonitor.LookForVictim));
			this.hungry.chaseVictim.ParamTransition<GameObject>(this.victim, this.hungry.lookingForVictim, GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.IsNull).EventTransition(GameHashes.TargetLost, this.hungry.lookingForVictim, null).Enter(new StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State.Callback(MosquitoHungerMonitor.InitiatePokeBehaviour)).EventHandler(GameHashes.EntityPoked, new GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.GameEvent.Callback(MosquitoHungerMonitor.OnVictimPoked)).Exit(new StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State.Callback(MosquitoHungerMonitor.AbortPokeBehaviour)).Exit(new StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State.Callback(MosquitoHungerMonitor.ClearTarget)).Target(this.victim).EventTransition(GameHashes.TagsChanged, this.hungry.lookingForVictim, GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.Not(new StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.Transition.ConditionCallback(MosquitoHungerMonitor.HasValidVictim))).EventTransition(GameHashes.EffectAdded, this.hungry.lookingForVictim, GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.Not(new StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.Transition.ConditionCallback(MosquitoHungerMonitor.HasValidVictim)));
		}

		// Token: 0x04006ED6 RID: 28374
		public GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State satisfied;

		// Token: 0x04006ED7 RID: 28375
		public MosquitoHungerMonitor.States.HungryStates hungry;

		// Token: 0x04006ED8 RID: 28376
		public StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.TargetParameter victim;

		// Token: 0x0200289E RID: 10398
		public class HungryStates : GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State
		{
			// Token: 0x0400B2FA RID: 45818
			public GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State lookingForVictim;

			// Token: 0x0400B2FB RID: 45819
			public GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State chaseVictim;
		}
	}

	// Token: 0x02001483 RID: 5251
	public class Instance : GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.GameInstance
	{
		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x0600900F RID: 36879 RVA: 0x0036D84B File Offset: 0x0036BA4B
		public GameObject Victim
		{
			get
			{
				return base.sm.victim.Get(this);
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x06009010 RID: 36880 RVA: 0x0036D85E File Offset: 0x0036BA5E
		public bool IsFed
		{
			get
			{
				return this.effects.HasEffect("MosquitoFed");
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x06009012 RID: 36882 RVA: 0x0036D879 File Offset: 0x0036BA79
		// (set) Token: 0x06009011 RID: 36881 RVA: 0x0036D870 File Offset: 0x0036BA70
		public Navigator navigator { get; private set; }

		// Token: 0x06009013 RID: 36883 RVA: 0x0036D881 File Offset: 0x0036BA81
		public Instance(MosquitoHungerMonitor master) : base(master)
		{
			this.effects = base.GetComponent<Effects>();
			this.navigator = base.GetComponent<Navigator>();
		}

		// Token: 0x06009014 RID: 36884 RVA: 0x0036D8A2 File Offset: 0x0036BAA2
		public void ApplyFedEffect()
		{
			this.effects.Add("MosquitoFed", true);
		}

		// Token: 0x04006EDA RID: 28378
		private Effects effects;
	}
}
