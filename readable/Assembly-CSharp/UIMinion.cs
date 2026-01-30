using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02000EB9 RID: 3769
public class UIMinion : KMonoBehaviour, UIMinionOrMannequin.ITarget
{
	// Token: 0x1700084A RID: 2122
	// (get) Token: 0x060078C4 RID: 30916 RVA: 0x002E73C5 File Offset: 0x002E55C5
	public GameObject SpawnedAvatar
	{
		get
		{
			if (this.spawn == null)
			{
				this.TrySpawn();
			}
			return this.spawn;
		}
	}

	// Token: 0x1700084B RID: 2123
	// (get) Token: 0x060078C5 RID: 30917 RVA: 0x002E73E1 File Offset: 0x002E55E1
	// (set) Token: 0x060078C6 RID: 30918 RVA: 0x002E73E9 File Offset: 0x002E55E9
	public Option<Personality> Personality { get; private set; }

	// Token: 0x060078C7 RID: 30919 RVA: 0x002E73F2 File Offset: 0x002E55F2
	protected override void OnSpawn()
	{
		this.TrySpawn();
	}

	// Token: 0x060078C8 RID: 30920 RVA: 0x002E73FC File Offset: 0x002E55FC
	public void TrySpawn()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(MinionUIPortrait.ID), base.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = 0.38f;
			this.animController.Play("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
			BaseMinionConfig.ConfigureSymbols(this.animController.gameObject, true);
			this.spawn = this.animController.gameObject;
		}
	}

	// Token: 0x060078C9 RID: 30921 RVA: 0x002E74A3 File Offset: 0x002E56A3
	public void SetMinion(Personality personality)
	{
		this.SpawnedAvatar.GetComponent<Accessorizer>().ApplyMinionPersonality(personality);
		this.Personality = personality;
		base.gameObject.AddOrGet<MinionVoiceProviderMB>().voice = MinionVoice.ByPersonality(personality);
	}

	// Token: 0x060078CA RID: 30922 RVA: 0x002E74E0 File Offset: 0x002E56E0
	public void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> outfit)
	{
		outfit = UIMinionOrMannequinITargetExtensions.GetOutfitWithDefaultItems(outfitType, outfit);
		WearableAccessorizer component = this.SpawnedAvatar.GetComponent<WearableAccessorizer>();
		component.ClearClothingItems(null);
		component.ApplyClothingItems(outfitType, outfit);
	}

	// Token: 0x060078CB RID: 30923 RVA: 0x002E7518 File Offset: 0x002E5718
	public MinionVoice GetMinionVoice()
	{
		return MinionVoice.ByObject(this.SpawnedAvatar).UnwrapOr(MinionVoice.Random(), null);
	}

	// Token: 0x060078CC RID: 30924 RVA: 0x002E7540 File Offset: 0x002E5740
	public void React(UIMinionOrMannequinReactSource source)
	{
		if (source != UIMinionOrMannequinReactSource.OnPersonalityChanged && this.lastReactSource == source)
		{
			KAnim.Anim currentAnim = this.animController.GetCurrentAnim();
			if (currentAnim != null && currentAnim.name != "idle_default")
			{
				return;
			}
		}
		switch (source)
		{
		case UIMinionOrMannequinReactSource.OnPersonalityChanged:
			this.animController.Play("react", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnWholeOutfitChanged:
		case UIMinionOrMannequinReactSource.OnBottomChanged:
			this.animController.Play("react_bottoms", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnHatChanged:
			this.animController.Play("react_glasses", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnTopChanged:
			this.animController.Play("react_tops", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnGlovesChanged:
			this.animController.Play("react_gloves", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnShoesChanged:
			this.animController.Play("react_shoes", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		}
		this.animController.Play("cheer_pre", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Queue("cheer_loop", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Queue("cheer_pst", KAnim.PlayMode.Once, 1f, 0f);
		IL_195:
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
		this.lastReactSource = source;
	}

	// Token: 0x04005431 RID: 21553
	public const float ANIM_SCALE = 0.38f;

	// Token: 0x04005432 RID: 21554
	private KBatchedAnimController animController;

	// Token: 0x04005433 RID: 21555
	private GameObject spawn;

	// Token: 0x04005435 RID: 21557
	private UIMinionOrMannequinReactSource lastReactSource;
}
