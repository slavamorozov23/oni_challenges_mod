using System;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000E72 RID: 3698
public class SearchBar : KMonoBehaviour
{
	// Token: 0x1700081F RID: 2079
	// (get) Token: 0x06007581 RID: 30081 RVA: 0x002CD4FE File Offset: 0x002CB6FE
	public string CurrentSearchValue
	{
		get
		{
			if (!string.IsNullOrEmpty(this.inputField.text))
			{
				return this.inputField.text;
			}
			return "";
		}
	}

	// Token: 0x17000820 RID: 2080
	// (get) Token: 0x06007582 RID: 30082 RVA: 0x002CD523 File Offset: 0x002CB723
	public bool IsInputFieldEmpty
	{
		get
		{
			return this.inputField.text == "";
		}
	}

	// Token: 0x17000821 RID: 2081
	// (get) Token: 0x06007584 RID: 30084 RVA: 0x002CD543 File Offset: 0x002CB743
	// (set) Token: 0x06007583 RID: 30083 RVA: 0x002CD53A File Offset: 0x002CB73A
	public bool isEditing { get; protected set; }

	// Token: 0x06007585 RID: 30085 RVA: 0x002CD54B File Offset: 0x002CB74B
	public virtual void SetPlaceholder(string text)
	{
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = text;
	}

	// Token: 0x06007586 RID: 30086 RVA: 0x002CD564 File Offset: 0x002CB764
	protected override void OnSpawn()
	{
		this.inputField.ActivateInputField();
		KInputTextField kinputTextField = this.inputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(this.OnFocus));
		this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		this.inputField.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
		this.clearButton.onClick += this.ClearSearch;
		this.SetPlaceholder(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER);
	}

	// Token: 0x06007587 RID: 30087 RVA: 0x002CD606 File Offset: 0x002CB806
	protected void SetEditingState(bool editing)
	{
		this.isEditing = editing;
		Action<bool> editingStateChanged = this.EditingStateChanged;
		if (editingStateChanged != null)
		{
			editingStateChanged(this.isEditing);
		}
		KScreenManager.Instance.RefreshStack();
	}

	// Token: 0x06007588 RID: 30088 RVA: 0x002CD630 File Offset: 0x002CB830
	protected virtual void OnValueChanged(string value)
	{
		Action<string> valueChanged = this.ValueChanged;
		if (valueChanged == null)
		{
			return;
		}
		valueChanged(value);
	}

	// Token: 0x06007589 RID: 30089 RVA: 0x002CD643 File Offset: 0x002CB843
	protected virtual void OnEndEdit(string value)
	{
		this.SetEditingState(false);
	}

	// Token: 0x0600758A RID: 30090 RVA: 0x002CD64C File Offset: 0x002CB84C
	protected virtual void OnFocus()
	{
		this.SetEditingState(true);
		UISounds.PlaySound(UISounds.Sound.ClickHUD);
		System.Action focused = this.Focused;
		if (focused == null)
		{
			return;
		}
		focused();
	}

	// Token: 0x0600758B RID: 30091 RVA: 0x002CD66B File Offset: 0x002CB86B
	public virtual void ClearSearch()
	{
		this.SetValue("");
	}

	// Token: 0x0600758C RID: 30092 RVA: 0x002CD678 File Offset: 0x002CB878
	public void SetValue(string value)
	{
		this.inputField.text = value;
		Action<string> valueChanged = this.ValueChanged;
		if (valueChanged == null)
		{
			return;
		}
		valueChanged(value);
	}

	// Token: 0x04005150 RID: 20816
	[SerializeField]
	protected KInputTextField inputField;

	// Token: 0x04005151 RID: 20817
	[SerializeField]
	protected KButton clearButton;

	// Token: 0x04005153 RID: 20819
	public Action<string> ValueChanged;

	// Token: 0x04005154 RID: 20820
	public Action<bool> EditingStateChanged;

	// Token: 0x04005155 RID: 20821
	public System.Action Focused;
}
