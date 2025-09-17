#include<bits/stdc++.h>
#include <typeinfo>
using namespace std;
int main()
{
	cout << typeid(7.8f/2).name()<<endl;
	 cout << int(3.14)<<endl;
	 cout << (int)3.14<<endl;
	  cout << (2, 3, "23")<<endl;
	  cout <<endl;
	int arr[]={1,3,5,6,12};
	int arr1[5];
	for(int i = 4;i>=0;i--)
	{
		arr1[4-i]=arr[i];
	}
	for(int j = 0;j<5;j++ )
	{
		cout << arr1[j] << endl;
	}
}
