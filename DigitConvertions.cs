using System;
using System.Collections.Generic;
using System.Text;

namespace dkxce
{
    public static class DigitConvertions
    {
        public static uint to4(float input)
        {
            unsafe
            {
                uint res = 0;
                float* p_val = (float*)&res;
                *p_val = input;
                return res;
            };
        }

        public static void from4(uint input, out float output)
        {
            unsafe
            {
                float res = 0;
                uint* p_val = (uint*)&res;
                *p_val = input;
                output = res;
            };
        }

        public static uint to4(short x, short y)
        {
            unsafe
            {
                uint xy = 0;
                short* px = (short*)&xy;
                short* py = px + 1;

                *px = x;
                *py = y;

                return xy;
            };
        }

        public static void from4(uint input, out short x, out short y)
        {
            unsafe
            {
                uint xy = input;
                short* px = (short*)&xy;
                short* py = px + 1;
                x = *px;
                y = *py;
            }
        }

        public static uint to4(ushort x, ushort y)
        {
            unsafe
            {
                uint xy = 0;
                ushort* px = (ushort*)&xy;
                ushort* py = px + 1;

                *px = x;
                *py = y;

                return xy;
            };
        }

        public static void from4(uint input, out ushort x, out ushort y)
        {
            unsafe
            {
                uint xy = input;
                ushort* px = (ushort*)&xy;
                ushort* py = px + 1;
                x = *px;
                y = *py;
            }
        }

        public static uint to4(short x, ushort y)
        {
            unsafe
            {
                uint xy = 0;
                short* px = (short*)&xy;
                ushort* py = (ushort*)px + 1;

                *px = x;
                *py = y;

                return xy;
            };
        }

        public static void from4(uint input, out short x, out ushort y)
        {
            unsafe
            {
                uint xy = input;
                short* px = (short*)&xy;
                ushort* py = (ushort*)px + 1;
                x = *px;
                y = *py;
            }
        }

        public static uint to4(short x, byte y, byte z)
        {
            unsafe
            {
                uint xy = 0;
                short* px = (short*)&xy;
                byte* py = (byte*)px + 1;
                byte* pz = py + 1;

                *px = x;
                *py = y;
                *pz = z;

                return xy;
            };
        }

        public static void from4(uint input, out short x, out byte y, out byte z)
        {
            unsafe
            {
                uint xy = input;
                short* px = (short*)&xy;
                byte* py = (byte*)px + 1;
                byte* pz = py + 1;
                x = *px;
                y = *py;
                z = *pz;
            }
        }

        public static uint to4(ushort x, byte y, byte z)
        {
            unsafe
            {
                uint xy = 0;
                ushort* px = (ushort*)&xy;
                byte* py = (byte*)px + 1;
                byte* pz = py + 1;

                *px = x;
                *py = y;
                *pz = z;

                return xy;
            };
        }

        public static void from4(uint input, out ushort x, out byte y, out byte z)
        {
            unsafe
            {
                uint xy = input;
                ushort* px = (ushort*)&xy;
                byte* py = (byte*)px + 1;
                byte* pz = py + 1;
                x = *px;
                y = *py;
                z = *pz;
            }
        }

        public static uint to4(byte[] input)
        {
            unsafe
            {
                uint val = 0;
                byte* pval = (byte*)&val;

                for (int i = 0; i < 4 && i < input.Length; i++)
                    *pval++ = input[i];

                return val;
            };
        }

        public static void from4(uint input, out byte[] output)
        {
            unsafe
            {
                uint v = input;
                byte* pv = (byte*)&v;

                byte[] res = new byte[4];
                for (int i = 0; i < 4 && i < res.Length; i++)
                    res[i] = *pv++;

                output = res;
            };
        }

        public static ulong to8(double input)
        {
            unsafe
            {
                ulong res = 0;
                double* p_val = (double*)&res;
                *p_val = input;
                return res;
            };
        }

        public static void from8(ulong input, out double output)
        {
            unsafe
            {
                double res = 0;
                ulong* p_val = (ulong*)&res;
                *p_val = input;
                output = res;
            };
        }

        public static ulong to8(float x, float y)
        {
            unsafe
            {
                ulong xy = 0;
                float* px = (float*)&xy;
                float* py = px + 1;

                *px = x;
                *py = y;

                return xy;
            };
        }

        public static void from8(ulong input, out float x, out float y)
        {
            unsafe
            {
                ulong xy = input;
                float* px = (float*)&xy;
                float* py = px + 1;
                x = *px;
                y = *py;
            }
        }

        public static ulong to8(float x, int y)
        {
            unsafe
            {
                ulong xy = 0;
                float* px = (float*)&xy;
                int* py = (int*)px + 1;

                *px = x;
                *py = y;

                return xy;
            };
        }

        public static void from8(ulong input, out float x, out int y)
        {
            unsafe
            {
                ulong xy = input;
                float* px = (float*)&xy;
                int* py = (int*)px + 1;
                x = *px;
                y = *py;
            }
        }

        public static ulong to8(float x, uint y)
        {
            unsafe
            {
                ulong xy = 0;
                float* px = (float*)&xy;
                uint* py = (uint*)px + 1;

                *px = x;
                *py = y;

                return xy;
            };
        }

        public static void from8(ulong input, out float x, out uint y)
        {
            unsafe
            {
                ulong xy = input;
                float* px = (float*)&xy;
                uint* py = (uint*)px + 1;
                x = *px;
                y = *py;
            }
        }

        public static ulong to8(float x, short y, short z)
        {
            unsafe
            {
                ulong xy = 0;
                float* px = (float*)&xy;
                short* py = (short*)px + 1;
                short* pz = py + 1;

                *px = x;
                *py = y;
                *pz = z;

                return xy;
            };
        }

        public static void from8(ulong input, out float x, out short y, out short z)
        {
            unsafe
            {
                ulong xy = input;
                float* px = (float*)&xy;
                short* py = (short*)px + 1;
                short* pz = py + 1;
                x = *px;
                y = *py;
                z = *pz;
            }
        }

        public static ulong to8(float x, ushort y, ushort z)
        {
            unsafe
            {
                ulong xy = 0;
                float* px = (float*)&xy;
                ushort* py = (ushort*)px + 1;
                ushort* pz = py + 1;

                *px = x;
                *py = y;
                *pz = z;

                return xy;
            };
        }

        public static void from8(ulong input, out float x, out ushort y, out ushort z)
        {
            unsafe
            {
                ulong xy = input;
                float* px = (float*)&xy;
                ushort* py = (ushort*)px + 1;
                ushort* pz = py + 1;
                x = *px;
                y = *py;
                z = *pz;
            }
        }

        public static ulong to8(float x, short y, ushort z)
        {
            unsafe
            {
                ulong xy = 0;
                float* px = (float*)&xy;
                short* py = (short*)px + 1;
                ushort* pz = (ushort*)py + 1;

                *px = x;
                *py = y;
                *pz = z;

                return xy;
            };
        }

        public static void from8(ulong input, out float x, out short y, out ushort z)
        {
            unsafe
            {
                ulong xy = input;
                float* px = (float*)&xy;
                short* py = (short*)px + 1;
                ushort* pz = (ushort*)py + 1;
                x = *px;
                y = *py;
                z = *pz;
            }
        }

        public static ulong to8(float x, short y, byte za, byte zb)
        {
            unsafe
            {
                ulong xy = 0;
                float* px = (float*)&xy;
                short* py = (short*)px + 1;
                byte* pza = (byte*)py + 1;
                byte* pzb = pza + 1;

                *px = x;
                *py = y;
                *pza = za;
                *pzb = zb;

                return xy;
            };
        }

        public static void from8(ulong input, out float x, out short y, out byte za, out byte zb)
        {
            unsafe
            {
                ulong xy = input;
                float* px = (float*)&xy;
                short* py = (short*)px + 1;
                byte* pza = (byte*)py + 1;
                byte* pzb = pza + 1;
                x = *px;
                y = *py;
                za = *pza;
                zb = *pzb;
            }
        }

        public static ulong to8(float x, ushort y, byte za, byte zb)
        {
            unsafe
            {
                ulong xy = 0;
                float* px = (float*)&xy;
                ushort* py = (ushort*)px + 1;
                byte* pza = (byte*)py + 1;
                byte* pzb = pza + 1;

                *px = x;
                *py = y;
                *pza = za;
                *pzb = zb;

                return xy;
            };
        }

        public static void from8(ulong input, out float x, out ushort y, out byte za, out byte zb)
        {
            unsafe
            {
                ulong xy = input;
                float* px = (float*)&xy;
                ushort* py = (ushort*)px + 1;
                byte* pza = (byte*)py + 1;
                byte* pzb = pza + 1;
                x = *px;
                y = *py;
                za = *pza;
                zb = *pzb;
            }
        }

        public static ulong to8(short[] input)
        {
            unsafe
            {
                ulong val = 0;
                short* pval = (short*)&val;

                for (int i = 0; i < 4 && i < input.Length; i++)
                    *pval++ = input[i];

                return val;
            };
        }

        public static void from8(ulong input, out short[] output)
        {
            unsafe
            {
                ulong v = input;
                short* pv = (short*)&v;

                short[] res = new short[4];
                for (int i = 0; i < 4 && i < res.Length; i++)
                    res[i] = *pv++;

                output = res;
            };
        }

        public static ulong to8(ushort[] input)
        {
            unsafe
            {
                ulong val = 0;
                ushort* pval = (ushort*)&val;

                for (int i = 0; i < 4 && i < input.Length; i++)
                    *pval++ = input[i];

                return val;
            };
        }

        public static void from8(ulong input, out ushort[] output)
        {
            unsafe
            {
                ulong v = input;
                ushort* pv = (ushort*)&v;

                ushort[] res = new ushort[4];
                for (int i = 0; i < 4 && i < res.Length; i++)
                    res[i] = *pv++;

                output = res;
            };
        }

        public static ulong to8(byte[] input)
        {
            unsafe
            {
                ulong val = 0;
                byte* pval = (byte*)&val;

                for (int i = 0; i < 8 && i < input.Length; i++)
                    *pval++ = input[i];

                return val;
            };
        }

        public static void from8(ulong input, out byte[] output)
        {
            unsafe
            {
                ulong v = input;
                byte* pv = (byte*)&v;

                byte[] res = new byte[8];
                for (int i = 0; i < 8 && i < res.Length; i++)
                    res[i] = *pv++;

                output = res;
            };
        }
    }
}
