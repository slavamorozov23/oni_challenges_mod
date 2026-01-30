using System;
using Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D59 RID: 3417
public class KleiPermitDioramaVis_Fallback : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060069B2 RID: 27058 RVA: 0x00280235 File Offset: 0x0027E435
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060069B3 RID: 27059 RVA: 0x0028023D File Offset: 0x0027E43D
	public void ConfigureSetup()
	{
	}

	// Token: 0x060069B4 RID: 27060 RVA: 0x0028023F File Offset: 0x0027E43F
	public void ConfigureWith(PermitResource permit)
	{
		this.sprite.sprite = PermitPresentationInfo.GetUnknownSprite();
		this.editorOnlyErrorMessageParent.gameObject.SetActive(false);
	}

	// Token: 0x060069B5 RID: 27061 RVA: 0x00280262 File Offset: 0x0027E462
	public KleiPermitDioramaVis_Fallback WithError(string error)
	{
		this.error = error;
		global::Debug.Log("[KleiInventoryScreen Error] Had to use fallback vis. " + error);
		return this;
	}

	// Token: 0x040048A9 RID: 18601
	[SerializeField]
	private Image sprite;

	// Token: 0x040048AA RID: 18602
	[SerializeField]
	private RectTransform editorOnlyErrorMessageParent;

	// Token: 0x040048AB RID: 18603
	[SerializeField]
	private TextMeshProUGUI editorOnlyErrorMessageText;

	// Token: 0x040048AC RID: 18604
	private Option<string> error;
}
