#include<iostream>
using namespace std;
class Player {
public:
	int x, y;
	int speed=1;
};


void Move(Player& player,int xa,int ya)
{
	player.x += xa * player.speed;
	player.y += ya * player.speed;
}


void tanxin()
{
	int zhifu = 169;
	int mianzhi[] = {20,10,5,1};
	int zhanghshu = 0;
	for (int i = 0;i < 4;i++)
	{
		if (zhifu > mianzhi[i]) {
			int linshi= zhifu / mianzhi[i];
			zhanghshu += linshi;
			zhifu = zhifu % mianzhi[i];

			cout << "面值=" << mianzhi[i] << " 共" << linshi << "张" << endl;

			if (zhifu == 0) {
				break;
			}
		}
	}

	cout << zhanghshu << endl;
	
}
int main()
{
	tanxin();
	system("pause");
	return 0;
}



