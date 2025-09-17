#include<bits/stdc++.h>
using namespace std;
int main()
{
	int arr[] = {300,500,1000,350,200,400,250};
	int max = arr[0];
	int count = sizeof(arr)/sizeof(arr[0]);
	for(int i = 0;i<count;i++)
	{
		max=arr[i]>max?arr[i]:max;
	}
    cout << max << endl;
}
