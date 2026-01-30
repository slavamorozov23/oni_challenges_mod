using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

// Token: 0x02000618 RID: 1560
public class OldNoteEntriesV5
{
	// Token: 0x060024BF RID: 9407 RVA: 0x000D3640 File Offset: 0x000D1840
	public void Deserialize(BinaryReader reader)
	{
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			OldNoteEntriesV5.NoteStorageBlock item = default(OldNoteEntriesV5.NoteStorageBlock);
			item.Deserialize(reader);
			this.storageBlocks.Add(item);
		}
	}

	// Token: 0x04001584 RID: 5508
	public List<OldNoteEntriesV5.NoteStorageBlock> storageBlocks = new List<OldNoteEntriesV5.NoteStorageBlock>();

	// Token: 0x020014E8 RID: 5352
	[StructLayout(LayoutKind.Explicit)]
	public struct NoteEntry
	{
		// Token: 0x04006FF3 RID: 28659
		[FieldOffset(0)]
		public int reportEntryId;

		// Token: 0x04006FF4 RID: 28660
		[FieldOffset(4)]
		public int noteHash;

		// Token: 0x04006FF5 RID: 28661
		[FieldOffset(8)]
		public float value;
	}

	// Token: 0x020014E9 RID: 5353
	[StructLayout(LayoutKind.Explicit)]
	public struct NoteEntryArray
	{
		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06009182 RID: 37250 RVA: 0x00371593 File Offset: 0x0036F793
		public int StructSizeInBytes
		{
			get
			{
				return Marshal.SizeOf(typeof(OldNoteEntriesV5.NoteEntry));
			}
		}

		// Token: 0x04006FF6 RID: 28662
		[FieldOffset(0)]
		public byte[] bytes;

		// Token: 0x04006FF7 RID: 28663
		[FieldOffset(0)]
		public OldNoteEntriesV5.NoteEntry[] structs;
	}

	// Token: 0x020014EA RID: 5354
	public struct NoteStorageBlock
	{
		// Token: 0x06009183 RID: 37251 RVA: 0x003715A4 File Offset: 0x0036F7A4
		public void Deserialize(BinaryReader reader)
		{
			this.entryCount = reader.ReadInt32();
			this.entries.bytes = reader.ReadBytes(this.entries.StructSizeInBytes * this.entryCount);
		}

		// Token: 0x04006FF8 RID: 28664
		public int entryCount;

		// Token: 0x04006FF9 RID: 28665
		public OldNoteEntriesV5.NoteEntryArray entries;
	}
}
