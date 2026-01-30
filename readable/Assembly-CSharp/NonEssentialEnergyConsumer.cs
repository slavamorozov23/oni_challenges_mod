using System;

// Token: 0x02000A71 RID: 2673
public class NonEssentialEnergyConsumer : EnergyConsumer
{
	// Token: 0x1700054E RID: 1358
	// (get) Token: 0x06004DB7 RID: 19895 RVA: 0x001C387E File Offset: 0x001C1A7E
	// (set) Token: 0x06004DB8 RID: 19896 RVA: 0x001C3886 File Offset: 0x001C1A86
	public override bool IsPowered
	{
		get
		{
			return this.isPowered;
		}
		protected set
		{
			if (value == this.isPowered)
			{
				return;
			}
			this.isPowered = value;
			Action<bool> poweredStateChanged = this.PoweredStateChanged;
			if (poweredStateChanged == null)
			{
				return;
			}
			poweredStateChanged(this.isPowered);
		}
	}

	// Token: 0x040033C6 RID: 13254
	public Action<bool> PoweredStateChanged;

	// Token: 0x040033C7 RID: 13255
	private bool isPowered;
}
