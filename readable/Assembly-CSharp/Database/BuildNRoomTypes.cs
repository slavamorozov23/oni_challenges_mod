using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000F7E RID: 3966
	public class BuildNRoomTypes : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007D7E RID: 32126 RVA: 0x0031E002 File Offset: 0x0031C202
		public BuildNRoomTypes(RoomType roomType, int numToCreate = 1)
		{
			this.roomType = roomType;
			this.numToCreate = numToCreate;
		}

		// Token: 0x06007D7F RID: 32127 RVA: 0x0031E018 File Offset: 0x0031C218
		public override bool Success()
		{
			int num = 0;
			using (List<Room>.Enumerator enumerator = Game.Instance.roomProber.rooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.roomType == this.roomType)
					{
						num++;
					}
				}
			}
			return num >= this.numToCreate;
		}

		// Token: 0x06007D80 RID: 32128 RVA: 0x0031E08C File Offset: 0x0031C28C
		public void Deserialize(IReader reader)
		{
			string id = reader.ReadKleiString();
			this.roomType = Db.Get().RoomTypes.Get(id);
			this.numToCreate = reader.ReadInt32();
		}

		// Token: 0x06007D81 RID: 32129 RVA: 0x0031E0C4 File Offset: 0x0031C2C4
		public override string GetProgress(bool complete)
		{
			int num = 0;
			using (List<Room>.Enumerator enumerator = Game.Instance.roomProber.rooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.roomType == this.roomType)
					{
						num++;
					}
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_N_ROOMS, this.roomType.Name, complete ? this.numToCreate : num, this.numToCreate);
		}

		// Token: 0x04005C32 RID: 23602
		private RoomType roomType;

		// Token: 0x04005C33 RID: 23603
		private int numToCreate;
	}
}
