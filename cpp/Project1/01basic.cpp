#include <iostream>
#include "common.h"
using namespace std;


//定义个log，让编译其知道有这个函数，
//即使这个函数没有定义，也可以编译通过，会在连接阶段报错
//这个函数可以在其他文件中定义，且只能定义一次
//类似这样的函数声明，可以放到一个头文件中(.h)中，在需要的地方通过#include引用这个头文件
void log(const char* message);


//static 建议编译器在调用该函数的地方直接插入函数体代码（即内联展开），以减少函数调用的开销
inline void Multiply2(int a, int b) {}

//即该函数只在当前源文件（.cpp）中可见，不能被其他文件访问
static int Multiply(int a, int b)
{
	/*如果log没有定义的情况下，这里的编译LINK会报错：
	* 无法解析的外部符号 "void __cdecl log(char const *)" (?log@@YAXPEBD@Z)，函数 "int __cdecl Multiply(int,int)" (?Multiply@@YAHHH@Z) 中引用了该符号
	即使Multiply没有被任何函数调用的情况下这里也无法编译通过
	*/
	log("Multiply called");
	return a * b;
}
int main01()
{
	int variable = 8;
	std::cout << variable << endl;
	log("hello");

	std::cout << Multiply(variable,3) << endl;
	//====数据类型====
	// char(1),short(2),int(4),long(4/8),long long(8)，float(4),double(8)
	char c1 = 65;
	char c3 = 0b1000001;
	char c2 = 'A';
	short s1 = 65;
	// A A A 65
	cout << c1 <<" "<<c2 << " " << c3 << " " <<  s1 << " " << endl;
	//默认是有符号整数表示的范围为 -2^15---2^15-1
	short s2 = 23;
	//无符号可以表示，所有的位都用来存储数据，范围为0--2^16-1
	unsigned short s3 = 65535;

	//小数后面的f提示该数字是float，否则会当作double处理
	float f1 = 5.5f;
	double db1 = 6.6;

	bool b1 = true; //1
	bool b2 = "hello"; //1 
	bool b3 = 1; // 1
	bool b4 = -1; // 1 
	bool b5 = 0; // 0
	// bool类型0表示false，其他的都是true
	cout << (b5 ==true) << endl;
	cout << (b5 == 1) << endl;
	

	//sizeof 可以查询类型或变量的长度
	int abc = 123;
	cout << sizeof(int) << endl;
	cout << sizeof(abc) << endl;

	//这句只有在main函数中可以省略
	return 0;
}