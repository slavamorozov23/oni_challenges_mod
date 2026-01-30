using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001039 RID: 4153
	public class CommonSickEffectSickness : Sickness.SicknessComponent
	{
		// Token: 0x060080C4 RID: 32964 RVA: 0x0033C0E8 File Offset: 0x0033A2E8
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("contaminated_crew_fx_kanim", go.transform.GetPosition() + new Vector3(0f, 0f, -0.1f), go.transform, true, Grid.SceneLayer.Front, false);
			kbatchedAnimController.Play("fx_loop", KAnim.PlayMode.Loop, 1f, 0f);
			return kbatchedAnimController;
		}

		// Token: 0x060080C5 RID: 32965 RVA: 0x0033C148 File Offset: 0x0033A348
		public override void OnCure(GameObject go, object instance_data)
		{
			((KAnimControllerBase)instance_data).gameObject.DeleteObject();
		}
	}
}
