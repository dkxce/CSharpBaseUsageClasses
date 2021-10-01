using System;
using System.Collections.Generic;

namespace Utils
{
    unsafe public class MyUBitConverter
    {
        public bool isLittleEndian = true;

        public byte[] ToBytes(char val)
        {
            return new byte[] { (byte)val };
        }

        public char[] ToChar(byte[] array)
        {
            List<char> result = new List<char>();
            for (int i = 0; i < array.Length; i++) result.Add((char)array[i]);
            return result.ToArray();
        }

        public byte[] ToBytes(char[] array)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < array.Length; i++) result.Add((byte)array[i]);
            return result.ToArray();
        }

        unsafe public short ToInt16(byte[] array, int startIndex)
        {
            int size = 2;
            byte* ra = stackalloc byte[size];
            for (int i = 0; i < size; i++) ra[isLittleEndian ? i : size - 1 - i] = array[i + startIndex];
            return *(short*)ra;
        }

        unsafe public byte[] ToBytes(short val)
        {
            return ToBytes(&val, 2);
        }

        unsafe public short[] ToInt16(byte[] array, int startIndex, int count)
        {
            int size = 2;
            byte* b = stackalloc byte[size];
            List<short> res = new List<short>();
            for (int i = 0; i < count; i += size)
            {
                for (int j = 0; j < size; j++) b[isLittleEndian ? j : size - 1 - j] = array[i + startIndex + j];
                res.Add(*((short*)b));
            };
            return res.ToArray();
        }

        unsafe public byte[] ToBytes(short[] array)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < array.Length; i++)
            {
                short i_val = array[i];
                result.AddRange(ToBytes(&i_val, 2));
            };
            return result.ToArray();
        }

        unsafe public ushort ToUInt16(byte[] array, int startIndex)
        {
            int size = 2;
            byte* ra = stackalloc byte[size];
            for (int i = 0; i < size; i++) ra[isLittleEndian ? i : size - 1 - i] = array[i + startIndex];
            return *(ushort*)ra;
        }

        unsafe public byte[] ToBytes(ushort val)
        {
            return ToBytes(&val, 2);
        }

        unsafe public ushort[] ToUInt16(byte[] array, int startIndex, int count)
        {
            int size = 2;
            byte* b = stackalloc byte[size];
            List<ushort> res = new List<ushort>();
            for (int i = 0; i < count; i += size)
            {
                for (int j = 0; j < size; j++) b[isLittleEndian ? j : size - 1 - j] = array[i + startIndex + j];
                res.Add(*((ushort*)b));
            };
            return res.ToArray();
        }

        unsafe public byte[] ToBytes(ushort[] array)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < array.Length; i++)
            {
                ushort i_val = array[i];
                result.AddRange(ToBytes(&i_val, 2));
            };
            return result.ToArray();
        }

        unsafe public int ToInt32(byte[] array, int startIndex)
        {
            int size = 4;
            byte* ra = stackalloc byte[size];
            for (int i = 0; i < size; i++) ra[isLittleEndian ? i : size - 1 - i] = array[i + startIndex];
            return *(int*)ra;
        }

        unsafe public byte[] ToBytes(int val)
        {
            return ToBytes(&val, 4);
        }

        unsafe public int[] ToInt32(byte[] array, int startIndex, int count)
        {
            int size = 4;
            byte* b = stackalloc byte[size];
            List<int> res = new List<int>();
            for (int i = 0; i < count; i += size)
            {
                for (int j = 0; j < size; j++) b[isLittleEndian ? j : size - 1 - j] = array[i + startIndex + j];
                res.Add(*((int*)b));
            };
            return res.ToArray();
        }

        unsafe public byte[] ToBytes(int[] array)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < array.Length; i++)
            {
                int i_val = array[i];
                result.AddRange(ToBytes(&i_val, 4));
            };
            return result.ToArray();
        }

        unsafe public uint ToUInt32(byte[] array, int startIndex)
        {
            int size = 4;
            byte* ra = stackalloc byte[size];
            for (int i = 0; i < size; i++) ra[isLittleEndian ? i : size - 1 - i] = array[i + startIndex];
            return *(uint*)ra;
        }

        unsafe public byte[] ToBytes(uint val)
        {
            return ToBytes(&val, 4);
        }

        unsafe public uint[] ToUInt32(byte[] array, int startIndex, int count)
        {
            int size = 4;
            byte* b = stackalloc byte[size];
            List<uint> res = new List<uint>();
            for (int i = 0; i < count; i += size)
            {
                for (int j = 0; j < size; j++) b[isLittleEndian ? j : size - 1 - j] = array[i + startIndex + j];
                res.Add(*((uint*)b));
            };
            return res.ToArray();
        }

        unsafe public byte[] ToBytes(uint[] array)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < array.Length; i++)
            {
                uint i_val = array[i];
                result.AddRange(ToBytes(&i_val, 4));
            };
            return result.ToArray();
        }

        unsafe public long ToInt64(byte[] array, int startIndex)
        {
            int size = 8;
            byte* ra = stackalloc byte[size];
            for (int i = 0; i < size; i++) ra[isLittleEndian ? i : size - 1 - i] = array[i + startIndex];
            return *(long*)ra;
        }

        unsafe public byte[] ToBytes(long val)
        {
            return ToBytes(&val, 8);
        }

        unsafe public long[] ToInt64(byte[] array, int startIndex, int count)
        {
            int size = 8;
            byte* b = stackalloc byte[size];
            List<long> res = new List<long>();
            for (int i = 0; i < count; i += size)
            {
                for (int j = 0; j < size; j++) b[isLittleEndian ? j : size - 1 - j] = array[i + startIndex + j];
                res.Add(*((long*)b));
            };
            return res.ToArray();
        }

        unsafe public byte[] ToBytes(long[] array)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < array.Length; i++)
            {
                long i_val = array[i];
                result.AddRange(ToBytes(&i_val, 8));
            };
            return result.ToArray();
        }

        unsafe public ulong ToUInt64(byte[] array, int startIndex)
        {
            int size = 8;
            byte* ra = stackalloc byte[size];
            for (int i = 0; i < size; i++) ra[isLittleEndian ? i : size - 1 - i] = array[i + startIndex];
            return *(ulong*)ra;
        }

        unsafe public byte[] ToBytes(ulong val)
        {
            return ToBytes(&val, 8);
        }

        unsafe public ulong[] ToUInt64(byte[] array, int startIndex, int count)
        {
            int size = 8;
            byte* b = stackalloc byte[size];
            List<ulong> res = new List<ulong>();
            for (int i = 0; i < count; i += size)
            {
                for (int j = 0; j < size; j++) b[isLittleEndian ? j : size - 1 - j] = array[i + startIndex + j];
                res.Add(*((ulong*)b));
            };
            return res.ToArray();
        }

        unsafe public byte[] ToBytes(ulong[] array)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < array.Length; i++)
            {
                ulong i_val = array[i];
                result.AddRange(ToBytes(&i_val, 8));
            };
            return result.ToArray();
        }

        unsafe public float ToFloat(byte[] array, int startIndex)
        {
            int size = 4;
            byte* ra = stackalloc byte[size];
            for (int i = 0; i < size; i++) ra[isLittleEndian ? i : size - 1 - i] = array[i + startIndex];
            return *(float*)ra;
        }

        unsafe public byte[] ToBytes(float val)
        {
            return ToBytes(&val, 4);
        }

        unsafe public float[] ToFloat(byte[] array, int startIndex, int count)
        {
            int size = 4;
            byte* b = stackalloc byte[size];
            List<float> res = new List<float>();
            for (int i = 0; i < count; i += size)
            {
                for (int j = 0; j < size; j++) b[isLittleEndian ? j : size - 1 - j] = array[i + startIndex + j];
                res.Add(*((float*)b));
            };
            return res.ToArray();
        }

        unsafe public byte[] ToBytes(float[] array)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < array.Length; i++)
            {
                float i_val = array[i];
                result.AddRange(ToBytes(&i_val, 4));
            };
            return result.ToArray();
        }

        unsafe public double ToDouble(byte[] array, int startIndex)
        {
            int size = 8;
            byte* ra = stackalloc byte[size];
            for (int i = 0; i < size; i++) ra[isLittleEndian ? i : size - 1 - i] = array[i + startIndex];
            return *(double*)ra;
        }

        unsafe public byte[] ToBytes(double val)
        {
            return ToBytes(&val, 8);
        }

        unsafe public double[] ToDouble(byte[] array, int startIndex, int count)
        {
            int size = 8;
            byte* b = stackalloc byte[size];
            List<double> res = new List<double>();
            for (int i = 0; i < count; i += size)
            {
                for (int j = 0; j < size; j++) b[isLittleEndian ? j : size - 1 - j] = array[i + startIndex + j];
                res.Add(*((double*)b));
            };
            return res.ToArray();
        }

        unsafe public byte[] ToBytes(double[] array)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < array.Length; i++)
            {
                double i_val = array[i];
                result.AddRange(ToBytes(&i_val, 8));
            };
            return result.ToArray();
        }

        unsafe public DateTime ToDateTime(byte[] array, int startIndex)
        {
            int size = 8;
            byte* ra = stackalloc byte[size];
            for (int i = 0; i < size; i++) ra[isLittleEndian ? i : size - 1 - i] = array[i + startIndex];
            return DateTime.FromOADate(*(double*)ra);
        }

        unsafe public byte[] ToBytes(DateTime val)
        {
            double dt = val.ToOADate();
            return ToBytes(&val, 8);
        }

        unsafe public DateTime[] ToDateTime(byte[] array, int startIndex, int count)
        {
            int size = 8;
            byte* b = stackalloc byte[size];
            List<DateTime> res = new List<DateTime>();
            for (int i = 0; i < count; i += size)
            {
                for (int j = 0; j < size; j++) b[isLittleEndian ? j : size - 1 - j] = array[i + startIndex + j];
                res.Add(DateTime.FromOADate(*((double*)b)));
            };
            return res.ToArray();
        }

        unsafe public byte[] ToBytes(DateTime[] array)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < array.Length; i++)
            {
                double i_val = array[i].ToOADate();
                result.AddRange(ToBytes(&i_val, 8));
            };
            return result.ToArray();
        }

        unsafe private byte[] ToBytes(void* ptr, int length)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < length; i++)
                result.Add(*((byte*)ptr + i));
            if (isLittleEndian == false) result.Reverse();
            return result.ToArray();
        }

        unsafe public static byte[] ToBytes(void* ptr, int length, bool reverse)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < length; i++)
                result.Add(*((byte*)ptr + i));
            if (reverse) result.Reverse();
            return result.ToArray();
        }

        public static void Test()
        {
            int[] ex_a = new int[] { (int)1, (int)2 };
            double[] ex_b = new double[] { 123.45, 67.89 };

            bool def_isle = BitConverter.IsLittleEndian;
            UnsafeBitConverter lebc = new UnsafeBitConverter();
            lebc.isLittleEndian = false;
            UnsafeBitConverter bebc = new UnsafeBitConverter();
            bebc.isLittleEndian = true;

            byte[] def_a = BitConverter.GetBytes(ex_a[0]);
            byte[] le_ex_a = lebc.ToBytes(ex_a);
            byte[] be_ex_a = bebc.ToBytes(ex_a);
            int[] le_ex_ar = lebc.ToInt32(le_ex_a, 0, le_ex_a.Length);
            int[] be_ex_ar = bebc.ToInt32(be_ex_a, 0, be_ex_a.Length);
            int le_ex_as = lebc.ToUInt16(le_ex_a, 2);
            int be_ex_as = bebc.ToUInt16(be_ex_a, 0);

            byte[] def_b = BitConverter.GetBytes(ex_b[0]);
            byte[] le_ex_b = lebc.ToBytes(ex_b);
            byte[] be_ex_b = bebc.ToBytes(ex_b);
            double[] le_ex_br = lebc.ToDouble(le_ex_b, 0, le_ex_b.Length);
            double[] be_ex_br = bebc.ToDouble(be_ex_b, 0, be_ex_b.Length);
            double le_ex_bs = lebc.ToUInt16(le_ex_b, 0);
            double be_ex_bs = bebc.ToUInt16(be_ex_b, 0);
        }

        public static void PointerSamples()
        {
            unsafe
            {
                byte* bytes = stackalloc byte[4];
                byte* first = bytes;
                //*(first++) = 200; *(first++) = 0; *(first++) = 0; *(first) = 0;
                bytes[0] = 200; bytes[1] = 0; bytes[2] = 0; bytes[3] = 0;

                void* v_any = bytes; // указатель на неизвестный тип.
                void* v_last = first + 3;
                char* v_cptr = (char*)bytes;
                char v_char = *(char*)bytes;
                byte v_byte = *(byte*)bytes;
                sbyte v_sbyte = *(sbyte*)bytes;
                ushort v_ushort = *(ushort*)bytes;
                short v_short = *(short*)bytes;
                uint v_uint = *(uint*)bytes;
                int v_int = *(int*)bytes;

                ////////////////

                int i_val = 0x04030201;
                int* i_ptr = &i_val;
                byte i_b_1 = *((byte*)i_ptr);
                byte i_b_2 = *((byte*)i_ptr + 1);
                byte i_b_3 = *((byte*)i_ptr + 2);
                byte i_b_4 = *((byte*)i_ptr + 3);

                ////////////////

                ushort* word = stackalloc ushort[2];
                *(word) = 0x0605; *(word + 1) = 0x0807;
                //word[0] = 0x0605; word[1] = 0x0807;
                uint* wori = (uint*)word; // 0x05060708
                byte w_b_5 = *((byte*)word);
                byte w_b_6 = *((byte*)word + 1);
                byte w_b_7 = *((byte*)word + 2);
                byte w_b_8 = *((byte*)word + 3);

                /////////////////

                char* str_ptr = stackalloc char[4];
                str_ptr[0] = 'D'; str_ptr[1] = 'E'; str_ptr[2] = 'M'; str_ptr[3] = 'O'; // DEMO
                string str_val = new string(str_ptr, 0, 4);
                uint str_int = *((uint*)str_ptr); // DEMO as uint
                byte* str_arr = (byte*)str_ptr; // DEMO as byte[]
                byte str_M = *(str_arr + 2); // M as byte

                char[] ns_val = "WORK".ToCharArray(); // W O R K
                fixed (char* ns_ptr = &ns_val[0]) // new char[] { W, O, R, K }
                {
                    char ns_0 = *(ns_ptr); // W
                    char ns_1 = *(ns_ptr + 1); // O
                    char ns_2 = *(ns_ptr + 2); // R
                    char ns_3 = *(ns_ptr + 3); // K
                    uint ns_i = *((uint*)ns_ptr); // WORK as uint
                };

                int int_val = 0;
                int* int_ptr = &int_val;
                int_ptr[0] = 5; // 1..4 byte
                byte* int_bpt = ((byte*)int_ptr) + 1;  // 2 byte
                *int_bpt = 1; // 2 byte only
                *(((byte*)int_ptr) + 2) = 0xFF; // 3 byte only
            };
        }
    }

}
