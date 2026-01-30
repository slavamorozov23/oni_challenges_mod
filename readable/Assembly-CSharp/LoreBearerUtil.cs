using System;
using STRINGS;
using UnityEngine;

// Token: 0x020009F6 RID: 2550
public static class LoreBearerUtil
{
	// Token: 0x06004A6E RID: 19054 RVA: 0x001AF123 File Offset: 0x001AD323
	public static void AddLoreTo(GameObject prefabOrGameObject)
	{
		prefabOrGameObject.AddOrGet<LoreBearer>();
	}

	// Token: 0x06004A6F RID: 19055 RVA: 0x001AF12C File Offset: 0x001AD32C
	public static void AddLoreTo(GameObject prefabOrGameObject, LoreBearerAction unlockLoreFn)
	{
		KPrefabID component = prefabOrGameObject.GetComponent<KPrefabID>();
		if (component.IsInitialized())
		{
			prefabOrGameObject.AddOrGet<LoreBearer>().Internal_SetContent(unlockLoreFn);
			return;
		}
		prefabOrGameObject.AddComponent<LoreBearer>();
		component.prefabInitFn += delegate(GameObject gameObject)
		{
			gameObject.GetComponent<LoreBearer>().Internal_SetContent(unlockLoreFn);
		};
	}

	// Token: 0x06004A70 RID: 19056 RVA: 0x001AF184 File Offset: 0x001AD384
	public static void AddLoreTo(GameObject prefabOrGameObject, string[] collectionsToUnlockFrom)
	{
		KPrefabID component = prefabOrGameObject.GetComponent<KPrefabID>();
		if (component.IsInitialized())
		{
			prefabOrGameObject.AddOrGet<LoreBearer>().Internal_SetContent(LoreBearerUtil.UnlockNextInCollections(collectionsToUnlockFrom));
			return;
		}
		prefabOrGameObject.AddComponent<LoreBearer>();
		component.prefabInitFn += delegate(GameObject gameObject)
		{
			gameObject.GetComponent<LoreBearer>().Internal_SetContent(LoreBearerUtil.UnlockNextInCollections(collectionsToUnlockFrom));
		};
	}

	// Token: 0x06004A71 RID: 19057 RVA: 0x001AF1DE File Offset: 0x001AD3DE
	public static LoreBearerAction UnlockSpecificEntry(string unlockId, string searchDisplayText, bool focus = false)
	{
		return delegate(InfoDialogScreen screen)
		{
			Game.Instance.unlocks.Unlock(unlockId, true);
			screen.AddPlainText(searchDisplayText);
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(unlockId, focus), false);
		};
	}

	// Token: 0x06004A72 RID: 19058 RVA: 0x001AF205 File Offset: 0x001AD405
	public static LoreBearerAction UnlockSpecificEntryThenNext(string unlockId, string searchDisplayText, Action<InfoDialogScreen> next, bool focus = false)
	{
		return delegate(InfoDialogScreen screen)
		{
			if (!Game.Instance.unlocks.IsUnlocked(unlockId))
			{
				Game.Instance.unlocks.Unlock(unlockId, true);
				screen.AddPlainText(searchDisplayText);
				screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(unlockId, focus), false);
				return;
			}
			next(screen);
		};
	}

	// Token: 0x06004A73 RID: 19059 RVA: 0x001AF234 File Offset: 0x001AD434
	public static void UnlockNextEmail(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("emails", false);
		if (text != null)
		{
			string str = "SEARCH" + UnityEngine.Random.Range(1, 6).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_SUCCESS." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
			return;
		}
		string str2 = "SEARCH" + UnityEngine.Random.Range(1, 8).ToString();
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_FAIL." + str2));
	}

	// Token: 0x06004A74 RID: 19060 RVA: 0x001AF2E0 File Offset: 0x001AD4E0
	public static void UnlockNextResearchNote(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("researchnotes", false);
		if (text != null)
		{
			string str = "SEARCH" + UnityEngine.Random.Range(1, 3).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_TECHNOLOGY_SUCCESS." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
			return;
		}
		string str2 = "SEARCH1";
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + str2));
	}

	// Token: 0x06004A75 RID: 19061 RVA: 0x001AF378 File Offset: 0x001AD578
	public static void UnlockNextJournalEntry(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("journals", false);
		if (text != null)
		{
			string str = "SEARCH" + UnityEngine.Random.Range(1, 6).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
			return;
		}
		string str2 = "SEARCH1";
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + str2));
	}

	// Token: 0x06004A76 RID: 19062 RVA: 0x001AF410 File Offset: 0x001AD610
	public static void UnlockNextDimensionalLore(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("dimensionallore", true);
		if (text != null)
		{
			string str = "SEARCH" + UnityEngine.Random.Range(1, 6).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
			return;
		}
		string str2 = "SEARCH1";
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + str2));
	}

	// Token: 0x06004A77 RID: 19063 RVA: 0x001AF4A8 File Offset: 0x001AD6A8
	public static void UnlockNextSpaceEntry(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("space", false);
		if (text != null)
		{
			string str = "SEARCH" + UnityEngine.Random.Range(1, 7).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_SPACEPOI_SUCCESS." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
			return;
		}
		string str2 = "SEARCH" + UnityEngine.Random.Range(1, 4).ToString();
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_SPACEPOI_FAIL." + str2));
	}

	// Token: 0x06004A78 RID: 19064 RVA: 0x001AF554 File Offset: 0x001AD754
	public static void UnlockNextDeskPodiumEntry(InfoDialogScreen screen)
	{
		if (!Game.Instance.unlocks.IsUnlocked("story_trait_critter_manipulator_parking"))
		{
			Game.Instance.unlocks.Unlock("story_trait_critter_manipulator_parking", true);
			string str = "SEARCH" + UnityEngine.Random.Range(1, 1).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_PODIUM." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID("story_trait_critter_manipulator_parking", false), false);
			return;
		}
		string str2 = "SEARCH" + UnityEngine.Random.Range(1, 8).ToString();
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_FAIL." + str2));
	}

	// Token: 0x06004A79 RID: 19065 RVA: 0x001AF616 File Offset: 0x001AD816
	public static LoreBearerAction UnlockNextInCollections(string[] collectionsToUnlockFrom)
	{
		return delegate(InfoDialogScreen screen)
		{
			foreach (string collectionID in collectionsToUnlockFrom)
			{
				string text = Game.Instance.unlocks.UnlockNext(collectionID, false);
				if (text != null)
				{
					screen.AddPlainText(UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS.SEARCH1);
					screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
					return;
				}
			}
			string str = "SEARCH1";
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + str));
		};
	}

	// Token: 0x06004A7A RID: 19066 RVA: 0x001AF62F File Offset: 0x001AD82F
	public static void NerualVacillator(InfoDialogScreen screen)
	{
		Game.Instance.unlocks.Unlock("neuralvacillator", true);
		LoreBearerUtil.UnlockNextResearchNote(screen);
	}

	// Token: 0x06004A7B RID: 19067 RVA: 0x001AF64C File Offset: 0x001AD84C
	public static Action<InfoDialogScreen> OpenCodexByLockKeyID(string key, bool focusContent = false)
	{
		return delegate(InfoDialogScreen dialog)
		{
			dialog.Deactivate();
			ManagementMenu.Instance.OpenCodexToLockId(key, focusContent);
		};
	}

	// Token: 0x06004A7C RID: 19068 RVA: 0x001AF66C File Offset: 0x001AD86C
	public static Action<InfoDialogScreen> OpenCodexByEntryID(string id)
	{
		return delegate(InfoDialogScreen dialog)
		{
			dialog.Deactivate();
			ManagementMenu.Instance.OpenCodexToEntry(id, null);
		};
	}
}
