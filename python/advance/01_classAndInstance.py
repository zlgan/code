class Student(object):
   def __init__(self,name,score):
       self.__name=name
       self.__score=score
   def print_score(self):
       print("%s:%s"%(self.__name,self.__score))
   def set_score(self,score):
       if 0<=score<=100:
           self.__score=score
       else:
           raise ValueError("bad score")
rr=Student("ruirui",100)
rr.__name="weiwei"
rr.set_score(66)
# rr.set_score(101)
rr.print_score()

#判断一个变量是否是某个类型可以用isinstance()判断
print(isinstance(rr,Student))


class Animal(object):
    def run(self):
        print("Animal is running...")

class Dog(Animal):
    def run(self):
        print("Dog is running...")
    def eat(self):
        print("Eating meat...")

class Cat(Animal):
    def run(self):
        print("Cat is running...")

def run_twice(animal):
    animal.run()
    animal.run()

dog=Dog()
dog.run()
cat=Cat()
cat.run()

print(isinstance(dog,Animal))
print(isinstance(dog,Dog))
print(isinstance(dog,object))

run_twice(Animal())
run_twice(Dog())
run_twice(Cat())   

class Tortoise():
    def run(self):
        print("Tortoise is running slowly...")

run_twice(Tortoise())


#获取对象信息
print(type(123))
print(type(abs))
print(type(rr))
#获得一个对象的所有属性和方法，可以使用dir()函数，它返回一个包含字符串的list
print(dir(rr))


