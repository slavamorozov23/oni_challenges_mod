using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000DF3 RID: 3571
public class ResourceCategoryScreen : KScreen
{
	// Token: 0x060070B6 RID: 28854 RVA: 0x002AEE20 File Offset: 0x002AD020
	public static void DestroyInstance()
	{
		ResourceCategoryScreen.Instance = null;
	}

	// Token: 0x060070B7 RID: 28855 RVA: 0x002AEE28 File Offset: 0x002AD028
	protected override void OnActivate()
	{
		base.OnActivate();
		ResourceCategoryScreen.Instance = this;
		base.ConsumeMouseScroll = true;
		MultiToggle hiderButton = this.HiderButton;
		hiderButton.onClick = (System.Action)Delegate.Combine(hiderButton.onClick, new System.Action(this.OnHiderClick));
		this.OnHiderClick();
		this.CreateTagSetHeaders(GameTags.MaterialCategories, GameUtil.MeasureUnit.mass);
		this.CreateTagSetHeaders(GameTags.CalorieCategories, GameUtil.MeasureUnit.kcal);
		this.CreateTagSetHeaders(GameTags.UnitCategories, GameUtil.MeasureUnit.quantity);
		if (!this.DisplayedCategories.ContainsKey(GameTags.Miscellaneous))
		{
			ResourceCategoryHeader value = this.NewCategoryHeader(GameTags.Miscellaneous, GameUtil.MeasureUnit.mass);
			this.DisplayedCategories.Add(GameTags.Miscellaneous, value);
		}
		this.DisplayedCategoryKeys = this.DisplayedCategories.Keys.ToArray<Tag>();
	}

	// Token: 0x060070B8 RID: 28856 RVA: 0x002AEEE0 File Offset: 0x002AD0E0
	private void CreateTagSetHeaders(IEnumerable<Tag> set, GameUtil.MeasureUnit measure)
	{
		foreach (Tag tag in set)
		{
			ResourceCategoryHeader value = this.NewCategoryHeader(tag, measure);
			this.DisplayedCategories.Add(tag, value);
		}
	}

	// Token: 0x060070B9 RID: 28857 RVA: 0x002AEF38 File Offset: 0x002AD138
	private void OnHiderClick()
	{
		this.HiderButton.NextState();
		if (this.HiderButton.CurrentState == 0)
		{
			this.targetContentHideHeight = 0f;
			return;
		}
		this.targetContentHideHeight = Mathf.Min(((float)Screen.height - this.maxHeightPadding) / GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>().GetCanvasScale(), this.CategoryContainer.rectTransform().rect.height);
	}

	// Token: 0x060070BA RID: 28858 RVA: 0x002AEFB0 File Offset: 0x002AD1B0
	private void Update()
	{
		if (ClusterManager.Instance.activeWorld.worldInventory == null)
		{
			return;
		}
		if (this.HideTarget.minHeight != this.targetContentHideHeight)
		{
			float num = this.HideTarget.minHeight;
			float num2 = this.targetContentHideHeight - num;
			num2 = Mathf.Clamp(num2 * this.HideSpeedFactor * Time.unscaledDeltaTime, (num2 > 0f) ? (-num2) : num2, (num2 > 0f) ? num2 : (-num2));
			num += num2;
			this.HideTarget.minHeight = num;
		}
		for (int i = 0; i < 1; i++)
		{
			Tag tag = this.DisplayedCategoryKeys[this.categoryUpdatePacer];
			ResourceCategoryHeader resourceCategoryHeader = this.DisplayedCategories[tag];
			if (DiscoveredResources.Instance.IsDiscovered(tag) && !resourceCategoryHeader.gameObject.activeInHierarchy)
			{
				resourceCategoryHeader.gameObject.SetActive(true);
			}
			resourceCategoryHeader.UpdateContents();
			this.categoryUpdatePacer = (this.categoryUpdatePacer + 1) % this.DisplayedCategoryKeys.Length;
		}
		if (this.HiderButton.CurrentState != 0)
		{
			this.targetContentHideHeight = Mathf.Min(((float)Screen.height - this.maxHeightPadding) / GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>().GetCanvasScale(), this.CategoryContainer.rectTransform().rect.height);
		}
		if (MeterScreen.Instance != null && !MeterScreen.Instance.StartValuesSet)
		{
			MeterScreen.Instance.InitializeValues();
		}
	}

	// Token: 0x060070BB RID: 28859 RVA: 0x002AF123 File Offset: 0x002AD323
	private ResourceCategoryHeader NewCategoryHeader(Tag categoryTag, GameUtil.MeasureUnit measure)
	{
		GameObject gameObject = Util.KInstantiateUI(this.Prefab_CategoryBar, this.CategoryContainer.gameObject, false);
		gameObject.name = "CategoryHeader_" + categoryTag.Name;
		ResourceCategoryHeader component = gameObject.GetComponent<ResourceCategoryHeader>();
		component.SetTag(categoryTag, measure);
		return component;
	}

	// Token: 0x060070BC RID: 28860 RVA: 0x002AF160 File Offset: 0x002AD360
	public static string QuantityTextForMeasure(float quantity, GameUtil.MeasureUnit measure)
	{
		switch (measure)
		{
		case GameUtil.MeasureUnit.mass:
			return GameUtil.GetFormattedMass(quantity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		case GameUtil.MeasureUnit.kcal:
			return GameUtil.GetFormattedCalories(quantity, GameUtil.TimeSlice.None, true);
		}
		return quantity.ToString();
	}

	// Token: 0x04004DAD RID: 19885
	public static ResourceCategoryScreen Instance;

	// Token: 0x04004DAE RID: 19886
	public GameObject Prefab_CategoryBar;

	// Token: 0x04004DAF RID: 19887
	public Transform CategoryContainer;

	// Token: 0x04004DB0 RID: 19888
	public MultiToggle HiderButton;

	// Token: 0x04004DB1 RID: 19889
	public KLayoutElement HideTarget;

	// Token: 0x04004DB2 RID: 19890
	private float HideSpeedFactor = 12f;

	// Token: 0x04004DB3 RID: 19891
	private float maxHeightPadding = 480f;

	// Token: 0x04004DB4 RID: 19892
	private float targetContentHideHeight;

	// Token: 0x04004DB5 RID: 19893
	public Dictionary<Tag, ResourceCategoryHeader> DisplayedCategories = new Dictionary<Tag, ResourceCategoryHeader>();

	// Token: 0x04004DB6 RID: 19894
	private Tag[] DisplayedCategoryKeys;

	// Token: 0x04004DB7 RID: 19895
	private int categoryUpdatePacer;
}
