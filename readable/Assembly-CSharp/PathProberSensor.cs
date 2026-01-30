using System;

// Token: 0x02000525 RID: 1317
public class PathProberSensor : Sensor
{
	// Token: 0x06001C6E RID: 7278 RVA: 0x0009C774 File Offset: 0x0009A974
	public PathProberSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = sensors.GetComponent<Navigator>();
	}

	// Token: 0x06001C6F RID: 7279 RVA: 0x0009C789 File Offset: 0x0009A989
	public override void Update()
	{
		this.navigator.UpdateProbe(false);
	}

	// Token: 0x040010C8 RID: 4296
	private Navigator navigator;
}
