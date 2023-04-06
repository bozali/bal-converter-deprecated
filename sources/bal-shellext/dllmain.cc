#include <Windows.h>

#include <win32/registry.h>
#include <bal-ctxmenu.h>
#include <utils.h>

#include <iostream>
#include <string>


#if !defined(RETURN_IF_FAILED)
#	define RETURN_IF_FAILED(expr) if (FAILED(expr)) return E_UNEXPECTED
#endif // RETURN_IF_FAILED

core::DllContext g_context;


BOOL WINAPI DllMain(HINSTANCE instance, DWORD reason, LPVOID reserved)
{
	switch (reason)
	{
	case DLL_PROCESS_ATTACH:
		g_context.dll_instance = instance;
		break;

	case DLL_THREAD_ATTACH:
		break;

	case DLL_THREAD_DETACH:
		break;

	case DLL_PROCESS_DETACH:
		break;
	}

	return TRUE;
}


EXTERN_C HRESULT STDAPICALLTYPE DllCanunloadNow()
{
	return E_NOTIMPL;
}


EXTERN_C HRESULT STDAPICALLTYPE DllGetClassObject(REFCLSID rclsid, REFIID riid, LPVOID* ppv)
{
	return E_NOTIMPL;
}


EXTERN_C BALSHELLEXT_DLL HRESULT STDAPICALLTYPE DllRegisterServer()
{
	const std::wstring clsid_key_path = TEXT("SOFTWARE\\Classes\\CLSID\\") + utils::CLSIDToString(CLSID_BalContextMenu);

	// Create the CLSID key
	auto clsid_key = win32::Registry::LocalMachine().CreateSubKey(clsid_key_path);

	// Create InProcServer32 key
	auto inprocserver_key = clsid_key.CreateSubKey(TEXT("InProcServer32"));
	
	// Create (default) value (Path to dll)
	inprocserver_key.SetValue(nullptr, core::GetModuleFileNameFromContext(g_context).c_str());

	// Create ThradingModel value (Apartment)
	inprocserver_key.SetValue(TEXT("ThreadingModel"), TEXT("Apartment"));

	// Register shell extension for specific file types
	const std::wstring classes_mp3_clsid_path = TEXT(".mp3\\ShellEx\\CustomMenuHandlers\\" + utils::CLSIDToString(CLSID_BalContextMenu));

	auto mp3_clsid_key = win32::Registry::ClassesRoot().CreateSubKey(classes_mp3_clsid_path);

	SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, nullptr, nullptr);

	return S_OK;
}


EXTERN_C HRESULT STDAPICALLTYPE DllUnregsterServer()
{
	return E_NOTIMPL;
}
