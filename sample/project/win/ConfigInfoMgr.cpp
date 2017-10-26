#include "StdAfx.h"
#include "ConfigInfoMgr.h"
#include "Util.h"

#define MAX_FIELD_LEN 100
#define MAX_USER_SIG_LEN 1024

ConfigInfoMgr* ConfigInfoMgr::GetInst()
{
	static ConfigInfoMgr info;
	return &info;
}

ConfigInfoMgr::ConfigInfoMgr()
{
	m_configInfoFileName = GetDCPath() + _T("ConfigInfo.ini");
	m_appNameConfigInfo = _T("ConfigInfo");
	m_keyNameAppCount = _T("AppCount");
	m_keyNameSceneType = _T("SceneType");
	m_keyNameAccountType = _T("AccountType");
	m_keyNameAppIdAt3rd = _T("AppIdAt3rd");
	m_keyNameSdkAppId = _T("SdkAppId");

	m_keyNameRelationId = _T("RelationId");
	m_keyNameAccountCount = _T("AccountCount");
	m_keyNameIdentifier = _T("Identifier");
	m_keyNameUserSig = _T("UserSig");
	
	m_configInfo.relationId = _T("");
}
ConfigInfoMgr::~ConfigInfoMgr(void)
{
			
}


void ConfigInfoMgr::LoadConfigInfo()
{

	CFileFind fFind;
	bool isExist = fFind.FindFile(m_configInfoFileName); 
	if(!isExist)
	{		
		//创建配置文件
		string fileName = StrWToStrA(m_configInfoFileName);
		FILE *pFile;
		pFile = fopen(fileName.c_str(),"w");
		fclose(pFile);
	}
	else
	{
		TCHAR accountTypeTmp[MAX_FIELD_LEN] = { 0 };
		GetPrivateProfileString(m_appNameConfigInfo, m_keyNameAccountType, _T(""), accountTypeTmp, MAX_FIELD_LEN, m_configInfoFileName);
		m_configInfo.accountType = accountTypeTmp;

		TCHAR appIdAt3rdTmp[MAX_FIELD_LEN] = { 0 };
		GetPrivateProfileString(m_appNameConfigInfo, m_keyNameAppIdAt3rd, _T(""), appIdAt3rdTmp, MAX_FIELD_LEN, m_configInfoFileName);
		m_configInfo.appIdAt3rd = appIdAt3rdTmp;

		TCHAR SdkAppIdTmp[MAX_FIELD_LEN] = { 0 };
		GetPrivateProfileString(m_appNameConfigInfo, m_keyNameSdkAppId, _T(""), SdkAppIdTmp, MAX_FIELD_LEN, m_configInfoFileName);
		m_configInfo.sdkAppId = SdkAppIdTmp;

		TCHAR relationIdTmp[MAX_FIELD_LEN] = { 0 };
		GetPrivateProfileString(m_appNameConfigInfo, m_keyNameRelationId, _T(""), relationIdTmp, MAX_FIELD_LEN, m_configInfoFileName);
		m_configInfo.relationId = relationIdTmp;

		TCHAR identifierTmp[MAX_FIELD_LEN] = { 0 };
		GetPrivateProfileString(m_appNameConfigInfo, m_keyNameIdentifier, _T(""), identifierTmp, MAX_FIELD_LEN, m_configInfoFileName);
		m_configInfo.identifier = identifierTmp;

		TCHAR userSigTmp[MAX_USER_SIG_LEN] = { 0 };
		GetPrivateProfileString(m_appNameConfigInfo, m_keyNameUserSig, _T(""), userSigTmp, MAX_USER_SIG_LEN, m_configInfoFileName);
		m_configInfo.userSig = userSigTmp;
	}

}

void ConfigInfoMgr::SaveConfigInfo()
{
	//::DeleteFile(m_configInfoFileName);

	//CString appCountTmp;
	//appCountTmp.Format(_T("%u"), m_configInfo.appInfoList.size());
	//::WritePrivateProfileString(m_appNameConfigInfo, m_keyNameAppCount, appCountTmp, m_configInfoFileName);

	//for(int i = 0; i < m_configInfo.appInfoList.size(); i++)
	//{		
	//	CString keyNameSceneTypeTmp;
	//	CString keyNameAccountTypeTmp;
	//	CString keyNameAppIdAt3rdTmp;
	//	CString keyNameSdkAppIdTmp;
	//	keyNameSceneTypeTmp.Format(_T("%s%003d"), m_keyNameSceneType.GetString(), i);
	//	keyNameAccountTypeTmp.Format(_T("%s%003d"), m_keyNameAccountType.GetString(), i);
	//	keyNameAppIdAt3rdTmp.Format(_T("%s%003d"), m_keyNameAppIdAt3rd.GetString(), i);
	//	keyNameSdkAppIdTmp.Format(_T("%s%003d"), m_keyNameSdkAppId.GetString(), i);

	//	::WritePrivateProfileString(m_appNameConfigInfo, keyNameSceneTypeTmp, m_configInfo.appInfoList[i].sceneType, m_configInfoFileName);
	//	::WritePrivateProfileString(m_appNameConfigInfo, keyNameAccountTypeTmp, m_configInfo.appInfoList[i].accountType, m_configInfoFileName);
	//	::WritePrivateProfileString(m_appNameConfigInfo, keyNameAppIdAt3rdTmp, m_configInfo.appInfoList[i].appIdAt3rd, m_configInfoFileName);
	//	::WritePrivateProfileString(m_appNameConfigInfo, keyNameSdkAppIdTmp, m_configInfo.appInfoList[i].sdkAppId, m_configInfoFileName);
	//}

	//::WritePrivateProfileString(m_appNameConfigInfo, m_keyNameRelationId, m_configInfo.relationId, m_configInfoFileName);

	//CString accountCountTmp;
	//accountCountTmp.Format(_T("%u"), m_configInfo.accountInfoList.size());
	//::WritePrivateProfileString(m_appNameConfigInfo, m_keyNameAccountCount, accountCountTmp, m_configInfoFileName);

	//for(int i = 0; i < m_configInfo.accountInfoList.size(); i++)
	//{		
	//	CString keyNameIdentifierTmp;
	//	CString keyNameUserSigTmp;
	//	keyNameIdentifierTmp.Format(_T("%s%003d"), m_keyNameIdentifier.GetString(), i);
	//	keyNameUserSigTmp.Format(_T("%s%003d"), m_keyNameUserSig.GetString(), i);
	//	::WritePrivateProfileString(m_appNameConfigInfo, keyNameIdentifierTmp, m_configInfo.accountInfoList[i].identifier, m_configInfoFileName);
	//	::WritePrivateProfileString(m_appNameConfigInfo, keyNameUserSigTmp, m_configInfo.accountInfoList[i].userSig, m_configInfoFileName);
	//}
}


ConfigInfo ConfigInfoMgr::GetConfigInfo()
{
	return m_configInfo;
}









