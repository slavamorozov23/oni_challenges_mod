using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D61 RID: 3425
public class KScrollbarVisibility : MonoBehaviour
{
	// Token: 0x06006A0A RID: 27146 RVA: 0x0028189E File Offset: 0x0027FA9E
	private void Start()
	{
		this.Update();
	}

	// Token: 0x06006A0B RID: 27147 RVA: 0x002818A8 File Offset: 0x0027FAA8
	private void Update()
	{
		if (this.content.content == null)
		{
			return;
		}
		bool flag = false;
		Vector2 vector = new Vector2(this.parent.rect.width, this.parent.rect.height);
		Vector2 sizeDelta = this.content.content.GetComponent<RectTransform>().sizeDelta;
		if ((sizeDelta.x >= vector.x && this.checkWidth) || (sizeDelta.y >= vector.y && this.checkHeight))
		{
			flag = true;
		}
		if (this.scrollbar.gameObject.activeSelf != flag)
		{
			this.scrollbar.gameObject.SetActive(flag);
			if (this.others != null)
			{
				GameObject[] array = this.others;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(flag);
				}
			}
		}
	}

	// Token: 0x040048D6 RID: 18646
	[SerializeField]
	private ScrollRect content;

	// Token: 0x040048D7 RID: 18647
	[SerializeField]
	private RectTransform parent;

	// Token: 0x040048D8 RID: 18648
	[SerializeField]
	private bool checkWidth = true;

	// Token: 0x040048D9 RID: 18649
	[SerializeField]
	private bool checkHeight = true;

	// Token: 0x040048DA RID: 18650
	[SerializeField]
	private Scrollbar scrollbar;

	// Token: 0x040048DB RID: 18651
	[SerializeField]
	private GameObject[] others;
}
