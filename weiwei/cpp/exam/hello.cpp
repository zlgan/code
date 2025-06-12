#include<bits/stdc++.h>
using namespace std;
int main()
{
	cout <<"请给这个电影打分："<<endl; 
	int a=0;
	cin>>a;
	
	switch(a)
	{
		case 10:
		case 9:
			cout << "您认为这是经典的电影"<< endl;
			break;
		case 8:
			cout << "您认为这是非常好的电影"<< endl;
			break;
		case 7:
		case 6:
			cout << "您认为这是普通的电影"<< endl;
			break;
		default:
			cout << "您认为这是烂电影"<< endl;
	}
	
	system("pause");
}

