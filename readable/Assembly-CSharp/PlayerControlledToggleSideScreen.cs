using System;
using UnityEngine;

// Token: 0x02000E62 RID: 3682
public class PlayerControlledToggleSideScreen : SideScreenContent, IRenderEveryTick
{
	// Token: 0x060074E9 RID: 29929 RVA: 0x002CA0C8 File Offset: 0x002C82C8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.toggleButton.onClick += this.ClickToggle;
		this.togglePendingStatusItem = new StatusItem("PlayerControlledToggleSideScreen", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
	}

	// Token: 0x060074EA RID: 29930 RVA: 0x002CA11B File Offset: 0x002C831B
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IPlayerControlledToggle>() != null;
	}

	// Token: 0x060074EB RID: 29931 RVA: 0x002CA128 File Offset: 0x002C8328
	public void RenderEveryTick(float dt)
	{
		if (base.isActiveAndEnabled)
		{
			if (!this.keyDown && (Input.GetKeyDown(KeyCode.Return) & Time.unscaledTime - this.lastKeyboardShortcutTime > 0.1f))
			{
				if (SpeedControlScreen.Instance.IsPaused)
				{
					this.RequestToggle();
				}
				else
				{
					this.Toggle();
				}
				this.lastKeyboardShortcutTime = Time.unscaledTime;
				this.keyDown = true;
			}
			if (this.keyDown && Input.GetKeyUp(KeyCode.Return))
			{
				this.keyDown = false;
			}
		}
	}

	// Token: 0x060074EC RID: 29932 RVA: 0x002CA1A6 File Offset: 0x002C83A6
	private void ClickToggle()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			this.RequestToggle();
			return;
		}
		this.Toggle();
	}

	// Token: 0x060074ED RID: 29933 RVA: 0x002CA1C4 File Offset: 0x002C83C4
	private void RequestToggle()
	{
		this.target.ToggleRequested = !this.target.ToggleRequested;
		if (this.target.ToggleRequested && SpeedControlScreen.Instance.IsPaused)
		{
			this.target.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, this.togglePendingStatusItem, this);
		}
		else
		{
			this.target.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
		this.UpdateVisuals(this.target.ToggleRequested ? (!this.target.ToggledOn()) : this.target.ToggledOn(), true);
	}

	// Token: 0x060074EE RID: 29934 RVA: 0x002CA280 File Offset: 0x002C8480
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IPlayerControlledToggle>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received is not an IPlayerControlledToggle");
			return;
		}
		this.UpdateVisuals(this.target.ToggleRequested ? (!this.target.ToggledOn()) : this.target.ToggledOn(), false);
		this.titleKey = this.target.SideScreenTitleKey;
	}

	// Token: 0x060074EF RID: 29935 RVA: 0x002CA300 File Offset: 0x002C8500
	private void Toggle()
	{
		this.target.ToggledByPlayer();
		this.UpdateVisuals(this.target.ToggledOn(), true);
		this.target.ToggleRequested = false;
		this.target.GetSelectable().RemoveStatusItem(this.togglePendingStatusItem, false);
	}

	// Token: 0x060074F0 RID: 29936 RVA: 0x002CA350 File Offset: 0x002C8550
	private void UpdateVisuals(bool state, bool smooth)
	{
		if (state != this.currentState)
		{
			if (smooth)
			{
				this.kbac.Play(state ? PlayerControlledToggleSideScreen.ON_ANIMS : PlayerControlledToggleSideScreen.OFF_ANIMS, KAnim.PlayMode.Once);
			}
			else
			{
				this.kbac.Play(state ? PlayerControlledToggleSideScreen.ON_ANIMS[1] : PlayerControlledToggleSideScreen.OFF_ANIMS[1], KAnim.PlayMode.Once, 1f, 0f);
			}
		}
		this.currentState = state;
	}

	// Token: 0x040050D8 RID: 20696
	public IPlayerControlledToggle target;

	// Token: 0x040050D9 RID: 20697
	public KButton toggleButton;

	// Token: 0x040050DA RID: 20698
	protected static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on"
	};

	// Token: 0x040050DB RID: 20699
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"off_pre",
		"off"
	};

	// Token: 0x040050DC RID: 20700
	public float animScaleBase = 0.25f;

	// Token: 0x040050DD RID: 20701
	private StatusItem togglePendingStatusItem;

	// Token: 0x040050DE RID: 20702
	[SerializeField]
	private KBatchedAnimController kbac;

	// Token: 0x040050DF RID: 20703
	private float lastKeyboardShortcutTime;

	// Token: 0x040050E0 RID: 20704
	private const float KEYBOARD_COOLDOWN = 0.1f;

	// Token: 0x040050E1 RID: 20705
	private bool keyDown;

	// Token: 0x040050E2 RID: 20706
	private bool currentState;
}
