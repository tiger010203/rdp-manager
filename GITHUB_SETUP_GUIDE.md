# 🚀 GitHub自动编译设置指南

## 📋 操作步骤（5分钟完成）

### 第1步：创建GitHub仓库
1. 登录 [GitHub](https://github.com)
2. 点击右上角 "+" → "New repository"
3. 仓库名称：`rdp-manager`
4. 设置为 **Public**（必须，才能使用免费的GitHub Actions）
5. 勾选 "Add a README file"
6. 点击 "Create repository"

### 第2步：上传项目文件
1. 在新创建的仓库页面，点击 "uploading an existing file"
2. 将以下文件拖拽上传：
   ```
   📁 .github/workflows/build.yml
   📁 src/rdp_manager.cpp
   📁 src/RDPManager.cs
   📁 src/RDPManager.csproj
   📄 README.md
   📄 LICENSE
   ```
3. 在底部填写提交信息："Initial commit"
4. 点击 "Commit changes"

### 第3步：等待自动编译
1. 上传完成后，GitHub会自动开始编译
2. 点击仓库顶部的 "Actions" 标签查看进度
3. 等待编译完成（大约3-5分钟）

### 第4步：下载exe文件
1. 编译完成后，点击 "Releases" 标签
2. 会看到自动生成的新版本
3. 下载以下文件：
   - **rdp_manager_cpp.exe** （C++版本，推荐）
   - **RDPManager_csharp.exe** （C#版本）

## 🎯 自动编译说明

### 编译触发条件
- ✅ 每次代码推送都会自动编译
- ✅ 自动生成新的Release版本
- ✅ 提供两个版本的exe文件下载

### 编译环境
- **Windows Server 2022** 最新版
- **MinGW-w64** 编译C++版本
- **.NET 6.0** 编译C#版本
- **自动化测试** 确保编译成功

### 生成的文件
- **rdp_manager_cpp.exe** - 零依赖，体积约100KB
- **RDPManager_csharp.exe** - 功能完整，体积约50MB

## 📥 下载链接格式

创建成功后，您的下载链接将是：
```
https://github.com/YOUR_USERNAME/rdp-manager/releases/latest
```

将 `YOUR_USERNAME` 替换为您的GitHub用户名。

## ⚠️ 重要提示

1. **仓库必须设为Public** - 免费账户只能在公开仓库使用GitHub Actions
2. **首次编译需要时间** - 大约3-5分钟
3. **自动更新** - 每次修改代码都会自动编译新版本
4. **下载统计** - GitHub会显示下载次数统计

## 🔧 如果编译失败

1. 检查 "Actions" 标签中的错误信息
2. 确保所有文件都正确上传
3. 检查文件路径是否正确

## 📞 需要帮助？

如果在设置过程中遇到问题，请告诉我具体的错误信息，我会帮您解决！

---

**完成后，您就有了一个自动编译的GitHub仓库，每次都能下载到最新的exe文件！**

