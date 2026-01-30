using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000529 RID: 1321
[AddComponentMenu("KMonoBehaviour/scripts/Sensors")]
public class Sensors : KMonoBehaviour
{
	// Token: 0x06001C8A RID: 7306 RVA: 0x0009CB22 File Offset: 0x0009AD22
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<Brain>().onPreUpdate += this.OnBrainPreUpdate;
	}

	// Token: 0x06001C8B RID: 7307 RVA: 0x0009CB44 File Offset: 0x0009AD44
	public SensorType GetSensor<SensorType>() where SensorType : Sensor
	{
		foreach (Sensor sensor in this.sensors)
		{
			if (typeof(SensorType).IsAssignableFrom(sensor.GetType()))
			{
				return (SensorType)((object)sensor);
			}
		}
		global::Debug.LogError("Missing sensor of type: " + typeof(SensorType).Name);
		return default(SensorType);
	}

	// Token: 0x06001C8C RID: 7308 RVA: 0x0009CBDC File Offset: 0x0009ADDC
	public void Add(Sensor sensor)
	{
		this.sensors.Add(sensor);
		if (sensor.IsEnabled)
		{
			sensor.Update();
		}
	}

	// Token: 0x06001C8D RID: 7309 RVA: 0x0009CBF8 File Offset: 0x0009ADF8
	public void UpdateSensors()
	{
		foreach (Sensor sensor in this.sensors)
		{
			if (sensor.IsEnabled)
			{
				sensor.Update();
			}
		}
	}

	// Token: 0x06001C8E RID: 7310 RVA: 0x0009CC54 File Offset: 0x0009AE54
	private void OnBrainPreUpdate()
	{
		this.UpdateSensors();
	}

	// Token: 0x06001C8F RID: 7311 RVA: 0x0009CC5C File Offset: 0x0009AE5C
	public void ShowEditor()
	{
		foreach (Sensor sensor in this.sensors)
		{
			sensor.ShowEditor();
		}
	}

	// Token: 0x040010D4 RID: 4308
	public List<Sensor> sensors = new List<Sensor>();
}
