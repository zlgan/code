s1="test1"
s2="test2"
print(s1+s2) #字符串拼接
print(s1,s2) #拼接，以空格分隔

msg="hello,wrold"
print(msg.title()) #首字母大写，其余小写 Hello,Wrold
print(msg.upper()) 
print(msg.lower())

#转义字符
msg2="Hello\nWrold" # 换行输出 \n表示换行

print(msg2)

msg3=r"Hello\nWrold" # r表示不转义，原样输出
print(msg3)

#删除空白字符
stripTest=" Hello World "
print(len(stripTest.lstrip())) #删除左边空白
print(len(stripTest.rstrip())) #删除右边空白
print(len(stripTest.strip())) #删除左右空白


