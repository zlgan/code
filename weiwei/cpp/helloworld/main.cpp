// 编译的时侯将iostream的内容拷贝到这里 
#include <iostream>
// i input 
// o output 
//stream 流水
//引入命名空间， 简化cout的写法
using namespace std;
//编译的时候代码中的常量名会被替换成值
#define pi 3.1415
#define myname "weiwei"

int main()
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
	system("pause");
	return 0;
}