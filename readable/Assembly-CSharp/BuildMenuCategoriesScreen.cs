using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C49 RID: 3145
public class BuildMenuCategoriesScreen : KIconToggleMenu
{
	// Token: 0x06005F38 RID: 24376 RVA: 0x0022D40F File Offset: 0x0022B60F
	public override float GetSortKey()
	{
		return 7f;
	}

	// Token: 0x170006FE RID: 1790
	// (get) Token: 0x06005F39 RID: 24377 RVA: 0x0022D416 File Offset: 0x0022B616
	public HashedString Category
	{
		get
		{
			return this.category;
		}
	}

	// Token: 0x06005F3A RID: 24378 RVA: 0x0022D41E File Offset: 0x0022B61E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.onSelect += this.OnClickCategory;
	}

	// Token: 0x06005F3B RID: 24379 RVA: 0x0022D438 File Offset: 0x0022B638
	public void Configure(HashedString category, int depth, object data, Dictionary<HashedString, List<BuildingDef>> categorized_building_map, Dictionary<HashedString, List<HashedString>> categorized_category_map, BuildMenuBuildingsScreen buildings_screen)
	{
		this.category = category;
		this.categorizedBuildingMap = categorized_building_map;
		this.categorizedCategoryMap = categorized_category_map;
		this.buildingsScreen = buildings_screen;
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		if (typeof(IList<BuildMenu.BuildingInfo>).IsAssignableFrom(data.GetType()))
		{
			this.buildingInfos = (IList<BuildMenu.BuildingInfo>)data;
		}
		else if (typeof(IList<BuildMenu.DisplayInfo>).IsAssignableFrom(data.GetType()))
		{
			this.subcategories = new List<HashedString>();
			foreach (BuildMenu.DisplayInfo displayInfo in ((IList<BuildMenu.DisplayInfo>)data))
			{
				string iconName = displayInfo.iconName;
				string text = HashCache.Get().Get(displayInfo.category).ToUpper();
				text = text.Replace(" ", "");
				KIconToggleMenu.ToggleInfo item = new KIconToggleMenu.ToggleInfo(Strings.Get("STRINGS.UI.NEWBUILDCATEGORIES." + text + ".NAME"), iconName, new BuildMenuCategoriesScreen.UserData
				{
					category = displayInfo.category,
					depth = depth,
					requirementsState = PlanScreen.RequirementsState.Tech
				}, displayInfo.hotkey, Strings.Get("STRINGS.UI.NEWBUILDCATEGORIES." + text + ".TOOLTIP"), "");
				list.Add(item);
				this.subcategories.Add(displayInfo.category);
			}
			base.Setup(list);
			this.toggles.ForEach(delegate(KToggle to)
			{
				foreach (ImageToggleState imageToggleState in to.GetComponents<ImageToggleState>())
				{
					if (imageToggleState.TargetImage.sprite != null && imageToggleState.TargetImage.name == "FG" && !imageToggleState.useSprites)
					{
						imageToggleState.SetSprites(Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"), imageToggleState.TargetImage.sprite, imageToggleState.TargetImage.sprite, Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"));
					}
				}
				to.GetComponent<KToggle>().soundPlayer.Enabled = false;
			});
		}
		this.UpdateBuildableStates(true);
	}

	// Token: 0x06005F3C RID: 24380 RVA: 0x0022D5E0 File Offset: 0x0022B7E0
	private void OnClickCategory(KIconToggleMenu.ToggleInfo toggle_info)
	{
		BuildMenuCategoriesScreen.UserData userData = (BuildMenuCategoriesScreen.UserData)toggle_info.userData;
		PlanScreen.RequirementsState requirementsState = userData.requirementsState;
		if (requirementsState - PlanScreen.RequirementsState.Materials <= 1)
		{
			if (this.selectedCategory != userData.category)
			{
				this.selectedCategory = userData.category;
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
			}
			else
			{
				this.selectedCategory = HashedString.Invalid;
				this.ClearSelection();
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
			}
		}
		else
		{
			this.selectedCategory = HashedString.Invalid;
			this.ClearSelection();
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		toggle_info.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
		if (this.onCategoryClicked != null)
		{
			this.onCategoryClicked(this.selectedCategory, userData.depth);
		}
	}

	// Token: 0x06005F3D RID: 24381 RVA: 0x0022D6AC File Offset: 0x0022B8AC
	private void UpdateButtonStates()
	{
		if (this.toggleInfo != null && this.toggleInfo.Count > 0)
		{
			foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
			{
				BuildMenuCategoriesScreen.UserData userData = (BuildMenuCategoriesScreen.UserData)toggleInfo.userData;
				HashedString x = userData.category;
				PlanScreen.RequirementsState categoryRequirements = this.GetCategoryRequirements(x);
				bool flag = categoryRequirements == PlanScreen.RequirementsState.Tech;
				toggleInfo.toggle.gameObject.SetActive(!flag);
				if (categoryRequirements != PlanScreen.RequirementsState.Materials)
				{
					if (categoryRequirements == PlanScreen.RequirementsState.Complete)
					{
						ImageToggleState.State state = (!this.selectedCategory.IsValid || x != this.selectedCategory) ? ImageToggleState.State.Inactive : ImageToggleState.State.Active;
						if (userData.currentToggleState == null || userData.currentToggleState.GetValueOrDefault() != state)
						{
							userData.currentToggleState = new ImageToggleState.State?(state);
							this.SetImageToggleState(toggleInfo.toggle.gameObject, state);
						}
					}
				}
				else
				{
					toggleInfo.toggle.fgImage.SetAlpha(flag ? 0.2509804f : 1f);
					ImageToggleState.State state2 = (this.selectedCategory.IsValid && x == this.selectedCategory) ? ImageToggleState.State.DisabledActive : ImageToggleState.State.Disabled;
					if (userData.currentToggleState == null || userData.currentToggleState.GetValueOrDefault() != state2)
					{
						userData.currentToggleState = new ImageToggleState.State?(state2);
						this.SetImageToggleState(toggleInfo.toggle.gameObject, state2);
					}
				}
				toggleInfo.toggle.fgImage.transform.Find("ResearchIcon").gameObject.gameObject.SetActive(flag);
			}
		}
	}

	// Token: 0x06005F3E RID: 24382 RVA: 0x0022D870 File Offset: 0x0022BA70
	private void SetImageToggleState(GameObject target, ImageToggleState.State state)
	{
		ImageToggleState[] components = target.GetComponents<ImageToggleState>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].SetState(state);
		}
	}

	// Token: 0x06005F3F RID: 24383 RVA: 0x0022D89C File Offset: 0x0022BA9C
	private PlanScreen.RequirementsState GetCategoryRequirements(HashedString category)
	{
		bool flag = true;
		bool flag2 = true;
		List<BuildingDef> list;
		if (this.categorizedBuildingMap.TryGetValue(category, out list))
		{
			if (list.Count <= 0)
			{
				goto IL_F3;
			}
			using (List<BuildingDef>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BuildingDef buildingDef = enumerator.Current;
					if (buildingDef.ShowInBuildMenu && buildingDef.IsAvailable())
					{
						PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(buildingDef);
						flag = (flag && requirementsState == PlanScreen.RequirementsState.Tech);
						flag2 = (flag2 && (requirementsState == PlanScreen.RequirementsState.Materials || requirementsState == PlanScreen.RequirementsState.Tech));
					}
				}
				goto IL_F3;
			}
		}
		List<HashedString> list2;
		if (this.categorizedCategoryMap.TryGetValue(category, out list2))
		{
			foreach (HashedString hashedString in list2)
			{
				PlanScreen.RequirementsState categoryRequirements = this.GetCategoryRequirements(hashedString);
				flag = (flag && categoryRequirements == PlanScreen.RequirementsState.Tech);
				flag2 = (flag2 && (categoryRequirements == PlanScreen.RequirementsState.Materials || categoryRequirements == PlanScreen.RequirementsState.Tech));
			}
		}
		IL_F3:
		PlanScreen.RequirementsState result;
		if (flag)
		{
			result = PlanScreen.RequirementsState.Tech;
		}
		else if (flag2)
		{
			result = PlanScreen.RequirementsState.Materials;
		}
		else
		{
			result = PlanScreen.RequirementsState.Complete;
		}
		if (DebugHandler.InstantBuildMode)
		{
			result = PlanScreen.RequirementsState.Complete;
		}
		return result;
	}

	// Token: 0x06005F40 RID: 24384 RVA: 0x0022D9D4 File Offset: 0x0022BBD4
	public void UpdateNotifications(ICollection<HashedString> updated_categories)
	{
		if (this.toggleInfo == null)
		{
			return;
		}
		this.UpdateBuildableStates(false);
		foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
		{
			HashedString item = ((BuildMenuCategoriesScreen.UserData)toggleInfo.userData).category;
			if (updated_categories.Contains(item))
			{
				toggleInfo.toggle.gameObject.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
			}
		}
	}

	// Token: 0x06005F41 RID: 24385 RVA: 0x0022DA5C File Offset: 0x0022BC5C
	public override void Close()
	{
		base.Close();
		this.selectedCategory = HashedString.Invalid;
		this.SetHasFocus(false);
		if (this.buildingInfos != null)
		{
			this.buildingsScreen.Close();
		}
	}

	// Token: 0x06005F42 RID: 24386 RVA: 0x0022DA89 File Offset: 0x0022BC89
	[ContextMenu("ForceUpdateBuildableStates")]
	private void ForceUpdateBuildableStates()
	{
		this.UpdateBuildableStates(true);
	}

	// Token: 0x06005F43 RID: 24387 RVA: 0x0022DA94 File Offset: 0x0022BC94
	public void UpdateBuildableStates(bool skip_flourish)
	{
		if (this.subcategories != null && this.subcategories.Count > 0)
		{
			this.UpdateButtonStates();
			using (IEnumerator<KIconToggleMenu.ToggleInfo> enumerator = this.toggleInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KIconToggleMenu.ToggleInfo toggleInfo = enumerator.Current;
					BuildMenuCategoriesScreen.UserData userData = (BuildMenuCategoriesScreen.UserData)toggleInfo.userData;
					HashedString hashedString = userData.category;
					PlanScreen.RequirementsState categoryRequirements = this.GetCategoryRequirements(hashedString);
					if (userData.requirementsState != categoryRequirements)
					{
						userData.requirementsState = categoryRequirements;
						toggleInfo.userData = userData;
						if (!skip_flourish)
						{
							toggleInfo.toggle.ActivateFlourish(false);
							string text = "NotificationPing";
							if (!toggleInfo.toggle.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag(text))
							{
								toggleInfo.toggle.gameObject.GetComponent<Animator>().Play(text);
								BuildMenu.Instance.PlayNewBuildingSounds();
							}
						}
					}
				}
				return;
			}
		}
		this.buildingsScreen.UpdateBuildableStates();
	}

	// Token: 0x06005F44 RID: 24388 RVA: 0x0022DB98 File Offset: 0x0022BD98
	protected override void OnShow(bool show)
	{
		if (this.buildingInfos != null)
		{
			if (show)
			{
				this.buildingsScreen.Configure(this.category, this.buildingInfos);
				this.buildingsScreen.Show(true);
			}
			else
			{
				this.buildingsScreen.Close();
			}
		}
		base.OnShow(show);
	}

	// Token: 0x06005F45 RID: 24389 RVA: 0x0022DBE8 File Offset: 0x0022BDE8
	public override void ClearSelection()
	{
		this.selectedCategory = HashedString.Invalid;
		base.ClearSelection();
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.isOn = false;
		}
	}

	// Token: 0x06005F46 RID: 24390 RVA: 0x0022DC4C File Offset: 0x0022BE4C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.modalKeyInputBehaviour)
		{
			if (this.HasFocus)
			{
				if (e.TryConsume(global::Action.Escape))
				{
					Game.Instance.Trigger(288942073, null);
					return;
				}
				base.OnKeyDown(e);
				if (!e.Consumed)
				{
					global::Action action = e.GetAction();
					if (action >= global::Action.BUILD_MENU_START_INTERCEPT)
					{
						e.TryConsume(action);
						return;
					}
				}
			}
		}
		else
		{
			base.OnKeyDown(e);
			if (e.Consumed)
			{
				this.UpdateButtonStates();
			}
		}
	}

	// Token: 0x06005F47 RID: 24391 RVA: 0x0022DCBC File Offset: 0x0022BEBC
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.modalKeyInputBehaviour)
		{
			if (this.HasFocus)
			{
				if (e.TryConsume(global::Action.Escape))
				{
					Game.Instance.Trigger(288942073, null);
					return;
				}
				base.OnKeyUp(e);
				if (!e.Consumed)
				{
					global::Action action = e.GetAction();
					if (action >= global::Action.BUILD_MENU_START_INTERCEPT)
					{
						e.TryConsume(action);
						return;
					}
				}
			}
		}
		else
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06005F48 RID: 24392 RVA: 0x0022DD1E File Offset: 0x0022BF1E
	public override void SetHasFocus(bool has_focus)
	{
		base.SetHasFocus(has_focus);
		if (this.focusIndicator != null)
		{
			this.focusIndicator.color = (has_focus ? this.focusedColour : this.unfocusedColour);
		}
	}

	// Token: 0x04003F88 RID: 16264
	public Action<HashedString, int> onCategoryClicked;

	// Token: 0x04003F89 RID: 16265
	[SerializeField]
	public bool modalKeyInputBehaviour;

	// Token: 0x04003F8A RID: 16266
	[SerializeField]
	private Image focusIndicator;

	// Token: 0x04003F8B RID: 16267
	[SerializeField]
	private Color32 focusedColour;

	// Token: 0x04003F8C RID: 16268
	[SerializeField]
	private Color32 unfocusedColour;

	// Token: 0x04003F8D RID: 16269
	private IList<HashedString> subcategories;

	// Token: 0x04003F8E RID: 16270
	private Dictionary<HashedString, List<BuildingDef>> categorizedBuildingMap;

	// Token: 0x04003F8F RID: 16271
	private Dictionary<HashedString, List<HashedString>> categorizedCategoryMap;

	// Token: 0x04003F90 RID: 16272
	private BuildMenuBuildingsScreen buildingsScreen;

	// Token: 0x04003F91 RID: 16273
	private HashedString category;

	// Token: 0x04003F92 RID: 16274
	private IList<BuildMenu.BuildingInfo> buildingInfos;

	// Token: 0x04003F93 RID: 16275
	private HashedString selectedCategory = HashedString.Invalid;

	// Token: 0x02001DEE RID: 7662
	private class UserData
	{
		// Token: 0x04008CAF RID: 36015
		public HashedString category;

		// Token: 0x04008CB0 RID: 36016
		public int depth;

		// Token: 0x04008CB1 RID: 36017
		public PlanScreen.RequirementsState requirementsState;

		// Token: 0x04008CB2 RID: 36018
		public ImageToggleState.State? currentToggleState;
	}
}
