using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
[AddComponentMenu("KMonoBehaviour/prefabs/UIRotator")]
public class UIRotator : KMonoBehaviour
{
	// Token: 0x0600002A RID: 42 RVA: 0x00002BC3 File Offset: 0x00000DC3
	protected override void OnPrefabInit()
	{
		this.rotationSpeed = UnityEngine.Random.Range(this.minRotationSpeed, this.maxRotationSpeed);
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002BDC File Offset: 0x00000DDC
	private void Update()
	{
		base.GetComponent<RectTransform>().Rotate(0f, 0f, this.rotationSpeed * Time.unscaledDeltaTime);
	}

	// Token: 0x04000032 RID: 50
	public float minRotationSpeed = 1f;

	// Token: 0x04000033 RID: 51
	public float maxRotationSpeed = 1f;

	// Token: 0x04000034 RID: 52
	public float rotationSpeed = 1f;
}
