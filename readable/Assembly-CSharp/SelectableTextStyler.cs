using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000E09 RID: 3593
public class SelectableTextStyler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x060071D6 RID: 29142 RVA: 0x002B7D6B File Offset: 0x002B5F6B
	private void Start()
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
	}

	// Token: 0x060071D7 RID: 29143 RVA: 0x002B7D7A File Offset: 0x002B5F7A
	private void SetState(SelectableTextStyler.State state, SelectableTextStyler.HoverState hover_state)
	{
		if (state == SelectableTextStyler.State.Normal)
		{
			if (hover_state != SelectableTextStyler.HoverState.Normal)
			{
				if (hover_state == SelectableTextStyler.HoverState.Hovered)
				{
					this.target.textStyleSetting = this.normalHovered;
				}
			}
			else
			{
				this.target.textStyleSetting = this.normalNormal;
			}
		}
		this.target.ApplySettings();
	}

	// Token: 0x060071D8 RID: 29144 RVA: 0x002B7DB7 File Offset: 0x002B5FB7
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Hovered);
	}

	// Token: 0x060071D9 RID: 29145 RVA: 0x002B7DC6 File Offset: 0x002B5FC6
	public void OnPointerExit(PointerEventData eventData)
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
	}

	// Token: 0x060071DA RID: 29146 RVA: 0x002B7DD5 File Offset: 0x002B5FD5
	public void OnPointerClick(PointerEventData eventData)
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
	}

	// Token: 0x04004E9A RID: 20122
	[SerializeField]
	private LocText target;

	// Token: 0x04004E9B RID: 20123
	[SerializeField]
	private SelectableTextStyler.State state;

	// Token: 0x04004E9C RID: 20124
	[SerializeField]
	private TextStyleSetting normalNormal;

	// Token: 0x04004E9D RID: 20125
	[SerializeField]
	private TextStyleSetting normalHovered;

	// Token: 0x02002095 RID: 8341
	public enum State
	{
		// Token: 0x0400969D RID: 38557
		Normal
	}

	// Token: 0x02002096 RID: 8342
	public enum HoverState
	{
		// Token: 0x0400969F RID: 38559
		Normal,
		// Token: 0x040096A0 RID: 38560
		Hovered
	}
}
