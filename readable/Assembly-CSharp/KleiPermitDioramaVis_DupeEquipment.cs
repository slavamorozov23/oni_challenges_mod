using System;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D58 RID: 3416
public class KleiPermitDioramaVis_DupeEquipment : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060069AE RID: 27054 RVA: 0x002801BD File Offset: 0x0027E3BD
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060069AF RID: 27055 RVA: 0x002801C5 File Offset: 0x0027E3C5
	public void ConfigureSetup()
	{
		this.uiMannequin.shouldShowOutfitWithDefaultItems = false;
	}

	// Token: 0x060069B0 RID: 27056 RVA: 0x002801D4 File Offset: 0x0027E3D4
	public void ConfigureWith(PermitResource permit)
	{
		ClothingItemResource clothingItemResource = permit as ClothingItemResource;
		if (clothingItemResource != null)
		{
			this.uiMannequin.SetOutfit(clothingItemResource.outfitType, new ClothingItemResource[]
			{
				clothingItemResource
			});
			this.uiMannequin.ReactToClothingItemChange(clothingItemResource.Category);
		}
		this.dioramaBGImage.sprite = KleiPermitDioramaVis.GetDioramaBackground(permit.Category);
	}

	// Token: 0x040048A5 RID: 18597
	[SerializeField]
	private UIMannequin uiMannequin;

	// Token: 0x040048A6 RID: 18598
	[Header("Diorama Backgrounds")]
	[SerializeField]
	private Image dioramaBGImage;

	// Token: 0x040048A7 RID: 18599
	[SerializeField]
	private Sprite clothingBG;

	// Token: 0x040048A8 RID: 18600
	[SerializeField]
	private Sprite atmosuitBG;
}
