using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F71 RID: 3953
	public class ReachedSpace : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007D3A RID: 32058 RVA: 0x0031D83A File Offset: 0x0031BA3A
		public ReachedSpace(SpaceDestinationType destinationType = null)
		{
			this.destinationType = destinationType;
		}

		// Token: 0x06007D3B RID: 32059 RVA: 0x0031D849 File Offset: 0x0031BA49
		public override string Name()
		{
			if (this.destinationType != null)
			{
				return string.Format(COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION, this.destinationType.Name);
			}
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACH_SPACE_ANY_DESTINATION;
		}

		// Token: 0x06007D3C RID: 32060 RVA: 0x0031D878 File Offset: 0x0031BA78
		public override string Description()
		{
			if (this.destinationType != null)
			{
				return string.Format(COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION_DESCRIPTION, this.destinationType.Name);
			}
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACH_SPACE_ANY_DESTINATION_DESCRIPTION;
		}

		// Token: 0x06007D3D RID: 32061 RVA: 0x0031D8A8 File Offset: 0x0031BAA8
		public override bool Success()
		{
			foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
			{
				if (spacecraft.state != Spacecraft.MissionState.Grounded && spacecraft.state != Spacecraft.MissionState.Destroyed)
				{
					SpaceDestination destination = SpacecraftManager.instance.GetDestination(SpacecraftManager.instance.savedSpacecraftDestinations[spacecraft.id]);
					if (this.destinationType == null || destination.GetDestinationType() == this.destinationType)
					{
						if (this.destinationType == Db.Get().SpaceDestinationTypes.Wormhole)
						{
							Game.Instance.unlocks.Unlock("temporaltear", true);
						}
						return true;
					}
				}
			}
			return SpacecraftManager.instance.hasVisitedWormHole;
		}

		// Token: 0x06007D3E RID: 32062 RVA: 0x0031D984 File Offset: 0x0031BB84
		public void Deserialize(IReader reader)
		{
			if (reader.ReadByte() <= 0)
			{
				string id = reader.ReadKleiString();
				this.destinationType = Db.Get().SpaceDestinationTypes.Get(id);
			}
		}

		// Token: 0x06007D3F RID: 32063 RVA: 0x0031D9B9 File Offset: 0x0031BBB9
		public override string GetProgress(bool completed)
		{
			if (this.destinationType == null)
			{
				return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET;
			}
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET_TO_WORMHOLE;
		}

		// Token: 0x04005C2C RID: 23596
		private SpaceDestinationType destinationType;
	}
}
