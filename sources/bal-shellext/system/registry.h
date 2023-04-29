#pragma once

#include <core/exports.h>

#include <windows.h>

#include <iostream>
#include <memory>
#include <string>


namespace system {

class BALSHELLEXT_DLL RegistryKey
{
public:
	explicit RegistryKey(HKEY handle);

	RegistryKey CreateSubKey(const std::wstring_view subkey_path) const;

	RegistryKey OpenSubKey(const std::wstring_view subkey_path) const;

	void DeleteSubKey(const std::wstring_view subkey_path) const;

	template <typename T>
	void SetValue(LPCWSTR name, T value) {}

	template <>
	void SetValue(LPCWSTR name, LPCWSTR value) {
		const size_t size = wcsnlen_s(value, MAX_PATH);

		HRESULT hr = RegSetValueEx(handle_, name, 0, REG_SZ, reinterpret_cast<const BYTE*>(value), (size + 1) * sizeof(WCHAR));

		if (FAILED(hr)) {
			std::cerr << "Failed to set registry value: " << std::hex << hr << std::endl;
		}
	}

private:
	HKEY handle_;
};


class BALSHELLEXT_DLL Registry
{
public:

	FORCEINLINE static RegistryKey LocalMachine() {
		return RegistryKey(static_cast<HKEY>(HKEY_LOCAL_MACHINE));
	}

	FORCEINLINE static RegistryKey ClassesRoot() {
		return RegistryKey(static_cast<HKEY>(HKEY_CLASSES_ROOT));
	}

private:
};


}