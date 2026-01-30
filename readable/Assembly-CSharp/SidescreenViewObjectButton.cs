using System;
using UnityEngine;

// Token: 0x02000414 RID: 1044
public class SidescreenViewObjectButton : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x06001584 RID: 5508 RVA: 0x0007B034 File Offset: 0x00079234
	public bool IsValid()
	{
		SidescreenViewObjectButton.Mode trackMode = this.TrackMode;
		if (trackMode != SidescreenViewObjectButton.Mode.Target)
		{
			return trackMode == SidescreenViewObjectButton.Mode.Cell && Grid.IsValidCell(this.TargetCell);
		}
		return this.Target != null;
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06001585 RID: 5509 RVA: 0x0007B069 File Offset: 0x00079269
	public string SidescreenButtonText
	{
		get
		{
			return this.Text;
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06001586 RID: 5510 RVA: 0x0007B071 File Offset: 0x00079271
	public string SidescreenButtonTooltip
	{
		get
		{
			return this.Tooltip;
		}
	}

	// Token: 0x06001587 RID: 5511 RVA: 0x0007B079 File Offset: 0x00079279
	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06001588 RID: 5512 RVA: 0x0007B080 File Offset: 0x00079280
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06001589 RID: 5513 RVA: 0x0007B083 File Offset: 0x00079283
	public bool SidescreenButtonInteractable()
	{
		return this.IsValid();
	}

	// Token: 0x0600158A RID: 5514 RVA: 0x0007B08B File Offset: 0x0007928B
	public int HorizontalGroupID()
	{
		return this.horizontalGroupID;
	}

	// Token: 0x0600158B RID: 5515 RVA: 0x0007B094 File Offset: 0x00079294
	public void OnSidescreenButtonPressed()
	{
		if (this.IsValid())
		{
			SidescreenViewObjectButton.Mode trackMode = this.TrackMode;
			if (trackMode == SidescreenViewObjectButton.Mode.Target)
			{
				GameUtil.FocusCamera(this.Target.transform.GetPosition(), 2f, true, true);
				return;
			}
			if (trackMode == SidescreenViewObjectButton.Mode.Cell)
			{
				GameUtil.FocusCamera(Grid.CellToPos(this.TargetCell), 2f, true, true);
				return;
			}
		}
		else
		{
			base.gameObject.Trigger(1980521255, null);
		}
	}

	// Token: 0x0600158C RID: 5516 RVA: 0x0007B0FD File Offset: 0x000792FD
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x04000CDE RID: 3294
	public string Text;

	// Token: 0x04000CDF RID: 3295
	public string Tooltip;

	// Token: 0x04000CE0 RID: 3296
	public SidescreenViewObjectButton.Mode TrackMode;

	// Token: 0x04000CE1 RID: 3297
	public GameObject Target;

	// Token: 0x04000CE2 RID: 3298
	public int TargetCell;

	// Token: 0x04000CE3 RID: 3299
	public int horizontalGroupID = -1;

	// Token: 0x0200126C RID: 4716
	public enum Mode
	{
		// Token: 0x040067C9 RID: 26569
		Target,
		// Token: 0x040067CA RID: 26570
		Cell
	}
}
