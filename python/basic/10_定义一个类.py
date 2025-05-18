#定义一个类
class Player():
    #类的构造函数
    def __init__(self,name,age):
        super().__init__()
        self.name=name
        self.age=age
    def sayHello(self):
        print("Hello,my name is ",self.name,"and my age is ",self.age)
    def intro(self):
        print("I'am a player")

#实例化一个对象
player1= Player("tom",30)
player1.sayHello()


#类的继承
class nbaPlayer(Player):
    def __init__(self, name, age):
        #在构造函数中给父类的构造函数传递参数
        super().__init__(name, age)
        self.category="nba"
    def intro(self):
        print("I'am a ",self.category,"player")

kobe= nbaPlayer("kobe",40)
kobe.sayHello()
kobe.intro()