using System;
using KSerialization;
using UnityEngine;

// Token: 0x020007AA RID: 1962
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTimerSensor : Switch, ISaveLoadable, ISim33ms
{
	// Token: 0x06003392 RID: 13202 RVA: 0x00125E52 File Offset: 0x00124052
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicTimerSensor>(-905833192, LogicTimerSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06003393 RID: 13203 RVA: 0x00125E6C File Offset: 0x0012406C
	private void OnCopySettings(object data)
	{
		LogicTimerSensor component = ((GameObject)data).GetComponent<LogicTimerSensor>();
		if (component != null)
		{
			this.onDuration = component.onDuration;
			this.offDuration = component.offDuration;
			this.timeElapsedInCurrentState = component.timeElapsedInCurrentState;
			this.displayCyclesMode = component.displayCyclesMode;
		}
	}

	// Token: 0x06003394 RID: 13204 RVA: 0x00125EBE File Offset: 0x001240BE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06003395 RID: 13205 RVA: 0x00125EF4 File Offset: 0x001240F4
	public void Sim33ms(float dt)
	{
		if (this.onDuration == 0f && this.offDuration == 0f)
		{
			return;
		}
		this.timeElapsedInCurrentState += dt;
		bool flag = base.IsSwitchedOn;
		if (flag)
		{
			if (this.timeElapsedInCurrentState >= this.onDuration)
			{
				flag = false;
				this.timeElapsedInCurrentState -= this.onDuration;
			}
		}
		else if (this.timeElapsedInCurrentState >= this.offDuration)
		{
			flag = true;
			this.timeElapsedInCurrentState -= this.offDuration;
		}
		this.SetState(flag);
	}

	// Token: 0x06003396 RID: 13206 RVA: 0x00125F83 File Offset: 0x00124183
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06003397 RID: 13207 RVA: 0x00125F92 File Offset: 0x00124192
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06003398 RID: 13208 RVA: 0x00125FB0 File Offset: 0x001241B0
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06003399 RID: 13209 RVA: 0x00126038 File Offset: 0x00124238
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x0600339A RID: 13210 RVA: 0x0012608B File Offset: 0x0012428B
	public void ResetTimer()
	{
		this.SetState(true);
		this.OnSwitchToggled(true);
		this.timeElapsedInCurrentState = 0f;
	}

	// Token: 0x04001F30 RID: 7984
	[Serialize]
	public float onDuration = 10f;

	// Token: 0x04001F31 RID: 7985
	[Serialize]
	public float offDuration = 10f;

	// Token: 0x04001F32 RID: 7986
	[Serialize]
	public bool displayCyclesMode;

	// Token: 0x04001F33 RID: 7987
	private bool wasOn;

	// Token: 0x04001F34 RID: 7988
	[SerializeField]
	[Serialize]
	public float timeElapsedInCurrentState;

	// Token: 0x04001F35 RID: 7989
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001F36 RID: 7990
	private static readonly EventSystem.IntraObjectHandler<LogicTimerSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTimerSensor>(delegate(LogicTimerSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
