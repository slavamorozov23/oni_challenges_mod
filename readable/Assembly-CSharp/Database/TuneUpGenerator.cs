using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F95 RID: 3989
	public class TuneUpGenerator : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DDF RID: 32223 RVA: 0x0031FB6F File Offset: 0x0031DD6F
		public TuneUpGenerator(float numChoreseToComplete)
		{
			this.numChoreseToComplete = numChoreseToComplete;
		}

		// Token: 0x06007DE0 RID: 32224 RVA: 0x0031FB80 File Offset: 0x0031DD80
		public override bool Success()
		{
			float num = 0f;
			ReportManager.ReportEntry entry = ReportManager.Instance.TodaysReport.GetEntry(ReportManager.ReportType.ChoreStatus);
			for (int i = 0; i < entry.contextEntries.Count; i++)
			{
				ReportManager.ReportEntry reportEntry = entry.contextEntries[i];
				if (reportEntry.context == Db.Get().ChoreTypes.PowerTinker.Name)
				{
					num += reportEntry.Negative;
				}
			}
			string name = Db.Get().ChoreTypes.PowerTinker.Name;
			int count = ReportManager.Instance.reports.Count;
			for (int j = 0; j < count; j++)
			{
				ReportManager.ReportEntry entry2 = ReportManager.Instance.reports[j].GetEntry(ReportManager.ReportType.ChoreStatus);
				int count2 = entry2.contextEntries.Count;
				for (int k = 0; k < count2; k++)
				{
					ReportManager.ReportEntry reportEntry2 = entry2.contextEntries[k];
					if (reportEntry2.context == name)
					{
						num += reportEntry2.Negative;
					}
				}
			}
			this.choresCompleted = Math.Abs(num);
			return Math.Abs(num) >= this.numChoreseToComplete;
		}

		// Token: 0x06007DE1 RID: 32225 RVA: 0x0031FCAC File Offset: 0x0031DEAC
		public void Deserialize(IReader reader)
		{
			this.numChoreseToComplete = reader.ReadSingle();
		}

		// Token: 0x06007DE2 RID: 32226 RVA: 0x0031FCBC File Offset: 0x0031DEBC
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CHORES_OF_TYPE, complete ? this.numChoreseToComplete : this.choresCompleted, this.numChoreseToComplete, Db.Get().ChoreTypes.PowerTinker.Name);
		}

		// Token: 0x04005C4F RID: 23631
		private float numChoreseToComplete;

		// Token: 0x04005C50 RID: 23632
		private float choresCompleted;
	}
}
