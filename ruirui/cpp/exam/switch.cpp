#include<bits/stdc++.h>
using namespace std;
int main()
{
	cout << "电影，请打分："<<endl;
	int score = 0;
	cin >> score;
	switch(score)
	{
		case 10:
		case 9:
			cout <<"经典电影"<<endl;
			break;
		case 8:
		case 7:
			cout <<"不错电影"<<endl;
			break;
		default:
			cout <<"烂片电影"<<endl;
			break;
	}

}
