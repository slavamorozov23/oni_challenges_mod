using System;
using STRINGS;

// Token: 0x02000D8B RID: 3467
public class AchievementEarnedMessage : Message
{
	// Token: 0x06006BFA RID: 27642 RVA: 0x00290AD8 File Offset: 0x0028ECD8
	public override bool ShowDialog()
	{
		return false;
	}

	// Token: 0x06006BFB RID: 27643 RVA: 0x00290ADB File Offset: 0x0028ECDB
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x06006BFC RID: 27644 RVA: 0x00290AE2 File Offset: 0x0028ECE2
	public override string GetMessageBody()
	{
		return "";
	}

	// Token: 0x06006BFD RID: 27645 RVA: 0x00290AE9 File Offset: 0x0028ECE9
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.COLONY_ACHIEVEMENT_EARNED.NAME;
	}

	// Token: 0x06006BFE RID: 27646 RVA: 0x00290AF5 File Offset: 0x0028ECF5
	public override string GetTooltip()
	{
		return MISC.NOTIFICATIONS.COLONY_ACHIEVEMENT_EARNED.TOOLTIP;
	}

	// Token: 0x06006BFF RID: 27647 RVA: 0x00290B01 File Offset: 0x0028ED01
	public override bool IsValid()
	{
		return true;
	}

	// Token: 0x06006C00 RID: 27648 RVA: 0x00290B04 File Offset: 0x0028ED04
	public override void OnClick()
	{
		RetireColonyUtility.SaveColonySummaryData();
		MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
	}
}
