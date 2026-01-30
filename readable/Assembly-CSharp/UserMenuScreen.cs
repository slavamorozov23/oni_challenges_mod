using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000C7C RID: 3196
public class UserMenuScreen : KIconButtonMenu
{
	// Token: 0x060061B6 RID: 25014 RVA: 0x00241480 File Offset: 0x0023F680
	protected override void OnPrefabInit()
	{
		this.keepMenuOpen = true;
		base.OnPrefabInit();
		this.priorityScreen = Util.KInstantiateUI<PriorityScreen>(this.priorityScreenPrefab.gameObject, this.priorityScreenParent, false);
		this.priorityScreen.InstantiateButtons(new Action<PrioritySetting>(this.OnPriorityClicked), true);
		this.buttonParent.transform.SetAsLastSibling();
	}

	// Token: 0x060061B7 RID: 25015 RVA: 0x002414DF File Offset: 0x0023F6DF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.Subscribe(1980521255, new Action<object>(this.OnUIRefresh));
		KInputManager.InputChange.AddListener(new UnityAction(base.RefreshButtonTooltip));
	}

	// Token: 0x060061B8 RID: 25016 RVA: 0x00241519 File Offset: 0x0023F719
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(base.RefreshButtonTooltip));
		base.OnForcedCleanUp();
	}

	// Token: 0x060061B9 RID: 25017 RVA: 0x00241537 File Offset: 0x0023F737
	public void SetSelected(GameObject go)
	{
		this.ClearPrioritizable();
		this.selected = go;
		this.RefreshPrioritizable();
	}

	// Token: 0x060061BA RID: 25018 RVA: 0x0024154C File Offset: 0x0023F74C
	private void ClearPrioritizable()
	{
		if (this.selected != null)
		{
			Prioritizable component = this.selected.GetComponent<Prioritizable>();
			if (component != null)
			{
				Prioritizable prioritizable = component;
				prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Remove(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnPriorityChanged));
			}
		}
	}

	// Token: 0x060061BB RID: 25019 RVA: 0x002415A0 File Offset: 0x0023F7A0
	private void RefreshPrioritizable()
	{
		if (this.selected != null)
		{
			Prioritizable component = this.selected.GetComponent<Prioritizable>();
			if (component != null && component.IsPrioritizable())
			{
				Prioritizable prioritizable = component;
				prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnPriorityChanged));
				this.priorityScreen.gameObject.SetActive(true);
				this.priorityScreen.SetScreenPriority(component.GetMasterPriority(), false);
				return;
			}
			this.priorityScreen.gameObject.SetActive(false);
		}
	}

	// Token: 0x060061BC RID: 25020 RVA: 0x00241630 File Offset: 0x0023F830
	public void Refresh(GameObject go)
	{
		if (go != this.selected)
		{
			return;
		}
		this.buttonInfos.Clear();
		this.slidersInfos.Clear();
		Game.Instance.userMenu.AppendToScreen(go, this);
		base.SetButtons(this.buttonInfos);
		base.RefreshButtons();
		this.RefreshSliders();
		this.ClearPrioritizable();
		this.RefreshPrioritizable();
		if ((this.sliders == null || this.sliders.Count == 0) && (this.buttonInfos == null || this.buttonInfos.Count == 0) && !this.priorityScreen.gameObject.activeSelf)
		{
			base.transform.parent.gameObject.SetActive(false);
			return;
		}
		base.transform.parent.gameObject.SetActive(true);
	}

	// Token: 0x060061BD RID: 25021 RVA: 0x00241700 File Offset: 0x0023F900
	public void AddSliders(IList<UserMenu.SliderInfo> sliders)
	{
		this.slidersInfos.AddRange(sliders);
	}

	// Token: 0x060061BE RID: 25022 RVA: 0x0024170E File Offset: 0x0023F90E
	public void AddButtons(IList<KIconButtonMenu.ButtonInfo> buttons)
	{
		this.buttonInfos.AddRange(buttons);
	}

	// Token: 0x060061BF RID: 25023 RVA: 0x0024171C File Offset: 0x0023F91C
	private void OnUIRefresh(object data)
	{
		this.Refresh(data as GameObject);
	}

	// Token: 0x060061C0 RID: 25024 RVA: 0x0024172C File Offset: 0x0023F92C
	public void RefreshSliders()
	{
		if (this.sliders != null)
		{
			for (int i = 0; i < this.sliders.Count; i++)
			{
				UnityEngine.Object.Destroy(this.sliders[i].gameObject);
			}
			this.sliders = null;
		}
		if (this.slidersInfos == null || this.slidersInfos.Count == 0)
		{
			return;
		}
		this.sliders = new List<MinMaxSlider>();
		for (int j = 0; j < this.slidersInfos.Count; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.sliderPrefab.gameObject, Vector3.zero, Quaternion.identity);
			this.slidersInfos[j].sliderGO = gameObject;
			MinMaxSlider component = gameObject.GetComponent<MinMaxSlider>();
			this.sliders.Add(component);
			Transform parent = (this.sliderParent != null) ? this.sliderParent.transform : base.transform;
			gameObject.transform.SetParent(parent, false);
			gameObject.SetActive(true);
			gameObject.name = "Slider";
			if (component.toolTip)
			{
				component.toolTip.toolTip = this.slidersInfos[j].toolTip;
			}
			component.lockType = this.slidersInfos[j].lockType;
			component.interactable = this.slidersInfos[j].interactable;
			component.minLimit = this.slidersInfos[j].minLimit;
			component.maxLimit = this.slidersInfos[j].maxLimit;
			component.currentMinValue = this.slidersInfos[j].currentMinValue;
			component.currentMaxValue = this.slidersInfos[j].currentMaxValue;
			component.onMinChange = this.slidersInfos[j].onMinChange;
			component.onMaxChange = this.slidersInfos[j].onMaxChange;
			component.direction = this.slidersInfos[j].direction;
			component.SetMode(this.slidersInfos[j].mode);
			component.SetMinMaxValue(this.slidersInfos[j].currentMinValue, this.slidersInfos[j].currentMaxValue, this.slidersInfos[j].minLimit, this.slidersInfos[j].maxLimit);
		}
	}

	// Token: 0x060061C1 RID: 25025 RVA: 0x00241990 File Offset: 0x0023FB90
	private void OnPriorityClicked(PrioritySetting priority)
	{
		if (this.selected != null)
		{
			Prioritizable component = this.selected.GetComponent<Prioritizable>();
			if (component != null)
			{
				component.SetMasterPriority(priority);
			}
		}
	}

	// Token: 0x060061C2 RID: 25026 RVA: 0x002419C7 File Offset: 0x0023FBC7
	private void OnPriorityChanged(PrioritySetting priority)
	{
		this.priorityScreen.SetScreenPriority(priority, false);
	}

	// Token: 0x04004170 RID: 16752
	private GameObject selected;

	// Token: 0x04004171 RID: 16753
	public MinMaxSlider sliderPrefab;

	// Token: 0x04004172 RID: 16754
	public GameObject sliderParent;

	// Token: 0x04004173 RID: 16755
	public PriorityScreen priorityScreenPrefab;

	// Token: 0x04004174 RID: 16756
	public GameObject priorityScreenParent;

	// Token: 0x04004175 RID: 16757
	private List<MinMaxSlider> sliders = new List<MinMaxSlider>();

	// Token: 0x04004176 RID: 16758
	private List<UserMenu.SliderInfo> slidersInfos = new List<UserMenu.SliderInfo>();

	// Token: 0x04004177 RID: 16759
	private List<KIconButtonMenu.ButtonInfo> buttonInfos = new List<KIconButtonMenu.ButtonInfo>();

	// Token: 0x04004178 RID: 16760
	private PriorityScreen priorityScreen;
}
