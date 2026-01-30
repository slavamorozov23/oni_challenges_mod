using System;

// Token: 0x02000BD5 RID: 3029
public readonly struct SpaceScannerTarget
{
	// Token: 0x06005AC7 RID: 23239 RVA: 0x0020E003 File Offset: 0x0020C203
	private SpaceScannerTarget(string id)
	{
		this.id = id;
	}

	// Token: 0x06005AC8 RID: 23240 RVA: 0x0020E00C File Offset: 0x0020C20C
	public static SpaceScannerTarget MeteorShower()
	{
		return new SpaceScannerTarget("meteor_shower");
	}

	// Token: 0x06005AC9 RID: 23241 RVA: 0x0020E018 File Offset: 0x0020C218
	public static SpaceScannerTarget BallisticObject()
	{
		return new SpaceScannerTarget("ballistic_object");
	}

	// Token: 0x06005ACA RID: 23242 RVA: 0x0020E024 File Offset: 0x0020C224
	public static SpaceScannerTarget RocketBaseGame(LaunchConditionManager rocket)
	{
		return new SpaceScannerTarget(string.Format("rocket_base_game::{0}", rocket.GetComponent<KPrefabID>().InstanceID));
	}

	// Token: 0x06005ACB RID: 23243 RVA: 0x0020E045 File Offset: 0x0020C245
	public static SpaceScannerTarget RocketDlc1(Clustercraft rocket)
	{
		return new SpaceScannerTarget(string.Format("rocket_dlc1::{0}", rocket.GetComponent<KPrefabID>().InstanceID));
	}

	// Token: 0x04003C87 RID: 15495
	public readonly string id;
}
