using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MinecraftServerScanner.Library
{
    public class OutboundMessage
    {
        private List<Byte> _buffer;
        public OutboundMessage(Byte? prefix = null)
        {
            _buffer = new List<Byte>();

            // Add the prefix to the buffer
            if(prefix != null)
                _buffer.Add((Byte)prefix);
        }

        public void WriteVarInt(int value, List<Byte> tempBuffer = null)
        {
            var b = tempBuffer ?? _buffer;
            while ((value & 128) != 0)
            {
                b.Add((byte)(value & 127 | 128));
                value = (int)((uint)value) >> 7;
            }
            b.Add((byte)value);
        }

        public void WriteShort(short value, List<Byte> tempBuffer = null)
        {
            var b = tempBuffer ?? _buffer;
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void WriteString(string data, List<Byte> tempBuffer = null)
        {
            var b = tempBuffer ?? _buffer;
            var buffer = Encoding.UTF8.GetBytes(data);
            WriteVarInt(buffer.Length);
            b.AddRange(buffer);
        }

        public Byte[] GetData()
        {
            List<Byte> _output = new List<Byte>();
            this.WriteVarInt(_buffer.Count, _output);
            _output.AddRange(_buffer);

            return _output.ToArray();
        }
    }
}
