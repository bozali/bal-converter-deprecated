#include <system/registry.h>

using namespace system;


RegistryKey::RegistryKey(const HKEY handle)
	: handle_(handle)
{
}


RegistryKey RegistryKey::CreateSubKey(const std::wstring_view subkey_path) const
{
	HKEY subkey_handle;
	DWORD disposition;

	HRESULT hr = RegCreateKeyEx(handle_, subkey_path.data(), 0, nullptr, REG_OPTION_NON_VOLATILE, KEY_WRITE, nullptr, &subkey_handle, &disposition);

	if (FAILED(hr)) {
		std::cerr << "Failed to create registry key: " << std::hex << hr << std::endl;
	}

	return RegistryKey(subkey_handle);
}


RegistryKey RegistryKey::OpenSubKey(const std::wstring_view subkey_path) const
{
	HKEY subkey_handle;
	DWORD disposition;

	HRESULT hr = RegOpenKeyEx(handle_, subkey_path.data(), REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, &subkey_handle);

	if (FAILED(hr)) {
		std::cerr << "Failed to create registry key: " << std::hex << hr << std::endl;
	}

	return RegistryKey(subkey_handle);
}


void RegistryKey::DeleteSubKey(const std::wstring_view subkey_path) const
{
	HRESULT hr = RegDeleteKey(handle_, subkey_path.data());

	if (FAILED(hr)) {
		std::cerr << "Failed to delete registry key: " << std::hex << hr << std::endl;
	}
}
