using System;
using UnityEngine;

// Token: 0x02000563 RID: 1379
public static class SymbolOverrideControllerUtil
{
	// Token: 0x06001EB6 RID: 7862 RVA: 0x000A6DA4 File Offset: 0x000A4FA4
	public static SymbolOverrideController AddToPrefab(GameObject prefab)
	{
		SymbolOverrideController result = prefab.AddComponent<SymbolOverrideController>();
		KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
		DebugUtil.Assert(component != null, "SymbolOverrideController must be added after a KBatchedAnimController component.");
		component.usingNewSymbolOverrideSystem = true;
		return result;
	}

	// Token: 0x06001EB7 RID: 7863 RVA: 0x000A6DCC File Offset: 0x000A4FCC
	public static void AddBuildOverride(this SymbolOverrideController symbol_override_controller, KAnimFileData anim_file_data, int priority = 0)
	{
		for (int i = 0; i < anim_file_data.build.symbols.Length; i++)
		{
			KAnim.Build.Symbol symbol = anim_file_data.build.symbols[i];
			symbol_override_controller.AddSymbolOverride(new HashedString(symbol.hash.HashValue), symbol, priority);
		}
	}

	// Token: 0x06001EB8 RID: 7864 RVA: 0x000A6E18 File Offset: 0x000A5018
	public static void RemoveBuildOverride(this SymbolOverrideController symbol_override_controller, KAnimFileData anim_file_data, int priority = 0)
	{
		for (int i = 0; i < anim_file_data.build.symbols.Length; i++)
		{
			KAnim.Build.Symbol symbol = anim_file_data.build.symbols[i];
			symbol_override_controller.RemoveSymbolOverride(new HashedString(symbol.hash.HashValue), priority);
		}
	}

	// Token: 0x06001EB9 RID: 7865 RVA: 0x000A6E64 File Offset: 0x000A5064
	public static void TryRemoveBuildOverride(this SymbolOverrideController symbol_override_controller, KAnimFileData anim_file_data, int priority = 0)
	{
		for (int i = 0; i < anim_file_data.build.symbols.Length; i++)
		{
			KAnim.Build.Symbol symbol = anim_file_data.build.symbols[i];
			symbol_override_controller.TryRemoveSymbolOverride(new HashedString(symbol.hash.HashValue), priority);
		}
	}

	// Token: 0x06001EBA RID: 7866 RVA: 0x000A6EAF File Offset: 0x000A50AF
	public static bool TryRemoveSymbolOverride(this SymbolOverrideController symbol_override_controller, HashedString target_symbol, int priority = 0)
	{
		return symbol_override_controller.GetSymbolOverrideIdx(target_symbol, priority) >= 0 && symbol_override_controller.RemoveSymbolOverride(target_symbol, priority);
	}

	// Token: 0x06001EBB RID: 7867 RVA: 0x000A6EC8 File Offset: 0x000A50C8
	public static void ApplySymbolOverridesByAffix(this SymbolOverrideController symbol_override_controller, KAnimFile anim_file, string prefix = null, string postfix = null, int priority = 0)
	{
		for (int i = 0; i < anim_file.GetData().build.symbols.Length; i++)
		{
			KAnim.Build.Symbol symbol = anim_file.GetData().build.symbols[i];
			string text = HashCache.Get().Get(symbol.hash);
			if (prefix != null && postfix != null)
			{
				if (text.StartsWith(prefix) && text.EndsWith(postfix))
				{
					string text2 = text.Substring(prefix.Length, text.Length - prefix.Length);
					text2 = text2.Substring(0, text2.Length - postfix.Length);
					symbol_override_controller.AddSymbolOverride(text2, symbol, priority);
				}
			}
			else if (prefix != null && text.StartsWith(prefix))
			{
				symbol_override_controller.AddSymbolOverride(text.Substring(prefix.Length, text.Length - prefix.Length), symbol, priority);
			}
			else if (postfix != null && text.EndsWith(postfix))
			{
				symbol_override_controller.AddSymbolOverride(text.Substring(0, text.Length - postfix.Length), symbol, priority);
			}
		}
	}
}
