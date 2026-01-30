using System;
using UnityEngine;

// Token: 0x02000E93 RID: 3731
public class SimpleTransformAnimation : MonoBehaviour
{
	// Token: 0x06007735 RID: 30517 RVA: 0x002D92E5 File Offset: 0x002D74E5
	private void Start()
	{
	}

	// Token: 0x06007736 RID: 30518 RVA: 0x002D92E7 File Offset: 0x002D74E7
	private void Update()
	{
		base.transform.Rotate(this.rotationSpeed * Time.unscaledDeltaTime);
		base.transform.Translate(this.translateSpeed * Time.unscaledDeltaTime);
	}

	// Token: 0x0400529F RID: 21151
	[SerializeField]
	private Vector3 rotationSpeed;

	// Token: 0x040052A0 RID: 21152
	[SerializeField]
	private Vector3 translateSpeed;
}
