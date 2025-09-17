#include<bits/stdc++.h>
using namespace std;
int main()
{
	int bi=0;
	cout<<"请输入小明要买的签字笔的数量（1≤X≤10）：";
	cin>>bi;
	int ben=0;
	cout<<"请输入小明要买的记事本的数量（1≤X≤10）：";
	cin>>ben;
	int ch=0;
	cout<<"请输入小明要买的尺子的数量（1≤X≤10）：";
	cin>>ch;
	int money=0;
	cout<<"请输入小明的钱：";
	cin>>money;
	int qian=bi*2+ben*5+ch*3;
	if(money>=qian)
	{
		cout<<"Yes"<<endl;
	}
	else
	{
		cout<<"No"<<endl;
		cout<<"缺了"<<qian-money<<"元"<<endl; 
	}
}
