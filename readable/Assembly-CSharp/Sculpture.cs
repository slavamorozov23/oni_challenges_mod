using System;
using UnityEngine;

// Token: 0x02000634 RID: 1588
public class Sculpture : Artable
{
	// Token: 0x060025F4 RID: 9716 RVA: 0x000DA680 File Offset: 0x000D8880
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (Sculpture.sculptureOverrides == null)
		{
			Sculpture.sculptureOverrides = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_sculpture_kanim")
			};
		}
		this.overrideAnims = Sculpture.sculptureOverrides;
		this.synchronizeAnims = false;
	}

	// Token: 0x060025F5 RID: 9717 RVA: 0x000DA6C0 File Offset: 0x000D88C0
	public override void SetStage(string stage_id, bool skip_effect)
	{
		base.SetStage(stage_id, skip_effect);
		bool flag = base.CurrentStage == "Default";
		if (Db.GetArtableStages().Get(stage_id) == null)
		{
			global::Debug.LogError("Missing stage: " + stage_id);
		}
		if (!skip_effect && !flag)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("sculpture_fx_kanim", base.transform.GetPosition(), base.transform, false, Grid.SceneLayer.Front, false);
			kbatchedAnimController.destroyOnAnimComplete = true;
			kbatchedAnimController.transform.SetLocalPosition(Vector3.zero);
			kbatchedAnimController.Play("poof", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x04001660 RID: 5728
	private static KAnimFile[] sculptureOverrides;
}
