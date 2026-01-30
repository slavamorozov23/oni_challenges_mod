using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000E36 RID: 3638
public class DoorToggleSideScreen : SideScreenContent
{
	// Token: 0x06007379 RID: 29561 RVA: 0x002C165D File Offset: 0x002BF85D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.InitButtons();
	}

	// Token: 0x0600737A RID: 29562 RVA: 0x002C166C File Offset: 0x002BF86C
	private void InitButtons()
	{
		this.buttonList.Add(new DoorToggleSideScreen.DoorButtonInfo
		{
			button = this.openButton,
			state = Door.ControlState.Opened,
			currentString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.OPEN,
			pendingString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.OPEN_PENDING
		});
		this.buttonList.Add(new DoorToggleSideScreen.DoorButtonInfo
		{
			button = this.autoButton,
			state = Door.ControlState.Auto,
			currentString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.AUTO,
			pendingString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.AUTO_PENDING
		});
		this.buttonList.Add(new DoorToggleSideScreen.DoorButtonInfo
		{
			button = this.closeButton,
			state = Door.ControlState.Locked,
			currentString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.CLOSE,
			pendingString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.CLOSE_PENDING
		});
		using (List<DoorToggleSideScreen.DoorButtonInfo>.Enumerator enumerator = this.buttonList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DoorToggleSideScreen.DoorButtonInfo info = enumerator.Current;
				info.button.onClick += delegate()
				{
					this.target.QueueStateChange(info.state);
					this.Refresh();
				};
			}
		}
	}

	// Token: 0x0600737B RID: 29563 RVA: 0x002C17C8 File Offset: 0x002BF9C8
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Door>() != null;
	}

	// Token: 0x0600737C RID: 29564 RVA: 0x002C17D8 File Offset: 0x002BF9D8
	public override void SetTarget(GameObject target)
	{
		if (this.target != null)
		{
			this.ClearTarget();
		}
		base.SetTarget(target);
		this.target = target.GetComponent<Door>();
		this.accessTarget = target.GetComponent<AccessControl>();
		if (this.target == null)
		{
			return;
		}
		target.Subscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
		target.Subscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
		this.Refresh();
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600737D RID: 29565 RVA: 0x002C186C File Offset: 0x002BFA6C
	public override void ClearTarget()
	{
		if (this.target != null)
		{
			this.target.Unsubscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
			this.target.Unsubscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
		}
		this.target = null;
	}

	// Token: 0x0600737E RID: 29566 RVA: 0x002C18C8 File Offset: 0x002BFAC8
	private void Refresh()
	{
		string text = null;
		string text2 = null;
		if (this.buttonList == null || this.buttonList.Count == 0)
		{
			this.InitButtons();
		}
		foreach (DoorToggleSideScreen.DoorButtonInfo doorButtonInfo in this.buttonList)
		{
			if (this.target.CurrentState == doorButtonInfo.state && this.target.RequestedState == doorButtonInfo.state)
			{
				doorButtonInfo.button.isOn = true;
				text = doorButtonInfo.currentString;
				foreach (ImageToggleState imageToggleState in doorButtonInfo.button.GetComponentsInChildren<ImageToggleState>())
				{
					imageToggleState.SetActive();
					imageToggleState.SetActive();
				}
				doorButtonInfo.button.GetComponent<ImageToggleStateThrobber>().enabled = false;
			}
			else if (this.target.RequestedState == doorButtonInfo.state)
			{
				doorButtonInfo.button.isOn = true;
				text2 = doorButtonInfo.pendingString;
				foreach (ImageToggleState imageToggleState2 in doorButtonInfo.button.GetComponentsInChildren<ImageToggleState>())
				{
					imageToggleState2.SetActive();
					imageToggleState2.SetActive();
				}
				doorButtonInfo.button.GetComponent<ImageToggleStateThrobber>().enabled = true;
			}
			else
			{
				doorButtonInfo.button.isOn = false;
				foreach (ImageToggleState imageToggleState3 in doorButtonInfo.button.GetComponentsInChildren<ImageToggleState>())
				{
					imageToggleState3.SetInactive();
					imageToggleState3.SetInactive();
				}
				doorButtonInfo.button.GetComponent<ImageToggleStateThrobber>().enabled = false;
			}
		}
		string text3 = text;
		if (text2 != null)
		{
			text3 = string.Format(UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.PENDING_FORMAT, text3, text2);
		}
		if (this.accessTarget != null && !this.accessTarget.Online)
		{
			text3 = string.Format(UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.ACCESS_FORMAT, text3, UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.ACCESS_OFFLINE);
		}
		if (this.target.building.Def.PrefabID == POIDoorInternalConfig.ID)
		{
			text3 = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.POI_INTERNAL;
			using (List<DoorToggleSideScreen.DoorButtonInfo>.Enumerator enumerator = this.buttonList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DoorToggleSideScreen.DoorButtonInfo doorButtonInfo2 = enumerator.Current;
					doorButtonInfo2.button.gameObject.SetActive(false);
				}
				goto IL_2A1;
			}
		}
		foreach (DoorToggleSideScreen.DoorButtonInfo doorButtonInfo3 in this.buttonList)
		{
			bool active = doorButtonInfo3.state != Door.ControlState.Auto || this.target.allowAutoControl;
			doorButtonInfo3.button.gameObject.SetActive(active);
		}
		IL_2A1:
		this.description.text = text3;
		this.description.gameObject.SetActive(!string.IsNullOrEmpty(text3));
		this.ContentContainer.SetActive(!this.target.isSealed);
	}

	// Token: 0x0600737F RID: 29567 RVA: 0x002C1C00 File Offset: 0x002BFE00
	private void OnDoorStateChanged(object data)
	{
		this.Refresh();
	}

	// Token: 0x06007380 RID: 29568 RVA: 0x002C1C08 File Offset: 0x002BFE08
	private void OnAccessControlChanged(object data)
	{
		this.Refresh();
	}

	// Token: 0x04004FD7 RID: 20439
	[SerializeField]
	private KToggle openButton;

	// Token: 0x04004FD8 RID: 20440
	[SerializeField]
	private KToggle autoButton;

	// Token: 0x04004FD9 RID: 20441
	[SerializeField]
	private KToggle closeButton;

	// Token: 0x04004FDA RID: 20442
	[SerializeField]
	private LocText description;

	// Token: 0x04004FDB RID: 20443
	private Door target;

	// Token: 0x04004FDC RID: 20444
	private AccessControl accessTarget;

	// Token: 0x04004FDD RID: 20445
	private List<DoorToggleSideScreen.DoorButtonInfo> buttonList = new List<DoorToggleSideScreen.DoorButtonInfo>();

	// Token: 0x020020B5 RID: 8373
	private struct DoorButtonInfo
	{
		// Token: 0x040096FE RID: 38654
		public KToggle button;

		// Token: 0x040096FF RID: 38655
		public Door.ControlState state;

		// Token: 0x04009700 RID: 38656
		public string currentString;

		// Token: 0x04009701 RID: 38657
		public string pendingString;
	}
}
