


#include <iostream>
#include <string>
using namespace std;
int main02()
{	//15输入代码cin<<
	int a = 0;
	cout << "请给变量a付一个值：" << endl;
	//cin >> a;
	cout << "变量a的值为" << a << endl;
	//********************************************
	//16+ - * / 
	int a1 = 10;
	float b = 3.1f;
	cout << a1 + b << endl;
	cout << a1 - b << endl;
	cout << a1 * b << endl;
	cout << a1 / b << endl;
	cout << 10 % 3 << endl;
	//17在取余中不可以有小数
	//cout << b % a1 << endl;
	//18********************************************
	int a2 = 10;
	int b2 = ++a2;//先给a2+1，再给b2赋值
	int b3 = a2++;//先给b2赋值，再给a2+1
	cout << "a2=" << a2 << endl;
	cout << "b3=" << b3 << endl;
	//19********************************************
	//a+=2,a-=2,a*=2,a/=2,a%=2
	int a3 = 10;
	a3 += 2; //12
	a3 -= 2; //8
	a3 *= 2; //20
	a3 /= 2; //5
	a3 %= 2; //0
	//20^********************************************
	int a4 = 10;
	int b4 = 20;
	cout << (a4 == b4) << endl;
	cout << (a4 != b4) << endl;
	cout << (a4 > b4) << endl;
	cout << (a4 < b4) << endl;
	cout << (a4 >= b4) << endl;
	cout << (a4 <= b4) << endl;
	//21~23******************************************
	//！真变假，假变真。
	int a5 = 10;
	cout << !a << endl;//0
	cout << !!a << endl;//1l
	//&& 一个是否，就是否
	int b5 = 10;
	cout << (a5 && b5) << endl;
	//|| 一个是真，就是真
	a5 = 0;
	cout << (a5 || b5) << endl;
	cout << (a5 || 0) << endl;




	system("pause");
	return 0;
}