using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x0200103A RID: 4154
	public class CustomSickEffectSickness : Sickness.SicknessComponent
	{
		// Token: 0x060080C7 RID: 32967 RVA: 0x0033C162 File Offset: 0x0033A362
		public CustomSickEffectSickness(string effect_kanim, string effect_anim_name)
		{
			this.kanim = effect_kanim;
			this.animName = effect_anim_name;
		}

		// Token: 0x060080C8 RID: 32968 RVA: 0x0033C178 File Offset: 0x0033A378
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(this.kanim, go.transform.GetPosition() + new Vector3(0f, 0f, -0.1f), go.transform, true, Grid.SceneLayer.Front, false);
			kbatchedAnimController.Play(this.animName, KAnim.PlayMode.Loop, 1f, 0f);
			return kbatchedAnimController;
		}

		// Token: 0x060080C9 RID: 32969 RVA: 0x0033C1DA File Offset: 0x0033A3DA
		public override void OnCure(GameObject go, object instance_data)
		{
			((KAnimControllerBase)instance_data).gameObject.DeleteObject();
		}

		// Token: 0x0400618A RID: 24970
		private string kanim;

		// Token: 0x0400618B RID: 24971
		private string animName;
	}
}
