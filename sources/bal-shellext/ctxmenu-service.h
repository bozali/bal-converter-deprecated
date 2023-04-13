#pragma once

#include <Windows.h>

#include <string>
#include <vector>

class ContextMenuService
{
public:
	const std::vector<MENUITEMINFOW> GetMenuItemsFromExtension(std::string_view extension, size_t base_id) const;
};
