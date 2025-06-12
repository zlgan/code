// 编译的时侯将iostream的内容拷贝到这里 
#include <iostream>
#include <string>
// i input 
// o output 
//stream 流水
//引入命名空间， 简化cout的写法
using namespace std;
//编译的时候代码中的常量名会被替换成值
#define pi 3.1415
#define myname "weiwei"

int main01()
{
	int a = 35;
	cout << "hello world"<< endl<< a << endl;

	//圆的半径，周长公式=2* pi * r
	int r = 2;
	cout << "圆的周长是： " << 2 * pi * r << endl;
	const short month = 12; //月份
	//month=13常量不能修改
	cout << "一年总共有" << month << "个月"<<endl;
	cout << "my name is " << myname << endl;
	/*
	sizeof()是用来计算某个数据类型的空间大小。
	*******************
	一个字节有8个位
	*******************
	char=一个字符//字符串l1
	short=两个字节
	int=四个字节
	long=（windows是4个字节，linux是8个字节）
	long long=八个字节
	！！！！！！
	*/
	cout << "sizeof(char)=" << sizeof(char) << endl;
	cout << "sizeof(short)=" << sizeof(short) << endl;
	cout << "sizeof(int)=" << sizeof(int) << endl;
	cout << "sizeof(long)=" << sizeof(long) << endl;
	cout << "sizeof(long long)=" << sizeof(long long) << endl;
	//****************************************************
	float f1 = 3.14f;
	double d1 = 3.14;
	cout << "f1=" << f1 << endl;
	cout << "d1=" << d1 << endl;
	cout << "sizeof(f1)=" << sizeof(f1)<< endl;
	cout << "sizeof(d1)=" << sizeof(d1)<< endl;
	float f2 = 34e-3;
	cout << "答案是：" << f2 << endl; //0.034
	//char 赋值中的'a'只能用单引号
	char ch = 'a';
	cout << ch << endl;//a
	cout << (int)ch << endl;//97


	//转义字符：
	//\t制表符
	cout << "aaaaaaaaa\tbbb" << endl;
	//\n换行符
 	cout << "aaaaaaaaa\nbbb" << endl;
	//\\输出一个反斜杠
	cout << "\\" << endl;
	
	//字符串的用法：
	//c语言风格：
	char str[] = "hello world";
	cout << str << endl;
	//c++风格：
	string str2 = "hello world";
	cout << str2 << endl;

	int z = 5;
	bool y = (z == 8);
	cout << y << endl;
	bool y2 = "hello";
	cout << y2 << endl;
	cout << "sizeof(bool)=" << sizeof(bool) << endl;//1
	system("pause");
	return 0;
}