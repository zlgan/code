#include <bits/stdc++.h>
using namespace std;
int main()
{
	int num = 1000;
	int xj = 0;
	do
	{
		int g = 0;
		int s = 0;
		int b = 0;
		int q = 0;
		g = num%10;
		s = num/10%10;
		b = num/100%10;
		q = num/1000;
		xj = g*g*g*g+s*s*s*s+b*b*b*b+q*q*q*q;
		if (xj==num)
		{
			cout <<"Ë®ÏÉ»¨Êı£º"<< num << endl;
		}
		num++;
	}while(num<10000);

}
