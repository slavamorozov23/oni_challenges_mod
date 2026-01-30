using System;
using KSerialization;

// Token: 0x02000D91 RID: 3473
public class EODReportMessage : Message
{
	// Token: 0x06006C24 RID: 27684 RVA: 0x00290D22 File Offset: 0x0028EF22
	public EODReportMessage(string title, string tooltip)
	{
		this.day = GameUtil.GetCurrentCycle();
		this.title = title;
		this.tooltip = tooltip;
	}

	// Token: 0x06006C25 RID: 27685 RVA: 0x00290D43 File Offset: 0x0028EF43
	public EODReportMessage()
	{
	}

	// Token: 0x06006C26 RID: 27686 RVA: 0x00290D4B File Offset: 0x0028EF4B
	public override string GetSound()
	{
		return null;
	}

	// Token: 0x06006C27 RID: 27687 RVA: 0x00290D4E File Offset: 0x0028EF4E
	public override string GetMessageBody()
	{
		return "";
	}

	// Token: 0x06006C28 RID: 27688 RVA: 0x00290D55 File Offset: 0x0028EF55
	public override string GetTooltip()
	{
		return this.tooltip;
	}

	// Token: 0x06006C29 RID: 27689 RVA: 0x00290D5D File Offset: 0x0028EF5D
	public override string GetTitle()
	{
		return this.title;
	}

	// Token: 0x06006C2A RID: 27690 RVA: 0x00290D65 File Offset: 0x0028EF65
	public void OpenReport()
	{
		ManagementMenu.Instance.OpenReports(this.day);
	}

	// Token: 0x06006C2B RID: 27691 RVA: 0x00290D77 File Offset: 0x0028EF77
	public override bool ShowDialog()
	{
		return false;
	}

	// Token: 0x06006C2C RID: 27692 RVA: 0x00290D7A File Offset: 0x0028EF7A
	public override void OnClick()
	{
		this.OpenReport();
	}

	// Token: 0x04004A25 RID: 18981
	[Serialize]
	private int day;

	// Token: 0x04004A26 RID: 18982
	[Serialize]
	private string title;

	// Token: 0x04004A27 RID: 18983
	[Serialize]
	private string tooltip;
}
