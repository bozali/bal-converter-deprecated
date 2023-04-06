#pragma once

#include <Windows.h>

#include <string>


#if !defined(BALSHELLEXT_EXPORTS)
#	define BALSHELLEXT_DLL __declspec(dllimport)
#else
#	define BALSHELLEXT_DLL __declspec(dllexport)
#endif // BALSHELLEXT_EXPORT


namespace core {

struct DllContext
{
	HINSTANCE dll_instance;
};


EXTERN_C BALSHELLEXT_DLL std::wstring WINAPI GetModuleFileNameFromContext(const DllContext& context);

}

