using System;
using System.Collections.Generic;

// Token: 0x02000992 RID: 2450
[Obsolete("No longer used. Use IGameObjectEffectDescriptor instead", false)]
public interface IEffectDescriptor
{
	// Token: 0x06004677 RID: 18039
	List<Descriptor> GetDescriptors(BuildingDef def);
}
