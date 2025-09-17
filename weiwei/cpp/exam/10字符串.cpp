#include <bits/stdc++.h>
using namespace std;

void xiao()
{
	cout<<"haha"<<endl;
}

int add(int a,int b)
{
	return a+b;
}
int chefa(int a,int b)
{
	return a*b;
}
//int xiab(string nr,string lm)
//{
//	for(int i=0;i<nr.size();i++)
//	{
//		int j=0;
//		for(;j<lm.size();j++)
//		{
//			if(lm[j]!=nr[i+j])
//			{
//				break;
//			}
//			else
//			{
//				cout<<j<<endl;
//			}
//		}
//	}
//}






















int ymy(string str1,string str2)
{
	for(int i=0;i<str1.size();i++)
	{
		int j=0;
		for(;j<str2.size();j++)
		{
			if(str2[j]!=str1[i+j])
			{
				break;
			}
		}
		if(j==str2.size())
		{
			return i;
		}
		
		
		
		
	}
}
int main()
{
	string str1="fasfsafbadfdafdasdlf";
	string str2="bad";
	//字符串实际上类似与字符组成的数组
	cout<<ymy(str1,str2)<<endl;
//	chr chr[]={'m','y',' ','n'}
//	string str1="my name is weiwei";

//	cout << str1[0]<<endl;
//	cout << str1[0]<<endl;
//	cout << str1.size()<<endl;
//	int sum=0;
//	for (int i=0; i<str1.size(); i++)
//	{
//		if (str1[i]=='i')
//		{
//			sum+=1;
//		}
//	}
//	cout<<sum<<endl;
//	cout <<str1<<endl;

	int a=32;
	xiao();
	int jg=add(5,a);
	cout<<jg<<endl;
	cout<<chefa(8,4)<<endl;
	return 0;
}






