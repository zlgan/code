
#include <iostream>
#include <string>
using namespace std;
int main()
{
	int num1 = 0;
	int num2 = 0;
	int num3 = 0;
	int n = 0;
	cout << "请输入第一只的体重：" << endl;
	cin >> num1;
	cout << "请输入第二只的体重：" << endl;
	cin >> num2;
	cout << "请输入第三只的体重：" << endl;
	cin >> num3;

	/*if (num1 > num2)
	{
		if (num1 > num3)
		{
			cout << "第一只小猪最重" << endl;
		}
		else
		{
			cout << "第三只小猪最重" << endl;
		}
	}
	else
	{
		if (num2 > num3)
		{
			cout << "第二只小猪最重" << endl;
		}
		else
		{
			cout << "第三只小猪最重" << endl;
		}
	}*/
	//a>b?a:b
	n=num1 > num2 ? num1 : num2;
	n=n > num3 ? n : num3;
	cout << n << endl;
	system("pause");
	return 0;
}