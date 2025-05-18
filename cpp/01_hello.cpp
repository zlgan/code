/*
    1. 预处理
    g++ -E 01_hello.cpp -o 01_hello.i
    2. 编译
    g++ -c 01_hello.cpp -o 01_hello.o 
    3. 汇编
    objdump -d -S 01_hello.o > disassembly.asm
*/
#include "log.h"
#define PI 3.14
int main()
{
    log("Hello World!");
    return 0;
    int r=10
    log("mj=" + PI * r * r);
}