using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CCE RID: 3278
public class CodexLargeSpacer : CodexWidget<CodexLargeSpacer>
{
	// Token: 0x06006537 RID: 25911 RVA: 0x00261577 File Offset: 0x0025F777
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
