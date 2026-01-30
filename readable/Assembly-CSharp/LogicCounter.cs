using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000791 RID: 1937
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicCounter : Switch, ISaveLoadable
{
	// Token: 0x060031A4 RID: 12708 RVA: 0x0011E7AD File Offset: 0x0011C9AD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicCounter>(-905833192, LogicCounter.OnCopySettingsDelegate);
	}

	// Token: 0x060031A5 RID: 12709 RVA: 0x0011E7C8 File Offset: 0x0011C9C8
	private void OnCopySettings(object data)
	{
		LogicCounter component = ((GameObject)data).GetComponent<LogicCounter>();
		if (component != null)
		{
			this.maxCount = component.maxCount;
			this.resetCountAtMax = component.resetCountAtMax;
			this.advancedMode = component.advancedMode;
		}
	}

	// Token: 0x060031A6 RID: 12710 RVA: 0x0011E810 File Offset: 0x0011CA10
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Combine(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		if (this.maxCount == 0)
		{
			this.maxCount = 10;
		}
		base.Subscribe<LogicCounter>(-801688580, LogicCounter.OnLogicValueChangedDelegate);
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", component.FlipY ? "meter_dn" : "meter_up", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront, Vector3.zero, null);
		this.UpdateMeter();
	}

	// Token: 0x060031A7 RID: 12711 RVA: 0x0011E8D5 File Offset: 0x0011CAD5
	protected override void OnCleanUp()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Remove(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
	}

	// Token: 0x060031A8 RID: 12712 RVA: 0x0011E902 File Offset: 0x0011CB02
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x060031A9 RID: 12713 RVA: 0x0011E911 File Offset: 0x0011CB11
	public void UpdateLogicCircuit()
	{
		if (this.receivedFirstSignal)
		{
			base.GetComponent<LogicPorts>().SendSignal(LogicCounter.OUTPUT_PORT_ID, this.switchedOn ? 1 : 0);
		}
	}

	// Token: 0x060031AA RID: 12714 RVA: 0x0011E938 File Offset: 0x0011CB38
	public void UpdateMeter()
	{
		float num = (float)(this.currentCount % (this.advancedMode ? this.maxCount : 10));
		this.meter.SetPositionPercent(num / 9f);
	}

	// Token: 0x060031AB RID: 12715 RVA: 0x0011E974 File Offset: 0x0011CB74
	public void UpdateVisualState(bool force = false)
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (!this.receivedFirstSignal)
		{
			component.Play("off", KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		if (this.wasOn != this.switchedOn || force)
		{
			int num = (this.switchedOn ? 4 : 0) + (this.wasResetting ? 2 : 0) + (this.wasIncrementing ? 1 : 0);
			this.wasOn = this.switchedOn;
			component.Play("on_" + num.ToString(), KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x060031AC RID: 12716 RVA: 0x0011EA1C File Offset: 0x0011CC1C
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == LogicCounter.INPUT_PORT_ID)
		{
			int newValue = logicValueChanged.newValue;
			this.receivedFirstSignal = true;
			if (LogicCircuitNetwork.IsBitActive(0, newValue))
			{
				if (!this.wasIncrementing)
				{
					this.wasIncrementing = true;
					if (!this.wasResetting)
					{
						if (this.currentCount == this.maxCount || this.currentCount >= 10)
						{
							this.currentCount = 0;
						}
						this.currentCount++;
						this.UpdateMeter();
						this.SetCounterState();
						if (this.currentCount == this.maxCount && this.resetCountAtMax)
						{
							this.resetRequested = true;
						}
					}
				}
			}
			else
			{
				this.wasIncrementing = false;
			}
		}
		else
		{
			if (!(logicValueChanged.portID == LogicCounter.RESET_PORT_ID))
			{
				return;
			}
			int newValue2 = logicValueChanged.newValue;
			this.receivedFirstSignal = true;
			if (LogicCircuitNetwork.IsBitActive(0, newValue2))
			{
				if (!this.wasResetting)
				{
					this.wasResetting = true;
					this.ResetCounter();
				}
			}
			else
			{
				this.wasResetting = false;
			}
		}
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
	}

	// Token: 0x060031AD RID: 12717 RVA: 0x0011EB34 File Offset: 0x0011CD34
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x060031AE RID: 12718 RVA: 0x0011EB87 File Offset: 0x0011CD87
	public void ResetCounter()
	{
		this.resetRequested = false;
		this.currentCount = 0;
		this.SetState(false);
		if (this.advancedMode)
		{
			this.pulsingActive = false;
			this.pulseTicksRemaining = 0;
		}
		this.UpdateVisualState(true);
		this.UpdateMeter();
		this.UpdateLogicCircuit();
	}

	// Token: 0x060031AF RID: 12719 RVA: 0x0011EBC8 File Offset: 0x0011CDC8
	public void LogicTick()
	{
		if (this.resetRequested)
		{
			this.ResetCounter();
		}
		if (this.pulsingActive)
		{
			this.pulseTicksRemaining--;
			if (this.pulseTicksRemaining <= 0)
			{
				this.pulsingActive = false;
				this.SetState(false);
				this.UpdateVisualState(false);
				this.UpdateMeter();
				this.UpdateLogicCircuit();
			}
		}
	}

	// Token: 0x060031B0 RID: 12720 RVA: 0x0011EC24 File Offset: 0x0011CE24
	public void SetCounterState()
	{
		this.SetState(this.advancedMode ? (this.currentCount % this.maxCount == 0) : (this.currentCount == this.maxCount));
		if (this.advancedMode && this.currentCount % this.maxCount == 0)
		{
			this.pulsingActive = true;
			this.pulseTicksRemaining = 2;
		}
	}

	// Token: 0x04001DDE RID: 7646
	[Serialize]
	public int maxCount;

	// Token: 0x04001DDF RID: 7647
	[Serialize]
	public int currentCount;

	// Token: 0x04001DE0 RID: 7648
	[Serialize]
	public bool resetCountAtMax;

	// Token: 0x04001DE1 RID: 7649
	[Serialize]
	public bool advancedMode;

	// Token: 0x04001DE2 RID: 7650
	private bool wasOn;

	// Token: 0x04001DE3 RID: 7651
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001DE4 RID: 7652
	private static readonly EventSystem.IntraObjectHandler<LogicCounter> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicCounter>(delegate(LogicCounter component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001DE5 RID: 7653
	private static readonly EventSystem.IntraObjectHandler<LogicCounter> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicCounter>(delegate(LogicCounter component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001DE6 RID: 7654
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicCounterInput");

	// Token: 0x04001DE7 RID: 7655
	public static readonly HashedString RESET_PORT_ID = new HashedString("LogicCounterReset");

	// Token: 0x04001DE8 RID: 7656
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LogicCounterOutput");

	// Token: 0x04001DE9 RID: 7657
	private bool resetRequested;

	// Token: 0x04001DEA RID: 7658
	[Serialize]
	private bool wasResetting;

	// Token: 0x04001DEB RID: 7659
	[Serialize]
	private bool wasIncrementing;

	// Token: 0x04001DEC RID: 7660
	[Serialize]
	public bool receivedFirstSignal;

	// Token: 0x04001DED RID: 7661
	private bool pulsingActive;

	// Token: 0x04001DEE RID: 7662
	private const int pulseLength = 1;

	// Token: 0x04001DEF RID: 7663
	private int pulseTicksRemaining;

	// Token: 0x04001DF0 RID: 7664
	private MeterController meter;
}
