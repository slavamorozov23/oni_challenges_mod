using System;
using UnityEngine;

// Token: 0x020004DB RID: 1243
[AddComponentMenu("KMonoBehaviour/scripts/User")]
public class User : KMonoBehaviour
{
	// Token: 0x06001AC3 RID: 6851 RVA: 0x000938BF File Offset: 0x00091ABF
	public void OnStateMachineStop(string reason, StateMachine.Status status)
	{
		if (status == StateMachine.Status.Success)
		{
			base.Trigger(58624316, null);
			return;
		}
		base.Trigger(1572098533, null);
	}
}
