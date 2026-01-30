using System;
using UnityEngine;

// Token: 0x02000E4D RID: 3661
public class LoreBearerSideScreen : SideScreenContent
{
	// Token: 0x06007414 RID: 29716 RVA: 0x002C5093 File Offset: 0x002C3293
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LoreBearer>() != null;
	}

	// Token: 0x06007415 RID: 29717 RVA: 0x002C50A1 File Offset: 0x002C32A1
	public override int GetSideScreenSortOrder()
	{
		return this.target.GetSideScreenSortOrder();
	}

	// Token: 0x06007416 RID: 29718 RVA: 0x002C50AE File Offset: 0x002C32AE
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<LoreBearer>();
		this.Refresh();
	}

	// Token: 0x06007417 RID: 29719 RVA: 0x002C50D8 File Offset: 0x002C32D8
	private void Refresh()
	{
		this.button.isInteractable = this.target.SidescreenButtonInteractable();
		this.button.ClearOnClick();
		this.button.onClick += this.target.OnSidescreenButtonPressed;
		this.button.onClick += this.Refresh;
		this.button.GetComponentInChildren<LocText>().SetText(this.target.SidescreenButtonText);
		this.button.GetComponent<ToolTip>().SetSimpleTooltip(this.target.SidescreenButtonTooltip);
	}

	// Token: 0x04005049 RID: 20553
	public const int DefaultButtonMenuSideScreenSortOrder = 20;

	// Token: 0x0400504A RID: 20554
	public KButton button;

	// Token: 0x0400504B RID: 20555
	private LoreBearer target;
}
