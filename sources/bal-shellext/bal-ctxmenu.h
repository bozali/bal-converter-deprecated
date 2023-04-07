#pragma once

#include <Windows.h>
#include <initguid.h>
#include <ShlObj.h>

#include <core.h>

// {3956E7F6-392B-493B-B0D2-89AF7E60E342}
DEFINE_GUID(CLSID_BalContextMenu, 0x3956e7f6, 0x392b, 0x493b, 0xb0, 0xd2, 0x89, 0xaf, 0x7e, 0x60, 0xe3, 0x42);

extern core::DllContext g_context;


class BalContextMenu : public IContextMenu, public IShellExtInit, public IUnknown
{
public:
	HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid, LPVOID* object);

	ULONG STDMETHODCALLTYPE AddRef();

	ULONG STDMETHODCALLTYPE Release();

	HRESULT STDMETHODCALLTYPE Initialize(PCIDLIST_ABSOLUTE pidlFolder, IDataObject* pdtobj, HKEY hkeyProgID);

	HRESULT STDMETHODCALLTYPE QueryContextMenu(HMENU menu, UINT index_menu, UINT cmd_first, UINT cmd_last, UINT flags);

	HRESULT STDMETHODCALLTYPE InvokeCommand(LPCMINVOKECOMMANDINFO pici);

	HRESULT STDMETHODCALLTYPE GetCommandString(UINT_PTR cmd, UINT type, UINT* reserved, CHAR* name, UINT cch_max);

private:
	ULONG ref_count_;
};


class BalClassFactory : IClassFactory, IUnknown
{
public:
	HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid, LPVOID* object);

	ULONG STDMETHODCALLTYPE AddRef();

	ULONG STDMETHODCALLTYPE Release();

	HRESULT STDMETHODCALLTYPE CreateInstance(IUnknown* unkown, REFIID riid, LPVOID* object);

	HRESULT STDMETHODCALLTYPE LockServer(BOOL lock);

private:
	ULONG ref_count_;
};
