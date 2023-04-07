#include <Windows.h>

#include <win32/registry.h>
#include <bal-factory.h>
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


HRESULT STDAPICALLTYPE DllCanUnloadNow()
{
	return g_context.object_count > 0 ? S_FALSE : S_OK;
}

HRESULT STDAPICALLTYPE DllGetClassObject(REFCLSID rclsid, REFIID riid, LPVOID* ppv)
{
	if (!IsEqualCLSID(rclsid, CLSID_BalContextMenu)) {
		return CLASS_E_CLASSNOTAVAILABLE;
	}

	if (!ppv) {
		return E_INVALIDARG;
	}


	*ppv = nullptr;

	auto factory = new BalClassFactory();

	*ppv = factory;

	HRESULT hr = E_UNEXPECTED;


	if (factory != nullptr)
	{
		factory->AddRef();
		hr = factory->QueryInterface(riid, ppv);
		factory->Release();
	}

	// return hr;
}


HRESULT STDAPICALLTYPE DllRegisterServer()
{
	// Create the CLSID key
	auto clsid_key = win32::Registry::LocalMachine().CreateSubKey(TEXT("SOFTWARE\\Classes\\CLSID\\") + utils::CLSIDToString(CLSID_BalContextMenu));

	// Create InProcServer32 key
	auto inprocserver_key = clsid_key.CreateSubKey(TEXT("InProcServer32"));
	
	// Create (default) value (Path to dll)
	inprocserver_key.SetValue(nullptr, core::GetModuleFileNameFromContext(g_context).c_str());

	// Create ThradingModel value (Apartment)
	inprocserver_key.SetValue(TEXT("ThreadingModel"), TEXT("Apartment"));

	// Set an approved value
	win32::Registry::LocalMachine()
		.OpenSubKey(TEXT("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Shell Extensions\\Approved"))
		.SetValue(utils::CLSIDToString(CLSID_BalContextMenu).c_str(), TEXT("Bal Converter Shell Extensions"));

	// Register shell extension for specific file types
	const std::wstring classes_mp3_clsid_path = TEXT("*\\ShellEx\\ContextMenuHandlers\\BalConverter");
	
	win32::Registry::ClassesRoot()
		.CreateSubKey(classes_mp3_clsid_path)
		.SetValue(nullptr, utils::CLSIDToString(CLSID_BalContextMenu).c_str());

	SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, nullptr, nullptr);

	return S_OK;
}


HRESULT STDAPICALLTYPE DllUnregisterServer()
{
	win32::Registry::LocalMachine().DeleteSubKey(TEXT("SOFTWARE\\Classes\\CLSID\\") + utils::CLSIDToString(CLSID_BalContextMenu) + TEXT("\\InprocServer32"));

	SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, nullptr, nullptr);

	return S_OK;
}
