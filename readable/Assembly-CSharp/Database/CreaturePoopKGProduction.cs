using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F97 RID: 3991
	public class CreaturePoopKGProduction : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DE7 RID: 32231 RVA: 0x0031FE11 File Offset: 0x0031E011
		public CreaturePoopKGProduction(Tag poopElement, float amountToPoop)
		{
			this.poopElement = poopElement;
			this.amountToPoop = amountToPoop;
		}

		// Token: 0x06007DE8 RID: 32232 RVA: 0x0031FE28 File Offset: 0x0031E028
		public override bool Success()
		{
			return Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(this.poopElement) && Game.Instance.savedInfo.creaturePoopAmount[this.poopElement] >= this.amountToPoop;
		}

		// Token: 0x06007DE9 RID: 32233 RVA: 0x0031FE78 File Offset: 0x0031E078
		public void Deserialize(IReader reader)
		{
			this.amountToPoop = reader.ReadSingle();
			string name = reader.ReadKleiString();
			this.poopElement = new Tag(name);
		}

		// Token: 0x06007DEA RID: 32234 RVA: 0x0031FEA4 File Offset: 0x0031E0A4
		public override string GetProgress(bool complete)
		{
			float num = 0f;
			Game.Instance.savedInfo.creaturePoopAmount.TryGetValue(this.poopElement, out num);
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.POOP_PRODUCTION, GameUtil.GetFormattedMass(complete ? this.amountToPoop : num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"), GameUtil.GetFormattedMass(this.amountToPoop, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"));
		}

		// Token: 0x04005C53 RID: 23635
		private Tag poopElement;

		// Token: 0x04005C54 RID: 23636
		private float amountToPoop;
	}
}
