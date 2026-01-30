using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F8B RID: 3979
	public class TravelXUsingTransitTubes : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DB6 RID: 32182 RVA: 0x0031F0BA File Offset: 0x0031D2BA
		public TravelXUsingTransitTubes(NavType navType, int distanceToTravel)
		{
			this.navType = navType;
			this.distanceToTravel = distanceToTravel;
		}

		// Token: 0x06007DB7 RID: 32183 RVA: 0x0031F0D0 File Offset: 0x0031D2D0
		public override bool Success()
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Navigator component = minionIdentity.GetComponent<Navigator>();
				if (component != null && component.distanceTravelledByNavType.ContainsKey(this.navType))
				{
					num += component.distanceTravelledByNavType[this.navType];
				}
			}
			return num >= this.distanceToTravel;
		}

		// Token: 0x06007DB8 RID: 32184 RVA: 0x0031F164 File Offset: 0x0031D364
		public void Deserialize(IReader reader)
		{
			byte b = reader.ReadByte();
			this.navType = (NavType)b;
			this.distanceToTravel = reader.ReadInt32();
		}

		// Token: 0x06007DB9 RID: 32185 RVA: 0x0031F18C File Offset: 0x0031D38C
		public override string GetProgress(bool complete)
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Navigator component = minionIdentity.GetComponent<Navigator>();
				if (component != null && component.distanceTravelledByNavType.ContainsKey(this.navType))
				{
					num += component.distanceTravelledByNavType[this.navType];
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TRAVELED_IN_TUBES, complete ? this.distanceToTravel : num, this.distanceToTravel);
		}

		// Token: 0x04005C47 RID: 23623
		private int distanceToTravel;

		// Token: 0x04005C48 RID: 23624
		private NavType navType;
	}
}
