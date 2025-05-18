#引入外部模块，模块名就是文件名
import module.Func as Func  
print(Func.add(3,5))
#引入外部模块，并定义别名
import module.Func as myFunc
print(myFunc.add(3,5))