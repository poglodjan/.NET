#include "Color.h"
namespace ExportLib {
	extern "C" __declspec(dllexport) void SayFromCPP(const char* mg);
	extern "C" __declspec(dllexport) const char* GetSecretMsgFromCPP();
	extern "C" __declspec(dllexport) void DestroySecretMsgFromCPP(const char*);
															
	extern "C" __declspec(dllexport) bool AreTheSame(bool __stdcall comparer(const char*, const char*), const char* a, const char* b);

	extern "C" __declspec(dllexport) void SortNumbers(int *numbers, int size);

	extern "C" __declspec(dllexport) void Negative(Color* color);
}