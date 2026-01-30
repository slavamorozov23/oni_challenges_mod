using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000E03 RID: 3587
public class ScheduleScreenColumnEntry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerDownHandler
{
	// Token: 0x060071A9 RID: 29097 RVA: 0x002B6824 File Offset: 0x002B4A24
	public void OnPointerEnter(PointerEventData event_data)
	{
		this.RunCallbacks();
	}

	// Token: 0x060071AA RID: 29098 RVA: 0x002B682C File Offset: 0x002B4A2C
	private void RunCallbacks()
	{
		if (Input.GetMouseButton(0) && this.onLeftClick != null)
		{
			this.onLeftClick();
		}
	}

	// Token: 0x060071AB RID: 29099 RVA: 0x002B6849 File Offset: 0x002B4A49
	public void OnPointerDown(PointerEventData event_data)
	{
		this.RunCallbacks();
	}

	// Token: 0x04004E71 RID: 20081
	public Image image;

	// Token: 0x04004E72 RID: 20082
	public System.Action onLeftClick;
}
