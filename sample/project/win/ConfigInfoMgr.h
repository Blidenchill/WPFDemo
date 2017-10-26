#pragma once
#include <vector>
using namespace std;

typedef struct tagConfigInfo
{
	CString relationId;
	CString accountType;
	CString appIdAt3rd;
	CString sdkAppId;
	CString identifier;
	CString userSig;
}ConfigInfo;

class ConfigInfoMgr
{
private:
	ConfigInfoMgr();
public:
	~ConfigInfoMgr(void);
	static ConfigInfoMgr *GetInst();
public:
	void LoadConfigInfo();
	void SaveConfigInfo();
	ConfigInfo GetConfigInfo();

private:
	CString m_configInfoFileName;
	CString m_appNameConfigInfo;
	CString m_keyNameAppCount;
	CString m_keyNameSceneType;
	CString m_keyNameAccountType;
	CString m_keyNameAppIdAt3rd;
	CString m_keyNameSdkAppId;
	
	CString m_keyNameRelationId;
	CString m_keyNameAccountCount;
	CString m_keyNameIdentifier;
	CString m_keyNameUserSig;

	ConfigInfo m_configInfo;
};

