using System;
using UnityEngine;

// Token: 0x02000DE2 RID: 3554
[AddComponentMenu("KMonoBehaviour/scripts/PriorityButton")]
public class PriorityButton : KMonoBehaviour
{
	// Token: 0x170007D1 RID: 2001
	// (get) Token: 0x06006FBE RID: 28606 RVA: 0x002A7128 File Offset: 0x002A5328
	// (set) Token: 0x06006FBF RID: 28607 RVA: 0x002A7130 File Offset: 0x002A5330
	public PrioritySetting priority
	{
		get
		{
			return this._priority;
		}
		set
		{
			this._priority = value;
			if (this.its != null)
			{
				if (this.priority.priority_class == PriorityScreen.PriorityClass.high)
				{
					this.its.colorStyleSetting = this.highStyle;
				}
				else
				{
					this.its.colorStyleSetting = this.normalStyle;
				}
				this.its.RefreshColorStyle();
				this.its.ResetColor();
			}
		}
	}

	// Token: 0x06006FC0 RID: 28608 RVA: 0x002A719A File Offset: 0x002A539A
	protected override void OnPrefabInit()
	{
		this.toggle.onClick += this.OnClick;
	}

	// Token: 0x06006FC1 RID: 28609 RVA: 0x002A71B3 File Offset: 0x002A53B3
	private void OnClick()
	{
		if (this.playSelectionSound)
		{
			PriorityScreen.PlayPriorityConfirmSound(this.priority);
		}
		if (this.onClick != null)
		{
			this.onClick(this.priority);
		}
	}

	// Token: 0x04004C97 RID: 19607
	public KToggle toggle;

	// Token: 0x04004C98 RID: 19608
	public LocText text;

	// Token: 0x04004C99 RID: 19609
	public ToolTip tooltip;

	// Token: 0x04004C9A RID: 19610
	[MyCmpGet]
	private ImageToggleState its;

	// Token: 0x04004C9B RID: 19611
	public ColorStyleSetting normalStyle;

	// Token: 0x04004C9C RID: 19612
	public ColorStyleSetting highStyle;

	// Token: 0x04004C9D RID: 19613
	public bool playSelectionSound = true;

	// Token: 0x04004C9E RID: 19614
	public Action<PrioritySetting> onClick;

	// Token: 0x04004C9F RID: 19615
	private PrioritySetting _priority;
}
