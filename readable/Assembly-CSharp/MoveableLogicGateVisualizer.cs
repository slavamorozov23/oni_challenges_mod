using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000799 RID: 1945
[SkipSaveFileSerialization]
public class MoveableLogicGateVisualizer : LogicGateBase
{
	// Token: 0x06003244 RID: 12868 RVA: 0x00122264 File Offset: 0x00120464
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.cell = -1;
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		this.OnOverlayChanged(OverlayScreen.Instance.mode);
		base.Subscribe<MoveableLogicGateVisualizer>(-1643076535, MoveableLogicGateVisualizer.OnRotatedDelegate);
	}

	// Token: 0x06003245 RID: 12869 RVA: 0x001222C5 File Offset: 0x001204C5
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		this.Unregister();
		base.OnCleanUp();
	}

	// Token: 0x06003246 RID: 12870 RVA: 0x001222F9 File Offset: 0x001204F9
	private void OnOverlayChanged(HashedString mode)
	{
		if (mode == OverlayModes.Logic.ID)
		{
			this.Register();
			return;
		}
		this.Unregister();
	}

	// Token: 0x06003247 RID: 12871 RVA: 0x00122315 File Offset: 0x00120515
	private void OnRotated(object data)
	{
		this.Unregister();
		this.OnOverlayChanged(OverlayScreen.Instance.mode);
	}

	// Token: 0x06003248 RID: 12872 RVA: 0x00122330 File Offset: 0x00120530
	private void Update()
	{
		if (this.visChildren.Count <= 0)
		{
			return;
		}
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (num == this.cell)
		{
			return;
		}
		this.cell = num;
		this.Unregister();
		this.Register();
	}

	// Token: 0x06003249 RID: 12873 RVA: 0x0012237C File Offset: 0x0012057C
	private GameObject CreateUIElem(int cell, bool is_input)
	{
		GameObject gameObject = Util.KInstantiate(LogicGateBase.uiSrcData.prefab, Grid.CellToPosCCC(cell, Grid.SceneLayer.Front), Quaternion.identity, GameScreenManager.Instance.worldSpaceCanvas, null, true, 0);
		Image component = gameObject.GetComponent<Image>();
		component.sprite = (is_input ? LogicGateBase.uiSrcData.inputSprite : LogicGateBase.uiSrcData.outputSprite);
		component.raycastTarget = false;
		return gameObject;
	}

	// Token: 0x0600324A RID: 12874 RVA: 0x001223E0 File Offset: 0x001205E0
	private void Register()
	{
		if (this.visChildren.Count > 0)
		{
			return;
		}
		base.enabled = true;
		this.visChildren.Add(this.CreateUIElem(base.OutputCellOne, false));
		if (base.RequiresFourOutputs)
		{
			this.visChildren.Add(this.CreateUIElem(base.OutputCellTwo, false));
			this.visChildren.Add(this.CreateUIElem(base.OutputCellThree, false));
			this.visChildren.Add(this.CreateUIElem(base.OutputCellFour, false));
		}
		this.visChildren.Add(this.CreateUIElem(base.InputCellOne, true));
		if (base.RequiresTwoInputs)
		{
			this.visChildren.Add(this.CreateUIElem(base.InputCellTwo, true));
		}
		else if (base.RequiresFourInputs)
		{
			this.visChildren.Add(this.CreateUIElem(base.InputCellTwo, true));
			this.visChildren.Add(this.CreateUIElem(base.InputCellThree, true));
			this.visChildren.Add(this.CreateUIElem(base.InputCellFour, true));
		}
		if (base.RequiresControlInputs)
		{
			this.visChildren.Add(this.CreateUIElem(base.ControlCellOne, true));
			this.visChildren.Add(this.CreateUIElem(base.ControlCellTwo, true));
		}
	}

	// Token: 0x0600324B RID: 12875 RVA: 0x00122530 File Offset: 0x00120730
	private void Unregister()
	{
		if (this.visChildren.Count <= 0)
		{
			return;
		}
		base.enabled = false;
		this.cell = -1;
		foreach (GameObject original in this.visChildren)
		{
			Util.KDestroyGameObject(original);
		}
		this.visChildren.Clear();
	}

	// Token: 0x04001E8D RID: 7821
	private int cell;

	// Token: 0x04001E8E RID: 7822
	protected List<GameObject> visChildren = new List<GameObject>();

	// Token: 0x04001E8F RID: 7823
	private static readonly EventSystem.IntraObjectHandler<MoveableLogicGateVisualizer> OnRotatedDelegate = new EventSystem.IntraObjectHandler<MoveableLogicGateVisualizer>(delegate(MoveableLogicGateVisualizer component, object data)
	{
		component.OnRotated(data);
	});
}
