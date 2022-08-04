# HybridCLR问题整理



### 原理

- [HybridCLR技术原理剖析 - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/531468413)
- 运行C#的虚拟机，区别于ILRuntime的是，是和il2cpp混合在一起的，是没有穿透的。

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
  - 对于值类型的**泛型容器**，处理的不好。
    - 譬如，定义struct SDataC，然后使用Dictionary<string, CDataC>时，会报错“GetManaged2NativeMethodPointer. sinature:i1i88sri1 not support. System.Collections.Generic Dictionary"2:Trylnsert”

  - 对于引用类型，没啥问题。
    - 譬如，定义class CDataC，然后使用Dictionary<string, CDataC>时，不会报错

  - 值类型的数组，是支持的，譬如MyHotUpdateValueType[]
  - **解决方案**，对于值类型，使用**自己定义的泛型容器**，且让此泛型容器，也是**解释执行**就可以了。
  - 值类型的，AOT泛型delegate，支持，譬如System.Action<SDataB>
  - 值类型的，泛型Tuple，不支持，譬如Tuple<float, int, string>，譬如Tuple<SDataB, int>
- 热更新dll中，泛型函数，是否支持？
  - 泛型函数，支持。譬如 public static void Clone<T>(in T src, ref T dst)，是可以的
- 热更新dll中，泛型类，是否支持？
  - 支持。
- 对于返回struct类型的async实现， 需要做上一条相似的AOT泛型处理
- 不支持delegate的BeginInvoke, EndInvoke
- 支持在资源中挂载热更新脚本，但需要在打包时做少量特殊处理
- 暂不支持增量式gc
- **external函数，需要放到非热更部分**。譬如xlua.so
  -  [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
  - 放在ExternalLib工程中。
- 如果是**向C++中传递C#的的delegate函数，应该放在非热更**部分。
  - 检查所有Marshal.GetFunctionPointerForDelegate关键字修饰的
  - 放在ExternalLib工程中。
  - 譬如xlua的回调函数CSharpWrapperCallerImpl
- AOT部分，如果有静态代理函数指针，热更新部分，是无法直接赋值的，否则，会运行时崩溃。
  - 譬如，热更新部分有：public static Callback ImportType_Callback;
  - 非热更新部分，如果这样设置StaticLuaCallbacksAPI.ImportType_Callback = ImportType;
  - 执行ImportType_Callback()，会导致崩溃
- 非AOT的代码，遇到了，Directory<Struct, xxx>，居然会崩溃，所有用结构体的，都需要查一下。
  - 用CrazyDirectory代替，大部分是可行的，少量，就得加桥接函数了
- 代码中，如果有TupleValue类型的，应该去注册下，在RefTypes.cs中。注册譬如这些：
  - new Dictionary<DefaultEnum, int>()
  - new HashSet<DefaultEnum>()
  - new ValueTuple<int, string>()
  - new List<ValueTuple<int, string>>()
- Array.IndexOf<T>，也会引起异常：GetManaged2NativeMethodPointer. sinature:i4i8i16i4i4 not support.. System.Array::IndexOfImpl
- 代理函数，会引起崩溃：ExecutionEngineException: GetManaged2NativeMethodPointer. sinature:vi4vf3i1 not support.. System.Action`3::Invoke
  - 应该是：System.Action<int, Vector3, bool>.invoke()
- Async会引起崩溃，如何处理？编译的热更脚本，使用dev模式
  - scriptCompilationSettings.options = ScriptCompilationOptions.DevelopmentBuild;
- 如果所有代码，都放在AssemblyCSharp，变成解释执行，性能估计有影响。
  - 建议，各个模块，尽量用asmdef定义，拆分dll；






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
  - （不需要拷贝）然后将文件MethodBridge_arm64.cpp和MethodBridge_armv7.cpp，从HybridCLRData\LocalIl2CppData\il2cpp\libil2cpp\huatuo\interpreter拷贝到HybridCLRData\hybridclr_repo\huatuo\interpreter中
  - 发现，如果是List，就不会报错；Dictionary，就会报错。
  - **TODO**。手动展开泛型类和泛型函数。
- 各种Strip引起的问题。由于dll都热更新了，所以在编译的时候，是查不到热更dll的依赖的，所以导致很多模块会strip掉。譬如UnityEngine.AssetBundleModule
  - 提供工具，检索依赖，并生成到link.xml中
- 启动Bugly会闪退
- 检索DLL中的所有struct类型的数据。
- 检索dll中，是否使用dictionary，改成CrazyDictionary
- 在lua中，执行某个C#函数时，崩溃了，期望能打出在哪个函数中崩溃。
  - 譬如，在PushCSharpWrapper时，记录下函数名字
  - 在CallCSharpWrapper时，打印函数名字
- 找到引擎的所有struct类型，提前做好泛型注册，譬如，只注册Dictionary和List即可。
- 提前找到自己项目的struct，检查是否用了System.List等
- 测试协程，是否有问题
- 测试**内存使用**情况。
- 优化性能，优化加减乘除。资料：
  - [基于栈实现的虚拟机 & 基于寄存器实现虚拟机 - 简书 (jianshu.com)](https://www.jianshu.com/p/beb2de57e3c3)
  - [【转】解释器，树遍历解释器，基于栈与基于寄存器 - DKMP - 博客园 (cnblogs.com)](https://www.cnblogs.com/budapeng/p/5537781.html)
  - 




### 性能对比

#### 资料

- [Lua VS C# benchmarks, Which programming language or compiler is faster (programming-language-benchmarks.vercel.app)](https://programming-language-benchmarks.vercel.app/lua-vs-csharp)
- [DNS/benchmark-language: BENCHMARK: Lua vs vs LuaJIT vs C (MSVC, GCC, LLVM) vs Java vs Perl vs Javascript vs Python vs C# (.NET CLR, Mono) vs Ruby vs R (github.com)](https://github.com/DNS/benchmark-language)

#### 自己的对比

8K的战斗回放，用mumu模拟器。

- 使用huatuo解释执行，2000ms
- 使用il2cpp，1000ms



### 程序设计

1. 实现自己的List，HashSet，Dictionary
2. 检查代码，看是否有值类型的容器。
   1. 读取dll，读取struct，读取List、set、dictionary，看是否有struct的，如果有，且并不是用自己的List的，那么抛异常
3. 读取配置文件，并生成值类型对应的容器，譬如Dictionary_int_ValueType等
   1. 没必要必须实现值类型的所有容器，所以可配的，反而更好
   2. 生成在单独的文件夹
   3. 可配在属性上？



### 遇到的各种情况

```
2022-07-26 16:42:41.685 30917-30956/com.b3.gameclient E/Unity: LuaException: c# exception:System.InvalidOperationException: Collection was modified; enumeration operation may not execute.
      at System.Collections.Generic.List`1+Enumerator[T].MoveNextRare () [0x00000] in <00000000000000000000000000000000>:0 
      at System.Collections.Generic.List`1+Enumerator[T].MoveNext () [0x00000] in <00000000000000000000000000000000>:0 
      at Timing.Game.LuaBindingMono.GetBindingComponents () [0x00000] in <00000000000000000000000000000000>:0 
      at XLua.ObjectTranslator.TimingGameLuaBindingMono_m_GetBindingComponents (System.IntPtr L, System.Int32 gen_param_count) [0x00000] in <00000000000000000000000000000000>:0 
      at XLua.ObjectTranslator.CallCSharpWrapper (System.IntPtr L, System.Int32 funcidx, System.Int32 top) [0x00000] in <00000000000000000000000000000000>:0 
      at XLua.StaticLuaCallbacks.CSharpWrapperCallerImpl (System.IntPtr L, System.Int32 funcidx, System.Int32 top) [0x00000] in <00000000000000000000000000000000>:0 
      at XLua.StaticLuaCallbacksObj.CSharpWrapperCallerImpl (System.I
```

```
2022-07-27 10:51:27.919 16165-16284/com.b3.gameclient E/Unity: LuaException: c# exception:System.ExecutionEngineException: GetManaged2NativeMethodPointer. sinature:i4i8i16i4i4 not support.. System.Array::IndexOfImpl
      at Timing.Battle.MapGridMgr.UpdateNeighbours (Timing.HexGrid.HexTileData tileData) [0x00000] in <00000000000000000000000000000000>:0 
      at Timing.Battle.Map.CreateHexTile () [0x00000] in <00000000000000000000000000000000>:0 
      at Timing.Battle.Map.CreateMap (Google.Protobuf.Collections.RepeatedField`1[T] players) [0x00000] in <00000000000000000000000000000000>:0 
      at Timing.Battle.BattleWorld.PrepareBattle (SCMatchResult data, System.Boolean enableSync) [0x00000] in <00000000000000000000000000000000>:0 
      at Timing.Battle.BattleLogic.PrepareBattle (SCMatchResult data) [0x00000] in <00000000000000000000000000000000>:0 
      at Timing.Game.BattleLogicMgr.PrepareBattle (System.Int32 mode, System.UInt64 owner, System.UInt64 look, System.Byte[] matchInfoData) [0x00000] in <00000000000000000000000000000000>:0 
      at XLua.ObjectTranslator.TimingGameBattleLogicMgr_m_
```

```
2022-07-27 12:20:25.559 18754-18861/com.b3.gameclient E/Unity: ExecutionEngineException: GetManaged2NativeMethodPointer. sinature:vi4vf3i1 not support.. System.Action`3::Invoke
      at Timing.Game.BattleSystem.DispatchEvent[T1,T2,T3] (System.Int32 key, T1 arg1, T2 arg2, T3 arg3) [0x00000] in <00000000000000000000000000000000>:0 
      at Timing.Game.BattleSystem.EntityPosChange (System.Int32 eid, System.Int32 hexEid, System.Boolean immediate, System.Boolean forRealTime) [0x00000] in <00000000000000000000000000000000>:0 
      at Timing.Game.BattleSystem.SyncEntityPos (SyncEntityPos body, System.Boolean forRealTime) [0x00000] in <00000000000000000000000000000000>:0 
      at Timing.Game.SyncHandler.HandleSyncEntityPos (SyncMsg msg, System.Boolean forRealTime) [0x00000] in <00000000000000000000000000000000>:0 
      at Timing.Game.SyncMsgHandler.HandleMsg (SyncMsg msg, System.Boolean realTime) [0x00000] in <00000000000000000000000000000000>:0 
      at Timing.Game.BattleSyncMgr.NormalUpdate (System.Single deltaTime) [0x00000] in <00000000000000000000000000000000>:0 
      at Timing.Game.BattleSyncM
```

```
2022-07-27 18:40:14.971 27264-27518/com.b3.gameclient E/Unity: NullReferenceException: Object reference not set to an instance of an object.
      at System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitOnCompleted[TAwaiter,TStateMachine] (TAwaiter& awaiter, TStateMachine& stateMachine) [0x00000] in <00000000000000000000000000000000>:0 
      at Timing.Game.InputMgr.DoQueryCommandGetMovePath (System.Int32 startEid, System.Int32 selectEid, System.Action`1[T] callback) [0x00000] in <00000000000000000000000000000000>:0 
      at System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start[TStateMachine] (TStateMachine& stateMachine) [0x00000] in <00000000000000000000000000000000>:0 
      at Timing.Game.InputMgr.DoQueryCommandGetMovePath (System.Int32 startEid, System.Int32 selectEid, System.Action`1[T] callback) [0x00000] in <00000000000000000000000000000000>:0 
      at Timing.Render.MonoPlayer.SetTileDragTo (System.Int32 toTile) [0x00000] in <00000000000000000000000000000000>:0 
      at Timing.Render.MonoPlayer.SyncDragMsg (SyncDragMsg msg) [0x00000] in <0000000000000000000000000000000

```





## 性能测试

### 环境1

设备：**晓龙625**

环境：北叁战旗527版本，测试一场逻辑帧执行总时间

对比环境为：il2cpp和huatuo两种情况

#### 情况1，207个逻辑帧

observer_bin_1658995283

1. huatuo，耗费2922ms，statushash=715074174965
2. il2cpp，耗费1339ms，statushash=715074174965
3. 结论il2cpp=2.18倍huatuo

#### 情况2，1220个逻辑帧

observer_bin_1659336429

1. huatuo，耗费21307ms，statushash=909475774144
2. il2cpp，耗费4556ms ，statushash=909475774144
3. 结论il2cpp=4.67倍huatuo

#### 情况3，621个逻辑帧

observer_bin_1659326280

1. huatuo，耗费5279ms，statushash=856709496935
2. il2cpp，耗费 1186ms，statushash= 856709496935
3. 结论il2cpp=4.45倍huatuo

#### 情况4，415个逻辑帧

observer_bin_1659335867

1. huatuo，耗费1653ms，statushash=690162789156
2. il2cpp，耗费 493ms，statushash= 690162789156
3. 结论il2cpp=3.35倍huatuo



### 环境2

设备：**晓龙855（小米9）**

环境：北叁战旗527版本，测试一场逻辑帧执行总时间

对比环境为：il2cpp和huatuo两种情况

#### 情况1，207个逻辑帧

observer_bin_1658995283

1. huatuo，耗费1349ms，statushash=715074174965
2. il2cpp，耗费839ms，statushash=715074174965
3. 结论il2cpp=1.60倍huatuo

#### 情况2，1220个逻辑帧

observer_bin_1659336429

1. huatuo，耗费5673ms，statushash=909475774144
2. il2cpp，耗费2711ms ，statushash=909475774144
3. 结论il2cpp=2.09倍huatuo

#### 情况3，621个逻辑帧

observer_bin_1659326280

1. huatuo，耗费1429ms，statushash=856709496935
2. il2cpp，耗费 674ms，statushash= 856709496935
3. 结论il2cpp=2.12倍huatuo

#### 情况4，415个逻辑帧

observer_bin_1659335867

1. huatuo，耗费608ms，statushash=690162789156
2. il2cpp，耗费 308ms，statushash= 690162789156
3. 结论il2cpp=1.97倍huatuo



### 环境3

[Programming-Language-Benchmarks/bench/algorithm/spectral-norm at main · hanabi1224/Programming-Language-Benchmarks (github.com)](https://github.com/hanabi1224/Programming-Language-Benchmarks/tree/main/bench/algorithm/spectral-norm)

使用**spectral-norm**算法，对比测试lua, il2cpp, c#解释执行情况

-----------------------------------------------------------------------------------------

huatuo.apk

设备：**晓龙855（小米9）**

lua testSpectral, N=2000, ret=1.274224, cost=9181ms

c#  testSpectral, N=2000, ret=1.274224, cost=64549ms

设备：**晓龙625**

lua testSpectral, N=2000, ret=1.274224, cost=49801ms

c#  testSpectral, N=2000, ret=1.274224, cost=293730ms

-----------------------------------------------------------------------------------------

il2cpp.apk

设备：**晓龙855（小米9）**

lua testSpectral, N=2000, ret=1.274224, cost=9102ms

c#  testSpectral, N=2000, ret=1.274224, cost=12593ms

设备：**晓龙625**

lua testSpectral, N=2000, ret=1.274224, cost=49904ms

c#  testSpectral, N=2000, ret=1.274224, cost=30029ms



### 环境4

对比测试所有基础操作，譬如加减乘除、数学运算等

仅对比了huatuo和xlua。

设备：**晓龙855（小米9）**

总结：huatuo比xlua差最多2倍

```
N=21474836, a=1813836300.01998, b=1404737454.31521

c# AddTest, c=3218573754.33519, cost=1541ms
c# SubTest, c=409098845.70477, cost=1550ms
c# MulTest, c=2.54796378663459E+18, cost=1472ms
c# DivTest, c=1.29122797605208, cost=1501ms
c# ModTest, c=409098845.70477, cost=1688ms
c# UnmTest, c=-1813836300.01998, cost=1330ms
c# SqrtTest, c=42589.1570710196, cost=1688ms
c# CeilTest, c=1813836301, cost=1676ms
c# AbsTest, c=1813836300.01998, cost=1679ms
c# SinTest, c=0.626193018652133, cost=4376ms
c# CosTest, c=0.779668072574047, cost=4329ms
c# TanTest, c=0.803153342658727, cost=4745ms
c# AcosTest, c=NaN, cost=1772ms
c# AtanTest, c=1.57079632624358, cost=2124ms
c# Atan2Test, c=0.911825853812368, cost=2455ms

Lua AddTest, c=3218573754.33519, cost=931ms
Lua SubTest, c=409098845.70477, cost=928ms
Lua MulTest, c=2.54796378663459E+18, cost=930ms
Lua DivTest, c=1.29122797605208, cost=933ms
Lua ModTest, c=409098845.70477, cost=1128ms
Lua UnmTest, c=-1813836300.01998, cost=895ms
Lua SqrtTest, c=42589.1570710196, cost=1927ms
Lua CeilTest, c=1813836301, cost=1881ms
Lua AbsTest, c=1813836300.01998, cost=1844ms
Lua SinTest, c=0.626193018652133, cost=4599ms
Lua CosTest, c=0.779668072574047, cost=4538ms
Lua TanTest, c=0.803153342658727, cost=5177ms
Lua AcosTest, c=NaN, cost=1848ms
Lua AtanTest, c=1.57079632624358, cost=2584ms
Lua Atan2Test, c=0.911825853812368, cost=2678ms


il2cpp c# AddTest, c=3218573754.33519, cost=148ms
il2cpp c# SubTest, c=409098845.70477, cost=152ms
il2cpp c# MulTest, c=2.54796378663459E+18, cost=146ms
il2cpp c# DivTest, c=1.29122797605208, cost=110ms
il2cpp c# ModTest, c=409098845.70477, cost=318ms
il2cpp c# UnmTest, c=-1813836300.01998, cost=112ms
il2cpp c# SqrtTest, c=42589.1570710196, cost=198ms
il2cpp c# CeilTest, c=1813836301, cost=169ms
il2cpp c# AbsTest, c=1813836300.01998, cost=166ms
il2cpp c# SinTest, c=0.626193018652133, cost=2993ms
il2cpp c# CosTest, c=0.779668072574047, cost=2937ms
il2cpp c# TanTest, c=0.803153342658727, cost=3432ms
il2cpp c# AcosTest, c=NaN, cost=284ms
il2cpp c# AtanTest, c=1.57079632624358, cost=782ms
il2cpp c# Atan2Test, c=0.911825853812368, cost=962ms
```

设备：**晓龙625**

总结：huatuo比xlua差最多2.3倍

```
c# AddTest, c=3218573754.33519, cost=8542ms
c# SubTest, c=409098845.70477, cost=8712ms
c# MulTest, c=2.54796378663459E+18, cost=8714ms
c# DivTest, c=1.29122797605208, cost=9011ms
c# ModTest, c=409098845.70477, cost=9345ms
c# UnmTest, c=-1813836300.01998, cost=7779ms
c# SqrtTest, c=42589.1570710196, cost=9950ms
c# CeilTest, c=1813836301, cost=9766ms
c# AbsTest, c=1813836300.01998, cost=9768ms
c# SinTest, c=0.626193018652133, cost=21941ms
c# CosTest, c=0.779668072574047, cost=21878ms
c# TanTest, c=0.803153342658727, cost=23339ms
c# AcosTest, c=NaN, cost=10219ms
c# AtanTest, c=1.57079632624358, cost=11941ms
c# Atan2Test, c=0.911825853812368, cost=13742ms


Lua AddTest, c=3218573754.33519, cost=3698ms
Lua SubTest, c=409098845.70477, cost=3696ms
Lua MulTest, c=2.54796378663459E+18, cost=3698ms
Lua DivTest, c=1.29122797605208, cost=3927ms
Lua ModTest, c=409098845.70477, cost=4727ms
Lua UnmTest, c=-1813836300.01998, cost=3608ms
Lua SqrtTest, c=42589.1570710196, cost=8384ms
Lua CeilTest, c=1813836301, cost=8166ms
Lua AbsTest, c=1813836300.01998, cost=7985ms
Lua SinTest, c=0.626193018652133, cost=19805ms
Lua CosTest, c=0.779668072574047, cost=19695ms
Lua TanTest, c=0.803153342658727, cost=20882ms
Lua AcosTest, c=NaN, cost=8006ms
Lua AtanTest, c=1.57079632624358, cost=11687ms
Lua Atan2Test, c=0.911825853812368, cost=11782ms


il2cpp c# AddTest, c=3218573754.33519, cost=492ms
il2cpp c# SubTest, c=409098845.70477, cost=484ms
il2cpp c# MulTest, c=2.54796378663459E+18, cost=484ms
il2cpp c# DivTest, c=1.29122797605208, cost=615ms
il2cpp c# ModTest, c=409098845.70477, cost=1044ms
il2cpp c# UnmTest, c=-1813836300.01998, cost=396ms
il2cpp c# SqrtTest, c=42589.1570710196, cost=825ms
il2cpp c# CeilTest, c=1813836301, cost=627ms
il2cpp c# AbsTest, c=1813836300.01998, cost=627ms
il2cpp c# SinTest, c=0.626193018652133, cost=12433ms
il2cpp c# CosTest, c=0.779668072574047, cost=12368ms
il2cpp c# TanTest, c=0.803153342658727, cost=14053ms
il2cpp c# AcosTest, c=NaN, cost=990ms
il2cpp c# AtanTest, c=1.57079632624358, cost=2573ms
il2cpp c# Atan2Test, c=0.911825853812368, cost=3291ms
```

