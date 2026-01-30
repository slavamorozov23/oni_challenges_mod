using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000C13 RID: 3091
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Vent")]
public class Vent : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x170006B2 RID: 1714
	// (get) Token: 0x06005CF3 RID: 23795 RVA: 0x0021A583 File Offset: 0x00218783
	// (set) Token: 0x06005CF4 RID: 23796 RVA: 0x0021A58B File Offset: 0x0021878B
	public int SortKey
	{
		get
		{
			return this.sortKey;
		}
		set
		{
			this.sortKey = value;
		}
	}

	// Token: 0x06005CF5 RID: 23797 RVA: 0x0021A594 File Offset: 0x00218794
	public void UpdateVentedMass(SimHashes element, float mass)
	{
		if (!this.lifeTimeVentMass.ContainsKey(element))
		{
			this.lifeTimeVentMass.Add(element, mass);
			return;
		}
		Dictionary<SimHashes, float> dictionary = this.lifeTimeVentMass;
		dictionary[element] += mass;
	}

	// Token: 0x06005CF6 RID: 23798 RVA: 0x0021A5D6 File Offset: 0x002187D6
	public float GetVentedMass(SimHashes element)
	{
		if (this.lifeTimeVentMass.ContainsKey(element))
		{
			return this.lifeTimeVentMass[element];
		}
		return 0f;
	}

	// Token: 0x06005CF7 RID: 23799 RVA: 0x0021A5F8 File Offset: 0x002187F8
	public bool Closed()
	{
		bool flag = false;
		return (this.operational.Flags.TryGetValue(LogicOperationalController.LogicOperationalFlag, out flag) && !flag) || (this.operational.Flags.TryGetValue(BuildingEnabledButton.EnabledFlag, out flag) && !flag);
	}

	// Token: 0x06005CF8 RID: 23800 RVA: 0x0021A644 File Offset: 0x00218844
	protected override void OnSpawn()
	{
		Building component = base.GetComponent<Building>();
		this.cell = component.GetUtilityOutputCell();
		this.smi = new Vent.StatesInstance(this);
		this.smi.StartSM();
	}

	// Token: 0x06005CF9 RID: 23801 RVA: 0x0021A67C File Offset: 0x0021887C
	public Vent.State GetEndPointState()
	{
		Vent.State result = Vent.State.Invalid;
		Endpoint endpoint = this.endpointType;
		if (endpoint != Endpoint.Source)
		{
			if (endpoint == Endpoint.Sink)
			{
				result = Vent.State.Ready;
				int num = this.cell;
				if (!this.IsValidOutputCell(num))
				{
					result = (Grid.Solid[num] ? Vent.State.Blocked : Vent.State.OverPressure);
				}
			}
		}
		else
		{
			result = (this.IsConnected() ? Vent.State.Ready : Vent.State.Blocked);
		}
		return result;
	}

	// Token: 0x06005CFA RID: 23802 RVA: 0x0021A6D0 File Offset: 0x002188D0
	public bool IsConnected()
	{
		UtilityNetwork networkForCell = Conduit.GetNetworkManager(this.conduitType).GetNetworkForCell(this.cell);
		return networkForCell != null && (networkForCell as FlowUtilityNetwork).HasSinks;
	}

	// Token: 0x170006B3 RID: 1715
	// (get) Token: 0x06005CFB RID: 23803 RVA: 0x0021A704 File Offset: 0x00218904
	public bool IsBlocked
	{
		get
		{
			return this.GetEndPointState() != Vent.State.Ready;
		}
	}

	// Token: 0x06005CFC RID: 23804 RVA: 0x0021A714 File Offset: 0x00218914
	private bool IsValidOutputCell(int output_cell)
	{
		bool result = false;
		if ((this.structure == null || !this.structure.IsEntombed() || !this.Closed()) && !Grid.Solid[output_cell])
		{
			result = (Grid.Mass[output_cell] < this.overpressureMass);
		}
		return result;
	}

	// Token: 0x06005CFD RID: 23805 RVA: 0x0021A768 File Offset: 0x00218968
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		string formattedMass = GameUtil.GetFormattedMass(this.overpressureMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.OVER_PRESSURE_MASS, formattedMass), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.OVER_PRESSURE_MASS, formattedMass), Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x04003DD4 RID: 15828
	private int cell = -1;

	// Token: 0x04003DD5 RID: 15829
	private int sortKey;

	// Token: 0x04003DD6 RID: 15830
	[Serialize]
	public Dictionary<SimHashes, float> lifeTimeVentMass = new Dictionary<SimHashes, float>();

	// Token: 0x04003DD7 RID: 15831
	private Vent.StatesInstance smi;

	// Token: 0x04003DD8 RID: 15832
	[SerializeField]
	public ConduitType conduitType = ConduitType.Gas;

	// Token: 0x04003DD9 RID: 15833
	[SerializeField]
	public Endpoint endpointType;

	// Token: 0x04003DDA RID: 15834
	[SerializeField]
	public float overpressureMass = 1f;

	// Token: 0x04003DDB RID: 15835
	[NonSerialized]
	public bool showConnectivityIcons = true;

	// Token: 0x04003DDC RID: 15836
	[MyCmpGet]
	[NonSerialized]
	public Structure structure;

	// Token: 0x04003DDD RID: 15837
	[MyCmpGet]
	[NonSerialized]
	public Operational operational;

	// Token: 0x02001DA6 RID: 7590
	public enum State
	{
		// Token: 0x04008BDA RID: 35802
		Invalid,
		// Token: 0x04008BDB RID: 35803
		Ready,
		// Token: 0x04008BDC RID: 35804
		Blocked,
		// Token: 0x04008BDD RID: 35805
		OverPressure,
		// Token: 0x04008BDE RID: 35806
		Closed
	}

	// Token: 0x02001DA7 RID: 7591
	public class StatesInstance : GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.GameInstance
	{
		// Token: 0x0600B1B4 RID: 45492 RVA: 0x003DDB3D File Offset: 0x003DBD3D
		public StatesInstance(Vent master) : base(master)
		{
			this.exhaust = master.GetComponent<Exhaust>();
		}

		// Token: 0x0600B1B5 RID: 45493 RVA: 0x003DDB52 File Offset: 0x003DBD52
		public bool NeedsExhaust()
		{
			return this.exhaust != null && base.master.GetEndPointState() != Vent.State.Ready && base.master.endpointType == Endpoint.Source;
		}

		// Token: 0x0600B1B6 RID: 45494 RVA: 0x003DDB80 File Offset: 0x003DBD80
		public bool Blocked()
		{
			return base.master.GetEndPointState() == Vent.State.Blocked && base.master.endpointType > Endpoint.Source;
		}

		// Token: 0x0600B1B7 RID: 45495 RVA: 0x003DDBA0 File Offset: 0x003DBDA0
		public bool OverPressure()
		{
			return this.exhaust != null && base.master.GetEndPointState() == Vent.State.OverPressure && base.master.endpointType > Endpoint.Source;
		}

		// Token: 0x0600B1B8 RID: 45496 RVA: 0x003DDBD0 File Offset: 0x003DBDD0
		public void CheckTransitions()
		{
			if (this.NeedsExhaust())
			{
				base.smi.GoTo(base.sm.needExhaust);
				return;
			}
			if (base.master.Closed())
			{
				base.smi.GoTo(base.sm.closed);
				return;
			}
			if (this.Blocked())
			{
				base.smi.GoTo(base.sm.open.blocked);
				return;
			}
			if (this.OverPressure())
			{
				base.smi.GoTo(base.sm.open.overPressure);
				return;
			}
			base.smi.GoTo(base.sm.open.idle);
		}

		// Token: 0x0600B1B9 RID: 45497 RVA: 0x003DDC83 File Offset: 0x003DBE83
		public StatusItem SelectStatusItem(StatusItem gas_status_item, StatusItem liquid_status_item)
		{
			if (base.master.conduitType != ConduitType.Gas)
			{
				return liquid_status_item;
			}
			return gas_status_item;
		}

		// Token: 0x04008BDF RID: 35807
		private Exhaust exhaust;
	}

	// Token: 0x02001DA8 RID: 7592
	public class States : GameStateMachine<Vent.States, Vent.StatesInstance, Vent>
	{
		// Token: 0x0600B1BA RID: 45498 RVA: 0x003DDC98 File Offset: 0x003DBE98
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.open.idle;
			this.root.Update("CheckTransitions", delegate(Vent.StatesInstance smi, float dt)
			{
				smi.CheckTransitions();
			}, UpdateRate.SIM_200ms, false);
			this.open.TriggerOnEnter(GameHashes.VentOpen, null);
			this.closed.TriggerOnEnter(GameHashes.VentClosed, null);
			this.open.blocked.ToggleStatusItem((Vent.StatesInstance smi) => smi.SelectStatusItem(Db.Get().BuildingStatusItems.GasVentObstructed, Db.Get().BuildingStatusItems.LiquidVentObstructed), null);
			this.open.overPressure.ToggleStatusItem((Vent.StatesInstance smi) => smi.SelectStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure, Db.Get().BuildingStatusItems.LiquidVentOverPressure), null);
		}

		// Token: 0x04008BE0 RID: 35808
		public Vent.States.OpenState open;

		// Token: 0x04008BE1 RID: 35809
		public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State closed;

		// Token: 0x04008BE2 RID: 35810
		public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State needExhaust;

		// Token: 0x02002A4A RID: 10826
		public class OpenState : GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State
		{
			// Token: 0x0400BADF RID: 47839
			public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State idle;

			// Token: 0x0400BAE0 RID: 47840
			public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State blocked;

			// Token: 0x0400BAE1 RID: 47841
			public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State overPressure;
		}
	}
}
