using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000ECB RID: 3787
public class DialogPanel : MonoBehaviour, IDeselectHandler, IEventSystemHandler
{
	// Token: 0x06007940 RID: 31040 RVA: 0x002E96A4 File Offset: 0x002E78A4
	public void OnDeselect(BaseEventData eventData)
	{
		if (this.destroyOnDeselect)
		{
			foreach (object obj in base.transform)
			{
				Util.KDestroyGameObject(((Transform)obj).gameObject);
			}
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400549D RID: 21661
	public bool destroyOnDeselect = true;
}
