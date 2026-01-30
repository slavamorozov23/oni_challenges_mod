using System;
using UnityEngine;

// Token: 0x02000B91 RID: 2961
public interface ILaunchableRocket
{
	// Token: 0x1700065E RID: 1630
	// (get) Token: 0x0600586C RID: 22636
	LaunchableRocketRegisterType registerType { get; }

	// Token: 0x1700065F RID: 1631
	// (get) Token: 0x0600586D RID: 22637
	GameObject LaunchableGameObject { get; }

	// Token: 0x17000660 RID: 1632
	// (get) Token: 0x0600586E RID: 22638
	float rocketSpeed { get; }

	// Token: 0x17000661 RID: 1633
	// (get) Token: 0x0600586F RID: 22639
	bool isLanding { get; }
}
