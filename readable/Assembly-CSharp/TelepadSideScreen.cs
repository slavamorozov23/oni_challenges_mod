using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E85 RID: 3717
public class TelepadSideScreen : SideScreenContent
{
	// Token: 0x06007652 RID: 30290 RVA: 0x002D22D0 File Offset: 0x002D04D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.viewImmigrantsBtn.onClick += delegate()
		{
			ImmigrantScreen.InitializeImmigrantScreen(this.targetTelepad);
			Game.Instance.Trigger(288942073, null);
		};
		this.viewColonySummaryBtn.onClick += delegate()
		{
			this.newAchievementsEarned.gameObject.SetActive(false);
			MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
		};
		this.openRolesScreenButton.onClick += delegate()
		{
			ManagementMenu.Instance.ToggleSkills();
		};
		this.BuildVictoryConditions();
	}

	// Token: 0x06007653 RID: 30291 RVA: 0x002D2341 File Offset: 0x002D0541
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Telepad>() != null;
	}

	// Token: 0x06007654 RID: 30292 RVA: 0x002D2350 File Offset: 0x002D0550
	public override void SetTarget(GameObject target)
	{
		Telepad component = target.GetComponent<Telepad>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a telepad associated with it.");
			return;
		}
		this.targetTelepad = component;
		if (this.targetTelepad != null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x06007655 RID: 30293 RVA: 0x002D23A8 File Offset: 0x002D05A8
	private void Update()
	{
		if (this.targetTelepad != null)
		{
			if (GameFlowManager.Instance != null && GameFlowManager.Instance.IsGameOver())
			{
				base.gameObject.SetActive(false);
				this.timeLabel.text = UI.UISIDESCREENS.TELEPADSIDESCREEN.GAMEOVER;
				this.SetContentState(true);
			}
			else
			{
				if (this.targetTelepad.GetComponent<Operational>().IsOperational)
				{
					this.timeLabel.text = string.Format(UI.UISIDESCREENS.TELEPADSIDESCREEN.NEXTPRODUCTION, GameUtil.GetFormattedCycles(this.targetTelepad.GetTimeRemaining(), "F1", false));
				}
				else
				{
					base.gameObject.SetActive(false);
				}
				this.SetContentState(!Immigration.Instance.ImmigrantsAvailable);
			}
			this.UpdateVictoryConditions();
			this.UpdateAchievementsUnlocked();
			this.UpdateSkills();
		}
	}

	// Token: 0x06007656 RID: 30294 RVA: 0x002D2480 File Offset: 0x002D0680
	private void SetContentState(bool isLabel)
	{
		if (this.timeLabel.gameObject.activeInHierarchy != isLabel)
		{
			this.timeLabel.gameObject.SetActive(isLabel);
		}
		if (this.viewImmigrantsBtn.gameObject.activeInHierarchy == isLabel)
		{
			this.viewImmigrantsBtn.gameObject.SetActive(!isLabel);
		}
	}

	// Token: 0x06007657 RID: 30295 RVA: 0x002D24D8 File Offset: 0x002D06D8
	private void BuildVictoryConditions()
	{
		foreach (ColonyAchievement colonyAchievement in Db.Get().ColonyAchievements.resources)
		{
			if (colonyAchievement.isVictoryCondition && !colonyAchievement.Disabled && colonyAchievement.IsValidForSave())
			{
				Dictionary<ColonyAchievementRequirement, GameObject> dictionary = new Dictionary<ColonyAchievementRequirement, GameObject>();
				this.victoryAchievementWidgets.Add(colonyAchievement, dictionary);
				GameObject gameObject = Util.KInstantiateUI(this.conditionContainerTemplate, this.victoryConditionsContainer, true);
				gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(colonyAchievement.Name);
				foreach (ColonyAchievementRequirement colonyAchievementRequirement in colonyAchievement.requirementChecklist)
				{
					VictoryColonyAchievementRequirement victoryColonyAchievementRequirement = colonyAchievementRequirement as VictoryColonyAchievementRequirement;
					if (victoryColonyAchievementRequirement != null)
					{
						GameObject gameObject2 = Util.KInstantiateUI(this.checkboxLinePrefab, gameObject, true);
						gameObject2.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(victoryColonyAchievementRequirement.Name());
						gameObject2.GetComponent<ToolTip>().SetSimpleTooltip(victoryColonyAchievementRequirement.Description());
						dictionary.Add(colonyAchievementRequirement, gameObject2);
					}
					else
					{
						global::Debug.LogWarning(string.Format("Colony achievement {0} is not a victory requirement but it is attached to a victory achievement {1}.", colonyAchievementRequirement.GetType().ToString(), colonyAchievement.Name));
					}
				}
				this.entries.Add(colonyAchievement.Id, dictionary);
			}
		}
	}

	// Token: 0x06007658 RID: 30296 RVA: 0x002D267C File Offset: 0x002D087C
	private void UpdateVictoryConditions()
	{
		foreach (ColonyAchievement colonyAchievement in Db.Get().ColonyAchievements.resources)
		{
			if (colonyAchievement.isVictoryCondition && !colonyAchievement.Disabled && colonyAchievement.IsValidForSave())
			{
				foreach (ColonyAchievementRequirement colonyAchievementRequirement in colonyAchievement.requirementChecklist)
				{
					this.entries[colonyAchievement.Id][colonyAchievementRequirement].GetComponent<HierarchyReferences>().GetReference<Image>("Check").enabled = colonyAchievementRequirement.Success();
				}
			}
		}
		foreach (KeyValuePair<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>> keyValuePair in this.victoryAchievementWidgets)
		{
			foreach (KeyValuePair<ColonyAchievementRequirement, GameObject> keyValuePair2 in keyValuePair.Value)
			{
				keyValuePair2.Value.GetComponent<ToolTip>().SetSimpleTooltip(keyValuePair2.Key.GetProgress(keyValuePair2.Key.Success()));
			}
		}
	}

	// Token: 0x06007659 RID: 30297 RVA: 0x002D2800 File Offset: 0x002D0A00
	private void UpdateAchievementsUnlocked()
	{
		if (SaveGame.Instance.ColonyAchievementTracker.achievementsToDisplay.Count > 0)
		{
			this.newAchievementsEarned.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600765A RID: 30298 RVA: 0x002D282C File Offset: 0x002D0A2C
	private void UpdateSkills()
	{
		bool active = false;
		foreach (object obj in Components.MinionResumes)
		{
			MinionResume minionResume = (MinionResume)obj;
			if (!minionResume.HasTag(GameTags.Dead) && minionResume.TotalSkillPointsGained - minionResume.SkillsMastered > 0)
			{
				active = true;
				break;
			}
		}
		this.skillPointsAvailable.gameObject.SetActive(active);
	}

	// Token: 0x040051DD RID: 20957
	[SerializeField]
	private LocText timeLabel;

	// Token: 0x040051DE RID: 20958
	[SerializeField]
	private KButton viewImmigrantsBtn;

	// Token: 0x040051DF RID: 20959
	[SerializeField]
	private Telepad targetTelepad;

	// Token: 0x040051E0 RID: 20960
	[SerializeField]
	private KButton viewColonySummaryBtn;

	// Token: 0x040051E1 RID: 20961
	[SerializeField]
	private Image newAchievementsEarned;

	// Token: 0x040051E2 RID: 20962
	[SerializeField]
	private KButton openRolesScreenButton;

	// Token: 0x040051E3 RID: 20963
	[SerializeField]
	private Image skillPointsAvailable;

	// Token: 0x040051E4 RID: 20964
	[SerializeField]
	private GameObject victoryConditionsContainer;

	// Token: 0x040051E5 RID: 20965
	[SerializeField]
	private GameObject conditionContainerTemplate;

	// Token: 0x040051E6 RID: 20966
	[SerializeField]
	private GameObject checkboxLinePrefab;

	// Token: 0x040051E7 RID: 20967
	private Dictionary<string, Dictionary<ColonyAchievementRequirement, GameObject>> entries = new Dictionary<string, Dictionary<ColonyAchievementRequirement, GameObject>>();

	// Token: 0x040051E8 RID: 20968
	private Dictionary<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>> victoryAchievementWidgets = new Dictionary<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>>();
}
