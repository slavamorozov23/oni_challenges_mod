using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database;
using UnityEngine;

// Token: 0x0200057F RID: 1407
public class BlueprintCollection
{
	// Token: 0x06001F4A RID: 8010 RVA: 0x000AACEA File Offset: 0x000A8EEA
	public void AddBlueprintsFrom<T>(T provider) where T : BlueprintProvider
	{
		provider.blueprintCollection = this;
		provider.Internal_PreSetupBlueprints();
		provider.SetupBlueprints();
	}

	// Token: 0x06001F4B RID: 8011 RVA: 0x000AAD10 File Offset: 0x000A8F10
	public void AddBlueprintsFrom(BlueprintCollection collection)
	{
		this.artables.AddRange(collection.artables);
		this.buildingFacades.AddRange(collection.buildingFacades);
		this.clothingItems.AddRange(collection.clothingItems);
		this.balloonArtistFacades.AddRange(collection.balloonArtistFacades);
		this.stickerBombFacades.AddRange(collection.stickerBombFacades);
		this.equippableFacades.AddRange(collection.equippableFacades);
		this.monumentParts.AddRange(collection.monumentParts);
		this.outfits.AddRange(collection.outfits);
	}

	// Token: 0x06001F4C RID: 8012 RVA: 0x000AADA8 File Offset: 0x000A8FA8
	public void PostProcess()
	{
		if (Application.isPlaying)
		{
			this.artables.RemoveAll(new Predicate<ArtableInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.buildingFacades.RemoveAll(new Predicate<BuildingFacadeInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.clothingItems.RemoveAll(new Predicate<ClothingItemInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.balloonArtistFacades.RemoveAll(new Predicate<BalloonArtistFacadeInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.stickerBombFacades.RemoveAll(new Predicate<StickerBombFacadeInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.equippableFacades.RemoveAll(new Predicate<EquippableFacadeInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.monumentParts.RemoveAll(new Predicate<MonumentPartInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.outfits.RemoveAll(new Predicate<ClothingOutfitResource>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
		}
	}

	// Token: 0x06001F4E RID: 8014 RVA: 0x000AAEEC File Offset: 0x000A90EC
	[CompilerGenerated]
	internal static bool <PostProcess>g__ShouldExcludeBlueprint|10_0(IHasDlcRestrictions blueprintDlcInfo)
	{
		if (!DlcManager.IsCorrectDlcSubscribed(blueprintDlcInfo))
		{
			return true;
		}
		IBlueprintInfo blueprintInfo = blueprintDlcInfo as IBlueprintInfo;
		KAnimFile kanimFile;
		if (blueprintInfo != null && !Assets.TryGetAnim(blueprintInfo.animFile, out kanimFile))
		{
			DebugUtil.DevAssert(false, string.Concat(new string[]
			{
				"Couldnt find anim \"",
				blueprintInfo.animFile,
				"\" for blueprint \"",
				blueprintInfo.id,
				"\""
			}), null);
		}
		return false;
	}

	// Token: 0x04001241 RID: 4673
	public List<ArtableInfo> artables = new List<ArtableInfo>();

	// Token: 0x04001242 RID: 4674
	public List<BuildingFacadeInfo> buildingFacades = new List<BuildingFacadeInfo>();

	// Token: 0x04001243 RID: 4675
	public List<ClothingItemInfo> clothingItems = new List<ClothingItemInfo>();

	// Token: 0x04001244 RID: 4676
	public List<BalloonArtistFacadeInfo> balloonArtistFacades = new List<BalloonArtistFacadeInfo>();

	// Token: 0x04001245 RID: 4677
	public List<StickerBombFacadeInfo> stickerBombFacades = new List<StickerBombFacadeInfo>();

	// Token: 0x04001246 RID: 4678
	public List<EquippableFacadeInfo> equippableFacades = new List<EquippableFacadeInfo>();

	// Token: 0x04001247 RID: 4679
	public List<MonumentPartInfo> monumentParts = new List<MonumentPartInfo>();

	// Token: 0x04001248 RID: 4680
	public List<ClothingOutfitResource> outfits = new List<ClothingOutfitResource>();
}
