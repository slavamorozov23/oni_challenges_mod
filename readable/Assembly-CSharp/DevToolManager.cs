using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x020006A0 RID: 1696
public class DevToolManager
{
	// Token: 0x17000203 RID: 515
	// (get) Token: 0x060029C7 RID: 10695 RVA: 0x000F198D File Offset: 0x000EFB8D
	public bool Show
	{
		get
		{
			return this.showImGui;
		}
	}

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x060029C8 RID: 10696 RVA: 0x000F1995 File Offset: 0x000EFB95
	private bool quickDevEnabled
	{
		get
		{
			return DebugHandler.enabled && GenericGameSettings.instance.quickDevTools;
		}
	}

	// Token: 0x060029C9 RID: 10697 RVA: 0x000F19AC File Offset: 0x000EFBAC
	public DevToolManager()
	{
		DevToolManager.Instance = this;
		this.RegisterDevTool<DevToolSimDebug>("Debuggers/Sim Debug");
		this.RegisterDevTool<DevToolStateMachineDebug>("Debuggers/State Machine");
		this.RegisterDevTool<DevToolSaveGameInfo>("Debuggers/Save Game Info");
		this.RegisterDevTool<DevToolPerformanceInfo>("Debuggers/Performance Info");
		this.RegisterDevTool<DevToolPrintingPodDebug>("Debuggers/Printing Pod Debug");
		this.RegisterDevTool<DevToolBigBaseMutations>("Debuggers/Big Base Mutation Utilities");
		this.RegisterDevTool<DevToolNavGrid>("Debuggers/Nav Grid");
		this.RegisterDevTool<DevToolResearchDebugger>("Debuggers/Research");
		this.RegisterDevTool<DevToolStatusItems>("Debuggers/StatusItems");
		this.RegisterDevTool<DevToolUI>("Debuggers/UI");
		this.RegisterDevTool<DevToolUnlockedIds>("Debuggers/UnlockedIds List");
		this.RegisterDevTool<DevToolStringsTable>("Debuggers/StringsTable");
		this.RegisterDevTool<DevToolChoreDebugger>("Debuggers/Chore");
		this.RegisterDevTool<DevToolAllThingsCritter>("Debuggers/Critter Info");
		this.RegisterDevTool<DevToolBatchedAnimDebug>("Debuggers/Batched Anim");
		this.RegisterDevTool<DevTool_StoryTraits_Reveal>("Debuggers/Story Traits Reveal");
		this.RegisterDevTool<DevTool_StoryTrait_CritterManipulator>("Debuggers/Story Trait - Critter Manipulator");
		this.RegisterDevTool<DevToolAnimEventManager>("Debuggers/Anim Event Manager");
		this.RegisterDevTool<DevToolSceneBrowser>("Scene/Browser");
		this.RegisterDevTool<DevToolSceneInspector>("Scene/Inspector");
		this.menuNodes.AddAction("Help/" + UI.FRONTEND.DEVTOOLS.TITLE.text, delegate
		{
			this.warning.ShouldDrawWindow = true;
		});
		this.RegisterDevTool<DevToolCommandPalette>("Help/Command Palette");
		this.RegisterAdditionalDevToolsByReflection();
	}

	// Token: 0x060029CA RID: 10698 RVA: 0x000F1B20 File Offset: 0x000EFD20
	public void Init()
	{
		this.UserAcceptedWarning = (KPlayerPrefs.GetInt("ShowDevtools", 0) == 1);
	}

	// Token: 0x060029CB RID: 10699 RVA: 0x000F1B38 File Offset: 0x000EFD38
	private void RegisterDevTool<T>(string location) where T : DevTool, new()
	{
		this.menuNodes.AddAction(location, delegate
		{
			this.panels.AddPanelFor<T>();
		});
		this.dontAutomaticallyRegisterTypes.Add(typeof(T));
		this.devToolNameDict[typeof(T)] = Path.GetFileName(location);
	}

	// Token: 0x060029CC RID: 10700 RVA: 0x000F1B90 File Offset: 0x000EFD90
	private void RegisterAdditionalDevToolsByReflection()
	{
		using (List<Type>.Enumerator enumerator = ReflectionUtil.CollectTypesThatInheritOrImplement<DevTool>(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Type type = enumerator.Current;
				if (!type.IsAbstract && !this.dontAutomaticallyRegisterTypes.Contains(type) && ReflectionUtil.HasDefaultConstructor(type))
				{
					this.menuNodes.AddAction("Debuggers/" + DevToolUtil.GenerateDevToolName(type), delegate
					{
						this.panels.AddPanelFor((DevTool)Activator.CreateInstance(type));
					});
				}
			}
		}
	}

	// Token: 0x060029CD RID: 10701 RVA: 0x000F1C4C File Offset: 0x000EFE4C
	public void UpdateShouldShowTools()
	{
		if (!DebugHandler.enabled)
		{
			this.showImGui = false;
			return;
		}
		bool flag = Input.GetKeyDown(KeyCode.BackQuote) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl));
		if (!this.toggleKeyWasDown && flag)
		{
			this.showImGui = !this.showImGui;
		}
		this.toggleKeyWasDown = flag;
	}

	// Token: 0x060029CE RID: 10702 RVA: 0x000F1CB4 File Offset: 0x000EFEB4
	public void UpdateTools()
	{
		if (!DebugHandler.enabled)
		{
			return;
		}
		if (this.showImGui)
		{
			if (this.warning.ShouldDrawWindow)
			{
				this.warning.DrawWindow(out this.warning.ShouldDrawWindow);
			}
			if (!this.UserAcceptedWarning)
			{
				this.warning.DrawMenuBar();
			}
			else
			{
				this.DrawMenu();
				this.panels.Render();
				if (this.showImguiState)
				{
					if (ImGui.Begin("ImGui state", ref this.showImguiState))
					{
						ImGui.Checkbox("ImGui.GetIO().WantCaptureMouse", ImGui.GetIO().WantCaptureMouse);
						ImGui.Checkbox("ImGui.GetIO().WantCaptureKeyboard", ImGui.GetIO().WantCaptureKeyboard);
					}
					ImGui.End();
				}
				if (this.showImguiDemo)
				{
					ImGui.ShowDemoWindow(ref this.showImguiDemo);
				}
			}
		}
		this.UpdateConsumingGameInputs();
		this.UpdateShortcuts();
	}

	// Token: 0x060029CF RID: 10703 RVA: 0x000F1D8B File Offset: 0x000EFF8B
	private void UpdateShortcuts()
	{
		if ((this.showImGui || this.quickDevEnabled) && this.UserAcceptedWarning)
		{
			this.<UpdateShortcuts>g__DoUpdate|26_0();
		}
	}

	// Token: 0x060029D0 RID: 10704 RVA: 0x000F1DAC File Offset: 0x000EFFAC
	private void DrawMenu()
	{
		this.menuFontSize.InitializeIfNeeded();
		if (ImGui.BeginMainMenuBar())
		{
			this.menuNodes.Draw();
			this.menuFontSize.DrawMenu();
			if (ImGui.BeginMenu("IMGUI"))
			{
				ImGui.Checkbox("ImGui state", ref this.showImguiState);
				ImGui.Checkbox("ImGui Demo", ref this.showImguiDemo);
				ImGui.EndMenu();
			}
			ImGui.EndMainMenuBar();
		}
	}

	// Token: 0x060029D1 RID: 10705 RVA: 0x000F1E1C File Offset: 0x000F001C
	private unsafe void UpdateConsumingGameInputs()
	{
		this.doesImGuiWantInput = false;
		if (this.showImGui)
		{
			this.doesImGuiWantInput = (*ImGui.GetIO().WantCaptureMouse || *ImGui.GetIO().WantCaptureKeyboard);
			if (!this.prevDoesImGuiWantInput && this.doesImGuiWantInput)
			{
				DevToolManager.<UpdateConsumingGameInputs>g__OnInputEnterImGui|28_0();
			}
			if (this.prevDoesImGuiWantInput && !this.doesImGuiWantInput)
			{
				DevToolManager.<UpdateConsumingGameInputs>g__OnInputExitImGui|28_1();
			}
		}
		if (this.prevShowImGui && this.prevDoesImGuiWantInput && !this.showImGui)
		{
			DevToolManager.<UpdateConsumingGameInputs>g__OnInputExitImGui|28_1();
		}
		this.prevShowImGui = this.showImGui;
		this.prevDoesImGuiWantInput = this.doesImGuiWantInput;
		KInputManager.devToolFocus = (this.showImGui && this.doesImGuiWantInput);
	}

	// Token: 0x060029D4 RID: 10708 RVA: 0x000F1EF0 File Offset: 0x000F00F0
	[CompilerGenerated]
	private void <UpdateShortcuts>g__DoUpdate|26_0()
	{
		if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Space))
		{
			DevToolCommandPalette.Init();
			this.showImGui = true;
		}
		if (Input.GetKeyDown(KeyCode.Comma))
		{
			DevToolUI.PingHoveredObject();
			this.showImGui = true;
		}
	}

	// Token: 0x060029D5 RID: 10709 RVA: 0x000F1F40 File Offset: 0x000F0140
	[CompilerGenerated]
	internal static void <UpdateConsumingGameInputs>g__OnInputEnterImGui|28_0()
	{
		UnityMouseCatcherUI.SetEnabled(true);
		GameInputManager inputManager = Global.GetInputManager();
		for (int i = 0; i < inputManager.GetControllerCount(); i++)
		{
			inputManager.GetController(i).HandleCancelInput();
		}
	}

	// Token: 0x060029D6 RID: 10710 RVA: 0x000F1F76 File Offset: 0x000F0176
	[CompilerGenerated]
	internal static void <UpdateConsumingGameInputs>g__OnInputExitImGui|28_1()
	{
		UnityMouseCatcherUI.SetEnabled(false);
	}

	// Token: 0x040018BE RID: 6334
	public const string SHOW_DEVTOOLS = "ShowDevtools";

	// Token: 0x040018BF RID: 6335
	public static DevToolManager Instance;

	// Token: 0x040018C0 RID: 6336
	private bool toggleKeyWasDown;

	// Token: 0x040018C1 RID: 6337
	private bool showImGui;

	// Token: 0x040018C2 RID: 6338
	private bool prevShowImGui;

	// Token: 0x040018C3 RID: 6339
	private bool doesImGuiWantInput;

	// Token: 0x040018C4 RID: 6340
	private bool prevDoesImGuiWantInput;

	// Token: 0x040018C5 RID: 6341
	private bool showImguiState;

	// Token: 0x040018C6 RID: 6342
	private bool showImguiDemo;

	// Token: 0x040018C7 RID: 6343
	public bool UserAcceptedWarning;

	// Token: 0x040018C8 RID: 6344
	private DevToolWarning warning = new DevToolWarning();

	// Token: 0x040018C9 RID: 6345
	private DevToolMenuFontSize menuFontSize = new DevToolMenuFontSize();

	// Token: 0x040018CA RID: 6346
	public DevPanelList panels = new DevPanelList();

	// Token: 0x040018CB RID: 6347
	public DevToolMenuNodeList menuNodes = new DevToolMenuNodeList();

	// Token: 0x040018CC RID: 6348
	public Dictionary<Type, string> devToolNameDict = new Dictionary<Type, string>();

	// Token: 0x040018CD RID: 6349
	private HashSet<Type> dontAutomaticallyRegisterTypes = new HashSet<Type>();
}
