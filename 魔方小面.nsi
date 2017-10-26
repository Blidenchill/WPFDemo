; 该脚本使用 易量安装(az.eliang.com) 向导生成
; 安装程序初始定义常量
!define PRODUCT_NAME "Mofanghr"
!define PRODUCT_NAME_CN "魔方小聘"
!define PRODUCT_VERSION "2.1.2.1"
!define INSTALL_VERSION "2.1.2.1"
!define PRODUCT_PUBLISHER "魔方招聘（北京）科技有限公司"
!define PRODUCT_WEB_SITE "http://www.mofanghr.com"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

!define InstallCSInfo "检测到系统中未安装微软 .Net Framework 4.0 运行时，进行自动安装。。。（此过程将持续几分钟）"
!define InstallCSInfoUpdata "检测到系统中未安装微软 .Net Framework 4.0 补丁，进行自动安装。。。（此过程将持续几分钟）"

!define Path AutoPackage\Release\*.*
!define PathRtf AutoPackage\Release\软件协议.rtf
!define PathUI AutoPackage\UI

!define Exe mofanghr.exe

!define SI 正在运行中，请关闭后重试。。。

SetCompress off

; 提升安装程序权限(vista,win7,win8)
RequestExecutionLevel admin

; ------ MUI 现代界面定义 ------
!include "MUI2.nsh"
!include WinMessages.nsh
!include "WinVer.nsh" 

; MUI 预定义常量
!define MUI_ABORTWARNING
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "${PathUI}\m2.bmp"
!define MUI_HEADERIMAGE_UNBITMAP "${PathUI}\m2.bmp"
!define MUI_ICON "${PathUI}\Icon.ico"
!define MUI_UNICON "${PathUI}\Icon.ico"
!define MUI_WELCOMEFINISHPAGE_BITMAP "${PathUI}\m1.bmp"
!define MUI_UNWELCOMEFINISHPAGE_BITMAP "${PathUI}\m1.bmp"
!define MUI_FINISHPAGE_RUN
!define MUI_FINISHPAGE_RUN_FUNCTION "LaunchLink"
; 欢迎页面
!insertmacro MUI_PAGE_WELCOME
; 许可协议页面
!insertmacro MUI_PAGE_LICENSE "${PathRtf}"
; 安装目录选择页面
!insertmacro MUI_PAGE_DIRECTORY
; 安装过程页面
!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_PAGE_FINISH
; 安装完成页面
;!insertmacro MUI_PAGE_FINISH

; 安装卸载过程页面
!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

; 安装界面包含的语言设置
!insertmacro MUI_LANGUAGE "SimpChinese"

; ------ MUI 现代界面定义结束 ------

Name "${PRODUCT_NAME_CN} ${PRODUCT_VERSION}"
OutFile "XiaoPin_Setup.exe"
;ELiangID 统计编号     /*  安装统计项名称：【AlmightyEye.S-VER】  */
InstallDir "$PROGRAMFILES\${PRODUCT_NAME}"
InstallDirRegKey HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
ShowInstDetails show
ShowUninstDetails show
BrandingText "魔方招聘（北京）科技有限公司"

;安装包版本号格式必须为x.x.x.x的4组整数,每组整数范围0~65535,如:2.0.1.2
;若使用易量统计,版本号将用于区分不同版本的安装情况,此时建议用户务必填写正确的版本号



VIProductVersion "${INSTALL_VERSION}"
VIAddVersionKey /LANG=${LANG_SimpChinese} "ProductName"      "${PRODUCT_NAME}"
VIAddVersionKey /LANG=${LANG_SimpChinese} "Comments"         "魔方小面(MF Technology Co., Ltd)"
VIAddVersionKey /LANG=${LANG_SimpChinese} "CompanyName"      "魔方招聘（北京）科技有限公司"
VIAddVersionKey /LANG=${LANG_SimpChinese} "LegalCopyright"   "魔方招聘（北京）科技有限公司(http://www.mofanghr.com)"
VIAddVersionKey /LANG=${LANG_SimpChinese} "FileDescription"  "魔方小面"
VIAddVersionKey /LANG=${LANG_SimpChinese} "ProductVersion"   "${INSTALL_VERSION}"
VIAddVersionKey /LANG=${LANG_SimpChinese} "FileVersion"      "${INSTALL_VERSION}"

Section "7invensun" SEC01

 SetOutPath "$INSTDIR"
 SetOverwrite on
 File /r /x *.pdb /x *.vshost.exe /x *.vshost.exe.config /x *.vshost.exe.manifest "${Path}"
SectionEnd

Section -AdditionalIcons
  
  CreateDirectory "$SMPROGRAMS\${PRODUCT_NAME_CN}"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME_CN}\卸载 ${PRODUCT_NAME_CN}.lnk" "$INSTDIR\uninst.exe"

  IfFileExists $INSTDIR\${Exe} 0 +2
    CreateShortCut "$DESKTOP\${PRODUCT_NAME_CN}.lnk" "$INSTDIR\${Exe}"
   
  IfFileExists $INSTDIR\${Exe} 0 +2
    CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME_CN}\${PRODUCT_NAME_CN}.lnk" "$INSTDIR\${Exe}"
   
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "InstallString" "$INSTDIR\${Exe}"
SectionEnd

/******************************
*  以下是安装程序的卸载部分  *
******************************/

Section Uninstall
  Delete "$INSTDIR\uninst.exe"
  RMDir /r "$INSTDIR\images"
  RMDir /r "$INSTDIR\Audio"
  RMDir /r "$INSTDIR"

  Delete "$DESKTOP\${PRODUCT_NAME_CN}.lnk"
  Delete "$STARTMENU\${PRODUCT_NAME_CN}.lnk"
  RMDir /r "$SMPROGRAMS\${PRODUCT_NAME_CN}"
 
  IfFileExists "$STARTMENU\Programs\Startup\${PRODUCT_NAME_CN}.lnk" 0 +2
    Delete "$STARTMENU\Programs\Startup\${PRODUCT_NAME_CN}.lnk"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
SectionEnd

Section JudgeNETFramework
  Call GetNetFrameworkTargetVersion
  Pop $R1
  ${If} $R1 != '4.0.0'
  Call InstallDotNetFramework
  ${ENDIF} 

SectionEnd


Section AsyncAwait
	Call GetNetFrameworkVersion
	Pop $R1
	${If} $R1 == '4.0.30319'						
		Call IS64
		Pop $R2
		${If} $R2 == 'x64'
			Call KB2468871X64
			Pop $R3
			${If} $R3 != 'Y'
				Call InstallAsyncX64
			${ENDIF} 
		${Else}
			Call KB2468871X86
			Pop $R4
			${If} $R4 != 'Y'
				Call InstallAsyncX86
			${ENDIF} 
		${ENDIF} 
	${ENDIF} 	
SectionEnd
/* 埋点 */
Section SPM
	System::Call 'Urlmon.DLL::URLDownloadToFileA(i0, t"http://t.mofanghr.com/?channel=xiaopin&eventType=pv&spm=5.0.1.1.1&version=${PRODUCT_VERSION}&clientTimestamp=1481121294291", t"nsis.txt", i0,i0)i.s'
SectionEnd
/* 根据 NSIS 脚本编辑规则,所有 Function 区段必须放置在 Section 区段之后编写,以避免安装程序出现未可预知的问题. */

Function GetNetFrameworkTargetVersion
   Push $1
     ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "TargetVersion"
   Exch $1
FunctionEnd

Function GetNetFrameworkVersion
   Push $1
     ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Version"
   Exch $1
FunctionEnd

Function IS64
	Push $1
    System::Call "kernel32::GetCurrentProcess() i .s"
    System::Call "kernel32::IsWow64Process(i s, *i .r0)"
    StrCmp $0 "0" is32bit is64bit
    is32bit:
			StrCpy $1 "x86"
            Goto exit
    is64bit:
		StrCpy $1 "x64"		
    exit:
	 Exch $1
FunctionEnd

Function KB2468871X86
    Push $1
     ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\Updates\Microsoft .NET Framework 4 Client Profile\KB2468871" "ThisVersionInstalled"
    Exch $1
FunctionEnd

Function KB2468871X64
    Push $1
     ReadRegDWORD $1 HKLM "SOFTWARE\Wow6432Node\Microsoft\Updates\Microsoft .NET Framework 4 Client Profile\KB2468871" "ThisVersionInstalled"
    Exch $1
FunctionEnd

Function InstallDotNetFramework    
   Banner::show /set 76 "安装.NET Framework耐心等待" "如遇问题请联系010-59423287"
   ExecWait '${InstallCSInfo}' $0
   System::Call 'Urlmon.DLL::URLDownloadToFileA(i0, t"https://public.mofanghr.com/xiaopin/latest/dotNetFx40_Full_x86_x64.exe", t"dotNetFx40_Full_x86_x64.exe", i0,i0)i.s'
   ExecWait 'dotNetFx40_Full_x86_x64.exe /q /norestart' $0
   Banner::destroy
FunctionEnd

Function InstallAsyncX86     
   ExecWait '${InstallCSInfoUpdata}' $0
   System::Call 'Urlmon.DLL::URLDownloadToFileA(i0, t"https://public.mofanghr.com/xiaopin/latest/NDP40-KB2468871-v2-x86.exe", t"NDP40-KB2468871-v2-x86.exe", i0,i0)i.s'
   ExecWait 'NDP40-KB2468871-v2-x86.exe /q /norestart' $0
FunctionEnd

Function InstallAsyncX64   
   ExecWait '${InstallCSInfoUpdata}' $0
   System::Call 'Urlmon.DLL::URLDownloadToFileA(i0, t"https://public.mofanghr.com/xiaopin/latest/NDP40-KB2468871-v2-x64.exe", t"NDP40-KB2468871-v2-x64.exe", i0,i0)i.s'
   ExecWait 'NDP40-KB2468871-v2-x64.exe /q /norestart' $0
FunctionEnd

Function LaunchLink
   ExecShell "" "$INSTDIR\${Exe}"
FunctionEnd

Function .onInit
  
  Call OtherJudge
  Call DeleteBefore

FunctionEnd

Function un.onInit

  Call un.OtherJudge

FunctionEnd


Function OtherJudge
 
   FindProcDLL::FindProc "${Exe}"
   Pop $R0
   IntCmp $R0 1 0 no_run
   MessageBox MB_ICONSTOP "${Exe} ${SI}"
   Quit
   no_run:

FunctionEnd

Function un.OtherJudge
 
   FindProcDLL::FindProc "${Exe}"
   Pop $R0
   IntCmp $R0 1 0 no_run
   MessageBox MB_ICONSTOP "${Exe} ${SI}"
   Quit
   no_run:

FunctionEnd
 
Function un.onUninstSuccess
FunctionEnd


Var UNINSTALL_PROG
Var OLD_VER
Var OLD_PATH


Function DeleteBefore
  ClearErrors
  ReadRegStr $UNINSTALL_PROG ${PRODUCT_UNINST_ROOT_KEY} ${PRODUCT_UNINST_KEY} "UninstallString"
  IfErrors  done
  
  ReadRegStr $OLD_VER ${PRODUCT_UNINST_ROOT_KEY} ${PRODUCT_UNINST_KEY} "DisplayVersion"
  MessageBox MB_OKCANCEL|MB_ICONQUESTION \
    "检测到本机已经安装了 ${PRODUCT_NAME_CN} $OLD_VER。\
    $\n$\n安装过程会自动卸载当前版本是否确定？" \
      /SD IDOK \
      IDOK uninstall \
      
  Abort
  
uninstall:
  StrCpy $OLD_PATH $UNINSTALL_PROG -10


  ExecWait '"$UNINSTALL_PROG" /S _?=$OLD_PATH' $0
  DetailPrint "uninst.exe returned $0"
  Delete "$DOCUMENTS\mofanghr\userinfo.db"
  Delete "$UNINSTALL_PROG"
  RMDir $OLD_PATH


done:
FunctionEnd

