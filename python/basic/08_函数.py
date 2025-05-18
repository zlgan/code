#定义函数
def hello1():
    print("hello python")

#调用函数
hello1()

#定义个带默认值的函数，可以不传递该参数
def hello2(name="testname"):
    print("hello",name)

hello2("leo")
hello2()

#定义个参数不确定的函数，num相当于一个列表
def add(*num):
    result=0
    for item in num:
        result+=item
    return result

print(add(1,2,3,4)) #10
print(add(1,2,3,4,5,6,7,8,9)) #45

#定义一个项目不确定的字典参数(关键字参数)
def sendEmail(**data):
    for key,val in data.items():
        print(key,"=",val)

sendEmail(title="testTitle",to="aaa",cc="cclist")
