using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D6C RID: 3436
public class CrewJobsScreen : CrewListScreen<CrewJobsEntry>
{
	// Token: 0x06006A8B RID: 27275 RVA: 0x002850DC File Offset: 0x002832DC
	protected override void OnActivate()
	{
		CrewJobsScreen.Instance = this;
		foreach (ChoreGroup item in Db.Get().ChoreGroups.resources)
		{
			this.choreGroups.Add(item);
		}
		base.OnActivate();
	}

	// Token: 0x06006A8C RID: 27276 RVA: 0x0028514C File Offset: 0x0028334C
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		base.RefreshCrewPortraitContent();
		this.SortByPreviousSelected();
	}

	// Token: 0x06006A8D RID: 27277 RVA: 0x00285160 File Offset: 0x00283360
	protected override void OnForcedCleanUp()
	{
		CrewJobsScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06006A8E RID: 27278 RVA: 0x00285170 File Offset: 0x00283370
	protected override void SpawnEntries()
	{
		base.SpawnEntries();
		foreach (MinionIdentity identity in Components.LiveMinionIdentities.Items)
		{
			CrewJobsEntry component = Util.KInstantiateUI(this.Prefab_CrewEntry, this.EntriesPanelTransform.gameObject, false).GetComponent<CrewJobsEntry>();
			component.Populate(identity);
			this.EntryObjects.Add(component);
		}
		this.SortEveryoneToggle.group = this.sortToggleGroup;
		ImageToggleState toggleImage = this.SortEveryoneToggle.GetComponentInChildren<ImageToggleState>(true);
		this.SortEveryoneToggle.onValueChanged.AddListener(delegate(bool value)
		{
			this.SortByName(!this.SortEveryoneToggle.isOn);
			this.lastSortToggle = this.SortEveryoneToggle;
			this.lastSortReversed = !this.SortEveryoneToggle.isOn;
			this.ResetSortToggles(this.SortEveryoneToggle);
			if (this.SortEveryoneToggle.isOn)
			{
				toggleImage.SetActive();
				return;
			}
			toggleImage.SetInactive();
		});
		this.SortByPreviousSelected();
		this.dirty = true;
	}

	// Token: 0x06006A8F RID: 27279 RVA: 0x00285250 File Offset: 0x00283450
	private void SortByPreviousSelected()
	{
		if (this.sortToggleGroup == null || this.lastSortToggle == null)
		{
			return;
		}
		int childCount = this.ColumnTitlesContainer.childCount;
		for (int i = 0; i < childCount; i++)
		{
			if (i < this.choreGroups.Count && this.ColumnTitlesContainer.GetChild(i).Find("Title").GetComponentInChildren<Toggle>() == this.lastSortToggle)
			{
				this.SortByEffectiveness(this.choreGroups[i], this.lastSortReversed, false);
				return;
			}
		}
		if (this.SortEveryoneToggle == this.lastSortToggle)
		{
			base.SortByName(this.lastSortReversed);
		}
	}

	// Token: 0x06006A90 RID: 27280 RVA: 0x00285304 File Offset: 0x00283504
	protected override void PositionColumnTitles()
	{
		base.PositionColumnTitles();
		int childCount = this.ColumnTitlesContainer.childCount;
		for (int i = 0; i < childCount; i++)
		{
			if (i < this.choreGroups.Count)
			{
				Toggle sortToggle = this.ColumnTitlesContainer.GetChild(i).Find("Title").GetComponentInChildren<Toggle>();
				this.ColumnTitlesContainer.GetChild(i).rectTransform().localScale = Vector3.one;
				ChoreGroup chore_group = this.choreGroups[i];
				ImageToggleState toggleImage = sortToggle.GetComponentInChildren<ImageToggleState>(true);
				sortToggle.group = this.sortToggleGroup;
				sortToggle.onValueChanged.AddListener(delegate(bool value)
				{
					bool playSound = false;
					if (this.lastSortToggle == sortToggle)
					{
						playSound = true;
					}
					this.SortByEffectiveness(chore_group, !sortToggle.isOn, playSound);
					this.lastSortToggle = sortToggle;
					this.lastSortReversed = !sortToggle.isOn;
					this.ResetSortToggles(sortToggle);
					if (sortToggle.isOn)
					{
						toggleImage.SetActive();
						return;
					}
					toggleImage.SetInactive();
				});
			}
			ToolTip JobTooltip = this.ColumnTitlesContainer.GetChild(i).GetComponent<ToolTip>();
			ToolTip jobTooltip = JobTooltip;
			jobTooltip.OnToolTip = (Func<string>)Delegate.Combine(jobTooltip.OnToolTip, new Func<string>(() => this.GetJobTooltip(JobTooltip.gameObject)));
			Button componentInChildren = this.ColumnTitlesContainer.GetChild(i).GetComponentInChildren<Button>();
			this.EveryoneToggles.Add(componentInChildren, CrewJobsScreen.everyoneToggleState.on);
		}
		for (int j = 0; j < this.choreGroups.Count; j++)
		{
			ChoreGroup chore_group = this.choreGroups[j];
			Button b = this.EveryoneToggles.Keys.ElementAt(j);
			this.EveryoneToggles.Keys.ElementAt(j).onClick.AddListener(delegate()
			{
				this.ToggleJobEveryone(b, chore_group);
			});
		}
		Button key = this.EveryoneToggles.ElementAt(this.EveryoneToggles.Count - 1).Key;
		key.transform.parent.Find("Title").gameObject.GetComponentInChildren<Toggle>().gameObject.SetActive(false);
		key.onClick.AddListener(delegate()
		{
			this.ToggleAllTasksEveryone();
		});
		this.EveryoneAllTaskToggle = new KeyValuePair<Button, CrewJobsScreen.everyoneToggleState>(key, this.EveryoneAllTaskToggle.Value);
	}

	// Token: 0x06006A91 RID: 27281 RVA: 0x00285550 File Offset: 0x00283750
	private string GetJobTooltip(GameObject go)
	{
		ToolTip component = go.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		OverviewColumnIdentity component2 = go.GetComponent<OverviewColumnIdentity>();
		if (component2.columnID != "AllTasks")
		{
			ChoreGroup choreGroup = Db.Get().ChoreGroups.Get(component2.columnID);
			component.AddMultiStringTooltip(component2.Column_DisplayName, this.TextStyle_JobTooltip_Title);
			component.AddMultiStringTooltip(choreGroup.description, this.TextStyle_JobTooltip_Description);
			component.AddMultiStringTooltip("\n", this.TextStyle_JobTooltip_Description);
			component.AddMultiStringTooltip(UI.TOOLTIPS.JOBSSCREEN_ATTRIBUTES, this.TextStyle_JobTooltip_Description);
			component.AddMultiStringTooltip("•  " + choreGroup.attribute.Name, this.TextStyle_JobTooltip_RelevantAttributes);
		}
		return "";
	}

	// Token: 0x06006A92 RID: 27282 RVA: 0x00285610 File Offset: 0x00283810
	private void ToggleAllTasksEveryone()
	{
		string name = "HUD_Click_Deselect";
		if (this.EveryoneAllTaskToggle.Value != CrewJobsScreen.everyoneToggleState.on)
		{
			name = "HUD_Click";
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(name, false));
		for (int i = 0; i < this.choreGroups.Count; i++)
		{
			this.SetJobEveryone(this.EveryoneAllTaskToggle.Value != CrewJobsScreen.everyoneToggleState.on, this.choreGroups[i]);
		}
	}

	// Token: 0x06006A93 RID: 27283 RVA: 0x0028567C File Offset: 0x0028387C
	private void SetJobEveryone(Button button, ChoreGroup chore_group)
	{
		this.SetJobEveryone(this.EveryoneToggles[button] != CrewJobsScreen.everyoneToggleState.on, chore_group);
	}

	// Token: 0x06006A94 RID: 27284 RVA: 0x00285698 File Offset: 0x00283898
	private void SetJobEveryone(bool state, ChoreGroup chore_group)
	{
		foreach (CrewJobsEntry crewJobsEntry in this.EntryObjects)
		{
			crewJobsEntry.consumer.SetPermittedByUser(chore_group, state);
		}
	}

	// Token: 0x06006A95 RID: 27285 RVA: 0x002856F0 File Offset: 0x002838F0
	private void ToggleJobEveryone(Button button, ChoreGroup chore_group)
	{
		string name = "HUD_Click_Deselect";
		if (this.EveryoneToggles[button] != CrewJobsScreen.everyoneToggleState.on)
		{
			name = "HUD_Click";
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(name, false));
		foreach (CrewJobsEntry crewJobsEntry in this.EntryObjects)
		{
			crewJobsEntry.consumer.SetPermittedByUser(chore_group, this.EveryoneToggles[button] != CrewJobsScreen.everyoneToggleState.on);
		}
	}

	// Token: 0x06006A96 RID: 27286 RVA: 0x00285780 File Offset: 0x00283980
	private void SortByEffectiveness(ChoreGroup chore_group, bool reverse, bool playSound)
	{
		if (playSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
		}
		List<CrewJobsEntry> list = new List<CrewJobsEntry>(this.EntryObjects);
		list.Sort(delegate(CrewJobsEntry a, CrewJobsEntry b)
		{
			float value = a.Identity.GetAttributes().GetValue(chore_group.attribute.Id);
			float value2 = b.Identity.GetAttributes().GetValue(chore_group.attribute.Id);
			return value.CompareTo(value2);
		});
		base.ReorderEntries(list, reverse);
	}

	// Token: 0x06006A97 RID: 27287 RVA: 0x002857D4 File Offset: 0x002839D4
	private void ResetSortToggles(Toggle exceptToggle)
	{
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			Toggle componentInChildren = this.ColumnTitlesContainer.GetChild(i).Find("Title").GetComponentInChildren<Toggle>();
			if (!(componentInChildren == null))
			{
				ImageToggleState componentInChildren2 = componentInChildren.GetComponentInChildren<ImageToggleState>(true);
				if (componentInChildren != exceptToggle)
				{
					componentInChildren2.SetDisabled();
				}
			}
		}
		ImageToggleState componentInChildren3 = this.SortEveryoneToggle.GetComponentInChildren<ImageToggleState>(true);
		if (this.SortEveryoneToggle != exceptToggle)
		{
			componentInChildren3.SetDisabled();
		}
	}

	// Token: 0x06006A98 RID: 27288 RVA: 0x00285854 File Offset: 0x00283A54
	private void Refresh()
	{
		if (this.dirty)
		{
			int childCount = this.ColumnTitlesContainer.childCount;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < childCount; i++)
			{
				bool flag3 = false;
				bool flag4 = false;
				if (this.choreGroups.Count - 1 >= i)
				{
					ChoreGroup chore_group = this.choreGroups[i];
					for (int j = 0; j < this.EntryObjects.Count; j++)
					{
						ChoreConsumer consumer = this.EntryObjects[j].GetComponent<CrewJobsEntry>().consumer;
						if (consumer.IsPermittedByTraits(chore_group))
						{
							if (consumer.IsPermittedByUser(chore_group))
							{
								flag3 = true;
								flag = true;
							}
							else
							{
								flag4 = true;
								flag2 = true;
							}
						}
					}
					if (flag3 && flag4)
					{
						this.EveryoneToggles[this.EveryoneToggles.ElementAt(i).Key] = CrewJobsScreen.everyoneToggleState.mixed;
					}
					else if (flag3)
					{
						this.EveryoneToggles[this.EveryoneToggles.ElementAt(i).Key] = CrewJobsScreen.everyoneToggleState.on;
					}
					else
					{
						this.EveryoneToggles[this.EveryoneToggles.ElementAt(i).Key] = CrewJobsScreen.everyoneToggleState.off;
					}
					Button componentInChildren = this.ColumnTitlesContainer.GetChild(i).GetComponentInChildren<Button>();
					ImageToggleState component = componentInChildren.GetComponentsInChildren<Image>(true)[1].GetComponent<ImageToggleState>();
					switch (this.EveryoneToggles[componentInChildren])
					{
					case CrewJobsScreen.everyoneToggleState.off:
						component.SetDisabled();
						break;
					case CrewJobsScreen.everyoneToggleState.mixed:
						component.SetInactive();
						break;
					case CrewJobsScreen.everyoneToggleState.on:
						component.SetActive();
						break;
					}
				}
			}
			if (flag && flag2)
			{
				this.EveryoneAllTaskToggle = new KeyValuePair<Button, CrewJobsScreen.everyoneToggleState>(this.EveryoneAllTaskToggle.Key, CrewJobsScreen.everyoneToggleState.mixed);
			}
			else if (flag)
			{
				this.EveryoneAllTaskToggle = new KeyValuePair<Button, CrewJobsScreen.everyoneToggleState>(this.EveryoneAllTaskToggle.Key, CrewJobsScreen.everyoneToggleState.on);
			}
			else if (flag2)
			{
				this.EveryoneAllTaskToggle = new KeyValuePair<Button, CrewJobsScreen.everyoneToggleState>(this.EveryoneAllTaskToggle.Key, CrewJobsScreen.everyoneToggleState.off);
			}
			ImageToggleState component2 = this.EveryoneAllTaskToggle.Key.GetComponentsInChildren<Image>(true)[1].GetComponent<ImageToggleState>();
			switch (this.EveryoneAllTaskToggle.Value)
			{
			case CrewJobsScreen.everyoneToggleState.off:
				component2.SetDisabled();
				break;
			case CrewJobsScreen.everyoneToggleState.mixed:
				component2.SetInactive();
				break;
			case CrewJobsScreen.everyoneToggleState.on:
				component2.SetActive();
				break;
			}
			this.screenWidth = this.EntriesPanelTransform.rectTransform().sizeDelta.x;
			this.ScrollRectTransform.GetComponent<LayoutElement>().minWidth = this.screenWidth;
			float num = 31f;
			base.GetComponent<LayoutElement>().minWidth = this.screenWidth + num;
			this.dirty = false;
		}
	}

	// Token: 0x06006A99 RID: 27289 RVA: 0x00285ADF File Offset: 0x00283CDF
	private void Update()
	{
		this.Refresh();
	}

	// Token: 0x06006A9A RID: 27290 RVA: 0x00285AE7 File Offset: 0x00283CE7
	public void Dirty(object data = null)
	{
		this.dirty = true;
	}

	// Token: 0x0400494B RID: 18763
	public static CrewJobsScreen Instance;

	// Token: 0x0400494C RID: 18764
	private Dictionary<Button, CrewJobsScreen.everyoneToggleState> EveryoneToggles = new Dictionary<Button, CrewJobsScreen.everyoneToggleState>();

	// Token: 0x0400494D RID: 18765
	private KeyValuePair<Button, CrewJobsScreen.everyoneToggleState> EveryoneAllTaskToggle;

	// Token: 0x0400494E RID: 18766
	public TextStyleSetting TextStyle_JobTooltip_Title;

	// Token: 0x0400494F RID: 18767
	public TextStyleSetting TextStyle_JobTooltip_Description;

	// Token: 0x04004950 RID: 18768
	public TextStyleSetting TextStyle_JobTooltip_RelevantAttributes;

	// Token: 0x04004951 RID: 18769
	public Toggle SortEveryoneToggle;

	// Token: 0x04004952 RID: 18770
	private List<ChoreGroup> choreGroups = new List<ChoreGroup>();

	// Token: 0x04004953 RID: 18771
	private bool dirty;

	// Token: 0x04004954 RID: 18772
	private float screenWidth;

	// Token: 0x02001FA3 RID: 8099
	public enum everyoneToggleState
	{
		// Token: 0x04009379 RID: 37753
		off,
		// Token: 0x0400937A RID: 37754
		mixed,
		// Token: 0x0400937B RID: 37755
		on
	}
}
