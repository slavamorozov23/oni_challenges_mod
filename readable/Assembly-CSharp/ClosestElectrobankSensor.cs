using System;
using System.Collections.Generic;

// Token: 0x0200051F RID: 1311
public class ClosestElectrobankSensor : ClosestPickupableSensor<Electrobank>
{
	// Token: 0x06001C5A RID: 7258 RVA: 0x0009C14A File Offset: 0x0009A34A
	public ClosestElectrobankSensor(Sensors sensors, bool shouldStartActive) : base(sensors, GameTags.ChargedPortableBattery, shouldStartActive)
	{
		this.bionicIncompatiobleElectrobankTags = new Tag[GameTags.BionicIncompatibleBatteries.Count];
		GameTags.BionicIncompatibleBatteries.CopyTo(this.bionicIncompatiobleElectrobankTags, 0);
	}

	// Token: 0x06001C5B RID: 7259 RVA: 0x0009C180 File Offset: 0x0009A380
	public override HashSet<Tag> GetForbbidenTags()
	{
		HashSet<Tag> forbbidenTags = base.GetForbbidenTags();
		if (this.bionicIncompatiobleElectrobankTags != null && this.bionicIncompatiobleElectrobankTags.Length != 0)
		{
			HashSet<Tag> hashSet = forbbidenTags;
			foreach (Tag item in this.bionicIncompatiobleElectrobankTags)
			{
				if (!forbbidenTags.Contains(item))
				{
					hashSet.Add(item);
				}
			}
			return hashSet;
		}
		return forbbidenTags;
	}

	// Token: 0x040010B3 RID: 4275
	private Tag[] bionicIncompatiobleElectrobankTags;
}
