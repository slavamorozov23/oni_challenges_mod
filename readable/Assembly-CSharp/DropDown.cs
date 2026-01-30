using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D04 RID: 3332
[AddComponentMenu("KMonoBehaviour/scripts/DropDown")]
public class DropDown : KMonoBehaviour
{
	// Token: 0x17000784 RID: 1924
	// (get) Token: 0x06006716 RID: 26390 RVA: 0x0026D937 File Offset: 0x0026BB37
	// (set) Token: 0x06006717 RID: 26391 RVA: 0x0026D93F File Offset: 0x0026BB3F
	public bool open { get; private set; }

	// Token: 0x17000785 RID: 1925
	// (get) Token: 0x06006718 RID: 26392 RVA: 0x0026D948 File Offset: 0x0026BB48
	public List<IListableOption> Entries
	{
		get
		{
			return this.entries;
		}
	}

	// Token: 0x06006719 RID: 26393 RVA: 0x0026D950 File Offset: 0x0026BB50
	public void Initialize(IEnumerable<IListableOption> contentKeys, Action<IListableOption, object> onEntrySelectedAction, Func<IListableOption, IListableOption, object, int> sortFunction = null, Action<DropDownEntry, object> refreshAction = null, bool displaySelectedValueWhenClosed = true, object targetData = null)
	{
		this.targetData = targetData;
		this.sortFunction = sortFunction;
		this.onEntrySelectedAction = onEntrySelectedAction;
		this.displaySelectedValueWhenClosed = displaySelectedValueWhenClosed;
		this.rowRefreshAction = refreshAction;
		this.ChangeContent(contentKeys);
		this.openButton.ClearOnClick();
		this.openButton.onClick += delegate()
		{
			this.OnClick();
		};
		this.canvasScaler = GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>();
	}

	// Token: 0x0600671A RID: 26394 RVA: 0x0026D9C1 File Offset: 0x0026BBC1
	public void CustomizeEmptyRow(string txt, Sprite icon)
	{
		this.emptyRowLabel = txt;
		this.emptyRowSprite = icon;
	}

	// Token: 0x0600671B RID: 26395 RVA: 0x0026D9D1 File Offset: 0x0026BBD1
	public void OnClick()
	{
		if (!this.open)
		{
			this.Open();
			return;
		}
		this.Close();
	}

	// Token: 0x0600671C RID: 26396 RVA: 0x0026D9E8 File Offset: 0x0026BBE8
	public void ChangeContent(IEnumerable<IListableOption> contentKeys)
	{
		this.entries.Clear();
		foreach (IListableOption item in contentKeys)
		{
			this.entries.Add(item);
		}
		this.built = false;
	}

	// Token: 0x0600671D RID: 26397 RVA: 0x0026DA48 File Offset: 0x0026BC48
	private void Update()
	{
		if (!this.open)
		{
			return;
		}
		if (!Input.GetMouseButtonDown(0) && Input.GetAxis("Mouse ScrollWheel") == 0f && !KInputManager.steamInputInterpreter.GetSteamInputActionIsDown(global::Action.MouseLeft))
		{
			return;
		}
		float canvasScale = this.canvasScaler.GetCanvasScale();
		if (this.scrollRect.rectTransform().GetPosition().x + this.scrollRect.rectTransform().sizeDelta.x * canvasScale < KInputManager.GetMousePos().x || this.scrollRect.rectTransform().GetPosition().x > KInputManager.GetMousePos().x || this.scrollRect.rectTransform().GetPosition().y - this.scrollRect.rectTransform().sizeDelta.y * canvasScale > KInputManager.GetMousePos().y || this.scrollRect.rectTransform().GetPosition().y < KInputManager.GetMousePos().y)
		{
			this.Close();
		}
	}

	// Token: 0x0600671E RID: 26398 RVA: 0x0026DB4C File Offset: 0x0026BD4C
	private void Build(List<IListableOption> contentKeys)
	{
		this.built = true;
		for (int i = this.contentContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.contentContainer.GetChild(i));
		}
		this.rowLookup.Clear();
		if (this.addEmptyRow)
		{
			this.emptyRow = Util.KInstantiateUI(this.rowEntryPrefab, this.contentContainer.gameObject, true);
			this.emptyRow.GetComponent<KButton>().onClick += delegate()
			{
				this.onEntrySelectedAction(null, this.targetData);
				if (this.displaySelectedValueWhenClosed)
				{
					this.selectedLabel.text = (this.emptyRowLabel ?? UI.DROPDOWN.NONE);
				}
				this.Close();
			};
			string text = this.emptyRowLabel ?? UI.DROPDOWN.NONE;
			this.emptyRow.GetComponent<DropDownEntry>().label.text = text;
			if (this.emptyRowSprite != null)
			{
				this.emptyRow.GetComponent<DropDownEntry>().image.sprite = this.emptyRowSprite;
			}
		}
		for (int j = 0; j < contentKeys.Count; j++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.rowEntryPrefab, this.contentContainer.gameObject, true);
			IListableOption id = contentKeys[j];
			gameObject.GetComponent<DropDownEntry>().entryData = id;
			gameObject.GetComponent<KButton>().onClick += delegate()
			{
				this.onEntrySelectedAction(id, this.targetData);
				if (this.displaySelectedValueWhenClosed)
				{
					this.selectedLabel.text = id.GetProperName();
				}
				this.Close();
			};
			this.rowLookup.Add(id, gameObject);
		}
		this.RefreshEntries();
		this.Close();
		this.scrollRect.gameObject.transform.SetParent(this.targetDropDownContainer.transform);
		this.scrollRect.gameObject.SetActive(false);
	}

	// Token: 0x0600671F RID: 26399 RVA: 0x0026DCEC File Offset: 0x0026BEEC
	private void RefreshEntries()
	{
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
		{
			DropDownEntry component = keyValuePair.Value.GetComponent<DropDownEntry>();
			component.label.text = keyValuePair.Key.GetProperName();
			if (component.portrait != null && keyValuePair.Key is IAssignableIdentity)
			{
				component.portrait.SetIdentityObject(keyValuePair.Key as IAssignableIdentity, true);
			}
		}
		if (this.sortFunction != null)
		{
			this.entries.Sort((IListableOption a, IListableOption b) => this.sortFunction(a, b, this.targetData));
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.rowLookup[this.entries[i]].transform.SetAsFirstSibling();
			}
			if (this.emptyRow != null)
			{
				this.emptyRow.transform.SetAsFirstSibling();
			}
		}
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair2 in this.rowLookup)
		{
			DropDownEntry component2 = keyValuePair2.Value.GetComponent<DropDownEntry>();
			this.rowRefreshAction(component2, this.targetData);
		}
		if (this.emptyRow != null)
		{
			this.rowRefreshAction(this.emptyRow.GetComponent<DropDownEntry>(), this.targetData);
		}
	}

	// Token: 0x06006720 RID: 26400 RVA: 0x0026DE90 File Offset: 0x0026C090
	protected override void OnCleanUp()
	{
		Util.KDestroyGameObject(this.scrollRect);
		base.OnCleanUp();
	}

	// Token: 0x06006721 RID: 26401 RVA: 0x0026DEA4 File Offset: 0x0026C0A4
	public void Open()
	{
		if (this.open)
		{
			return;
		}
		if (!this.built)
		{
			this.Build(this.entries);
		}
		else
		{
			this.RefreshEntries();
		}
		this.open = true;
		this.scrollRect.gameObject.SetActive(true);
		this.scrollRect.rectTransform().localScale = Vector3.one;
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
		{
			keyValuePair.Value.SetActive(true);
		}
		float num = Mathf.Max(32f, this.rowEntryPrefab.GetComponent<LayoutElement>().preferredHeight);
		this.scrollRect.rectTransform().sizeDelta = new Vector2(this.scrollRect.rectTransform().sizeDelta.x, num * (float)Mathf.Min(this.contentContainer.childCount, 8));
		Vector3 vector = this.dropdownAlignmentTarget.TransformPoint(this.dropdownAlignmentTarget.rect.x, this.dropdownAlignmentTarget.rect.y, 0f);
		Vector2 v = new Vector2(Mathf.Min(0f, (float)Screen.width - (vector.x + (this.rowEntryPrefab.GetComponent<LayoutElement>().minWidth * this.canvasScaler.GetCanvasScale() + DropDown.edgePadding.x))), -Mathf.Min(0f, vector.y - (this.scrollRect.rectTransform().sizeDelta.y * this.canvasScaler.GetCanvasScale() + DropDown.edgePadding.y)));
		vector += v;
		this.scrollRect.rectTransform().SetPosition(vector);
	}

	// Token: 0x06006722 RID: 26402 RVA: 0x0026E088 File Offset: 0x0026C288
	public void Close()
	{
		if (!this.open)
		{
			return;
		}
		this.open = false;
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
		{
			keyValuePair.Value.SetActive(false);
		}
		this.scrollRect.SetActive(false);
	}

	// Token: 0x04004688 RID: 18056
	public GameObject targetDropDownContainer;

	// Token: 0x04004689 RID: 18057
	public LocText selectedLabel;

	// Token: 0x0400468B RID: 18059
	public KButton openButton;

	// Token: 0x0400468C RID: 18060
	public Transform contentContainer;

	// Token: 0x0400468D RID: 18061
	public GameObject scrollRect;

	// Token: 0x0400468E RID: 18062
	public RectTransform dropdownAlignmentTarget;

	// Token: 0x0400468F RID: 18063
	public GameObject rowEntryPrefab;

	// Token: 0x04004690 RID: 18064
	public bool addEmptyRow = true;

	// Token: 0x04004691 RID: 18065
	private static Vector2 edgePadding = new Vector2(8f, 8f);

	// Token: 0x04004692 RID: 18066
	public object targetData;

	// Token: 0x04004693 RID: 18067
	private List<IListableOption> entries = new List<IListableOption>();

	// Token: 0x04004694 RID: 18068
	private Action<IListableOption, object> onEntrySelectedAction;

	// Token: 0x04004695 RID: 18069
	private Action<DropDownEntry, object> rowRefreshAction;

	// Token: 0x04004696 RID: 18070
	public Dictionary<IListableOption, GameObject> rowLookup = new Dictionary<IListableOption, GameObject>();

	// Token: 0x04004697 RID: 18071
	private Func<IListableOption, IListableOption, object, int> sortFunction;

	// Token: 0x04004698 RID: 18072
	private GameObject emptyRow;

	// Token: 0x04004699 RID: 18073
	private string emptyRowLabel;

	// Token: 0x0400469A RID: 18074
	private Sprite emptyRowSprite;

	// Token: 0x0400469B RID: 18075
	private bool built;

	// Token: 0x0400469C RID: 18076
	private bool displaySelectedValueWhenClosed = true;

	// Token: 0x0400469D RID: 18077
	private const int ROWS_BEFORE_SCROLL = 8;

	// Token: 0x0400469E RID: 18078
	private KCanvasScaler canvasScaler;
}
