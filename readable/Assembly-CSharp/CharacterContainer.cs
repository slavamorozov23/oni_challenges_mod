using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000C4D RID: 3149
public class CharacterContainer : KScreen, ITelepadDeliverableContainer
{
	// Token: 0x06005F78 RID: 24440 RVA: 0x0022F5C6 File Offset: 0x0022D7C6
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x17000700 RID: 1792
	// (get) Token: 0x06005F79 RID: 24441 RVA: 0x0022F5CE File Offset: 0x0022D7CE
	public MinionStartingStats Stats
	{
		get
		{
			return this.stats;
		}
	}

	// Token: 0x06005F7A RID: 24442 RVA: 0x0022F5D8 File Offset: 0x0022D7D8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.allAvailableClothingOutfits = new List<Option<ClothingOutfitTarget>>();
		foreach (ClothingOutfitTarget value in from outfit in ClothingOutfitTarget.GetAllTemplates()
		where outfit.OutfitType == ClothingOutfitUtility.OutfitType.Clothing
		select outfit)
		{
			bool flag = false;
			foreach (string id in value.ReadItems())
			{
				ClothingItemResource clothingItemResource = Db.Get().Permits.ClothingItems.TryGet(id);
				if (clothingItemResource != null && !clothingItemResource.IsUnlocked())
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.allAvailableClothingOutfits.Add(value);
			}
		}
		this.Initialize();
		this.characterNameTitle.OnStartedEditing += this.OnStartedEditing;
		this.characterNameTitle.OnNameChanged += this.OnNameChanged;
		this.reshuffleButton.onClick += delegate()
		{
			this.Reshuffle(true);
		};
		List<IListableOption> list = new List<IListableOption>();
		foreach (SkillGroup item in new List<SkillGroup>(Db.Get().SkillGroups.resources))
		{
			list.Add(item);
		}
		list.Remove(Db.Get().SkillGroups.BionicSkills);
		this.archetypeDropDown.Initialize(list, new Action<IListableOption, object>(this.OnArchetypeEntryClick), new Func<IListableOption, IListableOption, object, int>(this.archetypeDropDownSort), new Action<DropDownEntry, object>(this.archetypeDropEntryRefreshAction), false, null);
		this.archetypeDropDown.CustomizeEmptyRow(Strings.Get("STRINGS.UI.CHARACTERCONTAINER_NOARCHETYPESELECTED"), this.noArchetypeIcon);
		List<IListableOption> contentKeys = new List<IListableOption>
		{
			new CharacterContainer.MinionModelOption(DUPLICANTS.MODEL.STANDARD.NAME, new List<Tag>
			{
				GameTags.Minions.Models.Standard
			}, Assets.GetSprite("ui_duplicant_minion_selection")),
			new CharacterContainer.MinionModelOption(DUPLICANTS.MODEL.BIONIC.NAME, new List<Tag>
			{
				GameTags.Minions.Models.Bionic
			}, Assets.GetSprite("ui_duplicant_bionicminion_selection"))
		};
		this.modelDropDown.Initialize(contentKeys, new Action<IListableOption, object>(this.OnModelEntryClick), new Func<IListableOption, IListableOption, object, int>(this.modelDropDownSort), new Action<DropDownEntry, object>(this.modelDropEntryRefreshAction), true, null);
		this.modelDropDown.CustomizeEmptyRow(UI.CHARACTERCONTAINER_ALL_MODELS, Assets.GetSprite(this.allModelSprite));
		base.StartCoroutine(this.DelayedGeneration());
	}

	// Token: 0x06005F7B RID: 24443 RVA: 0x0022F898 File Offset: 0x0022DA98
	public void ForceStopEditingTitle()
	{
		this.characterNameTitle.ForceStopEditing();
	}

	// Token: 0x06005F7C RID: 24444 RVA: 0x0022F8A5 File Offset: 0x0022DAA5
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x06005F7D RID: 24445 RVA: 0x0022F8AC File Offset: 0x0022DAAC
	private IEnumerator DelayedGeneration()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		this.GenerateCharacter(this.controller.IsStarterMinion, null);
		yield break;
	}

	// Token: 0x06005F7E RID: 24446 RVA: 0x0022F8BB File Offset: 0x0022DABB
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.animController != null)
		{
			this.animController.gameObject.DeleteObject();
			this.animController = null;
		}
	}

	// Token: 0x06005F7F RID: 24447 RVA: 0x0022F8E8 File Offset: 0x0022DAE8
	protected override void OnForcedCleanUp()
	{
		CharacterContainer.containers.Remove(this);
		base.OnForcedCleanUp();
	}

	// Token: 0x06005F80 RID: 24448 RVA: 0x0022F8FC File Offset: 0x0022DAFC
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.controller != null)
		{
			CharacterSelectionController characterSelectionController = this.controller;
			characterSelectionController.OnLimitReachedEvent = (System.Action)Delegate.Remove(characterSelectionController.OnLimitReachedEvent, new System.Action(this.OnCharacterSelectionLimitReached));
			CharacterSelectionController characterSelectionController2 = this.controller;
			characterSelectionController2.OnLimitUnreachedEvent = (System.Action)Delegate.Remove(characterSelectionController2.OnLimitUnreachedEvent, new System.Action(this.OnCharacterSelectionLimitUnReached));
			CharacterSelectionController characterSelectionController3 = this.controller;
			characterSelectionController3.OnReshuffleEvent = (Action<bool>)Delegate.Remove(characterSelectionController3.OnReshuffleEvent, new Action<bool>(this.Reshuffle));
		}
	}

	// Token: 0x06005F81 RID: 24449 RVA: 0x0022F994 File Offset: 0x0022DB94
	private void Initialize()
	{
		this.iconGroups = new List<GameObject>();
		this.traitEntries = new List<GameObject>();
		this.expectationLabels = new List<LocText>();
		this.aptitudeEntries = new List<GameObject>();
		if (CharacterContainer.containers == null)
		{
			CharacterContainer.containers = new List<CharacterContainer>();
		}
		CharacterContainer.containers.Add(this);
	}

	// Token: 0x06005F82 RID: 24450 RVA: 0x0022F9E9 File Offset: 0x0022DBE9
	private void OnNameChanged(string newName)
	{
		this.stats.Name = newName;
		this.stats.personality.Name = newName;
		this.description.text = this.stats.personality.description;
	}

	// Token: 0x06005F83 RID: 24451 RVA: 0x0022FA23 File Offset: 0x0022DC23
	private void OnStartedEditing()
	{
		KScreenManager.Instance.RefreshStack();
	}

	// Token: 0x06005F84 RID: 24452 RVA: 0x0022FA30 File Offset: 0x0022DC30
	public void SetMinion(MinionStartingStats statsProposed)
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			this.DeselectDeliverable();
		}
		this.stats = statsProposed;
		if (this.animController != null)
		{
			UnityEngine.Object.Destroy(this.animController.gameObject);
			this.animController = null;
		}
		this.SetAnimator();
		this.SetInfoText();
		base.StartCoroutine(this.SetAttributes());
		this.selectButton.ClearOnClick();
		if (!this.controller.IsStarterMinion)
		{
			this.selectButton.enabled = true;
			this.selectButton.onClick += delegate()
			{
				this.SelectDeliverable();
			};
		}
	}

	// Token: 0x06005F85 RID: 24453 RVA: 0x0022FAE4 File Offset: 0x0022DCE4
	public void GenerateCharacter(bool is_starter, string guaranteedAptitudeID = null)
	{
		int num = 0;
		do
		{
			this.stats = new MinionStartingStats(this.permittedModels, is_starter, guaranteedAptitudeID, null, false);
			num++;
		}
		while (this.IsCharacterInvalid() && num < 20);
		if (this.animController != null)
		{
			UnityEngine.Object.Destroy(this.animController.gameObject);
			this.animController = null;
		}
		this.SetAnimator();
		this.SetInfoText();
		base.StartCoroutine(this.SetAttributes());
		this.selectButton.ClearOnClick();
		if (!this.controller.IsStarterMinion)
		{
			this.selectButton.enabled = true;
			this.selectButton.onClick += delegate()
			{
				this.SelectDeliverable();
			};
		}
		Option<ClothingOutfitTarget> selectedOutfit = ClothingOutfitTarget.TryFromTemplateId(this.stats.personality.GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.Clothing));
		if (selectedOutfit.IsSome())
		{
			this.outfitSelectorIndex = this.allAvailableClothingOutfits.FindIndex((Option<ClothingOutfitTarget> outfit) => outfit.Unwrap().OutfitId == selectedOutfit.Unwrap().OutfitId);
		}
		else
		{
			this.outfitSelectorIndex = this.allAvailableClothingOutfits.FindIndex((Option<ClothingOutfitTarget> outfit) => outfit.Unwrap().OutfitId == this.stats.personality.GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.Clothing));
		}
		if (this.outfitSelectorIndex == -1)
		{
			this.outfitSelectorIndex = this.allAvailableClothingOutfits.FindIndex((Option<ClothingOutfitTarget> outfit) => outfit.Unwrap().OutfitId == CharacterContainer.defaultShirtIdxToDefaultOutfitID[this.stats.personality.body]);
		}
		this.RefreshOutfitSelector();
	}

	// Token: 0x06005F86 RID: 24454 RVA: 0x0022FC30 File Offset: 0x0022DE30
	private void SetAnimator()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(GameTags.MinionSelectPreview), this.contentBody.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = this.baseCharacterScale;
		}
		BaseMinionConfig.ConfigureSymbols(this.animController.gameObject, true);
		this.stats.ApplyTraits(this.animController.gameObject);
		this.stats.ApplyRace(this.animController.gameObject);
		this.stats.ApplyAccessories(this.animController.gameObject);
		this.stats.ApplyOutfit(this.stats.personality, this.animController.gameObject, this.stats.GetSelectedOutfitOption());
		this.stats.ApplyJoyResponseOutfit(this.stats.personality, this.animController.gameObject);
		this.stats.ApplyExperience(this.animController.gameObject);
		HashedString idleAnim = this.GetIdleAnim(this.stats);
		this.idle_anim = Assets.GetAnim(idleAnim);
		if (this.idle_anim != null)
		{
			this.animController.AddAnimOverrides(this.idle_anim, 0f);
		}
		KAnimFile anim = Assets.GetAnim(new HashedString("crewSelect_fx_kanim"));
		this.bgAnimController.SwapAnims(new KAnimFile[]
		{
			Assets.GetAnim(CharacterContainer.portraitBGAnims[this.stats.personality.model])
		});
		this.bgAnimController.Play("crewSelect_bg", KAnim.PlayMode.Loop, 1f, 0f);
		if (anim != null)
		{
			this.animController.AddAnimOverrides(anim, 0f);
		}
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06005F87 RID: 24455 RVA: 0x0022FE28 File Offset: 0x0022E028
	private HashedString GetIdleAnim(MinionStartingStats minionStartingStats)
	{
		List<HashedString> list = new List<HashedString>();
		foreach (KeyValuePair<HashedString, string[]> keyValuePair in CharacterContainer.traitIdleAnims)
		{
			foreach (Trait trait in minionStartingStats.Traits)
			{
				if (keyValuePair.Value.Contains(trait.Id))
				{
					list.Add(keyValuePair.Key);
				}
			}
			if (keyValuePair.Value.Contains(minionStartingStats.joyTrait.Id) || keyValuePair.Value.Contains(minionStartingStats.stressTrait.Id))
			{
				list.Add(keyValuePair.Key);
			}
		}
		if (list.Count > 0)
		{
			return list.ToArray()[UnityEngine.Random.Range(0, list.Count)];
		}
		return CharacterContainer.idleAnims[UnityEngine.Random.Range(0, CharacterContainer.idleAnims.Length)];
	}

	// Token: 0x06005F88 RID: 24456 RVA: 0x0022FF54 File Offset: 0x0022E154
	private string GetOutfitName(int index)
	{
		if (index == -1)
		{
			return Strings.Get("STRINGS.UI.CHARACTERCONTAINER_NO_OUTFIT");
		}
		return this.allAvailableClothingOutfits[index].Unwrap().ReadName();
	}

	// Token: 0x06005F89 RID: 24457 RVA: 0x0022FF94 File Offset: 0x0022E194
	private void RefreshOutfitSelector()
	{
		CharacterContainer.<>c__DisplayClass76_0 CS$<>8__locals1 = new CharacterContainer.<>c__DisplayClass76_0();
		CS$<>8__locals1.<>4__this = this;
		Image reference = this.outfitSelectorReferences.GetReference<Image>("CurrentOutfitIcon");
		Image reference2 = this.outfitSelectorReferences.GetReference<Image>("NextOutfitIcon");
		MultiToggle component = reference2.transform.parent.GetComponent<MultiToggle>();
		Image reference3 = this.outfitSelectorReferences.GetReference<Image>("PreviousOutfitIcon");
		MultiToggle component2 = reference3.transform.parent.GetComponent<MultiToggle>();
		MultiToggle reference4 = this.outfitSelectorReferences.GetReference<MultiToggle>("PreviousOutfitButton");
		MultiToggle reference5 = this.outfitSelectorReferences.GetReference<MultiToggle>("NextOutfitButton");
		CS$<>8__locals1.expandedMenu = this.outfitSelectorReferences.GetReference<RectTransform>("ExpandedMenu");
		CS$<>8__locals1.expandButton = this.outfitSelectorReferences.GetReference<MultiToggle>("CollapsedButton");
		CS$<>8__locals1.expandButton.onClick = null;
		MultiToggle expandButton = CS$<>8__locals1.expandButton;
		expandButton.onClick = (System.Action)Delegate.Combine(expandButton.onClick, new System.Action(delegate()
		{
			CS$<>8__locals1.<>4__this.outfitSelectorExpanded = !CS$<>8__locals1.<>4__this.outfitSelectorExpanded;
			CS$<>8__locals1.expandButton.gameObject.SetActive(!CS$<>8__locals1.<>4__this.outfitSelectorExpanded);
			CS$<>8__locals1.expandedMenu.gameObject.SetActive(CS$<>8__locals1.<>4__this.outfitSelectorExpanded);
			CS$<>8__locals1.<>4__this.RefreshOutfitSelector();
		}));
		MultiToggle reference6 = this.outfitSelectorReferences.GetReference<MultiToggle>("CurrentOutfitButton");
		reference6.onClick = null;
		reference6.onClick = (System.Action)Delegate.Combine(reference6.onClick, new System.Action(delegate()
		{
			CS$<>8__locals1.<>4__this.outfitSelectorExpanded = !CS$<>8__locals1.<>4__this.outfitSelectorExpanded;
			CS$<>8__locals1.expandButton.gameObject.SetActive(!CS$<>8__locals1.<>4__this.outfitSelectorExpanded);
			CS$<>8__locals1.expandedMenu.gameObject.SetActive(CS$<>8__locals1.<>4__this.outfitSelectorExpanded);
			CS$<>8__locals1.<>4__this.RefreshOutfitSelector();
		}));
		reference.sprite = CS$<>8__locals1.<RefreshOutfitSelector>g__GetClothingIcon|0(0);
		CS$<>8__locals1.expandButton.gameObject.GetComponentInChildrenOnly<Image>().sprite = reference.sprite;
		reference2.sprite = CS$<>8__locals1.<RefreshOutfitSelector>g__GetClothingIcon|0(-1);
		reference3.sprite = CS$<>8__locals1.<RefreshOutfitSelector>g__GetClothingIcon|0(1);
		CS$<>8__locals1.expandButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.SafeStringFormat(Strings.Get("STRINGS.UI.CHARACTERCONTAINER_EXPAND_OUTFIT_SELECTOR_BUTTON"), new object[]
		{
			this.GetOutfitName(this.outfitSelectorIndex)
		}));
		string outfitName = this.GetOutfitName(this.outfitSelectorIndex);
		reference.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(outfitName + "\n\n" + UI.CHARACTERCONTAINER_CONFIRM_OUTFIT_SELECTION_TOOLTIP);
		reference4.onClick = null;
		MultiToggle multiToggle = reference4;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			CS$<>8__locals1.<>4__this.outfitSelectorIndex = CS$<>8__locals1.<>4__this.GetOutfitSelectorIndex(1);
			CS$<>8__locals1.<>4__this.RefreshOutfitSelector();
			CS$<>8__locals1.<>4__this.stats.ApplyOutfit(CS$<>8__locals1.<>4__this.stats.personality, CS$<>8__locals1.<>4__this.animController.gameObject, (CS$<>8__locals1.<>4__this.outfitSelectorIndex == -1) ? default(Option<ClothingOutfitTarget>) : CS$<>8__locals1.<>4__this.allAvailableClothingOutfits[CS$<>8__locals1.<>4__this.outfitSelectorIndex]);
			if (CS$<>8__locals1.<>4__this.fxAnim != null)
			{
				CS$<>8__locals1.<>4__this.fxAnim.Play("loop", KAnim.PlayMode.Once, 1f, 0f);
			}
			UISounds.Instance.PlaySound3D(GlobalAssets.GetSound("DupeShuffle", false));
		}));
		component2.onClick = reference4.onClick;
		int index = this.GetOutfitSelectorIndex(1);
		string outfitName2 = this.GetOutfitName(index);
		reference3.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(outfitName2);
		reference5.onClick = null;
		MultiToggle multiToggle2 = reference5;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			CS$<>8__locals1.<>4__this.outfitSelectorIndex = CS$<>8__locals1.<>4__this.GetOutfitSelectorIndex(-1);
			CS$<>8__locals1.<>4__this.RefreshOutfitSelector();
			CS$<>8__locals1.<>4__this.stats.ApplyOutfit(CS$<>8__locals1.<>4__this.stats.personality, CS$<>8__locals1.<>4__this.animController.gameObject, (CS$<>8__locals1.<>4__this.outfitSelectorIndex == -1) ? default(Option<ClothingOutfitTarget>) : CS$<>8__locals1.<>4__this.allAvailableClothingOutfits[CS$<>8__locals1.<>4__this.outfitSelectorIndex]);
			if (CS$<>8__locals1.<>4__this.fxAnim != null)
			{
				CS$<>8__locals1.<>4__this.fxAnim.Play("loop", KAnim.PlayMode.Once, 1f, 0f);
			}
			UISounds.Instance.PlaySound3D(GlobalAssets.GetSound("DupeShuffle", false));
		}));
		component.onClick = reference5.onClick;
		int index2 = this.GetOutfitSelectorIndex(-1);
		string outfitName3 = this.GetOutfitName(index2);
		reference2.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(outfitName3);
		this.stats.overrideOutfitID = ((this.outfitSelectorIndex == -1) ? null : this.allAvailableClothingOutfits[this.outfitSelectorIndex].Unwrap().OutfitId);
	}

	// Token: 0x06005F8A RID: 24458 RVA: 0x00230278 File Offset: 0x0022E478
	private int GetOutfitSelectorIndex(int indexOffset)
	{
		int count = this.allAvailableClothingOutfits.Count;
		int num = this.outfitSelectorIndex + indexOffset;
		if (num >= count)
		{
			num = -1;
		}
		if (num < -1)
		{
			num = count - 1;
		}
		return num;
	}

	// Token: 0x06005F8B RID: 24459 RVA: 0x002302AC File Offset: 0x0022E4AC
	private void SetInfoText()
	{
		this.traitEntries.ForEach(delegate(GameObject tl)
		{
			UnityEngine.Object.Destroy(tl.gameObject);
		});
		this.traitEntries.Clear();
		this.characterNameTitle.SetTitle(this.stats.Name);
		this.traitHeaderLabel.SetText((this.stats.personality.model == GameTags.Minions.Models.Bionic) ? UI.CHARACTERCONTAINER_TRAITS_TITLE_BIONIC : UI.CHARACTERCONTAINER_TRAITS_TITLE);
		for (int i = 1; i < this.stats.Traits.Count; i++)
		{
			Trait trait = this.stats.Traits[i];
			LocText locText = trait.PositiveTrait ? this.goodTrait : this.badTrait;
			LocText locText2 = Util.KInstantiateUI<LocText>(locText.gameObject, locText.transform.parent.gameObject, false);
			locText2.gameObject.SetActive(true);
			locText2.text = this.stats.Traits[i].GetName();
			locText2.color = (trait.PositiveTrait ? Constants.POSITIVE_COLOR : Constants.NEGATIVE_COLOR);
			locText2.GetComponent<ToolTip>().SetSimpleTooltip(trait.GetTooltip());
			for (int j = 0; j < trait.SelfModifiers.Count; j++)
			{
				GameObject gameObject = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject.SetActive(true);
				LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
				string format = (trait.SelfModifiers[j].Value > 0f) ? UI.CHARACTERCONTAINER_ATTRIBUTEMODIFIER_INCREASED : UI.CHARACTERCONTAINER_ATTRIBUTEMODIFIER_DECREASED;
				componentInChildren.text = string.Format(format, Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + trait.SelfModifiers[j].AttributeId.ToUpper() + ".NAME"));
				trait.SelfModifiers[j].AttributeId == "GermResistance";
				Klei.AI.Attribute attribute = Db.Get().Attributes.Get(trait.SelfModifiers[j].AttributeId);
				string text = attribute.Description;
				text = string.Concat(new string[]
				{
					text,
					"\n\n",
					Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + trait.SelfModifiers[j].AttributeId.ToUpper() + ".NAME"),
					": ",
					trait.SelfModifiers[j].GetFormattedString()
				});
				List<AttributeConverter> convertersForAttribute = Db.Get().AttributeConverters.GetConvertersForAttribute(attribute);
				for (int k = 0; k < convertersForAttribute.Count; k++)
				{
					string text2 = convertersForAttribute[k].DescriptionFromAttribute(convertersForAttribute[k].multiplier * trait.SelfModifiers[j].Value, null);
					if (text2 != "")
					{
						text = text + "\n    • " + text2;
					}
				}
				componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(text);
				this.traitEntries.Add(gameObject);
			}
			if (trait.disabledChoreGroups != null)
			{
				GameObject gameObject2 = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject2.SetActive(true);
				LocText componentInChildren2 = gameObject2.GetComponentInChildren<LocText>();
				componentInChildren2.text = trait.GetDisabledChoresString(false);
				string text3 = "";
				string text4 = "";
				for (int l = 0; l < trait.disabledChoreGroups.Length; l++)
				{
					if (l > 0)
					{
						text3 += ", ";
						text4 += "\n";
					}
					text3 += trait.disabledChoreGroups[l].Name;
					text4 += trait.disabledChoreGroups[l].description;
				}
				componentInChildren2.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(DUPLICANTS.TRAITS.CANNOT_DO_TASK_TOOLTIP, text3, text4));
				this.traitEntries.Add(gameObject2);
			}
			if (trait.ignoredEffects != null && trait.ignoredEffects.Length != 0)
			{
				GameObject gameObject3 = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject3.SetActive(true);
				LocText componentInChildren3 = gameObject3.GetComponentInChildren<LocText>();
				componentInChildren3.text = trait.GetIgnoredEffectsString(false);
				string text5 = "";
				for (int m = 0; m < trait.ignoredEffects.Length; m++)
				{
					if (m > 0)
					{
						text5 += "\n";
					}
					text5 += string.Format(DUPLICANTS.TRAITS.IGNORED_EFFECTS_TOOLTIP, Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + trait.ignoredEffects[m].ToUpper() + ".NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + trait.ignoredEffects[m].ToUpper() + ".CAUSE"));
					if (m < trait.ignoredEffects.Length - 1)
					{
						text5 += ",";
					}
				}
				componentInChildren3.GetComponent<ToolTip>().SetSimpleTooltip(text5);
				this.traitEntries.Add(gameObject3);
			}
			StringEntry stringEntry = null;
			if (trait.ShortDescCB != null || Strings.TryGet("STRINGS.DUPLICANTS.TRAITS." + trait.Id.ToUpper() + ".SHORT_DESC", out stringEntry))
			{
				string text6 = (trait.ShortDescCB != null) ? trait.ShortDescCB() : stringEntry.String;
				string simpleTooltip = (trait.ShortDescTooltipCB != null) ? trait.ShortDescTooltipCB() : Strings.Get("STRINGS.DUPLICANTS.TRAITS." + trait.Id.ToUpper() + ".SHORT_DESC_TOOLTIP");
				GameObject gameObject4 = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject4.SetActive(true);
				LocText componentInChildren4 = gameObject4.GetComponentInChildren<LocText>();
				componentInChildren4.text = text6;
				componentInChildren4.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
				this.traitEntries.Add(gameObject4);
			}
			this.traitEntries.Add(locText2.gameObject);
		}
		this.aptitudeEntries.ForEach(delegate(GameObject al)
		{
			UnityEngine.Object.Destroy(al.gameObject);
		});
		this.aptitudeEntries.Clear();
		this.expectationLabels.ForEach(delegate(LocText el)
		{
			UnityEngine.Object.Destroy(el.gameObject);
		});
		this.expectationLabels.Clear();
		if (this.stats.personality.model == GameTags.Minions.Models.Bionic)
		{
			this.aptitudeContainer.SetActive(false);
		}
		else
		{
			this.aptitudeContainer.SetActive(true);
			List<string> list = new List<string>();
			foreach (KeyValuePair<SkillGroup, float> keyValuePair in this.stats.skillAptitudes)
			{
				if (keyValuePair.Value != 0f)
				{
					SkillGroup skillGroup = Db.Get().SkillGroups.Get(keyValuePair.Key.IdHash);
					if (skillGroup == null)
					{
						global::Debug.LogWarningFormat("Role group not found for aptitude: {0}", new object[]
						{
							keyValuePair.Key
						});
					}
					else
					{
						GameObject gameObject5 = Util.KInstantiateUI(this.aptitudeEntry.gameObject, this.aptitudeContainer, false);
						LocText locText3 = Util.KInstantiateUI<LocText>(this.aptitudeLabel.gameObject, gameObject5, false);
						locText3.gameObject.SetActive(true);
						locText3.text = skillGroup.Name;
						string simpleTooltip2;
						if (skillGroup.choreGroupID != "")
						{
							ChoreGroup choreGroup = Db.Get().ChoreGroups.Get(skillGroup.choreGroupID);
							simpleTooltip2 = string.Format(DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION_CHOREGROUP, skillGroup.Name, DUPLICANTSTATS.APTITUDE_BONUS, choreGroup.description);
						}
						else
						{
							simpleTooltip2 = string.Format(DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION, skillGroup.Name, DUPLICANTSTATS.APTITUDE_BONUS);
						}
						locText3.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip2);
						string id = keyValuePair.Key.relevantAttributes[0].Id;
						float num = (float)this.stats.StartingLevels[id];
						LocText locText4 = Util.KInstantiateUI<LocText>(this.attributeLabelAptitude.gameObject, gameObject5, false);
						locText4.gameObject.SetActive(!list.Contains(id));
						locText4.text = "+" + num.ToString() + " " + keyValuePair.Key.relevantAttributes[0].Name;
						string text7 = keyValuePair.Key.relevantAttributes[0].Description;
						text7 = string.Concat(new string[]
						{
							text7,
							"\n\n",
							keyValuePair.Key.relevantAttributes[0].Name,
							": +",
							num.ToString()
						});
						List<AttributeConverter> convertersForAttribute2 = Db.Get().AttributeConverters.GetConvertersForAttribute(keyValuePair.Key.relevantAttributes[0]);
						for (int n = 0; n < convertersForAttribute2.Count; n++)
						{
							text7 = text7 + "\n    • " + convertersForAttribute2[n].DescriptionFromAttribute(convertersForAttribute2[n].multiplier * num, null);
						}
						list.Add(id);
						locText4.GetComponent<ToolTip>().SetSimpleTooltip(text7);
						gameObject5.gameObject.SetActive(true);
						this.aptitudeEntries.Add(gameObject5);
					}
				}
			}
		}
		if (this.stats.stressTrait != null)
		{
			LocText locText5 = Util.KInstantiateUI<LocText>(this.expectationRight.gameObject, this.expectationRight.transform.parent.gameObject, false);
			locText5.gameObject.SetActive(true);
			locText5.text = string.Format(UI.CHARACTERCONTAINER_STRESSTRAIT, this.stats.stressTrait.GetName());
			locText5.GetComponent<ToolTip>().SetSimpleTooltip(this.stats.stressTrait.GetTooltip());
			this.expectationLabels.Add(locText5);
		}
		if (this.stats.joyTrait != null)
		{
			LocText locText6 = Util.KInstantiateUI<LocText>(this.expectationRight.gameObject, this.expectationRight.transform.parent.gameObject, false);
			locText6.gameObject.SetActive(true);
			locText6.text = string.Format(UI.CHARACTERCONTAINER_JOYTRAIT, this.stats.joyTrait.GetName());
			locText6.GetComponent<ToolTip>().SetSimpleTooltip(this.stats.joyTrait.GetTooltip());
			this.expectationLabels.Add(locText6);
		}
		this.description.text = this.stats.personality.description;
	}

	// Token: 0x06005F8C RID: 24460 RVA: 0x00230DE0 File Offset: 0x0022EFE0
	private IEnumerator SetAttributes()
	{
		yield return null;
		this.iconGroups.ForEach(delegate(GameObject icg)
		{
			UnityEngine.Object.Destroy(icg);
		});
		this.iconGroups.Clear();
		List<AttributeInstance> list = new List<AttributeInstance>(this.animController.gameObject.GetAttributes().AttributeTable);
		list.RemoveAll((AttributeInstance at) => at.Attribute.ShowInUI != Klei.AI.Attribute.Display.Skill);
		list = (from at in list
		orderby at.Name
		select at).ToList<AttributeInstance>();
		for (int i = 0; i < list.Count; i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.iconGroup.gameObject, this.iconGroup.transform.parent.gameObject, false);
			LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
			gameObject.SetActive(true);
			float totalValue = list[i].GetTotalValue();
			if (totalValue > 0f)
			{
				componentInChildren.color = Constants.POSITIVE_COLOR;
			}
			else if (totalValue == 0f)
			{
				componentInChildren.color = Constants.NEUTRAL_COLOR;
			}
			else
			{
				componentInChildren.color = Constants.NEGATIVE_COLOR;
			}
			componentInChildren.text = string.Format(UI.CHARACTERCONTAINER_SKILL_VALUE, GameUtil.AddPositiveSign(totalValue.ToString(), totalValue > 0f), list[i].Name);
			AttributeInstance attributeInstance = list[i];
			string text = attributeInstance.Description;
			if (attributeInstance.Attribute.converters.Count > 0)
			{
				text += "\n";
				foreach (AttributeConverter attributeConverter in attributeInstance.Attribute.converters)
				{
					AttributeConverterInstance converter = this.animController.gameObject.GetComponent<Klei.AI.AttributeConverters>().GetConverter(attributeConverter.Id);
					string text2 = converter.DescriptionFromAttribute(converter.Evaluate(), converter.gameObject);
					if (text2 != null)
					{
						text = text + "\n" + text2;
					}
				}
			}
			gameObject.GetComponent<ToolTip>().SetSimpleTooltip(text);
			this.iconGroups.Add(gameObject);
		}
		yield break;
	}

	// Token: 0x06005F8D RID: 24461 RVA: 0x00230DF0 File Offset: 0x0022EFF0
	public void SelectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.AddDeliverable(this.stats);
		}
		if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
		{
			MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 1f, true);
		}
		this.selectButton.GetComponent<ImageToggleState>().SetActive();
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.DeselectDeliverable();
			if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
			{
				MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 0f, true);
			}
		};
		this.selectedBorder.SetActive(true);
		this.titleBar.color = this.selectedTitleColor;
		this.animController.Play("cheer_pre", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Play("cheer_loop", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06005F8E RID: 24462 RVA: 0x00230ED8 File Offset: 0x0022F0D8
	public void DeselectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.RemoveDeliverable(this.stats);
		}
		this.selectButton.GetComponent<ImageToggleState>().SetInactive();
		this.selectButton.Deselect();
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
		this.selectedBorder.SetActive(false);
		this.titleBar.color = this.deselectedTitleColor;
		this.animController.Queue("cheer_pst", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06005F8F RID: 24463 RVA: 0x00230F9E File Offset: 0x0022F19E
	private void OnReplacedEvent(ITelepadDeliverable deliverable)
	{
		if (deliverable == this.stats)
		{
			this.DeselectDeliverable();
		}
	}

	// Token: 0x06005F90 RID: 24464 RVA: 0x00230FB0 File Offset: 0x0022F1B0
	private void OnCharacterSelectionLimitReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		if (this.controller.AllowsReplacing)
		{
			this.selectButton.onClick += this.ReplaceCharacterSelection;
			return;
		}
		this.selectButton.onClick += this.CantSelectCharacter;
	}

	// Token: 0x06005F91 RID: 24465 RVA: 0x00231026 File Offset: 0x0022F226
	private void CantSelectCharacter()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x06005F92 RID: 24466 RVA: 0x00231038 File Offset: 0x0022F238
	private void ReplaceCharacterSelection()
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.RemoveLast();
		this.SelectDeliverable();
	}

	// Token: 0x06005F93 RID: 24467 RVA: 0x0023105C File Offset: 0x0022F25C
	private void OnCharacterSelectionLimitUnReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
	}

	// Token: 0x06005F94 RID: 24468 RVA: 0x002310B0 File Offset: 0x0022F2B0
	public void SetReshufflingState(bool enable)
	{
		this.reshuffleButton.gameObject.SetActive(enable);
		this.archetypeDropDown.gameObject.SetActive(enable);
		this.modelDropDown.transform.parent.gameObject.SetActive(enable && Game.IsDlcActiveForCurrentSave("DLC3_ID"));
	}

	// Token: 0x06005F95 RID: 24469 RVA: 0x0023110C File Offset: 0x0022F30C
	public void Reshuffle(bool is_starter)
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			this.DeselectDeliverable();
		}
		if (this.fxAnim != null)
		{
			this.fxAnim.Play("loop", KAnim.PlayMode.Once, 1f, 0f);
		}
		this.GenerateCharacter(is_starter, this.guaranteedAptitudeID);
	}

	// Token: 0x06005F96 RID: 24470 RVA: 0x0023117C File Offset: 0x0022F37C
	public void SetController(CharacterSelectionController csc)
	{
		if (csc == this.controller)
		{
			return;
		}
		this.controller = csc;
		CharacterSelectionController characterSelectionController = this.controller;
		characterSelectionController.OnLimitReachedEvent = (System.Action)Delegate.Combine(characterSelectionController.OnLimitReachedEvent, new System.Action(this.OnCharacterSelectionLimitReached));
		CharacterSelectionController characterSelectionController2 = this.controller;
		characterSelectionController2.OnLimitUnreachedEvent = (System.Action)Delegate.Combine(characterSelectionController2.OnLimitUnreachedEvent, new System.Action(this.OnCharacterSelectionLimitUnReached));
		CharacterSelectionController characterSelectionController3 = this.controller;
		characterSelectionController3.OnReshuffleEvent = (Action<bool>)Delegate.Combine(characterSelectionController3.OnReshuffleEvent, new Action<bool>(this.Reshuffle));
		CharacterSelectionController characterSelectionController4 = this.controller;
		characterSelectionController4.OnReplacedEvent = (Action<ITelepadDeliverable>)Delegate.Combine(characterSelectionController4.OnReplacedEvent, new Action<ITelepadDeliverable>(this.OnReplacedEvent));
	}

	// Token: 0x06005F97 RID: 24471 RVA: 0x0023123C File Offset: 0x0022F43C
	public void DisableSelectButton()
	{
		this.selectButton.soundPlayer.AcceptClickCondition = (() => false);
		this.selectButton.GetComponent<ImageToggleState>().SetDisabled();
		this.selectButton.soundPlayer.Enabled = false;
	}

	// Token: 0x06005F98 RID: 24472 RVA: 0x0023129C File Offset: 0x0022F49C
	private bool IsCharacterInvalid()
	{
		return CharacterContainer.containers.Find((CharacterContainer container) => container != null && container.stats != null && container != this && container.stats.personality.Id == this.stats.personality.Id && container.stats.IsValid) != null || (Game.Instance != null && !Game.IsDlcActiveForCurrentSave(this.stats.personality.requiredDlcId)) || (this.stats.personality.model != GameTags.Minions.Models.Bionic && Components.LiveMinionIdentities.Items.Any((MinionIdentity id) => id.personalityResourceId == this.stats.personality.Id));
	}

	// Token: 0x06005F99 RID: 24473 RVA: 0x0023132B File Offset: 0x0022F52B
	public string GetValueColor(bool isPositive)
	{
		if (!isPositive)
		{
			return "<color=#ff2222ff>";
		}
		return "<color=green>";
	}

	// Token: 0x06005F9A RID: 24474 RVA: 0x0023133B File Offset: 0x0022F53B
	public override void OnPointerEnter(PointerEventData eventData)
	{
		this.scroll_rect.mouseIsOver = true;
		base.OnPointerEnter(eventData);
	}

	// Token: 0x06005F9B RID: 24475 RVA: 0x00231350 File Offset: 0x0022F550
	public override void OnPointerExit(PointerEventData eventData)
	{
		this.scroll_rect.mouseIsOver = false;
		base.OnPointerExit(eventData);
	}

	// Token: 0x06005F9C RID: 24476 RVA: 0x00231368 File Offset: 0x0022F568
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.IsAction(global::Action.Escape) || e.IsAction(global::Action.MouseRight))
		{
			this.characterNameTitle.ForceStopEditing();
			this.controller.OnPressBack();
			this.archetypeDropDown.scrollRect.gameObject.SetActive(false);
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
			return;
		}
		if (this.archetypeDropDown.scrollRect.activeInHierarchy)
		{
			KScrollRect component = this.archetypeDropDown.scrollRect.GetComponent<KScrollRect>();
			Vector2 point = component.rectTransform().InverseTransformPoint(KInputManager.GetMousePos());
			if (component.rectTransform().rect.Contains(point))
			{
				component.mouseIsOver = true;
			}
			else
			{
				component.mouseIsOver = false;
			}
			component.OnKeyDown(e);
			return;
		}
		this.scroll_rect.OnKeyDown(e);
	}

	// Token: 0x06005F9D RID: 24477 RVA: 0x00231438 File Offset: 0x0022F638
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
			return;
		}
		if (this.archetypeDropDown.scrollRect.activeInHierarchy)
		{
			KScrollRect component = this.archetypeDropDown.scrollRect.GetComponent<KScrollRect>();
			Vector2 point = component.rectTransform().InverseTransformPoint(KInputManager.GetMousePos());
			if (component.rectTransform().rect.Contains(point))
			{
				component.mouseIsOver = true;
			}
			else
			{
				component.mouseIsOver = false;
			}
			component.OnKeyUp(e);
			return;
		}
		this.scroll_rect.OnKeyUp(e);
	}

	// Token: 0x06005F9E RID: 24478 RVA: 0x002314C7 File Offset: 0x0022F6C7
	protected override void OnCmpEnable()
	{
		base.OnActivate();
		if (this.stats == null)
		{
			return;
		}
		this.SetAnimator();
	}

	// Token: 0x06005F9F RID: 24479 RVA: 0x002314DE File Offset: 0x0022F6DE
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.characterNameTitle.ForceStopEditing();
	}

	// Token: 0x06005FA0 RID: 24480 RVA: 0x002314F4 File Offset: 0x0022F6F4
	private void OnArchetypeEntryClick(IListableOption skill, object data)
	{
		if (skill != null)
		{
			SkillGroup skillGroup = skill as SkillGroup;
			this.guaranteedAptitudeID = skillGroup.Id;
			this.selectedArchetypeIcon.sprite = Assets.GetSprite(skillGroup.archetypeIcon);
			this.Reshuffle(true);
			return;
		}
		this.guaranteedAptitudeID = null;
		this.selectedArchetypeIcon.sprite = this.dropdownArrowIcon;
		this.Reshuffle(true);
	}

	// Token: 0x06005FA1 RID: 24481 RVA: 0x00231559 File Offset: 0x0022F759
	private int archetypeDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		if (b.Equals("Random"))
		{
			return -1;
		}
		return b.GetProperName().CompareTo(a.GetProperName());
	}

	// Token: 0x06005FA2 RID: 24482 RVA: 0x0023157C File Offset: 0x0022F77C
	private void archetypeDropEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		if (entry.entryData != null)
		{
			SkillGroup skillGroup = entry.entryData as SkillGroup;
			entry.image.sprite = Assets.GetSprite(skillGroup.archetypeIcon);
		}
	}

	// Token: 0x06005FA3 RID: 24483 RVA: 0x002315B8 File Offset: 0x0022F7B8
	private void OnModelEntryClick(IListableOption listItem, object data)
	{
		bool flag = false;
		if (listItem == null)
		{
			this.permittedModels = this.allMinionModels;
			this.selectedModelIcon.sprite = Assets.GetSprite(this.allModelSprite);
			this.Reshuffle(true);
		}
		else
		{
			CharacterContainer.MinionModelOption minionModelOption = listItem as CharacterContainer.MinionModelOption;
			if (minionModelOption != null)
			{
				flag = (minionModelOption.permittedModels.Count == 1 && minionModelOption.permittedModels[0] == GameTags.Minions.Models.Bionic);
				this.permittedModels = minionModelOption.permittedModels;
				this.selectedModelIcon.sprite = minionModelOption.sprite;
				this.Reshuffle(true);
			}
		}
		this.reshuffleButton.soundPlayer.widget_sound_events()[0].OverrideAssetName = (flag ? "DupeShuffle_bionic" : "DupeShuffle");
	}

	// Token: 0x06005FA4 RID: 24484 RVA: 0x0023167A File Offset: 0x0022F87A
	private int modelDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		return a.GetProperName().CompareTo(b.GetProperName());
	}

	// Token: 0x06005FA5 RID: 24485 RVA: 0x00231690 File Offset: 0x0022F890
	private void modelDropEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		if (entry.entryData != null)
		{
			CharacterContainer.MinionModelOption minionModelOption = entry.entryData as CharacterContainer.MinionModelOption;
			entry.image.sprite = minionModelOption.sprite;
		}
	}

	// Token: 0x04003FAD RID: 16301
	public const string SHUFFLE_BUTTON_DEFAULT_SOUND_NAME_ON_USE = "DupeShuffle";

	// Token: 0x04003FAE RID: 16302
	public const string SHUFFLE_BUTTON_BIONIC_SOUND_NAME_ON_USE = "DupeShuffle_bionic";

	// Token: 0x04003FAF RID: 16303
	private static readonly Dictionary<int, string> defaultShirtIdxToDefaultOutfitID = new Dictionary<int, string>
	{
		{
			1,
			"StandardRed"
		},
		{
			2,
			"StandardBlue"
		},
		{
			3,
			"StandardYellow"
		},
		{
			4,
			"StandardGreen"
		},
		{
			5,
			"permit_standard_bionic_outfit"
		},
		{
			414842661,
			"permit_standard_regal_neutronium_outfit"
		}
	};

	// Token: 0x04003FB0 RID: 16304
	[SerializeField]
	private GameObject contentBody;

	// Token: 0x04003FB1 RID: 16305
	[SerializeField]
	private LocText characterName;

	// Token: 0x04003FB2 RID: 16306
	[SerializeField]
	private EditableTitleBar characterNameTitle;

	// Token: 0x04003FB3 RID: 16307
	[SerializeField]
	private LocText characterJob;

	// Token: 0x04003FB4 RID: 16308
	[SerializeField]
	private LocText traitHeaderLabel;

	// Token: 0x04003FB5 RID: 16309
	public GameObject selectedBorder;

	// Token: 0x04003FB6 RID: 16310
	[SerializeField]
	private Image titleBar;

	// Token: 0x04003FB7 RID: 16311
	[SerializeField]
	private Color selectedTitleColor;

	// Token: 0x04003FB8 RID: 16312
	[SerializeField]
	private Color deselectedTitleColor;

	// Token: 0x04003FB9 RID: 16313
	[SerializeField]
	private KButton reshuffleButton;

	// Token: 0x04003FBA RID: 16314
	private KBatchedAnimController animController;

	// Token: 0x04003FBB RID: 16315
	[SerializeField]
	private KBatchedAnimController bgAnimController;

	// Token: 0x04003FBC RID: 16316
	[SerializeField]
	private GameObject iconGroup;

	// Token: 0x04003FBD RID: 16317
	private List<GameObject> iconGroups;

	// Token: 0x04003FBE RID: 16318
	[SerializeField]
	private LocText goodTrait;

	// Token: 0x04003FBF RID: 16319
	[SerializeField]
	private LocText badTrait;

	// Token: 0x04003FC0 RID: 16320
	[SerializeField]
	private GameObject aptitudeContainer;

	// Token: 0x04003FC1 RID: 16321
	[SerializeField]
	private GameObject aptitudeEntry;

	// Token: 0x04003FC2 RID: 16322
	[SerializeField]
	private Transform aptitudeLabel;

	// Token: 0x04003FC3 RID: 16323
	[SerializeField]
	private Transform attributeLabelAptitude;

	// Token: 0x04003FC4 RID: 16324
	[SerializeField]
	private Transform attributeLabelTrait;

	// Token: 0x04003FC5 RID: 16325
	[SerializeField]
	private LocText expectationRight;

	// Token: 0x04003FC6 RID: 16326
	private List<LocText> expectationLabels;

	// Token: 0x04003FC7 RID: 16327
	[SerializeField]
	private DropDown archetypeDropDown;

	// Token: 0x04003FC8 RID: 16328
	[SerializeField]
	private Image selectedArchetypeIcon;

	// Token: 0x04003FC9 RID: 16329
	[SerializeField]
	private Sprite noArchetypeIcon;

	// Token: 0x04003FCA RID: 16330
	[SerializeField]
	private Sprite dropdownArrowIcon;

	// Token: 0x04003FCB RID: 16331
	private string guaranteedAptitudeID;

	// Token: 0x04003FCC RID: 16332
	private List<GameObject> aptitudeEntries;

	// Token: 0x04003FCD RID: 16333
	private List<GameObject> traitEntries;

	// Token: 0x04003FCE RID: 16334
	[SerializeField]
	private LocText description;

	// Token: 0x04003FCF RID: 16335
	[SerializeField]
	private Image selectedModelIcon;

	// Token: 0x04003FD0 RID: 16336
	[SerializeField]
	private DropDown modelDropDown;

	// Token: 0x04003FD1 RID: 16337
	[SerializeField]
	private HierarchyReferences outfitSelectorReferences;

	// Token: 0x04003FD2 RID: 16338
	private List<Tag> permittedModels = new List<Tag>
	{
		GameTags.Minions.Models.Standard,
		GameTags.Minions.Models.Bionic
	};

	// Token: 0x04003FD3 RID: 16339
	[SerializeField]
	private KToggle selectButton;

	// Token: 0x04003FD4 RID: 16340
	[SerializeField]
	private KBatchedAnimController fxAnim;

	// Token: 0x04003FD5 RID: 16341
	private string allModelSprite = "ui_duplicant_any_selection";

	// Token: 0x04003FD6 RID: 16342
	private static Dictionary<Tag, string> portraitBGAnims = new Dictionary<Tag, string>
	{
		{
			GameTags.Minions.Models.Standard,
			"crewselect_backdrop_kanim"
		},
		{
			GameTags.Minions.Models.Bionic,
			"updated_crewSelect_bionic_backdrop_kanim"
		}
	};

	// Token: 0x04003FD7 RID: 16343
	private MinionStartingStats stats;

	// Token: 0x04003FD8 RID: 16344
	private CharacterSelectionController controller;

	// Token: 0x04003FD9 RID: 16345
	private static List<CharacterContainer> containers;

	// Token: 0x04003FDA RID: 16346
	private KAnimFile idle_anim;

	// Token: 0x04003FDB RID: 16347
	[HideInInspector]
	public bool addMinionToIdentityList = true;

	// Token: 0x04003FDC RID: 16348
	[SerializeField]
	private Sprite enabledSpr;

	// Token: 0x04003FDD RID: 16349
	[SerializeField]
	private KScrollRect scroll_rect;

	// Token: 0x04003FDE RID: 16350
	private static readonly Dictionary<HashedString, string[]> traitIdleAnims = new Dictionary<HashedString, string[]>
	{
		{
			"anim_idle_food_kanim",
			new string[]
			{
				"Foodie"
			}
		},
		{
			"anim_idle_animal_lover_kanim",
			new string[]
			{
				"RanchingUp"
			}
		},
		{
			"anim_idle_loner_kanim",
			new string[]
			{
				"Loner"
			}
		},
		{
			"anim_idle_mole_hands_kanim",
			new string[]
			{
				"MoleHands"
			}
		},
		{
			"anim_idle_buff_kanim",
			new string[]
			{
				"StrongArm"
			}
		},
		{
			"anim_idle_distracted_kanim",
			new string[]
			{
				"CantResearch",
				"CantBuild",
				"CantCook",
				"CantDig"
			}
		},
		{
			"anim_idle_coaster_kanim",
			new string[]
			{
				"HappySinger"
			}
		}
	};

	// Token: 0x04003FDF RID: 16351
	private List<Tag> allMinionModels = new List<Tag>
	{
		GameTags.Minions.Models.Standard,
		GameTags.Minions.Models.Bionic
	};

	// Token: 0x04003FE0 RID: 16352
	private static readonly HashedString[] idleAnims = new HashedString[]
	{
		"anim_idle_healthy_kanim",
		"anim_idle_susceptible_kanim",
		"anim_idle_keener_kanim",
		"anim_idle_fastfeet_kanim",
		"anim_idle_breatherdeep_kanim",
		"anim_idle_breathershallow_kanim"
	};

	// Token: 0x04003FE1 RID: 16353
	public float baseCharacterScale = 0.38f;

	// Token: 0x04003FE2 RID: 16354
	private List<Option<ClothingOutfitTarget>> allAvailableClothingOutfits;

	// Token: 0x04003FE3 RID: 16355
	private int outfitSelectorIndex;

	// Token: 0x04003FE4 RID: 16356
	private bool outfitSelectorExpanded;

	// Token: 0x02001DF4 RID: 7668
	[Serializable]
	public struct ProfessionIcon
	{
		// Token: 0x04008CBE RID: 36030
		public string professionName;

		// Token: 0x04008CBF RID: 36031
		public Sprite iconImg;
	}

	// Token: 0x02001DF5 RID: 7669
	private class MinionModelOption : IListableOption
	{
		// Token: 0x0600B29F RID: 45727 RVA: 0x003E092D File Offset: 0x003DEB2D
		public MinionModelOption(string name, List<Tag> permittedModels, Sprite sprite)
		{
			this.properName = name;
			this.permittedModels = permittedModels;
			this.sprite = sprite;
		}

		// Token: 0x0600B2A0 RID: 45728 RVA: 0x003E094A File Offset: 0x003DEB4A
		public string GetProperName()
		{
			return this.properName;
		}

		// Token: 0x04008CC0 RID: 36032
		private string properName;

		// Token: 0x04008CC1 RID: 36033
		public List<Tag> permittedModels;

		// Token: 0x04008CC2 RID: 36034
		public Sprite sprite;
	}
}
