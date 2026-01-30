using System;
using UnityEngine;

// Token: 0x02000D2F RID: 3375
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenPlainText")]
public class InfoScreenPlainText : KMonoBehaviour
{
	// Token: 0x06006844 RID: 26692 RVA: 0x0027525D File Offset: 0x0027345D
	public void SetText(string text)
	{
		this.locText.text = text;
	}

	// Token: 0x0400479D RID: 18333
	[SerializeField]
	private LocText locText;
}
