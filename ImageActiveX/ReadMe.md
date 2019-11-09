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
+ ImageActiveX：
	1.插件功能项目
	2.只有基础的3个功能代码，如果需要其他方法可自行扩展
+ ImageActiveXSetUp：
	1.插件安装包项目
+ 测试网页：
	1.MyActiveXTest.html:用于测试ActiveX功能
+ 插件安装包：
	1.ImageActiveXSetUp编译的插件安装包 可直接安装用于测试使用

## 插件功能测试：
1.解压ImageActiveXSetUp.zip
2.安装Setup.exe->选择“任何人”
3.用IE浏览器打开MyActiveXTest.html 点击功能按钮测试
