using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200063A RID: 1594
[AddComponentMenu("KMonoBehaviour/scripts/SnapOn")]
public class SnapOn : KMonoBehaviour
{
	// Token: 0x06002607 RID: 9735 RVA: 0x000DA9AC File Offset: 0x000D8BAC
	protected override void OnPrefabInit()
	{
		this.kanimController = base.GetComponent<KAnimControllerBase>();
	}

	// Token: 0x06002608 RID: 9736 RVA: 0x000DA9BC File Offset: 0x000D8BBC
	protected override void OnSpawn()
	{
		foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
		{
			if (snapPoint.automatic)
			{
				this.DoAttachSnapOn(snapPoint);
			}
		}
	}

	// Token: 0x06002609 RID: 9737 RVA: 0x000DAA18 File Offset: 0x000D8C18
	public void AttachSnapOnByName(string name)
	{
		foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
		{
			if (snapPoint.pointName == name)
			{
				HashedString context = base.GetComponent<AnimEventHandler>().GetContext();
				if (!context.IsValid || !snapPoint.context.IsValid || context == snapPoint.context)
				{
					this.DoAttachSnapOn(snapPoint);
				}
			}
		}
	}

	// Token: 0x0600260A RID: 9738 RVA: 0x000DAAAC File Offset: 0x000D8CAC
	public void DetachSnapOnByName(string name)
	{
		foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
		{
			if (snapPoint.pointName == name)
			{
				HashedString context = base.GetComponent<AnimEventHandler>().GetContext();
				if (!context.IsValid || !snapPoint.context.IsValid || context == snapPoint.context)
				{
					base.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(snapPoint.overrideSymbol, 5);
					this.kanimController.SetSymbolVisiblity(snapPoint.overrideSymbol, false);
					break;
				}
			}
		}
	}

	// Token: 0x0600260B RID: 9739 RVA: 0x000DAB64 File Offset: 0x000D8D64
	private void DoAttachSnapOn(SnapOn.SnapPoint point)
	{
		SnapOn.OverrideEntry overrideEntry = null;
		KAnimFile buildFile = point.buildFile;
		string symbol_name = "";
		if (this.overrideMap.TryGetValue(point.pointName, out overrideEntry))
		{
			buildFile = overrideEntry.buildFile;
			symbol_name = overrideEntry.symbolName;
		}
		KAnim.Build.Symbol symbol = SnapOn.GetSymbol(buildFile, symbol_name);
		base.GetComponent<SymbolOverrideController>().AddSymbolOverride(point.overrideSymbol, symbol, 5);
		this.kanimController.SetSymbolVisiblity(point.overrideSymbol, true);
	}

	// Token: 0x0600260C RID: 9740 RVA: 0x000DABD8 File Offset: 0x000D8DD8
	private static KAnim.Build.Symbol GetSymbol(KAnimFile anim_file, string symbol_name)
	{
		KAnim.Build.Symbol result = anim_file.GetData().build.symbols[0];
		KAnimHashedString y = new KAnimHashedString(symbol_name);
		foreach (KAnim.Build.Symbol symbol in anim_file.GetData().build.symbols)
		{
			if (symbol.hash == y)
			{
				result = symbol;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600260D RID: 9741 RVA: 0x000DAC39 File Offset: 0x000D8E39
	public void AddOverride(string point_name, KAnimFile build_override, string symbol_name)
	{
		this.overrideMap[point_name] = new SnapOn.OverrideEntry
		{
			buildFile = build_override,
			symbolName = symbol_name
		};
	}

	// Token: 0x0600260E RID: 9742 RVA: 0x000DAC5A File Offset: 0x000D8E5A
	public void RemoveOverride(string point_name)
	{
		this.overrideMap.Remove(point_name);
	}

	// Token: 0x0400166F RID: 5743
	private KAnimControllerBase kanimController;

	// Token: 0x04001670 RID: 5744
	public List<SnapOn.SnapPoint> snapPoints = new List<SnapOn.SnapPoint>();

	// Token: 0x04001671 RID: 5745
	private Dictionary<string, SnapOn.OverrideEntry> overrideMap = new Dictionary<string, SnapOn.OverrideEntry>();

	// Token: 0x02001514 RID: 5396
	[Serializable]
	public class SnapPoint
	{
		// Token: 0x0400709F RID: 28831
		public string pointName;

		// Token: 0x040070A0 RID: 28832
		public bool automatic = true;

		// Token: 0x040070A1 RID: 28833
		public HashedString context;

		// Token: 0x040070A2 RID: 28834
		public KAnimFile buildFile;

		// Token: 0x040070A3 RID: 28835
		public HashedString overrideSymbol;
	}

	// Token: 0x02001515 RID: 5397
	public class OverrideEntry
	{
		// Token: 0x040070A4 RID: 28836
		public KAnimFile buildFile;

		// Token: 0x040070A5 RID: 28837
		public string symbolName;
	}
}
