using System;
using Database;
using UnityEngine;

// Token: 0x02000D34 RID: 3380
public readonly struct JoyResponseScreenConfig
{
	// Token: 0x0600686F RID: 26735 RVA: 0x0027902F File Offset: 0x0027722F
	private JoyResponseScreenConfig(JoyResponseOutfitTarget target, Option<JoyResponseDesignerScreen.GalleryItem> initalSelectedItem)
	{
		this.target = target;
		this.initalSelectedItem = initalSelectedItem;
		this.isValid = true;
	}

	// Token: 0x06006870 RID: 26736 RVA: 0x00279046 File Offset: 0x00277246
	public JoyResponseScreenConfig WithInitialSelection(Option<BalloonArtistFacadeResource> initialSelectedItem)
	{
		return new JoyResponseScreenConfig(this.target, JoyResponseDesignerScreen.GalleryItem.Of(initialSelectedItem));
	}

	// Token: 0x06006871 RID: 26737 RVA: 0x0027905E File Offset: 0x0027725E
	public static JoyResponseScreenConfig Minion(GameObject minionInstance)
	{
		return new JoyResponseScreenConfig(JoyResponseOutfitTarget.FromMinion(minionInstance), Option.None);
	}

	// Token: 0x06006872 RID: 26738 RVA: 0x00279075 File Offset: 0x00277275
	public static JoyResponseScreenConfig Personality(Personality personality)
	{
		return new JoyResponseScreenConfig(JoyResponseOutfitTarget.FromPersonality(personality), Option.None);
	}

	// Token: 0x06006873 RID: 26739 RVA: 0x0027908C File Offset: 0x0027728C
	public static JoyResponseScreenConfig From(MinionBrowserScreen.GridItem item)
	{
		MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget = item as MinionBrowserScreen.GridItem.PersonalityTarget;
		if (personalityTarget != null)
		{
			return JoyResponseScreenConfig.Personality(personalityTarget.personality);
		}
		MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget = item as MinionBrowserScreen.GridItem.MinionInstanceTarget;
		if (minionInstanceTarget != null)
		{
			return JoyResponseScreenConfig.Minion(minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	// Token: 0x06006874 RID: 26740 RVA: 0x002790CA File Offset: 0x002772CA
	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.joyResponseDesignerScreen.GetComponent<JoyResponseDesignerScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.joyResponseDesignerScreen, null);
	}

	// Token: 0x040047BE RID: 18366
	public readonly JoyResponseOutfitTarget target;

	// Token: 0x040047BF RID: 18367
	public readonly Option<JoyResponseDesignerScreen.GalleryItem> initalSelectedItem;

	// Token: 0x040047C0 RID: 18368
	public readonly bool isValid;
}
