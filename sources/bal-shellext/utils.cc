#include <utils.h>
#include <iostream>
#include <wincodec.h>

#pragma comment(lib, "windowscodecs.lib")

using namespace utils;


EXTERN_C BALSHELLEXT_DLL std::wstring WINAPI  CLSIDToString(const CLSID& clsid)
{
	LPOLESTR clsid_str;
	HRESULT hr = StringFromCLSID(clsid, &clsid_str);

	if (FAILED(hr)) {
		std::cerr << "Failed to convert CLSID to string: " << std::hex << hr << std::endl;
		return TEXT("");
	}

	std::wstring result(clsid_str);
	CoTaskMemFree(clsid_str);

	return result;
}

EXTERN_C BALSHELLEXT_DLL HBITMAP Create32BitHBITMAP(UINT cx, UINT cy, PBYTE* ppbBits)
{
	BITMAPINFO bmi;
	ZeroMemory(&bmi, sizeof(bmi));
	bmi.bmiHeader.biSize = sizeof(bmi.bmiHeader);
	bmi.bmiHeader.biWidth = cx;
	bmi.bmiHeader.biHeight = -(LONG)cy;
	bmi.bmiHeader.biPlanes = 1;
	bmi.bmiHeader.biBitCount = 32;
	bmi.bmiHeader.biCompression = BI_RGB;
	HDC hDC = GetDC(NULL);
	HBITMAP hbmp = CreateDIBSection(hDC, &bmi, DIB_RGB_COLORS, (void**)ppbBits, NULL, 0);
	ReleaseDC(NULL, hDC);
	return(hbmp);
}


EXTERN_C BALSHELLEXT_DLL HBITMAP ConvertIconToBitmap(HICON hicon)
{
	IWICImagingFactory* pFactory;
	IWICBitmap* pBitmap;
	IWICFormatConverter* pConverter;
	HBITMAP ret = NULL;

	if (SUCCEEDED(CoCreateInstance(CLSID_WICImagingFactory, NULL,
		CLSCTX_INPROC_SERVER, IID_IWICImagingFactory, (void**)&pFactory)))
	{
		if (SUCCEEDED(pFactory->CreateBitmapFromHICON(hicon, &pBitmap)))
		{
			if (SUCCEEDED(pFactory->CreateFormatConverter(&pConverter)))
			{
				UINT cx, cy;
				PBYTE pbBits;
				HBITMAP hbmp;

				if (SUCCEEDED(pConverter->Initialize(pBitmap, GUID_WICPixelFormat32bppPBGRA,
					WICBitmapDitherTypeNone, NULL, 0.0f, WICBitmapPaletteTypeCustom)) && SUCCEEDED(
						pConverter->GetSize(&cx, &cy)) && NULL != (hbmp = Create32BitHBITMAP(cx, cy, &pbBits)))
				{
					UINT cbStride = cx * sizeof(UINT32);
					UINT cbBitmap = cy * cbStride;

					if (SUCCEEDED(pConverter->CopyPixels(NULL, cbStride, cbBitmap, pbBits)))
						ret = hbmp;
					else
						DeleteObject(hbmp);
				}

				pConverter->Release();
			}

			pBitmap->Release();
		}

		pFactory->Release();
	}

	return ret;
}