using System;
using Klei.AI;

// Token: 0x02000434 RID: 1076
[Serializable]
public struct SpiceInstance
{
	// Token: 0x17000066 RID: 102
	// (get) Token: 0x0600164B RID: 5707 RVA: 0x0007F062 File Offset: 0x0007D262
	public AttributeModifier CalorieModifier
	{
		get
		{
			return SpiceGrinder.SettingOptions[this.Id].Spice.CalorieModifier;
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x0600164C RID: 5708 RVA: 0x0007F07E File Offset: 0x0007D27E
	public AttributeModifier FoodModifier
	{
		get
		{
			return SpiceGrinder.SettingOptions[this.Id].Spice.FoodModifier;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x0600164D RID: 5709 RVA: 0x0007F09A File Offset: 0x0007D29A
	public Effect StatBonus
	{
		get
		{
			return SpiceGrinder.SettingOptions[this.Id].StatBonus;
		}
	}

	// Token: 0x04000D44 RID: 3396
	public Tag Id;

	// Token: 0x04000D45 RID: 3397
	public float TotalKG;
}
