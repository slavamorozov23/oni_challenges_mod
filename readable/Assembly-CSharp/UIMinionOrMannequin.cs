using System;
using System.Collections.Generic;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000EBB RID: 3771
public class UIMinionOrMannequin : KMonoBehaviour
{
	// Token: 0x1700084C RID: 2124
	// (get) Token: 0x060078CE RID: 30926 RVA: 0x002E7711 File Offset: 0x002E5911
	// (set) Token: 0x060078CF RID: 30927 RVA: 0x002E7719 File Offset: 0x002E5919
	public UIMinionOrMannequin.ITarget current { get; private set; }

	// Token: 0x060078D0 RID: 30928 RVA: 0x002E7722 File Offset: 0x002E5922
	protected override void OnSpawn()
	{
		this.TrySpawn();
	}

	// Token: 0x060078D1 RID: 30929 RVA: 0x002E772C File Offset: 0x002E592C
	public bool TrySpawn()
	{
		bool flag = false;
		if (this.mannequin.IsNullOrDestroyed())
		{
			GameObject gameObject = new GameObject("UIMannequin");
			gameObject.AddOrGet<RectTransform>().Fill(Padding.All(10f));
			gameObject.transform.SetParent(base.transform, false);
			AspectRatioFitter aspectRatioFitter = gameObject.AddOrGet<AspectRatioFitter>();
			aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
			aspectRatioFitter.aspectRatio = 1f;
			this.mannequin = gameObject.AddOrGet<UIMannequin>();
			this.mannequin.TrySpawn();
			gameObject.SetActive(false);
			flag = true;
		}
		if (this.minion.IsNullOrDestroyed())
		{
			GameObject gameObject2 = new GameObject("UIMinion");
			gameObject2.AddOrGet<RectTransform>().Fill(Padding.All(10f));
			gameObject2.transform.SetParent(base.transform, false);
			AspectRatioFitter aspectRatioFitter2 = gameObject2.AddOrGet<AspectRatioFitter>();
			aspectRatioFitter2.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
			aspectRatioFitter2.aspectRatio = 1f;
			this.minion = gameObject2.AddOrGet<UIMinion>();
			this.minion.TrySpawn();
			gameObject2.SetActive(false);
			flag = true;
		}
		if (flag)
		{
			this.SetAsMannequin();
		}
		return flag;
	}

	// Token: 0x060078D2 RID: 30930 RVA: 0x002E7834 File Offset: 0x002E5A34
	public UIMinionOrMannequin.ITarget SetFrom(Option<Personality> personality)
	{
		if (personality.IsSome())
		{
			return this.SetAsMinion(personality.Unwrap());
		}
		return this.SetAsMannequin();
	}

	// Token: 0x060078D3 RID: 30931 RVA: 0x002E7854 File Offset: 0x002E5A54
	public UIMinion SetAsMinion(Personality personality)
	{
		this.mannequin.gameObject.SetActive(false);
		this.minion.gameObject.SetActive(true);
		this.minion.SetMinion(personality);
		this.current = this.minion;
		return this.minion;
	}

	// Token: 0x060078D4 RID: 30932 RVA: 0x002E78A1 File Offset: 0x002E5AA1
	public UIMannequin SetAsMannequin()
	{
		this.minion.gameObject.SetActive(false);
		this.mannequin.gameObject.SetActive(true);
		this.current = this.mannequin;
		return this.mannequin;
	}

	// Token: 0x060078D5 RID: 30933 RVA: 0x002E78D8 File Offset: 0x002E5AD8
	public MinionVoice GetMinionVoice()
	{
		return MinionVoice.ByObject(this.current.SpawnedAvatar).UnwrapOr(MinionVoice.Random(), null);
	}

	// Token: 0x0400543F RID: 21567
	public UIMinion minion;

	// Token: 0x04005440 RID: 21568
	public UIMannequin mannequin;

	// Token: 0x02002120 RID: 8480
	public interface ITarget
	{
		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x0600BB75 RID: 47989
		GameObject SpawnedAvatar { get; }

		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x0600BB76 RID: 47990
		Option<Personality> Personality { get; }

		// Token: 0x0600BB77 RID: 47991
		void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> clothingItems);

		// Token: 0x0600BB78 RID: 47992
		void React(UIMinionOrMannequinReactSource source);
	}
}
