using System;
using UnityEngine;

// Token: 0x02000CA5 RID: 3237
public class SizePulse : MonoBehaviour
{
	// Token: 0x0600630B RID: 25355 RVA: 0x0024B6B8 File Offset: 0x002498B8
	private void Start()
	{
		if (base.GetComponents<SizePulse>().Length > 1)
		{
			UnityEngine.Object.Destroy(this);
		}
		RectTransform rectTransform = (RectTransform)base.transform;
		this.from = rectTransform.localScale;
		this.cur = this.from;
		this.to = this.from * this.multiplier;
	}

	// Token: 0x0600630C RID: 25356 RVA: 0x0024B718 File Offset: 0x00249918
	private void Update()
	{
		float num = this.updateWhenPaused ? Time.unscaledDeltaTime : Time.deltaTime;
		num *= this.speed;
		SizePulse.State state = this.state;
		if (state != SizePulse.State.Up)
		{
			if (state == SizePulse.State.Down)
			{
				this.cur = Vector2.Lerp(this.cur, this.from, num);
				if ((this.from - this.cur).sqrMagnitude < 0.0001f)
				{
					this.cur = this.from;
					this.state = SizePulse.State.Finished;
					if (this.onComplete != null)
					{
						this.onComplete();
					}
				}
			}
		}
		else
		{
			this.cur = Vector2.Lerp(this.cur, this.to, num);
			if ((this.to - this.cur).sqrMagnitude < 0.0001f)
			{
				this.cur = this.to;
				this.state = SizePulse.State.Down;
			}
		}
		((RectTransform)base.transform).localScale = new Vector3(this.cur.x, this.cur.y, 1f);
	}

	// Token: 0x04004319 RID: 17177
	public System.Action onComplete;

	// Token: 0x0400431A RID: 17178
	public Vector2 from = Vector2.one;

	// Token: 0x0400431B RID: 17179
	public Vector2 to = Vector2.one;

	// Token: 0x0400431C RID: 17180
	public float multiplier = 1.25f;

	// Token: 0x0400431D RID: 17181
	public float speed = 1f;

	// Token: 0x0400431E RID: 17182
	public bool updateWhenPaused;

	// Token: 0x0400431F RID: 17183
	private Vector2 cur;

	// Token: 0x04004320 RID: 17184
	private SizePulse.State state;

	// Token: 0x02001ED0 RID: 7888
	private enum State
	{
		// Token: 0x040090B8 RID: 37048
		Up,
		// Token: 0x040090B9 RID: 37049
		Down,
		// Token: 0x040090BA RID: 37050
		Finished
	}
}
