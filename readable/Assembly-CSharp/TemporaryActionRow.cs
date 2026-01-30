using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000EAC RID: 3756
public class TemporaryActionRow : KMonoBehaviour, IRender200ms
{
	// Token: 0x17000841 RID: 2113
	// (get) Token: 0x06007850 RID: 30800 RVA: 0x002E4754 File Offset: 0x002E2954
	// (set) Token: 0x0600784F RID: 30799 RVA: 0x002E474B File Offset: 0x002E294B
	public float MaxHeight { get; private set; }

	// Token: 0x17000842 RID: 2114
	// (get) Token: 0x06007852 RID: 30802 RVA: 0x002E4765 File Offset: 0x002E2965
	// (set) Token: 0x06007851 RID: 30801 RVA: 0x002E475C File Offset: 0x002E295C
	public bool IsVisible { get; private set; }

	// Token: 0x17000843 RID: 2115
	// (get) Token: 0x06007853 RID: 30803 RVA: 0x002E476D File Offset: 0x002E296D
	public bool ShouldProgressBarBeEnabled
	{
		get
		{
			return this.ShowTimeout && this.Lifetime > 0f && this.lastSpecifiedLifetime > 0f;
		}
	}

	// Token: 0x17000844 RID: 2116
	// (get) Token: 0x06007855 RID: 30805 RVA: 0x002E479C File Offset: 0x002E299C
	// (set) Token: 0x06007854 RID: 30804 RVA: 0x002E4793 File Offset: 0x002E2993
	public float Lifetime { get; private set; } = -1f;

	// Token: 0x17000845 RID: 2117
	// (get) Token: 0x06007857 RID: 30807 RVA: 0x002E47AD File Offset: 0x002E29AD
	// (set) Token: 0x06007856 RID: 30806 RVA: 0x002E47A4 File Offset: 0x002E29A4
	public bool ShowTimeout { get; set; } = true;

	// Token: 0x17000846 RID: 2118
	// (get) Token: 0x06007859 RID: 30809 RVA: 0x002E47BE File Offset: 0x002E29BE
	// (set) Token: 0x06007858 RID: 30808 RVA: 0x002E47B5 File Offset: 0x002E29B5
	public bool ShowOnSpawn { get; set; } = true;

	// Token: 0x17000847 RID: 2119
	// (get) Token: 0x0600785B RID: 30811 RVA: 0x002E47CF File Offset: 0x002E29CF
	// (set) Token: 0x0600785A RID: 30810 RVA: 0x002E47C6 File Offset: 0x002E29C6
	public bool HideOnClick { get; set; } = true;

	// Token: 0x0600785C RID: 30812 RVA: 0x002E47D8 File Offset: 0x002E29D8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.layoutElement = base.GetComponent<LayoutElement>();
		this.button = base.GetComponent<Button>();
		this.button.onClick.AddListener(new UnityAction(this._OnRowClicked));
		this.MaxHeight = this.layoutElement.minHeight;
		this.HideImmediatly();
	}

	// Token: 0x0600785D RID: 30813 RVA: 0x002E4836 File Offset: 0x002E2A36
	private void Update()
	{
		if (!this.HasBeenShown && this.ShowOnSpawn)
		{
			this.RefreshContentWidth();
			if (this.Content.sizeDelta.x > 0f)
			{
				this.Show();
			}
		}
	}

	// Token: 0x0600785E RID: 30814 RVA: 0x002E486D File Offset: 0x002E2A6D
	private void _OnRowClicked()
	{
		Action<TemporaryActionRow> onRowClicked = this.OnRowClicked;
		if (onRowClicked != null)
		{
			onRowClicked(this);
		}
		if (this.HideOnClick)
		{
			this.Hide();
		}
	}

	// Token: 0x0600785F RID: 30815 RVA: 0x002E488F File Offset: 0x002E2A8F
	private void _OnRowHidden()
	{
		Action<TemporaryActionRow> onRowHidden = this.OnRowHidden;
		if (onRowHidden == null)
		{
			return;
		}
		onRowHidden(this);
	}

	// Token: 0x06007860 RID: 30816 RVA: 0x002E48A2 File Offset: 0x002E2AA2
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (base.isSpawned)
		{
			this.RefreshContentWidth();
		}
	}

	// Token: 0x06007861 RID: 30817 RVA: 0x002E48B8 File Offset: 0x002E2AB8
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.HideImmediatly();
		this._OnRowHidden();
	}

	// Token: 0x06007862 RID: 30818 RVA: 0x002E48CC File Offset: 0x002E2ACC
	public void SetLifetime(float lifetime)
	{
		this.Lifetime = lifetime;
		this.lastSpecifiedLifetime = lifetime;
		this.UpdateTimeout();
	}

	// Token: 0x06007863 RID: 30819 RVA: 0x002E48E4 File Offset: 0x002E2AE4
	private void UpdateTimeout()
	{
		bool shouldProgressBarBeEnabled = this.ShouldProgressBarBeEnabled;
		if (shouldProgressBarBeEnabled != this.TimeoutBarSection.gameObject.activeInHierarchy)
		{
			this.TimeoutBarSection.gameObject.SetActive(shouldProgressBarBeEnabled);
		}
		if (shouldProgressBarBeEnabled)
		{
			this.TimeoutImage.fillAmount = Mathf.Clamp(this.Lifetime / this.lastSpecifiedLifetime, 0f, 1f);
		}
	}

	// Token: 0x06007864 RID: 30820 RVA: 0x002E4948 File Offset: 0x002E2B48
	public void Render200ms(float dt)
	{
		if (this.HasBeenShown && this.Lifetime > 0f && this.IsVisible)
		{
			this.Lifetime -= dt;
			if (this.Lifetime <= 0f)
			{
				this.Hide();
			}
			this.UpdateTimeout();
		}
	}

	// Token: 0x06007865 RID: 30821 RVA: 0x002E4999 File Offset: 0x002E2B99
	public void Setup(string text, string tooltip, Sprite icon = null)
	{
		this.Label.SetText(text);
		this.Tooltip.SetSimpleTooltip(tooltip);
		this.Image.sprite = icon;
		this.IconSection.gameObject.SetActive(icon != null);
	}

	// Token: 0x06007866 RID: 30822 RVA: 0x002E49D8 File Offset: 0x002E2BD8
	public void Show()
	{
		this.AbortCoroutine();
		this.IsVisible = true;
		this.HasBeenShown = true;
		this.button.interactable = true;
		if (base.gameObject.activeInHierarchy)
		{
			this.SetContentToHiddenPosition();
			this.layoutCoroutine = this.RunEnterHeightAnimation(delegate
			{
				this.layoutCoroutine = this.RunEnterSlideAnimation(null);
			});
		}
	}

	// Token: 0x06007867 RID: 30823 RVA: 0x002E4A30 File Offset: 0x002E2C30
	public void HideImmediatly()
	{
		this.AbortCoroutine();
		this.IsVisible = false;
		this.Content.localPosition = new Vector3(-(base.transform as RectTransform).sizeDelta.x, this.Content.localPosition.y, this.Content.localPosition.z);
		this.layoutElement.minHeight = 0f;
		this.button.interactable = false;
	}

	// Token: 0x06007868 RID: 30824 RVA: 0x002E4AAC File Offset: 0x002E2CAC
	public void Hide()
	{
		this.AbortCoroutine();
		this.IsVisible = false;
		this.button.interactable = false;
		if (base.gameObject.activeInHierarchy)
		{
			this.layoutCoroutine = this.RunExitSlideAnimation(delegate
			{
				this.layoutCoroutine = this.RunExitHeightAnimation(new System.Action(this._OnRowHidden));
			});
		}
	}

	// Token: 0x06007869 RID: 30825 RVA: 0x002E4AEC File Offset: 0x002E2CEC
	private void AbortCoroutine()
	{
		if (this.layoutCoroutine != null)
		{
			base.StopCoroutine(this.layoutCoroutine);
			this.layoutCoroutine = null;
		}
	}

	// Token: 0x0600786A RID: 30826 RVA: 0x002E4B0C File Offset: 0x002E2D0C
	private void RefreshContentWidth()
	{
		RectTransform rectTransform = base.transform as RectTransform;
		if (rectTransform.sizeDelta.x != this.Content.sizeDelta.x)
		{
			this.Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.sizeDelta.x);
		}
	}

	// Token: 0x0600786B RID: 30827 RVA: 0x002E4B5C File Offset: 0x002E2D5C
	private void SetContentToHiddenPosition()
	{
		this.RefreshContentWidth();
		Vector3 v = this.Content.anchoredPosition;
		v.x = -(base.transform as RectTransform).sizeDelta.x;
		this.Content.anchoredPosition = v;
	}

	// Token: 0x0600786C RID: 30828 RVA: 0x002E4BAE File Offset: 0x002E2DAE
	private Coroutine RunEnterSlideAnimation(System.Action onAnimationEnds = null)
	{
		return base.StartCoroutine(this.SlideTransitionAnimation(0.4f, true, (float n) => Mathf.Sqrt(n), onAnimationEnds));
	}

	// Token: 0x0600786D RID: 30829 RVA: 0x002E4BE2 File Offset: 0x002E2DE2
	private Coroutine RunExitSlideAnimation(System.Action onAnimationEnds = null)
	{
		return base.StartCoroutine(this.SlideTransitionAnimation(0.4f, false, (float n) => Mathf.Pow(n, 2f), onAnimationEnds));
	}

	// Token: 0x0600786E RID: 30830 RVA: 0x002E4C16 File Offset: 0x002E2E16
	private Coroutine RunEnterHeightAnimation(System.Action onAnimationEnds = null)
	{
		return base.StartCoroutine(this.HeightTransitionAnimation(0.5f, true, (float n) => Mathf.Sqrt(n), onAnimationEnds));
	}

	// Token: 0x0600786F RID: 30831 RVA: 0x002E4C4A File Offset: 0x002E2E4A
	private Coroutine RunExitHeightAnimation(System.Action onAnimationEnds = null)
	{
		return base.StartCoroutine(this.HeightTransitionAnimation(0.3f, false, (float n) => Mathf.Pow(n, 2f), onAnimationEnds));
	}

	// Token: 0x06007870 RID: 30832 RVA: 0x002E4C7E File Offset: 0x002E2E7E
	private IEnumerator SlideTransitionAnimation(float duration, bool show, Func<float, float> curveModifier = null, System.Action onAnimationEnds = null)
	{
		float num = -(base.transform as RectTransform).sizeDelta.x;
		float num2 = 0f;
		float contentInitialXPosition = show ? num : this.Content.anchoredPosition.x;
		float targetPosition = show ? num2 : num;
		float timePassed = 0f;
		Vector3 v = this.Content.anchoredPosition;
		while (timePassed < duration)
		{
			this.RefreshContentWidth();
			float num3 = timePassed / duration;
			if (curveModifier != null)
			{
				num3 = curveModifier(num3);
			}
			v = this.Content.anchoredPosition;
			v.x = Mathf.Lerp(contentInitialXPosition, targetPosition, num3);
			this.Content.anchoredPosition = v;
			timePassed += Time.unscaledDeltaTime;
			yield return null;
		}
		this.RefreshContentWidth();
		v = this.Content.anchoredPosition;
		v.x = targetPosition;
		this.Content.anchoredPosition = v;
		yield return null;
		if (onAnimationEnds != null)
		{
			onAnimationEnds();
		}
		yield break;
	}

	// Token: 0x06007871 RID: 30833 RVA: 0x002E4CAA File Offset: 0x002E2EAA
	private IEnumerator HeightTransitionAnimation(float duration, bool show, Func<float, float> curveModifier = null, System.Action onAnimationEnds = null)
	{
		Transform transform = base.transform;
		float initialHeight = this.layoutElement.minHeight;
		float targetHeight = show ? this.MaxHeight : 0f;
		float timePassed = 0f;
		float minHeight = this.layoutElement.minHeight;
		while (timePassed < duration)
		{
			this.RefreshContentWidth();
			float num = timePassed / duration;
			if (curveModifier != null)
			{
				num = curveModifier(num);
			}
			minHeight = Mathf.Lerp(initialHeight, targetHeight, num);
			this.layoutElement.minHeight = minHeight;
			timePassed += Time.unscaledDeltaTime;
			yield return null;
		}
		this.RefreshContentWidth();
		minHeight = targetHeight;
		this.layoutElement.minHeight = minHeight;
		yield return null;
		if (onAnimationEnds != null)
		{
			onAnimationEnds();
		}
		yield break;
	}

	// Token: 0x040053D1 RID: 21457
	public const float ROW_HEIGHT_ANIM_ENTRY_DURATION = 0.5f;

	// Token: 0x040053D2 RID: 21458
	public const float ROW_HEIGHT_ANIM_EXIT_DURATION = 0.3f;

	// Token: 0x040053D3 RID: 21459
	public const float SLIDE_ENTER_ANIM_DURATION = 0.4f;

	// Token: 0x040053D4 RID: 21460
	public const float SLIDE_EXIT_ANIM_DURATION = 0.4f;

	// Token: 0x040053D7 RID: 21463
	public RectTransform Content;

	// Token: 0x040053D8 RID: 21464
	public RectTransform IconSection;

	// Token: 0x040053D9 RID: 21465
	public RectTransform TimeoutBarSection;

	// Token: 0x040053DA RID: 21466
	public KImage Image;

	// Token: 0x040053DB RID: 21467
	public Image TimeoutImage;

	// Token: 0x040053DC RID: 21468
	public LocText Label;

	// Token: 0x040053DD RID: 21469
	public ToolTip Tooltip;

	// Token: 0x040053DE RID: 21470
	public Action<TemporaryActionRow> OnRowClicked;

	// Token: 0x040053DF RID: 21471
	public Action<TemporaryActionRow> OnRowHidden;

	// Token: 0x040053E0 RID: 21472
	private LayoutElement layoutElement;

	// Token: 0x040053E1 RID: 21473
	private Coroutine layoutCoroutine;

	// Token: 0x040053E2 RID: 21474
	private Button button;

	// Token: 0x040053E7 RID: 21479
	private bool HasBeenShown;

	// Token: 0x040053E8 RID: 21480
	private float lastSpecifiedLifetime = -1f;
}
