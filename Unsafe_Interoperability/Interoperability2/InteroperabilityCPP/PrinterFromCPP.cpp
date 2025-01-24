#include "PrinterFromCPP.h"
#include <stdio.h>
#include<string.h>

void ExportLib::SayFromCPP(const char* mg)
{
	printf("I say: '%s' from c++", mg);
}

const char* ExportLib::GetSecretMsgFromCPP()
{
	const char message[] = "Secret msg From C++";
	char* copy = new char[sizeof(message)];
	memcpy_s(copy, sizeof(message), message, sizeof(message));
	return copy;
}

void ExportLib::DestroySecretMsgFromCPP(const char* str)
{
	delete[] str;
}

bool ExportLib::AreTheSame(bool __stdcall comparer(const char*, const char*), const char* a, const char* b)
{
    return comparer(a, b);
}

void ExportLib::SortNumbers(int* numbers, int size)
{
	for (int i = 0; i < size - 1; i++)
		for (int j = 0; j < size - 1; j++) {
			if (numbers[j] < numbers[j + 1]) {
				int tmp = numbers[j];
				numbers[j] = numbers[j + 1];
				numbers[j + 1] = tmp;
			}
		}
}

void ExportLib::Negative(Color* color)
{
	for (int i = 0; i < 4; i++)
		color->rgba[i] = 1 - color->rgba[i];
}
