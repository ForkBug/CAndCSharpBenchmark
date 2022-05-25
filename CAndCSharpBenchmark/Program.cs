
using BenchmarkDotNet.Attributes;
using System.Runtime.InteropServices;

namespace CAndCSharpBenchmark
{
    public class Program
    {

        int DebianSimpleNum = 16000;

        [DllImport("./DebianSimple.dll", EntryPoint = "DebianSimpleC", CallingConvention = CallingConvention.Cdecl), SuppressGCTransition]
        static extern void DebianSimpleC(int count);

        [Benchmark]
        public void DebianSimpleCpp()
        {
            DebianSimpleC(DebianSimpleNum);
        }

        [Benchmark]
        public void DebianSimpleCSharp()
        {
            int Num = DebianSimpleNum;

            int w, h, bit_num = 0;
            byte byte_acc = 0;
            int i, iter = 50;
            double x, y, limit = 2.0;
            double Zr, Zi, Cr, Ci, Tr, Ti;

            w = h = Num;

            byte k = 0;
            //Console.WriteLine("P4\n{0} {1}\n", w, h);

            for (y = 0; y < h; ++y)
            {
                for (x = 0; x < w; ++x)
                {
                    Zr = Zi = Tr = Ti = 0.0;
                    Cr = (2.0 * x / w - 1.5); Ci = (2.0 * y / h - 1.0);

                    for (i = 0; i < iter && (Tr + Ti <= limit * limit); ++i)
                    {
                        Zi = 2.0 * Zr * Zi + Ci;
                        Zr = Tr - Ti + Cr;
                        Tr = Zr * Zr;
                        Ti = Zi * Zi;

                    }

                    byte_acc <<= 1;
                    if (Tr + Ti <= limit * limit) byte_acc |= 0x01;



                    ++bit_num;

                    if (bit_num == 8)
                    {
                        k &= byte_acc;
                        byte_acc = 0;
                        bit_num = 0;
                    }
                    else if (x == w - 1)
                    {
                        byte_acc <<= (8 - w % 8);
                        k &= byte_acc;
                        byte_acc = 0;
                        bit_num = 0;
                    }
                }
            }
            File.WriteAllText("temp2.txt", $"{k}");
        }

        static void Main(string[] args)
        {
            BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Program));
        }
    }
}