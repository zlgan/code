#include <bits/stdc++.h>
using namespace std;
bool sfhws(int a)
{
	int b=0;
	while (a>b)
	{
		int n=a%10;
		a=a/10;
		b=b*10+n;
	}
	return (b/10==a||b==a)?true:false;
}
int main()
{
	cout <<sfhws(123321)<<endl;
	cout <<sfhws(12321)<<endl;
	cout <<sfhws(1233251)<<endl;
	
}
