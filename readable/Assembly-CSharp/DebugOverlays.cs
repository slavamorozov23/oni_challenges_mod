using System;

// Token: 0x02000C52 RID: 3154
public class DebugOverlays : KScreen
{
	// Token: 0x17000703 RID: 1795
	// (get) Token: 0x06005FDA RID: 24538 RVA: 0x00232ABF File Offset: 0x00230CBF
	// (set) Token: 0x06005FDB RID: 24539 RVA: 0x00232AC6 File Offset: 0x00230CC6
	public static DebugOverlays instance { get; private set; }

	// Token: 0x06005FDC RID: 24540 RVA: 0x00232AD0 File Offset: 0x00230CD0
	protected override void OnPrefabInit()
	{
		DebugOverlays.instance = this;
		KPopupMenu componentInChildren = base.GetComponentInChildren<KPopupMenu>();
		componentInChildren.SetOptions(new string[]
		{
			"None",
			"Rooms",
			"Lighting",
			"Style",
			"Flow"
		});
		KPopupMenu kpopupMenu = componentInChildren;
		kpopupMenu.OnSelect = (Action<string, int>)Delegate.Combine(kpopupMenu.OnSelect, new Action<string, int>(this.OnSelect));
		base.gameObject.SetActive(false);
	}

	// Token: 0x06005FDD RID: 24541 RVA: 0x00232B4C File Offset: 0x00230D4C
	private void OnSelect(string str, int index)
	{
		if (str == "None")
		{
			SimDebugView.Instance.SetMode(OverlayModes.None.ID);
			return;
		}
		if (str == "Flow")
		{
			SimDebugView.Instance.SetMode(SimDebugView.OverlayModes.Flow);
			return;
		}
		if (str == "Lighting")
		{
			SimDebugView.Instance.SetMode(OverlayModes.Light.ID);
			return;
		}
		if (!(str == "Rooms"))
		{
			Debug.LogError("Unknown debug view: " + str);
			return;
		}
		SimDebugView.Instance.SetMode(OverlayModes.Rooms.ID);
	}
}
