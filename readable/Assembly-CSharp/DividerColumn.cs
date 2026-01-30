using System;
using UnityEngine;

// Token: 0x02000D83 RID: 3459
public class DividerColumn : TableColumn
{
	// Token: 0x06006B86 RID: 27526 RVA: 0x0028CA9C File Offset: 0x0028AC9C
	public DividerColumn(Func<bool> revealed = null, string scrollerID = "") : base(delegate(IAssignableIdentity minion, GameObject widget_go)
	{
		if (revealed != null)
		{
			if (revealed())
			{
				if (!widget_go.activeSelf)
				{
					widget_go.SetActive(true);
					return;
				}
			}
			else if (widget_go.activeSelf)
			{
				widget_go.SetActive(false);
				return;
			}
		}
		else
		{
			widget_go.SetActive(true);
		}
	}, null, null, null, revealed, false, scrollerID)
	{
	}

	// Token: 0x06006B87 RID: 27527 RVA: 0x0028CAD3 File Offset: 0x0028ACD3
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
	}

	// Token: 0x06006B88 RID: 27528 RVA: 0x0028CAEB File Offset: 0x0028ACEB
	public override GameObject GetMinionWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
	}

	// Token: 0x06006B89 RID: 27529 RVA: 0x0028CB03 File Offset: 0x0028AD03
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
	}
}
