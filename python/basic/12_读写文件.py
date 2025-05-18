  
#定义文件路径
filepath ="files/test.txt"
#以写模式打开文件，并指定文件句柄
with open(filepath,"w", encoding= "utf-8") as myFile:
    myFile.write("hello\n")
    myFile.write("python\n")
    myFile.write("world\n")
with open(filepath,'r',encoding="utf-8") as myFile:
    print(myFile.read())