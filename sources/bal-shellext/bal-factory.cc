#include <bal-factory.h>
#include <bal-ctxmenu.h>


HRESULT STDMETHODCALLTYPE BalClassFactory::QueryInterface(REFIID riid, LPVOID* object)
{
	if (IsEqualIID(riid, IID_IUnknown))
	{
		*object = this;
		AddRef();

		return S_OK;
	}
	else if (IsEqualIID(riid, IID_IClassFactory))
	{
		*object = static_cast<IClassFactory*>(this);
		AddRef();

		return S_OK;
	}

	*object = nullptr;
	return E_NOINTERFACE;
}


ULONG STDMETHODCALLTYPE BalClassFactory::AddRef()
{
	g_context.Increment();
	return InterlockedIncrement(&ref_count_);
}


ULONG STDMETHODCALLTYPE BalClassFactory::Release()
{
	g_context.Decrement();
	ULONG temp_ref_count = InterlockedDecrement(&ref_count_);

	if (temp_ref_count == 0)
	{
		delete this;
	}

	return temp_ref_count;
}


HRESULT STDMETHODCALLTYPE BalClassFactory::CreateInstance(IUnknown* unkown, REFIID riid, LPVOID* object)
{
	if (!object) {
		return E_INVALIDARG;
	}

	if (unkown != nullptr) {
		return CLASS_E_NOAGGREGATION;
	}

	HRESULT hr = E_UNEXPECTED;

	if (IsEqualIID(riid, IID_IShellExtInit) || IsEqualIID(riid, IID_IContextMenu))
	{
		auto context_menu = new BalContextMenu();

		if (context_menu == nullptr) {
			return E_OUTOFMEMORY;
		}

		context_menu->AddRef();

		hr = context_menu->QueryInterface(riid, object);

		context_menu->Release();
	}
	else {
		hr = E_NOINTERFACE;
	}

	return hr;
}


HRESULT STDMETHODCALLTYPE BalClassFactory::LockServer(BOOL lock)
{
	return S_OK;
}
