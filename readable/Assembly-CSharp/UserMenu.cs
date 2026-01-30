using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000671 RID: 1649
public class UserMenu
{
	// Token: 0x06002805 RID: 10245 RVA: 0x000E674F File Offset: 0x000E494F
	public void Refresh(GameObject go)
	{
		Game.Instance.Trigger(1980521255, go);
	}

	// Token: 0x06002806 RID: 10246 RVA: 0x000E6764 File Offset: 0x000E4964
	public void AddButton(GameObject go, KIconButtonMenu.ButtonInfo button, float sort_order = 1f)
	{
		if (button.onClick != null)
		{
			System.Action callback = button.onClick;
			button.onClick = delegate()
			{
				callback();
				Game.Instance.Trigger(1980521255, go);
			};
		}
		this.buttons.Add(new KeyValuePair<KIconButtonMenu.ButtonInfo, float>(button, sort_order));
	}

	// Token: 0x06002807 RID: 10247 RVA: 0x000E67B6 File Offset: 0x000E49B6
	public void AddSlider(GameObject go, UserMenu.SliderInfo slider)
	{
		this.sliders.Add(slider);
	}

	// Token: 0x06002808 RID: 10248 RVA: 0x000E67C4 File Offset: 0x000E49C4
	public void AppendToScreen(GameObject go, UserMenuScreen screen)
	{
		this.buttons.Clear();
		this.sliders.Clear();
		go.Trigger(493375141, null);
		if (this.buttons.Count > 0)
		{
			this.buttons.Sort(delegate(KeyValuePair<KIconButtonMenu.ButtonInfo, float> x, KeyValuePair<KIconButtonMenu.ButtonInfo, float> y)
			{
				if (x.Value == y.Value)
				{
					return 0;
				}
				if (x.Value > y.Value)
				{
					return 1;
				}
				return -1;
			});
			for (int i = 0; i < this.buttons.Count; i++)
			{
				this.sortedButtons.Add(this.buttons[i].Key);
			}
			screen.AddButtons(this.sortedButtons);
			this.sortedButtons.Clear();
		}
		if (this.sliders.Count > 0)
		{
			screen.AddSliders(this.sliders);
		}
	}

	// Token: 0x0400177E RID: 6014
	public const float DECONSTRUCT_PRIORITY = 0f;

	// Token: 0x0400177F RID: 6015
	public const float DRAWPATHS_PRIORITY = 0.1f;

	// Token: 0x04001780 RID: 6016
	public const float FOLLOWCAM_PRIORITY = 0.3f;

	// Token: 0x04001781 RID: 6017
	public const float SETDIRECTION_PRIORITY = 0.4f;

	// Token: 0x04001782 RID: 6018
	public const float AUTOBOTTLE_PRIORITY = 0.4f;

	// Token: 0x04001783 RID: 6019
	public const float AUTOREPAIR_PRIORITY = 0.5f;

	// Token: 0x04001784 RID: 6020
	public const float DEFAULT_PRIORITY = 1f;

	// Token: 0x04001785 RID: 6021
	public const float SUITEQUIP_PRIORITY = 2f;

	// Token: 0x04001786 RID: 6022
	public const float AUTODISINFECT_PRIORITY = 10f;

	// Token: 0x04001787 RID: 6023
	public const float ROCKETUSAGERESTRICTION_PRIORITY = 11f;

	// Token: 0x04001788 RID: 6024
	private List<KeyValuePair<KIconButtonMenu.ButtonInfo, float>> buttons = new List<KeyValuePair<KIconButtonMenu.ButtonInfo, float>>();

	// Token: 0x04001789 RID: 6025
	private List<UserMenu.SliderInfo> sliders = new List<UserMenu.SliderInfo>();

	// Token: 0x0400178A RID: 6026
	private List<KIconButtonMenu.ButtonInfo> sortedButtons = new List<KIconButtonMenu.ButtonInfo>();

	// Token: 0x0200153D RID: 5437
	public class SliderInfo
	{
		// Token: 0x04007133 RID: 28979
		public MinMaxSlider.LockingType lockType = MinMaxSlider.LockingType.Drag;

		// Token: 0x04007134 RID: 28980
		public MinMaxSlider.Mode mode;

		// Token: 0x04007135 RID: 28981
		public Slider.Direction direction;

		// Token: 0x04007136 RID: 28982
		public bool interactable = true;

		// Token: 0x04007137 RID: 28983
		public bool lockRange;

		// Token: 0x04007138 RID: 28984
		public string toolTip;

		// Token: 0x04007139 RID: 28985
		public string toolTipMin;

		// Token: 0x0400713A RID: 28986
		public string toolTipMax;

		// Token: 0x0400713B RID: 28987
		public float minLimit;

		// Token: 0x0400713C RID: 28988
		public float maxLimit = 100f;

		// Token: 0x0400713D RID: 28989
		public float currentMinValue = 10f;

		// Token: 0x0400713E RID: 28990
		public float currentMaxValue = 90f;

		// Token: 0x0400713F RID: 28991
		public GameObject sliderGO;

		// Token: 0x04007140 RID: 28992
		public Action<MinMaxSlider> onMinChange;

		// Token: 0x04007141 RID: 28993
		public Action<MinMaxSlider> onMaxChange;
	}
}
