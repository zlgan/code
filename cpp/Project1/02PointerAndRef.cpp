#include <iostream>
#include "common.h"
using namespace std;

void Increment1(int* ptr)  
{  
	(*ptr)++;  
}

void Increment2(int& v1)
{
	v1++;
}

int main02()
{


	//==========指针==========

	int var = 0xFF00AB11;
	string abc = "abc";


	// c语言风格的字符串占用4个字节
	//0x000000d6fdb7fa04 {0x62 'b', 0x62 'b', 0x62 'b', 0x00 '\0'}
	char chr[] = "bbb";


	//内存地址
	// 占用了11个字节
	//0x000000D6FDB7F988  40 21 dd 63 b1 02 00 00 61 62 63
	string* ptr2 = &abc;


	int* ptr = &var;
	*ptr = 10;


	/*
	使用 memset 函数对已分配的内存空间进行初始化。它将从 buffer 指向的内存地址开始的 8 个字节都设置为 0 值
	*/
	char* buffer = new char[8];
	memset(buffer, 0, 8);
 
	char** ptr3 = &buffer;
	cout << var << endl;


	//==========引用==========
	cout << "==========引用==========" << endl;

	// 引用，是指针的语法糖
	// 引用相當于變量的別名
	int a = 10;
	
	//ref 引用了a，他并不是一个变量
	int& ref = a;
	cout << ref << endl;
	ref = 2;
	cout << a << endl;

	Increment1(&a);// 2 
	cout << a << endl;//3
	a = 5;
	Increment2(a);//6
	cout << a << endl;
	a = 5;
	int b = 4;
	int& ref2 = a;
	ref2 = b;
	ref2++;
	cout << "a= " << a << ",b= " << b << endl; //a= 5,b= 4
	return 0;




}