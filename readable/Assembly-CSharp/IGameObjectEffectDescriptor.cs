using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000991 RID: 2449
public interface IGameObjectEffectDescriptor
{
	// Token: 0x06004676 RID: 18038
	List<Descriptor> GetDescriptors(GameObject go);
}
