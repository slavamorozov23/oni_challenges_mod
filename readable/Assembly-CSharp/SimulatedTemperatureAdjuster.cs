using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000B4D RID: 2893
public class SimulatedTemperatureAdjuster
{
	// Token: 0x06005571 RID: 21873 RVA: 0x001F2D84 File Offset: 0x001F0F84
	public SimulatedTemperatureAdjuster(float simulated_temperature, float heat_capacity, float thermal_conductivity, Storage storage)
	{
		this.temperature = simulated_temperature;
		this.heatCapacity = heat_capacity;
		this.thermalConductivity = thermal_conductivity;
		this.storage = storage;
		storage.gameObject.Subscribe(824508782, new Action<object>(this.OnActivechanged));
		storage.gameObject.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		Operational component = storage.gameObject.GetComponent<Operational>();
		this.OnActivechanged(component);
	}

	// Token: 0x06005572 RID: 21874 RVA: 0x001F2E04 File Offset: 0x001F1004
	public List<Descriptor> GetDescriptors()
	{
		return SimulatedTemperatureAdjuster.GetDescriptors(this.temperature);
	}

	// Token: 0x06005573 RID: 21875 RVA: 0x001F2E14 File Offset: 0x001F1014
	public static List<Descriptor> GetDescriptors(float temperature)
	{
		List<Descriptor> list = new List<Descriptor>();
		string formattedTemperature = GameUtil.GetFormattedTemperature(temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
		Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.ITEM_TEMPERATURE_ADJUST, formattedTemperature), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ITEM_TEMPERATURE_ADJUST, formattedTemperature), Descriptor.DescriptorType.Effect, false);
		list.Add(item);
		return list;
	}

	// Token: 0x06005574 RID: 21876 RVA: 0x001F2E64 File Offset: 0x001F1064
	private void Register(SimTemperatureTransfer stt)
	{
		stt.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Remove(stt.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnItemSimRegistered));
		stt.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Combine(stt.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnItemSimRegistered));
		if (Sim.IsValidHandle(stt.SimHandle))
		{
			this.OnItemSimRegistered(stt);
		}
	}

	// Token: 0x06005575 RID: 21877 RVA: 0x001F2ECC File Offset: 0x001F10CC
	private void Unregister(SimTemperatureTransfer stt)
	{
		stt.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Remove(stt.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnItemSimRegistered));
		if (Sim.IsValidHandle(stt.SimHandle))
		{
			SimMessages.ModifyElementChunkTemperatureAdjuster(stt.SimHandle, 0f, 0f, 0f);
		}
	}

	// Token: 0x06005576 RID: 21878 RVA: 0x001F2F24 File Offset: 0x001F1124
	private void OnItemSimRegistered(SimTemperatureTransfer stt)
	{
		if (stt == null)
		{
			return;
		}
		if (Sim.IsValidHandle(stt.SimHandle))
		{
			float num = this.temperature;
			float heat_capacity = this.heatCapacity;
			float thermal_conductivity = this.thermalConductivity;
			if (!this.active)
			{
				num = 0f;
				heat_capacity = 0f;
				thermal_conductivity = 0f;
			}
			SimMessages.ModifyElementChunkTemperatureAdjuster(stt.SimHandle, num, heat_capacity, thermal_conductivity);
		}
	}

	// Token: 0x06005577 RID: 21879 RVA: 0x001F2F88 File Offset: 0x001F1188
	private void OnActivechanged(object data)
	{
		Operational operational = (Operational)data;
		this.active = operational.IsActive;
		if (this.active)
		{
			using (List<GameObject>.Enumerator enumerator = this.storage.items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = enumerator.Current;
					if (gameObject != null)
					{
						SimTemperatureTransfer component = gameObject.GetComponent<SimTemperatureTransfer>();
						this.OnItemSimRegistered(component);
					}
				}
				return;
			}
		}
		foreach (GameObject gameObject2 in this.storage.items)
		{
			if (gameObject2 != null)
			{
				SimTemperatureTransfer component2 = gameObject2.GetComponent<SimTemperatureTransfer>();
				this.Unregister(component2);
			}
		}
	}

	// Token: 0x06005578 RID: 21880 RVA: 0x001F3068 File Offset: 0x001F1268
	public void CleanUp()
	{
		this.storage.gameObject.Unsubscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		foreach (GameObject gameObject in this.storage.items)
		{
			if (gameObject != null)
			{
				SimTemperatureTransfer component = gameObject.GetComponent<SimTemperatureTransfer>();
				this.Unregister(component);
			}
		}
	}

	// Token: 0x06005579 RID: 21881 RVA: 0x001F30F4 File Offset: 0x001F12F4
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		SimTemperatureTransfer component = gameObject.GetComponent<SimTemperatureTransfer>();
		if (component == null)
		{
			return;
		}
		Pickupable component2 = gameObject.GetComponent<Pickupable>();
		if (component2 == null)
		{
			return;
		}
		if (this.active && component2.storage == this.storage)
		{
			this.Register(component);
			return;
		}
		this.Unregister(component);
	}

	// Token: 0x040039AC RID: 14764
	private float temperature;

	// Token: 0x040039AD RID: 14765
	private float heatCapacity;

	// Token: 0x040039AE RID: 14766
	private float thermalConductivity;

	// Token: 0x040039AF RID: 14767
	private bool active;

	// Token: 0x040039B0 RID: 14768
	private Storage storage;
}
