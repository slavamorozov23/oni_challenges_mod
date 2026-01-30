using System;
using ProcGen;

namespace Database
{
	// Token: 0x02000F67 RID: 3943
	public class Story : Resource, IComparable<Story>
	{
		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06007D08 RID: 32008 RVA: 0x0031D2E5 File Offset: 0x0031B4E5
		// (set) Token: 0x06007D09 RID: 32009 RVA: 0x0031D2ED File Offset: 0x0031B4ED
		public int HashId { get; private set; }

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06007D0A RID: 32010 RVA: 0x0031D2F6 File Offset: 0x0031B4F6
		public WorldTrait StoryTrait
		{
			get
			{
				if (this._cachedStoryTrait == null)
				{
					this._cachedStoryTrait = SettingsCache.GetCachedStoryTrait(this.worldgenStoryTraitKey, false);
				}
				return this._cachedStoryTrait;
			}
		}

		// Token: 0x06007D0B RID: 32011 RVA: 0x0031D318 File Offset: 0x0031B518
		public Story(string id, string worldgenStoryTraitKey, int displayOrder)
		{
			this.Id = id;
			this.worldgenStoryTraitKey = worldgenStoryTraitKey;
			this.displayOrder = displayOrder;
			this.kleiUseOnlyCoordinateOrder = -1;
			this.updateNumber = -1;
			this.sandboxStampTemplateId = null;
			this.HashId = Hash.SDBMLower(id);
		}

		// Token: 0x06007D0C RID: 32012 RVA: 0x0031D358 File Offset: 0x0031B558
		public Story(string id, string worldgenStoryTraitKey, int displayOrder, int kleiUseOnlyCoordinateOrder, int updateNumber, string sandboxStampTemplateId)
		{
			this.Id = id;
			this.worldgenStoryTraitKey = worldgenStoryTraitKey;
			this.displayOrder = displayOrder;
			this.updateNumber = updateNumber;
			this.sandboxStampTemplateId = sandboxStampTemplateId;
			this.kleiUseOnlyCoordinateOrder = kleiUseOnlyCoordinateOrder;
			this.HashId = Hash.SDBMLower(id);
		}

		// Token: 0x06007D0D RID: 32013 RVA: 0x0031D3A4 File Offset: 0x0031B5A4
		public int CompareTo(Story other)
		{
			return this.displayOrder.CompareTo(other.displayOrder);
		}

		// Token: 0x06007D0E RID: 32014 RVA: 0x0031D3C5 File Offset: 0x0031B5C5
		public bool IsNew()
		{
			return this.updateNumber == LaunchInitializer.UpdateNumber();
		}

		// Token: 0x06007D0F RID: 32015 RVA: 0x0031D3D4 File Offset: 0x0031B5D4
		public Story AutoStart()
		{
			this.autoStart = true;
			return this;
		}

		// Token: 0x06007D10 RID: 32016 RVA: 0x0031D3DE File Offset: 0x0031B5DE
		public Story SetKeepsake(string prefabId)
		{
			this.keepsakePrefabId = prefabId;
			return this;
		}

		// Token: 0x04005C1D RID: 23581
		public const int MODDED_STORY = -1;

		// Token: 0x04005C1E RID: 23582
		public int kleiUseOnlyCoordinateOrder;

		// Token: 0x04005C20 RID: 23584
		public bool autoStart;

		// Token: 0x04005C21 RID: 23585
		public string keepsakePrefabId;

		// Token: 0x04005C22 RID: 23586
		public readonly string worldgenStoryTraitKey;

		// Token: 0x04005C23 RID: 23587
		private readonly int displayOrder;

		// Token: 0x04005C24 RID: 23588
		private readonly int updateNumber;

		// Token: 0x04005C25 RID: 23589
		public string sandboxStampTemplateId;

		// Token: 0x04005C26 RID: 23590
		private WorldTrait _cachedStoryTrait;
	}
}
