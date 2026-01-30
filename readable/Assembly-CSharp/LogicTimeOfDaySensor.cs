using System;
using KSerialization;
using UnityEngine;

// Token: 0x020007A9 RID: 1961
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTimeOfDaySensor : Switch, ISaveLoadable, ISim200ms
{
	// Token: 0x06003388 RID: 13192 RVA: 0x00125C38 File Offset: 0x00123E38
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicTimeOfDaySensor>(-905833192, LogicTimeOfDaySensor.OnCopySettingsDelegate);
	}

	// Token: 0x06003389 RID: 13193 RVA: 0x00125C54 File Offset: 0x00123E54
	private void OnCopySettings(object data)
	{
		LogicTimeOfDaySensor component = ((GameObject)data).GetComponent<LogicTimeOfDaySensor>();
		if (component != null)
		{
			this.startTime = component.startTime;
			this.duration = component.duration;
		}
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x00125C8E File Offset: 0x00123E8E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x0600338B RID: 13195 RVA: 0x00125CC4 File Offset: 0x00123EC4
	public void Sim200ms(float dt)
	{
		float currentCycleAsPercentage = GameClock.Instance.GetCurrentCycleAsPercentage();
		bool state = false;
		if (currentCycleAsPercentage >= this.startTime && currentCycleAsPercentage < this.startTime + this.duration)
		{
			state = true;
		}
		if (currentCycleAsPercentage < this.startTime + this.duration - 1f)
		{
			state = true;
		}
		this.SetState(state);
	}

	// Token: 0x0600338C RID: 13196 RVA: 0x00125D18 File Offset: 0x00123F18
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x0600338D RID: 13197 RVA: 0x00125D27 File Offset: 0x00123F27
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x0600338E RID: 13198 RVA: 0x00125D48 File Offset: 0x00123F48
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

	// Token: 0x0600338F RID: 13199 RVA: 0x00125DD0 File Offset: 0x00123FD0
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001F2B RID: 7979
	[SerializeField]
	[Serialize]
	public float startTime;

	// Token: 0x04001F2C RID: 7980
	[SerializeField]
	[Serialize]
	public float duration = 1f;

	// Token: 0x04001F2D RID: 7981
	private bool wasOn;

	// Token: 0x04001F2E RID: 7982
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001F2F RID: 7983
	private static readonly EventSystem.IntraObjectHandler<LogicTimeOfDaySensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTimeOfDaySensor>(delegate(LogicTimeOfDaySensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
