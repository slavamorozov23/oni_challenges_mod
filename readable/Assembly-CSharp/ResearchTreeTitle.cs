using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000007 RID: 7
public class ResearchTreeTitle : MonoBehaviour
{
	// Token: 0x0600001F RID: 31 RVA: 0x000027D1 File Offset: 0x000009D1
	public void SetLabel(string txt)
	{
		this.treeLabel.text = txt;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x000027DF File Offset: 0x000009DF
	public void SetColor(int id)
	{
		this.BG.enabled = (id % 2 != 0);
	}

	// Token: 0x0400001C RID: 28
	[Header("References")]
	[SerializeField]
	private LocText treeLabel;

	// Token: 0x0400001D RID: 29
	[SerializeField]
	private Image BG;
}
