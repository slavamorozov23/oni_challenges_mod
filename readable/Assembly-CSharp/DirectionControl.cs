using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000747 RID: 1863
[AddComponentMenu("KMonoBehaviour/scripts/DirectionControl")]
public class DirectionControl : KMonoBehaviour
{
	// Token: 0x06002EF9 RID: 12025 RVA: 0x0010F0E8 File Offset: 0x0010D2E8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.allowedDirection = WorkableReactable.AllowedDirection.Any;
		this.directionInfos = new DirectionControl.DirectionInfo[]
		{
			new DirectionControl.DirectionInfo
			{
				allowLeft = true,
				allowRight = true,
				iconName = "action_direction_both",
				name = UI.USERMENUACTIONS.WORKABLE_DIRECTION_BOTH.NAME,
				tooltip = UI.USERMENUACTIONS.WORKABLE_DIRECTION_BOTH.TOOLTIP
			},
			new DirectionControl.DirectionInfo
			{
				allowLeft = true,
				allowRight = false,
				iconName = "action_direction_left",
				name = UI.USERMENUACTIONS.WORKABLE_DIRECTION_LEFT.NAME,
				tooltip = UI.USERMENUACTIONS.WORKABLE_DIRECTION_LEFT.TOOLTIP
			},
			new DirectionControl.DirectionInfo
			{
				allowLeft = false,
				allowRight = true,
				iconName = "action_direction_right",
				name = UI.USERMENUACTIONS.WORKABLE_DIRECTION_RIGHT.NAME,
				tooltip = UI.USERMENUACTIONS.WORKABLE_DIRECTION_RIGHT.TOOLTIP
			}
		};
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DirectionControl, this);
	}

	// Token: 0x06002EFA RID: 12026 RVA: 0x0010F214 File Offset: 0x0010D414
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SetAllowedDirection(this.allowedDirection);
		base.Subscribe<DirectionControl>(493375141, DirectionControl.OnRefreshUserMenuDelegate);
		base.Subscribe<DirectionControl>(-905833192, DirectionControl.OnCopySettingsDelegate);
	}

	// Token: 0x06002EFB RID: 12027 RVA: 0x0010F24C File Offset: 0x0010D44C
	private void SetAllowedDirection(WorkableReactable.AllowedDirection new_direction)
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		DirectionControl.DirectionInfo directionInfo = this.directionInfos[(int)new_direction];
		bool flag = directionInfo.allowLeft && directionInfo.allowRight;
		bool is_visible = !flag && directionInfo.allowLeft;
		bool is_visible2 = !flag && directionInfo.allowRight;
		component.SetSymbolVisiblity("arrow2", flag);
		component.SetSymbolVisiblity("arrow_left", is_visible);
		component.SetSymbolVisiblity("arrow_right", is_visible2);
		if (new_direction != this.allowedDirection)
		{
			this.allowedDirection = new_direction;
			if (this.onDirectionChanged != null)
			{
				this.onDirectionChanged(this.allowedDirection);
			}
		}
	}

	// Token: 0x06002EFC RID: 12028 RVA: 0x0010F2F3 File Offset: 0x0010D4F3
	private void OnChangeWorkableDirection()
	{
		this.SetAllowedDirection((WorkableReactable.AllowedDirection.Left + (int)this.allowedDirection) % (WorkableReactable.AllowedDirection)this.directionInfos.Length);
	}

	// Token: 0x06002EFD RID: 12029 RVA: 0x0010F30C File Offset: 0x0010D50C
	private void OnCopySettings(object data)
	{
		DirectionControl component = ((GameObject)data).GetComponent<DirectionControl>();
		this.SetAllowedDirection(component.allowedDirection);
	}

	// Token: 0x06002EFE RID: 12030 RVA: 0x0010F334 File Offset: 0x0010D534
	private void OnRefreshUserMenu(object data)
	{
		int num = (int)((WorkableReactable.AllowedDirection.Left + (int)this.allowedDirection) % (WorkableReactable.AllowedDirection)this.directionInfos.Length);
		DirectionControl.DirectionInfo directionInfo = this.directionInfos[num];
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo(directionInfo.iconName, directionInfo.name, new System.Action(this.OnChangeWorkableDirection), global::Action.NumActions, null, null, null, directionInfo.tooltip, true), 0.4f);
	}

	// Token: 0x04001BD0 RID: 7120
	[Serialize]
	public WorkableReactable.AllowedDirection allowedDirection;

	// Token: 0x04001BD1 RID: 7121
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001BD2 RID: 7122
	private DirectionControl.DirectionInfo[] directionInfos;

	// Token: 0x04001BD3 RID: 7123
	public Action<WorkableReactable.AllowedDirection> onDirectionChanged;

	// Token: 0x04001BD4 RID: 7124
	private static readonly EventSystem.IntraObjectHandler<DirectionControl> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<DirectionControl>(delegate(DirectionControl component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001BD5 RID: 7125
	private static readonly EventSystem.IntraObjectHandler<DirectionControl> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DirectionControl>(delegate(DirectionControl component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02001624 RID: 5668
	private struct DirectionInfo
	{
		// Token: 0x040073DB RID: 29659
		public bool allowLeft;

		// Token: 0x040073DC RID: 29660
		public bool allowRight;

		// Token: 0x040073DD RID: 29661
		public string iconName;

		// Token: 0x040073DE RID: 29662
		public string name;

		// Token: 0x040073DF RID: 29663
		public string tooltip;
	}
}
