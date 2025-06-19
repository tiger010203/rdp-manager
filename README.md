# Windows远程桌面管理工具

[![Build Status](https://github.com/YOUR_USERNAME/rdp-manager/workflows/Build%20RDP%20Manager/badge.svg)](https://github.com/YOUR_USERNAME/rdp-manager/actions)

## 🎯 项目简介

这是一个专为Windows系统设计的零写入远程桌面管理工具，具有以下特性：

- **完全后台运行** - 无任何界面，完全隐藏
- **全局快捷键** - 按 Ctrl+Shift+R 快速激活
- **零写入设计** - 绝不在本地留下任何缓存或文件
- **自动关闭** - 10秒无操作自动关闭所有窗口
- **开机启动** - 支持开机自动启动

## 📥 下载使用

### 直接下载编译好的exe文件：
👉 **[点击这里下载最新版本](https://github.com/YOUR_USERNAME/rdp-manager/releases/latest)**

提供两个版本：
- **rdp_manager_cpp.exe** - C++版本，零依赖，体积小（推荐）
- **RDPManager_csharp.exe** - C#版本，功能完整，兼容性好

## 🚀 使用方法

1. **下载exe文件**
2. **以管理员身份运行**（必需，用于监听全局快捷键）
3. **按 Ctrl+Shift+R** 激活程序
4. **输入域名** 获取服务器列表
5. **双击服务器** 进行远程连接

## 📋 功能特点

### 核心功能
- ✅ 全局快捷键监听（Ctrl+Shift+R）
- ✅ 域名获取服务器信息
- ✅ 服务器列表显示（一行一个）
- ✅ RDP远程桌面连接
- ✅ 10秒无操作自动关闭

### 安全特性
- ✅ **零写入** - 不创建任何临时文件
- ✅ **内存处理** - 所有数据仅存储在内存中
- ✅ **无缓存** - 不使用系统缓存或注册表
- ✅ **自动清理** - 程序退出后无任何痕迹

### 服务器信息格式
程序从指定域名获取服务器信息，格式为：
```
192.168.1.100:3389:administrator:password123
192.168.1.101:3389:user:pass456
```
每行一个服务器，格式：`IP:端口:用户名:密码`

## ⚙️ 开机启动设置

### 方法1：启动文件夹
1. 按 `Win+R` 打开运行对话框
2. 输入 `shell:startup` 并回车
3. 将exe文件复制到启动文件夹

### 方法2：任务计划程序
1. 打开任务计划程序
2. 创建基本任务
3. 设置开机启动
4. 选择exe文件路径

## 🔧 技术实现

### C++版本特点
- 使用Windows API实现全局快捷键
- WinINet库处理HTTP请求
- 纯内存RDP连接处理
- 零依赖，直接运行

### C#版本特点
- .NET Framework/Core支持
- Windows Forms GUI框架
- 异步HTTP请求处理
- 完善的异常处理机制

## ⚠️ 注意事项

1. **管理员权限**：程序需要管理员权限来监听全局快捷键
2. **防火墙设置**：确保允许程序访问网络
3. **RDP服务**：目标服务器需要开启RDP服务
4. **网络连接**：需要网络连接来获取服务器信息

## 🛠️ 开发构建

### 自动构建
每次代码推送都会自动构建并发布新版本到 [Releases](https://github.com/YOUR_USERNAME/rdp-manager/releases) 页面。

### 本地构建

#### C++版本
```bash
g++ -o rdp_manager.exe src/rdp_manager.cpp -luser32 -lwininet -lws2_32 -static-libgcc -static-libstdc++ -mwindows
```

#### C#版本
```bash
dotnet publish src/RDPManager.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

## 📄 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情。

## 🤝 贡献

欢迎提交 Issue 和 Pull Request！

## 📞 支持

如有问题或建议，请：
1. 提交 [Issue](https://github.com/YOUR_USERNAME/rdp-manager/issues)
2. 查看 [Wiki](https://github.com/YOUR_USERNAME/rdp-manager/wiki) 文档
3. 参与 [Discussions](https://github.com/YOUR_USERNAME/rdp-manager/discussions) 讨论

---

**⭐ 如果这个项目对您有帮助，请给个Star支持一下！**

