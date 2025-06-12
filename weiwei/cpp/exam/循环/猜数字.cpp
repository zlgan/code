#include<bits/stdc++.h> 
using namespace std;
int main()
{
	srand((unsigned int)time(NULL));
	int zhi=rand() % 100 +1;
	int kehu=0;
	while(1)
	{
		cout<<"请输入一个数来猜："<<endl;
		cin>>kehu;
		if(kehu>zhi)
		{
			cout<<"你猜的大了"<<endl; 
		}
		else if(kehu<zhi)
		{
			cout<<"你猜的小了"<<endl; 
		}
		else
		{
			cout<<"你猜的对了"<<endl;
			break; 
		}
	}
}
