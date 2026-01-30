using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x02000D9C RID: 3484
[SerializationConfig(MemberSerialization.OptIn)]
public class SerializedList<ItemType>
{
	// Token: 0x170007B3 RID: 1971
	// (get) Token: 0x06006C7E RID: 27774 RVA: 0x002917DA File Offset: 0x0028F9DA
	public int Count
	{
		get
		{
			return this.items.Count;
		}
	}

	// Token: 0x06006C7F RID: 27775 RVA: 0x002917E7 File Offset: 0x0028F9E7
	public IEnumerator<ItemType> GetEnumerator()
	{
		return this.items.GetEnumerator();
	}

	// Token: 0x170007B4 RID: 1972
	public ItemType this[int idx]
	{
		get
		{
			return this.items[idx];
		}
	}

	// Token: 0x06006C81 RID: 27777 RVA: 0x00291807 File Offset: 0x0028FA07
	public void Add(ItemType item)
	{
		this.items.Add(item);
	}

	// Token: 0x06006C82 RID: 27778 RVA: 0x00291815 File Offset: 0x0028FA15
	public void Remove(ItemType item)
	{
		this.items.Remove(item);
	}

	// Token: 0x06006C83 RID: 27779 RVA: 0x00291824 File Offset: 0x0028FA24
	public void RemoveAt(int idx)
	{
		this.items.RemoveAt(idx);
	}

	// Token: 0x06006C84 RID: 27780 RVA: 0x00291832 File Offset: 0x0028FA32
	public bool Contains(ItemType item)
	{
		return this.items.Contains(item);
	}

	// Token: 0x06006C85 RID: 27781 RVA: 0x00291840 File Offset: 0x0028FA40
	public void Clear()
	{
		this.items.Clear();
	}

	// Token: 0x06006C86 RID: 27782 RVA: 0x00291850 File Offset: 0x0028FA50
	[OnSerializing]
	private void OnSerializing()
	{
		MemoryStream memoryStream = new MemoryStream();
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(this.items.Count);
		foreach (ItemType itemType in this.items)
		{
			binaryWriter.WriteKleiString(itemType.GetType().FullName);
			long position = binaryWriter.BaseStream.Position;
			binaryWriter.Write(0);
			long position2 = binaryWriter.BaseStream.Position;
			Serializer.SerializeTypeless(itemType, binaryWriter);
			long position3 = binaryWriter.BaseStream.Position;
			long num = position3 - position2;
			binaryWriter.BaseStream.Position = position;
			binaryWriter.Write((int)num);
			binaryWriter.BaseStream.Position = position3;
		}
		memoryStream.Flush();
		this.serializationBuffer = memoryStream.ToArray();
	}

	// Token: 0x06006C87 RID: 27783 RVA: 0x00291950 File Offset: 0x0028FB50
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.serializationBuffer == null)
		{
			return;
		}
		FastReader fastReader = new FastReader(this.serializationBuffer);
		int num = fastReader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			string text = fastReader.ReadKleiString();
			int num2 = fastReader.ReadInt32();
			int position = fastReader.Position;
			Type type = Type.GetType(text);
			if (type == null)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Type no longer exists: " + text
				});
				fastReader.SkipBytes(num2);
			}
			else
			{
				ItemType itemType;
				if (typeof(ItemType) != type)
				{
					itemType = (ItemType)((object)Activator.CreateInstance(type));
				}
				else
				{
					itemType = default(ItemType);
				}
				Deserializer.DeserializeTypeless(itemType, fastReader);
				if (fastReader.Position != position + num2)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Expected to be at offset",
						position + num2,
						"but was only at offset",
						fastReader.Position,
						". Skipping to catch up."
					});
					fastReader.SkipBytes(position + num2 - fastReader.Position);
				}
				this.items.Add(itemType);
			}
		}
	}

	// Token: 0x04004A46 RID: 19014
	[Serialize]
	private byte[] serializationBuffer;

	// Token: 0x04004A47 RID: 19015
	private List<ItemType> items = new List<ItemType>();
}
