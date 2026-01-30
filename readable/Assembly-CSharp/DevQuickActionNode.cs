using System;
using TMPro;
using UnityEngine;

// Token: 0x02000686 RID: 1670
public class DevQuickActionNode : MonoBehaviour
{
	// Token: 0x170001FE RID: 510
	// (get) Token: 0x06002927 RID: 10535 RVA: 0x000EAA1B File Offset: 0x000E8C1B
	public new RectTransform transform
	{
		get
		{
			return base.transform as RectTransform;
		}
	}

	// Token: 0x06002928 RID: 10536 RVA: 0x000EAA28 File Offset: 0x000E8C28
	public void SetChildrenSeparationSpace(float space)
	{
		this.space = space;
	}

	// Token: 0x06002929 RID: 10537 RVA: 0x000EAA31 File Offset: 0x000E8C31
	public virtual void Recycle()
	{
		this.parentNode = null;
		this.OnNodeInteractedWith = null;
		base.gameObject.SetActive(false);
		Action<DevQuickActionNode> onRecycle = this.OnRecycle;
		if (onRecycle == null)
		{
			return;
		}
		onRecycle(this);
	}

	// Token: 0x04001849 RID: 6217
	public TextMeshProUGUI label;

	// Token: 0x0400184A RID: 6218
	protected DevQuickActionNode parentNode;

	// Token: 0x0400184B RID: 6219
	public Action<DevQuickActionNode> OnRecycle;

	// Token: 0x0400184C RID: 6220
	protected System.Action OnNodeInteractedWith;

	// Token: 0x0400184D RID: 6221
	protected float space = 100f;
}
