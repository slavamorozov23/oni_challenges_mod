using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000ECF RID: 3791
[AddComponentMenu("KMonoBehaviour/scripts/MultiToggle")]
public class MultiToggle : KMonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	// Token: 0x1700084E RID: 2126
	// (get) Token: 0x06007963 RID: 31075 RVA: 0x002EA81D File Offset: 0x002E8A1D
	public int CurrentState
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x06007964 RID: 31076 RVA: 0x002EA825 File Offset: 0x002E8A25
	public void NextState()
	{
		this.ChangeState((this.state + 1) % this.states.Length);
	}

	// Token: 0x06007965 RID: 31077 RVA: 0x002EA83E File Offset: 0x002E8A3E
	protected virtual void Update()
	{
		if (this.clickHeldDown)
		{
			this.totalHeldTime += Time.unscaledDeltaTime;
			if (this.totalHeldTime > this.heldTimeThreshold && this.onHold != null)
			{
				this.onHold();
			}
		}
	}

	// Token: 0x06007966 RID: 31078 RVA: 0x002EA87B File Offset: 0x002E8A7B
	protected override void OnDisable()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			this.RefreshHoverColor();
			this.pointerOver = false;
			this.StopHolding();
		}
	}

	// Token: 0x06007967 RID: 31079 RVA: 0x002EA89D File Offset: 0x002E8A9D
	public void ChangeState(int new_state_index, bool forceRefreshState)
	{
		if (forceRefreshState)
		{
			this.stateDirty = true;
		}
		this.ChangeState(new_state_index);
	}

	// Token: 0x06007968 RID: 31080 RVA: 0x002EA8B0 File Offset: 0x002E8AB0
	public void ChangeState(int new_state_index)
	{
		if (!this.stateDirty && new_state_index == this.state)
		{
			return;
		}
		this.stateDirty = false;
		this.state = new_state_index;
		try
		{
			this.toggle_image.sprite = this.states[new_state_index].sprite;
			this.toggle_image.color = this.states[new_state_index].color;
			if (this.states[new_state_index].use_rect_margins)
			{
				this.toggle_image.rectTransform().sizeDelta = this.states[new_state_index].rect_margins;
			}
		}
		catch
		{
			string text = base.gameObject.name;
			Transform transform = base.transform;
			while (transform.parent != null)
			{
				text = text.Insert(0, transform.name + ">");
				transform = transform.parent;
			}
			global::Debug.LogError("Multi Toggle state index out of range: " + text + " idx:" + new_state_index.ToString(), base.gameObject);
		}
		foreach (StatePresentationSetting statePresentationSetting in this.states[this.state].additional_display_settings)
		{
			if (!(statePresentationSetting.image_target == null))
			{
				statePresentationSetting.image_target.sprite = statePresentationSetting.sprite;
				statePresentationSetting.image_target.color = statePresentationSetting.color;
			}
		}
		this.RefreshHoverColor();
	}

	// Token: 0x06007969 RID: 31081 RVA: 0x002EAA2C File Offset: 0x002E8C2C
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (!this.allowRightClick && eventData.button == PointerEventData.InputButton.Right)
		{
			return;
		}
		if (this.states.Length - 1 < this.state)
		{
			global::Debug.LogWarning("Multi toggle has too few / no states");
		}
		bool flag = false;
		if (this.onDoubleClick != null && eventData.clickCount == 2)
		{
			flag = this.onDoubleClick();
		}
		if (this.onClick != null && !flag)
		{
			this.onClick();
		}
		this.RefreshHoverColor();
	}

	// Token: 0x0600796A RID: 31082 RVA: 0x002EAAA4 File Offset: 0x002E8CA4
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.pointerOver = true;
		if (!KInputManager.isFocused)
		{
			return;
		}
		KInputManager.SetUserActive();
		if (this.states.Length == 0)
		{
			return;
		}
		if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
		{
			this.toggle_image.color = this.states[this.state].color_on_hover;
		}
		if (this.states[this.state].use_rect_margins)
		{
			this.toggle_image.rectTransform().sizeDelta = this.states[this.state].rect_margins;
		}
		foreach (StatePresentationSetting statePresentationSetting in this.states[this.state].additional_display_settings)
		{
			if (!(statePresentationSetting.image_target == null) && statePresentationSetting.use_color_on_hover)
			{
				statePresentationSetting.image_target.color = statePresentationSetting.color_on_hover;
			}
		}
		if (this.onEnter != null)
		{
			this.onEnter();
		}
	}

	// Token: 0x0600796B RID: 31083 RVA: 0x002EABE0 File Offset: 0x002E8DE0
	protected void RefreshHoverColor()
	{
		if (base.gameObject.activeInHierarchy)
		{
			if (this.pointerOver)
			{
				if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
				{
					this.toggle_image.color = this.states[this.state].color_on_hover;
				}
				foreach (StatePresentationSetting statePresentationSetting in this.states[this.state].additional_display_settings)
				{
					if (!(statePresentationSetting.image_target == null) && !(statePresentationSetting.image_target == null) && statePresentationSetting.use_color_on_hover)
					{
						statePresentationSetting.image_target.color = statePresentationSetting.color_on_hover;
					}
				}
			}
			return;
		}
		if (this.states.Length == 0)
		{
			return;
		}
		if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
		{
			this.toggle_image.color = this.states[this.state].color;
		}
		foreach (StatePresentationSetting statePresentationSetting2 in this.states[this.state].additional_display_settings)
		{
			if (!(statePresentationSetting2.image_target == null) && statePresentationSetting2.use_color_on_hover)
			{
				statePresentationSetting2.image_target.color = statePresentationSetting2.color;
			}
		}
	}

	// Token: 0x0600796C RID: 31084 RVA: 0x002EADA4 File Offset: 0x002E8FA4
	public void OnPointerExit(PointerEventData eventData)
	{
		this.pointerOver = false;
		if (!KInputManager.isFocused)
		{
			return;
		}
		KInputManager.SetUserActive();
		if (this.states.Length == 0)
		{
			return;
		}
		if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
		{
			this.toggle_image.color = this.states[this.state].color;
		}
		if (this.states[this.state].use_rect_margins)
		{
			this.toggle_image.rectTransform().sizeDelta = this.states[this.state].rect_margins;
		}
		foreach (StatePresentationSetting statePresentationSetting in this.states[this.state].additional_display_settings)
		{
			if (!(statePresentationSetting.image_target == null) && statePresentationSetting.use_color_on_hover)
			{
				statePresentationSetting.image_target.color = statePresentationSetting.color;
			}
		}
		if (this.onExit != null)
		{
			this.onExit();
		}
	}

	// Token: 0x0600796D RID: 31085 RVA: 0x002EAEE0 File Offset: 0x002E90E0
	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (!this.allowRightClick && eventData.button == PointerEventData.InputButton.Right)
		{
			return;
		}
		this.clickHeldDown = true;
		if (this.play_sound_on_click)
		{
			ToggleState toggleState = this.states[this.state];
			string on_click_override_sound_path = toggleState.on_click_override_sound_path;
			bool has_sound_parameter = toggleState.has_sound_parameter;
			if (on_click_override_sound_path == "")
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
				return;
			}
			if (on_click_override_sound_path != "" && has_sound_parameter)
			{
				KFMOD.PlayUISoundWithParameter(GlobalAssets.GetSound("General_Item_Click", false), toggleState.sound_parameter_name, toggleState.sound_parameter_value);
				KFMOD.PlayUISoundWithParameter(GlobalAssets.GetSound(on_click_override_sound_path, false), toggleState.sound_parameter_name, toggleState.sound_parameter_value);
				return;
			}
			KFMOD.PlayUISound(GlobalAssets.GetSound(on_click_override_sound_path, false));
		}
	}

	// Token: 0x0600796E RID: 31086 RVA: 0x002EAF9F File Offset: 0x002E919F
	public virtual void OnPointerUp(PointerEventData eventData)
	{
		if (!this.allowRightClick && eventData.button == PointerEventData.InputButton.Right)
		{
			return;
		}
		this.StopHolding();
	}

	// Token: 0x0600796F RID: 31087 RVA: 0x002EAFBC File Offset: 0x002E91BC
	private void StopHolding()
	{
		if (this.clickHeldDown)
		{
			if (this.play_sound_on_release && this.states[this.state].on_release_override_sound_path != "")
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound(this.states[this.state].on_release_override_sound_path, false));
			}
			this.clickHeldDown = false;
			if (this.onStopHold != null)
			{
				this.onStopHold();
			}
		}
		this.totalHeldTime = 0f;
	}

	// Token: 0x040054CF RID: 21711
	[Header("Settings")]
	[SerializeField]
	public ToggleState[] states;

	// Token: 0x040054D0 RID: 21712
	public bool play_sound_on_click = true;

	// Token: 0x040054D1 RID: 21713
	public bool play_sound_on_release;

	// Token: 0x040054D2 RID: 21714
	public Image toggle_image;

	// Token: 0x040054D3 RID: 21715
	protected int state;

	// Token: 0x040054D4 RID: 21716
	public System.Action onClick;

	// Token: 0x040054D5 RID: 21717
	private bool stateDirty = true;

	// Token: 0x040054D6 RID: 21718
	public Func<bool> onDoubleClick;

	// Token: 0x040054D7 RID: 21719
	public System.Action onEnter;

	// Token: 0x040054D8 RID: 21720
	public System.Action onExit;

	// Token: 0x040054D9 RID: 21721
	public System.Action onHold;

	// Token: 0x040054DA RID: 21722
	public System.Action onStopHold;

	// Token: 0x040054DB RID: 21723
	public bool allowRightClick = true;

	// Token: 0x040054DC RID: 21724
	protected bool clickHeldDown;

	// Token: 0x040054DD RID: 21725
	protected float totalHeldTime;

	// Token: 0x040054DE RID: 21726
	protected float heldTimeThreshold = 0.4f;

	// Token: 0x040054DF RID: 21727
	private bool pointerOver;
}
