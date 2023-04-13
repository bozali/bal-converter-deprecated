#include <ctxmenu-service.h>


const std::vector<MENUITEMINFOW> ContextMenuService::GetMenuItemsFromExtension(std::string_view extension, size_t base_id) const
{
	static WCHAR s_mp3_menu_text[MAX_PATH] = TEXT("MP3");

	std::vector<MENUITEMINFOW> menus;

	if (extension == ".mp4") {
		MENUITEMINFOW mp3_menu;
		SecureZeroMemory(&mp3_menu, sizeof(mp3_menu));

		mp3_menu.wID = base_id + 1;
		mp3_menu.cbSize = sizeof(mp3_menu);
		mp3_menu.fMask = MIIM_STRING | MIIM_ID;
		mp3_menu.dwTypeData = s_mp3_menu_text;
		mp3_menu.fType = MFT_STRING;

		menus.emplace_back(mp3_menu);
	}

	return menus;
}
