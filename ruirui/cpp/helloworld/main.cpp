#include <iostream>
#include <string>

using namespace std;
int main()
{
 	cout <<"hello world"<<endl;
	int pig1 = 0;
	int pig2 = 0;
	int pig3 = 0;
	cout << "请输入第一个小猪的体重" << endl;
	cin >> pig1;
	cout << "请输入第二个小猪的体重" << endl;
	cin >> pig2;
	cout << "请输入第三个小猪的体重" << endl;
	cin >> pig3;
	//if (pig1 > pig2)
	//{
	//	if (pig1 > pig3)
	//	{
	//		cout << "第一只小猪最重" << endl;
	//	}
	//	else
	//	{
	//		cout << "第三只小猪最重" << endl;

	//	}

	//}
	//else
	//{
	//	if (pig2 > pig3)
	//	{
	//		cout << "第二只小猪最重" << endl;
	//	}
	//	else
	//	{
	//		cout << "第三只小猪最重" << endl;
	//	}
	//}   

	int i = (pig1 > pig2 ? pig1 : pig2);
	i = (i > pig3 ? i : pig3);
	cout << "最重的是" << i << endl;
}
	system("pause");
	return 0;

}	
