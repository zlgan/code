#include<bits/stdc++.h>
using namespace std;
int main()
{
	int n ; 
	int sum=0;
	freopen("sum.in","r",stdin); //read
	freopen("sum.out","w",stdout); //write
	cin >>n;
	for(int i=1;i<=n;i++) sum+=i;
	cout <<sum;
	fclose(stdin);
	fclose(stdout);
	
}
