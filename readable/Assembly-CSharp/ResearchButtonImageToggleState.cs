using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C71 RID: 3185
public class ResearchButtonImageToggleState : ImageToggleState
{
	// Token: 0x06006109 RID: 24841 RVA: 0x0023B14C File Offset: 0x0023934C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Research.Instance.Subscribe(-1914338957, new Action<object>(this.UpdateActiveResearch));
		Research.Instance.Subscribe(-125623018, new Action<object>(this.RefreshProgressBar));
		this.toggle = base.GetComponent<KToggle>();
	}

	// Token: 0x0600610A RID: 24842 RVA: 0x0023B1A3 File Offset: 0x002393A3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateActiveResearch(null);
		this.RestartCoroutine();
	}

	// Token: 0x0600610B RID: 24843 RVA: 0x0023B1B8 File Offset: 0x002393B8
	protected override void OnCleanUp()
	{
		this.AbortCoroutine();
		Research.Instance.Unsubscribe(-1914338957, new Action<object>(this.UpdateActiveResearch));
		Research.Instance.Unsubscribe(-125623018, new Action<object>(this.RefreshProgressBar));
		base.OnCleanUp();
	}

	// Token: 0x0600610C RID: 24844 RVA: 0x0023B207 File Offset: 0x00239407
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RestartCoroutine();
	}

	// Token: 0x0600610D RID: 24845 RVA: 0x0023B215 File Offset: 0x00239415
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.AbortCoroutine();
	}

	// Token: 0x0600610E RID: 24846 RVA: 0x0023B223 File Offset: 0x00239423
	private void AbortCoroutine()
	{
		if (this.scrollIconCoroutine != null)
		{
			base.StopCoroutine(this.scrollIconCoroutine);
		}
		this.scrollIconCoroutine = null;
	}

	// Token: 0x0600610F RID: 24847 RVA: 0x0023B240 File Offset: 0x00239440
	private void RestartCoroutine()
	{
		this.AbortCoroutine();
		if (base.gameObject.activeInHierarchy)
		{
			this.scrollIconCoroutine = base.StartCoroutine(this.ScrollIcon());
		}
	}

	// Token: 0x06006110 RID: 24848 RVA: 0x0023B268 File Offset: 0x00239468
	private void UpdateActiveResearch(object o)
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch == null)
		{
			this.currentResearchIcons = null;
		}
		else
		{
			this.currentResearchIcons = new Sprite[activeResearch.tech.unlockedItems.Count];
			for (int i = 0; i < activeResearch.tech.unlockedItems.Count; i++)
			{
				TechItem techItem = activeResearch.tech.unlockedItems[i];
				this.currentResearchIcons[i] = techItem.UISprite();
			}
		}
		this.ResetCoroutineTimers();
		this.RefreshProgressBar(o);
	}

	// Token: 0x06006111 RID: 24849 RVA: 0x0023B2F0 File Offset: 0x002394F0
	public void RefreshProgressBar(object o)
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch == null)
		{
			this.progressBar.fillAmount = 0f;
			return;
		}
		this.progressBar.fillAmount = activeResearch.GetTotalPercentageComplete();
	}

	// Token: 0x06006112 RID: 24850 RVA: 0x0023B32D File Offset: 0x0023952D
	public void SetProgressBarVisibility(bool viisble)
	{
		this.progressBar.enabled = viisble;
	}

	// Token: 0x06006113 RID: 24851 RVA: 0x0023B33B File Offset: 0x0023953B
	public override void SetActive()
	{
		base.SetActive();
		this.SetProgressBarVisibility(false);
	}

	// Token: 0x06006114 RID: 24852 RVA: 0x0023B34A File Offset: 0x0023954A
	public override void SetDisabledActive()
	{
		base.SetDisabledActive();
		this.SetProgressBarVisibility(false);
	}

	// Token: 0x06006115 RID: 24853 RVA: 0x0023B359 File Offset: 0x00239559
	public override void SetDisabled()
	{
		base.SetDisabled();
		this.SetProgressBarVisibility(false);
	}

	// Token: 0x06006116 RID: 24854 RVA: 0x0023B368 File Offset: 0x00239568
	public override void SetInactive()
	{
		base.SetInactive();
		this.SetProgressBarVisibility(true);
		this.RefreshProgressBar(null);
	}

	// Token: 0x06006117 RID: 24855 RVA: 0x0023B37E File Offset: 0x0023957E
	private void ResetCoroutineTimers()
	{
		this.mainIconScreenTime = 0f;
		this.itemScreenTime = 0f;
		this.item_idx = -1;
	}

	// Token: 0x17000707 RID: 1799
	// (get) Token: 0x06006118 RID: 24856 RVA: 0x0023B39D File Offset: 0x0023959D
	private bool ReadyToDisplayIcons
	{
		get
		{
			return this.progressBar.enabled && this.currentResearchIcons != null && this.item_idx >= 0 && this.item_idx < this.currentResearchIcons.Length;
		}
	}

	// Token: 0x06006119 RID: 24857 RVA: 0x0023B3CF File Offset: 0x002395CF
	private IEnumerator ScrollIcon()
	{
		while (Application.isPlaying)
		{
			if (this.mainIconScreenTime < this.researchLogoDuration)
			{
				this.toggle.fgImage.Opacity(1f);
				if (this.toggle.fgImage.overrideSprite != null)
				{
					this.toggle.fgImage.overrideSprite = null;
				}
				this.item_idx = 0;
				this.itemScreenTime = 0f;
				this.mainIconScreenTime += Time.unscaledDeltaTime;
				if (this.progressBar.enabled && this.mainIconScreenTime >= this.researchLogoDuration && this.ReadyToDisplayIcons)
				{
					yield return this.toggle.fgImage.FadeAway(this.fadingDuration, () => this.progressBar.enabled && this.mainIconScreenTime >= this.researchLogoDuration && this.ReadyToDisplayIcons);
				}
				yield return null;
			}
			else if (this.ReadyToDisplayIcons)
			{
				if (this.toggle.fgImage.overrideSprite != this.currentResearchIcons[this.item_idx])
				{
					this.toggle.fgImage.overrideSprite = this.currentResearchIcons[this.item_idx];
				}
				yield return this.toggle.fgImage.FadeToVisible(this.fadingDuration, () => this.ReadyToDisplayIcons);
				while (this.itemScreenTime < this.durationPerResearchItemIcon && this.ReadyToDisplayIcons)
				{
					this.itemScreenTime += Time.unscaledDeltaTime;
					yield return null;
				}
				yield return this.toggle.fgImage.FadeAway(this.fadingDuration, () => this.ReadyToDisplayIcons);
				if (this.ReadyToDisplayIcons)
				{
					this.itemScreenTime = 0f;
					this.item_idx++;
				}
				yield return null;
			}
			else
			{
				this.mainIconScreenTime = 0f;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x040040F4 RID: 16628
	public Image progressBar;

	// Token: 0x040040F5 RID: 16629
	private KToggle toggle;

	// Token: 0x040040F6 RID: 16630
	[Header("Scroll Options")]
	public float researchLogoDuration = 5f;

	// Token: 0x040040F7 RID: 16631
	public float durationPerResearchItemIcon = 0.6f;

	// Token: 0x040040F8 RID: 16632
	public float fadingDuration = 0.2f;

	// Token: 0x040040F9 RID: 16633
	private Coroutine scrollIconCoroutine;

	// Token: 0x040040FA RID: 16634
	private Sprite[] currentResearchIcons;

	// Token: 0x040040FB RID: 16635
	private float mainIconScreenTime;

	// Token: 0x040040FC RID: 16636
	private float itemScreenTime;

	// Token: 0x040040FD RID: 16637
	private int item_idx = -1;
}
