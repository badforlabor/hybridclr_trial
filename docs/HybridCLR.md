# HybridCLR问题整理



### 原理



### 限制

[限制和注意事项 | Focus Creative Games (focus-creative-games.github.io)](https://focus-creative-games.github.io/hybridclr/performance/limit/)

- AOT泛型
- 对于返回struct类型的async实现， 需要做上一条相似的AOT泛型处理
- 不支持delegate的BeginInvoke, EndInvoke
- 支持在资源中挂载热更新脚本，但需要在打包时做少量特殊处理
- 暂不支持增量式gc



### TODO

- 测试泛型
  - 值类型
  - 引用类型
- 测试绑在场景中的脚本，是否能热更新
  - 无法热更新。无法执行？
  - 譬如，在Main场景中，新建一个GO，并榜上PrintHello.cs脚本
  - 原因：是更新的dll，在打包时，会被过滤掉，官方有说此件事情。搜索OnFilterAssemblies，就能看到
- 测试绑在prefab上的脚本，是否能热更新
- 更新代码和资源->再加载所有的dll->在正常游戏
- 

