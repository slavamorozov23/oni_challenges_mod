using System;
using KSerialization;
using STRINGS;

// Token: 0x02000D9D RID: 3485
public class SkillMasteredMessage : Message
{
	// Token: 0x06006C89 RID: 27785 RVA: 0x00291A90 File Offset: 0x0028FC90
	public SkillMasteredMessage()
	{
	}

	// Token: 0x06006C8A RID: 27786 RVA: 0x00291A98 File Offset: 0x0028FC98
	public SkillMasteredMessage(MinionResume resume)
	{
		this.minionName = resume.GetProperName();
	}

	// Token: 0x06006C8B RID: 27787 RVA: 0x00291AAC File Offset: 0x0028FCAC
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x06006C8C RID: 27788 RVA: 0x00291AB4 File Offset: 0x0028FCB4
	public override string GetMessageBody()
	{
		Debug.Assert(this.minionName != null);
		string arg = string.Format(MISC.NOTIFICATIONS.SKILL_POINT_EARNED.LINE, this.minionName);
		return string.Format(MISC.NOTIFICATIONS.SKILL_POINT_EARNED.MESSAGEBODY, arg);
	}

	// Token: 0x06006C8D RID: 27789 RVA: 0x00291AF5 File Offset: 0x0028FCF5
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", this.minionName);
	}

	// Token: 0x06006C8E RID: 27790 RVA: 0x00291B0C File Offset: 0x0028FD0C
	public override string GetTooltip()
	{
		return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.TOOLTIP.Replace("{Duplicant}", this.minionName);
	}

	// Token: 0x06006C8F RID: 27791 RVA: 0x00291B23 File Offset: 0x0028FD23
	public override bool IsValid()
	{
		return this.minionName != null;
	}

	// Token: 0x04004A48 RID: 19016
	[Serialize]
	private string minionName;
}
