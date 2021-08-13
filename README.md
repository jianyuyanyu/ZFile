# WPF网盘

![build](https://github.com/chenxuuu/Mail-Box-Net-Disk/workflows/build/badge.svg)
![Build GUI client](https://github.com/chenxuuu/Mail-Box-Net-Disk/workflows/Build%20GUI%20client/badge.svg)

利用Net api+WPF实现网盘功能的工具。
## 下载

每次发布版本，会自动在GitHub的release页更新：

## 原理
利用Api+文件分片功实现文件上传于下载
## 框架
WPF：Net5+PrismWpf+RestSharp

Api：Net5+ SqlSugar+sqlite

## 功能

- [ ] 读取文件夹列表/新建文件夹
- [x] 上传小于限制大小的文件
- [x] 读取已存在的文件
- [x] 上传大文件自动分卷
- [x] 识别分卷上传的文件，下载自动合并
- [x] 支持文件夹上传
- [x] 支持文件夹下载
- [ ] win系统下的gui管理工具
- [ ] 其他系统下的gui管理工具

## 命令列表


## 加入该项目

如果你愿意忍受我写的智障代码，那么欢迎pr

## 其他

该项目的代码可以随意引用，但请保留指向该项目的说明文字
