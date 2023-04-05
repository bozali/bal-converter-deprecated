#include <utils.h>
#include <iostream>

using namespace utils;



EXTERN_C BALSHELLEXT_DLL std::wstring WINAPI  CLSIDToString(const CLSID& clsid)
{
	LPOLESTR clsid_str;
	HRESULT hr = StringFromCLSID(clsid, &clsid_str);

	if (FAILED(hr)) {
		std::cerr << "Failed to convert CLSID to string: " << std::hex << hr << std::endl;
		return TEXT("");
	}

	std::wstring result(clsid_str);
	CoTaskMemFree(clsid_str);

	return result;
}
