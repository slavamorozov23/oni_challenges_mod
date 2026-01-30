using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B70 RID: 2928
public class ClusterMapMeteorShowerVisualizer : ClusterGridEntity
{
	// Token: 0x1700061F RID: 1567
	// (get) Token: 0x060056A8 RID: 22184 RVA: 0x001F8D4A File Offset: 0x001F6F4A
	public override string Name
	{
		get
		{
			return this.p_name;
		}
	}

	// Token: 0x17000620 RID: 1568
	// (get) Token: 0x060056A9 RID: 22185 RVA: 0x001F8D52 File Offset: 0x001F6F52
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Meteor;
		}
	}

	// Token: 0x17000621 RID: 1569
	// (get) Token: 0x060056AA RID: 22186 RVA: 0x001F8D55 File Offset: 0x001F6F55
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000622 RID: 1570
	// (get) Token: 0x060056AB RID: 22187 RVA: 0x001F8D58 File Offset: 0x001F6F58
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x17000623 RID: 1571
	// (get) Token: 0x060056AC RID: 22188 RVA: 0x001F8D5C File Offset: 0x001F6F5C
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim(this.clusterAnimName),
					initialAnim = this.AnimName,
					animPlaySpeedModifier = 0.5f
				},
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("shower_identify_kanim"),
					initialAnim = "identify_off",
					playMode = KAnim.PlayMode.Once
				},
				this.questionMarkAnimConfig
			};
		}
	}

	// Token: 0x17000624 RID: 1572
	// (get) Token: 0x060056AD RID: 22189 RVA: 0x001F8DF4 File Offset: 0x001F6FF4
	public ClusterRevealLevel clusterCellRevealLevel
	{
		get
		{
			ClusterRevealLevel cellRevealLevel = ClusterGrid.Instance.GetCellRevealLevel(base.Location);
			if (cellRevealLevel == ClusterRevealLevel.Visible)
			{
				return cellRevealLevel;
			}
			if (this.forceRevealed)
			{
				return ClusterRevealLevel.Peeked;
			}
			return cellRevealLevel;
		}
	}

	// Token: 0x17000625 RID: 1573
	// (get) Token: 0x060056AE RID: 22190 RVA: 0x001F8E23 File Offset: 0x001F7023
	public string AnimName
	{
		get
		{
			if (!this.forceRevealed && (!this.revealed || this.clusterCellRevealLevel != ClusterRevealLevel.Visible))
			{
				return "unknown";
			}
			return "idle_loop";
		}
	}

	// Token: 0x17000626 RID: 1574
	// (get) Token: 0x060056AF RID: 22191 RVA: 0x001F8E49 File Offset: 0x001F7049
	public string QuestionMarkAnimName
	{
		get
		{
			if (!this.forceRevealed && (!this.revealed || this.clusterCellRevealLevel != ClusterRevealLevel.Visible))
			{
				return this.questionMarkAnimConfig.initialAnim;
			}
			return "off";
		}
	}

	// Token: 0x060056B0 RID: 22192 RVA: 0x001F8E78 File Offset: 0x001F7078
	public KBatchedAnimController CreateQuestionMarkInstance(KBatchedAnimController origin, Transform parent)
	{
		KBatchedAnimController kbatchedAnimController = UnityEngine.Object.Instantiate<KBatchedAnimController>(origin, parent);
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController.SwapAnims(new KAnimFile[]
		{
			this.questionMarkAnimConfig.animFile
		});
		kbatchedAnimController.Play(this.QuestionMarkAnimName, KAnim.PlayMode.Once, 1f, 0f);
		kbatchedAnimController.gameObject.AddOrGet<ClusterMapIconFixRotation>();
		return kbatchedAnimController;
	}

	// Token: 0x060056B1 RID: 22193 RVA: 0x001F8EDC File Offset: 0x001F70DC
	protected override void OnCleanUp()
	{
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
			if (entityVisAnim != null)
			{
				entityVisAnim.gameObject.SetActive(false);
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x060056B2 RID: 22194 RVA: 0x001F8F1D File Offset: 0x001F711D
	public void SetInitialLocation(AxialI startLocation)
	{
		this.m_location = startLocation;
		this.RefreshVisuals();
	}

	// Token: 0x060056B3 RID: 22195 RVA: 0x001F8F2C File Offset: 0x001F712C
	public override bool SpaceOutInSameHex()
	{
		return true;
	}

	// Token: 0x060056B4 RID: 22196 RVA: 0x001F8F2F File Offset: 0x001F712F
	public override bool KeepRotationWhenSpacingOutInHex()
	{
		return true;
	}

	// Token: 0x060056B5 RID: 22197 RVA: 0x001F8F32 File Offset: 0x001F7132
	public override bool ShowPath()
	{
		return this.m_selectable.IsSelected;
	}

	// Token: 0x060056B6 RID: 22198 RVA: 0x001F8F40 File Offset: 0x001F7140
	public override void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
		ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		switch (levelUsed)
		{
		case ClusterRevealLevel.Hidden:
			this.Deselect();
			break;
		case ClusterRevealLevel.Peeked:
		{
			KBatchedAnimController firstAnimController = entityVisAnim.GetFirstAnimController();
			if (firstAnimController != null)
			{
				firstAnimController.SwapAnims(new KAnimFile[]
				{
					this.AnimConfigs[0].animFile
				});
				KBatchedAnimController externalAnimController = this.CreateQuestionMarkInstance(entityVisAnim.peekControllerPrefab, firstAnimController.transform.parent);
				entityVisAnim.ManualAddAnimController(externalAnimController);
			}
			this.RefreshVisuals();
			this.Deselect();
			break;
		}
		case ClusterRevealLevel.Visible:
			this.RefreshVisuals();
			break;
		}
		KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
		if (animController != null && !this.revealed)
		{
			animController.gameObject.AddOrGet<ClusterMapIconFixRotation>();
		}
	}

	// Token: 0x060056B7 RID: 22199 RVA: 0x001F8FFD File Offset: 0x001F71FD
	public void Deselect()
	{
		if (this.m_selectable.IsSelected)
		{
			this.m_selectable.Unselect();
		}
	}

	// Token: 0x060056B8 RID: 22200 RVA: 0x001F9018 File Offset: 0x001F7218
	public void RefreshVisuals()
	{
		ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		if (entityVisAnim != null)
		{
			KBatchedAnimController firstAnimController = entityVisAnim.GetFirstAnimController();
			if (firstAnimController != null)
			{
				firstAnimController.Play(this.AnimName, KAnim.PlayMode.Loop, 1f, 0f);
			}
			KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
			if (animController != null)
			{
				animController.Play(this.QuestionMarkAnimName, KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x060056B9 RID: 22201 RVA: 0x001F9094 File Offset: 0x001F7294
	public void PlayRevealAnimation(bool playIdentifyAnimationIfVisible)
	{
		this.revealed = true;
		this.RefreshVisuals();
		if (playIdentifyAnimationIfVisible)
		{
			ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
			KBatchedAnimController animController = entityVisAnim.GetAnimController(1);
			entityVisAnim.GetAnimController(2);
			if (animController != null)
			{
				animController.Play("identify", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x060056BA RID: 22202 RVA: 0x001F90EF File Offset: 0x001F72EF
	public void PlayHideAnimation()
	{
		this.revealed = false;
		if (ClusterMapScreen.Instance.GetEntityVisAnim(this) != null)
		{
			this.RefreshVisuals();
		}
	}

	// Token: 0x04003A80 RID: 14976
	private ClusterGridEntity.AnimConfig questionMarkAnimConfig = new ClusterGridEntity.AnimConfig
	{
		animFile = Assets.GetAnim("shower_question_mark_kanim"),
		initialAnim = "idle",
		playMode = KAnim.PlayMode.Once
	};

	// Token: 0x04003A81 RID: 14977
	public string p_name;

	// Token: 0x04003A82 RID: 14978
	public string clusterAnimName;

	// Token: 0x04003A83 RID: 14979
	public bool revealed;

	// Token: 0x04003A84 RID: 14980
	public bool forceRevealed;
}
