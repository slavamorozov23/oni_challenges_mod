using System;
using UnityEngine;

// Token: 0x02000D08 RID: 3336
public class EventInfoDataHelper
{
	// Token: 0x0600673E RID: 26430 RVA: 0x0026E880 File Offset: 0x0026CA80
	public static EventInfoData GenerateStoryTraitData(string titleText, string descriptionText, string buttonText, string animFileName, EventInfoDataHelper.PopupType popupType, string buttonTooltip = null, GameObject[] minions = null, System.Action callback = null)
	{
		EventInfoData eventInfoData = new EventInfoData(titleText, descriptionText, animFileName);
		eventInfoData.minions = minions;
		if (popupType <= EventInfoDataHelper.PopupType.NORMAL || popupType != EventInfoDataHelper.PopupType.COMPLETE)
		{
			eventInfoData.showCallback = delegate()
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound("StoryTrait_Activation_Popup", false));
			};
		}
		else
		{
			eventInfoData.showCallback = delegate()
			{
				MusicManager.instance.PlaySong("Stinger_StoryTraitUnlock", false);
			};
		}
		EventInfoData.Option option = eventInfoData.AddOption(buttonText, null);
		option.callback = callback;
		option.tooltip = buttonTooltip;
		return eventInfoData;
	}

	// Token: 0x02001F36 RID: 7990
	public enum PopupType
	{
		// Token: 0x040091ED RID: 37357
		NONE = -1,
		// Token: 0x040091EE RID: 37358
		BEGIN,
		// Token: 0x040091EF RID: 37359
		NORMAL,
		// Token: 0x040091F0 RID: 37360
		COMPLETE
	}
}
