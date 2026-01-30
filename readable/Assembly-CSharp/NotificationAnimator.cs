using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D99 RID: 3481
public class NotificationAnimator : MonoBehaviour
{
	// Token: 0x06006C68 RID: 27752 RVA: 0x0029146E File Offset: 0x0028F66E
	public void Begin(bool startOffset = true)
	{
		this.Reset();
		this.animating = true;
		if (startOffset)
		{
			this.layoutElement.minWidth = 100f;
			return;
		}
		this.layoutElement.minWidth = 1f;
		this.speed = -10f;
	}

	// Token: 0x06006C69 RID: 27753 RVA: 0x002914AC File Offset: 0x0028F6AC
	private void Reset()
	{
		this.bounceCount = 2;
		this.layoutElement = base.GetComponent<LayoutElement>();
		this.layoutElement.minWidth = 0f;
		this.speed = 1f;
	}

	// Token: 0x06006C6A RID: 27754 RVA: 0x002914DC File Offset: 0x0028F6DC
	public void Stop()
	{
		this.Reset();
		this.animating = false;
	}

	// Token: 0x06006C6B RID: 27755 RVA: 0x002914EC File Offset: 0x0028F6EC
	private void LateUpdate()
	{
		if (!this.animating)
		{
			return;
		}
		this.layoutElement.minWidth -= this.speed;
		this.speed += 0.5f;
		if (this.layoutElement.minWidth <= 0f)
		{
			if (this.bounceCount > 0)
			{
				this.bounceCount--;
				this.speed = -this.speed / Mathf.Pow(2f, (float)(2 - this.bounceCount));
				this.layoutElement.minWidth = -this.speed;
				return;
			}
			this.layoutElement.minWidth = 0f;
			this.Stop();
		}
	}

	// Token: 0x04004A39 RID: 19001
	private const float START_SPEED = 1f;

	// Token: 0x04004A3A RID: 19002
	private const float ACCELERATION = 0.5f;

	// Token: 0x04004A3B RID: 19003
	private const float BOUNCE_DAMPEN = 2f;

	// Token: 0x04004A3C RID: 19004
	private const int BOUNCE_COUNT = 2;

	// Token: 0x04004A3D RID: 19005
	private const float OFFSETX = 100f;

	// Token: 0x04004A3E RID: 19006
	private float speed = 1f;

	// Token: 0x04004A3F RID: 19007
	private int bounceCount = 2;

	// Token: 0x04004A40 RID: 19008
	private LayoutElement layoutElement;

	// Token: 0x04004A41 RID: 19009
	[SerializeField]
	private bool animating = true;
}
