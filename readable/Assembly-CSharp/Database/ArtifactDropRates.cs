using System;
using TUNING;

namespace Database
{
	// Token: 0x02000F20 RID: 3872
	public class ArtifactDropRates : ResourceSet<ArtifactDropRate>
	{
		// Token: 0x06007C11 RID: 31761 RVA: 0x00302977 File Offset: 0x00300B77
		public ArtifactDropRates(ResourceSet parent) : base("ArtifactDropRates", parent)
		{
			this.CreateDropRates();
		}

		// Token: 0x06007C12 RID: 31762 RVA: 0x0030298C File Offset: 0x00300B8C
		private void CreateDropRates()
		{
			this.None = new ArtifactDropRate();
			this.None.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 1f);
			base.Add(this.None);
			this.Bad = new ArtifactDropRate();
			this.Bad.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Bad.AddItem(DECOR.SPACEARTIFACT.TIER0, 5f);
			this.Bad.AddItem(DECOR.SPACEARTIFACT.TIER1, 3f);
			this.Bad.AddItem(DECOR.SPACEARTIFACT.TIER2, 2f);
			base.Add(this.Bad);
			this.Mediocre = new ArtifactDropRate();
			this.Mediocre.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Mediocre.AddItem(DECOR.SPACEARTIFACT.TIER1, 5f);
			this.Mediocre.AddItem(DECOR.SPACEARTIFACT.TIER2, 3f);
			this.Mediocre.AddItem(DECOR.SPACEARTIFACT.TIER3, 2f);
			base.Add(this.Mediocre);
			this.Good = new ArtifactDropRate();
			this.Good.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Good.AddItem(DECOR.SPACEARTIFACT.TIER2, 5f);
			this.Good.AddItem(DECOR.SPACEARTIFACT.TIER3, 3f);
			this.Good.AddItem(DECOR.SPACEARTIFACT.TIER4, 2f);
			base.Add(this.Good);
			this.Great = new ArtifactDropRate();
			this.Great.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Great.AddItem(DECOR.SPACEARTIFACT.TIER3, 5f);
			this.Great.AddItem(DECOR.SPACEARTIFACT.TIER4, 3f);
			this.Great.AddItem(DECOR.SPACEARTIFACT.TIER5, 2f);
			base.Add(this.Great);
			this.Amazing = new ArtifactDropRate();
			this.Amazing.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Amazing.AddItem(DECOR.SPACEARTIFACT.TIER3, 3f);
			this.Amazing.AddItem(DECOR.SPACEARTIFACT.TIER4, 5f);
			this.Amazing.AddItem(DECOR.SPACEARTIFACT.TIER5, 2f);
			base.Add(this.Amazing);
			this.Perfect = new ArtifactDropRate();
			this.Perfect.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Perfect.AddItem(DECOR.SPACEARTIFACT.TIER4, 6f);
			this.Perfect.AddItem(DECOR.SPACEARTIFACT.TIER5, 4f);
			base.Add(this.Perfect);
		}

		// Token: 0x04005696 RID: 22166
		public ArtifactDropRate None;

		// Token: 0x04005697 RID: 22167
		public ArtifactDropRate Bad;

		// Token: 0x04005698 RID: 22168
		public ArtifactDropRate Mediocre;

		// Token: 0x04005699 RID: 22169
		public ArtifactDropRate Good;

		// Token: 0x0400569A RID: 22170
		public ArtifactDropRate Great;

		// Token: 0x0400569B RID: 22171
		public ArtifactDropRate Amazing;

		// Token: 0x0400569C RID: 22172
		public ArtifactDropRate Perfect;
	}
}
