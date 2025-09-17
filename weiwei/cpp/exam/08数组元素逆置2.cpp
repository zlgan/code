#include<bits/stdc++.h>
using namespace std;
int main()
{
	int arr[]={1,5,7,3,2,4};
	int start=0;
	int end=sizeof(arr)/sizeof(arr[0])-1;
	int temp=0;
	while(end>start)
	{
		temp=arr[start];
		arr[start]=arr[end];
		arr[end]=temp;
		start++;
		end--;
	}
	for(int i=0;i<sizeof(arr)/sizeof(arr[0]);i++)
	{
		cout << arr[i] <<", ";
	}
	int cout=2;
	cout<<cout<<endl;
}
