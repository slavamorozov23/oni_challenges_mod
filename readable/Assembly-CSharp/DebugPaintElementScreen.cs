using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CF2 RID: 3314
public class DebugPaintElementScreen : KScreen
{
	// Token: 0x1700077A RID: 1914
	// (get) Token: 0x06006657 RID: 26199 RVA: 0x002685FD File Offset: 0x002667FD
	// (set) Token: 0x06006658 RID: 26200 RVA: 0x00268604 File Offset: 0x00266804
	public static DebugPaintElementScreen Instance { get; private set; }

	// Token: 0x06006659 RID: 26201 RVA: 0x0026860C File Offset: 0x0026680C
	public static void DestroyInstance()
	{
		DebugPaintElementScreen.Instance = null;
	}

	// Token: 0x0600665A RID: 26202 RVA: 0x00268614 File Offset: 0x00266814
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DebugPaintElementScreen.Instance = this;
		this.SetupLocText();
		this.inputFields.Add(this.massInput);
		this.inputFields.Add(this.temperatureInput);
		this.inputFields.Add(this.diseaseCountInput);
		this.inputFields.Add(this.filterInput);
		foreach (KInputTextField kinputTextField in this.inputFields)
		{
			kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
			{
				base.isEditing = true;
			}));
			kinputTextField.onEndEdit.AddListener(delegate(string value)
			{
				base.isEditing = false;
			});
		}
		this.temperatureInput.onEndEdit.AddListener(delegate(string value)
		{
			this.OnChangeTemperature();
		});
		this.massInput.onEndEdit.AddListener(delegate(string value)
		{
			this.OnChangeMass();
		});
		this.diseaseCountInput.onEndEdit.AddListener(delegate(string value)
		{
			this.OnDiseaseCountChange();
		});
		base.gameObject.SetActive(false);
		this.activateOnSpawn = true;
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x0600665B RID: 26203 RVA: 0x0026875C File Offset: 0x0026695C
	private void SetupLocText()
	{
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("Title").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.TITLE;
		component.GetReference<LocText>("ElementLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ELEMENT;
		component.GetReference<LocText>("MassLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.MASS_KG;
		component.GetReference<LocText>("TemperatureLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.TEMPERATURE_KELVIN;
		component.GetReference<LocText>("DiseaseLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE;
		component.GetReference<LocText>("DiseaseCountLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE_COUNT;
		component.GetReference<LocText>("AddFoWMaskLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ADD_FOW_MASK;
		component.GetReference<LocText>("RemoveFoWMaskLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.REMOVE_FOW_MASK;
		this.elementButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ELEMENT;
		this.diseaseButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE;
		this.paintButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.PAINT;
		this.fillButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.FILL;
		this.spawnButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.SPAWN_ALL;
		this.sampleButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.SAMPLE;
		this.storeButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.STORE;
		this.affectBuildings.transform.parent.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.BUILDINGS;
		this.affectCells.transform.parent.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.CELLS;
	}

	// Token: 0x0600665C RID: 26204 RVA: 0x00268950 File Offset: 0x00266B50
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.element = SimHashes.Ice;
		this.diseaseIdx = byte.MaxValue;
		this.ConfigureElements();
		List<string> list = new List<string>();
		list.Insert(0, "None");
		foreach (Disease disease in Db.Get().Diseases.resources)
		{
			list.Add(disease.Name);
		}
		this.diseasePopup.SetOptions(list.ToArray());
		KPopupMenu kpopupMenu = this.diseasePopup;
		kpopupMenu.OnSelect = (Action<string, int>)Delegate.Combine(kpopupMenu.OnSelect, new Action<string, int>(this.OnSelectDisease));
		this.SelectDiseaseOption((int)this.diseaseIdx);
		this.paintButton.onClick += this.OnClickPaint;
		this.fillButton.onClick += this.OnClickFill;
		this.sampleButton.onClick += this.OnClickSample;
		this.storeButton.onClick += this.OnClickStore;
		if (SaveGame.Instance.worldGenSpawner.SpawnsRemain())
		{
			this.spawnButton.onClick += this.OnClickSpawn;
		}
		KPopupMenu kpopupMenu2 = this.elementPopup;
		kpopupMenu2.OnSelect = (Action<string, int>)Delegate.Combine(kpopupMenu2.OnSelect, new Action<string, int>(this.OnSelectElement));
		this.elementButton.onClick += this.elementPopup.OnClick;
		this.diseaseButton.onClick += this.diseasePopup.OnClick;
	}

	// Token: 0x0600665D RID: 26205 RVA: 0x00268B0C File Offset: 0x00266D0C
	private void FilterElements(string filterValue)
	{
		if (string.IsNullOrEmpty(filterValue))
		{
			foreach (KButtonMenu.ButtonInfo buttonInfo in this.elementPopup.GetButtons())
			{
				buttonInfo.uibutton.gameObject.SetActive(true);
			}
			return;
		}
		filterValue = this.filter.ToLower();
		foreach (KButtonMenu.ButtonInfo buttonInfo2 in this.elementPopup.GetButtons())
		{
			buttonInfo2.uibutton.gameObject.SetActive(buttonInfo2.text.ToLower().Contains(filterValue));
		}
	}

	// Token: 0x0600665E RID: 26206 RVA: 0x00268BD8 File Offset: 0x00266DD8
	private void ConfigureElements()
	{
		if (this.filter != null)
		{
			this.filter = this.filter.ToLower();
		}
		List<DebugPaintElementScreen.ElemDisplayInfo> list = new List<DebugPaintElementScreen.ElemDisplayInfo>();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.name != "Element Not Loaded" && element.substance != null && element.substance.showInEditor && (string.IsNullOrEmpty(this.filter) || element.name.ToLower().Contains(this.filter)))
			{
				list.Add(new DebugPaintElementScreen.ElemDisplayInfo
				{
					id = element.id,
					displayStr = element.name + " (" + element.GetStateString() + ")"
				});
			}
		}
		list.Sort((DebugPaintElementScreen.ElemDisplayInfo a, DebugPaintElementScreen.ElemDisplayInfo b) => a.displayStr.CompareTo(b.displayStr));
		if (string.IsNullOrEmpty(this.filter))
		{
			SimHashes[] array = new SimHashes[]
			{
				SimHashes.SlimeMold,
				SimHashes.Vacuum,
				SimHashes.Dirt,
				SimHashes.CarbonDioxide,
				SimHashes.Water,
				SimHashes.Oxygen
			};
			for (int i = 0; i < array.Length; i++)
			{
				Element element2 = ElementLoader.FindElementByHash(array[i]);
				list.Insert(0, new DebugPaintElementScreen.ElemDisplayInfo
				{
					id = element2.id,
					displayStr = element2.name + " (" + element2.GetStateString() + ")"
				});
			}
		}
		this.options_list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (DebugPaintElementScreen.ElemDisplayInfo elemDisplayInfo in list)
		{
			list2.Add(elemDisplayInfo.displayStr);
			this.options_list.Add(elemDisplayInfo.id.ToString());
		}
		this.elementPopup.SetOptions(list2);
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].id == this.element)
			{
				this.elementPopup.SelectOption(list2[j], j);
			}
		}
		this.elementPopup.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0f, 1f);
	}

	// Token: 0x0600665F RID: 26207 RVA: 0x00268E58 File Offset: 0x00267058
	private void OnClickSpawn()
	{
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			worldContainer.SetDiscovered(true);
		}
		SaveGame.Instance.worldGenSpawner.SpawnEverything();
		this.spawnButton.GetComponent<KButton>().isInteractable = false;
	}

	// Token: 0x06006660 RID: 26208 RVA: 0x00268ED0 File Offset: 0x002670D0
	private void OnClickPaint()
	{
		this.OnChangeMass();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		this.OnChangeFOWReveal();
		DebugTool.Instance.Activate(DebugTool.Type.ReplaceSubstance);
	}

	// Token: 0x06006661 RID: 26209 RVA: 0x00268EF5 File Offset: 0x002670F5
	private void OnClickStore()
	{
		this.OnChangeMass();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		this.OnChangeFOWReveal();
		DebugTool.Instance.Activate(DebugTool.Type.StoreSubstance);
	}

	// Token: 0x06006662 RID: 26210 RVA: 0x00268F1A File Offset: 0x0026711A
	private void OnClickSample()
	{
		this.OnChangeMass();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		this.OnChangeFOWReveal();
		DebugTool.Instance.Activate(DebugTool.Type.Sample);
	}

	// Token: 0x06006663 RID: 26211 RVA: 0x00268F3F File Offset: 0x0026713F
	private void OnClickFill()
	{
		this.OnChangeMass();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		DebugTool.Instance.Activate(DebugTool.Type.FillReplaceSubstance);
	}

	// Token: 0x06006664 RID: 26212 RVA: 0x00268F5E File Offset: 0x0026715E
	private void OnSelectElement(string str, int index)
	{
		this.element = (SimHashes)Enum.Parse(typeof(SimHashes), this.options_list[index]);
		this.elementButton.GetComponentInChildren<LocText>().text = str;
	}

	// Token: 0x06006665 RID: 26213 RVA: 0x00268F97 File Offset: 0x00267197
	private void OnSelectElement(SimHashes element)
	{
		this.element = element;
		this.elementButton.GetComponentInChildren<LocText>().text = ElementLoader.FindElementByHash(element).name;
	}

	// Token: 0x06006666 RID: 26214 RVA: 0x00268FBC File Offset: 0x002671BC
	private void OnSelectDisease(string str, int index)
	{
		this.diseaseIdx = byte.MaxValue;
		for (int i = 0; i < Db.Get().Diseases.Count; i++)
		{
			if (Db.Get().Diseases[i].Name == str)
			{
				this.diseaseIdx = (byte)i;
			}
		}
		this.SelectDiseaseOption((int)this.diseaseIdx);
	}

	// Token: 0x06006667 RID: 26215 RVA: 0x00269020 File Offset: 0x00267220
	private void SelectDiseaseOption(int diseaseIdx)
	{
		if (diseaseIdx == 255)
		{
			this.diseaseButton.GetComponentInChildren<LocText>().text = "None";
			return;
		}
		string name = Db.Get().Diseases[diseaseIdx].Name;
		this.diseaseButton.GetComponentInChildren<LocText>().text = name;
	}

	// Token: 0x06006668 RID: 26216 RVA: 0x00269074 File Offset: 0x00267274
	private void OnChangeFOWReveal()
	{
		if (this.paintPreventFOWReveal.isOn)
		{
			this.paintAllowFOWReveal.isOn = false;
		}
		if (this.paintAllowFOWReveal.isOn)
		{
			this.paintPreventFOWReveal.isOn = false;
		}
		this.set_prevent_fow_reveal = this.paintPreventFOWReveal.isOn;
		this.set_allow_fow_reveal = this.paintAllowFOWReveal.isOn;
	}

	// Token: 0x06006669 RID: 26217 RVA: 0x002690D8 File Offset: 0x002672D8
	public void OnChangeMass()
	{
		float num;
		try
		{
			num = Convert.ToSingle(this.massInput.text);
		}
		catch
		{
			num = -1f;
		}
		if (num <= 0f)
		{
			num = 1f;
			this.massInput.text = "1";
		}
		this.mass = num;
	}

	// Token: 0x0600666A RID: 26218 RVA: 0x00269138 File Offset: 0x00267338
	public void OnChangeTemperature()
	{
		float num;
		try
		{
			num = Convert.ToSingle(this.temperatureInput.text);
		}
		catch
		{
			num = -1f;
		}
		if (num <= 0f)
		{
			num = 1f;
			this.temperatureInput.text = "1";
		}
		this.temperature = num;
	}

	// Token: 0x0600666B RID: 26219 RVA: 0x00269198 File Offset: 0x00267398
	public void OnDiseaseCountChange()
	{
		int num;
		int.TryParse(this.diseaseCountInput.text, out num);
		if (num < 0)
		{
			num = 0;
			this.diseaseCountInput.text = "0";
		}
		this.diseaseCount = num;
	}

	// Token: 0x0600666C RID: 26220 RVA: 0x002691D5 File Offset: 0x002673D5
	public void OnElementsFilterEdited(string new_filter)
	{
		this.filter = (string.IsNullOrEmpty(this.filterInput.text) ? null : this.filterInput.text);
		this.FilterElements(this.filter);
	}

	// Token: 0x0600666D RID: 26221 RVA: 0x0026920C File Offset: 0x0026740C
	public void SampleCell(int cell)
	{
		this.massInput.text = Grid.Mass[cell].ToString();
		this.temperatureInput.text = Grid.Temperature[cell].ToString();
		this.OnSelectElement(ElementLoader.GetElementID(Grid.Element[cell].tag));
		this.OnChangeMass();
		this.OnChangeTemperature();
	}

	// Token: 0x040045D5 RID: 17877
	[Header("Current State")]
	public SimHashes element;

	// Token: 0x040045D6 RID: 17878
	[NonSerialized]
	public float mass = 1000f;

	// Token: 0x040045D7 RID: 17879
	[NonSerialized]
	public float temperature = -1f;

	// Token: 0x040045D8 RID: 17880
	[NonSerialized]
	public bool set_prevent_fow_reveal;

	// Token: 0x040045D9 RID: 17881
	[NonSerialized]
	public bool set_allow_fow_reveal;

	// Token: 0x040045DA RID: 17882
	[NonSerialized]
	public int diseaseCount;

	// Token: 0x040045DB RID: 17883
	public byte diseaseIdx;

	// Token: 0x040045DC RID: 17884
	[Header("Popup Buttons")]
	[SerializeField]
	private KButton elementButton;

	// Token: 0x040045DD RID: 17885
	[SerializeField]
	private KButton diseaseButton;

	// Token: 0x040045DE RID: 17886
	[Header("Popup Menus")]
	[SerializeField]
	private KPopupMenu elementPopup;

	// Token: 0x040045DF RID: 17887
	[SerializeField]
	private KPopupMenu diseasePopup;

	// Token: 0x040045E0 RID: 17888
	[Header("Value Inputs")]
	[SerializeField]
	private KInputTextField massInput;

	// Token: 0x040045E1 RID: 17889
	[SerializeField]
	private KInputTextField temperatureInput;

	// Token: 0x040045E2 RID: 17890
	[SerializeField]
	private KInputTextField diseaseCountInput;

	// Token: 0x040045E3 RID: 17891
	[SerializeField]
	private KInputTextField filterInput;

	// Token: 0x040045E4 RID: 17892
	[Header("Tool Buttons")]
	[SerializeField]
	private KButton paintButton;

	// Token: 0x040045E5 RID: 17893
	[SerializeField]
	private KButton fillButton;

	// Token: 0x040045E6 RID: 17894
	[SerializeField]
	private KButton sampleButton;

	// Token: 0x040045E7 RID: 17895
	[SerializeField]
	private KButton spawnButton;

	// Token: 0x040045E8 RID: 17896
	[SerializeField]
	private KButton storeButton;

	// Token: 0x040045E9 RID: 17897
	[Header("Parameter Toggles")]
	public Toggle paintElement;

	// Token: 0x040045EA RID: 17898
	public Toggle paintMass;

	// Token: 0x040045EB RID: 17899
	public Toggle paintTemperature;

	// Token: 0x040045EC RID: 17900
	public Toggle paintDisease;

	// Token: 0x040045ED RID: 17901
	public Toggle paintDiseaseCount;

	// Token: 0x040045EE RID: 17902
	public Toggle affectBuildings;

	// Token: 0x040045EF RID: 17903
	public Toggle affectCells;

	// Token: 0x040045F0 RID: 17904
	public Toggle paintPreventFOWReveal;

	// Token: 0x040045F1 RID: 17905
	public Toggle paintAllowFOWReveal;

	// Token: 0x040045F2 RID: 17906
	private List<KInputTextField> inputFields = new List<KInputTextField>();

	// Token: 0x040045F3 RID: 17907
	private List<string> options_list = new List<string>();

	// Token: 0x040045F4 RID: 17908
	private string filter;

	// Token: 0x02001F23 RID: 7971
	private struct ElemDisplayInfo
	{
		// Token: 0x040091A3 RID: 37283
		public SimHashes id;

		// Token: 0x040091A4 RID: 37284
		public string displayStr;
	}
}
