using System;

namespace Database
{
	// Token: 0x02000F34 RID: 3892
	public class CritterEmotions : ResourceSet<Thought>
	{
		// Token: 0x06007C57 RID: 31831 RVA: 0x0030E974 File Offset: 0x0030CB74
		public CritterEmotions(ResourceSet parent) : base("CritterEmotions", parent)
		{
			this.Hungry = new CritterEmotion("Hungry", false, Assets.GetSprite("crew_state_hungry"));
			this.Hot = new CritterEmotion("Hot", false, Assets.GetSprite("crew_state_temp_up"));
			this.Cold = new CritterEmotion("Cold", false, Assets.GetSprite("crew_state_temp_down"));
			this.Cramped = new CritterEmotion("Cramped", false, Assets.GetSprite("crew_state_stress"));
			this.Crowded = new CritterEmotion("Crowded", false, Assets.GetSprite("crew_state_stress"));
			this.Suffocating = new CritterEmotion("Suffocating", false, Assets.GetSprite("crew_state_cantbreathe"));
			this.WellFed = new CritterEmotion("WellFed", true, Assets.GetSprite("crew_state_binge_eat"));
			this.Happy = new CritterEmotion("Happy", true, Assets.GetSprite("crew_state_happy"));
		}

		// Token: 0x04005946 RID: 22854
		public CritterEmotion Hungry;

		// Token: 0x04005947 RID: 22855
		public CritterEmotion Hot;

		// Token: 0x04005948 RID: 22856
		public CritterEmotion Cold;

		// Token: 0x04005949 RID: 22857
		public CritterEmotion Cramped;

		// Token: 0x0400594A RID: 22858
		public CritterEmotion Crowded;

		// Token: 0x0400594B RID: 22859
		public CritterEmotion Suffocating;

		// Token: 0x0400594C RID: 22860
		public CritterEmotion WellFed;

		// Token: 0x0400594D RID: 22861
		public CritterEmotion Happy;
	}
}
