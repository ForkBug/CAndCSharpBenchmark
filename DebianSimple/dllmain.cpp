// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include<stdio.h>


BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}



extern "C" __declspec(dllexport)
void __cdecl DebianSimpleC(int num)
{
	int w, h, bit_num = 0;
	char byte_acc = 0;
	int i, iter = 50;
	double x, y, limit = 2.0;
	double Zr, Zi, Cr, Ci, Tr, Ti;

	w = h = num;


	FILE* strm = NULL;
	fopen_s(&strm, "temp.txt", "a");
	char k = 0;



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
	putc(k, strm);
	fclose(strm);
}

