using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x02000DA2 RID: 3490
public class TutorialMessageDialog : MessageDialog
{
	// Token: 0x170007B5 RID: 1973
	// (get) Token: 0x06006CA1 RID: 27809 RVA: 0x00291C8F File Offset: 0x0028FE8F
	public override bool CanDontShowAgain
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06006CA2 RID: 27810 RVA: 0x00291C92 File Offset: 0x0028FE92
	public override bool CanDisplay(Message message)
	{
		return typeof(TutorialMessage).IsAssignableFrom(message.GetType());
	}

	// Token: 0x06006CA3 RID: 27811 RVA: 0x00291CAC File Offset: 0x0028FEAC
	public override void SetMessage(Message base_message)
	{
		this.message = (base_message as TutorialMessage);
		this.description.text = this.message.GetMessageBody();
		if (!string.IsNullOrEmpty(this.message.videoClipId))
		{
			VideoClip video = Assets.GetVideo(this.message.videoClipId);
			this.SetVideo(video, this.message.videoOverlayName, this.message.videoTitleText);
		}
	}

	// Token: 0x06006CA4 RID: 27812 RVA: 0x00291D1C File Offset: 0x0028FF1C
	public void SetVideo(VideoClip clip, string overlayName, string titleText)
	{
		if (this.videoWidget == null)
		{
			this.videoWidget = Util.KInstantiateUI(this.videoWidgetPrefab, base.transform.gameObject, true).GetComponent<VideoWidget>();
			this.videoWidget.transform.SetAsFirstSibling();
		}
		this.videoWidget.SetClip(clip, overlayName, new List<string>
		{
			titleText,
			VIDEOS.TUTORIAL_HEADER
		});
	}

	// Token: 0x06006CA5 RID: 27813 RVA: 0x00291D92 File Offset: 0x0028FF92
	public override void OnClickAction()
	{
	}

	// Token: 0x06006CA6 RID: 27814 RVA: 0x00291D94 File Offset: 0x0028FF94
	public override void OnDontShowAgain()
	{
		Tutorial.Instance.HideTutorialMessage(this.message.messageId);
	}

	// Token: 0x04004A55 RID: 19029
	[SerializeField]
	private LocText description;

	// Token: 0x04004A56 RID: 19030
	private TutorialMessage message;

	// Token: 0x04004A57 RID: 19031
	[SerializeField]
	private GameObject videoWidgetPrefab;

	// Token: 0x04004A58 RID: 19032
	private VideoWidget videoWidget;
}
