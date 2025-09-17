#include<bits/stdc++.h>
using namespace std;
int main()
{
	int num=1000;
	do
	{
		int a=num%10;
		int b=(num/10)%10;
		int c=(num/100)%10;
		int d=num/1000;
		int z=a*a*a*a+b*b*b*b+c*c*c*c+d*d*d*d;
		if(z==num)
		{
			cout << num << endl;
		}
		num++;
	}while(num<10000);
}
