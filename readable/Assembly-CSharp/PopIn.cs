using System;
using UnityEngine;

// Token: 0x02000DE0 RID: 3552
public class PopIn : MonoBehaviour
{
	// Token: 0x06006FA8 RID: 28584 RVA: 0x002A67DE File Offset: 0x002A49DE
	private void OnEnable()
	{
		this.StartPopIn(true);
	}

	// Token: 0x06006FA9 RID: 28585 RVA: 0x002A67E8 File Offset: 0x002A49E8
	private void Update()
	{
		float num = Mathf.Lerp(base.transform.localScale.x, this.targetScale, Time.unscaledDeltaTime * this.speed);
		base.transform.localScale = new Vector3(num, num, 1f);
	}

	// Token: 0x06006FAA RID: 28586 RVA: 0x002A6834 File Offset: 0x002A4A34
	public void StartPopIn(bool force_reset = false)
	{
		if (force_reset)
		{
			base.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
		}
		this.targetScale = 1f;
	}

	// Token: 0x06006FAB RID: 28587 RVA: 0x002A6863 File Offset: 0x002A4A63
	public void StartPopOut()
	{
		this.targetScale = 0f;
	}

	// Token: 0x04004C81 RID: 19585
	private float targetScale;

	// Token: 0x04004C82 RID: 19586
	public float speed;
}
