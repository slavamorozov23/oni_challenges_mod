using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CC5 RID: 3269
public interface ICodexWidget
{
	// Token: 0x060064D9 RID: 25817
	void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles);
}
