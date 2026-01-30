using System;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000685 RID: 1669
public class DevQuickActionEndNode : DevQuickActionNode
{
	// Token: 0x06002923 RID: 10531 RVA: 0x000EA9BB File Offset: 0x000E8BBB
	protected void Awake()
	{
		this.button = base.GetComponent<Button>();
		this.button.onClick.AddListener(new UnityAction(this.ButtonClicked));
	}

	// Token: 0x06002924 RID: 10532 RVA: 0x000EA9E5 File Offset: 0x000E8BE5
	private void ButtonClicked()
	{
		System.Action onNodeInteractedWith = this.OnNodeInteractedWith;
		if (onNodeInteractedWith == null)
		{
			return;
		}
		onNodeInteractedWith();
	}

	// Token: 0x06002925 RID: 10533 RVA: 0x000EA9F7 File Offset: 0x000E8BF7
	public void Setup(string name, DevQuickActionNode parentNode, System.Action clickCB)
	{
		this.label.SetText(name);
		this.parentNode = parentNode;
		this.OnNodeInteractedWith = clickCB;
	}

	// Token: 0x04001848 RID: 6216
	private Button button;
}
