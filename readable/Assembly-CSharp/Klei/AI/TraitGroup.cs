using System;

namespace Klei.AI
{
	// Token: 0x02001059 RID: 4185
	public class TraitGroup : ModifierGroup<Trait>
	{
		// Token: 0x0600819E RID: 33182 RVA: 0x0033FB41 File Offset: 0x0033DD41
		public TraitGroup(string id, string name, bool is_spawn_trait) : base(id, name)
		{
			this.IsSpawnTrait = is_spawn_trait;
		}

		// Token: 0x04006211 RID: 25105
		public bool IsSpawnTrait;
	}
}
