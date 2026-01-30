using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200070D RID: 1805
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
[AddComponentMenu("KMonoBehaviour/scripts/Battery")]
public class Battery : KMonoBehaviour, IEnergyConsumer, ICircuitConnected, IGameObjectEffectDescriptor, IEnergyProducer
{
	// Token: 0x1700022C RID: 556
	// (get) Token: 0x06002CC9 RID: 11465 RVA: 0x0010488A File Offset: 0x00102A8A
	// (set) Token: 0x06002CCA RID: 11466 RVA: 0x00104892 File Offset: 0x00102A92
	public float WattsUsed { get; private set; }

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x06002CCB RID: 11467 RVA: 0x0010489B File Offset: 0x00102A9B
	public float WattsNeededWhenActive
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06002CCC RID: 11468 RVA: 0x001048A2 File Offset: 0x00102AA2
	public float PercentFull
	{
		get
		{
			return this.joulesAvailable / this.capacity;
		}
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06002CCD RID: 11469 RVA: 0x001048B1 File Offset: 0x00102AB1
	public float PreviousPercentFull
	{
		get
		{
			return this.PreviousJoulesAvailable / this.capacity;
		}
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06002CCE RID: 11470 RVA: 0x001048C0 File Offset: 0x00102AC0
	public float JoulesAvailable
	{
		get
		{
			return this.joulesAvailable;
		}
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06002CCF RID: 11471 RVA: 0x001048C8 File Offset: 0x00102AC8
	public float Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x06002CD0 RID: 11472 RVA: 0x001048D0 File Offset: 0x00102AD0
	// (set) Token: 0x06002CD1 RID: 11473 RVA: 0x001048D8 File Offset: 0x00102AD8
	public float ChargeCapacity { get; private set; }

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06002CD2 RID: 11474 RVA: 0x001048E1 File Offset: 0x00102AE1
	public int PowerSortOrder
	{
		get
		{
			return this.powerSortOrder;
		}
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06002CD3 RID: 11475 RVA: 0x001048E9 File Offset: 0x00102AE9
	public string Name
	{
		get
		{
			return base.GetComponent<KSelectable>().GetName();
		}
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06002CD4 RID: 11476 RVA: 0x001048F6 File Offset: 0x00102AF6
	// (set) Token: 0x06002CD5 RID: 11477 RVA: 0x001048FE File Offset: 0x00102AFE
	public int PowerCell { get; private set; }

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06002CD6 RID: 11478 RVA: 0x00104907 File Offset: 0x00102B07
	public ushort CircuitID
	{
		get
		{
			return Game.Instance.circuitManager.GetCircuitID(this);
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06002CD7 RID: 11479 RVA: 0x00104919 File Offset: 0x00102B19
	public bool IsConnected
	{
		get
		{
			return this.connectionStatus > CircuitManager.ConnectionStatus.NotConnected;
		}
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06002CD8 RID: 11480 RVA: 0x00104924 File Offset: 0x00102B24
	public bool IsPowered
	{
		get
		{
			return this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x06002CD9 RID: 11481 RVA: 0x0010492F File Offset: 0x00102B2F
	// (set) Token: 0x06002CDA RID: 11482 RVA: 0x00104937 File Offset: 0x00102B37
	public bool IsVirtual { get; protected set; }

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x06002CDB RID: 11483 RVA: 0x00104940 File Offset: 0x00102B40
	// (set) Token: 0x06002CDC RID: 11484 RVA: 0x00104948 File Offset: 0x00102B48
	public object VirtualCircuitKey { get; protected set; }

	// Token: 0x06002CDD RID: 11485 RVA: 0x00104954 File Offset: 0x00102B54
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Batteries.Add(this);
		Building component = base.GetComponent<Building>();
		this.PowerCell = component.GetPowerInputCell();
		base.Subscribe<Battery>(-1582839653, Battery.OnTagsChangedDelegate);
		this.OnTagsChanged(null);
		this.meter = (base.GetComponent<PowerTransformer>() ? null : new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		}));
		Game.Instance.circuitManager.Connect(this);
		Game.Instance.energySim.AddBattery(this);
	}

	// Token: 0x06002CDE RID: 11486 RVA: 0x00104A14 File Offset: 0x00102C14
	private void OnTagsChanged(object data)
	{
		if (this.HasAllTags(this.connectedTags))
		{
			Game.Instance.circuitManager.Connect(this);
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.BatteryJoulesAvailable, this);
			return;
		}
		Game.Instance.circuitManager.Disconnect(this, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.BatteryJoulesAvailable, false);
	}

	// Token: 0x06002CDF RID: 11487 RVA: 0x00104A98 File Offset: 0x00102C98
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveBattery(this);
		Game.Instance.circuitManager.Disconnect(this, true);
		Components.Batteries.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002CE0 RID: 11488 RVA: 0x00104ACC File Offset: 0x00102CCC
	public virtual void EnergySim200ms(float dt)
	{
		this.dt = dt;
		this.joulesConsumed = 0f;
		this.WattsUsed = 0f;
		this.ChargeCapacity = this.chargeWattage * dt;
		if (this.meter != null)
		{
			float percentFull = this.PercentFull;
			this.meter.SetPositionPercent(percentFull);
		}
		this.UpdateSounds();
		this.PreviousJoulesAvailable = this.JoulesAvailable;
		this.ConsumeEnergy(this.joulesLostPerSecond * dt, true);
	}

	// Token: 0x06002CE1 RID: 11489 RVA: 0x00104B40 File Offset: 0x00102D40
	private void UpdateSounds()
	{
		float previousPercentFull = this.PreviousPercentFull;
		float percentFull = this.PercentFull;
		if (percentFull == 0f && previousPercentFull != 0f)
		{
			base.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryDischarged);
		}
		if (percentFull > 0.999f && previousPercentFull <= 0.999f)
		{
			base.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryFull);
		}
		if (percentFull < 0.25f && previousPercentFull >= 0.25f)
		{
			base.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryWarning);
		}
	}

	// Token: 0x06002CE2 RID: 11490 RVA: 0x00104BBC File Offset: 0x00102DBC
	public void SetConnectionStatus(CircuitManager.ConnectionStatus status)
	{
		this.connectionStatus = status;
		if (status == CircuitManager.ConnectionStatus.NotConnected)
		{
			this.operational.SetActive(false, false);
			return;
		}
		this.operational.SetActive(this.operational.IsOperational && this.JoulesAvailable > 0f, false);
	}

	// Token: 0x06002CE3 RID: 11491 RVA: 0x00104C0C File Offset: 0x00102E0C
	public void AddEnergy(float joules)
	{
		this.joulesAvailable = Mathf.Min(this.capacity, this.JoulesAvailable + joules);
		this.joulesConsumed += joules;
		this.ChargeCapacity -= joules;
		this.WattsUsed = this.joulesConsumed / this.dt;
	}

	// Token: 0x06002CE4 RID: 11492 RVA: 0x00104C64 File Offset: 0x00102E64
	public void ConsumeEnergy(float joules, bool report = false)
	{
		if (report)
		{
			float num = Mathf.Min(this.JoulesAvailable, joules);
			ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyWasted, -num, StringFormatter.Replace(BUILDINGS.PREFABS.BATTERY.CHARGE_LOSS, "{Battery}", this.GetProperName()), null);
		}
		this.joulesAvailable = Mathf.Max(0f, this.JoulesAvailable - joules);
	}

	// Token: 0x06002CE5 RID: 11493 RVA: 0x00104CC2 File Offset: 0x00102EC2
	public void ConsumeEnergy(float joules)
	{
		this.ConsumeEnergy(joules, false);
	}

	// Token: 0x06002CE6 RID: 11494 RVA: 0x00104CCC File Offset: 0x00102ECC
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.powerTransformer == null)
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.REQUIRESPOWERGENERATOR, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESPOWERGENERATOR, Descriptor.DescriptorType.Requirement, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.BATTERYCAPACITY, GameUtil.GetFormattedJoules(this.capacity, "", GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.BATTERYCAPACITY, GameUtil.GetFormattedJoules(this.capacity, "", GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.BATTERYLEAK, GameUtil.GetFormattedJoules(this.joulesLostPerSecond, "F1", GameUtil.TimeSlice.PerCycle)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.BATTERYLEAK, GameUtil.GetFormattedJoules(this.joulesLostPerSecond, "F1", GameUtil.TimeSlice.PerCycle)), Descriptor.DescriptorType.Effect, false));
		}
		else
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.TRANSFORMER_INPUT_WIRE, UI.BUILDINGEFFECTS.TOOLTIPS.TRANSFORMER_INPUT_WIRE, Descriptor.DescriptorType.Requirement, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.TRANSFORMER_OUTPUT_WIRE, GameUtil.GetFormattedWattage(this.capacity, GameUtil.WattageFormatterUnit.Automatic, true)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.TRANSFORMER_OUTPUT_WIRE, GameUtil.GetFormattedWattage(this.capacity, GameUtil.WattageFormatterUnit.Automatic, true)), Descriptor.DescriptorType.Requirement, false));
		}
		return list;
	}

	// Token: 0x06002CE7 RID: 11495 RVA: 0x00104E14 File Offset: 0x00103014
	[ContextMenu("Refill Power")]
	public void DEBUG_RefillPower()
	{
		this.joulesAvailable = this.capacity;
	}

	// Token: 0x04001AAC RID: 6828
	[SerializeField]
	public float capacity;

	// Token: 0x04001AAD RID: 6829
	[SerializeField]
	public float chargeWattage = float.PositiveInfinity;

	// Token: 0x04001AAE RID: 6830
	[Serialize]
	private float joulesAvailable;

	// Token: 0x04001AAF RID: 6831
	[MyCmpGet]
	protected Operational operational;

	// Token: 0x04001AB0 RID: 6832
	[MyCmpGet]
	public PowerTransformer powerTransformer;

	// Token: 0x04001AB1 RID: 6833
	protected MeterController meter;

	// Token: 0x04001AB3 RID: 6835
	public float joulesLostPerSecond;

	// Token: 0x04001AB5 RID: 6837
	[SerializeField]
	public int powerSortOrder;

	// Token: 0x04001AB9 RID: 6841
	private float PreviousJoulesAvailable;

	// Token: 0x04001ABA RID: 6842
	private CircuitManager.ConnectionStatus connectionStatus;

	// Token: 0x04001ABB RID: 6843
	public static readonly Tag[] DEFAULT_CONNECTED_TAGS = new Tag[]
	{
		GameTags.Operational
	};

	// Token: 0x04001ABC RID: 6844
	[SerializeField]
	public Tag[] connectedTags = Battery.DEFAULT_CONNECTED_TAGS;

	// Token: 0x04001ABD RID: 6845
	private static readonly EventSystem.IntraObjectHandler<Battery> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Battery>(delegate(Battery component, object data)
	{
		component.OnTagsChanged(data);
	});

	// Token: 0x04001ABE RID: 6846
	private float dt;

	// Token: 0x04001ABF RID: 6847
	private float joulesConsumed;
}
