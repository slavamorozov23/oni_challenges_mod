using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F2A RID: 3882
	public class BuildingFacadeResource : PermitResource
	{
		// Token: 0x06007C2F RID: 31791 RVA: 0x003042D8 File Offset: 0x003024D8
		[Obsolete("Please use constructor with dlcIds parameter")]
		public BuildingFacadeResource(string Id, string Name, string Description, PermitRarity Rarity, string PrefabID, string AnimFile, Dictionary<string, string> workables = null) : this(Id, Name, Description, Rarity, PrefabID, AnimFile, workables, null, null)
		{
		}

		// Token: 0x06007C30 RID: 31792 RVA: 0x003042F8 File Offset: 0x003024F8
		[Obsolete("Please use constructor with dlcIds parameter")]
		public BuildingFacadeResource(string Id, string Name, string Description, PermitRarity Rarity, string PrefabID, string AnimFile, string[] dlcIds, Dictionary<string, string> workables = null) : this(Id, Name, Description, Rarity, PrefabID, AnimFile, workables, null, null)
		{
		}

		// Token: 0x06007C31 RID: 31793 RVA: 0x00304318 File Offset: 0x00302518
		public BuildingFacadeResource(string Id, string Name, string Description, PermitRarity Rarity, string PrefabID, string AnimFile, Dictionary<string, string> workables = null, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : base(Id, Name, Description, PermitCategory.Building, Rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.Id = Id;
			this.PrefabID = PrefabID;
			this.AnimFile = AnimFile;
			this.InteractFile = workables;
		}

		// Token: 0x06007C32 RID: 31794 RVA: 0x0030434C File Offset: 0x0030254C
		public void Init()
		{
			GameObject gameObject = Assets.TryGetPrefab(this.PrefabID);
			if (gameObject == null)
			{
				return;
			}
			gameObject.AddOrGet<BuildingFacade>();
			BuildingDef def = gameObject.GetComponent<Building>().Def;
			if (def != null)
			{
				def.AddFacade(this.Id);
				KAnimFileData data = def.AnimFiles[0].GetData();
				KAnimFileData data2 = Assets.GetAnim(this.AnimFile).GetData();
				for (int i = 0; i < data.animCount; i++)
				{
					KAnim.Anim anim = data.GetAnim(i);
					KAnim.Anim anim2 = data2.GetAnim(anim.name);
					if (anim2 != null)
					{
						bool flag = GameAudioSheets.Get().events.ContainsKey(anim.id);
						if (!GameAudioSheets.Get().events.ContainsKey(anim2.id) && flag)
						{
							GameAudioSheets.Get().skinToBaseAnim[anim2.id] = anim.id;
						}
					}
				}
			}
		}

		// Token: 0x06007C33 RID: 31795 RVA: 0x00304448 File Offset: 0x00302648
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(this.AnimFile), "ui", false, "");
			result.SetFacadeForPrefabID(this.PrefabID);
			return result;
		}

		// Token: 0x040056F9 RID: 22265
		public string PrefabID;

		// Token: 0x040056FA RID: 22266
		public string AnimFile;

		// Token: 0x040056FB RID: 22267
		public Dictionary<string, string> InteractFile;
	}
}
