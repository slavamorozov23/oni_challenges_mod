using System;
using UnityEngine;

// Token: 0x02000D0E RID: 3342
public class FoldOutPanel : KMonoBehaviour
{
	// Token: 0x0600676A RID: 26474 RVA: 0x0026FF54 File Offset: 0x0026E154
	protected override void OnSpawn()
	{
		MultiToggle componentInChildren = base.GetComponentInChildren<MultiToggle>();
		componentInChildren.onClick = (System.Action)Delegate.Combine(componentInChildren.onClick, new System.Action(this.OnClick));
		this.ToggleOpen(this.startOpen);
	}

	// Token: 0x0600676B RID: 26475 RVA: 0x0026FF89 File Offset: 0x0026E189
	private void OnClick()
	{
		this.ToggleOpen(!this.panelOpen);
	}

	// Token: 0x0600676C RID: 26476 RVA: 0x0026FF9A File Offset: 0x0026E19A
	private void ToggleOpen(bool open)
	{
		this.panelOpen = open;
		this.container.SetActive(this.panelOpen);
		base.GetComponentInChildren<MultiToggle>().ChangeState(this.panelOpen ? 1 : 0);
	}

	// Token: 0x040046DC RID: 18140
	private bool panelOpen = true;

	// Token: 0x040046DD RID: 18141
	public GameObject container;

	// Token: 0x040046DE RID: 18142
	public bool startOpen = true;
}
