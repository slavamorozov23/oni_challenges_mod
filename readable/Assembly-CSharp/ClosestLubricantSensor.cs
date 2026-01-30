using System;

// Token: 0x02000520 RID: 1312
public class ClosestLubricantSensor : ClosestPickupableSensor<Pickupable>
{
	// Token: 0x06001C5C RID: 7260 RVA: 0x0009C1DB File Offset: 0x0009A3DB
	public ClosestLubricantSensor(Sensors sensors, bool shouldStartActive) : base(sensors, GameTags.SolidLubricant, shouldStartActive)
	{
	}
}
