using System;

namespace Database
{
	// Token: 0x02000F3B RID: 3899
	public class EquippableFacades : ResourceSet<EquippableFacadeResource>
	{
		// Token: 0x06007C65 RID: 31845 RVA: 0x00310E70 File Offset: 0x0030F070
		public EquippableFacades(ResourceSet parent) : base("EquippableFacades", parent)
		{
			base.Initialize();
			foreach (EquippableFacadeInfo equippableFacadeInfo in Blueprints.Get().all.equippableFacades)
			{
				this.Add(equippableFacadeInfo.id, equippableFacadeInfo.name, equippableFacadeInfo.desc, equippableFacadeInfo.rarity, equippableFacadeInfo.defID, equippableFacadeInfo.buildOverride, equippableFacadeInfo.animFile, equippableFacadeInfo.GetRequiredDlcIds(), equippableFacadeInfo.GetForbiddenDlcIds());
			}
		}

		// Token: 0x06007C66 RID: 31846 RVA: 0x00310F14 File Offset: 0x0030F114
		[Obsolete("Please use Add(...) with required forbidden")]
		public void Add(string id, string name, string desc, PermitRarity rarity, string defID, string buildOverride, string animFile)
		{
			this.Add(id, name, desc, rarity, defID, buildOverride, animFile, null, null);
		}

		// Token: 0x06007C67 RID: 31847 RVA: 0x00310F34 File Offset: 0x0030F134
		[Obsolete("Please use Add(...) with required forbidden")]
		public void Add(string id, string name, string desc, PermitRarity rarity, string defID, string buildOverride, string animFile, string[] dlcIds)
		{
			DlcRestrictionsUtil.TemporaryHelperObject transientHelperObjectFromAllowList = DlcRestrictionsUtil.GetTransientHelperObjectFromAllowList(dlcIds);
			EquippableFacadeResource item = new EquippableFacadeResource(id, name, desc, rarity, buildOverride, defID, animFile, transientHelperObjectFromAllowList.GetRequiredDlcIds(), transientHelperObjectFromAllowList.GetForbiddenDlcIds());
			this.resources.Add(item);
		}

		// Token: 0x06007C68 RID: 31848 RVA: 0x00310F74 File Offset: 0x0030F174
		public void Add(string id, string name, string desc, PermitRarity rarity, string defID, string buildOverride, string animFile, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			EquippableFacadeResource item = new EquippableFacadeResource(id, name, desc, rarity, buildOverride, defID, animFile, requiredDlcIds, forbiddenDlcIds);
			this.resources.Add(item);
		}
	}
}
