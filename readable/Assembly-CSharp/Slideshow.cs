using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E9A RID: 3738
[AddComponentMenu("KMonoBehaviour/scripts/Slideshow")]
public class Slideshow : KMonoBehaviour
{
	// Token: 0x06007786 RID: 30598 RVA: 0x002DD448 File Offset: 0x002DB648
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.timeUntilNextSlide = this.timePerSlide;
		if (this.transparentIfEmpty && this.sprites != null && this.sprites.Length == 0)
		{
			this.imageTarget.color = Color.clear;
		}
		if (this.isExpandable)
		{
			this.button = base.GetComponent<KButton>();
			this.button.onClick += delegate()
			{
				if (this.onBeforePlay != null)
				{
					this.onBeforePlay();
				}
				SlideshowUpdateType slideshowUpdateType = this.updateType;
				if (slideshowUpdateType == SlideshowUpdateType.preloadedSprites)
				{
					VideoScreen.Instance.PlaySlideShow(this.sprites);
					return;
				}
				if (slideshowUpdateType != SlideshowUpdateType.loadOnDemand)
				{
					return;
				}
				VideoScreen.Instance.PlaySlideShow(this.files);
			};
		}
		if (this.nextButton != null)
		{
			this.nextButton.onClick += delegate()
			{
				this.nextSlide();
			};
		}
		if (this.prevButton != null)
		{
			this.prevButton.onClick += delegate()
			{
				this.prevSlide();
			};
		}
		if (this.pauseButton != null)
		{
			this.pauseButton.onClick += delegate()
			{
				this.SetPaused(!this.paused);
			};
		}
		if (this.closeButton != null)
		{
			this.closeButton.onClick += delegate()
			{
				VideoScreen.Instance.Stop();
				if (this.onEndingPlay != null)
				{
					this.onEndingPlay();
				}
			};
		}
	}

	// Token: 0x06007787 RID: 30599 RVA: 0x002DD550 File Offset: 0x002DB750
	public void SetPaused(bool state)
	{
		this.paused = state;
		if (this.pauseIcon != null)
		{
			this.pauseIcon.gameObject.SetActive(!this.paused);
		}
		if (this.unpauseIcon != null)
		{
			this.unpauseIcon.gameObject.SetActive(this.paused);
		}
		if (this.prevButton != null)
		{
			this.prevButton.gameObject.SetActive(this.paused);
		}
		if (this.nextButton != null)
		{
			this.nextButton.gameObject.SetActive(this.paused);
		}
	}

	// Token: 0x06007788 RID: 30600 RVA: 0x002DD5F8 File Offset: 0x002DB7F8
	private void resetSlide(bool enable)
	{
		this.timeUntilNextSlide = this.timePerSlide;
		this.currentSlide = 0;
		if (enable)
		{
			this.imageTarget.color = Color.white;
			return;
		}
		if (this.transparentIfEmpty)
		{
			this.imageTarget.color = Color.clear;
		}
	}

	// Token: 0x06007789 RID: 30601 RVA: 0x002DD644 File Offset: 0x002DB844
	private Sprite loadSlide(string file)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		Texture2D texture2D = new Texture2D(512, 768);
		texture2D.filterMode = FilterMode.Point;
		texture2D.LoadImage(File.ReadAllBytes(file));
		return Sprite.Create(texture2D, new Rect(Vector2.zero, new Vector2((float)texture2D.width, (float)texture2D.height)), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect);
	}

	// Token: 0x0600778A RID: 30602 RVA: 0x002DD6B4 File Offset: 0x002DB8B4
	public void SetFiles(string[] files, int loadFrame = -1)
	{
		if (files == null)
		{
			return;
		}
		this.files = files;
		bool flag = files.Length != 0 && files[0] != null;
		this.resetSlide(flag);
		if (flag)
		{
			int num = (loadFrame != -1) ? loadFrame : (files.Length - 1);
			string file = files[num];
			Sprite slide = this.loadSlide(file);
			this.setSlide(slide);
			this.currentSlideImage = slide;
		}
	}

	// Token: 0x0600778B RID: 30603 RVA: 0x002DD70C File Offset: 0x002DB90C
	public void updateSize(Sprite sprite)
	{
		Vector2 fittedSize = this.GetFittedSize(sprite, 960f, 960f);
		base.GetComponent<RectTransform>().sizeDelta = fittedSize;
	}

	// Token: 0x0600778C RID: 30604 RVA: 0x002DD737 File Offset: 0x002DB937
	public void SetSprites(Sprite[] sprites)
	{
		if (sprites == null)
		{
			return;
		}
		this.sprites = sprites;
		this.resetSlide(sprites.Length != 0 && sprites[0] != null);
		if (sprites.Length != 0 && sprites[0] != null)
		{
			this.setSlide(sprites[0]);
		}
	}

	// Token: 0x0600778D RID: 30605 RVA: 0x002DD774 File Offset: 0x002DB974
	public Vector2 GetFittedSize(Sprite sprite, float maxWidth, float maxHeight)
	{
		if (sprite == null || sprite.texture == null)
		{
			return Vector2.zero;
		}
		int width = sprite.texture.width;
		int height = sprite.texture.height;
		float num = maxWidth / (float)width;
		float num2 = maxHeight / (float)height;
		if (num < num2)
		{
			return new Vector2((float)width * num, (float)height * num);
		}
		return new Vector2((float)width * num2, (float)height * num2);
	}

	// Token: 0x0600778E RID: 30606 RVA: 0x002DD7DF File Offset: 0x002DB9DF
	public void setSlide(Sprite slide)
	{
		if (slide == null)
		{
			return;
		}
		this.imageTarget.texture = slide.texture;
		this.updateSize(slide);
	}

	// Token: 0x0600778F RID: 30607 RVA: 0x002DD803 File Offset: 0x002DBA03
	public void nextSlide()
	{
		this.setSlideIndex(this.currentSlide + 1);
	}

	// Token: 0x06007790 RID: 30608 RVA: 0x002DD813 File Offset: 0x002DBA13
	public void prevSlide()
	{
		this.setSlideIndex(this.currentSlide - 1);
	}

	// Token: 0x06007791 RID: 30609 RVA: 0x002DD824 File Offset: 0x002DBA24
	private void setSlideIndex(int slideIndex)
	{
		this.timeUntilNextSlide = this.timePerSlide;
		SlideshowUpdateType slideshowUpdateType = this.updateType;
		if (slideshowUpdateType != SlideshowUpdateType.preloadedSprites)
		{
			if (slideshowUpdateType != SlideshowUpdateType.loadOnDemand)
			{
				return;
			}
			if (slideIndex < 0)
			{
				slideIndex = this.files.Length + slideIndex;
			}
			this.currentSlide = slideIndex % this.files.Length;
			if (this.currentSlide == this.files.Length - 1)
			{
				this.timeUntilNextSlide *= this.timeFactorForLastSlide;
			}
			if (this.playInThumbnail)
			{
				if (this.currentSlideImage != null)
				{
					UnityEngine.Object.Destroy(this.currentSlideImage.texture);
					UnityEngine.Object.Destroy(this.currentSlideImage);
					GC.Collect();
				}
				this.currentSlideImage = this.loadSlide(this.files[this.currentSlide]);
				this.setSlide(this.currentSlideImage);
			}
		}
		else
		{
			if (slideIndex < 0)
			{
				slideIndex = this.sprites.Length + slideIndex;
			}
			this.currentSlide = slideIndex % this.sprites.Length;
			if (this.currentSlide == this.sprites.Length - 1)
			{
				this.timeUntilNextSlide *= this.timeFactorForLastSlide;
			}
			if (this.playInThumbnail)
			{
				this.setSlide(this.sprites[this.currentSlide]);
				return;
			}
		}
	}

	// Token: 0x06007792 RID: 30610 RVA: 0x002DD950 File Offset: 0x002DBB50
	private void Update()
	{
		if (this.updateType == SlideshowUpdateType.preloadedSprites && (this.sprites == null || this.sprites.Length == 0))
		{
			return;
		}
		if (this.updateType == SlideshowUpdateType.loadOnDemand && (this.files == null || this.files.Length == 0))
		{
			return;
		}
		if (this.paused)
		{
			return;
		}
		this.timeUntilNextSlide -= Time.unscaledDeltaTime;
		if (this.timeUntilNextSlide <= 0f)
		{
			this.nextSlide();
		}
	}

	// Token: 0x0400530F RID: 21263
	public RawImage imageTarget;

	// Token: 0x04005310 RID: 21264
	private string[] files;

	// Token: 0x04005311 RID: 21265
	private Sprite currentSlideImage;

	// Token: 0x04005312 RID: 21266
	private Sprite[] sprites;

	// Token: 0x04005313 RID: 21267
	public float timePerSlide = 1f;

	// Token: 0x04005314 RID: 21268
	public float timeFactorForLastSlide = 3f;

	// Token: 0x04005315 RID: 21269
	private int currentSlide;

	// Token: 0x04005316 RID: 21270
	private float timeUntilNextSlide;

	// Token: 0x04005317 RID: 21271
	private bool paused;

	// Token: 0x04005318 RID: 21272
	public bool playInThumbnail;

	// Token: 0x04005319 RID: 21273
	public SlideshowUpdateType updateType;

	// Token: 0x0400531A RID: 21274
	[SerializeField]
	private bool isExpandable;

	// Token: 0x0400531B RID: 21275
	[SerializeField]
	private KButton button;

	// Token: 0x0400531C RID: 21276
	[SerializeField]
	private bool transparentIfEmpty = true;

	// Token: 0x0400531D RID: 21277
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400531E RID: 21278
	[SerializeField]
	private KButton prevButton;

	// Token: 0x0400531F RID: 21279
	[SerializeField]
	private KButton nextButton;

	// Token: 0x04005320 RID: 21280
	[SerializeField]
	private KButton pauseButton;

	// Token: 0x04005321 RID: 21281
	[SerializeField]
	private Image pauseIcon;

	// Token: 0x04005322 RID: 21282
	[SerializeField]
	private Image unpauseIcon;

	// Token: 0x04005323 RID: 21283
	public Slideshow.onBeforeAndEndPlayDelegate onBeforePlay;

	// Token: 0x04005324 RID: 21284
	public Slideshow.onBeforeAndEndPlayDelegate onEndingPlay;

	// Token: 0x02002106 RID: 8454
	// (Invoke) Token: 0x0600BB26 RID: 47910
	public delegate void onBeforeAndEndPlayDelegate();
}
