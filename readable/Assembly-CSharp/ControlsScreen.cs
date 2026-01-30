using System;
using UnityEngine.UI;

// Token: 0x02000CE2 RID: 3298
public class ControlsScreen : KScreen
{
	// Token: 0x060065DC RID: 26076 RVA: 0x00265B00 File Offset: 0x00263D00
	protected override void OnPrefabInit()
	{
		BindingEntry[] bindingEntries = GameInputMapping.GetBindingEntries();
		string text = "";
		foreach (BindingEntry bindingEntry in bindingEntries)
		{
			text += bindingEntry.mAction.ToString();
			text += ": ";
			text += bindingEntry.mKeyCode.ToString();
			text += "\n";
		}
		this.controlLabel.text = text;
	}

	// Token: 0x060065DD RID: 26077 RVA: 0x00265B85 File Offset: 0x00263D85
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Help) || e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
		}
	}

	// Token: 0x0400456C RID: 17772
	public Text controlLabel;
}
