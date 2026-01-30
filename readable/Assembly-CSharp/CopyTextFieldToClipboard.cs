using System;
using UnityEngine;

// Token: 0x02000ECA RID: 3786
[AddComponentMenu("KMonoBehaviour/scripts/CopyTextFieldToClipboard")]
public class CopyTextFieldToClipboard : KMonoBehaviour
{
	// Token: 0x0600793D RID: 31037 RVA: 0x002E9660 File Offset: 0x002E7860
	protected override void OnPrefabInit()
	{
		this.button.onClick += this.OnClick;
	}

	// Token: 0x0600793E RID: 31038 RVA: 0x002E9679 File Offset: 0x002E7879
	private void OnClick()
	{
		TextEditor textEditor = new TextEditor();
		textEditor.text = this.GetText();
		textEditor.SelectAll();
		textEditor.Copy();
	}

	// Token: 0x0400549B RID: 21659
	public KButton button;

	// Token: 0x0400549C RID: 21660
	public Func<string> GetText;
}
