#pragma once

#include <ShlObj.h>
#include <core.h>
#include <string>


namespace utils {


EXTERN_C BALSHELLEXT_DLL std::wstring WINAPI CLSIDToString(const CLSID& clsid);



}
