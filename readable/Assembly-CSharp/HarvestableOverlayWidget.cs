using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D21 RID: 3361
[AddComponentMenu("KMonoBehaviour/scripts/HarvestableOverlayWidget")]
public class HarvestableOverlayWidget : KMonoBehaviour
{
	// Token: 0x060067FE RID: 26622 RVA: 0x0027430C File Offset: 0x0027250C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.condition_sprites.Add(WiltCondition.Condition.AtmosphereElement, this.sprite_atmosphere);
		this.condition_sprites.Add(WiltCondition.Condition.Darkness, this.sprite_light);
		this.condition_sprites.Add(WiltCondition.Condition.Drowning, this.sprite_liquid);
		this.condition_sprites.Add(WiltCondition.Condition.DryingOut, this.sprite_liquid);
		this.condition_sprites.Add(WiltCondition.Condition.Fertilized, this.sprite_resource);
		this.condition_sprites.Add(WiltCondition.Condition.IlluminationComfort, this.sprite_light);
		this.condition_sprites.Add(WiltCondition.Condition.Irrigation, this.sprite_liquid);
		this.condition_sprites.Add(WiltCondition.Condition.Pressure, this.sprite_pressure);
		this.condition_sprites.Add(WiltCondition.Condition.Temperature, this.sprite_temperature);
		this.condition_sprites.Add(WiltCondition.Condition.Receptacle, this.sprite_receptacle);
		for (int i = 0; i < this.horizontal_containers.Length; i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.horizontal_container_prefab, this.vertical_container, false);
			this.horizontal_containers[i] = gameObject;
		}
		for (int j = 0; j < 14; j++)
		{
			if (this.condition_sprites.ContainsKey((WiltCondition.Condition)j))
			{
				GameObject gameObject2 = Util.KInstantiateUI(this.icon_gameobject_prefab, base.gameObject, false);
				gameObject2.GetComponent<Image>().sprite = this.condition_sprites[(WiltCondition.Condition)j];
				this.condition_icons.Add((WiltCondition.Condition)j, gameObject2);
			}
		}
	}

	// Token: 0x060067FF RID: 26623 RVA: 0x00274458 File Offset: 0x00272658
	public void Refresh(HarvestDesignatable target_harvestable)
	{
		Image image = this.bar.GetComponent<HierarchyReferences>().GetReference("Fill") as Image;
		if (target_harvestable.growingStateManager != null)
		{
			float num = target_harvestable.growingStateManager.PercentGrown();
			image.rectTransform.offsetMin = new Vector2(image.rectTransform.offsetMin.x, 3f);
			if (this.bar.activeSelf != !target_harvestable.CanBeHarvested())
			{
				this.bar.SetActive(!target_harvestable.CanBeHarvested());
			}
			float num2 = target_harvestable.CanBeHarvested() ? 3f : (19f - 19f * num + 3f);
			image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, -num2);
		}
		else if (this.bar.activeSelf)
		{
			this.bar.SetActive(false);
		}
		WiltCondition component = target_harvestable.GetComponent<WiltCondition>();
		if (component != null)
		{
			for (int i = 0; i < this.horizontal_containers.Length; i++)
			{
				this.horizontal_containers[i].SetActive(false);
			}
			foreach (KeyValuePair<WiltCondition.Condition, GameObject> keyValuePair in this.condition_icons)
			{
				keyValuePair.Value.SetActive(false);
			}
			if (!component.IsWilting())
			{
				this.vertical_container.SetActive(false);
				image.color = HarvestableOverlayWidget.growing_color;
				return;
			}
			this.vertical_container.SetActive(true);
			image.color = HarvestableOverlayWidget.wilting_color;
			List<WiltCondition.Condition> list = component.CurrentWiltSources();
			if (list.Count > 0)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (this.condition_icons.ContainsKey(list[j]))
					{
						this.condition_icons[list[j]].SetActive(true);
						this.horizontal_containers[j / 2].SetActive(true);
						this.condition_icons[list[j]].transform.SetParent(this.horizontal_containers[j / 2].transform);
					}
				}
				return;
			}
		}
		else
		{
			image.color = HarvestableOverlayWidget.growing_color;
			this.vertical_container.SetActive(false);
		}
	}

	// Token: 0x04004763 RID: 18275
	[SerializeField]
	private GameObject vertical_container;

	// Token: 0x04004764 RID: 18276
	[SerializeField]
	private GameObject bar;

	// Token: 0x04004765 RID: 18277
	private const int icons_per_row = 2;

	// Token: 0x04004766 RID: 18278
	private const float bar_fill_range = 19f;

	// Token: 0x04004767 RID: 18279
	private const float bar_fill_offset = 3f;

	// Token: 0x04004768 RID: 18280
	private static Color growing_color = new Color(0.9843137f, 0.6901961f, 0.23137255f, 1f);

	// Token: 0x04004769 RID: 18281
	private static Color wilting_color = new Color(0.5647059f, 0.5647059f, 0.5647059f, 1f);

	// Token: 0x0400476A RID: 18282
	[SerializeField]
	private Sprite sprite_liquid;

	// Token: 0x0400476B RID: 18283
	[SerializeField]
	private Sprite sprite_atmosphere;

	// Token: 0x0400476C RID: 18284
	[SerializeField]
	private Sprite sprite_pressure;

	// Token: 0x0400476D RID: 18285
	[SerializeField]
	private Sprite sprite_temperature;

	// Token: 0x0400476E RID: 18286
	[SerializeField]
	private Sprite sprite_resource;

	// Token: 0x0400476F RID: 18287
	[SerializeField]
	private Sprite sprite_light;

	// Token: 0x04004770 RID: 18288
	[SerializeField]
	private Sprite sprite_receptacle;

	// Token: 0x04004771 RID: 18289
	[SerializeField]
	private GameObject horizontal_container_prefab;

	// Token: 0x04004772 RID: 18290
	private GameObject[] horizontal_containers = new GameObject[7];

	// Token: 0x04004773 RID: 18291
	[SerializeField]
	private GameObject icon_gameobject_prefab;

	// Token: 0x04004774 RID: 18292
	private Dictionary<WiltCondition.Condition, GameObject> condition_icons = new Dictionary<WiltCondition.Condition, GameObject>();

	// Token: 0x04004775 RID: 18293
	private Dictionary<WiltCondition.Condition, Sprite> condition_sprites = new Dictionary<WiltCondition.Condition, Sprite>();
}
