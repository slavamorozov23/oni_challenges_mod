using System;
using UnityEngine;

// Token: 0x02000EA8 RID: 3752
public class SwapUIAnimationController : MonoBehaviour
{
	// Token: 0x06007831 RID: 30769 RVA: 0x002E3BB4 File Offset: 0x002E1DB4
	public void SetState(bool Primary)
	{
		this.AnimationControllerObject_Primary.SetActive(Primary);
		if (!Primary)
		{
			this.AnimationControllerObject_Alternate.GetComponent<KAnimControllerBase>().TintColour = new Color(1f, 1f, 1f, 0.5f);
			this.AnimationControllerObject_Primary.GetComponent<KAnimControllerBase>().TintColour = Color.clear;
		}
		this.AnimationControllerObject_Alternate.SetActive(!Primary);
		if (Primary)
		{
			this.AnimationControllerObject_Primary.GetComponent<KAnimControllerBase>().TintColour = Color.white;
			this.AnimationControllerObject_Alternate.GetComponent<KAnimControllerBase>().TintColour = Color.clear;
		}
	}

	// Token: 0x040053C7 RID: 21447
	public GameObject AnimationControllerObject_Primary;

	// Token: 0x040053C8 RID: 21448
	public GameObject AnimationControllerObject_Alternate;
}
