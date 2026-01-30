using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000E08 RID: 3592
public class SelectablePanel : MonoBehaviour, IDeselectHandler, IEventSystemHandler
{
	// Token: 0x060071D4 RID: 29140 RVA: 0x002B7D55 File Offset: 0x002B5F55
	public void OnDeselect(BaseEventData evt)
	{
		base.gameObject.SetActive(false);
	}
}
