using System;
using UnityEngine;

// Token: 0x02000528 RID: 1320
public class Sensor
{
	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06001C7E RID: 7294 RVA: 0x0009CA71 File Offset: 0x0009AC71
	// (set) Token: 0x06001C7D RID: 7293 RVA: 0x0009CA68 File Offset: 0x0009AC68
	public bool IsEnabled { get; private set; } = true;

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x06001C7F RID: 7295 RVA: 0x0009CA79 File Offset: 0x0009AC79
	// (set) Token: 0x06001C80 RID: 7296 RVA: 0x0009CA81 File Offset: 0x0009AC81
	public string Name { get; private set; }

	// Token: 0x06001C81 RID: 7297 RVA: 0x0009CA8A File Offset: 0x0009AC8A
	public Sensor(Sensors sensors, bool active)
	{
		this.sensors = sensors;
		this.SetActive(active);
		this.Name = base.GetType().Name;
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x0009CAB8 File Offset: 0x0009ACB8
	public Sensor(Sensors sensors)
	{
		this.sensors = sensors;
		this.Name = base.GetType().Name;
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x0009CADF File Offset: 0x0009ACDF
	public ComponentType GetComponent<ComponentType>()
	{
		return this.sensors.GetComponent<ComponentType>();
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x06001C84 RID: 7300 RVA: 0x0009CAEC File Offset: 0x0009ACEC
	public GameObject gameObject
	{
		get
		{
			return this.sensors.gameObject;
		}
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x06001C85 RID: 7301 RVA: 0x0009CAF9 File Offset: 0x0009ACF9
	public Transform transform
	{
		get
		{
			return this.gameObject.transform;
		}
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x0009CB06 File Offset: 0x0009AD06
	public virtual void SetActive(bool enabled)
	{
		this.IsEnabled = enabled;
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x0009CB0F File Offset: 0x0009AD0F
	public void Trigger(int hash, object data = null)
	{
		this.sensors.Trigger(hash, data);
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x0009CB1E File Offset: 0x0009AD1E
	public virtual void Update()
	{
	}

	// Token: 0x06001C89 RID: 7305 RVA: 0x0009CB20 File Offset: 0x0009AD20
	public virtual void ShowEditor()
	{
	}

	// Token: 0x040010D2 RID: 4306
	protected Sensors sensors;
}
