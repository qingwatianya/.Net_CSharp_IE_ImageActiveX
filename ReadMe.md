# 如何采用C#为IE浏览器开发ActiveX插件(组件)
------------------------------------------
## 什么是ActiveX插件？
**ActiveX(COM)是一种嵌入式技术，是OLE和OCX技术的融合，是一种更高级的形态；**

## ActiveX插件用处？
**主要用于浏览器与客户端电脑本地系统交互，可以作为对浏览器功能不足之处的弥补；**
**比如：浏览器无法直接访问客户端电脑文件；有的功能对性能要求较高；调用系统底层功能...**

## ActiveX插件限制？
**主要适合IE浏览器，其他的浏览器不兼容，出于安全考虑现在的浏览器基本上都限制了ActiveX插件功能**

## 项目结构：
+ ImageActiveX（dll插件功能项目）：
   1. 项目右击应用程序->属性->应用程序->程序集信息->然后选中“使程序集COM可见”，如下图所示：
   2. 项目右击应用程序->属性->生成->然后选中“为COM互操作注册”
   3. 注意：Main.class 顶部的[Guid("E52C3E70-B486-4743-B8AE-5D830A561442")] 用于插件注册标识 可以自己创建
   4. 只有基础的3个功能代码，如果需要其他方法可自行扩展
+ ImageActiveXSetUp(插件安装包项目)：
	1. 菜单扩展->管理扩展->搜索安装：Microsoft Visual Studio Installer Projects
	2. 新建项目选择：“SetUp Project” 或者 “安装项目”；
	3. 项目右键->添加(Add)->项目输出->项目：选择ImageActiveX插件项目	
+ 测试网页：
	1. MyActiveXTest.html:用于测试ActiveX功能
+ 截图：
	1. ImageActiveX、ImageActiveXSetUp 项目核心配置截图（插件项目_设置_01.png、插件项目_设置_02.png 、安装包项目_设置_01.png、安装包项目_设置_02.png、安装包项目_设置_03.png）
	2. 最终效果截图（最终结果.png）
+ 插件安装包：
	1. ImageActiveXSetUp编译的插件安装包 可直接安装用于测试使用

## 插件功能测试：
1. 解压ImageActiveXSetUp.zip
2. 安装Setup.exe->选择“任何人”
3. 用IE浏览器打开MyActiveXTest.html 点击功能按钮测试

## 最终测试效果：
![IE调用ActiveX测试效果](https://github.com/qingwatianya/.Net_CSharp_IE_ImageActiveX/blob/master/ImageActiveX/%E6%88%AA%E5%9B%BE/%E6%9C%80%E7%BB%88%E7%BB%93%E6%9E%9C.png "测试")