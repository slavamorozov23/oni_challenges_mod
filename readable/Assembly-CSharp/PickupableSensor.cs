using System;

// Token: 0x02000526 RID: 1318
public class PickupableSensor : Sensor
{
	// Token: 0x06001C70 RID: 7280 RVA: 0x0009C797 File Offset: 0x0009A997
	public PickupableSensor(Sensors sensors) : base(sensors)
	{
		this.worker = base.GetComponent<WorkerBase>();
		this.navigator = base.GetComponent<Navigator>();
	}

	// Token: 0x06001C71 RID: 7281 RVA: 0x0009C7B8 File Offset: 0x0009A9B8
	public override void Update()
	{
		GlobalChoreProvider.Instance.UpdateFetches(this.navigator);
		Game.Instance.fetchManager.UpdatePickups(this.navigator, this.worker);
	}

	// Token: 0x040010C9 RID: 4297
	private Navigator navigator;

	// Token: 0x040010CA RID: 4298
	private WorkerBase worker;
}
