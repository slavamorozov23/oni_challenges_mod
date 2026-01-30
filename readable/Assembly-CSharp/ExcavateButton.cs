using System;
using STRINGS;

// Token: 0x020002D7 RID: 727
public class ExcavateButton : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000ECE RID: 3790 RVA: 0x00055EB1 File Offset: 0x000540B1
	public string SidescreenButtonText
	{
		get
		{
			if (this.isMarkedForDig == null || !this.isMarkedForDig())
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_EXCAVATE_BUTTON;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_CANCEL_EXCAVATION_BUTTON;
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x06000ECF RID: 3791 RVA: 0x00055EDD File Offset: 0x000540DD
	public string SidescreenButtonTooltip
	{
		get
		{
			if (this.isMarkedForDig == null || !this.isMarkedForDig())
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_EXCAVATE_BUTTON_TOOLTIP;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_CANCEL_EXCAVATION_BUTTON_TOOLTIP;
		}
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x00055F09 File Offset: 0x00054109
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x00055F0C File Offset: 0x0005410C
	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x00055F13 File Offset: 0x00054113
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x00055F16 File Offset: 0x00054116
	public bool SidescreenButtonInteractable()
	{
		return true;
	}

	// Token: 0x06000ED4 RID: 3796 RVA: 0x00055F19 File Offset: 0x00054119
	public void OnSidescreenButtonPressed()
	{
		System.Action onButtonPressed = this.OnButtonPressed;
		if (onButtonPressed == null)
		{
			return;
		}
		onButtonPressed();
	}

	// Token: 0x06000ED5 RID: 3797 RVA: 0x00055F2B File Offset: 0x0005412B
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x0400099D RID: 2461
	public Func<bool> isMarkedForDig;

	// Token: 0x0400099E RID: 2462
	public System.Action OnButtonPressed;
}
