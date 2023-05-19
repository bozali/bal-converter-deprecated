#include <Windows.h>

#include <system/registry.h>
#include <core/context.h>

#include <com/bal-ctxmenu.h>
#include <com/bal-factory.h>
#include <utils.h>

#include <iostream>
#include <string>


core::DllContext g_context;

BOOL WINAPI DllMain(HINSTANCE instance, DWORD reason, LPVOID reserved)
{
	switch (reason)
	{
	case DLL_PROCESS_ATTACH:
		g_context.Initialize(instance);
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
	return g_context.GetRefCount() > 0 ? S_FALSE : S_OK;
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

	auto factory = new com::BalClassFactory();

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
	auto clsid_key = sys::Registry::LocalMachine().CreateSubKey(TEXT("SOFTWARE\\Classes\\CLSID\\") + utils::CLSIDToString(CLSID_BalContextMenu));

	// Create InProcServer32 key
	auto inprocserver_key = clsid_key.CreateSubKey(TEXT("InProcServer32"));
	
	// Create (default) value (Path to dll)
	inprocserver_key.SetValue(nullptr, g_context.GetModulePath().c_str());

	// Create ThradingModel value (Apartment)
	inprocserver_key.SetValue(TEXT("ThreadingModel"), TEXT("Apartment"));

	// Set an approved value
	sys::Registry::LocalMachine()
		.OpenSubKey(TEXT("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Shell Extensions\\Approved"))
		.SetValue(utils::CLSIDToString(CLSID_BalContextMenu).c_str(), TEXT("Bal Converter Shell Extensions"));

	// Register shell extension for specific file types
	const std::wstring classes_mp3_clsid_path = TEXT("*\\ShellEx\\ContextMenuHandlers\\BalConverter");
	
	sys::Registry::ClassesRoot()
		.CreateSubKey(classes_mp3_clsid_path)
		.SetValue(nullptr, utils::CLSIDToString(CLSID_BalContextMenu).c_str());

	SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, nullptr, nullptr);

	return S_OK;
}


HRESULT STDAPICALLTYPE DllUnregisterServer()
{
	sys::Registry::LocalMachine().DeleteSubKey(TEXT("SOFTWARE\\Classes\\CLSID\\") + utils::CLSIDToString(CLSID_BalContextMenu) + TEXT("\\InprocServer32"));

	SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, nullptr, nullptr);

	return S_OK;
}
