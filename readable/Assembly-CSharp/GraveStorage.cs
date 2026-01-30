using System;
using System.Collections.Generic;

// Token: 0x020005E7 RID: 1511
public class GraveStorage : Storage
{
	// Token: 0x060022F9 RID: 8953 RVA: 0x000CB68C File Offset: 0x000C988C
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		KAnimFile[] overrideAnims = null;
		if (this.workerTypeOverrideAnims.TryGetValue(worker.PrefabID(), out overrideAnims))
		{
			this.overrideAnims = overrideAnims;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x0400147F RID: 5247
	public Dictionary<Tag, KAnimFile[]> workerTypeOverrideAnims = new Dictionary<Tag, KAnimFile[]>();
}
