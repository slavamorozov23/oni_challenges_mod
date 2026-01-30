using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D3C RID: 3388
public class KPopupMenu : KScreen
{
	// Token: 0x060068D8 RID: 26840 RVA: 0x0027B2A8 File Offset: 0x002794A8
	public void SetOptions(IList<string> options)
	{
		List<KButtonMenu.ButtonInfo> list = new List<KButtonMenu.ButtonInfo>();
		for (int i = 0; i < options.Count; i++)
		{
			int index = i;
			string option = options[i];
			list.Add(new KButtonMenu.ButtonInfo(option, global::Action.NumActions, delegate()
			{
				this.SelectOption(option, index);
			}, null, null));
		}
		this.Buttons = list.ToArray();
	}

	// Token: 0x060068D9 RID: 26841 RVA: 0x0027B320 File Offset: 0x00279520
	public void OnClick()
	{
		if (this.Buttons != null)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.buttonMenu.SetButtons(this.Buttons);
			this.buttonMenu.RefreshButtons();
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x060068DA RID: 26842 RVA: 0x0027B377 File Offset: 0x00279577
	public void SelectOption(string option, int index)
	{
		if (this.OnSelect != null)
		{
			this.OnSelect(option, index);
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x060068DB RID: 26843 RVA: 0x0027B39A File Offset: 0x0027959A
	public IList<KButtonMenu.ButtonInfo> GetButtons()
	{
		return this.Buttons;
	}

	// Token: 0x04004806 RID: 18438
	[SerializeField]
	private KButtonMenu buttonMenu;

	// Token: 0x04004807 RID: 18439
	private KButtonMenu.ButtonInfo[] Buttons;

	// Token: 0x04004808 RID: 18440
	public Action<string, int> OnSelect;
}
