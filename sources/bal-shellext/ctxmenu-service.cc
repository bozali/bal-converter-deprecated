#include <ctxmenu-service.h>


ContextMenuService::ContextMenuService()
{
}


std::vector<MENUITEMINFOW> ContextMenuService::GetMenuItemsFromExtension(const std::string_view extension, const size_t base_id)
{
	std::vector<MENUITEMINFOW> menus;

	if (extension == ".mp4") {
		MENUITEMINFOW mp3_menu;
		SecureZeroMemory(&mp3_menu, sizeof(mp3_menu));

		mp3_menu.wID = base_id + menus.size();
		mp3_menu.cbSize = sizeof(mp3_menu);
		mp3_menu.fMask = MIIM_STRING | MIIM_ID;
		mp3_menu.dwTypeData = FormatNames::kMP3MenuText;
		mp3_menu.fType = MFT_STRING;

		menus.emplace_back(mp3_menu);
	}


	return menus;
}
