using System;
using UnityEngine;

// Token: 0x020008FF RID: 2303
public class DreamBubble : KMonoBehaviour
{
	// Token: 0x1700046B RID: 1131
	// (get) Token: 0x06003FF4 RID: 16372 RVA: 0x00167E8E File Offset: 0x0016608E
	// (set) Token: 0x06003FF3 RID: 16371 RVA: 0x00167E85 File Offset: 0x00166085
	public bool IsVisible { get; private set; }

	// Token: 0x06003FF5 RID: 16373 RVA: 0x00167E96 File Offset: 0x00166096
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.dreamBackgroundComponent.SetSymbolVisiblity(this.snapToPivotSymbol, false);
		this.SetVisibility(false);
	}

	// Token: 0x06003FF6 RID: 16374 RVA: 0x00167EBC File Offset: 0x001660BC
	public void Tick(float dt)
	{
		if (this._currentDream != null && this._currentDream.Icons.Length != 0)
		{
			float num = this._timePassedSinceDreamStarted / this._currentDream.secondPerImage;
			int num2 = Mathf.FloorToInt(num);
			float num3 = num - (float)num2;
			int num4 = (int)Mathf.Repeat((float)Mathf.FloorToInt(num), (float)this._currentDream.Icons.Length);
			if (this.dreamContentComponent.sprite != this._currentDream.Icons[num4])
			{
				this.dreamContentComponent.sprite = this._currentDream.Icons[num4];
			}
			this.dreamContentComponent.rectTransform.localScale = Vector3.one * num3;
			this._color.a = (Mathf.Sin(num3 * 6.2831855f - 1.5707964f) + 1f) * 0.5f;
			this.dreamContentComponent.color = this._color;
			this._timePassedSinceDreamStarted += dt;
		}
	}

	// Token: 0x06003FF7 RID: 16375 RVA: 0x00167FB8 File Offset: 0x001661B8
	public void SetDream(Dream dream)
	{
		this._currentDream = dream;
		this.dreamBackgroundComponent.Stop();
		this.dreamBackgroundComponent.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim(dream.BackgroundAnim)
		};
		this.dreamContentComponent.color = this._color;
		this.dreamContentComponent.enabled = (dream != null && dream.Icons != null && dream.Icons.Length != 0);
		this._timePassedSinceDreamStarted = 0f;
		this._color.a = 0f;
	}

	// Token: 0x06003FF8 RID: 16376 RVA: 0x0016804C File Offset: 0x0016624C
	public void SetVisibility(bool visible)
	{
		this.IsVisible = visible;
		this.dreamBackgroundComponent.SetVisiblity(visible);
		this.dreamContentComponent.gameObject.SetActive(visible);
		if (visible)
		{
			if (this._currentDream != null)
			{
				this.dreamBackgroundComponent.Play("dream_loop", KAnim.PlayMode.Loop, 1f, 0f);
			}
			this.dreamBubbleBorderKanim.Play("dream_bubble_loop", KAnim.PlayMode.Loop, 1f, 0f);
			this.maskKanim.Play("dream_bubble_mask", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		this.dreamBackgroundComponent.Stop();
		this.maskKanim.Stop();
		this.dreamBubbleBorderKanim.Stop();
	}

	// Token: 0x06003FF9 RID: 16377 RVA: 0x0016810A File Offset: 0x0016630A
	public void StopDreaming()
	{
		this._currentDream = null;
		this.SetVisibility(false);
	}

	// Token: 0x04002795 RID: 10133
	public KBatchedAnimController dreamBackgroundComponent;

	// Token: 0x04002796 RID: 10134
	public KBatchedAnimController maskKanim;

	// Token: 0x04002797 RID: 10135
	public KBatchedAnimController dreamBubbleBorderKanim;

	// Token: 0x04002798 RID: 10136
	public KImage dreamContentComponent;

	// Token: 0x04002799 RID: 10137
	private const string dreamBackgroundAnimationName = "dream_loop";

	// Token: 0x0400279A RID: 10138
	private const string dreamMaskAnimationName = "dream_bubble_mask";

	// Token: 0x0400279B RID: 10139
	private const string dreamBubbleBorderAnimationName = "dream_bubble_loop";

	// Token: 0x0400279C RID: 10140
	private HashedString snapToPivotSymbol = new HashedString("snapto_pivot");

	// Token: 0x0400279E RID: 10142
	private Dream _currentDream;

	// Token: 0x0400279F RID: 10143
	private float _timePassedSinceDreamStarted;

	// Token: 0x040027A0 RID: 10144
	private Color _color = Color.white;

	// Token: 0x040027A1 RID: 10145
	private const float PI_2 = 6.2831855f;

	// Token: 0x040027A2 RID: 10146
	private const float HALF_PI = 1.5707964f;
}
