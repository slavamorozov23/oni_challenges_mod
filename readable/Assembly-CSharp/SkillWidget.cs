using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02000E95 RID: 3733
[AddComponentMenu("KMonoBehaviour/scripts/SkillWidget")]
public class SkillWidget : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
	// Token: 0x1700083B RID: 2107
	// (get) Token: 0x06007747 RID: 30535 RVA: 0x002D9A13 File Offset: 0x002D7C13
	// (set) Token: 0x06007748 RID: 30536 RVA: 0x002D9A1B File Offset: 0x002D7C1B
	public string skillID { get; private set; }

	// Token: 0x06007749 RID: 30537 RVA: 0x002D9A24 File Offset: 0x002D7C24
	public void Refresh(string skillID)
	{
		Skill skill = Db.Get().Skills.Get(skillID);
		if (skill == null)
		{
			global::Debug.LogWarning("DbSkills is missing skillId " + skillID);
			return;
		}
		this.Name.text = skill.Name;
		SkillGroup skillGroup = Db.Get().SkillGroups.Get(skill.skillGroup);
		if (!string.IsNullOrEmpty(skillGroup.choreGroupID))
		{
			LocText name = this.Name;
			name.text = name.text + "\n(" + skillGroup.Name + ")";
		}
		this.skillID = skillID;
		this.tooltip.SetSimpleTooltip(this.SkillTooltip(skill));
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.skillsScreen.CurrentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		MinionResume minionResume = null;
		if (minionIdentity != null)
		{
			minionResume = minionIdentity.GetComponent<MinionResume>();
			MinionResume.SkillMasteryConditions[] skillMasteryConditions = minionResume.GetSkillMasteryConditions(skillID);
			bool flag = minionResume.CanMasterSkill(skillMasteryConditions);
			if (!(minionResume == null) && (minionResume.HasMasteredSkill(skillID) || flag))
			{
				this.TitleBarBG.color = (minionResume.HasMasteredSkill(skillID) ? this.header_color_has_skill : this.header_color_can_assign);
				this.hatImage.material = this.defaultMaterial;
			}
			else
			{
				this.TitleBarBG.color = this.header_color_disabled;
				this.hatImage.material = this.desaturatedMaterial;
			}
		}
		else if (storedMinionIdentity != null)
		{
			if (storedMinionIdentity.HasMasteredSkill(skillID))
			{
				this.TitleBarBG.color = this.header_color_has_skill;
				this.hatImage.material = this.defaultMaterial;
			}
			else
			{
				this.TitleBarBG.color = this.header_color_disabled;
				this.hatImage.material = this.desaturatedMaterial;
			}
		}
		this.hatImage.sprite = Assets.GetSprite(skill.badge);
		bool active = false;
		bool flag2 = false;
		if (minionResume != null)
		{
			flag2 = minionResume.HasBeenGrantedSkill(skill);
			float num;
			minionResume.AptitudeBySkillGroup.TryGetValue(skill.skillGroup, out num);
			active = (num > 0f && !flag2);
		}
		this.aptitudeBox.SetActive(active);
		this.grantedBox.SetActive(flag2);
		if (flag2)
		{
			Sprite skillGrantSourceIcon = minionResume.GetSkillGrantSourceIcon(skill.Id);
			if (skillGrantSourceIcon != null)
			{
				this.grantedIcon.sprite = skillGrantSourceIcon;
			}
		}
		this.traitDisabledIcon.SetActive(minionResume != null && !minionResume.IsAbleToLearnSkill(skill.Id));
		string text = "";
		List<string> list = new List<string>();
		foreach (MinionIdentity minionIdentity2 in Components.LiveMinionIdentities.Items)
		{
			MinionResume component = minionIdentity2.GetComponent<MinionResume>();
			if (component != null && component.HasMasteredSkill(skillID))
			{
				list.Add(component.GetProperName());
			}
		}
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			foreach (MinionStorage.Info info in minionStorage.GetStoredMinionInfo())
			{
				if (info.serializedMinion != null)
				{
					StoredMinionIdentity storedMinionIdentity2 = info.serializedMinion.Get<StoredMinionIdentity>();
					if (storedMinionIdentity2 != null && storedMinionIdentity2.HasMasteredSkill(skillID))
					{
						list.Add(storedMinionIdentity2.GetProperName());
					}
				}
			}
		}
		this.masteryCount.gameObject.SetActive(list.Count > 0);
		foreach (string str in list)
		{
			text = text + "\n    • " + str;
		}
		this.masteryCount.SetSimpleTooltip((list.Count > 0) ? string.Format(UI.ROLES_SCREEN.WIDGET.NUMBER_OF_MASTERS_TOOLTIP, text) : UI.ROLES_SCREEN.WIDGET.NO_MASTERS_TOOLTIP.text);
		this.masteryCount.GetComponentInChildren<LocText>().text = list.Count.ToString();
	}

	// Token: 0x0600774A RID: 30538 RVA: 0x002D9E94 File Offset: 0x002D8094
	public void RefreshLines()
	{
		this.prerequisiteSkillWidgets.Clear();
		List<Vector2> list = new List<Vector2>();
		foreach (string text in Db.Get().Skills.Get(this.skillID).priorSkills)
		{
			list.Add(this.skillsScreen.GetSkillWidgetLineTargetPosition(text));
			this.prerequisiteSkillWidgets.Add(this.skillsScreen.GetSkillWidget(text));
		}
		if (this.lines != null)
		{
			for (int i = this.lines.Length - 1; i >= 0; i--)
			{
				UnityEngine.Object.Destroy(this.lines[i].gameObject);
			}
		}
		this.linePoints.Clear();
		for (int j = 0; j < list.Count; j++)
		{
			float num = this.lines_left.GetPosition().x - list[j].x - 12f;
			float y = 0f;
			this.linePoints.Add(new Vector2(0f, y));
			this.linePoints.Add(new Vector2(-num, y));
			this.linePoints.Add(new Vector2(-num, y));
			this.linePoints.Add(new Vector2(-num, -(this.lines_left.GetPosition().y - list[j].y)));
			this.linePoints.Add(new Vector2(-num, -(this.lines_left.GetPosition().y - list[j].y)));
			this.linePoints.Add(new Vector2(-(this.lines_left.GetPosition().x - list[j].x), -(this.lines_left.GetPosition().y - list[j].y)));
		}
		this.lines = new UILineRenderer[this.linePoints.Count / 2];
		int num2 = 0;
		for (int k = 0; k < this.linePoints.Count; k += 2)
		{
			GameObject gameObject = new GameObject("Line");
			gameObject.AddComponent<RectTransform>();
			gameObject.transform.SetParent(this.lines_left.transform);
			gameObject.transform.SetLocalPosition(Vector3.zero);
			gameObject.rectTransform().sizeDelta = Vector2.zero;
			this.lines[num2] = gameObject.AddComponent<UILineRenderer>();
			this.lines[num2].color = new Color(0.6509804f, 0.6509804f, 0.6509804f, 1f);
			this.lines[num2].Points = new Vector2[]
			{
				this.linePoints[k],
				this.linePoints[k + 1]
			};
			num2++;
		}
	}

	// Token: 0x0600774B RID: 30539 RVA: 0x002DA1A8 File Offset: 0x002D83A8
	public void ToggleBorderHighlight(bool on)
	{
		this.borderHighlight.SetActive(on);
		if (this.lines != null)
		{
			foreach (UILineRenderer uilineRenderer in this.lines)
			{
				uilineRenderer.color = (on ? this.line_color_active : this.line_color_default);
				uilineRenderer.LineThickness = (float)(on ? 4 : 2);
				uilineRenderer.SetAllDirty();
			}
		}
		for (int j = 0; j < this.prerequisiteSkillWidgets.Count; j++)
		{
			this.prerequisiteSkillWidgets[j].ToggleBorderHighlight(on);
		}
	}

	// Token: 0x0600774C RID: 30540 RVA: 0x002DA233 File Offset: 0x002D8433
	public string SkillTooltip(Skill skill)
	{
		return "" + SkillWidget.SkillPerksString(skill) + "\n" + this.DuplicantSkillString(skill);
	}

	// Token: 0x0600774D RID: 30541 RVA: 0x002DA258 File Offset: 0x002D8458
	public static string SkillPerksString(Skill skill)
	{
		string text = "";
		foreach (SkillPerk skillPerk in skill.perks)
		{
			if (Game.IsCorrectDlcActiveForCurrentSave(skillPerk))
			{
				string text2 = GameUtil.NamesOfBuildingsRequiringSkillPerk(skillPerk.Id);
				if (!string.IsNullOrEmpty(text))
				{
					text += "\n";
				}
				text += ((text2 != null) ? text2 : skillPerk.Name);
			}
		}
		return text;
	}

	// Token: 0x0600774E RID: 30542 RVA: 0x002DA2E8 File Offset: 0x002D84E8
	public string CriteriaString(Skill skill)
	{
		bool flag = false;
		string text = "";
		text = text + "<b>" + UI.ROLES_SCREEN.ASSIGNMENT_REQUIREMENTS.TITLE + "</b>\n";
		SkillGroup skillGroup = Db.Get().SkillGroups.Get(skill.skillGroup);
		if (skillGroup != null && skillGroup.relevantAttributes != null)
		{
			foreach (Klei.AI.Attribute attribute in skillGroup.relevantAttributes)
			{
				if (attribute != null)
				{
					text = text + "    • " + string.Format(UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.SKILLGROUP_ENABLED.DESCRIPTION, attribute.Name) + "\n";
					flag = true;
				}
			}
		}
		if (skill.priorSkills.Count > 0)
		{
			flag = true;
			for (int i = 0; i < skill.priorSkills.Count; i++)
			{
				text = text + "    • " + string.Format("{0}", Db.Get().Skills.Get(skill.priorSkills[i]).Name);
				text += "</color>";
				if (i != skill.priorSkills.Count - 1)
				{
					text += "\n";
				}
			}
		}
		if (!flag)
		{
			text = text + "    • " + string.Format(UI.ROLES_SCREEN.ASSIGNMENT_REQUIREMENTS.NONE, skill.Name);
		}
		return text;
	}

	// Token: 0x0600774F RID: 30543 RVA: 0x002DA458 File Offset: 0x002D8658
	public string DuplicantSkillString(Skill skill)
	{
		string text = "";
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.skillsScreen.CurrentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity != null)
		{
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			if (component == null)
			{
				return "";
			}
			LocString loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.CAN_MASTER;
			if (component.HasMasteredSkill(skill.Id))
			{
				if (component.HasBeenGrantedSkill(skill))
				{
					text += "\n";
					loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.SKILL_GRANTED;
					text += string.Format(loc_string, minionIdentity.GetProperName(), skill.Name);
				}
			}
			else
			{
				MinionResume.SkillMasteryConditions[] skillMasteryConditions = component.GetSkillMasteryConditions(skill.Id);
				if (!component.CanMasterSkill(skillMasteryConditions))
				{
					bool flag = false;
					text += "\n";
					loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.CANNOT_MASTER;
					text += string.Format(loc_string, minionIdentity.GetProperName(), skill.Name);
					if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.UnableToLearn))
					{
						flag = true;
						string choreGroupID = Db.Get().SkillGroups.Get(skill.skillGroup).choreGroupID;
						Trait trait;
						minionIdentity.GetComponent<Traits>().IsChoreGroupDisabled(choreGroupID, out trait);
						text += "\n";
						loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.PREVENTED_BY_TRAIT;
						text += string.Format(loc_string, trait.Name);
					}
					if (!flag)
					{
						if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.MissingPreviousSkill))
						{
							text += "\n";
							loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.REQUIRES_PREVIOUS_SKILLS;
							text += string.Format(loc_string, Array.Empty<object>());
						}
					}
					if (!flag)
					{
						if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.NeedsSkillPoints))
						{
							text += "\n";
							loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.REQUIRES_MORE_SKILL_POINTS;
							text += string.Format(loc_string, Array.Empty<object>());
						}
					}
				}
				else
				{
					if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.StressWarning))
					{
						text += "\n";
						loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.STRESS_WARNING_MESSAGE;
						text += string.Format(loc_string, skill.Name, minionIdentity.GetProperName());
					}
					if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.SkillAptitude))
					{
						text += "\n";
						loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.SKILL_APTITUDE;
						text += string.Format(loc_string, minionIdentity.GetProperName(), skill.Name);
					}
				}
			}
		}
		return text;
	}

	// Token: 0x06007750 RID: 30544 RVA: 0x002DA746 File Offset: 0x002D8946
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.ToggleBorderHighlight(true);
		this.skillsScreen.HoverSkill(this.skillID);
		this.soundPlayer.Play(1);
	}

	// Token: 0x06007751 RID: 30545 RVA: 0x002DA76C File Offset: 0x002D896C
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ToggleBorderHighlight(false);
		this.skillsScreen.HoverSkill(null);
	}

	// Token: 0x06007752 RID: 30546 RVA: 0x002DA784 File Offset: 0x002D8984
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.skillsScreen.CurrentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity != null)
		{
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			if (DebugHandler.InstantBuildMode && component.AvailableSkillpoints < 1)
			{
				component.ForceAddSkillPoint();
			}
			MinionResume.SkillMasteryConditions[] skillMasteryConditions = component.GetSkillMasteryConditions(this.skillID);
			bool flag = component.CanMasterSkill(skillMasteryConditions);
			if (component != null && !component.HasMasteredSkill(this.skillID) && flag)
			{
				component.MasterSkill(this.skillID);
				this.skillsScreen.RefreshAll();
			}
		}
	}

	// Token: 0x06007753 RID: 30547 RVA: 0x002DA828 File Offset: 0x002D8A28
	public void OnPointerDown(PointerEventData eventData)
	{
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.skillsScreen.CurrentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		MinionResume minionResume = null;
		bool flag = false;
		if (minionIdentity != null)
		{
			minionResume = minionIdentity.GetComponent<MinionResume>();
			MinionResume.SkillMasteryConditions[] skillMasteryConditions = minionResume.GetSkillMasteryConditions(this.skillID);
			flag = minionResume.CanMasterSkill(skillMasteryConditions);
		}
		if (minionResume != null && !minionResume.HasMasteredSkill(this.skillID) && flag)
		{
			KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
			return;
		}
		KFMOD.PlayUISound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x040052AF RID: 21167
	[SerializeField]
	private LocText Name;

	// Token: 0x040052B0 RID: 21168
	[SerializeField]
	private LocText Description;

	// Token: 0x040052B1 RID: 21169
	[SerializeField]
	private Image TitleBarBG;

	// Token: 0x040052B2 RID: 21170
	[SerializeField]
	private SkillsScreen skillsScreen;

	// Token: 0x040052B3 RID: 21171
	[SerializeField]
	private ToolTip tooltip;

	// Token: 0x040052B4 RID: 21172
	[SerializeField]
	private RectTransform lines_left;

	// Token: 0x040052B5 RID: 21173
	[SerializeField]
	public RectTransform lines_right;

	// Token: 0x040052B6 RID: 21174
	[SerializeField]
	private Color header_color_has_skill;

	// Token: 0x040052B7 RID: 21175
	[SerializeField]
	private Color header_color_can_assign;

	// Token: 0x040052B8 RID: 21176
	[SerializeField]
	private Color header_color_disabled;

	// Token: 0x040052B9 RID: 21177
	[SerializeField]
	private Color line_color_default;

	// Token: 0x040052BA RID: 21178
	[SerializeField]
	private Color line_color_active;

	// Token: 0x040052BB RID: 21179
	[SerializeField]
	private Image hatImage;

	// Token: 0x040052BC RID: 21180
	[SerializeField]
	private GameObject borderHighlight;

	// Token: 0x040052BD RID: 21181
	[SerializeField]
	private ToolTip masteryCount;

	// Token: 0x040052BE RID: 21182
	[SerializeField]
	private GameObject aptitudeBox;

	// Token: 0x040052BF RID: 21183
	[SerializeField]
	private GameObject grantedBox;

	// Token: 0x040052C0 RID: 21184
	[SerializeField]
	private Image grantedIcon;

	// Token: 0x040052C1 RID: 21185
	[SerializeField]
	private GameObject traitDisabledIcon;

	// Token: 0x040052C2 RID: 21186
	public TextStyleSetting TooltipTextStyle_Header;

	// Token: 0x040052C3 RID: 21187
	public TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;

	// Token: 0x040052C4 RID: 21188
	private List<SkillWidget> prerequisiteSkillWidgets = new List<SkillWidget>();

	// Token: 0x040052C5 RID: 21189
	private UILineRenderer[] lines;

	// Token: 0x040052C6 RID: 21190
	private List<Vector2> linePoints = new List<Vector2>();

	// Token: 0x040052C7 RID: 21191
	public Material defaultMaterial;

	// Token: 0x040052C8 RID: 21192
	public Material desaturatedMaterial;

	// Token: 0x040052C9 RID: 21193
	public ButtonSoundPlayer soundPlayer;
}
