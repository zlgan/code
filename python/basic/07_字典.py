db_config={
    "ip":"127.0.0.1",
    "port":80,
    "uid":"admin",
    "pwd":123456
}

print(db_config)

#新增字典项目
db_config["timeout"]=30
print(db_config)

#修改字典项目
db_config["timeout"]=60
print(db_config)

#删除字典项目
del db_config["timeout"]
print(db_config)



for k,v in db_config.items():
    print("{0}的值是{1}".format(k,v))

#遍历字典元素
for key,value in db_config.items():
    print(key,"=",value)

#遍历字典Key
for key in db_config.keys():
    print(key,"is",db_config[key])

#遍历value db_config.values()



