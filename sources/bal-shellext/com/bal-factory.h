#pragma once

#include <Windows.h>
#include <initguid.h>
#include <ShlObj.h>

namespace com {

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

}