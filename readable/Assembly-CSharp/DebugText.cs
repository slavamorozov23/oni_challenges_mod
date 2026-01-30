using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008CD RID: 2253
[AddComponentMenu("KMonoBehaviour/scripts/DebugText")]
public class DebugText : KMonoBehaviour
{
	// Token: 0x06003E71 RID: 15985 RVA: 0x0015D962 File Offset: 0x0015BB62
	public static void DestroyInstance()
	{
		DebugText.Instance = null;
	}

	// Token: 0x06003E72 RID: 15986 RVA: 0x0015D96A File Offset: 0x0015BB6A
	protected override void OnPrefabInit()
	{
		DebugText.Instance = this;
	}

	// Token: 0x06003E73 RID: 15987 RVA: 0x0015D974 File Offset: 0x0015BB74
	public void Draw(string text, Vector3 pos, Color color)
	{
		DebugText.Entry item = new DebugText.Entry
		{
			text = text,
			pos = pos,
			color = color
		};
		this.entries.Add(item);
	}

	// Token: 0x06003E74 RID: 15988 RVA: 0x0015D9B0 File Offset: 0x0015BBB0
	private void LateUpdate()
	{
		foreach (Text text in this.texts)
		{
			UnityEngine.Object.Destroy(text.gameObject);
		}
		this.texts.Clear();
		foreach (DebugText.Entry entry in this.entries)
		{
			GameObject gameObject = new GameObject();
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.GetComponent<RectTransform>());
			gameObject.transform.SetPosition(entry.pos);
			rectTransform.localScale = new Vector3(0.02f, 0.02f, 1f);
			Text text2 = gameObject.AddComponent<Text>();
			text2.font = Assets.DebugFont;
			text2.text = entry.text;
			text2.color = entry.color;
			text2.horizontalOverflow = HorizontalWrapMode.Overflow;
			text2.verticalOverflow = VerticalWrapMode.Overflow;
			text2.alignment = TextAnchor.MiddleCenter;
			this.texts.Add(text2);
		}
		this.entries.Clear();
	}

	// Token: 0x04002684 RID: 9860
	public static DebugText Instance;

	// Token: 0x04002685 RID: 9861
	private List<DebugText.Entry> entries = new List<DebugText.Entry>();

	// Token: 0x04002686 RID: 9862
	private List<Text> texts = new List<Text>();

	// Token: 0x020018EB RID: 6379
	private struct Entry
	{
		// Token: 0x04007C70 RID: 31856
		public string text;

		// Token: 0x04007C71 RID: 31857
		public Vector3 pos;

		// Token: 0x04007C72 RID: 31858
		public Color color;
	}
}
