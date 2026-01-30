using System;
using UnityEngine;

// Token: 0x020009C2 RID: 2498
[AddComponentMenu("KMonoBehaviour/scripts/SelectMarker")]
public class SelectMarker : KMonoBehaviour
{
	// Token: 0x0600489A RID: 18586 RVA: 0x001A3211 File Offset: 0x001A1411
	public void SetTargetTransform(Transform target_transform)
	{
		this.targetTransform = target_transform;
		this.LateUpdate();
	}

	// Token: 0x0600489B RID: 18587 RVA: 0x001A3220 File Offset: 0x001A1420
	private void LateUpdate()
	{
		if (this.targetTransform == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		Vector3 position = this.targetTransform.GetPosition();
		KCollider2D component = this.targetTransform.GetComponent<KCollider2D>();
		if (component != null)
		{
			position.x = component.bounds.center.x;
			position.y = component.bounds.center.y + component.bounds.size.y / 2f + 0.1f;
		}
		else
		{
			position.y += 2f;
		}
		Vector3 b = new Vector3(0f, (Mathf.Sin(Time.unscaledTime * 4f) + 1f) * this.animationOffset, 0f);
		base.transform.SetPosition(position + b);
	}

	// Token: 0x0400304D RID: 12365
	public float animationOffset = 0.1f;

	// Token: 0x0400304E RID: 12366
	private Transform targetTransform;
}
