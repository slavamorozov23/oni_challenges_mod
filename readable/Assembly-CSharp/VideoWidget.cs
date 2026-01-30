using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x02000EC4 RID: 3780
[AddComponentMenu("KMonoBehaviour/scripts/VideoWidget")]
public class VideoWidget : KMonoBehaviour
{
	// Token: 0x06007914 RID: 30996 RVA: 0x002E8A5B File Offset: 0x002E6C5B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.button.onClick += this.Clicked;
		this.rawImage = this.thumbnailPlayer.GetComponent<RawImage>();
	}

	// Token: 0x06007915 RID: 30997 RVA: 0x002E8A8C File Offset: 0x002E6C8C
	private void Clicked()
	{
		VideoScreen.Instance.PlayVideo(this.clip, false, default(EventReference), false, true);
		if (!string.IsNullOrEmpty(this.overlayName))
		{
			VideoScreen.Instance.SetOverlayText(this.overlayName, this.texts);
		}
	}

	// Token: 0x06007916 RID: 30998 RVA: 0x002E8AD8 File Offset: 0x002E6CD8
	public void SetClip(VideoClip clip, string overlayName = null, List<string> texts = null)
	{
		if (clip == null)
		{
			global::Debug.LogWarning("Tried to assign null video clip to VideoWidget");
			return;
		}
		this.clip = clip;
		this.overlayName = overlayName;
		this.texts = texts;
		this.renderTexture = new RenderTexture(Convert.ToInt32(clip.width), Convert.ToInt32(clip.height), 16);
		this.thumbnailPlayer.targetTexture = this.renderTexture;
		this.rawImage.texture = this.renderTexture;
		base.StartCoroutine(this.ConfigureThumbnail());
	}

	// Token: 0x06007917 RID: 30999 RVA: 0x002E8B60 File Offset: 0x002E6D60
	private IEnumerator ConfigureThumbnail()
	{
		this.thumbnailPlayer.audioOutputMode = VideoAudioOutputMode.None;
		this.thumbnailPlayer.clip = this.clip;
		this.thumbnailPlayer.time = 0.0;
		this.thumbnailPlayer.Play();
		yield return null;
		yield break;
	}

	// Token: 0x06007918 RID: 31000 RVA: 0x002E8B6F File Offset: 0x002E6D6F
	private void Update()
	{
		if (this.thumbnailPlayer.isPlaying && this.thumbnailPlayer.time > 2.0)
		{
			this.thumbnailPlayer.Pause();
		}
	}

	// Token: 0x0400546B RID: 21611
	[SerializeField]
	private VideoClip clip;

	// Token: 0x0400546C RID: 21612
	[SerializeField]
	private VideoPlayer thumbnailPlayer;

	// Token: 0x0400546D RID: 21613
	[SerializeField]
	private KButton button;

	// Token: 0x0400546E RID: 21614
	[SerializeField]
	private string overlayName;

	// Token: 0x0400546F RID: 21615
	[SerializeField]
	private List<string> texts;

	// Token: 0x04005470 RID: 21616
	private RenderTexture renderTexture;

	// Token: 0x04005471 RID: 21617
	private RawImage rawImage;
}
