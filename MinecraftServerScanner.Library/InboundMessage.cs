using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MinecraftServerScanner.Library
{
    /// <summary>
    /// Read methods stolen from:
    /// https://wiki.vg/Protocol#VarInt_and_VarLong
    /// </summary>
    public class InboundMessage
    {
        public readonly Byte Id;
        public readonly Int32 Length;
        public Int32 Offset { get; set; }
        public Byte[] Buffer { get; private set; }

        public InboundMessage(NetworkStream stream)
        {
            var value = 0;
            var size = 0;
            int b;
            while (((b = stream.ReadByte()) & 0x80) == 0x80)
            {
                value |= (b & 0x7F) << (size++ * 7);
                if (size > 5)
                {
                    throw new IOException("This VarInt is an imposter!");
                }
            }
            this.Length = value | ((b & 0x7F) << (size * 7));

            this.Offset = 0;

            // Read the message into the internal buffer
            //this.Buffer = new Byte[this.Length];
            //stream.Read(this.Buffer, 0, this.Length);
            Int32 index = 0;
            List<Byte> _temp = new List<Byte>();

            while (_temp.Count < this.Length && index < this.Length * 2)
            {
                if (stream.DataAvailable)
                    _temp.Add((Byte)stream.ReadByte());
                else
                    Thread.Sleep(1);

                index++;
            }

            if(_temp.Count < this.Length)
            {
                throw new Exception("Not all data recieved!");
            }

            this.Buffer = _temp.ToArray();

            this.Id = this.ReadByte();
        }

        public byte ReadByte()
        {
            var b = this.Buffer[this.Offset];
            this.Offset += 1;
            return b;
        }

        public byte[] Read(int length)
        {
            if (this.Buffer.Length >= length)
            {
                var data = new byte[length];
                Array.Copy(this.Buffer, this.Offset, data, 0, length);
                this.Offset += length;
                return data;
            }

            throw new IOException("Buffer length too short!");
        }

        public int ReadVarInt()
        {
            var value = 0;
            var size = 0;
            int b;
            while (((b = ReadByte()) & 0x80) == 0x80)
            {
                value |= (b & 0x7F) << (size++ * 7);
                if (size > 5)
                {
                    throw new IOException("This VarInt is an imposter!");
                }
            }
            return value | ((b & 0x7F) << (size * 7));
        }

        public string ReadString(int length)
        {
            var data = Read(length);
            return Encoding.UTF8.GetString(data);
        }
    }
}
