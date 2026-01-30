using System;
using KSerialization;
using STRINGS;

// Token: 0x02000D9A RID: 3482
public class POITechResearchCompleteMessage : Message
{
	// Token: 0x06006C6D RID: 27757 RVA: 0x002915C0 File Offset: 0x0028F7C0
	public POITechResearchCompleteMessage()
	{
	}

	// Token: 0x06006C6E RID: 27758 RVA: 0x002915C8 File Offset: 0x0028F7C8
	public POITechResearchCompleteMessage(POITechItemUnlocks.Def unlocked_items)
	{
		this.unlockedItemsdef = unlocked_items;
		this.popupName = unlocked_items.PopUpName;
		this.animName = unlocked_items.animName;
	}

	// Token: 0x06006C6F RID: 27759 RVA: 0x002915F4 File Offset: 0x0028F7F4
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x06006C70 RID: 27760 RVA: 0x002915FC File Offset: 0x0028F7FC
	public override string GetMessageBody()
	{
		string text = "";
		for (int i = 0; i < this.unlockedItemsdef.POITechUnlockIDs.Count; i++)
		{
			TechItem techItem = Db.Get().TechItems.TryGet(this.unlockedItemsdef.POITechUnlockIDs[i]);
			if (techItem != null)
			{
				text = text + "\n    • " + techItem.Name;
			}
		}
		return string.Format(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE_NOLORE.MESSAGEBODY, text);
	}

	// Token: 0x06006C71 RID: 27761 RVA: 0x00291670 File Offset: 0x0028F870
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE_NOLORE.NAME;
	}

	// Token: 0x06006C72 RID: 27762 RVA: 0x0029167C File Offset: 0x0028F87C
	public override string GetTooltip()
	{
		return string.Format(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE_NOLORE.TOOLTIP, this.popupName);
	}

	// Token: 0x06006C73 RID: 27763 RVA: 0x00291693 File Offset: 0x0028F893
	public override bool IsValid()
	{
		return this.unlockedItemsdef != null;
	}

	// Token: 0x06006C74 RID: 27764 RVA: 0x0029169E File Offset: 0x0028F89E
	public override bool ShowDialog()
	{
		EventInfoData eventInfoData = new EventInfoData(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE_NOLORE.NAME, this.GetMessageBody(), this.animName);
		eventInfoData.AddDefaultOption(null);
		EventInfoScreen.ShowPopup(eventInfoData);
		Messenger.Instance.RemoveMessage(this);
		return false;
	}

	// Token: 0x06006C75 RID: 27765 RVA: 0x002916DA File Offset: 0x0028F8DA
	public override bool ShowDismissButton()
	{
		return false;
	}

	// Token: 0x06006C76 RID: 27766 RVA: 0x002916DD File Offset: 0x0028F8DD
	public override NotificationType GetMessageType()
	{
		return NotificationType.Messages;
	}

	// Token: 0x04004A42 RID: 19010
	[Serialize]
	public POITechItemUnlocks.Def unlockedItemsdef;

	// Token: 0x04004A43 RID: 19011
	[Serialize]
	public string popupName;

	// Token: 0x04004A44 RID: 19012
	[Serialize]
	public string animName;
}
