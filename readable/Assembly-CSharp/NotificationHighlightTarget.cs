using System;

// Token: 0x02000DC6 RID: 3526
public class NotificationHighlightTarget : KMonoBehaviour
{
	// Token: 0x06006E2C RID: 28204 RVA: 0x0029B949 File Offset: 0x00299B49
	protected void OnEnable()
	{
		this.controller = base.GetComponentInParent<NotificationHighlightController>();
		if (this.controller != null)
		{
			this.controller.AddTarget(this);
		}
	}

	// Token: 0x06006E2D RID: 28205 RVA: 0x0029B971 File Offset: 0x00299B71
	protected override void OnDisable()
	{
		if (this.controller != null)
		{
			this.controller.RemoveTarget(this);
		}
	}

	// Token: 0x06006E2E RID: 28206 RVA: 0x0029B98D File Offset: 0x00299B8D
	public void View()
	{
		base.GetComponentInParent<NotificationHighlightController>().TargetViewed(this);
	}

	// Token: 0x04004B47 RID: 19271
	public string targetKey;

	// Token: 0x04004B48 RID: 19272
	private NotificationHighlightController controller;
}
