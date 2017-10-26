; �ýű�ʹ�� ������װ(az.eliang.com) ������
; ��װ�����ʼ���峣��
!define PRODUCT_NAME "Mofanghr"
!define PRODUCT_NAME_CN "ħ��СƸ"
!define PRODUCT_VERSION "2.1.2.1"
!define INSTALL_VERSION "2.1.2.1"
!define PRODUCT_PUBLISHER "ħ����Ƹ���������Ƽ����޹�˾"
!define PRODUCT_WEB_SITE "http://www.mofanghr.com"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

!define InstallCSInfo "��⵽ϵͳ��δ��װ΢�� .Net Framework 4.0 ����ʱ�������Զ���װ���������˹��̽����������ӣ�"
!define InstallCSInfoUpdata "��⵽ϵͳ��δ��װ΢�� .Net Framework 4.0 �����������Զ���װ���������˹��̽����������ӣ�"

!define Path AutoPackage\Release\*.*
!define PathRtf AutoPackage\Release\���Э��.rtf
!define PathUI AutoPackage\UI

!define Exe mofanghr.exe

!define SI ���������У���رպ����ԡ�����

SetCompress off

; ������װ����Ȩ��(vista,win7,win8)
RequestExecutionLevel admin

; ------ MUI �ִ����涨�� ------
!include "MUI2.nsh"
!include WinMessages.nsh
!include "WinVer.nsh" 

; MUI Ԥ���峣��
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
; ��ӭҳ��
!insertmacro MUI_PAGE_WELCOME
; ���Э��ҳ��
!insertmacro MUI_PAGE_LICENSE "${PathRtf}"
; ��װĿ¼ѡ��ҳ��
!insertmacro MUI_PAGE_DIRECTORY
; ��װ����ҳ��
!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_PAGE_FINISH
; ��װ���ҳ��
;!insertmacro MUI_PAGE_FINISH

; ��װж�ع���ҳ��
!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

; ��װ�����������������
!insertmacro MUI_LANGUAGE "SimpChinese"

; ------ MUI �ִ����涨����� ------

Name "${PRODUCT_NAME_CN} ${PRODUCT_VERSION}"
OutFile "XiaoPin_Setup.exe"
;ELiangID ͳ�Ʊ��     /*  ��װͳ�������ƣ���AlmightyEye.S-VER��  */
InstallDir "$PROGRAMFILES\${PRODUCT_NAME}"
InstallDirRegKey HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
ShowInstDetails show
ShowUninstDetails show
BrandingText "ħ����Ƹ���������Ƽ����޹�˾"

;��װ���汾�Ÿ�ʽ����Ϊx.x.x.x��4������,ÿ��������Χ0~65535,��:2.0.1.2
;��ʹ������ͳ��,�汾�Ž��������ֲ�ͬ�汾�İ�װ���,��ʱ�����û������д��ȷ�İ汾��



VIProductVersion "${INSTALL_VERSION}"
VIAddVersionKey /LANG=${LANG_SimpChinese} "ProductName"      "${PRODUCT_NAME}"
VIAddVersionKey /LANG=${LANG_SimpChinese} "Comments"         "ħ��С��(MF Technology Co., Ltd)"
VIAddVersionKey /LANG=${LANG_SimpChinese} "CompanyName"      "ħ����Ƹ���������Ƽ����޹�˾"
VIAddVersionKey /LANG=${LANG_SimpChinese} "LegalCopyright"   "ħ����Ƹ���������Ƽ����޹�˾(http://www.mofanghr.com)"
VIAddVersionKey /LANG=${LANG_SimpChinese} "FileDescription"  "ħ��С��"
VIAddVersionKey /LANG=${LANG_SimpChinese} "ProductVersion"   "${INSTALL_VERSION}"
VIAddVersionKey /LANG=${LANG_SimpChinese} "FileVersion"      "${INSTALL_VERSION}"

Section "7invensun" SEC01

 SetOutPath "$INSTDIR"
 SetOverwrite on
 File /r /x *.pdb /x *.vshost.exe /x *.vshost.exe.config /x *.vshost.exe.manifest "${Path}"
SectionEnd

Section -AdditionalIcons
  
  CreateDirectory "$SMPROGRAMS\${PRODUCT_NAME_CN}"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME_CN}\ж�� ${PRODUCT_NAME_CN}.lnk" "$INSTDIR\uninst.exe"

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
*  �����ǰ�װ�����ж�ز���  *
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
/* ��� */
Section SPM
	System::Call 'Urlmon.DLL::URLDownloadToFileA(i0, t"http://t.mofanghr.com/?channel=xiaopin&eventType=pv&spm=5.0.1.1.1&version=${PRODUCT_VERSION}&clientTimestamp=1481121294291", t"nsis.txt", i0,i0)i.s'
SectionEnd
/* ���� NSIS �ű��༭����,���� Function ���α�������� Section ����֮���д,�Ա��ⰲװ�������δ��Ԥ֪������. */

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
   Banner::show /set 76 "��װ.NET Framework���ĵȴ�" "������������ϵ010-59423287"
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
    "��⵽�����Ѿ���װ�� ${PRODUCT_NAME_CN} $OLD_VER��\
    $\n$\n��װ���̻��Զ�ж�ص�ǰ�汾�Ƿ�ȷ����" \
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

