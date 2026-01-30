using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000DFF RID: 3583
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleBlockPainter")]
public class ScheduleBlockPainter : KMonoBehaviour, IPointerDownHandler, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	// Token: 0x06007182 RID: 29058 RVA: 0x002B5B90 File Offset: 0x002B3D90
	public void SetEntry(ScheduleScreenEntry entry)
	{
		this.entry = entry;
	}

	// Token: 0x06007183 RID: 29059 RVA: 0x002B5B99 File Offset: 0x002B3D99
	public void OnBeginDrag(PointerEventData eventData)
	{
		this.PaintBlocksBelow(eventData);
	}

	// Token: 0x06007184 RID: 29060 RVA: 0x002B5BA2 File Offset: 0x002B3DA2
	public void OnDrag(PointerEventData eventData)
	{
		this.PaintBlocksBelow(eventData);
	}

	// Token: 0x06007185 RID: 29061 RVA: 0x002B5BAB File Offset: 0x002B3DAB
	public void OnEndDrag(PointerEventData eventData)
	{
		this.PaintBlocksBelow(eventData);
	}

	// Token: 0x06007186 RID: 29062 RVA: 0x002B5BB4 File Offset: 0x002B3DB4
	public void OnPointerDown(PointerEventData eventData)
	{
		ScheduleBlockPainter.paintCounter = 0;
		this.PaintBlocksBelow(eventData);
	}

	// Token: 0x06007187 RID: 29063 RVA: 0x002B5BC4 File Offset: 0x002B3DC4
	private void PaintBlocksBelow(PointerEventData eventData)
	{
		if (ScheduleScreen.Instance.SelectedPaint.IsNullOrWhiteSpace())
		{
			return;
		}
		List<RaycastResult> list = new List<RaycastResult>();
		UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, list);
		if (list != null && list.Count > 0)
		{
			ScheduleBlockButton component = list[0].gameObject.GetComponent<ScheduleBlockButton>();
			if (component != null)
			{
				if (this.entry.PaintBlock(component))
				{
					string sound = GlobalAssets.GetSound("ScheduleMenu_Select", false);
					if (sound != null)
					{
						EventInstance instance = SoundEvent.BeginOneShot(sound, SoundListenerController.Instance.transform.GetPosition(), 1f, false);
						instance.setParameterByName("Drag_Count", (float)ScheduleBlockPainter.paintCounter, false);
						ScheduleBlockPainter.paintCounter++;
						SoundEvent.EndOneShot(instance);
						this.previousBlockTriedPainted = component.gameObject;
						return;
					}
				}
				else if (this.previousBlockTriedPainted != component.gameObject)
				{
					this.previousBlockTriedPainted = component.gameObject;
					string sound2 = GlobalAssets.GetSound("ScheduleMenu_Select_none", false);
					if (sound2 != null)
					{
						SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound2, SoundListenerController.Instance.transform.GetPosition(), 1f, false));
					}
				}
			}
		}
	}

	// Token: 0x04004E5D RID: 20061
	private ScheduleScreenEntry entry;

	// Token: 0x04004E5E RID: 20062
	private static int paintCounter;

	// Token: 0x04004E5F RID: 20063
	private GameObject previousBlockTriedPainted;
}
