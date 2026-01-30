using System;
using ImGuiNET;

// Token: 0x0200069D RID: 1693
public class DevToolMenuFontSize
{
	// Token: 0x17000201 RID: 513
	// (get) Token: 0x060029AC RID: 10668 RVA: 0x000EF751 File Offset: 0x000ED951
	// (set) Token: 0x060029AB RID: 10667 RVA: 0x000EF748 File Offset: 0x000ED948
	public bool initialized { get; private set; }

	// Token: 0x060029AD RID: 10669 RVA: 0x000EF75C File Offset: 0x000ED95C
	public void RefreshFontSize()
	{
		DevToolMenuFontSize.FontSizeCategory @int = (DevToolMenuFontSize.FontSizeCategory)KPlayerPrefs.GetInt("Imgui_font_size_category", 2);
		this.SetFontSizeCategory(@int);
	}

	// Token: 0x060029AE RID: 10670 RVA: 0x000EF77C File Offset: 0x000ED97C
	public void InitializeIfNeeded()
	{
		if (!this.initialized)
		{
			this.initialized = true;
			this.RefreshFontSize();
		}
	}

	// Token: 0x060029AF RID: 10671 RVA: 0x000EF794 File Offset: 0x000ED994
	public void DrawMenu()
	{
		if (ImGui.BeginMenu("Settings"))
		{
			bool flag = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Fabric;
			bool flag2 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Small;
			bool flag3 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Regular;
			bool flag4 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Large;
			if (ImGui.BeginMenu("Size"))
			{
				if (ImGui.Checkbox("Original Font", ref flag) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Fabric)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Fabric);
				}
				if (ImGui.Checkbox("Small Text", ref flag2) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Small)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Small);
				}
				if (ImGui.Checkbox("Regular Text", ref flag3) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Regular)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Regular);
				}
				if (ImGui.Checkbox("Large Text", ref flag4) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Large)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Large);
				}
				ImGui.EndMenu();
			}
			ImGui.EndMenu();
		}
	}

	// Token: 0x060029B0 RID: 10672 RVA: 0x000EF868 File Offset: 0x000EDA68
	public unsafe void SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory size)
	{
		this.fontSizeCategory = size;
		KPlayerPrefs.SetInt("Imgui_font_size_category", (int)size);
		ImGuiIOPtr io = ImGui.GetIO();
		if (size < (DevToolMenuFontSize.FontSizeCategory)io.Fonts.Fonts.Size)
		{
			ImFontPtr wrappedPtr = *io.Fonts.Fonts[(int)size];
			io.NativePtr->FontDefault = wrappedPtr;
		}
	}

	// Token: 0x04001897 RID: 6295
	public const string SETTINGS_KEY_FONT_SIZE_CATEGORY = "Imgui_font_size_category";

	// Token: 0x04001898 RID: 6296
	private DevToolMenuFontSize.FontSizeCategory fontSizeCategory;

	// Token: 0x02001561 RID: 5473
	public enum FontSizeCategory
	{
		// Token: 0x0400719A RID: 29082
		Fabric,
		// Token: 0x0400719B RID: 29083
		Small,
		// Token: 0x0400719C RID: 29084
		Regular,
		// Token: 0x0400719D RID: 29085
		Large
	}
}
