myRange=range(5)
#该函数生成的是一个range对象
print(myRange) #range(0, 5)
myList=list(myRange) #[0, 1, 2, 3, 4]

#range对象转列表
print(list(myRange))

#生成指定起始值，和指定步长的range对象
myRange2=range(1,11,2)
print(list(myRange2)) #[1, 3, 5, 7, 9]

#range对象的遍历，同列表
for item in myRange2:
    print(item)

#range的统计信息
print("test",myRange2[1:3])