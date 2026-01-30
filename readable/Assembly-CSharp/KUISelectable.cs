using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000D3E RID: 3390
[AddComponentMenu("KMonoBehaviour/scripts/KUISelectable")]
public class KUISelectable : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060068E7 RID: 26855 RVA: 0x0027B654 File Offset: 0x00279854
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x060068E8 RID: 26856 RVA: 0x0027B656 File Offset: 0x00279856
	protected override void OnSpawn()
	{
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x060068E9 RID: 26857 RVA: 0x0027B674 File Offset: 0x00279874
	public void SetTarget(GameObject target)
	{
		this.target = target;
	}

	// Token: 0x060068EA RID: 26858 RVA: 0x0027B67D File Offset: 0x0027987D
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.target != null)
		{
			SelectTool.Instance.SetHoverOverride(this.target.GetComponent<KSelectable>());
		}
	}

	// Token: 0x060068EB RID: 26859 RVA: 0x0027B6A2 File Offset: 0x002798A2
	public void OnPointerExit(PointerEventData eventData)
	{
		SelectTool.Instance.SetHoverOverride(null);
	}

	// Token: 0x060068EC RID: 26860 RVA: 0x0027B6AF File Offset: 0x002798AF
	private void OnClick()
	{
		if (this.target != null)
		{
			SelectTool.Instance.Select(this.target.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x060068ED RID: 26861 RVA: 0x0027B6D5 File Offset: 0x002798D5
	protected override void OnCmpDisable()
	{
		if (SelectTool.Instance != null)
		{
			SelectTool.Instance.SetHoverOverride(null);
		}
	}

	// Token: 0x04004810 RID: 18448
	private GameObject target;
}
