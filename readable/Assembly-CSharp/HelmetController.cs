using System;
using UnityEngine;

// Token: 0x02000936 RID: 2358
[AddComponentMenu("KMonoBehaviour/scripts/HelmetController")]
public class HelmetController : KMonoBehaviour
{
	// Token: 0x060041EE RID: 16878 RVA: 0x001740D8 File Offset: 0x001722D8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HelmetController>(-1617557748, HelmetController.OnEquippedDelegate);
		base.Subscribe<HelmetController>(-170173755, HelmetController.OnUnequippedDelegate);
	}

	// Token: 0x060041EF RID: 16879 RVA: 0x00174104 File Offset: 0x00172304
	private KBatchedAnimController GetAssigneeController()
	{
		Equippable component = base.GetComponent<Equippable>();
		if (component.assignee != null)
		{
			GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
			if (assigneeGameObject)
			{
				return assigneeGameObject.GetComponent<KBatchedAnimController>();
			}
		}
		return null;
	}

	// Token: 0x060041F0 RID: 16880 RVA: 0x00174140 File Offset: 0x00172340
	private GameObject GetAssigneeGameObject(IAssignableIdentity ass_id)
	{
		GameObject result = null;
		MinionAssignablesProxy minionAssignablesProxy = ass_id as MinionAssignablesProxy;
		if (minionAssignablesProxy)
		{
			result = minionAssignablesProxy.GetTargetGameObject();
		}
		else
		{
			MinionIdentity minionIdentity = ass_id as MinionIdentity;
			if (minionIdentity)
			{
				result = minionIdentity.gameObject;
			}
		}
		return result;
	}

	// Token: 0x060041F1 RID: 16881 RVA: 0x00174180 File Offset: 0x00172380
	private void OnEquipped(object data)
	{
		Equippable component = base.GetComponent<Equippable>();
		this.ShowHelmet();
		GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
		assigneeGameObject.Subscribe(961737054, new Action<object>(this.OnBeginRecoverBreath));
		assigneeGameObject.Subscribe(-2037519664, new Action<object>(this.OnEndRecoverBreath));
		assigneeGameObject.Subscribe(1347184327, new Action<object>(this.OnPathAdvanced));
		this.in_tube = false;
		this.is_flying = false;
		this.owner_navigator = assigneeGameObject.GetComponent<Navigator>();
	}

	// Token: 0x060041F2 RID: 16882 RVA: 0x0017420C File Offset: 0x0017240C
	private void OnUnequipped(object data)
	{
		this.owner_navigator = null;
		Equippable component = base.GetComponent<Equippable>();
		if (component != null)
		{
			this.HideHelmet();
			if (component.assignee != null)
			{
				GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
				if (assigneeGameObject)
				{
					assigneeGameObject.Unsubscribe(961737054, new Action<object>(this.OnBeginRecoverBreath));
					assigneeGameObject.Unsubscribe(-2037519664, new Action<object>(this.OnEndRecoverBreath));
					assigneeGameObject.Unsubscribe(1347184327, new Action<object>(this.OnPathAdvanced));
				}
			}
		}
	}

	// Token: 0x060041F3 RID: 16883 RVA: 0x00174298 File Offset: 0x00172498
	private void ShowHelmet()
	{
		KBatchedAnimController assigneeController = this.GetAssigneeController();
		if (assigneeController == null)
		{
			return;
		}
		KAnimHashedString kanimHashedString = new KAnimHashedString("snapTo_neck");
		if (!string.IsNullOrEmpty(this.anim_file))
		{
			KAnimFile anim = Assets.GetAnim(this.anim_file);
			assigneeController.GetComponent<SymbolOverrideController>().AddSymbolOverride(kanimHashedString, anim.GetData().build.GetSymbol(kanimHashedString), 6);
		}
		assigneeController.SetSymbolVisiblity(kanimHashedString, true);
		this.is_shown = true;
		this.UpdateJets();
	}

	// Token: 0x060041F4 RID: 16884 RVA: 0x0017431C File Offset: 0x0017251C
	private void HideHelmet()
	{
		this.is_shown = false;
		KBatchedAnimController assigneeController = this.GetAssigneeController();
		if (assigneeController == null)
		{
			return;
		}
		KAnimHashedString kanimHashedString = "snapTo_neck";
		if (!string.IsNullOrEmpty(this.anim_file))
		{
			SymbolOverrideController component = assigneeController.GetComponent<SymbolOverrideController>();
			if (component == null)
			{
				return;
			}
			component.RemoveSymbolOverride(kanimHashedString, 6);
		}
		assigneeController.SetSymbolVisiblity(kanimHashedString, false);
		this.UpdateJets();
	}

	// Token: 0x060041F5 RID: 16885 RVA: 0x00174386 File Offset: 0x00172586
	private void UpdateJets()
	{
		if (this.is_shown && this.is_flying)
		{
			this.EnableJets();
			return;
		}
		this.DisableJets();
	}

	// Token: 0x060041F6 RID: 16886 RVA: 0x001743A8 File Offset: 0x001725A8
	private void EnableJets()
	{
		if (!this.has_jets)
		{
			return;
		}
		if (this.jet_anim != null)
		{
			return;
		}
		this.jet_anim = this.AddTrackedAnim("jet", Assets.GetAnim("jetsuit_thruster_fx_kanim"), "loop", Grid.SceneLayer.Creatures, "snapTo_neck", true);
		this.glow_anim = this.AddTrackedAnim("glow", Assets.GetAnim("jetsuit_thruster_glow_fx_kanim"), "loop", Grid.SceneLayer.Front, "snapTo_neck", false);
	}

	// Token: 0x060041F7 RID: 16887 RVA: 0x00174428 File Offset: 0x00172628
	private void DisableJets()
	{
		if (!this.has_jets)
		{
			return;
		}
		if (this.jet_anim != null)
		{
			UnityEngine.Object.Destroy(this.jet_anim.gameObject);
			this.jet_anim = null;
		}
		if (this.glow_anim != null)
		{
			UnityEngine.Object.Destroy(this.glow_anim.gameObject);
			this.glow_anim = null;
		}
	}

	// Token: 0x060041F8 RID: 16888 RVA: 0x00174488 File Offset: 0x00172688
	private KBatchedAnimController AddTrackedAnim(string name, KAnimFile tracked_anim_file, string anim_clip, Grid.SceneLayer layer, string symbol_name, bool require_looping_sound = false)
	{
		KBatchedAnimController assigneeController = this.GetAssigneeController();
		if (assigneeController == null)
		{
			return null;
		}
		string name2 = assigneeController.name + "." + name;
		GameObject gameObject = new GameObject(name2);
		gameObject.SetActive(false);
		gameObject.transform.parent = assigneeController.transform;
		gameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name2);
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			tracked_anim_file
		};
		kbatchedAnimController.initialAnim = anim_clip;
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.sceneLayer = layer;
		if (require_looping_sound)
		{
			gameObject.AddComponent<LoopingSounds>();
		}
		gameObject.AddComponent<KBatchedAnimTracker>().symbol = symbol_name;
		bool flag;
		Vector3 position = assigneeController.GetSymbolTransform(symbol_name, out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(layer);
		gameObject.transform.SetPosition(position);
		gameObject.SetActive(true);
		kbatchedAnimController.Play(anim_clip, KAnim.PlayMode.Loop, 1f, 0f);
		return kbatchedAnimController;
	}

	// Token: 0x060041F9 RID: 16889 RVA: 0x0017458E File Offset: 0x0017278E
	private void OnBeginRecoverBreath(object data)
	{
		this.HideHelmet();
	}

	// Token: 0x060041FA RID: 16890 RVA: 0x00174596 File Offset: 0x00172796
	private void OnEndRecoverBreath(object data)
	{
		this.ShowHelmet();
	}

	// Token: 0x060041FB RID: 16891 RVA: 0x001745A0 File Offset: 0x001727A0
	private void OnPathAdvanced(object data)
	{
		if (this.owner_navigator == null)
		{
			return;
		}
		bool flag = this.owner_navigator.CurrentNavType == NavType.Hover;
		bool flag2 = this.owner_navigator.CurrentNavType == NavType.Tube;
		if (flag2 != this.in_tube)
		{
			this.in_tube = flag2;
			if (this.in_tube)
			{
				this.HideHelmet();
			}
			else
			{
				this.ShowHelmet();
			}
		}
		if (flag != this.is_flying)
		{
			this.is_flying = flag;
			this.UpdateJets();
		}
	}

	// Token: 0x04002929 RID: 10537
	public const string JET_LEFT_SYMBOL_NAME = "left_fire";

	// Token: 0x0400292A RID: 10538
	public const string JET_RIGHT_SYMBOL_NAME = "right_fire";

	// Token: 0x0400292B RID: 10539
	public string anim_file;

	// Token: 0x0400292C RID: 10540
	public bool has_jets;

	// Token: 0x0400292D RID: 10541
	private bool is_shown;

	// Token: 0x0400292E RID: 10542
	private bool in_tube;

	// Token: 0x0400292F RID: 10543
	private bool is_flying;

	// Token: 0x04002930 RID: 10544
	private Navigator owner_navigator;

	// Token: 0x04002931 RID: 10545
	public KBatchedAnimController jet_anim;

	// Token: 0x04002932 RID: 10546
	public KBatchedAnimController glow_anim;

	// Token: 0x04002933 RID: 10547
	private static readonly EventSystem.IntraObjectHandler<HelmetController> OnEquippedDelegate = new EventSystem.IntraObjectHandler<HelmetController>(delegate(HelmetController component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x04002934 RID: 10548
	private static readonly EventSystem.IntraObjectHandler<HelmetController> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<HelmetController>(delegate(HelmetController component, object data)
	{
		component.OnUnequipped(data);
	});
}
