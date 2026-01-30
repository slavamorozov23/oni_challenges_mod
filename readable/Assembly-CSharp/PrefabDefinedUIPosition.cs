using System;
using UnityEngine;

// Token: 0x02000D43 RID: 3395
public class PrefabDefinedUIPosition
{
	// Token: 0x0600694C RID: 26956 RVA: 0x0027E479 File Offset: 0x0027C679
	public void SetOn(GameObject gameObject)
	{
		if (this.position.HasValue)
		{
			gameObject.rectTransform().anchoredPosition = this.position.Value;
			return;
		}
		this.position = gameObject.rectTransform().anchoredPosition;
	}

	// Token: 0x0600694D RID: 26957 RVA: 0x0027E4B5 File Offset: 0x0027C6B5
	public void SetOn(Component component)
	{
		if (this.position.HasValue)
		{
			component.rectTransform().anchoredPosition = this.position.Value;
			return;
		}
		this.position = component.rectTransform().anchoredPosition;
	}

	// Token: 0x04004864 RID: 18532
	private Option<Vector2> position;
}
