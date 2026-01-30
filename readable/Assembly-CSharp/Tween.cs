using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000EB4 RID: 3764
public class Tween : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060078A9 RID: 30889 RVA: 0x002E64F4 File Offset: 0x002E46F4
	private void Awake()
	{
		this.Selectable = base.GetComponent<Selectable>();
	}

	// Token: 0x060078AA RID: 30890 RVA: 0x002E6502 File Offset: 0x002E4702
	public void OnPointerEnter(PointerEventData data)
	{
		this.Direction = 1f;
	}

	// Token: 0x060078AB RID: 30891 RVA: 0x002E650F File Offset: 0x002E470F
	public void OnPointerExit(PointerEventData data)
	{
		this.Direction = -1f;
	}

	// Token: 0x060078AC RID: 30892 RVA: 0x002E651C File Offset: 0x002E471C
	private void Update()
	{
		if (this.Selectable.interactable)
		{
			float x = base.transform.localScale.x;
			float num = x + this.Direction * Time.unscaledDeltaTime * Tween.ScaleSpeed;
			num = Mathf.Min(num, Tween.Scale);
			num = Mathf.Max(num, 1f);
			if (num != x)
			{
				base.transform.localScale = new Vector3(num, num, 1f);
			}
		}
	}

	// Token: 0x04005419 RID: 21529
	private static float Scale = 1.025f;

	// Token: 0x0400541A RID: 21530
	private static float ScaleSpeed = 0.5f;

	// Token: 0x0400541B RID: 21531
	private Selectable Selectable;

	// Token: 0x0400541C RID: 21532
	private float Direction = -1f;
}
