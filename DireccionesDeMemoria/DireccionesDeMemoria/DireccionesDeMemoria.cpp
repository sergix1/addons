// DireccionesDeMemoria.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <stdlib.h>
#include <stdio.h>
#include <iostream>
int main()
{
	int done = 10;
	char b = 'A';
	printf("%i \n", &done);
	std::cout << "address of char   :" << static_cast<void *>(&b) << std::endl;
	system("pause");
    return 0;
}

