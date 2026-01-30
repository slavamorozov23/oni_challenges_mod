using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000D92 RID: 3474
public class GenericMessage : Message
{
	// Token: 0x06006C2D RID: 27693 RVA: 0x00290D82 File Offset: 0x0028EF82
	public GenericMessage(string _title, string _body, string _tooltip, KMonoBehaviour click_focus = null)
	{
		this.title = _title;
		this.body = _body;
		this.tooltip = _tooltip;
		this.clickFocus.Set(click_focus);
	}

	// Token: 0x06006C2E RID: 27694 RVA: 0x00290DB7 File Offset: 0x0028EFB7
	public GenericMessage()
	{
	}

	// Token: 0x06006C2F RID: 27695 RVA: 0x00290DCA File Offset: 0x0028EFCA
	public override string GetSound()
	{
		return null;
	}

	// Token: 0x06006C30 RID: 27696 RVA: 0x00290DCD File Offset: 0x0028EFCD
	public override string GetMessageBody()
	{
		return this.body;
	}

	// Token: 0x06006C31 RID: 27697 RVA: 0x00290DD5 File Offset: 0x0028EFD5
	public override string GetTooltip()
	{
		return this.tooltip;
	}

	// Token: 0x06006C32 RID: 27698 RVA: 0x00290DDD File Offset: 0x0028EFDD
	public override string GetTitle()
	{
		return this.title;
	}

	// Token: 0x06006C33 RID: 27699 RVA: 0x00290DE8 File Offset: 0x0028EFE8
	public override void OnClick()
	{
		KMonoBehaviour kmonoBehaviour = this.clickFocus.Get();
		if (kmonoBehaviour == null)
		{
			return;
		}
		Transform transform = kmonoBehaviour.transform;
		if (transform == null)
		{
			return;
		}
		Vector3 position = transform.GetPosition();
		position.z = -40f;
		CameraController.Instance.SetTargetPos(position, 8f, true);
		if (transform.GetComponent<KSelectable>() != null)
		{
			SelectTool.Instance.Select(transform.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x04004A28 RID: 18984
	[Serialize]
	private string title;

	// Token: 0x04004A29 RID: 18985
	[Serialize]
	private string tooltip;

	// Token: 0x04004A2A RID: 18986
	[Serialize]
	private string body;

	// Token: 0x04004A2B RID: 18987
	[Serialize]
	private Ref<KMonoBehaviour> clickFocus = new Ref<KMonoBehaviour>();
}
