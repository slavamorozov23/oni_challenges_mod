using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000F4C RID: 3916
	public class PermitResources : ResourceSet<PermitResource>
	{
		// Token: 0x06007CA5 RID: 31909 RVA: 0x00314F1C File Offset: 0x0031311C
		public PermitResources(ResourceSet parent) : base("PermitResources", parent)
		{
			this.Root = new ResourceSet<Resource>("Root", null);
			this.Permits = new Dictionary<string, IEnumerable<PermitResource>>();
			this.BuildingFacades = new BuildingFacades(this.Root);
			this.Permits.Add(this.BuildingFacades.Id, this.BuildingFacades.resources);
			this.EquippableFacades = new EquippableFacades(this.Root);
			this.Permits.Add(this.EquippableFacades.Id, this.EquippableFacades.resources);
			this.ArtableStages = new ArtableStages(this.Root);
			this.Permits.Add(this.ArtableStages.Id, this.ArtableStages.resources);
			this.StickerBombs = new StickerBombs(this.Root);
			this.Permits.Add(this.StickerBombs.Id, this.StickerBombs.resources);
			this.ClothingItems = new ClothingItems(this.Root);
			this.ClothingOutfits = new ClothingOutfits(this.Root, this.ClothingItems);
			this.Permits.Add(this.ClothingItems.Id, this.ClothingItems.resources);
			this.BalloonArtistFacades = new BalloonArtistFacades(this.Root);
			this.Permits.Add(this.BalloonArtistFacades.Id, this.BalloonArtistFacades.resources);
			this.MonumentParts = new MonumentParts(this.Root);
			this.Permits.Add(this.MonumentParts.Id, this.MonumentParts.resources);
			foreach (IEnumerable<PermitResource> collection in this.Permits.Values)
			{
				this.resources.AddRange(collection);
			}
		}

		// Token: 0x06007CA6 RID: 31910 RVA: 0x00315118 File Offset: 0x00313318
		public void PostProcess()
		{
			this.BuildingFacades.PostProcess();
		}

		// Token: 0x04005B0E RID: 23310
		public ResourceSet Root;

		// Token: 0x04005B0F RID: 23311
		public BuildingFacades BuildingFacades;

		// Token: 0x04005B10 RID: 23312
		public EquippableFacades EquippableFacades;

		// Token: 0x04005B11 RID: 23313
		public ArtableStages ArtableStages;

		// Token: 0x04005B12 RID: 23314
		public StickerBombs StickerBombs;

		// Token: 0x04005B13 RID: 23315
		public ClothingItems ClothingItems;

		// Token: 0x04005B14 RID: 23316
		public ClothingOutfits ClothingOutfits;

		// Token: 0x04005B15 RID: 23317
		public MonumentParts MonumentParts;

		// Token: 0x04005B16 RID: 23318
		public BalloonArtistFacades BalloonArtistFacades;

		// Token: 0x04005B17 RID: 23319
		public Dictionary<string, IEnumerable<PermitResource>> Permits;
	}
}
