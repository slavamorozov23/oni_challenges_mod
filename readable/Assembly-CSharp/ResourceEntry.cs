using System;
using System.Collections;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000DF4 RID: 3572
[AddComponentMenu("KMonoBehaviour/scripts/ResourceEntry")]
public class ResourceEntry : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISim4000ms
{
	// Token: 0x060070BE RID: 28862 RVA: 0x002AF1C0 File Offset: 0x002AD3C0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.QuantityLabel.color = this.AvailableColor;
		this.NameLabel.color = this.AvailableColor;
		this.button.onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x060070BF RID: 28863 RVA: 0x002AF211 File Offset: 0x002AD411
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tooltip.OnToolTip = new Func<string>(this.OnToolTip);
		this.RefreshChart();
	}

	// Token: 0x060070C0 RID: 28864 RVA: 0x002AF238 File Offset: 0x002AD438
	private void OnClick()
	{
		this.lastClickTime = Time.unscaledTime;
		if (this.cachedPickupables == null)
		{
			this.cachedPickupables = ClusterManager.Instance.activeWorld.worldInventory.CreatePickupablesList(this.Resource);
			base.StartCoroutine(this.ClearCachedPickupablesAfterThreshold());
		}
		if (this.cachedPickupables == null)
		{
			return;
		}
		Pickupable pickupable = null;
		for (int i = 0; i < this.cachedPickupables.Count; i++)
		{
			this.selectionIdx++;
			int index = this.selectionIdx % this.cachedPickupables.Count;
			pickupable = this.cachedPickupables[index];
			if (pickupable != null && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
			{
				break;
			}
		}
		if (pickupable != null)
		{
			Transform transform = pickupable.transform;
			if (pickupable.storage != null)
			{
				transform = pickupable.storage.transform;
			}
			SelectTool.Instance.SelectAndFocus(transform.transform.GetPosition(), transform.GetComponent<KSelectable>(), Vector3.zero);
			for (int j = 0; j < this.cachedPickupables.Count; j++)
			{
				Pickupable pickupable2 = this.cachedPickupables[j];
				if (pickupable2 != null)
				{
					KAnimControllerBase component = pickupable2.GetComponent<KAnimControllerBase>();
					if (component != null)
					{
						component.HighlightColour = this.HighlightColor;
					}
				}
			}
		}
	}

	// Token: 0x060070C1 RID: 28865 RVA: 0x002AF394 File Offset: 0x002AD594
	private IEnumerator ClearCachedPickupablesAfterThreshold()
	{
		while (this.cachedPickupables != null && this.lastClickTime != 0f && Time.unscaledTime - this.lastClickTime < 10f)
		{
			yield return SequenceUtil.WaitForSeconds(1f);
		}
		this.cachedPickupables = null;
		yield break;
	}

	// Token: 0x060070C2 RID: 28866 RVA: 0x002AF3A4 File Offset: 0x002AD5A4
	public void GetAmounts(EdiblesManager.FoodInfo food_info, bool doExtras, out float available, out float total, out float reserved)
	{
		available = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(this.Resource, false);
		total = (doExtras ? ClusterManager.Instance.activeWorld.worldInventory.GetTotalAmount(this.Resource, false) : 0f);
		reserved = (doExtras ? MaterialNeeds.GetAmount(this.Resource, ClusterManager.Instance.activeWorldId, false) : 0f);
		if (food_info != null)
		{
			available *= food_info.CaloriesPerUnit;
			total *= food_info.CaloriesPerUnit;
			reserved *= food_info.CaloriesPerUnit;
		}
	}

	// Token: 0x060070C3 RID: 28867 RVA: 0x002AF444 File Offset: 0x002AD644
	private void GetAmounts(bool doExtras, out float available, out float total, out float reserved)
	{
		EdiblesManager.FoodInfo food_info = (this.Measure == GameUtil.MeasureUnit.kcal) ? EdiblesManager.GetFoodInfo(this.Resource.Name) : null;
		this.GetAmounts(food_info, doExtras, out available, out total, out reserved);
	}

	// Token: 0x060070C4 RID: 28868 RVA: 0x002AF47C File Offset: 0x002AD67C
	public void UpdateValue()
	{
		this.SetName(this.Resource.ProperName());
		bool allowInsufficientMaterialBuild = GenericGameSettings.instance.allowInsufficientMaterialBuild;
		float num;
		float num2;
		float num3;
		this.GetAmounts(allowInsufficientMaterialBuild, out num, out num2, out num3);
		if (this.currentQuantity != num)
		{
			this.currentQuantity = num;
			this.QuantityLabel.text = ResourceCategoryScreen.QuantityTextForMeasure(num, this.Measure);
		}
		Color color = this.AvailableColor;
		if (num3 > num2)
		{
			color = this.OverdrawnColor;
		}
		else if (num == 0f)
		{
			color = this.UnavailableColor;
		}
		if (this.QuantityLabel.color != color)
		{
			this.QuantityLabel.color = color;
		}
		if (this.NameLabel.color != color)
		{
			this.NameLabel.color = color;
		}
	}

	// Token: 0x060070C5 RID: 28869 RVA: 0x002AF544 File Offset: 0x002AD744
	private string OnToolTip()
	{
		float quantity;
		float quantity2;
		float quantity3;
		this.GetAmounts(true, out quantity, out quantity2, out quantity3);
		string text = this.NameLabel.text + "\n";
		text += string.Format(UI.RESOURCESCREEN.AVAILABLE_TOOLTIP, ResourceCategoryScreen.QuantityTextForMeasure(quantity, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity3, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity2, this.Measure));
		float delta = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.Resource).GetDelta(150f);
		if (delta != 0f)
		{
			text = text + "\n\n" + string.Format(UI.RESOURCESCREEN.TREND_TOOLTIP, (delta > 0f) ? UI.RESOURCESCREEN.INCREASING_STR : UI.RESOURCESCREEN.DECREASING_STR, GameUtil.GetFormattedMass(Mathf.Abs(delta), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		else
		{
			text = text + "\n\n" + UI.RESOURCESCREEN.TREND_TOOLTIP_NO_CHANGE;
		}
		return text;
	}

	// Token: 0x060070C6 RID: 28870 RVA: 0x002AF63A File Offset: 0x002AD83A
	public void SetName(string name)
	{
		this.NameLabel.text = name;
	}

	// Token: 0x060070C7 RID: 28871 RVA: 0x002AF648 File Offset: 0x002AD848
	public void SetTag(Tag t, GameUtil.MeasureUnit measure)
	{
		this.Resource = t;
		this.Measure = measure;
		this.cachedPickupables = null;
	}

	// Token: 0x060070C8 RID: 28872 RVA: 0x002AF660 File Offset: 0x002AD860
	private void Hover(bool is_hovering)
	{
		if (ClusterManager.Instance.activeWorld.worldInventory == null)
		{
			return;
		}
		if (is_hovering)
		{
			this.Background.color = this.BackgroundHoverColor;
		}
		else
		{
			this.Background.color = new Color(0f, 0f, 0f, 0f);
		}
		ICollection<Pickupable> pickupables = ClusterManager.Instance.activeWorld.worldInventory.GetPickupables(this.Resource, false);
		if (pickupables == null)
		{
			return;
		}
		foreach (Pickupable pickupable in pickupables)
		{
			if (!(pickupable == null))
			{
				KAnimControllerBase component = pickupable.GetComponent<KAnimControllerBase>();
				if (!(component == null))
				{
					if (is_hovering)
					{
						component.HighlightColour = this.HighlightColor;
					}
					else
					{
						component.HighlightColour = Color.black;
					}
				}
			}
		}
	}

	// Token: 0x060070C9 RID: 28873 RVA: 0x002AF754 File Offset: 0x002AD954
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.Hover(true);
	}

	// Token: 0x060070CA RID: 28874 RVA: 0x002AF75D File Offset: 0x002AD95D
	public void OnPointerExit(PointerEventData eventData)
	{
		this.Hover(false);
	}

	// Token: 0x060070CB RID: 28875 RVA: 0x002AF768 File Offset: 0x002AD968
	public void SetSprite(Tag t)
	{
		Element element = ElementLoader.FindElementByName(this.Resource.Name);
		if (element != null)
		{
			Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(element.substance.anim, "ui", false, "");
			if (uispriteFromMultiObjectAnim != null)
			{
				this.image.sprite = uispriteFromMultiObjectAnim;
			}
		}
	}

	// Token: 0x060070CC RID: 28876 RVA: 0x002AF7BA File Offset: 0x002AD9BA
	public void SetSprite(Sprite sprite)
	{
		this.image.sprite = sprite;
	}

	// Token: 0x060070CD RID: 28877 RVA: 0x002AF7C8 File Offset: 0x002AD9C8
	public void Sim4000ms(float dt)
	{
		this.RefreshChart();
	}

	// Token: 0x060070CE RID: 28878 RVA: 0x002AF7D0 File Offset: 0x002AD9D0
	private void RefreshChart()
	{
		if (this.sparkChart != null)
		{
			ResourceTracker resourceStatistic = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.Resource);
			this.sparkChart.GetComponentInChildren<LineLayer>().RefreshLine(resourceStatistic.ChartableData(3000f), "resourceAmount");
			this.sparkChart.GetComponentInChildren<SparkLayer>().SetColor(Constants.NEUTRAL_COLOR);
		}
	}

	// Token: 0x04004DB8 RID: 19896
	public Tag Resource;

	// Token: 0x04004DB9 RID: 19897
	public GameUtil.MeasureUnit Measure;

	// Token: 0x04004DBA RID: 19898
	public LocText NameLabel;

	// Token: 0x04004DBB RID: 19899
	public LocText QuantityLabel;

	// Token: 0x04004DBC RID: 19900
	public Image image;

	// Token: 0x04004DBD RID: 19901
	[SerializeField]
	private Color AvailableColor;

	// Token: 0x04004DBE RID: 19902
	[SerializeField]
	private Color UnavailableColor;

	// Token: 0x04004DBF RID: 19903
	[SerializeField]
	private Color OverdrawnColor;

	// Token: 0x04004DC0 RID: 19904
	[SerializeField]
	private Color HighlightColor;

	// Token: 0x04004DC1 RID: 19905
	[SerializeField]
	private Color BackgroundHoverColor;

	// Token: 0x04004DC2 RID: 19906
	[SerializeField]
	private Image Background;

	// Token: 0x04004DC3 RID: 19907
	[MyCmpGet]
	private ToolTip tooltip;

	// Token: 0x04004DC4 RID: 19908
	[MyCmpReq]
	private Button button;

	// Token: 0x04004DC5 RID: 19909
	public GameObject sparkChart;

	// Token: 0x04004DC6 RID: 19910
	private const float CLICK_RESET_TIME_THRESHOLD = 10f;

	// Token: 0x04004DC7 RID: 19911
	private int selectionIdx;

	// Token: 0x04004DC8 RID: 19912
	private float lastClickTime;

	// Token: 0x04004DC9 RID: 19913
	private List<Pickupable> cachedPickupables;

	// Token: 0x04004DCA RID: 19914
	private float currentQuantity = float.MinValue;
}
