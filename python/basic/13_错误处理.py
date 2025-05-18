try:
    10/0
except ZeroDivisionError  as zdr:
    print("error")
else:
    print("ok")
finally:
    print("end")
print("---------------------")
try:
    10/10
except ZeroDivisionError  as zdr:
    print("error")
else:
    print("ok")
finally:
    print("end")    