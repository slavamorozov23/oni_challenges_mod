using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Database;

// Token: 0x02000855 RID: 2133
public class ColonyAchievementStatus
{
	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x06003A9F RID: 15007 RVA: 0x0014737A File Offset: 0x0014557A
	public List<ColonyAchievementRequirement> Requirements
	{
		get
		{
			return this.m_achievement.requirementChecklist;
		}
	}

	// Token: 0x06003AA0 RID: 15008 RVA: 0x00147387 File Offset: 0x00145587
	public ColonyAchievementStatus(string achievementId)
	{
		this.m_achievement = Db.Get().ColonyAchievements.TryGet(achievementId);
		if (this.m_achievement == null)
		{
			this.m_achievement = new ColonyAchievement();
		}
	}

	// Token: 0x06003AA1 RID: 15009 RVA: 0x001473B8 File Offset: 0x001455B8
	public void UpdateAchievement()
	{
		if (this.Requirements.Count <= 0)
		{
			return;
		}
		if (this.m_achievement.Disabled)
		{
			return;
		}
		this.success = true;
		foreach (ColonyAchievementRequirement colonyAchievementRequirement in this.Requirements)
		{
			this.success &= colonyAchievementRequirement.Success();
			this.failed |= colonyAchievementRequirement.Fail();
		}
	}

	// Token: 0x06003AA2 RID: 15010 RVA: 0x00147450 File Offset: 0x00145650
	public static ColonyAchievementStatus Deserialize(IReader reader, string achievementId)
	{
		bool flag = reader.ReadByte() > 0;
		bool flag2 = reader.ReadByte() > 0;
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 22))
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				Type type = Type.GetType(reader.ReadKleiString());
				if (type != null)
				{
					AchievementRequirementSerialization_Deprecated achievementRequirementSerialization_Deprecated = FormatterServices.GetUninitializedObject(type) as AchievementRequirementSerialization_Deprecated;
					Debug.Assert(achievementRequirementSerialization_Deprecated != null, string.Format("Cannot deserialize old data for type {0}", type));
					achievementRequirementSerialization_Deprecated.Deserialize(reader);
				}
			}
		}
		return new ColonyAchievementStatus(achievementId)
		{
			success = flag,
			failed = flag2
		};
	}

	// Token: 0x06003AA3 RID: 15011 RVA: 0x001474F1 File Offset: 0x001456F1
	public void Serialize(BinaryWriter writer)
	{
		writer.Write(this.success ? 1 : 0);
		writer.Write(this.failed ? 1 : 0);
	}

	// Token: 0x0400239C RID: 9116
	public bool success;

	// Token: 0x0400239D RID: 9117
	public bool failed;

	// Token: 0x0400239E RID: 9118
	private ColonyAchievement m_achievement;
}
