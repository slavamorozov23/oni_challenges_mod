using System;
using System.Linq;
using UnityEngine;

// Token: 0x02000DAC RID: 3500
public readonly struct MinionBrowserScreenConfig
{
	// Token: 0x06006CEF RID: 27887 RVA: 0x002931A4 File Offset: 0x002913A4
	public MinionBrowserScreenConfig(MinionBrowserScreen.GridItem[] items, Option<MinionBrowserScreen.GridItem> defaultSelectedItem)
	{
		this.items = items;
		this.defaultSelectedItem = defaultSelectedItem;
		this.isValid = true;
	}

	// Token: 0x06006CF0 RID: 27888 RVA: 0x002931BC File Offset: 0x002913BC
	public static MinionBrowserScreenConfig Personalities(Option<Personality> defaultSelectedPersonality = default(Option<Personality>))
	{
		MinionBrowserScreen.GridItem.PersonalityTarget[] items = (from personality in Db.Get().Personalities.GetAll(true, false)
		select MinionBrowserScreen.GridItem.Of(personality)).ToArray<MinionBrowserScreen.GridItem.PersonalityTarget>();
		Option<MinionBrowserScreen.GridItem> option = defaultSelectedPersonality.AndThen<MinionBrowserScreen.GridItem>((Personality personality) => items.FirstOrDefault((MinionBrowserScreen.GridItem.PersonalityTarget item) => item.personality == personality));
		if (option.IsNone() && items.Length != 0)
		{
			option = items[0];
		}
		MinionBrowserScreen.GridItem[] array = items;
		return new MinionBrowserScreenConfig(array, option);
	}

	// Token: 0x06006CF1 RID: 27889 RVA: 0x00293254 File Offset: 0x00291454
	public static MinionBrowserScreenConfig MinionInstances(Option<GameObject> defaultSelectedMinionInstance = default(Option<GameObject>))
	{
		MinionBrowserScreen.GridItem.MinionInstanceTarget[] items = (from minionIdentity in Components.MinionIdentities.Items
		select MinionBrowserScreen.GridItem.Of(minionIdentity.gameObject)).ToArray<MinionBrowserScreen.GridItem.MinionInstanceTarget>();
		Option<MinionBrowserScreen.GridItem> option = defaultSelectedMinionInstance.AndThen<MinionBrowserScreen.GridItem>((GameObject minionInstance) => items.FirstOrDefault((MinionBrowserScreen.GridItem.MinionInstanceTarget item) => item.minionInstance == minionInstance));
		if (option.IsNone() && items.Length != 0)
		{
			option = items[0];
		}
		MinionBrowserScreen.GridItem[] array = items;
		return new MinionBrowserScreenConfig(array, option);
	}

	// Token: 0x06006CF2 RID: 27890 RVA: 0x002932E4 File Offset: 0x002914E4
	public void ApplyAndOpenScreen(System.Action onClose = null, ClothingOutfitUtility.OutfitType outfitType = ClothingOutfitUtility.OutfitType.Clothing)
	{
		LockerNavigator.Instance.duplicantCatalogueScreen.GetComponent<MinionBrowserScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.duplicantCatalogueScreen, onClose);
		LockerNavigator.Instance.duplicantCatalogueScreen.GetComponent<MinionBrowserScreen>().Cycler.GoTo((int)outfitType);
	}

	// Token: 0x04004A7B RID: 19067
	public readonly MinionBrowserScreen.GridItem[] items;

	// Token: 0x04004A7C RID: 19068
	public readonly Option<MinionBrowserScreen.GridItem> defaultSelectedItem;

	// Token: 0x04004A7D RID: 19069
	public readonly bool isValid;
}
