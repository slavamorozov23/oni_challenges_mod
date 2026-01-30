using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000BBA RID: 3002
public class ClusterMapVisualizer : KMonoBehaviour
{
	// Token: 0x06005A1D RID: 23069 RVA: 0x0020B32C File Offset: 0x0020952C
	public void Init(ClusterGridEntity entity, ClusterMapPathDrawer pathDrawer)
	{
		this.entity = entity;
		this.pathDrawer = pathDrawer;
		this.animControllers = new List<KBatchedAnimController>();
		if (this.animContainer == null)
		{
			GameObject gameObject = new GameObject("AnimContainer", new Type[]
			{
				typeof(RectTransform)
			});
			RectTransform component = base.GetComponent<RectTransform>();
			RectTransform component2 = gameObject.GetComponent<RectTransform>();
			component2.SetParent(component, false);
			component2.SetLocalPosition(new Vector3(0f, 0f, 0f));
			component2.sizeDelta = component.sizeDelta;
			component2.localScale = Vector3.one;
			this.animContainer = component2;
		}
		Vector3 position = ClusterGrid.Instance.GetPosition(entity);
		this.rectTransform().SetLocalPosition(position);
		this.RefreshPathDrawing();
		entity.Subscribe(543433792, new Action<object>(this.OnClusterDestinationChanged));
	}

	// Token: 0x06005A1E RID: 23070 RVA: 0x0020B402 File Offset: 0x00209602
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.doesTransitionAnimation)
		{
			new ClusterMapTravelAnimator.StatesInstance(this, this.entity).StartSM();
		}
	}

	// Token: 0x06005A1F RID: 23071 RVA: 0x0020B424 File Offset: 0x00209624
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.entity != null)
		{
			if (this.doesTransitionAnimation)
			{
				base.gameObject.GetSMI<ClusterMapTravelAnimator.StatesInstance>().keepRotationOnIdle = this.entity.KeepRotationWhenSpacingOutInHex();
			}
			if (this.entity is Clustercraft)
			{
				new ClusterMapRocketAnimator.StatesInstance(this, this.entity).StartSM();
				return;
			}
			if (this.entity is ClusterMapLongRangeMissileGridEntity)
			{
				new ClusterMapLongRangeMissileAnimator.StatesInstance(this, this.entity).StartSM();
				return;
			}
			if (this.entity is BallisticClusterGridEntity)
			{
				new ClusterMapBallisticAnimator.StatesInstance(this, this.entity).StartSM();
				return;
			}
			if (this.entity.Layer == EntityLayer.FX)
			{
				new ClusterMapFXAnimator.StatesInstance(this, this.entity).StartSM();
			}
		}
	}

	// Token: 0x06005A20 RID: 23072 RVA: 0x0020B4E8 File Offset: 0x002096E8
	protected override void OnCleanUp()
	{
		if (this.mapPath != null)
		{
			global::Util.KDestroyGameObject(this.mapPath);
		}
		if (this.entity != null)
		{
			this.entity.Unsubscribe(543433792, new Action<object>(this.OnClusterDestinationChanged));
		}
		base.OnCleanUp();
	}

	// Token: 0x06005A21 RID: 23073 RVA: 0x0020B53E File Offset: 0x0020973E
	private void OnClusterDestinationChanged(object _)
	{
		this.RefreshPathDrawing();
	}

	// Token: 0x06005A22 RID: 23074 RVA: 0x0020B548 File Offset: 0x00209748
	public void Select(bool selected)
	{
		if (this.animControllers == null || this.animControllers.Count == 0)
		{
			return;
		}
		if (!selected == this.isSelected)
		{
			this.isSelected = selected;
			this.RefreshPathDrawing();
		}
		this.GetFirstAnimController().SetSymbolVisiblity("selected", selected);
	}

	// Token: 0x06005A23 RID: 23075 RVA: 0x0020B59A File Offset: 0x0020979A
	public void PlayAnim(string animName, KAnim.PlayMode playMode)
	{
		if (this.animControllers.Count > 0)
		{
			this.GetFirstAnimController().Play(animName, playMode, 1f, 0f);
		}
	}

	// Token: 0x06005A24 RID: 23076 RVA: 0x0020B5C6 File Offset: 0x002097C6
	public KBatchedAnimController GetFirstAnimController()
	{
		return this.GetAnimController(0);
	}

	// Token: 0x06005A25 RID: 23077 RVA: 0x0020B5CF File Offset: 0x002097CF
	public KBatchedAnimController GetAnimController(int index)
	{
		if (index < this.animControllers.Count)
		{
			return this.animControllers[index];
		}
		return null;
	}

	// Token: 0x06005A26 RID: 23078 RVA: 0x0020B5ED File Offset: 0x002097ED
	public void ManualAddAnimController(KBatchedAnimController externalAnimController)
	{
		this.animControllers.Add(externalAnimController);
	}

	// Token: 0x06005A27 RID: 23079 RVA: 0x0020B5FC File Offset: 0x002097FC
	public void Show(ClusterRevealLevel level)
	{
		if (!this.entity.IsVisible)
		{
			level = ClusterRevealLevel.Hidden;
		}
		if (level == this.lastRevealLevel)
		{
			return;
		}
		this.lastRevealLevel = level;
		switch (level)
		{
		case ClusterRevealLevel.Hidden:
			base.gameObject.SetActive(false);
			break;
		case ClusterRevealLevel.Peeked:
		{
			this.ClearAnimControllers();
			KBatchedAnimController kbatchedAnimController = UnityEngine.Object.Instantiate<KBatchedAnimController>(this.peekControllerPrefab, this.animContainer);
			kbatchedAnimController.gameObject.SetActive(true);
			this.animControllers.Add(kbatchedAnimController);
			base.gameObject.SetActive(true);
			break;
		}
		case ClusterRevealLevel.Visible:
			this.ClearAnimControllers();
			if (this.animControllerPrefab != null && this.entity.AnimConfigs != null)
			{
				foreach (ClusterGridEntity.AnimConfig animConfig in this.entity.AnimConfigs)
				{
					KBatchedAnimController kbatchedAnimController2 = UnityEngine.Object.Instantiate<KBatchedAnimController>(this.animControllerPrefab, this.animContainer);
					kbatchedAnimController2.SwapAnims(new KAnimFile[]
					{
						animConfig.animFile
					});
					kbatchedAnimController2.initialMode = animConfig.playMode;
					kbatchedAnimController2.initialAnim = animConfig.initialAnim;
					kbatchedAnimController2.Offset = animConfig.animOffset;
					kbatchedAnimController2.gameObject.AddComponent<LoopingSounds>();
					if (animConfig.animPlaySpeedModifier != 0f)
					{
						kbatchedAnimController2.PlaySpeedMultiplier = animConfig.animPlaySpeedModifier;
					}
					if (!string.IsNullOrEmpty(animConfig.symbolSwapTarget) && !string.IsNullOrEmpty(animConfig.symbolSwapSymbol))
					{
						SymbolOverrideController component = kbatchedAnimController2.GetComponent<SymbolOverrideController>();
						KAnim.Build.Symbol symbol = kbatchedAnimController2.AnimFiles[0].GetData().build.GetSymbol(animConfig.symbolSwapSymbol);
						component.AddSymbolOverride(animConfig.symbolSwapTarget, symbol, 0);
					}
					kbatchedAnimController2.gameObject.SetActive(true);
					this.animControllers.Add(kbatchedAnimController2);
					this.entity.onClustermapVisualizerAnimCreated(kbatchedAnimController2, animConfig);
				}
			}
			base.gameObject.SetActive(true);
			break;
		}
		this.entity.OnClusterMapIconShown(level);
	}

	// Token: 0x06005A28 RID: 23080 RVA: 0x0020B818 File Offset: 0x00209A18
	public void RefreshPathDrawing()
	{
		if (this.entity == null)
		{
			return;
		}
		ClusterTraveler component = this.entity.GetComponent<ClusterTraveler>();
		if (component == null)
		{
			return;
		}
		List<AxialI> list = (this.entity.IsVisible && component.IsTraveling()) ? component.CurrentPath : null;
		if (list != null && list.Count > 0)
		{
			if (this.mapPath == null)
			{
				this.mapPath = this.pathDrawer.AddPath();
			}
			this.mapPath.SetPoints(ClusterMapPathDrawer.GetDrawPathList(base.transform.GetLocalPosition(), list));
			Color color;
			if (this.isSelected)
			{
				color = ClusterMapScreen.Instance.rocketSelectedPathColor;
			}
			else if (this.entity.ShowPath())
			{
				color = ClusterMapScreen.Instance.rocketPathColor;
			}
			else
			{
				color = new Color(0f, 0f, 0f, 0f);
			}
			this.mapPath.SetColor(color);
			return;
		}
		if (this.mapPath != null)
		{
			global::Util.KDestroyGameObject(this.mapPath);
			this.mapPath = null;
		}
	}

	// Token: 0x06005A29 RID: 23081 RVA: 0x0020B932 File Offset: 0x00209B32
	public void SetAnimRotation(float rotation)
	{
		this.animContainer.localRotation = Quaternion.Euler(0f, 0f, rotation);
	}

	// Token: 0x06005A2A RID: 23082 RVA: 0x0020B94F File Offset: 0x00209B4F
	public float GetPathAngle()
	{
		if (this.mapPath == null)
		{
			return 0f;
		}
		return this.mapPath.GetRotationForNextSegment();
	}

	// Token: 0x06005A2B RID: 23083 RVA: 0x0020B970 File Offset: 0x00209B70
	private void ClearAnimControllers()
	{
		if (this.animControllers == null)
		{
			return;
		}
		foreach (KBatchedAnimController kbatchedAnimController in this.animControllers)
		{
			global::Util.KDestroyGameObject(kbatchedAnimController.gameObject);
		}
		this.animControllers.Clear();
	}

	// Token: 0x04003C49 RID: 15433
	public KBatchedAnimController animControllerPrefab;

	// Token: 0x04003C4A RID: 15434
	public KBatchedAnimController peekControllerPrefab;

	// Token: 0x04003C4B RID: 15435
	public Transform nameTarget;

	// Token: 0x04003C4C RID: 15436
	public AlertVignette alertVignette;

	// Token: 0x04003C4D RID: 15437
	public bool doesTransitionAnimation;

	// Token: 0x04003C4E RID: 15438
	[HideInInspector]
	public Transform animContainer;

	// Token: 0x04003C4F RID: 15439
	private ClusterGridEntity entity;

	// Token: 0x04003C50 RID: 15440
	private ClusterMapPathDrawer pathDrawer;

	// Token: 0x04003C51 RID: 15441
	private ClusterMapPath mapPath;

	// Token: 0x04003C52 RID: 15442
	private List<KBatchedAnimController> animControllers;

	// Token: 0x04003C53 RID: 15443
	private bool isSelected;

	// Token: 0x04003C54 RID: 15444
	private ClusterRevealLevel lastRevealLevel;

	// Token: 0x02001D61 RID: 7521
	private class UpdateXPositionParameter : LoopingSoundParameterUpdater
	{
		// Token: 0x0600B10B RID: 45323 RVA: 0x003DC139 File Offset: 0x003DA339
		public UpdateXPositionParameter() : base("Starmap_Position_X")
		{
		}

		// Token: 0x0600B10C RID: 45324 RVA: 0x003DC158 File Offset: 0x003DA358
		public override void Add(LoopingSoundParameterUpdater.Sound sound)
		{
			ClusterMapVisualizer.UpdateXPositionParameter.Entry item = new ClusterMapVisualizer.UpdateXPositionParameter.Entry
			{
				transform = sound.transform,
				ev = sound.ev,
				parameterId = sound.description.GetParameterId(base.parameter)
			};
			this.entries.Add(item);
		}

		// Token: 0x0600B10D RID: 45325 RVA: 0x003DC1B0 File Offset: 0x003DA3B0
		public override void Update(float dt)
		{
			foreach (ClusterMapVisualizer.UpdateXPositionParameter.Entry entry in this.entries)
			{
				if (!(entry.transform == null))
				{
					EventInstance ev = entry.ev;
					ev.setParameterByID(entry.parameterId, entry.transform.GetPosition().x / (float)Screen.width, false);
				}
			}
		}

		// Token: 0x0600B10E RID: 45326 RVA: 0x003DC238 File Offset: 0x003DA438
		public override void Remove(LoopingSoundParameterUpdater.Sound sound)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].ev.handle == sound.ev.handle)
				{
					this.entries.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x04008B27 RID: 35623
		private List<ClusterMapVisualizer.UpdateXPositionParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateXPositionParameter.Entry>();

		// Token: 0x02002A41 RID: 10817
		private struct Entry
		{
			// Token: 0x0400BAB3 RID: 47795
			public Transform transform;

			// Token: 0x0400BAB4 RID: 47796
			public EventInstance ev;

			// Token: 0x0400BAB5 RID: 47797
			public PARAMETER_ID parameterId;
		}
	}

	// Token: 0x02001D62 RID: 7522
	private class UpdateYPositionParameter : LoopingSoundParameterUpdater
	{
		// Token: 0x0600B10F RID: 45327 RVA: 0x003DC290 File Offset: 0x003DA490
		public UpdateYPositionParameter() : base("Starmap_Position_Y")
		{
		}

		// Token: 0x0600B110 RID: 45328 RVA: 0x003DC2B0 File Offset: 0x003DA4B0
		public override void Add(LoopingSoundParameterUpdater.Sound sound)
		{
			ClusterMapVisualizer.UpdateYPositionParameter.Entry item = new ClusterMapVisualizer.UpdateYPositionParameter.Entry
			{
				transform = sound.transform,
				ev = sound.ev,
				parameterId = sound.description.GetParameterId(base.parameter)
			};
			this.entries.Add(item);
		}

		// Token: 0x0600B111 RID: 45329 RVA: 0x003DC308 File Offset: 0x003DA508
		public override void Update(float dt)
		{
			foreach (ClusterMapVisualizer.UpdateYPositionParameter.Entry entry in this.entries)
			{
				if (!(entry.transform == null))
				{
					EventInstance ev = entry.ev;
					ev.setParameterByID(entry.parameterId, entry.transform.GetPosition().y / (float)Screen.height, false);
				}
			}
		}

		// Token: 0x0600B112 RID: 45330 RVA: 0x003DC390 File Offset: 0x003DA590
		public override void Remove(LoopingSoundParameterUpdater.Sound sound)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].ev.handle == sound.ev.handle)
				{
					this.entries.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x04008B28 RID: 35624
		private List<ClusterMapVisualizer.UpdateYPositionParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateYPositionParameter.Entry>();

		// Token: 0x02002A42 RID: 10818
		private struct Entry
		{
			// Token: 0x0400BAB6 RID: 47798
			public Transform transform;

			// Token: 0x0400BAB7 RID: 47799
			public EventInstance ev;

			// Token: 0x0400BAB8 RID: 47800
			public PARAMETER_ID parameterId;
		}
	}

	// Token: 0x02001D63 RID: 7523
	private class UpdateZoomPercentageParameter : LoopingSoundParameterUpdater
	{
		// Token: 0x0600B113 RID: 45331 RVA: 0x003DC3E8 File Offset: 0x003DA5E8
		public UpdateZoomPercentageParameter() : base("Starmap_Zoom_Percentage")
		{
		}

		// Token: 0x0600B114 RID: 45332 RVA: 0x003DC408 File Offset: 0x003DA608
		public override void Add(LoopingSoundParameterUpdater.Sound sound)
		{
			ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry item = new ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry
			{
				ev = sound.ev,
				parameterId = sound.description.GetParameterId(base.parameter)
			};
			this.entries.Add(item);
		}

		// Token: 0x0600B115 RID: 45333 RVA: 0x003DC454 File Offset: 0x003DA654
		public override void Update(float dt)
		{
			foreach (ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry entry in this.entries)
			{
				EventInstance ev = entry.ev;
				ev.setParameterByID(entry.parameterId, ClusterMapScreen.Instance.CurrentZoomPercentage(), false);
			}
		}

		// Token: 0x0600B116 RID: 45334 RVA: 0x003DC4C0 File Offset: 0x003DA6C0
		public override void Remove(LoopingSoundParameterUpdater.Sound sound)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].ev.handle == sound.ev.handle)
				{
					this.entries.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x04008B29 RID: 35625
		private List<ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry>();

		// Token: 0x02002A43 RID: 10819
		private struct Entry
		{
			// Token: 0x0400BAB9 RID: 47801
			public Transform transform;

			// Token: 0x0400BABA RID: 47802
			public EventInstance ev;

			// Token: 0x0400BABB RID: 47803
			public PARAMETER_ID parameterId;
		}
	}
}
