#include<bits/stdc++.h>
using namespace std;
int main()
{
	int k=0;
	cout<<"请输入你的幸运数：";
	cin>>k;
	int i=0;
	cout<<"请输入你想从几开始："; 
	cin>>i;
	int w=0;
	cout<<"请输入你想从几结束：";
	cin>>w;
	int z=0;
	for(;i<w;i++)
	{
		if(i%k==0)
		{
			z+=i;
		}
		else if((i%10)==7)
		{
			z+=i;
		}
	}
	cout<<z<<endl; 
}
