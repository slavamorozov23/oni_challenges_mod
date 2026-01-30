using System;
using UnityEngine;

// Token: 0x02000D96 RID: 3478
public class MessageDialogFrame : KScreen
{
	// Token: 0x06006C4D RID: 27725 RVA: 0x00290F61 File Offset: 0x0028F161
	public override float GetSortKey()
	{
		return 15f;
	}

	// Token: 0x06006C4E RID: 27726 RVA: 0x00290F68 File Offset: 0x0028F168
	protected override void OnActivate()
	{
		this.closeButton.onClick += this.OnClickClose;
		this.nextMessageButton.onClick += this.OnClickNextMessage;
		MultiToggle multiToggle = this.dontShowAgainButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnClickDontShowAgain));
		bool flag = KPlayerPrefs.GetInt("HideTutorial_CheckState", 0) == 1;
		this.dontShowAgainButton.ChangeState(flag ? 0 : 1);
		base.Subscribe(Messenger.Instance.gameObject, -599791736, new Action<object>(this.OnMessagesChanged));
		this.OnMessagesChanged(null);
	}

	// Token: 0x06006C4F RID: 27727 RVA: 0x00291014 File Offset: 0x0028F214
	protected override void OnDeactivate()
	{
		base.Unsubscribe(Messenger.Instance.gameObject, -599791736, new Action<object>(this.OnMessagesChanged));
	}

	// Token: 0x06006C50 RID: 27728 RVA: 0x00291037 File Offset: 0x0028F237
	private void OnClickClose()
	{
		this.TryDontShowAgain();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06006C51 RID: 27729 RVA: 0x0029104A File Offset: 0x0028F24A
	private void OnClickNextMessage()
	{
		this.TryDontShowAgain();
		UnityEngine.Object.Destroy(base.gameObject);
		NotificationScreen.Instance.OnClickNextMessage();
	}

	// Token: 0x06006C52 RID: 27730 RVA: 0x00291068 File Offset: 0x0028F268
	private void OnClickDontShowAgain()
	{
		this.dontShowAgainButton.NextState();
		bool flag = this.dontShowAgainButton.CurrentState == 0;
		KPlayerPrefs.SetInt("HideTutorial_CheckState", flag ? 1 : 0);
	}

	// Token: 0x06006C53 RID: 27731 RVA: 0x002910A0 File Offset: 0x0028F2A0
	private void OnMessagesChanged(object data)
	{
		this.nextMessageButton.gameObject.SetActive(Messenger.Instance.Count != 0);
	}

	// Token: 0x06006C54 RID: 27732 RVA: 0x002910C0 File Offset: 0x0028F2C0
	public void SetMessage(MessageDialog dialog, Message message)
	{
		this.title.text = message.GetTitle().ToUpper();
		dialog.GetComponent<RectTransform>().SetParent(this.body.GetComponent<RectTransform>());
		RectTransform component = dialog.GetComponent<RectTransform>();
		component.offsetMin = Vector2.zero;
		component.offsetMax = Vector2.zero;
		dialog.transform.SetLocalPosition(Vector3.zero);
		dialog.SetMessage(message);
		dialog.OnClickAction();
		if (dialog.CanDontShowAgain)
		{
			this.dontShowAgainElement.SetActive(true);
			this.dontShowAgainDelegate = new System.Action(dialog.OnDontShowAgain);
			return;
		}
		this.dontShowAgainElement.SetActive(false);
		this.dontShowAgainDelegate = null;
	}

	// Token: 0x06006C55 RID: 27733 RVA: 0x0029116D File Offset: 0x0028F36D
	private void TryDontShowAgain()
	{
		if (this.dontShowAgainDelegate != null && this.dontShowAgainButton.CurrentState == 0)
		{
			this.dontShowAgainDelegate();
		}
	}

	// Token: 0x04004A2D RID: 18989
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004A2E RID: 18990
	[SerializeField]
	private KToggle nextMessageButton;

	// Token: 0x04004A2F RID: 18991
	[SerializeField]
	private GameObject dontShowAgainElement;

	// Token: 0x04004A30 RID: 18992
	[SerializeField]
	private MultiToggle dontShowAgainButton;

	// Token: 0x04004A31 RID: 18993
	[SerializeField]
	private LocText title;

	// Token: 0x04004A32 RID: 18994
	[SerializeField]
	private RectTransform body;

	// Token: 0x04004A33 RID: 18995
	private System.Action dontShowAgainDelegate;
}
