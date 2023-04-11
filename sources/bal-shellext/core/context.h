#pragma once

#include <core/exports.h>

#include <windows.h>

#include <string>


namespace core {

class BALSHELLEXT_DLL DllContext
{
public:
	DllContext();

	void Initialize(HINSTANCE instance);

	void Increment() {
		InterlockedIncrement(&ref_count_);
	}

	void Decrement() {
		InterlockedDecrement(&ref_count_);
	}

	[[nodiscard]] ULONG GetRefCount() const {
		return ref_count_;
	}

	[[nodiscard]] std::wstring GetModulePath() const;
	[[nodiscard]] HINSTANCE GetHandle() const;

private:
	HINSTANCE instance_;
	ULONG ref_count_;
};

}
