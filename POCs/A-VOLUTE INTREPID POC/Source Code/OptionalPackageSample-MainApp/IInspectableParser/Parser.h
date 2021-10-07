#pragma once

namespace IInspectableParser
{
    public ref class CParser sealed
    {
    public:
		CParser();
 		static uintptr_t CParser::GetInspectableFromObject(Object ^obj);
	};
}
