using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000788 RID: 1928
[SerializationConfig(MemberSerialization.OptIn)]
public class LimitValve : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x1700029D RID: 669
	// (get) Token: 0x0600312D RID: 12589 RVA: 0x0011BBD8 File Offset: 0x00119DD8
	public float RemainingCapacity
	{
		get
		{
			return Mathf.Max(0f, this.m_limit - this.m_amount);
		}
	}

	// Token: 0x0600312E RID: 12590 RVA: 0x0011BBF1 File Offset: 0x00119DF1
	public NonLinearSlider.Range[] GetRanges()
	{
		if (this.sliderRanges != null && this.sliderRanges.Length != 0)
		{
			return this.sliderRanges;
		}
		return NonLinearSlider.GetDefaultRange(this.maxLimitKg);
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x0600312F RID: 12591 RVA: 0x0011BC16 File Offset: 0x00119E16
	// (set) Token: 0x06003130 RID: 12592 RVA: 0x0011BC1E File Offset: 0x00119E1E
	public float Limit
	{
		get
		{
			return this.m_limit;
		}
		set
		{
			this.m_limit = value;
			this.Refresh();
		}
	}

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06003131 RID: 12593 RVA: 0x0011BC2D File Offset: 0x00119E2D
	// (set) Token: 0x06003132 RID: 12594 RVA: 0x0011BC35 File Offset: 0x00119E35
	public float Amount
	{
		get
		{
			return this.m_amount;
		}
		set
		{
			this.m_amount = value;
			base.BoxingTrigger<float>(-1722241721, this.m_amount);
			this.Refresh();
		}
	}

	// Token: 0x06003133 RID: 12595 RVA: 0x0011BC58 File Offset: 0x00119E58
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LimitValve>(-905833192, LimitValve.OnCopySettingsDelegate);
		foreach (KBatchedAnimController kbatchedAnimController in base.GetComponentsInChildren<KBatchedAnimController>())
		{
			if (kbatchedAnimController.name.Contains("_fg"))
			{
				this.fg_Controller = kbatchedAnimController;
				return;
			}
		}
	}

	// Token: 0x06003134 RID: 12596 RVA: 0x0011BCB0 File Offset: 0x00119EB0
	protected override void OnSpawn()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Combine(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		base.Subscribe<LimitValve>(-801688580, LimitValve.OnLogicValueChangedDelegate);
		if (this.conduitType == ConduitType.Gas || this.conduitType == ConduitType.Liquid)
		{
			ConduitBridge conduitBridge = this.conduitBridge;
			conduitBridge.desiredMassTransfer = (ConduitBridgeBase.DesiredMassTransfer)Delegate.Combine(conduitBridge.desiredMassTransfer, new ConduitBridgeBase.DesiredMassTransfer(this.DesiredMassTransfer));
			ConduitBridge conduitBridge2 = this.conduitBridge;
			conduitBridge2.OnMassTransfer = (ConduitBridgeBase.ConduitBridgeEvent)Delegate.Combine(conduitBridge2.OnMassTransfer, new ConduitBridgeBase.ConduitBridgeEvent(this.OnMassTransfer));
		}
		else if (this.conduitType == ConduitType.Solid)
		{
			SolidConduitBridge solidConduitBridge = this.solidConduitBridge;
			solidConduitBridge.desiredMassTransfer = (ConduitBridgeBase.DesiredMassTransfer)Delegate.Combine(solidConduitBridge.desiredMassTransfer, new ConduitBridgeBase.DesiredMassTransfer(this.DesiredMassTransfer));
			SolidConduitBridge solidConduitBridge2 = this.solidConduitBridge;
			solidConduitBridge2.OnMassTransfer = (ConduitBridgeBase.ConduitBridgeEvent)Delegate.Combine(solidConduitBridge2.OnMassTransfer, new ConduitBridgeBase.ConduitBridgeEvent(this.OnMassTransfer));
		}
		if (this.limitMeter == null)
		{
			this.limitMeter = new MeterController(this.controller, "meter_target_counter", "meter_counter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target_counter"
			});
		}
		this.Refresh();
		base.OnSpawn();
	}

	// Token: 0x06003135 RID: 12597 RVA: 0x0011BDF2 File Offset: 0x00119FF2
	protected override void OnCleanUp()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Remove(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		base.OnCleanUp();
	}

	// Token: 0x06003136 RID: 12598 RVA: 0x0011BE25 File Offset: 0x0011A025
	private void LogicTick()
	{
		if (this.m_resetRequested)
		{
			this.ResetAmount();
		}
	}

	// Token: 0x06003137 RID: 12599 RVA: 0x0011BE35 File Offset: 0x0011A035
	public void ResetAmount()
	{
		this.m_resetRequested = false;
		this.Amount = 0f;
	}

	// Token: 0x06003138 RID: 12600 RVA: 0x0011BE4C File Offset: 0x0011A04C
	private float DesiredMassTransfer(float dt, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable)
	{
		if (!this.operational.IsOperational)
		{
			return 0f;
		}
		if (this.conduitType == ConduitType.Solid && pickupable != null && GameTags.DisplayAsUnits.Contains(pickupable.KPrefabID.PrefabID()))
		{
			float num = pickupable.PrimaryElement.Units;
			if (this.RemainingCapacity < num)
			{
				num = (float)Mathf.FloorToInt(this.RemainingCapacity);
			}
			return num * pickupable.PrimaryElement.MassPerUnit;
		}
		return Mathf.Min(mass, this.RemainingCapacity);
	}

	// Token: 0x06003139 RID: 12601 RVA: 0x0011BED8 File Offset: 0x0011A0D8
	private void OnMassTransfer(SimHashes element, float transferredMass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable)
	{
		if (!LogicCircuitNetwork.IsBitActive(0, this.ports.GetInputValue(LimitValve.RESET_PORT_ID)))
		{
			if (this.conduitType == ConduitType.Gas || this.conduitType == ConduitType.Liquid)
			{
				this.Amount += transferredMass;
			}
			else if (this.conduitType == ConduitType.Solid && pickupable != null)
			{
				this.Amount += transferredMass / pickupable.PrimaryElement.MassPerUnit;
			}
		}
		if (this.lastElemenet != element && this.conduitType == ConduitType.Liquid)
		{
			Element element2 = ElementLoader.FindElementByHash(element);
			if (element2 != null)
			{
				Color color = element2.substance.colour;
				color.a = 1f;
				this.controller.SetSymbolTint(new KAnimHashedString("gradient"), color);
				this.fg_Controller.SetSymbolTint(new KAnimHashedString("water_color_fg"), color);
			}
		}
		this.lastElemenet = element;
		this.operational.SetActive(this.operational.IsOperational && transferredMass > 0f, false);
		this.Refresh();
	}

	// Token: 0x0600313A RID: 12602 RVA: 0x0011BFE4 File Offset: 0x0011A1E4
	private void Refresh()
	{
		if (this.operational == null)
		{
			return;
		}
		this.ports.SendSignal(LimitValve.OUTPUT_PORT_ID, (this.RemainingCapacity <= 0f) ? 1 : 0);
		this.operational.SetFlag(LimitValve.limitNotReached, this.RemainingCapacity > 0f);
		if (this.RemainingCapacity > 0f)
		{
			this.limitMeter.meterController.Play("meter_counter", KAnim.PlayMode.Paused, 1f, 0f);
			this.limitMeter.SetPositionPercent(this.Amount / this.Limit);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.LimitValveLimitNotReached, this);
			return;
		}
		this.limitMeter.meterController.Play("meter_on", KAnim.PlayMode.Paused, 1f, 0f);
		this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.LimitValveLimitReached, this);
	}

	// Token: 0x0600313B RID: 12603 RVA: 0x0011C104 File Offset: 0x0011A304
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == LimitValve.RESET_PORT_ID && LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue))
		{
			this.ResetAmount();
		}
	}

	// Token: 0x0600313C RID: 12604 RVA: 0x0011C140 File Offset: 0x0011A340
	private void OnCopySettings(object data)
	{
		LimitValve component = ((GameObject)data).GetComponent<LimitValve>();
		if (component != null)
		{
			this.Limit = component.Limit;
		}
	}

	// Token: 0x04001D7D RID: 7549
	public static readonly HashedString RESET_PORT_ID = new HashedString("LimitValveReset");

	// Token: 0x04001D7E RID: 7550
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LimitValveOutput");

	// Token: 0x04001D7F RID: 7551
	public static readonly Operational.Flag limitNotReached = new Operational.Flag("limitNotReached", Operational.Flag.Type.Requirement);

	// Token: 0x04001D80 RID: 7552
	public ConduitType conduitType;

	// Token: 0x04001D81 RID: 7553
	public float maxLimitKg = 100f;

	// Token: 0x04001D82 RID: 7554
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001D83 RID: 7555
	[MyCmpReq]
	private LogicPorts ports;

	// Token: 0x04001D84 RID: 7556
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x04001D85 RID: 7557
	private KBatchedAnimController fg_Controller;

	// Token: 0x04001D86 RID: 7558
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001D87 RID: 7559
	[MyCmpGet]
	private ConduitBridge conduitBridge;

	// Token: 0x04001D88 RID: 7560
	[MyCmpGet]
	private SolidConduitBridge solidConduitBridge;

	// Token: 0x04001D89 RID: 7561
	[Serialize]
	[SerializeField]
	private float m_limit;

	// Token: 0x04001D8A RID: 7562
	[Serialize]
	private float m_amount;

	// Token: 0x04001D8B RID: 7563
	[Serialize]
	private bool m_resetRequested;

	// Token: 0x04001D8C RID: 7564
	private MeterController limitMeter;

	// Token: 0x04001D8D RID: 7565
	public bool displayUnitsInsteadOfMass;

	// Token: 0x04001D8E RID: 7566
	public NonLinearSlider.Range[] sliderRanges;

	// Token: 0x04001D8F RID: 7567
	private SimHashes lastElemenet = SimHashes.Void;

	// Token: 0x04001D90 RID: 7568
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001D91 RID: 7569
	private static readonly EventSystem.IntraObjectHandler<LimitValve> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LimitValve>(delegate(LimitValve component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001D92 RID: 7570
	private static readonly EventSystem.IntraObjectHandler<LimitValve> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LimitValve>(delegate(LimitValve component, object data)
	{
		component.OnCopySettings(data);
	});
}
