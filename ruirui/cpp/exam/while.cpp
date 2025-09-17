#include<bits/stdc++.h>
using namespace std;
int main ()
{
//	int xh = 0;
//	while (xh <10)
//	{
//		cout << xh << endl;
//		xh++;
//	}
	srand(time(NULL));
	int num = rand() % 100+1;
	int num2 = 0;
	cout <<"请猜数"<<endl;
	while(1)
	{
		cin >> num2;
		if (num2 > num)
		{
			cout <<"你猜的数太大了"<<endl;
		}
		else if (num2 < num)
		{
			cout <<"你猜的数太小了"<<endl;
		}
		else
		{
			cout <<"恭喜你答对了"<<endl;
			break;
		}
	}

}
