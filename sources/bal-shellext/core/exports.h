#pragma once


#if !defined(BALSHELLEXT_EXPORTS)
#	define BALSHELLEXT_DLL __declspec(dllimport)
#else
#	define BALSHELLEXT_DLL __declspec(dllexport)
#endif // BALSHELLEXT_EXPORT
