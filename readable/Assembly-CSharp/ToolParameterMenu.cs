using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000EB2 RID: 3762
[AddComponentMenu("KMonoBehaviour/scripts/ToolParameterMenu")]
public class ToolParameterMenu : KMonoBehaviour
{
	// Token: 0x14000032 RID: 50
	// (add) Token: 0x06007894 RID: 30868 RVA: 0x002E5DD0 File Offset: 0x002E3FD0
	// (remove) Token: 0x06007895 RID: 30869 RVA: 0x002E5E08 File Offset: 0x002E4008
	public event System.Action onParametersChanged;

	// Token: 0x06007896 RID: 30870 RVA: 0x002E5E3D File Offset: 0x002E403D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ClearMenu();
	}

	// Token: 0x06007897 RID: 30871 RVA: 0x002E5E4C File Offset: 0x002E404C
	public void PopulateMenu(Dictionary<string, ToolParameterMenu.ToggleState> parameters)
	{
		this.ClearMenu();
		this.currentParameters = parameters;
		foreach (KeyValuePair<string, ToolParameterMenu.ToggleState> keyValuePair in parameters)
		{
			GameObject gameObject = Util.KInstantiateUI(this.widgetPrefab, this.widgetContainer, true);
			gameObject.GetComponentInChildren<LocText>().text = Strings.Get("STRINGS.UI.TOOLS.FILTERLAYERS." + keyValuePair.Key + ".NAME");
			ToolTip componentInChildren = gameObject.GetComponentInChildren<ToolTip>();
			if (componentInChildren != null)
			{
				componentInChildren.SetSimpleTooltip(Strings.Get("STRINGS.UI.TOOLS.FILTERLAYERS." + keyValuePair.Key + ".TOOLTIP"));
			}
			this.widgets.Add(keyValuePair.Key, gameObject);
			MultiToggle toggle = gameObject.GetComponentInChildren<MultiToggle>();
			ToolParameterMenu.ToggleState value = keyValuePair.Value;
			if (value == ToolParameterMenu.ToggleState.Disabled)
			{
				toggle.ChangeState(2);
			}
			else if (value == ToolParameterMenu.ToggleState.On)
			{
				toggle.ChangeState(1);
				this.lastEnabledFilter = keyValuePair.Key;
			}
			else
			{
				toggle.ChangeState(0);
			}
			MultiToggle toggle2 = toggle;
			toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
			{
				foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.widgets)
				{
					if (keyValuePair2.Value == toggle.transform.parent.gameObject)
					{
						if (this.currentParameters[keyValuePair2.Key] == ToolParameterMenu.ToggleState.Disabled)
						{
							break;
						}
						this.ChangeToSetting(keyValuePair2.Key);
						this.OnChange();
						break;
					}
				}
			}));
		}
		this.content.SetActive(true);
	}

	// Token: 0x06007898 RID: 30872 RVA: 0x002E5FD0 File Offset: 0x002E41D0
	public void ClearMenu()
	{
		this.content.SetActive(false);
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.widgets)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.widgets.Clear();
	}

	// Token: 0x06007899 RID: 30873 RVA: 0x002E6040 File Offset: 0x002E4240
	private void ChangeToSetting(string key)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.widgets)
		{
			if (this.currentParameters[keyValuePair.Key] != ToolParameterMenu.ToggleState.Disabled)
			{
				this.currentParameters[keyValuePair.Key] = ToolParameterMenu.ToggleState.Off;
			}
		}
		this.currentParameters[key] = ToolParameterMenu.ToggleState.On;
	}

	// Token: 0x0600789A RID: 30874 RVA: 0x002E60C4 File Offset: 0x002E42C4
	private void OnChange()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.widgets)
		{
			switch (this.currentParameters[keyValuePair.Key])
			{
			case ToolParameterMenu.ToggleState.On:
				keyValuePair.Value.GetComponentInChildren<MultiToggle>().ChangeState(1);
				this.lastEnabledFilter = keyValuePair.Key;
				break;
			case ToolParameterMenu.ToggleState.Off:
				keyValuePair.Value.GetComponentInChildren<MultiToggle>().ChangeState(0);
				break;
			case ToolParameterMenu.ToggleState.Disabled:
				keyValuePair.Value.GetComponentInChildren<MultiToggle>().ChangeState(2);
				break;
			}
		}
		if (this.onParametersChanged != null)
		{
			this.onParametersChanged();
		}
	}

	// Token: 0x0600789B RID: 30875 RVA: 0x002E6194 File Offset: 0x002E4394
	public string GetLastEnabledFilter()
	{
		return this.lastEnabledFilter;
	}

	// Token: 0x0400540D RID: 21517
	public GameObject content;

	// Token: 0x0400540E RID: 21518
	public GameObject widgetContainer;

	// Token: 0x0400540F RID: 21519
	public GameObject widgetPrefab;

	// Token: 0x04005411 RID: 21521
	private Dictionary<string, GameObject> widgets = new Dictionary<string, GameObject>();

	// Token: 0x04005412 RID: 21522
	private Dictionary<string, ToolParameterMenu.ToggleState> currentParameters;

	// Token: 0x04005413 RID: 21523
	private string lastEnabledFilter;

	// Token: 0x0200211A RID: 8474
	public class FILTERLAYERS
	{
		// Token: 0x0400981D RID: 38941
		public static string BUILDINGS = "BUILDINGS";

		// Token: 0x0400981E RID: 38942
		public static string TILES = "TILES";

		// Token: 0x0400981F RID: 38943
		public static string WIRES = "WIRES";

		// Token: 0x04009820 RID: 38944
		public static string LIQUIDCONDUIT = "LIQUIDPIPES";

		// Token: 0x04009821 RID: 38945
		public static string GASCONDUIT = "GASPIPES";

		// Token: 0x04009822 RID: 38946
		public static string SOLIDCONDUIT = "SOLIDCONDUITS";

		// Token: 0x04009823 RID: 38947
		public static string CLEANANDCLEAR = "CLEANANDCLEAR";

		// Token: 0x04009824 RID: 38948
		public static string DIGPLACER = "DIGPLACER";

		// Token: 0x04009825 RID: 38949
		public static string LOGIC = "LOGIC";

		// Token: 0x04009826 RID: 38950
		public static string BACKWALL = "BACKWALL";

		// Token: 0x04009827 RID: 38951
		public static string CONSTRUCTION = "CONSTRUCTION";

		// Token: 0x04009828 RID: 38952
		public static string DIG = "DIG";

		// Token: 0x04009829 RID: 38953
		public static string CLEAN = "CLEAN";

		// Token: 0x0400982A RID: 38954
		public static string OPERATE = "OPERATE";

		// Token: 0x0400982B RID: 38955
		public static string METAL = "METAL";

		// Token: 0x0400982C RID: 38956
		public static string BUILDABLE = "BUILDABLE";

		// Token: 0x0400982D RID: 38957
		public static string FILTER = "FILTER";

		// Token: 0x0400982E RID: 38958
		public static string LIQUIFIABLE = "LIQUIFIABLE";

		// Token: 0x0400982F RID: 38959
		public static string LIQUID = "LIQUID";

		// Token: 0x04009830 RID: 38960
		public static string CONSUMABLEORE = "CONSUMABLEORE";

		// Token: 0x04009831 RID: 38961
		public static string ORGANICS = "ORGANICS";

		// Token: 0x04009832 RID: 38962
		public static string FARMABLE = "FARMABLE";

		// Token: 0x04009833 RID: 38963
		public static string GAS = "GAS";

		// Token: 0x04009834 RID: 38964
		public static string MISC = "MISC";

		// Token: 0x04009835 RID: 38965
		public static string HEATFLOW = "HEATFLOW";

		// Token: 0x04009836 RID: 38966
		public static string ABSOLUTETEMPERATURE = "ABSOLUTETEMPERATURE";

		// Token: 0x04009837 RID: 38967
		public static string RELATIVETEMPERATURE = "RELATIVETEMPERATURE";

		// Token: 0x04009838 RID: 38968
		public static string ADAPTIVETEMPERATURE = "ADAPTIVETEMPERATURE";

		// Token: 0x04009839 RID: 38969
		public static string STATECHANGE = "STATECHANGE";

		// Token: 0x0400983A RID: 38970
		public static string ALL = "ALL";
	}

	// Token: 0x0200211B RID: 8475
	public enum ToggleState
	{
		// Token: 0x0400983C RID: 38972
		On,
		// Token: 0x0400983D RID: 38973
		Off,
		// Token: 0x0400983E RID: 38974
		Disabled
	}
}
