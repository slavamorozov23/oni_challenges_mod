using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000EA6 RID: 3750
public class StoryContentPanel : KMonoBehaviour
{
	// Token: 0x0600781B RID: 30747 RVA: 0x002E31C4 File Offset: 0x002E13C4
	public List<string> GetActiveStories()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair in this.storyStates)
		{
			if (keyValuePair.Value == StoryContentPanel.StoryState.Guaranteed)
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x0600781C RID: 30748 RVA: 0x002E3230 File Offset: 0x002E1430
	public void Init()
	{
		this.SpawnRows();
		this.RefreshRows();
		this.RefreshDescriptionPanel();
		this.SelectDefault();
		CustomGameSettings.Instance.OnStorySettingChanged += this.OnStorySettingChanged;
	}

	// Token: 0x0600781D RID: 30749 RVA: 0x002E3260 File Offset: 0x002E1460
	public void Cleanup()
	{
		CustomGameSettings.Instance.OnStorySettingChanged -= this.OnStorySettingChanged;
	}

	// Token: 0x0600781E RID: 30750 RVA: 0x002E3278 File Offset: 0x002E1478
	private void OnStorySettingChanged(SettingConfig config, SettingLevel level)
	{
		this.storyStates[config.id] = ((level.id == "Guaranteed") ? StoryContentPanel.StoryState.Guaranteed : StoryContentPanel.StoryState.Forbidden);
		this.RefreshStoryDisplay(config.id);
	}

	// Token: 0x0600781F RID: 30751 RVA: 0x002E32B0 File Offset: 0x002E14B0
	private void SpawnRows()
	{
		using (List<Story>.Enumerator enumerator = Db.Get().Stories.resources.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Story story = enumerator.Current;
				GameObject gameObject = global::Util.KInstantiateUI(this.storyRowPrefab, this.storyRowContainer, true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Label").SetText(Strings.Get(story.StoryTrait.name));
				MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
				component2.onClick = (System.Action)Delegate.Combine(component2.onClick, new System.Action(delegate()
				{
					this.SelectRow(story.Id);
				}));
				this.storyRows.Add(story.Id, gameObject);
				component.GetReference<Image>("Icon").sprite = Assets.GetSprite(story.StoryTrait.icon);
				MultiToggle reference = component.GetReference<MultiToggle>("checkbox");
				reference.onClick = (System.Action)Delegate.Combine(reference.onClick, new System.Action(delegate()
				{
					this.IncrementStorySetting(story.Id, true);
					this.RefreshStoryDisplay(story.Id);
				}));
				this.storyStates.Add(story.Id, this._defaultStoryState);
			}
		}
		this.RefreshAllStoryStates();
		this.mainScreen.RefreshStoryLabel();
	}

	// Token: 0x06007820 RID: 30752 RVA: 0x002E342C File Offset: 0x002E162C
	private void SelectRow(string id)
	{
		this.selectedStoryId = id;
		this.RefreshRows();
		this.RefreshDescriptionPanel();
	}

	// Token: 0x06007821 RID: 30753 RVA: 0x002E3444 File Offset: 0x002E1644
	public void SelectDefault()
	{
		foreach (KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair in this.storyStates)
		{
			if (keyValuePair.Value == StoryContentPanel.StoryState.Guaranteed)
			{
				this.SelectRow(keyValuePair.Key);
				return;
			}
		}
		using (Dictionary<string, StoryContentPanel.StoryState>.Enumerator enumerator = this.storyStates.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair2 = enumerator.Current;
				this.SelectRow(keyValuePair2.Key);
			}
		}
	}

	// Token: 0x06007822 RID: 30754 RVA: 0x002E34F4 File Offset: 0x002E16F4
	private void IncrementStorySetting(string storyId, bool forward = true)
	{
		int num = (int)this.storyStates[storyId];
		num += (forward ? 1 : -1);
		if (num < 0)
		{
			num += 2;
		}
		num %= 2;
		this.SetStoryState(storyId, (StoryContentPanel.StoryState)num);
		this.mainScreen.RefreshRowsAndDescriptions();
	}

	// Token: 0x06007823 RID: 30755 RVA: 0x002E3538 File Offset: 0x002E1738
	private void SetStoryState(string storyId, StoryContentPanel.StoryState state)
	{
		this.storyStates[storyId] = state;
		SettingConfig config = CustomGameSettings.Instance.StorySettings[storyId];
		CustomGameSettings.Instance.SetStorySetting(config, this.storyStates[storyId] == StoryContentPanel.StoryState.Guaranteed);
	}

	// Token: 0x06007824 RID: 30756 RVA: 0x002E3580 File Offset: 0x002E1780
	public void SelectRandomStories(int min = 5, int max = 5, bool useBias = false)
	{
		int num = UnityEngine.Random.Range(min, max);
		List<Story> list = new List<Story>(Db.Get().Stories.resources);
		List<Story> list2 = new List<Story>();
		list.Shuffle<Story>();
		int num2 = 0;
		while (num2 < num && list.Count - 1 >= num2)
		{
			list2.Add(list[num2]);
			num2++;
		}
		float num3 = 0.7f;
		int num4 = list2.Count((Story x) => x.IsNew());
		if (useBias && num4 == 0 && UnityEngine.Random.value < num3)
		{
			List<Story> list3 = (from x in Db.Get().Stories.resources
			where x.IsNew()
			select x).ToList<Story>();
			list3.Shuffle<Story>();
			if (list3.Count > 0)
			{
				list2.RemoveAt(0);
				list2.Add(list3[0]);
			}
		}
		if (!list2.Contains(Db.Get().Stories.HijackedHeadquarters))
		{
			list2.RemoveAt(0);
			list2.Add(Db.Get().Stories.HijackedHeadquarters);
		}
		foreach (Story story in list)
		{
			this.SetStoryState(story.Id, list2.Contains(story) ? StoryContentPanel.StoryState.Guaranteed : StoryContentPanel.StoryState.Forbidden);
		}
		this.RefreshAllStoryStates();
		this.mainScreen.RefreshRowsAndDescriptions();
	}

	// Token: 0x06007825 RID: 30757 RVA: 0x002E3718 File Offset: 0x002E1918
	private void RefreshAllStoryStates()
	{
		foreach (string id in this.storyRows.Keys)
		{
			this.RefreshStoryDisplay(id);
		}
	}

	// Token: 0x06007826 RID: 30758 RVA: 0x002E3770 File Offset: 0x002E1970
	private void RefreshStoryDisplay(string id)
	{
		MultiToggle reference = this.storyRows[id].GetComponent<HierarchyReferences>().GetReference<MultiToggle>("checkbox");
		StoryContentPanel.StoryState storyState = this.storyStates[id];
		if (storyState == StoryContentPanel.StoryState.Forbidden)
		{
			reference.ChangeState(0);
			return;
		}
		if (storyState != StoryContentPanel.StoryState.Guaranteed)
		{
			return;
		}
		reference.ChangeState(1);
	}

	// Token: 0x06007827 RID: 30759 RVA: 0x002E37C0 File Offset: 0x002E19C0
	private void RefreshRows()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.storyRows)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == this.selectedStoryId) ? 1 : 0);
		}
	}

	// Token: 0x06007828 RID: 30760 RVA: 0x002E3838 File Offset: 0x002E1A38
	private void RefreshDescriptionPanel()
	{
		if (this.selectedStoryId.IsNullOrWhiteSpace())
		{
			this.selectedStoryTitleLabel.SetText("");
			this.selectedStoryDescriptionLabel.SetText("");
			return;
		}
		WorldTrait storyTrait = Db.Get().Stories.GetStoryTrait(this.selectedStoryId, true);
		this.selectedStoryTitleLabel.SetText(Strings.Get(storyTrait.name));
		this.selectedStoryDescriptionLabel.SetText(Strings.Get(storyTrait.description));
		string s = storyTrait.icon.Replace("_icon", "_image");
		this.selectedStoryImage.sprite = Assets.GetSprite(s);
	}

	// Token: 0x06007829 RID: 30761 RVA: 0x002E38EC File Offset: 0x002E1AEC
	public string GetTraitsString(bool tooltip = false)
	{
		int num = 0;
		int num2 = 5;
		foreach (KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair in this.storyStates)
		{
			if (keyValuePair.Value == StoryContentPanel.StoryState.Guaranteed)
			{
				num++;
			}
		}
		string text = UI.FRONTEND.COLONYDESTINATIONSCREEN.STORY_TRAITS_HEADER;
		string str;
		if (num != 0)
		{
			if (num != 1)
			{
				str = GameUtil.SafeStringFormat(UI.FRONTEND.COLONYDESTINATIONSCREEN.TRAIT_COUNT, new object[]
				{
					num
				});
			}
			else
			{
				str = UI.FRONTEND.COLONYDESTINATIONSCREEN.SINGLE_TRAIT;
			}
		}
		else
		{
			str = UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS;
		}
		text = text + ": " + str;
		if (num > num2)
		{
			text = text + " " + UI.FRONTEND.COLONYDESTINATIONSCREEN.TOO_MANY_TRAITS_WARNING;
		}
		if (tooltip)
		{
			if (num > num2)
			{
				text = text + "\n\n" + UI.FRONTEND.COLONYDESTINATIONSCREEN.TOO_MANY_TRAITS_WARNING_TOOLTIP;
			}
			else
			{
				text = text + "\n\n" + UI.FRONTEND.COLONYDESTINATIONSCREEN.TRAIT_COUNT_TOOLTIP;
			}
		}
		return text;
	}

	// Token: 0x040053B0 RID: 21424
	[SerializeField]
	private GameObject storyRowPrefab;

	// Token: 0x040053B1 RID: 21425
	[SerializeField]
	private GameObject storyRowContainer;

	// Token: 0x040053B2 RID: 21426
	private Dictionary<string, GameObject> storyRows = new Dictionary<string, GameObject>();

	// Token: 0x040053B3 RID: 21427
	public const int DEFAULT_RANDOMIZE_STORY_COUNT = 5;

	// Token: 0x040053B4 RID: 21428
	private Dictionary<string, StoryContentPanel.StoryState> storyStates = new Dictionary<string, StoryContentPanel.StoryState>();

	// Token: 0x040053B5 RID: 21429
	private string selectedStoryId = "";

	// Token: 0x040053B6 RID: 21430
	[SerializeField]
	private ColonyDestinationSelectScreen mainScreen;

	// Token: 0x040053B7 RID: 21431
	[Header("Trait Count")]
	[Header("SelectedStory")]
	[SerializeField]
	private Image selectedStoryImage;

	// Token: 0x040053B8 RID: 21432
	[SerializeField]
	private LocText selectedStoryTitleLabel;

	// Token: 0x040053B9 RID: 21433
	[SerializeField]
	private LocText selectedStoryDescriptionLabel;

	// Token: 0x040053BA RID: 21434
	[SerializeField]
	private Sprite spriteForbidden;

	// Token: 0x040053BB RID: 21435
	[SerializeField]
	private Sprite spritePossible;

	// Token: 0x040053BC RID: 21436
	[SerializeField]
	private Sprite spriteGuaranteed;

	// Token: 0x040053BD RID: 21437
	private StoryContentPanel.StoryState _defaultStoryState;

	// Token: 0x040053BE RID: 21438
	private List<string> storyTraitSettings = new List<string>
	{
		"None",
		"Few",
		"Lots"
	};

	// Token: 0x02002113 RID: 8467
	private enum StoryState
	{
		// Token: 0x040097FA RID: 38906
		Forbidden,
		// Token: 0x040097FB RID: 38907
		Guaranteed,
		// Token: 0x040097FC RID: 38908
		LENGTH
	}
}
