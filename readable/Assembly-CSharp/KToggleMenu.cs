using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D3D RID: 3389
public class KToggleMenu : KScreen
{
	// Token: 0x1400002B RID: 43
	// (add) Token: 0x060068DD RID: 26845 RVA: 0x0027B3AC File Offset: 0x002795AC
	// (remove) Token: 0x060068DE RID: 26846 RVA: 0x0027B3E4 File Offset: 0x002795E4
	public event KToggleMenu.OnSelect onSelect;

	// Token: 0x060068DF RID: 26847 RVA: 0x0027B419 File Offset: 0x00279619
	public void Setup(IList<KToggleMenu.ToggleInfo> toggleInfo)
	{
		this.toggleInfo = toggleInfo;
		this.RefreshButtons();
	}

	// Token: 0x060068E0 RID: 26848 RVA: 0x0027B428 File Offset: 0x00279628
	protected void Setup()
	{
		this.RefreshButtons();
	}

	// Token: 0x060068E1 RID: 26849 RVA: 0x0027B430 File Offset: 0x00279630
	private void RefreshButtons()
	{
		foreach (KToggle ktoggle in this.toggles)
		{
			if (ktoggle != null)
			{
				UnityEngine.Object.Destroy(ktoggle.gameObject);
			}
		}
		this.toggles.Clear();
		if (this.toggleInfo == null)
		{
			return;
		}
		Transform parent = (this.toggleParent != null) ? this.toggleParent : base.transform;
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			int idx = i;
			KToggleMenu.ToggleInfo toggleInfo = this.toggleInfo[i];
			if (toggleInfo == null)
			{
				this.toggles.Add(null);
			}
			else
			{
				KToggle ktoggle2 = UnityEngine.Object.Instantiate<KToggle>(this.prefab, Vector3.zero, Quaternion.identity);
				ktoggle2.gameObject.name = "Toggle:" + toggleInfo.text;
				ktoggle2.transform.SetParent(parent, false);
				ktoggle2.group = this.group;
				ktoggle2.onClick += delegate()
				{
					this.OnClick(idx);
				};
				ktoggle2.GetComponentsInChildren<Text>(true)[0].text = toggleInfo.text;
				toggleInfo.toggle = ktoggle2;
				this.toggles.Add(ktoggle2);
			}
		}
	}

	// Token: 0x060068E2 RID: 26850 RVA: 0x0027B5A8 File Offset: 0x002797A8
	public int GetSelected()
	{
		return KToggleMenu.selected;
	}

	// Token: 0x060068E3 RID: 26851 RVA: 0x0027B5AF File Offset: 0x002797AF
	private void OnClick(int i)
	{
		UISounds.PlaySound(UISounds.Sound.ClickObject);
		if (this.onSelect == null)
		{
			return;
		}
		this.onSelect(this.toggleInfo[i]);
	}

	// Token: 0x060068E4 RID: 26852 RVA: 0x0027B5D8 File Offset: 0x002797D8
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.toggles == null)
		{
			return;
		}
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			global::Action hotKey = this.toggleInfo[i].hotKey;
			if (hotKey != global::Action.NumActions && e.TryConsume(hotKey))
			{
				this.toggles[i].Click();
				return;
			}
		}
	}

	// Token: 0x04004809 RID: 18441
	[SerializeField]
	private Transform toggleParent;

	// Token: 0x0400480A RID: 18442
	[SerializeField]
	private KToggle prefab;

	// Token: 0x0400480B RID: 18443
	[SerializeField]
	private ToggleGroup group;

	// Token: 0x0400480D RID: 18445
	protected IList<KToggleMenu.ToggleInfo> toggleInfo;

	// Token: 0x0400480E RID: 18446
	protected List<KToggle> toggles = new List<KToggle>();

	// Token: 0x0400480F RID: 18447
	private static int selected = -1;

	// Token: 0x02001F6E RID: 8046
	// (Invoke) Token: 0x0600B64D RID: 46669
	public delegate void OnSelect(KToggleMenu.ToggleInfo toggleInfo);

	// Token: 0x02001F6F RID: 8047
	public class ToggleInfo
	{
		// Token: 0x0600B650 RID: 46672 RVA: 0x003EFD2A File Offset: 0x003EDF2A
		public ToggleInfo(string text, object user_data = null, global::Action hotKey = global::Action.NumActions)
		{
			this.text = text;
			this.userData = user_data;
			this.hotKey = hotKey;
		}

		// Token: 0x040092DD RID: 37597
		public string text;

		// Token: 0x040092DE RID: 37598
		public object userData;

		// Token: 0x040092DF RID: 37599
		public KToggle toggle;

		// Token: 0x040092E0 RID: 37600
		public global::Action hotKey;
	}
}
