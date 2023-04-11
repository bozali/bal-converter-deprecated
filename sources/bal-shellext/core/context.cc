#include <core/context.h>

using namespace core;


DllContext::DllContext()
	: ref_count_(0)
{
}


void DllContext::Initialize(HINSTANCE instance)
{
	instance_ = instance;
}


std::wstring DllContext::GetModulePath() const
{
	WCHAR buffer[MAX_PATH];
	SecureZeroMemory(buffer, MAX_PATH);

	GetModuleFileName(instance_, buffer, MAX_PATH);
	return buffer;
}


HINSTANCE DllContext::GetHandle() const
{
	return instance_;
}
