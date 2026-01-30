using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D6B RID: 3435
public class CrewJobsEntry : CrewListEntry
{
	// Token: 0x1700079D RID: 1949
	// (get) Token: 0x06006A7D RID: 27261 RVA: 0x0028483A File Offset: 0x00282A3A
	// (set) Token: 0x06006A7E RID: 27262 RVA: 0x00284842 File Offset: 0x00282A42
	public ChoreConsumer consumer { get; private set; }

	// Token: 0x06006A7F RID: 27263 RVA: 0x0028484C File Offset: 0x00282A4C
	public override void Populate(MinionIdentity _identity)
	{
		base.Populate(_identity);
		this.consumer = _identity.GetComponent<ChoreConsumer>();
		ChoreConsumer consumer = this.consumer;
		consumer.choreRulesChanged = (System.Action)Delegate.Combine(consumer.choreRulesChanged, new System.Action(this.Dirty));
		foreach (ChoreGroup chore_group in Db.Get().ChoreGroups.resources)
		{
			this.CreateChoreButton(chore_group);
		}
		this.CreateAllTaskButton();
		this.dirty = true;
	}

	// Token: 0x06006A80 RID: 27264 RVA: 0x002848F0 File Offset: 0x00282AF0
	private void CreateChoreButton(ChoreGroup chore_group)
	{
		GameObject gameObject = Util.KInstantiateUI(this.Prefab_JobPriorityButton, base.transform.gameObject, false);
		gameObject.GetComponent<OverviewColumnIdentity>().columnID = chore_group.Id;
		gameObject.GetComponent<OverviewColumnIdentity>().Column_DisplayName = chore_group.Name;
		CrewJobsEntry.PriorityButton priorityButton = default(CrewJobsEntry.PriorityButton);
		priorityButton.button = gameObject.GetComponent<Button>();
		priorityButton.border = gameObject.transform.GetChild(1).GetComponent<Image>();
		priorityButton.baseBorderColor = priorityButton.border.color;
		priorityButton.background = gameObject.transform.GetChild(0).GetComponent<Image>();
		priorityButton.baseBackgroundColor = priorityButton.background.color;
		priorityButton.choreGroup = chore_group;
		priorityButton.ToggleIcon = gameObject.transform.GetChild(2).gameObject;
		priorityButton.tooltip = gameObject.GetComponent<ToolTip>();
		priorityButton.tooltip.OnToolTip = (() => this.OnPriorityButtonTooltip(priorityButton));
		priorityButton.button.onClick.AddListener(delegate()
		{
			this.OnPriorityPress(chore_group);
		});
		this.PriorityButtons.Add(priorityButton);
	}

	// Token: 0x06006A81 RID: 27265 RVA: 0x00284A6C File Offset: 0x00282C6C
	private void CreateAllTaskButton()
	{
		GameObject gameObject = Util.KInstantiateUI(this.Prefab_JobPriorityButtonAllTasks, base.transform.gameObject, false);
		gameObject.GetComponent<OverviewColumnIdentity>().columnID = "AllTasks";
		gameObject.GetComponent<OverviewColumnIdentity>().Column_DisplayName = "";
		Button b = gameObject.GetComponent<Button>();
		b.onClick.AddListener(delegate()
		{
			this.ToggleTasksAll(b);
		});
		CrewJobsEntry.PriorityButton priorityButton = default(CrewJobsEntry.PriorityButton);
		priorityButton.button = gameObject.GetComponent<Button>();
		priorityButton.border = gameObject.transform.GetChild(1).GetComponent<Image>();
		priorityButton.baseBorderColor = priorityButton.border.color;
		priorityButton.background = gameObject.transform.GetChild(0).GetComponent<Image>();
		priorityButton.baseBackgroundColor = priorityButton.background.color;
		priorityButton.ToggleIcon = gameObject.transform.GetChild(2).gameObject;
		priorityButton.tooltip = gameObject.GetComponent<ToolTip>();
		this.AllTasksButton = priorityButton;
	}

	// Token: 0x06006A82 RID: 27266 RVA: 0x00284B7C File Offset: 0x00282D7C
	private void ToggleTasksAll(Button button)
	{
		bool flag = this.rowToggleState != CrewJobsScreen.everyoneToggleState.on;
		string name = "HUD_Click_Deselect";
		if (flag)
		{
			name = "HUD_Click";
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(name, false));
		foreach (ChoreGroup chore_group in Db.Get().ChoreGroups.resources)
		{
			this.consumer.SetPermittedByUser(chore_group, flag);
		}
	}

	// Token: 0x06006A83 RID: 27267 RVA: 0x00284C08 File Offset: 0x00282E08
	private void OnPriorityPress(ChoreGroup chore_group)
	{
		bool flag = this.consumer.IsPermittedByUser(chore_group);
		string name = "HUD_Click";
		if (flag)
		{
			name = "HUD_Click_Deselect";
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(name, false));
		this.consumer.SetPermittedByUser(chore_group, !this.consumer.IsPermittedByUser(chore_group));
	}

	// Token: 0x06006A84 RID: 27268 RVA: 0x00284C5C File Offset: 0x00282E5C
	private void Refresh(object data = null)
	{
		if (this.identity == null)
		{
			this.dirty = false;
			return;
		}
		if (this.dirty)
		{
			Attributes attributes = this.identity.GetAttributes();
			foreach (CrewJobsEntry.PriorityButton priorityButton in this.PriorityButtons)
			{
				bool flag = this.consumer.IsPermittedByUser(priorityButton.choreGroup);
				if (priorityButton.ToggleIcon.activeSelf != flag)
				{
					priorityButton.ToggleIcon.SetActive(flag);
				}
				float t = Mathf.Min(attributes.Get(priorityButton.choreGroup.attribute).GetTotalValue() / 10f, 1f);
				Color baseBorderColor = priorityButton.baseBorderColor;
				baseBorderColor.r = Mathf.Lerp(priorityButton.baseBorderColor.r, 0.72156864f, t);
				baseBorderColor.g = Mathf.Lerp(priorityButton.baseBorderColor.g, 0.44313726f, t);
				baseBorderColor.b = Mathf.Lerp(priorityButton.baseBorderColor.b, 0.5803922f, t);
				if (priorityButton.border.color != baseBorderColor)
				{
					priorityButton.border.color = baseBorderColor;
				}
				Color color = priorityButton.baseBackgroundColor;
				color.a = Mathf.Lerp(0f, 1f, t);
				bool flag2 = this.consumer.IsPermittedByTraits(priorityButton.choreGroup);
				if (!flag2)
				{
					color = Color.clear;
					priorityButton.border.color = Color.clear;
					priorityButton.ToggleIcon.SetActive(false);
				}
				priorityButton.button.interactable = flag2;
				if (priorityButton.background.color != color)
				{
					priorityButton.background.color = color;
				}
			}
			int num = 0;
			int num2 = 0;
			foreach (ChoreGroup chore_group in Db.Get().ChoreGroups.resources)
			{
				if (this.consumer.IsPermittedByTraits(chore_group))
				{
					num2++;
					if (this.consumer.IsPermittedByUser(chore_group))
					{
						num++;
					}
				}
			}
			if (num == 0)
			{
				this.rowToggleState = CrewJobsScreen.everyoneToggleState.off;
			}
			else if (num < num2)
			{
				this.rowToggleState = CrewJobsScreen.everyoneToggleState.mixed;
			}
			else
			{
				this.rowToggleState = CrewJobsScreen.everyoneToggleState.on;
			}
			ImageToggleState component = this.AllTasksButton.ToggleIcon.GetComponent<ImageToggleState>();
			switch (this.rowToggleState)
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
			this.dirty = false;
		}
	}

	// Token: 0x06006A85 RID: 27269 RVA: 0x00284F4C File Offset: 0x0028314C
	private string OnPriorityButtonTooltip(CrewJobsEntry.PriorityButton b)
	{
		b.tooltip.ClearMultiStringTooltip();
		if (this.identity != null)
		{
			Attributes attributes = this.identity.GetAttributes();
			if (attributes != null)
			{
				if (!this.consumer.IsPermittedByTraits(b.choreGroup))
				{
					string newString = string.Format(UI.TOOLTIPS.JOBSSCREEN_CANNOTPERFORMTASK, this.consumer.GetComponent<MinionIdentity>().GetProperName());
					b.tooltip.AddMultiStringTooltip(newString, this.TooltipTextStyle_AbilityNegativeModifier);
					return "";
				}
				b.tooltip.AddMultiStringTooltip(UI.TOOLTIPS.JOBSSCREEN_RELEVANT_ATTRIBUTES, this.TooltipTextStyle_Ability);
				Klei.AI.Attribute attribute = b.choreGroup.attribute;
				AttributeInstance attributeInstance = attributes.Get(attribute);
				float totalValue = attributeInstance.GetTotalValue();
				TextStyleSetting styleSetting = this.TooltipTextStyle_Ability;
				if (totalValue > 0f)
				{
					styleSetting = this.TooltipTextStyle_AbilityPositiveModifier;
				}
				else if (totalValue < 0f)
				{
					styleSetting = this.TooltipTextStyle_AbilityNegativeModifier;
				}
				b.tooltip.AddMultiStringTooltip(attribute.Name + " " + attributeInstance.GetTotalValue().ToString(), styleSetting);
			}
		}
		return "";
	}

	// Token: 0x06006A86 RID: 27270 RVA: 0x00285065 File Offset: 0x00283265
	private void LateUpdate()
	{
		this.Refresh(null);
	}

	// Token: 0x06006A87 RID: 27271 RVA: 0x0028506E File Offset: 0x0028326E
	private void OnLevelUp(object data)
	{
		this.Dirty();
	}

	// Token: 0x06006A88 RID: 27272 RVA: 0x00285076 File Offset: 0x00283276
	private void Dirty()
	{
		this.dirty = true;
		CrewJobsScreen.Instance.Dirty(null);
	}

	// Token: 0x06006A89 RID: 27273 RVA: 0x0028508A File Offset: 0x0028328A
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.consumer != null)
		{
			ChoreConsumer consumer = this.consumer;
			consumer.choreRulesChanged = (System.Action)Delegate.Remove(consumer.choreRulesChanged, new System.Action(this.Dirty));
		}
	}

	// Token: 0x04004940 RID: 18752
	public GameObject Prefab_JobPriorityButton;

	// Token: 0x04004941 RID: 18753
	public GameObject Prefab_JobPriorityButtonAllTasks;

	// Token: 0x04004942 RID: 18754
	private List<CrewJobsEntry.PriorityButton> PriorityButtons = new List<CrewJobsEntry.PriorityButton>();

	// Token: 0x04004943 RID: 18755
	private CrewJobsEntry.PriorityButton AllTasksButton;

	// Token: 0x04004944 RID: 18756
	public TextStyleSetting TooltipTextStyle_Title;

	// Token: 0x04004945 RID: 18757
	public TextStyleSetting TooltipTextStyle_Ability;

	// Token: 0x04004946 RID: 18758
	public TextStyleSetting TooltipTextStyle_AbilityPositiveModifier;

	// Token: 0x04004947 RID: 18759
	public TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;

	// Token: 0x04004948 RID: 18760
	private bool dirty;

	// Token: 0x0400494A RID: 18762
	private CrewJobsScreen.everyoneToggleState rowToggleState;

	// Token: 0x02001FA0 RID: 8096
	[Serializable]
	public struct PriorityButton
	{
		// Token: 0x0400936B RID: 37739
		public Button button;

		// Token: 0x0400936C RID: 37740
		public GameObject ToggleIcon;

		// Token: 0x0400936D RID: 37741
		public ChoreGroup choreGroup;

		// Token: 0x0400936E RID: 37742
		public ToolTip tooltip;

		// Token: 0x0400936F RID: 37743
		public Image border;

		// Token: 0x04009370 RID: 37744
		public Image background;

		// Token: 0x04009371 RID: 37745
		public Color baseBorderColor;

		// Token: 0x04009372 RID: 37746
		public Color baseBackgroundColor;
	}
}
