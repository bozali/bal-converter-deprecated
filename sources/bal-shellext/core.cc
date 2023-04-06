#include <core.h>

using namespace core;


EXTERN_C BALSHELLEXT_DLL std::wstring WINAPI GetModuleFileNameFromContext(const DllContext& context)
{
	WCHAR buffer[MAX_PATH];
	SecureZeroMemory(buffer, MAX_PATH);

	GetModuleFileName(context.dll_instance, buffer, MAX_PATH);
	return buffer;
}
