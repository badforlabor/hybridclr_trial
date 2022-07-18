# HybridCLR问题整理



### 原理

- IL2CPP的虚拟机？
- 或者是，运行C#dll的虚拟机？应该是这个。
- [HybridCLR技术原理剖析 - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/531468413)

#### 优点

- huatuo中热更新部分元数据与AOT元数据无缝统一。像反射代码能够正常工作的，AOT部分也可以通过标准Reflection接口创建出热更新对象
- huatuo执行效率高。huatuo中热更新部分与主工程AOT部分交互**属于il2cpp内部交互**，效率极高。而其他方案则是独立虚拟机与il2cpp之间的效率，不仅交互麻烦还效率低下。
- huatuo对多线程支持良好。像多线程、ThreadStatic、async等等特性都是huatuo直接支持，其他方案除了async特性外均难以支持。
- huatuo兼容性极高。各种第三方库只要在il2cpp下能工作，在huatuo下也能正常工作。其他方案往往要大量魔改源码。
- huatuo内存效率极高。huatuo中热更新类型与主工程的AOT类型完全等价，占用一样多的空间。其他方案的同等类型则是假类型，不仅不能被runtime识别，还多占了数倍空间。
- 支持反射、多线程



### 限制

[限制和注意事项 | Focus Creative Games (focus-creative-games.github.io)](https://focus-creative-games.github.io/hybridclr/performance/limit/)

- AOT泛型
  - 对于值类型的泛型，处理的不好。
    - 譬如，定义struct SDataC，然后使用Dictionary<string, CDataC>时，会报错“GetManaged2NativeMethodPointer. sinature:i1i88sri1 not support. System.Collections.Generic Dictionary"2:Trylnsert”

  - 对于引用类型，没啥问题。
    - 譬如，定义class CDataC，然后使用Dictionary<string, CDataC>时，不会报错

  - 值类型的数组，是支持的，譬如MyHotUpdateValueType[]
  - 解决方案，对于值类型的，手动展开
    - 是可以的。譬如定义struct SDataC，class Dict_string_SDataC，使用Dict_string_SDataC代替Dictionary<string, CDataC>，是可以更新的。

- 泛型函数，是否支持？
- 对于返回struct类型的async实现， 需要做上一条相似的AOT泛型处理
- 不支持delegate的BeginInvoke, EndInvoke
- 支持在资源中挂载热更新脚本，但需要在打包时做少量特殊处理
- 暂不支持增量式gc





### TODO

- 测试泛型
  - 值类型，可行的
  - 引用类型，可行的
- 测试绑在**初始场景**中的脚本，是否能热更新
  - 初始场景，指的是打在安装包中的场景
  - 无法热更新。无法执行？
  - 譬如，在Main场景中，新建一个GO，并榜上PrintHello.cs脚本
  - 原因：是更新的dll，在打包时，会被过滤掉，官方有说此件事情。搜索OnFilterAssemblies，就能看到
  - 譬如，在Scene2中，新建一个GO，并榜上PrintHello.cs脚本
    - Main场景，启动，并加载完所有的dll
    - Main场景，有个按钮，能切换到Scene2
    - 点击切换，发现在Scene2中的PrintHello.cs也是丢失的。
  - 总结，Resource中代码，是无法热更新的。
- 测试绑在prefab上的脚本，是否能热更新
  - 猜测，如果是先加载dll，在加载prefab，应该是可以的。

- 测试绑在场景中的脚本，是否能热更新
  - 猜测，如果是先加载dll，在加载场景，应该是可以的

- 更新代码和资源->再加载所有的dll->再正常游戏
- 桥接函数，是不是没办法热更新？[AOT-interpreter桥接函数 | Focus Creative Games (focus-creative-games.github.io)](https://focus-creative-games.github.io/hybridclr/performance/method_bridge/)
  - 测试过程中，在安卓上，遇到了，提示ExecutionEngineException: GetManaged2NativeMethodPointer
  - 先在PrepareCustomMethodSignatures中，加入缺少的signature，譬如C56i8i8，注意，区分大小写
  - 然后，执行HybridCLR/MethodBridge/Generate_Arm64和HybridCLR/MethodBridge/Generate_Armv7
  - 然后将文件MethodBridge_arm64.cpp和MethodBridge_armv7.cpp，从HybridCLRData\LocalIl2CppData\il2cpp\libil2cpp\huatuo\interpreter拷贝到HybridCLRData\hybridclr_repo\huatuo\interpreter中
  - 发现，如果是List，就不会报错；Dictionary，就会报错。
  - **TODO**。手动展开泛型类和泛型函数。



### 程序设计

1. 实现自己的List，HashSet，Dictionary
2. 检查代码，看是否有值类型的容器。
   1. 读取dll，读取struct，读取List、set、dictionary，看是否有struct的，如果有，且并不是用自己的List的，那么抛异常
3. 读取配置文件，并生成值类型对应的容器，譬如Dictionary_int_ValueType等
   1. 没必要必须实现值类型的所有容器，所以可配的，反而更好
   2. 生成在单独的文件夹
   3. 可配在属性上？
4. 
