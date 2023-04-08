#include <bal-ctxmenu.h>
#include <strsafe.h>
#include <resource.h>
#include <filesystem>
#include <utils.h>

#include <wrl/client.h>

using namespace Microsoft::WRL;

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


HRESULT STDMETHODCALLTYPE BalContextMenu::Initialize(PCIDLIST_ABSOLUTE pidl_folder, IDataObject* pdtobj, HKEY hkey_progid)
{
	namespace fs = std::filesystem;

	FORMATETC format;
	SecureZeroMemory(&format, sizeof(format));

	format.cfFormat = CF_HDROP;
	format.dwAspect = DVASPECT_CONTENT;
	format.lindex = -1;
	format.tymed = TYMED_HGLOBAL;

	STGMEDIUM medium;
	SecureZeroMemory(&medium, sizeof(medium));

	if (FAILED(pdtobj->GetData(&format, &medium))) {
		return E_UNEXPECTED;
	}

	HDROP drop = static_cast<HDROP>(GlobalLock(medium.hGlobal));

	if (drop)
	{
		UINT num_files = DragQueryFile(drop, 0xFFFFFFFF, nullptr, 0);

		if (num_files != 1) {
			return E_INVALIDARG;
		}

		TCHAR filepath[MAX_PATH];

		if (DragQueryFile(drop, 0, filepath, MAX_PATH))
		{
			if (!fs::path(filepath).has_extension()) {
				return E_INVALIDARG;
			}

			const auto extension = fs::path(filepath).extension();
		}

		GlobalUnlock(medium.hGlobal);
	}

	return S_OK;
}


std::wstring GetLastErrorAsString()
{
	//Get the error message ID, if any.
	DWORD errorMessageID = ::GetLastError();
	if (errorMessageID == 0) {
		return std::wstring(); //No error message has been recorded
	}

	LPWSTR messageBuffer = nullptr;

	//Ask Win32 to give us the string version of that message ID.
	//The parameters we pass in, tell Win32 to create the buffer that holds the message for us (because we don't yet know how long the message string will be).
	size_t size = FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
		NULL, errorMessageID, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPWSTR)&messageBuffer, 0, NULL);

	//Copy the error message into a std::string.
	std::wstring message(messageBuffer, size);

	//Free the Win32's string's buffer.
	LocalFree(messageBuffer);

	return message;
}


HRESULT STDMETHODCALLTYPE BalContextMenu::QueryContextMenu(HMENU menu, UINT index_menu, UINT cmd_first, UINT cmd_last, UINT flags)
{
	if (flags & CMF_DEFAULTONLY)
	{
		return MAKE_HRESULT(SEVERITY_SUCCESS, FACILITY_NULL, 0);
	}

	WCHAR item_text[MAX_PATH] = TEXT("Bal Converter");

	HICON icon = (HICON)LoadImage(g_context.dll_instance, MAKEINTRESOURCE(IDI_LOGO), IMAGE_ICON, 16, 16, LR_DEFAULTSIZE | LR_LOADTRANSPARENT);

	MENUITEMINFOW item;
	item.wID = cmd_first + 1;
	item.cbSize = sizeof(item);
	item.fMask = MIIM_STRING | MIIM_ID | MIIM_BITMAP;
	item.dwTypeData = item_text;
	item.hbmpItem = utils::ConvertIconToBitmap(icon);
	item.fType = MFT_STRING;

	if (!InsertMenuItem(menu, index_menu, TRUE, &item)) {
		return HRESULT_FROM_WIN32(GetLastError());
	}

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
	return E_NOTIMPL;
}
