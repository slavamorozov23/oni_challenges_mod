using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000F42 RID: 3906
	public class MonumentParts : ResourceSet<MonumentPartResource>
	{
		// Token: 0x06007C84 RID: 31876 RVA: 0x003141F4 File Offset: 0x003123F4
		public MonumentParts(ResourceSet parent) : base("MonumentParts", parent)
		{
			base.Initialize();
			foreach (MonumentPartInfo monumentPartInfo in Blueprints.Get().all.monumentParts)
			{
				this.Add(monumentPartInfo.id, monumentPartInfo.name, monumentPartInfo.desc, monumentPartInfo.rarity, monumentPartInfo.animFile, monumentPartInfo.state, monumentPartInfo.symbolName, monumentPartInfo.part, monumentPartInfo.requiredDlcIds, monumentPartInfo.forbiddenDlcIds);
			}
		}

		// Token: 0x06007C85 RID: 31877 RVA: 0x003142A0 File Offset: 0x003124A0
		public void Add(string id, string name, string desc, PermitRarity rarity, string animFilename, string state, string symbolName, MonumentPartResource.Part part, string[] requiredDlcIds, string[] forbiddenDlcIds)
		{
			MonumentPartResource item = new MonumentPartResource(id, name, desc, rarity, animFilename, state, symbolName, part, requiredDlcIds, forbiddenDlcIds);
			this.resources.Add(item);
		}

		// Token: 0x06007C86 RID: 31878 RVA: 0x003142D0 File Offset: 0x003124D0
		public List<MonumentPartResource> GetParts(MonumentPartResource.Part part)
		{
			return this.resources.FindAll((MonumentPartResource mpr) => mpr.part == part);
		}
	}
}
