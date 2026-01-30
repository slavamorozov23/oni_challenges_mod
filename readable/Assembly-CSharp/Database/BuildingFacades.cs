using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000F29 RID: 3881
	public class BuildingFacades : ResourceSet<BuildingFacadeResource>
	{
		// Token: 0x06007C2C RID: 31788 RVA: 0x003041A0 File Offset: 0x003023A0
		public BuildingFacades(ResourceSet parent) : base("BuildingFacades", parent)
		{
			base.Initialize();
			foreach (BuildingFacadeInfo buildingFacadeInfo in Blueprints.Get().all.buildingFacades)
			{
				this.Add(buildingFacadeInfo.id, buildingFacadeInfo.name, buildingFacadeInfo.desc, buildingFacadeInfo.rarity, buildingFacadeInfo.prefabId, buildingFacadeInfo.animFile, buildingFacadeInfo.workables, buildingFacadeInfo.GetRequiredDlcIds(), buildingFacadeInfo.GetForbiddenDlcIds());
			}
		}

		// Token: 0x06007C2D RID: 31789 RVA: 0x00304250 File Offset: 0x00302450
		public void Add(string id, LocString Name, LocString Desc, PermitRarity rarity, string prefabId, string animFile, Dictionary<string, string> workables = null, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			BuildingFacadeResource item = new BuildingFacadeResource(id, Name, Desc, rarity, prefabId, animFile, workables, requiredDlcIds, forbiddenDlcIds);
			this.resources.Add(item);
		}

		// Token: 0x06007C2E RID: 31790 RVA: 0x00304288 File Offset: 0x00302488
		public void PostProcess()
		{
			foreach (BuildingFacadeResource buildingFacadeResource in this.resources)
			{
				buildingFacadeResource.Init();
			}
		}
	}
}
