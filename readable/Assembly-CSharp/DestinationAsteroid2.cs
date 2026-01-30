using System;
using ProcGen;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CF7 RID: 3319
[AddComponentMenu("KMonoBehaviour/scripts/DestinationAsteroid2")]
public class DestinationAsteroid2 : KMonoBehaviour
{
	// Token: 0x14000026 RID: 38
	// (add) Token: 0x06006683 RID: 26243 RVA: 0x0026974C File Offset: 0x0026794C
	// (remove) Token: 0x06006684 RID: 26244 RVA: 0x00269784 File Offset: 0x00267984
	public event Action<ColonyDestinationAsteroidBeltData> OnClicked;

	// Token: 0x06006685 RID: 26245 RVA: 0x002697B9 File Offset: 0x002679B9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.button.onClick += this.OnClickInternal;
	}

	// Token: 0x06006686 RID: 26246 RVA: 0x002697D8 File Offset: 0x002679D8
	public void SetAsteroid(ColonyDestinationAsteroidBeltData newAsteroidData)
	{
		if (this.asteroidData == null || newAsteroidData.beltPath != this.asteroidData.beltPath)
		{
			this.asteroidData = newAsteroidData;
			ProcGen.World getStartWorld = newAsteroidData.GetStartWorld;
			KAnimFile kanimFile;
			Assets.TryGetAnim(getStartWorld.asteroidIcon.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : getStartWorld.asteroidIcon, out kanimFile);
			if (kanimFile != null)
			{
				this.asteroidImage.gameObject.SetActive(false);
				this.animController.AnimFiles = new KAnimFile[]
				{
					kanimFile
				};
				this.animController.initialMode = KAnim.PlayMode.Loop;
				this.animController.initialAnim = "idle_loop";
				this.animController.gameObject.SetActive(true);
				if (this.animController.HasAnimation(this.animController.initialAnim))
				{
					this.animController.Play(this.animController.initialAnim, KAnim.PlayMode.Loop, 1f, 0f);
				}
			}
			else
			{
				this.animController.gameObject.SetActive(false);
				this.asteroidImage.gameObject.SetActive(true);
				this.asteroidImage.sprite = this.asteroidData.sprite;
				this.imageDlcFrom.gameObject.SetActive(false);
			}
			Sprite sprite = null;
			if (DlcManager.IsDlcId(this.asteroidData.Layout.dlcIdFrom))
			{
				sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(this.asteroidData.Layout.dlcIdFrom));
			}
			if (sprite != null)
			{
				this.imageDlcFrom.gameObject.SetActive(true);
				this.imageDlcFrom.sprite = sprite;
				return;
			}
			this.imageDlcFrom.gameObject.SetActive(false);
			this.imageDlcFrom.sprite = sprite;
		}
	}

	// Token: 0x06006687 RID: 26247 RVA: 0x002699A7 File Offset: 0x00267BA7
	private void OnClickInternal()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Clicked asteroid belt",
			this.asteroidData.beltPath
		});
		this.OnClicked(this.asteroidData);
	}

	// Token: 0x04004605 RID: 17925
	[SerializeField]
	private Image asteroidImage;

	// Token: 0x04004606 RID: 17926
	[SerializeField]
	private KButton button;

	// Token: 0x04004607 RID: 17927
	[SerializeField]
	private KBatchedAnimController animController;

	// Token: 0x04004608 RID: 17928
	[SerializeField]
	private Image imageDlcFrom;

	// Token: 0x0400460A RID: 17930
	private ColonyDestinationAsteroidBeltData asteroidData;
}
