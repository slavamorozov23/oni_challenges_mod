using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B18 RID: 2840
public class RocketUsageRestriction : GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>
{
	// Token: 0x060052B5 RID: 21173 RVA: 0x001E19FC File Offset: 0x001DFBFC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(delegate(RocketUsageRestriction.StatesInstance smi)
		{
			if (DlcManager.FeatureClusterSpaceEnabled() && smi.master.gameObject.GetMyWorld().IsModuleInterior)
			{
				smi.Subscribe(493375141, new Action<object>(smi.OnRefreshUserMenu));
				smi.GoToRestrictionState();
				return;
			}
			smi.StopSM("Not inside rocket or no cluster space");
		});
		this.restriction.Enter(new StateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State.Callback(this.AquireRocketControlStation)).Enter(delegate(RocketUsageRestriction.StatesInstance smi)
		{
			Components.RocketControlStations.OnAdd += new Action<RocketControlStation>(smi.ControlStationBuilt);
		}).Exit(delegate(RocketUsageRestriction.StatesInstance smi)
		{
			Components.RocketControlStations.OnAdd -= new Action<RocketControlStation>(smi.ControlStationBuilt);
		});
		this.restriction.uncontrolled.ToggleStatusItem(Db.Get().BuildingStatusItems.NoRocketRestriction, null).Enter(delegate(RocketUsageRestriction.StatesInstance smi)
		{
			this.RestrictUsage(smi, false);
		});
		this.restriction.controlled.DefaultState(this.restriction.controlled.nostation);
		this.restriction.controlled.nostation.Enter(new StateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State.Callback(this.OnRocketRestrictionChanged)).ParamTransition<GameObject>(this.rocketControlStation, this.restriction.controlled.controlled, GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.IsNotNull);
		this.restriction.controlled.controlled.OnTargetLost(this.rocketControlStation, this.restriction.controlled.nostation).Enter(new StateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State.Callback(this.OnRocketRestrictionChanged)).Target(this.rocketControlStation).EventHandler(GameHashes.RocketRestrictionChanged, new StateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State.Callback(this.OnRocketRestrictionChanged)).Target(this.masterTarget);
	}

	// Token: 0x060052B6 RID: 21174 RVA: 0x001E1BA1 File Offset: 0x001DFDA1
	private void OnRocketRestrictionChanged(RocketUsageRestriction.StatesInstance smi)
	{
		this.RestrictUsage(smi, smi.BuildingRestrictionsActive());
	}

	// Token: 0x060052B7 RID: 21175 RVA: 0x001E1BB0 File Offset: 0x001DFDB0
	private void RestrictUsage(RocketUsageRestriction.StatesInstance smi, bool restrict)
	{
		smi.master.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.RocketRestrictionInactive, !restrict && smi.isControlled, null);
		if (smi.isRestrictionApplied == restrict)
		{
			return;
		}
		smi.isRestrictionApplied = restrict;
		smi.operational.SetFlag(RocketUsageRestriction.rocketUsageAllowed, !smi.def.restrictOperational || !restrict);
		smi.master.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.RocketRestrictionActive, restrict, null);
		Storage[] components = smi.master.gameObject.GetComponents<Storage>();
		if (components != null && components.Length != 0)
		{
			for (int i = 0; i < components.Length; i++)
			{
				if (restrict)
				{
					smi.previousStorageAllowItemRemovalStates = new bool[components.Length];
					smi.previousStorageAllowItemRemovalStates[i] = components[i].allowItemRemoval;
					components[i].allowItemRemoval = false;
				}
				else if (smi.previousStorageAllowItemRemovalStates != null && i < smi.previousStorageAllowItemRemovalStates.Length)
				{
					components[i].allowItemRemoval = smi.previousStorageAllowItemRemovalStates[i];
				}
				foreach (GameObject go in components[i].items)
				{
					go.Trigger(-778359855, components[i]);
				}
			}
		}
		Ownable component = smi.master.GetComponent<Ownable>();
		if (restrict && component != null && component.IsAssigned())
		{
			component.Unassign();
		}
	}

	// Token: 0x060052B8 RID: 21176 RVA: 0x001E1D38 File Offset: 0x001DFF38
	private void AquireRocketControlStation(RocketUsageRestriction.StatesInstance smi)
	{
		if (!this.rocketControlStation.IsNull(smi))
		{
			return;
		}
		foreach (object obj in Components.RocketControlStations)
		{
			RocketControlStation rocketControlStation = (RocketControlStation)obj;
			if (rocketControlStation.GetMyWorldId() == smi.GetMyWorldId())
			{
				this.rocketControlStation.Set(rocketControlStation, smi);
			}
		}
	}

	// Token: 0x040037E9 RID: 14313
	public static readonly Operational.Flag rocketUsageAllowed = new Operational.Flag("rocketUsageAllowed", Operational.Flag.Type.Requirement);

	// Token: 0x040037EA RID: 14314
	private StateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.TargetParameter rocketControlStation;

	// Token: 0x040037EB RID: 14315
	public RocketUsageRestriction.RestrictionStates restriction;

	// Token: 0x02001C5E RID: 7262
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600AD3A RID: 44346 RVA: 0x003CEF0B File Offset: 0x003CD10B
		public override void Configure(GameObject prefab)
		{
			RocketControlStation.CONTROLLED_BUILDINGS.Add(prefab.PrefabID());
		}

		// Token: 0x040087C8 RID: 34760
		public bool initialControlledStateWhenBuilt = true;

		// Token: 0x040087C9 RID: 34761
		public bool restrictOperational = true;
	}

	// Token: 0x02001C5F RID: 7263
	public class ControlledStates : GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State
	{
		// Token: 0x040087CA RID: 34762
		public GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State nostation;

		// Token: 0x040087CB RID: 34763
		public GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State controlled;
	}

	// Token: 0x02001C60 RID: 7264
	public class RestrictionStates : GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State
	{
		// Token: 0x040087CC RID: 34764
		public GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State uncontrolled;

		// Token: 0x040087CD RID: 34765
		public RocketUsageRestriction.ControlledStates controlled;
	}

	// Token: 0x02001C61 RID: 7265
	public class StatesInstance : GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.GameInstance
	{
		// Token: 0x0600AD3E RID: 44350 RVA: 0x003CEF43 File Offset: 0x003CD143
		public StatesInstance(IStateMachineTarget master, RocketUsageRestriction.Def def) : base(master, def)
		{
			this.isControlled = def.initialControlledStateWhenBuilt;
		}

		// Token: 0x0600AD3F RID: 44351 RVA: 0x003CEF60 File Offset: 0x003CD160
		public void OnRefreshUserMenu(object data)
		{
			KIconButtonMenu.ButtonInfo button;
			if (this.isControlled)
			{
				button = new KIconButtonMenu.ButtonInfo("action_rocket_restriction_uncontrolled", UI.USERMENUACTIONS.ROCKETUSAGERESTRICTION.NAME_UNCONTROLLED, new System.Action(this.OnChange), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ROCKETUSAGERESTRICTION.TOOLTIP_UNCONTROLLED, true);
			}
			else
			{
				button = new KIconButtonMenu.ButtonInfo("action_rocket_restriction_controlled", UI.USERMENUACTIONS.ROCKETUSAGERESTRICTION.NAME_CONTROLLED, new System.Action(this.OnChange), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ROCKETUSAGERESTRICTION.TOOLTIP_CONTROLLED, true);
			}
			Game.Instance.userMenu.AddButton(base.gameObject, button, 11f);
		}

		// Token: 0x0600AD40 RID: 44352 RVA: 0x003CEFFC File Offset: 0x003CD1FC
		public void ControlStationBuilt(object o)
		{
			base.sm.AquireRocketControlStation(base.smi);
		}

		// Token: 0x0600AD41 RID: 44353 RVA: 0x003CF00F File Offset: 0x003CD20F
		private void OnChange()
		{
			this.isControlled = !this.isControlled;
			this.GoToRestrictionState();
		}

		// Token: 0x0600AD42 RID: 44354 RVA: 0x003CF028 File Offset: 0x003CD228
		public void GoToRestrictionState()
		{
			if (base.smi.isControlled)
			{
				base.smi.GoTo(base.sm.restriction.controlled);
				return;
			}
			base.smi.GoTo(base.sm.restriction.uncontrolled);
		}

		// Token: 0x0600AD43 RID: 44355 RVA: 0x003CF079 File Offset: 0x003CD279
		public bool BuildingRestrictionsActive()
		{
			return this.isControlled && !base.sm.rocketControlStation.IsNull(base.smi) && base.sm.rocketControlStation.Get<RocketControlStation>(base.smi).BuildingRestrictionsActive;
		}

		// Token: 0x040087CE RID: 34766
		[MyCmpGet]
		public Operational operational;

		// Token: 0x040087CF RID: 34767
		public bool[] previousStorageAllowItemRemovalStates;

		// Token: 0x040087D0 RID: 34768
		[Serialize]
		public bool isControlled = true;

		// Token: 0x040087D1 RID: 34769
		public bool isRestrictionApplied;
	}
}
