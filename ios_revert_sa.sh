#!/bin/bash
# 还原ios导出

path=$(cd `dirname $0`; pwd) #源目录为脚本所在目录
path_IOS=$path/Runtime/IOS #IOS

# 还原IOS目录
svn revert -R $path_IOS
# 将IOS目录下不在svn下的文件丢进垃圾桶
svn status --no-ignore $path_IOS | grep '^\?' | sed 's/^\? //' | xargs -Ixx mv xx ~/.Trash
