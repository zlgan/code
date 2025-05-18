
01_hello.o:     file format pe-x86-64


Disassembly of section .text:

0000000000000000 <_Z3logPKc>:
   0:	55                   	push   %rbp
   1:	48 89 e5             	mov    %rsp,%rbp
   4:	48 83 ec 20          	sub    $0x20,%rsp
   8:	48 89 4d 10          	mov    %rcx,0x10(%rbp)
   c:	48 8b 45 10          	mov    0x10(%rbp),%rax
  10:	48 89 c2             	mov    %rax,%rdx
  13:	48 8b 05 00 00 00 00 	mov    0x0(%rip),%rax        # 1a <_Z3logPKc+0x1a>
  1a:	48 89 c1             	mov    %rax,%rcx
  1d:	e8 00 00 00 00       	call   22 <_Z3logPKc+0x22>
  22:	48 89 c1             	mov    %rax,%rcx
  25:	48 8b 05 00 00 00 00 	mov    0x0(%rip),%rax        # 2c <_Z3logPKc+0x2c>
  2c:	48 89 c2             	mov    %rax,%rdx
  2f:	e8 00 00 00 00       	call   34 <_Z3logPKc+0x34>
  34:	90                   	nop
  35:	48 83 c4 20          	add    $0x20,%rsp
  39:	5d                   	pop    %rbp
  3a:	c3                   	ret

000000000000003b <main>:
  3b:	55                   	push   %rbp
  3c:	48 89 e5             	mov    %rsp,%rbp
  3f:	48 83 ec 20          	sub    $0x20,%rsp
  43:	e8 00 00 00 00       	call   48 <main+0xd>
  48:	48 8d 05 00 00 00 00 	lea    0x0(%rip),%rax        # 4f <main+0x14>
  4f:	48 89 c1             	mov    %rax,%rcx
  52:	e8 a9 ff ff ff       	call   0 <_Z3logPKc>
  57:	b8 00 00 00 00       	mov    $0x0,%eax
  5c:	48 83 c4 20          	add    $0x20,%rsp
  60:	5d                   	pop    %rbp
  61:	c3                   	ret
  62:	90                   	nop
  63:	90                   	nop
  64:	90                   	nop
  65:	90                   	nop
  66:	90                   	nop
  67:	90                   	nop
  68:	90                   	nop
  69:	90                   	nop
  6a:	90                   	nop
  6b:	90                   	nop
  6c:	90                   	nop
  6d:	90                   	nop
  6e:	90                   	nop
  6f:	90                   	nop
