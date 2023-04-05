#include <Windows.h>

#include <bal-ctxmenu.h>
#include <utils.h>

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


extern "C" __declspec(dllexport) HRESULT STDAPICALLTYPE DllRegisterServer()
{
	const std::wstring sub_key_path = TEXT("SOFTWARE\\Classes\\CLSID\\") + utils::CLSIDToString(CLSID_BalContextMenu);
	const std::wstring module_path = core::BalGetModuleFileName(g_context);
	const std::wstring threading_model = TEXT("Apartment");

	DWORD disposition;
	HKEY key;


	// Create CLSID key
	RETURN_IF_FAILED(RegCreateKeyEx(HKEY_LOCAL_MACHINE, sub_key_path.c_str(), 0, NULL, REG_OPTION_NON_VOLATILE, KEY_WRITE, NULL, &key, &disposition));

	// Create InProcServer32
	RETURN_IF_FAILED(RegCreateKeyEx(key, TEXT("InProcServer32"), 0, NULL, REG_OPTION_NON_VOLATILE, KEY_WRITE, NULL, &key, &disposition));

	// Set (default) value
	RETURN_IF_FAILED(RegSetValueEx(key, NULL, 0, REG_SZ, reinterpret_cast<const BYTE*>(module_path.c_str()), (module_path.size() + 1) * sizeof(WCHAR)));

	// Create ThreadingModel to Apartment
	RETURN_IF_FAILED(RegSetValueEx(key, TEXT("ThreadingModel"), 0, REG_SZ, reinterpret_cast<const BYTE*>(threading_model.c_str()), (threading_model.size() + 1) * sizeof(WCHAR)));

	RegCloseKey(key);

	// Create handler key
	RETURN_IF_FAILED(RegCreateKeyEx(HKEY_LOCAL_MACHINE, TEXT("SOFTWARE\\Classes\\txtfile\\ShellEx\\ContextMenuHandlers\\balshellext"), 0, NULL, REG_OPTION_NON_VOLATILE, KEY_WRITE, NULL, &key, &disposition));

	// Set handler key (default) value
	// RETURN_IF_FAILED(RegSetValueEx(key, NULL, 0, REG_SZ, reinterpret_cast<const BYTE*>()));

	SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, NULL, NULL);

	return S_OK;
}


EXTERN_C HRESULT STDAPICALLTYPE DllUnregsterServer()
{
	return E_NOTIMPL;
}
