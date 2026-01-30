using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CCB RID: 3275
public class CodexDividerLine : CodexWidget<CodexDividerLine>
{
	// Token: 0x06006525 RID: 25893 RVA: 0x002612CB File Offset: 0x0025F4CB
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		contentGameObject.GetComponent<LayoutElement>().minWidth = 530f;
	}
}
