#pragma once

#include <Windows.h>

#include <string>
#include <vector>

class FormatNames
{
public:
	inline static WCHAR kMP3MenuText[MAX_PATH] = TEXT("MP3");
	inline static WCHAR kMP4MenuText[MAX_PATH] = TEXT("MP4");
	inline static WCHAR kWavMenuText[MAX_PATH] = TEXT("Wav");
};



class ContextMenuService
{
public:
	ContextMenuService();

	[[nodiscard]] std::vector<MENUITEMINFOW> GetMenuItemsFromExtension(std::string_view extension, size_t base_id);

private:
};
