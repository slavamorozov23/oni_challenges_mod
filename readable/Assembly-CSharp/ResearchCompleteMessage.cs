using System;
using KSerialization;
using STRINGS;

// Token: 0x02000D9B RID: 3483
public class ResearchCompleteMessage : Message
{
	// Token: 0x06006C77 RID: 27767 RVA: 0x002916E0 File Offset: 0x0028F8E0
	public ResearchCompleteMessage()
	{
	}

	// Token: 0x06006C78 RID: 27768 RVA: 0x002916F3 File Offset: 0x0028F8F3
	public ResearchCompleteMessage(Tech tech)
	{
		this.tech.Set(tech);
	}

	// Token: 0x06006C79 RID: 27769 RVA: 0x00291712 File Offset: 0x0028F912
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x06006C7A RID: 27770 RVA: 0x0029171C File Offset: 0x0028F91C
	public override string GetMessageBody()
	{
		Tech tech = this.tech.Get();
		string text = "";
		for (int i = 0; i < tech.unlockedItems.Count; i++)
		{
			if (i != 0)
			{
				text += ", ";
			}
			text += tech.unlockedItems[i].Name;
		}
		return string.Format(MISC.NOTIFICATIONS.RESEARCHCOMPLETE.MESSAGEBODY, tech.Name, text);
	}

	// Token: 0x06006C7B RID: 27771 RVA: 0x0029178E File Offset: 0x0028F98E
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.RESEARCHCOMPLETE.NAME;
	}

	// Token: 0x06006C7C RID: 27772 RVA: 0x0029179C File Offset: 0x0028F99C
	public override string GetTooltip()
	{
		Tech tech = this.tech.Get();
		return string.Format(MISC.NOTIFICATIONS.RESEARCHCOMPLETE.TOOLTIP, tech.Name);
	}

	// Token: 0x06006C7D RID: 27773 RVA: 0x002917CA File Offset: 0x0028F9CA
	public override bool IsValid()
	{
		return this.tech.Get() != null;
	}

	// Token: 0x04004A45 RID: 19013
	[Serialize]
	private ResourceRef<Tech> tech = new ResourceRef<Tech>();
}
