using System;
using UnityEngine;

// Token: 0x02000EAF RID: 3759
public class TimeOfDayPositioner : KMonoBehaviour
{
	// Token: 0x06007885 RID: 30853 RVA: 0x002E59D8 File Offset: 0x002E3BD8
	public void SetTargetTimetable(GameObject TimetableRow)
	{
		if (TimetableRow == null)
		{
			this.targetRect = null;
			base.transform.SetParent(null);
			return;
		}
		RectTransform rectTransform = TimetableRow.GetComponent<HierarchyReferences>().GetReference<RectTransform>("BlockContainer").rectTransform();
		this.targetRect = rectTransform;
		base.transform.SetParent(this.targetRect.transform);
	}

	// Token: 0x06007886 RID: 30854 RVA: 0x002E5A38 File Offset: 0x002E3C38
	private void Update()
	{
		if (this.targetRect == null)
		{
			return;
		}
		if (base.transform.parent != this.targetRect.transform)
		{
			base.transform.parent = this.targetRect.transform;
		}
		float f = GameClock.Instance.GetCurrentCycleAsPercentage() * this.targetRect.rect.width;
		(base.transform as RectTransform).anchoredPosition = new Vector2(Mathf.Round(f), 0f);
	}

	// Token: 0x040053FC RID: 21500
	private RectTransform targetRect;
}
