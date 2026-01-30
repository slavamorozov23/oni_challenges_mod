using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000EB0 RID: 3760
[AddComponentMenu("KMonoBehaviour/scripts/TitleBar")]
public class TitleBar : KMonoBehaviour
{
	// Token: 0x06007888 RID: 30856 RVA: 0x002E5ACE File Offset: 0x002E3CCE
	public void SetTitle(string Name)
	{
		this.titleText.text = Name;
	}

	// Token: 0x06007889 RID: 30857 RVA: 0x002E5ADC File Offset: 0x002E3CDC
	public void SetSubText(string subtext, string tooltip = "")
	{
		this.subtextText.text = subtext;
		this.subtextText.GetComponent<ToolTip>().toolTip = tooltip;
	}

	// Token: 0x0600788A RID: 30858 RVA: 0x002E5AFB File Offset: 0x002E3CFB
	public void SetWarningActve(bool state)
	{
		this.WarningNotification.SetActive(state);
	}

	// Token: 0x0600788B RID: 30859 RVA: 0x002E5B09 File Offset: 0x002E3D09
	public void SetWarning(Sprite icon, string label)
	{
		this.SetWarningActve(true);
		this.NotificationIcon.sprite = icon;
		this.NotificationText.text = label;
	}

	// Token: 0x0600788C RID: 30860 RVA: 0x002E5B2A File Offset: 0x002E3D2A
	public void SetPortrait(GameObject target)
	{
		this.portrait.SetPortrait(target);
	}

	// Token: 0x040053FD RID: 21501
	public LocText titleText;

	// Token: 0x040053FE RID: 21502
	public LocText subtextText;

	// Token: 0x040053FF RID: 21503
	public GameObject WarningNotification;

	// Token: 0x04005400 RID: 21504
	public Text NotificationText;

	// Token: 0x04005401 RID: 21505
	public Image NotificationIcon;

	// Token: 0x04005402 RID: 21506
	public Sprite techIcon;

	// Token: 0x04005403 RID: 21507
	public Sprite materialIcon;

	// Token: 0x04005404 RID: 21508
	public TitleBarPortrait portrait;

	// Token: 0x04005405 RID: 21509
	public bool userEditable;

	// Token: 0x04005406 RID: 21510
	public bool setCameraControllerState = true;
}
