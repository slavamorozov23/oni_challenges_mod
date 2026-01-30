using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F82 RID: 3970
	public class EquipNDupes : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007D8F RID: 32143 RVA: 0x0031E4B5 File Offset: 0x0031C6B5
		public EquipNDupes(AssignableSlot equipmentSlot, int numToEquip)
		{
			this.equipmentSlot = equipmentSlot;
			this.numToEquip = numToEquip;
		}

		// Token: 0x06007D90 RID: 32144 RVA: 0x0031E4CC File Offset: 0x0031C6CC
		public override bool Success()
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Equipment equipment = minionIdentity.GetEquipment();
				if (equipment != null && equipment.IsSlotOccupied(this.equipmentSlot))
				{
					num++;
				}
			}
			return num >= this.numToEquip;
		}

		// Token: 0x06007D91 RID: 32145 RVA: 0x0031E54C File Offset: 0x0031C74C
		public void Deserialize(IReader reader)
		{
			string id = reader.ReadKleiString();
			this.equipmentSlot = Db.Get().AssignableSlots.Get(id);
			this.numToEquip = reader.ReadInt32();
		}

		// Token: 0x06007D92 RID: 32146 RVA: 0x0031E584 File Offset: 0x0031C784
		public override string GetProgress(bool complete)
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Equipment equipment = minionIdentity.GetEquipment();
				if (equipment != null && equipment.IsSlotOccupied(this.equipmentSlot))
				{
					num++;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CLOTHE_DUPES, complete ? this.numToEquip : num, this.numToEquip);
		}

		// Token: 0x04005C36 RID: 23606
		private AssignableSlot equipmentSlot;

		// Token: 0x04005C37 RID: 23607
		private int numToEquip;
	}
}
