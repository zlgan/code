#include<bits/stdc++.h>
using namespace std;
int main()
{
	for(int i=1;i<100;i++)
	{
		int g = i%10;
		int s = i/10; 
		if (g==7 || s==7 || i%7==0)
		{
			cout<<"ÇÃ×À×Ó"<<endl;
		} 
		else
		{
			cout<<i<<endl;
		}
	}
}
