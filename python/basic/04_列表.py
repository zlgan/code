frout=["apple","paire","blanan"]

#遍历数组
print("begin")
for item in frout:
    print("\t",item)
print("end.")


#新增元素
frout.append("测试")
#删除元素
frout.remove("apple")
#排序
frout.sort()

#遍历数组--带索引
print("begin")
for i,item in  enumerate(frout):
    print("\t",i,item)
print("end.")


#统计
myList=[1,4,9,3,2,0]
print(max(myList)) #9
print(sum(myList)) #19
print(min(myList)) #0

#切片 myList[<from>:<to>:<step length>]
#获取从列表索引from 开始到to-1的元素集合，from默认为0，to的默认值为索引最大值
#<step length> 默认为1，可以不写
print(myList[0:3]) #1,4,9
print(myList[:3]) #同上
print(myList[2:4]) #9,3
print(myList[2:3]) #9
print(myList[2:]) #9,3,2,0
#-1表示最大索引-1的位置
print(myList[2:-1]) #9,3,2
print(myList[2:2]) #[] 非法的切片
print(myList[2:1]) #[] 非法的切片

#带有三个参数的切片
print(myList[1:5:2]) #[4, 3]
print(myList[1::3]) #[4, 2]

mySlice=myList[2:5]
#改变切片的内容，不影响原列表
mySlice.append("20")
print(myList) #[1, 4, 9, 3, 2, 0]
print(mySlice) #[9, 3, 2, '20']


