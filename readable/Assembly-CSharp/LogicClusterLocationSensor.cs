using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000790 RID: 1936
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicClusterLocationSensor : Switch, ISaveLoadable, ISim200ms
{
	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06003194 RID: 12692 RVA: 0x0011E3D1 File Offset: 0x0011C5D1
	public bool ActiveInSpace
	{
		get
		{
			return this.activeInSpace;
		}
	}

	// Token: 0x06003195 RID: 12693 RVA: 0x0011E3D9 File Offset: 0x0011C5D9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicClusterLocationSensor>(-905833192, LogicClusterLocationSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06003196 RID: 12694 RVA: 0x0011E3F4 File Offset: 0x0011C5F4
	private void OnCopySettings(object data)
	{
		LogicClusterLocationSensor component = ((GameObject)data).GetComponent<LogicClusterLocationSensor>();
		if (component != null)
		{
			this.activeLocations.Clear();
			for (int i = 0; i < component.activeLocations.Count; i++)
			{
				this.SetLocationEnabled(component.activeLocations[i], true);
			}
			this.activeInSpace = component.activeInSpace;
		}
	}

	// Token: 0x06003197 RID: 12695 RVA: 0x0011E456 File Offset: 0x0011C656
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06003198 RID: 12696 RVA: 0x0011E489 File Offset: 0x0011C689
	public void SetLocationEnabled(AxialI location, bool setting)
	{
		if (!setting)
		{
			this.activeLocations.Remove(location);
			return;
		}
		if (!this.activeLocations.Contains(location))
		{
			this.activeLocations.Add(location);
		}
	}

	// Token: 0x06003199 RID: 12697 RVA: 0x0011E4B6 File Offset: 0x0011C6B6
	public void SetSpaceEnabled(bool setting)
	{
		this.activeInSpace = setting;
	}

	// Token: 0x0600319A RID: 12698 RVA: 0x0011E4C0 File Offset: 0x0011C6C0
	public void Sim200ms(float dt)
	{
		bool state = this.CheckCurrentLocationSelected();
		this.SetState(state);
	}

	// Token: 0x0600319B RID: 12699 RVA: 0x0011E4DC File Offset: 0x0011C6DC
	private bool CheckCurrentLocationSelected()
	{
		AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
		return this.activeLocations.Contains(myWorldLocation) || (this.activeInSpace && this.CheckInEmptySpace());
	}

	// Token: 0x0600319C RID: 12700 RVA: 0x0011E518 File Offset: 0x0011C718
	private bool CheckInEmptySpace()
	{
		bool result = true;
		AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (!worldContainer.IsModuleInterior && worldContainer.GetMyWorldLocation() == myWorldLocation)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600319D RID: 12701 RVA: 0x0011E594 File Offset: 0x0011C794
	public bool CheckLocationSelected(AxialI location)
	{
		return this.activeLocations.Contains(location);
	}

	// Token: 0x0600319E RID: 12702 RVA: 0x0011E5A2 File Offset: 0x0011C7A2
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x0600319F RID: 12703 RVA: 0x0011E5B1 File Offset: 0x0011C7B1
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060031A0 RID: 12704 RVA: 0x0011E5D0 File Offset: 0x0011C7D0
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
			bool flag = true;
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.IsModuleInterior && worldContainer.GetMyWorldLocation() == myWorldLocation)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				component.Play(this.switchedOn ? "on_space_pre" : "on_space_pst", KAnim.PlayMode.Once, 1f, 0f);
				component.Queue(this.switchedOn ? "on_space" : "off_space", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			component.Play(this.switchedOn ? "on_asteroid_pre" : "on_asteroid_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on_asteroid" : "off_asteroid", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x060031A1 RID: 12705 RVA: 0x0011E724 File Offset: 0x0011C924
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001DD9 RID: 7641
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001DDA RID: 7642
	[Serialize]
	private List<AxialI> activeLocations = new List<AxialI>();

	// Token: 0x04001DDB RID: 7643
	[Serialize]
	private bool activeInSpace = true;

	// Token: 0x04001DDC RID: 7644
	private bool wasOn;

	// Token: 0x04001DDD RID: 7645
	private static readonly EventSystem.IntraObjectHandler<LogicClusterLocationSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicClusterLocationSensor>(delegate(LogicClusterLocationSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
