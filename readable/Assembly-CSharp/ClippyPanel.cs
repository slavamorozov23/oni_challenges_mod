using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000EC9 RID: 3785
public class ClippyPanel : KScreen
{
	// Token: 0x06007939 RID: 31033 RVA: 0x002E9614 File Offset: 0x002E7814
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600793A RID: 31034 RVA: 0x002E961C File Offset: 0x002E781C
	protected override void OnActivate()
	{
		base.OnActivate();
		SpeedControlScreen.Instance.Pause(true, false);
		Game.Instance.Trigger(1634669191, null);
	}

	// Token: 0x0600793B RID: 31035 RVA: 0x002E9640 File Offset: 0x002E7840
	public void OnOk()
	{
		SpeedControlScreen.Instance.Unpause(true);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04005496 RID: 21654
	public Text title;

	// Token: 0x04005497 RID: 21655
	public Text detailText;

	// Token: 0x04005498 RID: 21656
	public Text flavorText;

	// Token: 0x04005499 RID: 21657
	public Image topicIcon;

	// Token: 0x0400549A RID: 21658
	private KButton okButton;
}
