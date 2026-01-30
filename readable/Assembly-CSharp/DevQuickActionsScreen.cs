using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200068A RID: 1674
public class DevQuickActionsScreen : MonoBehaviour
{
	// Token: 0x06002939 RID: 10553 RVA: 0x000EAC96 File Offset: 0x000E8E96
	public static void DestroyInstance()
	{
		DevQuickActionsScreen.Instance = null;
	}

	// Token: 0x0600293A RID: 10554 RVA: 0x000EACA0 File Offset: 0x000E8EA0
	private void Awake()
	{
		DevQuickActionsScreen.Instance = this;
		this.Pointer.SetVisibleState(false);
		DevQuickActionTargetFollower pointer = this.Pointer;
		pointer.OnToggleChanged = (Action<bool>)Delegate.Combine(pointer.OnToggleChanged, new Action<bool>(this.OnPointerToggleClicked));
		this.originalEndNode.gameObject.SetActive(false);
		this.originalCategoryDevNode.gameObject.SetActive(false);
	}

	// Token: 0x0600293B RID: 10555 RVA: 0x000EAD08 File Offset: 0x000E8F08
	private void OnPointerToggleClicked(bool val)
	{
		if (this.Target != null && this.RootNode != null)
		{
			if (val)
			{
				this.RootNode.Expand();
				return;
			}
			this.RootNode.Collapse();
		}
	}

	// Token: 0x0600293C RID: 10556 RVA: 0x000EAD40 File Offset: 0x000E8F40
	public void Toggle(GameObject target)
	{
		if (target == null)
		{
			this.Close();
			return;
		}
		if (this.Target != target)
		{
			this.Open(target);
			return;
		}
		this.Close();
	}

	// Token: 0x0600293D RID: 10557 RVA: 0x000EAD70 File Offset: 0x000E8F70
	public void Open(GameObject target)
	{
		if (this.Target != null && this.Target != target)
		{
			this.Close();
		}
		this.Target = target;
		if (target == null)
		{
			return;
		}
		Vector3 position = CameraController.Instance.overlayCamera.WorldToScreenPoint(target.transform.position);
		this.RootNode = this.GetUnsedCategoryNode();
		this.RootNode.Setup(target.GetProperName(), null);
		this.RootNode.transform.SetPosition(position);
		this.RootNode.SetChildrenSeparationSpace(50f);
		this.Target.Subscribe(1502190696, new Action<object>(this.OnTargetLost));
		List<IDevQuickAction> list = new List<IDevQuickAction>(this.Target.GetComponents<IDevQuickAction>());
		list.AddRange(this.Target.GetAllSMI<IDevQuickAction>());
		foreach (IDevQuickAction devQuickAction in list)
		{
			foreach (DevQuickActionInstruction devQuickActionInstruction in devQuickAction.GetDevInstructions())
			{
				string[] array = devQuickActionInstruction.Address.Split('/', StringSplitOptions.None);
				DevQuickActionCategoryNode devQuickActionCategoryNode = this.RootNode;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (i < array.Length - 1)
					{
						DevQuickActionCategoryNode devQuickActionCategoryNode2 = null;
						if (!this.registeredCategoryNodes.TryGetValue(text, out devQuickActionCategoryNode2))
						{
							devQuickActionCategoryNode2 = this.GetUnsedCategoryNode();
							devQuickActionCategoryNode2.Setup(text, devQuickActionCategoryNode);
							this.registeredCategoryNodes.Add(text, devQuickActionCategoryNode2);
							devQuickActionCategoryNode.AddChildren(devQuickActionCategoryNode2);
						}
						devQuickActionCategoryNode = devQuickActionCategoryNode2;
					}
					else
					{
						DevQuickActionEndNode unsedEndNode = this.GetUnsedEndNode();
						unsedEndNode.Setup(text, devQuickActionCategoryNode, devQuickActionInstruction.Action);
						devQuickActionCategoryNode.AddChildren(unsedEndNode);
						unsedEndNode.gameObject.SetActive(false);
					}
				}
			}
		}
		this.RootNode.Collapse();
		if (this.Pointer.IsToggleOn)
		{
			this.RootNode.Expand();
		}
		this.RootNode.gameObject.SetActive(false);
		this.Pointer.transform.position = this.RootNode.transform.position;
		this.Pointer.SetTarget(this.Target);
		this.Pointer.SetVisibleState(true);
	}

	// Token: 0x0600293E RID: 10558 RVA: 0x000EB008 File Offset: 0x000E9208
	public void Close()
	{
		if (this.Target != null)
		{
			this.Target.Unsubscribe(1502190696, new Action<object>(this.OnTargetLost));
		}
		this.Target = null;
		if (this.RootNode != null)
		{
			this.RootNode.Recycle();
			this.RootNode = null;
		}
		this.registeredCategoryNodes.Clear();
		this.Pointer.SetTarget(null);
		this.Pointer.SetVisibleState(false);
	}

	// Token: 0x0600293F RID: 10559 RVA: 0x000EB089 File Offset: 0x000E9289
	private void OnTargetLost(object o)
	{
		this.Close();
	}

	// Token: 0x06002940 RID: 10560 RVA: 0x000EB094 File Offset: 0x000E9294
	private DevQuickActionEndNode GetUnsedEndNode()
	{
		DevQuickActionEndNode devQuickActionEndNode = null;
		if (!this.recycledEndNodes.TryPop(out devQuickActionEndNode))
		{
			devQuickActionEndNode = Util.KInstantiateUI(this.originalEndNode.gameObject, this.originalEndNode.transform.parent.gameObject, false).GetComponent<DevQuickActionEndNode>();
		}
		this.SetupUnusedNodeForUse(devQuickActionEndNode);
		return devQuickActionEndNode;
	}

	// Token: 0x06002941 RID: 10561 RVA: 0x000EB0E8 File Offset: 0x000E92E8
	private DevQuickActionCategoryNode GetUnsedCategoryNode()
	{
		DevQuickActionCategoryNode devQuickActionCategoryNode = null;
		if (!this.recycledCategoriesNodes.TryPop(out devQuickActionCategoryNode))
		{
			devQuickActionCategoryNode = Util.KInstantiateUI(this.originalCategoryDevNode.gameObject, this.originalCategoryDevNode.transform.parent.gameObject, false).GetComponent<DevQuickActionCategoryNode>();
		}
		this.SetupUnusedNodeForUse(devQuickActionCategoryNode);
		return devQuickActionCategoryNode;
	}

	// Token: 0x06002942 RID: 10562 RVA: 0x000EB13A File Offset: 0x000E933A
	private void SetupUnusedNodeForUse(DevQuickActionNode node)
	{
		node.OnRecycle = new Action<DevQuickActionNode>(this.OnNodeRecycled);
		node.SetChildrenSeparationSpace(60f);
		node.gameObject.SetActive(true);
	}

	// Token: 0x06002943 RID: 10563 RVA: 0x000EB165 File Offset: 0x000E9365
	private void OnNodeRecycled(DevQuickActionNode node)
	{
		if (node is DevQuickActionCategoryNode)
		{
			this.recycledCategoriesNodes.Push(node as DevQuickActionCategoryNode);
			return;
		}
		if (node is DevQuickActionEndNode)
		{
			this.recycledEndNodes.Push(node as DevQuickActionEndNode);
		}
	}

	// Token: 0x04001857 RID: 6231
	public const float DEFAULT_SPACE = 60f;

	// Token: 0x04001858 RID: 6232
	public const float ROOT_SPACE = 50f;

	// Token: 0x04001859 RID: 6233
	public const char CATEGORY_DIVIDER = '/';

	// Token: 0x0400185A RID: 6234
	public DevQuickActionNode originalCategoryDevNode;

	// Token: 0x0400185B RID: 6235
	public DevQuickActionNode originalEndNode;

	// Token: 0x0400185C RID: 6236
	public DevQuickActionTargetFollower Pointer;

	// Token: 0x0400185D RID: 6237
	public Stack<DevQuickActionEndNode> recycledEndNodes = new Stack<DevQuickActionEndNode>();

	// Token: 0x0400185E RID: 6238
	public Stack<DevQuickActionCategoryNode> recycledCategoriesNodes = new Stack<DevQuickActionCategoryNode>();

	// Token: 0x0400185F RID: 6239
	private Dictionary<string, DevQuickActionCategoryNode> registeredCategoryNodes = new Dictionary<string, DevQuickActionCategoryNode>();

	// Token: 0x04001860 RID: 6240
	private GameObject Target;

	// Token: 0x04001861 RID: 6241
	private DevQuickActionCategoryNode RootNode;

	// Token: 0x04001862 RID: 6242
	public static DevQuickActionsScreen Instance;
}
