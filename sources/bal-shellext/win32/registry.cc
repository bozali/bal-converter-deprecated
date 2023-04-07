#include <win32/registry.h>

using namespace win32;


RegistryKey::RegistryKey(HKEY handle)
	: handle_(handle)
{
}


RegistryKey RegistryKey::CreateSubKey(const std::wstring_view subkey)
{
	HKEY subkey_handle;
	DWORD disposition;

	HRESULT hr = RegCreateKeyEx(handle_, subkey.data(), 0, nullptr, REG_OPTION_NON_VOLATILE, KEY_WRITE, nullptr, &subkey_handle, &disposition);

	if (FAILED(hr)) {
		std::cerr << "Failed to create registry key: " << std::hex << hr << std::endl;
	}

	return RegistryKey(subkey_handle);
}


RegistryKey RegistryKey::OpenSubKey(const std::wstring_view subkey)
{
	HKEY subkey_handle;
	DWORD disposition;

	HRESULT hr = RegOpenKeyEx(handle_, subkey.data(), REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, &subkey_handle);

	if (FAILED(hr)) {
		std::cerr << "Failed to create registry key: " << std::hex << hr << std::endl;
	}

	return RegistryKey(subkey_handle);
}

