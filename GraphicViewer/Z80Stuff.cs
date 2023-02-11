using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace GraphicViewer
{
    public class Z80FileLoader
    {
        public Z80File Load(string z80File)
        {
            byte compressedBit = 1 << 5;

            using (Stream fs = File.OpenRead(z80File))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    Z80Header header = ByteToType<Z80Header>(reader);
                    Z80HeaderV2 headerV2 = new Z80HeaderV2();
                    Z80HeaderV3 headerV3 = new Z80HeaderV3();
                    if (header.PC == 0)
                    {
                        ushort additionalHeaderSize = reader.ReadUInt16();
                        if (additionalHeaderSize == 23)
                        {
                            // version 2
                            headerV2 = ByteToType<Z80HeaderV2>(reader);
                        }
                        else
                        {
                            // version 3
                            headerV2 = ByteToType<Z80HeaderV2>(reader);
                            //byte[] throwaway = reader.ReadBytes(32); // Ignore the rest...
                            headerV3 = ByteToType<Z80HeaderV3>(reader);
                        }
                    }

                    bool compressed = ((header.Flags & compressedBit) == compressedBit) || header.PC == 0;
                    byte[] data = reader.ReadBytes((int)(fs.Length - fs.Position));
                    byte[] memory = Decompress(data, compressed); // headerV2.PC == 0 ? Decompress(data, compressed) : new byte[] { 0 };

                    return new Z80File(header, headerV2, memory);
                }
            }
        }

        private T ByteToType<T>(BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));

            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return theStructure;
        }

        /// <summary>
        /// Decompress the data in the .z80 file.
        /// </summary>
        private byte[] Decompress(byte[] m_buffer, bool m_isCompressed)
        {
            //byte[] m_memory = new byte[65535];
            List<byte> m_memory = new List<byte>();
            int m_memoryDataBlockStart = 0;
            int offset = 0; // Current offset into memory
            // TODO: It's 30 just now, but if this is v.2/3 of the file then it's not.. See docs.
            for (int i = m_memoryDataBlockStart; i < m_buffer.Length; i++)
            {
                if (m_buffer[i] == 0x00 && m_buffer[i + 1] == 0xED && m_buffer[i + 2] == 0xED && m_buffer[i + 3] == 0x00)
                {
                    break;
                }

                if (i < m_buffer.Length - 4)
                {
                    if (m_buffer[i] == 0xED && m_buffer[i + 1] == 0xED && m_isCompressed)
                    {
                        i += 2;
                        int repeat = m_buffer[i++];
                        byte value = m_buffer[i];
                        for (int j = 0; j < repeat; j++)
                        {
                            m_memory.Add(m_buffer[i]);
                        }
                    }
                    else
                    {
                        m_memory.Add(m_buffer[i]);
                    }
                }
                else
                {
                    m_memory.Add(m_buffer[i]);
                }
            }

            return m_memory.ToArray();
        }
    }

    public class Z80File
    {
        public Z80Header Header { get; private set; }

        public Z80HeaderV2 HeaderV2 { get; private set; }

        public byte[] Memory { get; private set; }

        internal Z80File(Z80Header header, Z80HeaderV2 v2Header, byte[] memory)
        {
            Header = header;
            HeaderV2 = v2Header;
            Memory = memory;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct Z80Header
    {
        [FieldOffset(0)]
        public byte A;
        [FieldOffset(1)]
        public byte F;
        [FieldOffset(2)]
        public ushort BC;
        [FieldOffset(4)]
        public ushort HL;
        [FieldOffset(6)]
        public ushort PC;
        [FieldOffset(8)]
        public ushort SP;
        [FieldOffset(10)]
        public byte IR;
        [FieldOffset(11)]
        public byte RefreshRegister;
        [FieldOffset(12)]
        public byte Flags;
        [FieldOffset(13)]
        public ushort DE;
        [FieldOffset(15)]
        public ushort BCAlt;
        [FieldOffset(17)]
        public ushort DEAlt;
        [FieldOffset(19)]
        public ushort HLAlt;
        [FieldOffset(21)]
        public byte AAlt;
        [FieldOffset(22)]
        public byte FAlt;
        [FieldOffset(23)]
        public ushort IY;
        [FieldOffset(25)]
        public ushort IX;
        [FieldOffset(27)]
        public byte IFF;
        [FieldOffset(28)]
        public byte IFF2;
        [FieldOffset(29)]
        public byte MoreFlags;
    }

    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct Z80HeaderV3
    {
        [FieldOffset(0)]
        public ushort LowTStateCounter;
        [FieldOffset(2)]
        public byte HighTStateCounter;
        [FieldOffset(3)]
        public byte SpectatorFlag;
        [FieldOffset(4)]
        public byte MGTRom;
        [FieldOffset(5)]
        public byte MultifaceRom;
        [FieldOffset(6)]
        public byte RomOrRam1;
        [FieldOffset(7)]
        public byte RomOrRam2;
        [FieldOffset(8)]
        public byte Key1; // 16
        [FieldOffset(9)]
        public byte Key2; // 16
        [FieldOffset(10)]
        public byte Key3; // 16
        [FieldOffset(11)]
        public byte Key4; // 16
        [FieldOffset(12)]
        public byte Key5; // 16
        [FieldOffset(13)]
        public byte Key6; // 16
        [FieldOffset(14)]
        public byte Key7; // 16
        [FieldOffset(15)]
        public byte Key8; // 16
        [FieldOffset(16)]
        public byte Key9; // 16
        [FieldOffset(17)]
        public byte Key10; // 16


        [FieldOffset(18)]
        public byte AKey1; // 16
        [FieldOffset(19)]
        public byte AKey2; // 16
        [FieldOffset(20)]
        public byte AKey3; // 16
        [FieldOffset(21)]
        public byte AKey4; // 16
        [FieldOffset(22)]
        public byte AKey5; // 16
        [FieldOffset(23)]
        public byte AKey6; // 16
        [FieldOffset(24)]
        public byte AKey7; // 16
        [FieldOffset(25)]
        public byte AKey8; // 16
        [FieldOffset(26)]
        public byte AKey9; // 16
        [FieldOffset(27)]
        public byte AKey10; // 16
        [FieldOffset(28)]
        public byte MGTType;
        [FieldOffset(29)]
        public byte DiscipleButton;
        [FieldOffset(30)]
        public byte DiscipleFlag;
        [FieldOffset(31)]   
        public byte LastOUT;
    }

    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct Z80HeaderV2
    {
        [FieldOffset(0)]
        public ushort PC;
        [FieldOffset(2)]
        public byte HardwareMode;
        [FieldOffset(3)]
        public byte Flag1;
        [FieldOffset(4)]
        public byte IIRomPaged;
        [FieldOffset(5)]
        public byte Flag2;
        [FieldOffset(6)]
        public byte SoundOut;
        [FieldOffset(7)]
        public byte SoundChip1; // 16
        [FieldOffset(8)]
        public byte SoundChip2; // 16
        [FieldOffset(9)]
        public byte SoundChip3; // 16
        [FieldOffset(10)]
        public byte SoundChip4; // 16
        [FieldOffset(11)]
        public byte SoundChip5; // 16
        [FieldOffset(12)]
        public byte SoundChip6; // 16
        [FieldOffset(13)]
        public byte SoundChip7; // 16
        [FieldOffset(14)]
        public byte SoundChip8; // 16
        [FieldOffset(15)]
        public byte SoundChip9; // 16
        [FieldOffset(16)]
        public byte SoundChip10; // 16
        [FieldOffset(17)]
        public byte SoundChip11; // 16
        [FieldOffset(18)]
        public byte SoundChip12; // 16
        [FieldOffset(19)]
        public byte SoundChip13; // 16
        [FieldOffset(20)]
        public byte SoundChip14; // 16
        [FieldOffset(21)]
        public byte SoundChip15; // 16
        [FieldOffset(22)]
        public byte SoundChip16; // 16
    }
}
