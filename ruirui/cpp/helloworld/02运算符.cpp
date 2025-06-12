// 编译的时侯将iostream的内容拷贝到这里 
#include <iostream>
#include <string>
using namespace std;

int main02()
{
	//输入cin 输出cout
	/*int in = 0;
	cout << "请输入整数变量" << endl;
	cin >> in;
	cout << "整数a = " << in << endl;*/

	//加减乘除
	cout << "========加减乘除======" << endl;

	int a1 = 10;
	int a2 = 5;
	cout << a1 - a2 << endl;
	cout << a1 + a2 << endl;
	cout << a1 * a2 << endl;
	cout << a1 / a2 << endl;

	double dd1 = 10;
	float dd2 = 5.5f;
	cout << dd1 - dd2 << endl;
	cout << dd1 + dd2 << endl;
	cout << dd1 * dd2 << endl;
	cout << dd1 / dd2 << endl;

	//取模
	cout << "========取模======" << endl;

	int qm1 = 10;
	int qm2 = 3;
	cout << qm1 % qm2 << endl;

	//递增
	cout << "========递增 递减======" << endl;
	//递增1：前置递增先对变量进行+1(也就是++)，再计算表达式。
	int dz1 = 10;
	int dz2 = dz1++ * 10;
	cout << "递增的第一个" << dz1 << endl;
	cout << "递增的第二个" << dz2 << endl;
	//后置递增先计算表达式再计算变量进行+1(也就是++)
	dz1 = 10;
	dz2 = ++dz1 * 10;
	cout << "递增的第一个" << dz1 << endl;
	cout << "递增的第二个" << dz2 << endl;

	//递减
	
	//前置递增先对变量进行-1(也就是--)，再计算表达式。
	int dj1 = 10;
	int dj2 = dj1-- * 10;
	cout << "递减的第一个" << dj1 << endl;
	cout << "递减的第二个" << dj2 << endl;
	//后置递增先计算表达式再计算变量进行-1(也就是--)
	dj1 = 10;
	dj2 = --dj1 * 10;
	cout << "递减的第一个" << dj1 << endl;
	cout << "递减的第二个" << dj2 << endl;

	//赋值运算符
	cout << "========赋值运算符======" << endl;
	int fz = 10;
	fz = 100;
	cout << "fz = " << fz << endl;//100

	fz = 10;
	cout << "fz +=2=" << (fz += 2) << endl;//12

	fz = 10;
	cout << "fz -=2=" << (fz -= 2) << endl;//8

	fz = 10;
	cout << "fz *=2=" << (fz *= 2) << endl;//20

	fz = 10;
	cout << "fz /=2 = " << (fz /= 2) << endl;//5

	fz = 10;
	cout << "fz %=2 =" << (fz %= 2) << endl;//0

	//比较运算符
	cout << "========比较运算符======" << endl;
	int bj1 = 10;
	int bj2 = 20;

	cout << (bj1 == bj2) << endl;//0
	cout << (bj1 != bj2) << endl;//1
	cout << (bj1 < bj2) << endl;//1
	cout << (bj1 > bj2) << endl;//0
	cout << (bj1 <= bj2) << endl;//1
	cout << (bj1 >= bj2) << endl;//0

	//逻辑运算符
	cout << "========逻辑运算符======" << endl;
	int lj = 10;
	int lj2 = 0;
	cout << !lj << endl;//0
	cout << !!lj << endl;//1

	cout << (lj && lj2) << endl;//0
	cout << (lj || lj2) << endl;//1
	lj = 0;
	cout << (lj || lj2) << endl;//0

	system("pause");

	return 0;
}