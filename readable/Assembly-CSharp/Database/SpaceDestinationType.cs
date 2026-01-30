using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Database
{
	// Token: 0x02000F58 RID: 3928
	[DebuggerDisplay("{Id}")]
	public class SpaceDestinationType : Resource
	{
		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06007CC8 RID: 31944 RVA: 0x00317AAC File Offset: 0x00315CAC
		// (set) Token: 0x06007CC9 RID: 31945 RVA: 0x00317AB4 File Offset: 0x00315CB4
		public int maxiumMass { get; private set; }

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06007CCA RID: 31946 RVA: 0x00317ABD File Offset: 0x00315CBD
		// (set) Token: 0x06007CCB RID: 31947 RVA: 0x00317AC5 File Offset: 0x00315CC5
		public int minimumMass { get; private set; }

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06007CCC RID: 31948 RVA: 0x00317ACE File Offset: 0x00315CCE
		public float replishmentPerCycle
		{
			get
			{
				return 1000f / (float)this.cyclesToRecover;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06007CCD RID: 31949 RVA: 0x00317ADD File Offset: 0x00315CDD
		public float replishmentPerSim1000ms
		{
			get
			{
				return 1000f / ((float)this.cyclesToRecover * 600f);
			}
		}

		// Token: 0x06007CCE RID: 31950 RVA: 0x00317AF4 File Offset: 0x00315CF4
		public SpaceDestinationType(string id, ResourceSet parent, string name, string description, int iconSize, string spriteName, Dictionary<SimHashes, MathUtil.MinMax> elementTable, Dictionary<string, int> recoverableEntities = null, ArtifactDropRate artifactDropRate = null, int max = 64000000, int min = 63994000, int cycles = 6, bool visitable = true) : base(id, parent, name)
		{
			this.typeName = name;
			this.description = description;
			this.iconSize = iconSize;
			this.spriteName = spriteName;
			this.elementTable = elementTable;
			this.recoverableEntities = recoverableEntities;
			this.artifactDropTable = artifactDropRate;
			this.maxiumMass = max;
			this.minimumMass = min;
			this.cyclesToRecover = cycles;
			this.visitable = visitable;
		}

		// Token: 0x04005B6B RID: 23403
		public const float MASS_TO_RECOVER = 1000f;

		// Token: 0x04005B6C RID: 23404
		public string typeName;

		// Token: 0x04005B6D RID: 23405
		public string description;

		// Token: 0x04005B6E RID: 23406
		public int iconSize = 128;

		// Token: 0x04005B6F RID: 23407
		public string spriteName;

		// Token: 0x04005B70 RID: 23408
		public Dictionary<SimHashes, MathUtil.MinMax> elementTable;

		// Token: 0x04005B71 RID: 23409
		public Dictionary<string, int> recoverableEntities;

		// Token: 0x04005B72 RID: 23410
		public ArtifactDropRate artifactDropTable;

		// Token: 0x04005B73 RID: 23411
		public bool visitable;

		// Token: 0x04005B76 RID: 23414
		public int cyclesToRecover;
	}
}
