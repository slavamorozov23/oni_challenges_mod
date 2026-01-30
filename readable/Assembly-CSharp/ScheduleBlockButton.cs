using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000DFE RID: 3582
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleBlockButton")]
public class ScheduleBlockButton : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600717C RID: 29052 RVA: 0x002B5A9C File Offset: 0x002B3C9C
	public void Setup(int hour)
	{
		if (hour < TRAITS.EARLYBIRD_SCHEDULEBLOCK)
		{
			base.GetComponent<HierarchyReferences>().GetReference<RectTransform>("MorningIcon").gameObject.SetActive(true);
		}
		else if (hour >= 21)
		{
			base.GetComponent<HierarchyReferences>().GetReference<RectTransform>("NightIcon").gameObject.SetActive(true);
		}
		base.gameObject.name = "ScheduleBlock_" + hour.ToString();
		this.ToggleHighlight(false);
	}

	// Token: 0x0600717D RID: 29053 RVA: 0x002B5B14 File Offset: 0x002B3D14
	public void SetBlockTypes(List<ScheduleBlockType> blockTypes)
	{
		ScheduleGroup scheduleGroup = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(blockTypes);
		if (scheduleGroup != null)
		{
			this.image.color = scheduleGroup.uiColor;
			this.toolTip.SetSimpleTooltip(scheduleGroup.Name);
			return;
		}
		this.toolTip.SetSimpleTooltip("UNKNOWN");
	}

	// Token: 0x0600717E RID: 29054 RVA: 0x002B5B68 File Offset: 0x002B3D68
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.ToggleHighlight(true);
	}

	// Token: 0x0600717F RID: 29055 RVA: 0x002B5B71 File Offset: 0x002B3D71
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ToggleHighlight(false);
	}

	// Token: 0x06007180 RID: 29056 RVA: 0x002B5B7A File Offset: 0x002B3D7A
	private void ToggleHighlight(bool on)
	{
		this.highlightObject.SetActive(on);
	}

	// Token: 0x04004E5A RID: 20058
	[SerializeField]
	private Image image;

	// Token: 0x04004E5B RID: 20059
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x04004E5C RID: 20060
	[SerializeField]
	private GameObject highlightObject;
}
