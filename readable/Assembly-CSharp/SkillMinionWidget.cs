using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000E94 RID: 3732
[AddComponentMenu("KMonoBehaviour/scripts/SkillMinionWidget")]
public class SkillMinionWidget : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x1700083A RID: 2106
	// (get) Token: 0x06007738 RID: 30520 RVA: 0x002D9327 File Offset: 0x002D7527
	// (set) Token: 0x06007739 RID: 30521 RVA: 0x002D932F File Offset: 0x002D752F
	public IAssignableIdentity assignableIdentity { get; private set; }

	// Token: 0x0600773A RID: 30522 RVA: 0x002D9338 File Offset: 0x002D7538
	public void SetMinon(IAssignableIdentity identity)
	{
		this.assignableIdentity = identity;
		this.portrait.SetIdentityObject(this.assignableIdentity, true);
		base.GetComponent<NotificationHighlightTarget>().targetKey = identity.GetSoleOwner().gameObject.GetInstanceID().ToString();
	}

	// Token: 0x0600773B RID: 30523 RVA: 0x002D9381 File Offset: 0x002D7581
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.ToggleHover(true);
		this.soundPlayer.Play(1);
	}

	// Token: 0x0600773C RID: 30524 RVA: 0x002D9396 File Offset: 0x002D7596
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ToggleHover(false);
	}

	// Token: 0x0600773D RID: 30525 RVA: 0x002D939F File Offset: 0x002D759F
	private void ToggleHover(bool on)
	{
		if (this.skillsScreen.CurrentlySelectedMinion != this.assignableIdentity)
		{
			this.SetColor(on ? this.hover_color : this.unselected_color);
		}
	}

	// Token: 0x0600773E RID: 30526 RVA: 0x002D93CB File Offset: 0x002D75CB
	private void SetColor(Color color)
	{
		this.background.color = color;
		if (this.assignableIdentity != null && this.assignableIdentity as StoredMinionIdentity != null)
		{
			base.GetComponent<CanvasGroup>().alpha = 0.6f;
		}
	}

	// Token: 0x0600773F RID: 30527 RVA: 0x002D9404 File Offset: 0x002D7604
	public void OnPointerClick(PointerEventData eventData)
	{
		this.skillsScreen.CurrentlySelectedMinion = this.assignableIdentity;
		base.GetComponent<NotificationHighlightTarget>().View();
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
	}

	// Token: 0x06007740 RID: 30528 RVA: 0x002D9434 File Offset: 0x002D7634
	public void Refresh()
	{
		if (this.assignableIdentity.IsNullOrDestroyed())
		{
			return;
		}
		this.portrait.SetIdentityObject(this.assignableIdentity, true);
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.assignableIdentity, out minionIdentity, out storedMinionIdentity);
		this.hatDropDown.gameObject.SetActive(true);
		string hat;
		if (minionIdentity != null)
		{
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			int availableSkillpoints = component.AvailableSkillpoints;
			int totalSkillPointsGained = component.TotalSkillPointsGained;
			this.masteryPoints.text = ((availableSkillpoints > 0) ? GameUtil.ApplyBoldString(GameUtil.ColourizeString(new Color(0.5f, 1f, 0.5f, 1f), availableSkillpoints.ToString())) : "0");
			AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(component);
			AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(component);
			this.morale.text = string.Format("{0}/{1}", attributeInstance.GetTotalValue(), attributeInstance2.GetTotalValue());
			this.RefreshToolTip(component);
			List<IListableOption> list = new List<IListableOption>();
			foreach (MinionResume.HatInfo hatInfo in component.GetAllHats())
			{
				list.Add(new HatListable(hatInfo.Source, hatInfo.Hat));
			}
			this.hatDropDown.Initialize(list, new Action<IListableOption, object>(this.OnHatDropEntryClick), new Func<IListableOption, IListableOption, object, int>(this.hatDropDownSort), new Action<DropDownEntry, object>(this.hatDropEntryRefreshAction), false, minionIdentity);
			hat = (string.IsNullOrEmpty(component.TargetHat) ? component.CurrentHat : component.TargetHat);
		}
		else
		{
			ToolTip component2 = base.GetComponent<ToolTip>();
			component2.ClearMultiStringTooltip();
			component2.AddMultiStringTooltip(string.Format(UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, storedMinionIdentity.GetStorageReason(), storedMinionIdentity.GetProperName()), null);
			hat = (string.IsNullOrEmpty(storedMinionIdentity.targetHat) ? storedMinionIdentity.currentHat : storedMinionIdentity.targetHat);
			this.masteryPoints.text = UI.TABLESCREENS.NA;
			this.morale.text = UI.TABLESCREENS.NA;
		}
		bool flag = this.skillsScreen.CurrentlySelectedMinion == this.assignableIdentity;
		if (this.skillsScreen.CurrentlySelectedMinion != null && this.assignableIdentity != null)
		{
			flag = (flag || this.skillsScreen.CurrentlySelectedMinion.GetSoleOwner() == this.assignableIdentity.GetSoleOwner());
		}
		this.SetColor(flag ? this.selected_color : this.unselected_color);
		HierarchyReferences component3 = base.GetComponent<HierarchyReferences>();
		this.RefreshHat(hat);
		component3.GetReference("openButton").gameObject.SetActive(minionIdentity != null);
	}

	// Token: 0x06007741 RID: 30529 RVA: 0x002D9718 File Offset: 0x002D7918
	private void RefreshToolTip(MinionResume resume)
	{
		if (resume != null)
		{
			AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(resume);
			AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(resume);
			ToolTip component = base.GetComponent<ToolTip>();
			component.ClearMultiStringTooltip();
			component.AddMultiStringTooltip(this.assignableIdentity.GetProperName() + "\n\n", this.TooltipTextStyle_Header);
			component.AddMultiStringTooltip(string.Format(UI.SKILLS_SCREEN.CURRENT_MORALE, attributeInstance.GetTotalValue(), attributeInstance2.GetTotalValue()), null);
			component.AddMultiStringTooltip("\n" + UI.DETAILTABS.STATS.NAME + "\n\n", this.TooltipTextStyle_Header);
			foreach (AttributeInstance attributeInstance3 in resume.GetAttributes())
			{
				if (attributeInstance3.Attribute.ShowInUI == Klei.AI.Attribute.Display.Skill)
				{
					string text = UIConstants.ColorPrefixWhite;
					if (attributeInstance3.GetTotalValue() > 0f)
					{
						text = UIConstants.ColorPrefixGreen;
					}
					else if (attributeInstance3.GetTotalValue() < 0f)
					{
						text = UIConstants.ColorPrefixRed;
					}
					component.AddMultiStringTooltip(string.Concat(new string[]
					{
						"    • ",
						attributeInstance3.Name,
						": ",
						text,
						attributeInstance3.GetTotalValue().ToString(),
						UIConstants.ColorSuffix
					}), null);
				}
			}
		}
	}

	// Token: 0x06007742 RID: 30530 RVA: 0x002D98AC File Offset: 0x002D7AAC
	public void RefreshHat(string hat)
	{
		base.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite(string.IsNullOrEmpty(hat) ? "hat_role_none" : hat);
	}

	// Token: 0x06007743 RID: 30531 RVA: 0x002D98E4 File Offset: 0x002D7AE4
	private void OnHatDropEntryClick(IListableOption hatOption, object data)
	{
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.assignableIdentity, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity == null)
		{
			return;
		}
		MinionResume component = minionIdentity.GetComponent<MinionResume>();
		if (hatOption != null)
		{
			base.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite((hatOption as HatListable).hat);
			if (component != null)
			{
				string hat = (hatOption as HatListable).hat;
				component.SetHats(component.CurrentHat, hat);
				if (component.OwnsHat(hat))
				{
					component.CreateHatChangeChore();
				}
			}
		}
		else
		{
			base.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite("hat_role_none");
			if (component != null)
			{
				component.SetHats(component.CurrentHat, null);
				component.ApplyTargetHat();
			}
		}
		this.skillsScreen.RefreshAll();
	}

	// Token: 0x06007744 RID: 30532 RVA: 0x002D99CC File Offset: 0x002D7BCC
	private void hatDropEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		if (entry.entryData != null)
		{
			HatListable hatListable = entry.entryData as HatListable;
			entry.image.sprite = Assets.GetSprite(hatListable.hat);
		}
	}

	// Token: 0x06007745 RID: 30533 RVA: 0x002D9A08 File Offset: 0x002D7C08
	private int hatDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		return 0;
	}

	// Token: 0x040052A1 RID: 21153
	[SerializeField]
	private SkillsScreen skillsScreen;

	// Token: 0x040052A2 RID: 21154
	[SerializeField]
	private CrewPortrait portrait;

	// Token: 0x040052A3 RID: 21155
	[SerializeField]
	private LocText masteryPoints;

	// Token: 0x040052A4 RID: 21156
	[SerializeField]
	private LocText morale;

	// Token: 0x040052A5 RID: 21157
	[SerializeField]
	private Image background;

	// Token: 0x040052A6 RID: 21158
	[SerializeField]
	private Image hat_background;

	// Token: 0x040052A7 RID: 21159
	[SerializeField]
	private Color selected_color;

	// Token: 0x040052A8 RID: 21160
	[SerializeField]
	private Color unselected_color;

	// Token: 0x040052A9 RID: 21161
	[SerializeField]
	private Color hover_color;

	// Token: 0x040052AA RID: 21162
	[SerializeField]
	private DropDown hatDropDown;

	// Token: 0x040052AB RID: 21163
	[SerializeField]
	private TextStyleSetting TooltipTextStyle_Header;

	// Token: 0x040052AC RID: 21164
	[SerializeField]
	private TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;

	// Token: 0x040052AD RID: 21165
	public ButtonSoundPlayer soundPlayer;
}
