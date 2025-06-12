// 编译的时侯将iostream的内容拷贝到这里 
#include <iostream>
<<<<<<< HEAD
=======
#include <string>
>>>>>>> 4fa7ecb9e8258fc2ff093433a6b119d76b26aaed
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
<<<<<<< HEAD
	cout << "hello world"<< endl<< a << endl;
=======
	cout << "hello world" << endl << a << endl;
>>>>>>> 4fa7ecb9e8258fc2ff093433a6b119d76b26aaed

	//圆的半径，周长公式=2* pi * r
	int r = 2;
	cout << "圆的周长是： " << 2 * pi * r << endl;
	const short month = 12; //月份
	//month=13常量不能修改
<<<<<<< HEAD
	cout << "一年总共有" << month << "个月"<<endl;
	cout << "my name is " << myname << endl;
	
=======
	cout << "一年总共有" << month << "个月" << endl;
	cout << "my name is " << myname << endl;

>>>>>>> 4fa7ecb9e8258fc2ff093433a6b119d76b26aaed
	int s = 0;
	//写法：sizeof(数据类型，变量)  解释：是用来计算数据类型占用的多少字节。
	//整数占用内存：
	cout << "sizeof(int)=" << sizeof(int) << endl;
	cout << "sizeof(short)=" << sizeof(short) << endl;
	cout << "sizeof(long)=" << sizeof(long) << endl;
	cout << "sizeof(long long)=" << sizeof(long long) << endl;
	cout << "sizeof(long)=" << sizeof(long) << endl;
	float f1 = 3.1415926f;
	double d1 = 3.1415926;
<<<<<<< HEAD
	cout << "sizeof(float)=" << f1 << endl;

	cout << "sizeof(double)=" << d1<< endl;
	float f2 = 34e-3;
	cout << "f2=" <<f2<< endl; //0.034

	char ch1 = 'a';
	cout << "char a = "<<ch1 << endl;

	system("pause");
	return 0;
}	
=======
	cout << f1 << endl;

	cout << d1 << endl;
	float f2 = 34e-3;
	cout << "f2=" << f2 << endl; //0.034

	char ch1 = 'A';
	cout << "char a = " << ch1 << endl;
	cout << (int)ch1 << endl;
	char str[] = "hallo world";
	cout << str << endl;
	string st = "hello world";
	cout << st << endl;
	bool dui = true;
	cout << dui << endl;
	dui = false;
	cout << dui << endl;
	cout << sizeof(bool) << endl;

	system("pause");

	return 0;

}
>>>>>>> 4fa7ecb9e8258fc2ff093433a6b119d76b26aaed
