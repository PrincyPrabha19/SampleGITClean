#include "pch.h"
#include "Parser.h"

using namespace IInspectableParser;
using namespace Platform;
using namespace Microsoft::WRL;
using namespace Microsoft::WRL::Wrappers;

CParser::CParser()
{
}

uintptr_t CParser::GetInspectableFromObject(Object ^obj)
{
	auto inspectable = reinterpret_cast<IInspectable*>(obj);
	return (uintptr_t)inspectable;
}


