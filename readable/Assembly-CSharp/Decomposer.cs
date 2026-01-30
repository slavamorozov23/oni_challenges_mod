using System;
using UnityEngine;

// Token: 0x020008CE RID: 2254
[AddComponentMenu("KMonoBehaviour/scripts/Decomposer")]
public class Decomposer : KMonoBehaviour
{
	// Token: 0x06003E76 RID: 15990 RVA: 0x0015DB1C File Offset: 0x0015BD1C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		StateMachineController component = base.GetComponent<StateMachineController>();
		if (component == null)
		{
			return;
		}
		DecompositionMonitor.Instance instance = new DecompositionMonitor.Instance(this, null, 1f, false);
		component.AddStateMachineInstance(instance);
		instance.StartSM();
		instance.dirtyWaterMaxRange = 3;
	}
}
