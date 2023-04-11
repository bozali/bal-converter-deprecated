#pragma once

#include <core/exports.h>

#include <Windows.h>
#include <ShlObj.h>
#include <string>


namespace utils {


EXTERN_C BALSHELLEXT_DLL std::wstring WINAPI CLSIDToString(const CLSID& clsid);

EXTERN_C BALSHELLEXT_DLL HBITMAP Create32BitHBITMAP(UINT cx, UINT cy, PBYTE* ppbBits);

EXTERN_C BALSHELLEXT_DLL HBITMAP ConvertIconToBitmap(HICON hicon);


}
