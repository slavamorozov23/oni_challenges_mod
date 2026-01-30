using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000F7D RID: 3965
	public class BuildRoomType : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007D7A RID: 32122 RVA: 0x0031DF45 File Offset: 0x0031C145
		public BuildRoomType(RoomType roomType)
		{
			this.roomType = roomType;
		}

		// Token: 0x06007D7B RID: 32123 RVA: 0x0031DF54 File Offset: 0x0031C154
		public override bool Success()
		{
			using (List<Room>.Enumerator enumerator = Game.Instance.roomProber.rooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.roomType == this.roomType)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007D7C RID: 32124 RVA: 0x0031DFBC File Offset: 0x0031C1BC
		public void Deserialize(IReader reader)
		{
			string id = reader.ReadKleiString();
			this.roomType = Db.Get().RoomTypes.Get(id);
		}

		// Token: 0x06007D7D RID: 32125 RVA: 0x0031DFE6 File Offset: 0x0031C1E6
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_A_ROOM, this.roomType.Name);
		}

		// Token: 0x04005C31 RID: 23601
		private RoomType roomType;
	}
}
