#include <bal-ctxmenu.h>
#include <strsafe.h>


HRESULT STDMETHODCALLTYPE BalContextMenu::QueryInterface(REFIID riid, LPVOID* object)
{
	if (IsEqualIID(riid, IID_IUnknown))
	{
		*object = this;
		AddRef();

		return S_OK;
	}
	else if (IsEqualIID(riid, IID_IContextMenu))
	{
		*object = static_cast<IContextMenu*>(this);
		AddRef();

		return S_OK;
	}
	else if (IsEqualIID(riid, IID_IShellExtInit))
	{
		*object = static_cast<IShellExtInit*>(this);
		AddRef();

		return S_OK;
	}
	
	*object = nullptr;
	return E_NOINTERFACE;
}


ULONG STDMETHODCALLTYPE BalContextMenu::AddRef()
{
	return InterlockedIncrement(&ref_count_);
}


ULONG STDMETHODCALLTYPE BalContextMenu::Release()
{
	ULONG temp_ref_count = InterlockedDecrement(&ref_count_);

	if (temp_ref_count == 0)
	{
		delete this;
	}

	return temp_ref_count;
}


HRESULT STDMETHODCALLTYPE BalContextMenu::Initialize(PCIDLIST_ABSOLUTE pidlFolder, IDataObject* pdtobj, HKEY hkeyProgID)
{
	return S_OK;
}


HRESULT STDMETHODCALLTYPE BalContextMenu::QueryContextMenu(HMENU menu, UINT index_menu, UINT cmd_first, UINT cmd_last, UINT flags)
{
	if (flags & CMF_DEFAULTONLY)
	{
		return MAKE_HRESULT(SEVERITY_SUCCESS, FACILITY_NULL, 0);
	}

	MENUITEMINFO item;
	
	WCHAR item_text[MAX_PATH] = TEXT("Bal Converter Menu");

	item.wID = cmd_first + 1;
	item.cbSize = sizeof(item);
	item.fMask = MIIM_STRING | MIIM_ID;
	item.dwTypeData = item_text;
	// item.hbmpItem = 

	// InsertMenuItem(menu, 0, TRUE, &item);
	// InsertMenu(menu, index_menu++, MF_STRING | MF_BYPOSITION, cmd_first, TEXT("Bal Converter Menu"));
	// InsertMenu(menu,
	// 	index_menu,
	// 	MF_STRING | MF_BYPOSITION,
	// 	cmd_first,
	// 	TEXT("&Bal Converter"));

	return MAKE_HRESULT(SEVERITY_SUCCESS, FACILITY_NULL, item.wID - cmd_first + 1);
}


HRESULT STDMETHODCALLTYPE BalContextMenu::InvokeCommand(LPCMINVOKECOMMANDINFO pici)
{
	if (HIWORD(pici))
	{
		return E_INVALIDARG;
	}

	switch (LOWORD(pici->lpVerb))
	{
	case 0:
		// Handle MyMenu command
		break;
	default:
		return E_INVALIDARG;
	}

	return S_OK;
}


HRESULT STDMETHODCALLTYPE BalContextMenu::GetCommandString(UINT_PTR cmd, UINT type, UINT* reserved, CHAR* name, UINT cch_max)
{
	if (type == GCS_HELPTEXT)
	{
		StringCchCopyA(name, cch_max, "Bal Converter Test");
		return S_OK;
	}

	return E_NOTIMPL;
}


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
	InterlockedIncrement(&g_context.object_count);
	return InterlockedIncrement(&ref_count_);
}


ULONG STDMETHODCALLTYPE BalClassFactory::Release()
{
	InterlockedDecrement(&g_context.object_count);
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
		MessageBox(nullptr, TEXT("Creating instacen"), TEXT("DEBUG"), MB_OK);

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
