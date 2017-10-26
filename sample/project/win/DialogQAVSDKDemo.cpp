
// DialogQAVSDKDemo.cpp : implementation file
//

#include "stdafx.h"
#include "QAVSDKApp.h"
#include "DialogQAVSDKDemo.h"
#include "afxdialogex.h"
#include "CustomWinMsg.h"
#include "ConfigInfoMgr.h"
#include "Util.h"
#include "av_common.h"

//crash�ϱ�
#include "crash_report.h"

#define ENABLE_UI_OPERATION_SAFETY

//ʵʱͨ�ų��������
#define SCENE_IM_EXPERIENCE _T("ʵʱͨ�ų��������")
//�������������
#define SCENE_ANCHOR_EXPERIENCE _T("�������������")

//Ĭ�ϳ�����ʵʱͨ�ų��������
#define DEFAULT_SCENE SCENE_IM_EXPERIENCE


#ifdef _DEBUG
#define new DEBUG_NEW
#endif

#define TIMER_SHOW_OPERRATION_TIPS 2
#define TIMER_EXTERNAL_CAPTURE 3
#define SHORT_TIME (1000)
#define LONG_TIME (3000)

struct ErrorInfo 
{
	int retCode;
	CString desc;
};

void LoginCallBack::OnSuccess() 
{
	::PostMessage(GetMainHWnd(), WM_ON_LOGIN, (WPARAM)AV_OK, 0);
};

void LoginCallBack::OnError(int retCode, const std::string &desc) 
{
	ErrorInfo *pInfo = new ErrorInfo;
	pInfo->retCode = retCode;
	pInfo->desc = desc.c_str();

	::PostMessage(GetMainHWnd(), WM_ON_LOGIN, (WPARAM)AV_ERR_FAILED, (LPARAM)pInfo);
};

void LogoutCallBack::OnSuccess() 
{
	::PostMessage(GetMainHWnd(), WM_ON_LOGOUT, (WPARAM)AV_OK, 0);
};

void LogoutCallBack::OnError(int retCode, const std::string &desc) 
{
	::PostMessage(GetMainHWnd(), WM_ON_LOGOUT, (WPARAM)AV_ERR_FAILED, retCode);
};

DialogQAVSDKDemo::DialogQAVSDKDemo(CWnd* pParent /*=NULL*/)
	: CDialogEx(DialogQAVSDKDemo::IDD, pParent)
    , m_sdkWrapper(this)
    , m_relationId(0)
	, m_identifier("")
    
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_isExitDemo = false;

	m_operationTipsLogining = _T("���ڵ�¼...");
	m_operationTipsLogouting = _T("�����˳���¼...");
	m_operationTipsEnterRooming = _T("���ڽ��뷿��...");
	m_operationTipsExitRooming = _T("�����˳�����...");
	m_operationTipsOpenCameraing = _T("���ڴ�����ͷ...");
	m_operationTipsCloseCameraing = _T("���ڹر�����ͷ...");
	m_operationTipsReqeustViewing = _T("����������...");
	m_operationTipsCancelViewing = _T("����ȡ������...");

	ConfigInfoMgr::GetInst()->LoadConfigInfo();

}

DialogQAVSDKDemo::~DialogQAVSDKDemo()
{
	ConfigInfoMgr::GetInst()->SaveConfigInfo();
}

void DialogQAVSDKDemo::DoDataExchange(CDataExchange* pDX)
{
    CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT_ACCOUNT_ID, m_editAccountID);
    DDX_Control(pDX, IDC_EDIT_ACCOUNT_TYPE, m_editAccountType);
    DDX_Control(pDX, IDC_EDIT_APP_ID_AT_3RD, m_editAppIdAt3rd);
    DDX_Control(pDX, IDC_EDIT_SDK_APP_ID, m_editSdkAppId);
    DDX_Control(pDX, IDC_EDIT_RELATION_ID, m_editRelationId);
    DDX_Control(pDX, IDC_LIST_ENDPOINT_LIST, m_listEndpointList);	
    DDX_Control(pDX, IDC_VIEW_LOCAL, m_viewLocalVideoRender);
    DDX_Control(pDX, IDC_VIEW_ROMOTE, m_viewBigVideoRender);
    DDX_Control(pDX, IDC_SLIDER_MIC_VOLUME, m_sliderAudioMicVolume);
    DDX_Control(pDX, IDC_STATIC_MIC_CUR_VOLUME, m_staticMicCurVolume);
    DDX_Control(pDX, IDC_SLIDER_PLAYER_VOLUME, m_sliderAudioPlayerVolume);
    DDX_Control(pDX, IDC_STATIC_PLAYER_CUR_VOLUME, m_staticPlayerCurVolume);
    DDX_Control(pDX, IDC_STATIC_OPERATION_TIPS, m_staticOperationTips);


}

BEGIN_MESSAGE_MAP(DialogQAVSDKDemo, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_RBUTTONDOWN()
	ON_WM_MOUSELEAVE()
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_WM_TIMER()
	ON_WM_CLOSE()
	ON_WM_DESTROY()
	ON_WM_HSCROLL()

	//ON_MESSAGE_VOID(WM_CLOSE, OnCloseDemo)
	ON_MESSAGE(WM_ON_LOGIN, OnLogin)
	ON_MESSAGE(WM_ON_LOGOUT, OnLogout)
	ON_MESSAGE(WM_ON_START_CONTEXT, OnStartContext)
	ON_MESSAGE(WM_ON_STOP_CONTEXT, OnStopContext)
	ON_MESSAGE(WM_ON_ENTER_ROOM, OnEnterRoom)
	ON_MESSAGE(WM_ON_EXIT_ROOM, OnExitRoom)
	ON_MESSAGE(WM_ON_ENDPOINTS_UPDATE_INFO, OnEnpointsUpdateInfo)
	ON_MESSAGE(WM_ON_REQUEST_VIEW_LIST, OnRequestViewList)
	ON_MESSAGE(WM_ON_CANCEL_ALL_VIEW, OnCancelAllView)
	ON_MESSAGE(WM_ON_MIC_OPERATION, OnMicOperation)
	ON_MESSAGE(WM_ON_PLAYER_OPERATION, OnPlayerOperation)
	ON_MESSAGE(WM_ON_CAMERA_OPERATION, OnCameraOperation)
    ON_MESSAGE(WM_ON_CAMERA_CHANGE, OnCameraChange)
	ON_MESSAGE(WM_ON_AUDIO_DEVICE_DETECT, OnAudioDeviceDetect)
	ON_MESSAGE(WM_LBUTTONDBLCLK, OnLButtonDBClick)
	ON_MESSAGE(DM_GETDEFID, OnGetDefId)

    END_MESSAGE_MAP()

BOOL DialogQAVSDKDemo::OnInitDialog()
{
	CDialogEx::OnInitDialog();
	ConfigInfo appConfigInfo = ConfigInfoMgr::GetInst()->GetConfigInfo();

	SetIcon(m_hIcon, TRUE); 
	SetIcon(m_hIcon, FALSE); 

	m_accountType = appConfigInfo.accountType;
	m_appIdAt3rd = appConfigInfo.appIdAt3rd;
	m_SdkAppId = appConfigInfo.sdkAppId;	

	m_editAccountType.SetWindowTextW(appConfigInfo.accountType);
	m_editAppIdAt3rd.SetWindowTextW(appConfigInfo.appIdAt3rd);
	m_editSdkAppId.SetWindowTextW(appConfigInfo.sdkAppId);
	m_editAccountID.SetWindowTextW(appConfigInfo.identifier);
	m_editRelationId.SetWindowTextW(appConfigInfo.relationId);

	m_relationId = _ttoi(appConfigInfo.relationId);
	m_identifier = appConfigInfo.identifier;

	m_sliderAudioMicVolume.SetRange(MIN_AUDIO_DEVICE_VOLUME, MAX_AUDIO_DEVICE_VOLUME);
	m_sliderAudioMicVolume.SetTicFreq(MAX_AUDIO_DEVICE_VOLUME);
	m_sliderAudioMicVolume.SetPos(70);
	m_staticMicCurVolume.SetWindowTextW(_T("70"));
	m_sliderAudioPlayerVolume.SetRange(MIN_AUDIO_DEVICE_VOLUME, MAX_AUDIO_DEVICE_VOLUME);
	m_sliderAudioPlayerVolume.SetTicFreq(MAX_AUDIO_DEVICE_VOLUME);
	m_sliderAudioPlayerVolume.SetPos(70);
	m_staticPlayerCurVolume.SetWindowTextW(_T("70"));

	CRect mainDialogRect; 
	GetWindowRect(&mainDialogRect);
	m_mainDialogWidth = mainDialogRect.right - mainDialogRect.left;
	m_mainDialogHeight = mainDialogRect.bottom - mainDialogRect.top;//����չ���߶�	

	m_mainDialogLeft = (::GetSystemMetrics(SM_CXSCREEN) - m_mainDialogWidth)/2;
	m_mainDialogTop = 0;
	SetWindowPos(GetDlgItem(IDD_DIALOG_DEMO), m_mainDialogLeft, m_mainDialogTop, m_mainDialogWidth, m_mainDialogHeight, 0);	

	m_isLogin = false;
	m_isEnterRoom = false;

	m_viewBigVideoRender.Init(VIDEO_RENDER_BIG_VIEW_WIDTH, VIDEO_RENDER_BIG_VIEW_HEIGHT, COLOR_FORMAT_RGB24);
	m_viewLocalVideoRender.Init(VIDEO_RENDER_SMALL_VIEW_WIDTH, VIDEO_RENDER_SMALL_VIEW_HEIGHT, COLOR_FORMAT_RGB24);	

	m_viewBigVideoRender.m_videoSrcType = VIDEO_SRC_TYPE_CAMERA;
	m_viewLocalVideoRender.m_videoSrcType = VIDEO_SRC_TYPE_CAMERA;

    //=============�Զ���¼=========
	Login();
	//=============================
	return TRUE;  // return TRUE  unless you set the focus to a control
}

void DialogQAVSDKDemo::OnSysCommand(UINT nID, LPARAM lParam)
{	
	CDialogEx::OnSysCommand(nID, lParam);
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void DialogQAVSDKDemo::OnPaint()
{	
	if (IsIconic())
	{
		
	}
	else
	{
    CDialog::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
HCURSOR DialogQAVSDKDemo::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void DialogQAVSDKDemo::TrackMenu(CWnd*pWnd)
{
	//CRect rc;
	//CMenu menu;

	//pWnd->GetWindowRect(&rc);
	//menu.LoadMenuW(IDR_MENU_ENDPOINT);
	//menu.GetSubMenu(0)->TrackPopupMenu(TPM_LEFTALIGN, rc.left, rc.top, this);
}

BOOL DialogQAVSDKDemo::PreTranslateMessage(MSG* pMsg)
{
	return CDialogEx::PreTranslateMessage(pMsg);
}

LONG DialogQAVSDKDemo::OnGetDefId(WPARAM wp, LPARAM lp)    
{  
	return MAKELONG(0,DC_HASDEFID);    
} 

void DialogQAVSDKDemo::OnClose()
{
	m_isExitDemo = true;
	ConfigInfoMgr::GetInst()->SaveConfigInfo();

	if(m_isEnterRoom)
	{
		int retCode = m_sdkWrapper.ExitRoom();
		if(retCode != AV_OK)ClearOperationTips();
		ShowRetCode("ExitRoom", retCode);
	}
	else if(m_isLogin)
	{
		int retCode = m_sdkWrapper.StopContext();
		if(retCode != AV_OK)ClearOperationTips();
		ShowRetCode("StopContext", retCode);
	}
	else
	{
		DestroyWindow();
	}
}

void DialogQAVSDKDemo::OnDestroy()
{
	PostQuitMessage(WM_QUIT);
}

void DialogQAVSDKDemo::OnTimer(UINT_PTR eventId)
{

	 if (eventId == TIMER_SHOW_OPERRATION_TIPS)
	{
		ClearOperationTips();
	}
	CWnd::OnTimer(eventId);
}

void DialogQAVSDKDemo::OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar)
{
	if((CWnd*)pScrollBar == (CWnd*)&m_sliderAudioMicVolume)  
	{  
		int volume = m_sliderAudioMicVolume.GetPos();  

		if (volume < MIN_AUDIO_DEVICE_VOLUME)
		{
			volume = MIN_AUDIO_DEVICE_VOLUME;
		}
		else if (volume > MAX_AUDIO_DEVICE_VOLUME)
		{
			volume = MAX_AUDIO_DEVICE_VOLUME;
		}
		CString volumeStr;
		volumeStr.Format(_T("%d"), volume);
		m_staticMicCurVolume.SetWindowTextW(volumeStr);
		m_sdkWrapper.SetMicVolume(volume);
	}  
	else if((CWnd*)pScrollBar == (CWnd*)&m_sliderAudioPlayerVolume)  
	{  
		int volume = m_sliderAudioPlayerVolume.GetPos();  

		if (volume < MIN_AUDIO_DEVICE_VOLUME)
		{
			volume = MIN_AUDIO_DEVICE_VOLUME;
		}
		else if (volume > MAX_AUDIO_DEVICE_VOLUME)
		{
			volume = MAX_AUDIO_DEVICE_VOLUME;
		}
		CString volumeStr;
		volumeStr.Format(_T("%d"), volume);
		m_staticPlayerCurVolume.SetWindowTextW(volumeStr);
		m_sdkWrapper.SetPlayerVolume(volume);
	}    
}

LONG DialogQAVSDKDemo::OnLogin(WPARAM wParam, LPARAM lParam)
{
	int retCode = wParam;
	if(retCode == AV_OK)
	{      
		//��¼�ɹ���ȥ��ʼ������ƵSDK
		m_sdkWrapper.CreateContext();
        m_sdkWrapper.GetAVContext()->SetSpearEngineMode(SPEAR_EGINE_MODE_WEBCLOUD);
		PostMsgToMain(XPVIDEO_LOGIN,1,0);

		int retCode = m_sdkWrapper.StartContext(m_contextStartParam);
		ShowRetCode("StartContext", retCode);
		
	}
	else
	{
		PostMsgToMain(XPVIDEO_LOGIN, 0, 0);
		ErrorInfo *pinfo = (ErrorInfo *)lParam;
		CString errCodeStr;
		errCodeStr.Format(_T("��¼ʱ���������룺%d, ������Ϣ��%s."), pinfo->retCode, pinfo->desc);

		delete pinfo;

		ShowMessageBox(errCodeStr);
	}

	return 0;
}

LONG DialogQAVSDKDemo::OnLogout(WPARAM wParam, LPARAM lParam)
{
	int retCode = wParam;
	if(retCode == AV_OK)
	{
		//TODO
	}
	else
	{
		int errCode = lParam;
		CString errCodeStr;
		errCodeStr.Format(_T("�˳���¼ʱ���������룺%d."), errCode);
		ShowMessageBox(errCodeStr);
	}

	TIMManager::get().Uninit();//�˳���¼���ͷ�ͨ��SDK
	if(m_isExitDemo)DestroyWindow();
	return 0;
}

LONG DialogQAVSDKDemo::OnStartContext(WPARAM wParam, LPARAM lParam)
{
	Error nError = (Error)wParam;
	if (AV_OK == nError)
	{
		//����ƵSDK��ʼ���ɹ�
		m_isLogin = true;
		//===============�Զ�������===========
		EnterRoom();
	}
	return 0;
}

LONG DialogQAVSDKDemo::OnStopContext(WPARAM wParam, LPARAM lParam)
{
	Error nError = (Error)wParam;
	if (AV_OK == nError)
	{
		m_sdkWrapper.DestroyContext();

		//�ͷ�����ƵSDK���˳���¼
		TIMManager::get().Logout(&m_logoutCallBack);
		
		m_isLogin = false;
		m_isEnterRoom = false;
	}


	return 0;
}

LONG DialogQAVSDKDemo::OnEnterRoom(WPARAM wParam, LPARAM lParam)
{
	Error retCode = (Error)wParam;
	if (AV_OK == retCode)
	{
		m_isEnterRoom = true;
		//������ͷ����˷硢������
		int retCode = AV_OK;
		/*
		�����ⲿ�������Ƶ�豸������
		ע�⣺
		. �ⲿ������ڲ�����ͷ�ǻ���ġ�
		. ����������ⲿ�����豸�������󣬾Ͳ�����ʹ���ڲ�����ͷ�ˣ�Ҳ���ǣ����Ҫʹ���ڲ�����ͷ���Ͳ��������ⲿ�����豸��������
		. ���Ҫʹ���ⲿ�����豸�������ø�����Ҳ��������ʹ�ã���Ȼ�������ˣ����SDK�ṩ��Ϊ׼ȷ��������Ϣ��
		. ���Ҫ���ã������ڽ��뷿��ǰ�����úá�
		*/		

		m_cameraList.clear();
		

		m_sdkWrapper.GetCameraList(m_cameraList);
		if(m_cameraList.size() > 0)
		{
			m_sdkWrapper.SetSelectedCameraId(m_cameraList[0].first);
			OpenCamera();
		}		

		m_micList.clear();		
		m_sdkWrapper.GetMicList(m_micList);		
		if(m_micList.size() > 0)
		{
			m_sdkWrapper.SetSelectedMicId(m_micList[0].first);
			OpenMic();
		}

		m_playerList.clear();
		m_sdkWrapper.GetPlayerList(m_playerList);
		if(m_playerList.size() > 0)
		{
			m_sdkWrapper.SetSelectedPlayerId(m_playerList[0].first);
			OpenPlayer();
		}
	}

	
	return 0;
}

LONG DialogQAVSDKDemo::OnExitRoom(WPARAM wParam, LPARAM lParam)
{

	m_isEnterRoom = false;

	m_viewLocalVideoRender.Clear();
	m_viewBigVideoRender.Clear();


	m_listEndpointList.ResetContent();	
	m_listEndpointList.SetWindowTextW(_T(""));
	m_micList.clear();
	m_playerList.clear();
	m_cameraList.clear();
		
	int retCode = (int)wParam;
	ShowRetCode("ExitRoom. ", retCode);
	if(m_isExitDemo)m_sdkWrapper.StopContext(); 
	return 0;
}

LONG DialogQAVSDKDemo::OnEnpointsUpdateInfo(WPARAM wParam, LPARAM lParam)
{
	UpdateEndpointList();
	UpadateVideoRenderInfo();
	return 0;
}

LONG DialogQAVSDKDemo::OnRequestViewList(WPARAM wParam, LPARAM lParam)
{
	UpadateVideoRenderInfo();

	Error retCode = (Error)wParam;
	if (AV_OK == retCode)
	{

	}
	else
	{
		ShowRetCode("RequestViewList. ", retCode);
	}
	return 0;
}

LONG DialogQAVSDKDemo::OnCancelAllView(WPARAM wParam, LPARAM lParam)
{
	UpadateVideoRenderInfo();

	Error retCode = (Error)wParam;
	if (AV_OK == retCode)
	{

	}
	else
	{
		ShowRetCode("CancelAllView.  ", retCode);
	}
	return 0;
}

LONG DialogQAVSDKDemo::OnMicOperation(WPARAM wParam, LPARAM lParam)
{
	Error retCode = (Error)wParam;
	AVDevice::DeviceOperation oper = (AVDevice::DeviceOperation)lParam;
	if(oper == AVDevice::DEVICE_OPERATION_OPEN)
	{
		if (AV_OK == retCode)
		{	
			uint32 micVolume = m_sdkWrapper.GetMicVolume();
			CString strMicVolume = _T("");
			strMicVolume.Format(_T("%d"), micVolume);
			m_sliderAudioMicVolume.SetPos(micVolume);
			m_staticMicCurVolume.SetWindowTextW(strMicVolume);
		}
		else if (AV_ERR_FAILED == retCode)
		{
			ShowRetCode("MicOperation. ", retCode);
		}
	}
	else if(oper == AVDevice::DEVICE_OPERATION_CLOSE)
	{
		if (AV_OK == retCode)
		{	
			uint32 micVolume = 0;
			CString strMicVolume = _T("");
			strMicVolume.Format(_T("%d"), micVolume);
			m_sliderAudioMicVolume.SetPos(micVolume);
			m_staticMicCurVolume.SetWindowTextW(strMicVolume);
		}
		else if (AV_ERR_FAILED == retCode)
		{
			ShowRetCode("MicOperation. ", retCode);
		}
	}

	if(retCode != AV_OK)ShowRetCode("Mic", retCode);
	return 0;
}

LONG DialogQAVSDKDemo::OnPlayerOperation(WPARAM wParam, LPARAM lParam)
{	
	Error retCode = (Error)wParam;
	AVDevice::DeviceOperation oper = (AVDevice::DeviceOperation)lParam;
	if(oper == AVDevice::DEVICE_OPERATION_OPEN)
	{
		if (AV_OK == retCode)
		{	
			uint32 playerVolume = m_sdkWrapper.GetPlayerVolume();
			CString strPlayerVolume = _T("");
			strPlayerVolume.Format(_T("%d"), playerVolume);
			m_sliderAudioPlayerVolume.SetPos(playerVolume);
			m_staticPlayerCurVolume.SetWindowTextW(strPlayerVolume);

		}
		else if (AV_ERR_FAILED == retCode)
		{
			ShowRetCode("PlayerOperation. ", retCode);
		}
	}
	else if(oper == AVDevice::DEVICE_OPERATION_CLOSE)
	{
		if (AV_OK == retCode)
		{	
			uint32 playerVolume = 0;
			CString strPlayerVolume = _T("");
			strPlayerVolume.Format(_T("%d"), playerVolume);
			m_sliderAudioPlayerVolume.SetPos(playerVolume);
			m_staticPlayerCurVolume.SetWindowTextW(strPlayerVolume);
		}
		else if (AV_ERR_FAILED == retCode)
		{
			ShowRetCode("PlayerOperation. ", retCode);
		}
	}

	if(retCode != AV_OK)ShowRetCode("Player", retCode);	
	return 0;
}

LONG DialogQAVSDKDemo::OnCameraOperation(WPARAM wParam, LPARAM lParam)
{
	Error retCode = (Error)wParam;
	AVDevice::DeviceOperation oper = (AVDevice::DeviceOperation)lParam;
	if(oper == AVDevice::DEVICE_OPERATION_OPEN)
	{		
		if (AV_OK == retCode)
		{	
			m_viewLocalVideoRender.m_identifier = m_contextStartParam.identifier;
			m_viewLocalVideoRender.m_videoSrcType = VIDEO_SRC_TYPE_CAMERA;
		}
		else if (AV_ERR_FAILED == retCode)
		{
			m_viewLocalVideoRender.m_identifier = "";
			m_viewLocalVideoRender.Clear();
		}
	}
	else if(oper == AVDevice::DEVICE_OPERATION_CLOSE)
	{		
		//TODO
		if (AV_OK == retCode)
		{	
			m_viewLocalVideoRender.m_identifier = "";
			m_viewLocalVideoRender.Clear();
		}
		else if (AV_ERR_FAILED == retCode)
		{

		}
	}
	if(retCode != AV_OK)ShowRetCode("Camera", retCode);
	return 0;
}

LONG DialogQAVSDKDemo::OnCameraChange(WPARAM wParam, LPARAM lParam)
{
    int newIdx = -1;

    std::string curCameraId;

    if (m_cameraList.size() > 0)
		curCameraId = m_sdkWrapper.GetSelectedCameraId();
	
    m_cameraList.clear();

	m_sdkWrapper.GetCameraList(m_cameraList);
	if(m_cameraList.size() > 0)
	{
		for (int i = 0; i < m_cameraList.size(); i++)
		{
            if(!curCameraId.empty() && curCameraId == m_cameraList[i].first)
                newIdx = i;
		}
	}
	else
	{
		m_viewLocalVideoRender.m_identifier = "";
		m_viewLocalVideoRender.Clear();
		return 0;
	}

    if(newIdx < 0) 
    {
        if (m_cameraList.size() > 0) 
        {
            m_sdkWrapper.SetSelectedCameraId(m_cameraList[0].first);
			OpenCamera();
        }
    }

    return 0;
}

void DialogQAVSDKDemo::PrepareEncParam()
{

}

void DialogQAVSDKDemo::ShowOperationTips(CString tips, int timeLen)
{
	KillTimer(TIMER_SHOW_OPERRATION_TIPS);
	m_staticOperationTips.SetWindowTextW(tips);
	SetTimer(TIMER_SHOW_OPERRATION_TIPS, timeLen, 0);
}

void DialogQAVSDKDemo::ClearOperationTips()
{
	m_staticOperationTips.SetWindowTextW(_T(""));
}

void DialogQAVSDKDemo::UpdateEndpointList()
{
	AVEndpoint** endpointList[1];
	int count = m_sdkWrapper.GetEndpointList(endpointList);
	if (count == 0) return;

	m_listEndpointList.ResetContent();
	for (int i = 0 ; i < count; i++)
	{
		std::string identifier = endpointList[0][i]->GetId();

		if(identifier.size() == 0)
		{
			//ShowRetCode("UserUpdate identifier.size() == 0", AV_ERR_FAILED);
			continue;
		}

		int j = 0;
		for(j = 0; j < i; j++)
		{
			std::string identifierTmp = endpointList[0][j]->GetId();
			if(identifier == identifierTmp)//repeat
			{
				break;
			}
		}

		if(j < i)//repeat
		{
			//ShowRetCode("UserUpdate identifier repeat", AV_ERR_FAILED);
			continue;
		}

		AVEndpoint *endpoint = m_sdkWrapper.GetEndpointById(identifier);
		if(endpoint)
		{
			if(endpoint->HasAudio() || endpoint->HasCameraVideo() || endpoint->HasScreenVideo())
			{
				//����״̬��־
				CString itemText = _T("");
				itemText.Format(_T("%s(%s%s,%s%s%s)"), CString(identifier.c_str()), 
				identifier == m_contextStartParam.identifier ? _T("�Լ�,") : _T(""), 
				endpoint->HasAudio() ? _T("����˵��") : _T("û˵��"),
				endpoint->HasCameraVideo() || endpoint->HasScreenVideo() ? _T("������Ƶ") : _T("û��Ƶ"),
				endpoint->HasCameraVideo() ? _T("-����ͷ") : _T(""),
				endpoint->HasScreenVideo() ? _T("-��Ļ����") : _T(""));
				m_listEndpointList.AddString(itemText);
			}	

			//==========================��ѩ��12��26���Զ�ͬ�����Զ������ͷ��˷�
			if (m_identifier != identifier.c_str())
			{
				if (endpoint->HasAudio())
				{
					int retCode = m_sdkWrapper.MuteAudio(identifier, true);
				}
				if (endpoint->HasCameraVideo())
				{
					std::vector<std::string> identifierList = m_sdkWrapper.GetRequestViewIdentifierList();
					std::vector<View> viewList = m_sdkWrapper.GetRequestViewParamList();

					VideoSrcType videoSrcType = VIDEO_SRC_TYPE_CAMERA;
					int i = 0;
					for (; i < identifierList.size(); i++)
					{
						if (identifierList[i] == identifier && viewList[i].video_src_type == videoSrcType)
						{
							break;//�ظ�
						}
					}
					if (i >= identifierList.size())
					{
						identifierList.push_back(identifier);
						View view;
						view.size_type = VIEW_SIZE_TYPE_BIG;
						view.video_src_type = videoSrcType;
						viewList.push_back(view);

						int retCode = m_sdkWrapper.RequestViewList(identifierList, viewList);
						if (retCode != AV_OK)
						{
							int i = 0;
							for (; i < identifierList.size(); i++)
							{
								if (identifierList[i] == identifier && viewList[i].video_src_type == videoSrcType)
								{
									identifierList.erase(identifierList.begin() + i);
									viewList.erase(viewList.begin() + i);
									break;
								}
							}
						}
					}
				}
			}
			//===============================��ѩ��=========================
		}
	}
}

void DialogQAVSDKDemo::UpadateVideoRenderInfo()
{
	std::vector<std::string> identifierList = m_sdkWrapper.GetRequestViewIdentifierList();
	std::vector<View> viewList = m_sdkWrapper.GetRequestViewParamList();

	bool bNeedClearBig = true;
	for (int i = 0; i < identifierList.size(); i++)
	{
		AVEndpoint *endpoint = m_sdkWrapper.GetEndpointById(identifierList[i]);
		bool hasVideo = endpoint != NULL && ((viewList[i].video_src_type == VIDEO_SRC_TYPE_CAMERA && endpoint->HasCameraVideo()) 
			|| (viewList[i].video_src_type == VIDEO_SRC_TYPE_SCREEN && endpoint->HasScreenVideo()));

		if(m_viewBigVideoRender.m_identifier == identifierList[i] 
			&& m_viewBigVideoRender.m_videoSrcType == viewList[i].video_src_type && hasVideo)bNeedClearBig = false;			
	}

	if(bNeedClearBig)m_viewBigVideoRender.Clear();
			
	for (int i = 0; i < identifierList.size(); i++)
	{
		if (viewList[i].video_src_type == VIDEO_SRC_TYPE_CAMERA)
		{
			m_viewBigVideoRender.m_identifier = identifierList[i];
			m_viewBigVideoRender.m_videoSrcType = viewList[i].video_src_type;
			continue;
		}
		
	}
}

void DialogQAVSDKDemo::ShowRetCode(std::string tipsStr, int retCode)
{
  if (retCode != AV_OK)
  {
    CString errInfo = _T("");
    switch (retCode)
    {
    case AV_ERR_FAILED: 
      errInfo = _T("AV_ERR_FAILED");
      break;
    
    case AV_ERR_INVALID_ARGUMENT: 
      errInfo = _T("AV_ERR_INVALID_ARGUMENT");
      break;
    
    case AV_ERR_TIMEOUT: 
      errInfo = _T("AV_ERR_TIMEOUT");
      break;
    case AV_ERR_NOT_IMPLEMENTED: 
      errInfo = _T("AV_ERR_NOT_IMPLEMENTED");
      break;
    
   
    case AV_ERR_NOT_IN_MAIN_THREAD: 
      errInfo = _T("AV_ERR_NOT_IN_MAIN_THREAD");
      break;

      //CONTEXT���(-1401 to -1500)      
    case AV_ERR_CONTEXT_NOT_EXIST: 
      errInfo = _T("AV_ERR_CONTEXT_NOT_EXIST");
      break;

      //�������(-1501 to -1600)     
    
    case AV_ERR_ROOM_NOT_EXIST: 
      errInfo = _T("AV_ERR_ROOM_NOT_EXIST");
      break;
    case AV_ERR_ROOM_NOT_EXITED: 
      errInfo = _T("AV_ERR_ROOM_NOT_EXITED");
      break;	

      //�豸���(-1601 to -1700)   
   
    case AV_ERR_DEVICE_NOT_EXIST: 
      errInfo = _T("AV_ERR_DEVICE_NOT_EXIST");
      break;
    case AV_ERR_ENDPOINT_HAS_NOT_VIDEO: 
      errInfo = _T("AV_ERR_ENDPOINT_HAS_NOT_VIDEO");
      break;	
    case AV_ERR_REPEATED_OPERATION: 
      errInfo = _T("AV_ERR_REPEATED_OPERATION");
      break;   
    case AV_ERR_HAS_IN_THE_STATE: 
      errInfo = _T("AV_ERR_HAS_IN_THE_STATE");
      break;		

      //��Ա���(-1701 to -1800)
   
    case AV_ERR_ENDPOINT_NOT_EXIST: 
      errInfo = _T("AV_ERR_ENDPOINT_NOT_EXIST");
      break;
  
    default: 
      errInfo = _T("��������");
      break;
    }

    CString errCodeStr;
    errCodeStr.Format(_T("ʱ���������룺%d, ������ʾ��Ϣ: %s."), retCode, errInfo);
    errCodeStr = CString(tipsStr.c_str()) + errCodeStr;
    ShowMessageBox(errCodeStr);
  }
}

void DialogQAVSDKDemo::OnVideoPreTreatment(VideoFrame* pFrame)
{
    //if (m_bEnablePreTreat
    //    && pFrame 
    //    && pFrame->data 
    //    && pFrame->data_size 
    //    && pFrame->desc.color_format == COLOR_FORMAT_I420
    //    && m_pYUVData)
    //{
    //    AddImg_I420(pFrame->data, pFrame->desc.width, pFrame->desc.height, m_pYUVData, m_nYUVWidth, m_nYUVHeight);
    //}
}

LONG DialogQAVSDKDemo::OnAudioDeviceDetect(WPARAM wParam, LPARAM lParam)
{
	bool isSelect = (bool)wParam;
	DetectedDeviceInfo* pInfo = (DetectedDeviceInfo*)lParam;

	if (pInfo->flow == Detect_Mic)
	{
		std::string curMic;
		curMic = m_sdkWrapper.GetSelectedMicId();

		m_micList.clear();
		m_sdkWrapper.GetMicList(m_micList);
		
		if(m_micList.size() > 0)
		{
			if (curMic.empty())
			{
				m_sdkWrapper.SetSelectedMicId(m_micList[0].first);
				OpenMic();
			}
		}
		else
		{
			m_sdkWrapper.SetSelectedMicId("");
		}
	}

	else if (pInfo->flow == Detect_Speaker)
    {
		std::string curSpeaker;
		curSpeaker = m_sdkWrapper.GetSelectedPlayerId();

		m_playerList.clear();
		m_sdkWrapper.GetPlayerList(m_playerList);


		if(m_playerList.size() > 0)
		{	
			if (curSpeaker.empty())
			{
				m_sdkWrapper.SetSelectedPlayerId(m_playerList[0].first);
				OpenPlayer();
			}
		}
		else
		{
			m_sdkWrapper.SetSelectedPlayerId("");
		}
	}

	delete pInfo;
	return 0;

}

LONG DialogQAVSDKDemo::OnLButtonDBClick(WPARAM wParam, LPARAM lParam)
{
	/*int px = GET_X_LPARAM(lParam);
	int py = GET_Y_LPARAM(lParam);

	RECT rectParent = {0};
	GetWindowRect(&rectParent);

	RECT rect = {0};

	m_viewLocalVideoRender.GetWindowRect(&rect);
	rect.top -= rectParent.top;
	rect.bottom -= rectParent.top;
	rect.left -= rectParent.left;
	rect.right -= rectParent.left;

	if( rect.top < py && rect.bottom > py &&
		rect.left < px && rect.right > px)
	{
		m_viewBigVideoRender.m_identifier = m_viewLocalVideoRender.m_identifier;
		m_viewBigVideoRender.m_videoSrcType = m_viewLocalVideoRender.m_videoSrcType;
		return 0;
	}

	m_viewRemoteVideoSmallRender1.GetWindowRect(&rect);
	rect.top -= rectParent.top;
	rect.bottom -= rectParent.top;
	rect.left -= rectParent.left;
	rect.right -= rectParent.left;

	if( rect.top < py && rect.bottom > py &&
		rect.left < px && rect.right > px)
	{
		if(!m_viewRemoteVideoSmallRender1.m_identifier.empty())
		{
			m_viewBigVideoRender.m_identifier = m_viewRemoteVideoSmallRender1.m_identifier;
			m_viewBigVideoRender.m_videoSrcType = m_viewRemoteVideoSmallRender1.m_videoSrcType;
		}
		return 0;
	}

	m_viewRemoteVideoSmallRender2.GetWindowRect(&rect);
	rect.top -= rectParent.top;
	rect.bottom -= rectParent.top;
	rect.left -= rectParent.left;
	rect.right -= rectParent.left;

	if( rect.top < py && rect.bottom > py &&
		rect.left < px && rect.right > px)
	{
		if(!m_viewRemoteVideoSmallRender2.m_identifier.empty())
		{
			m_viewBigVideoRender.m_identifier = m_viewRemoteVideoSmallRender2.m_identifier;
			m_viewBigVideoRender.m_videoSrcType = m_viewRemoteVideoSmallRender2.m_videoSrcType;
		}
		return 0;
	}

	m_viewRemoteVideoSmallRender3.GetWindowRect(&rect);
	rect.top -= rectParent.top;
	rect.bottom -= rectParent.top;
	rect.left -= rectParent.left;
	rect.right -= rectParent.left;

	if( rect.top < py && rect.bottom > py &&
		rect.left < px && rect.right > px)
	{
		if(!m_viewRemoteVideoSmallRender3.m_identifier.empty())
		{
			m_viewBigVideoRender.m_identifier = m_viewRemoteVideoSmallRender3.m_identifier;
			m_viewBigVideoRender.m_videoSrcType = m_viewRemoteVideoSmallRender3.m_videoSrcType;
		}
		return 0;
	}

	m_viewRemoteVideoSmallRender4.GetWindowRect(&rect);
	rect.top -= rectParent.top;
	rect.bottom -= rectParent.top;
	rect.left -= rectParent.left;
	rect.right -= rectParent.left;
	if( rect.top < py && rect.bottom > py &&
		rect.left < px && rect.right > px)
	{
		if(!m_viewRemoteVideoSmallRender4.m_identifier.empty())
		{
			m_viewBigVideoRender.m_identifier = m_viewRemoteVideoSmallRender4.m_identifier;
			m_viewBigVideoRender.m_videoSrcType = m_viewRemoteVideoSmallRender4.m_videoSrcType;
		}
		return 0;
	}*/

	return 0;
}

void DialogQAVSDKDemo::OnIMNewMessage(int n)
{

}

void DialogQAVSDKDemo::OnReceive(CcDataBase *data){
	int n = data->cmdId_;
}

void DialogQAVSDKDemo::OpenCamera()
{
	ShowOperationTips(m_operationTipsOpenCameraing, 1000);
	m_viewLocalVideoRender.m_identifier = m_contextStartParam.identifier;
	m_viewLocalVideoRender.m_videoSrcType = VIDEO_SRC_TYPE_CAMERA;

	int retCode = m_sdkWrapper.OpenCamera();
	if (retCode != AV_OK)ClearOperationTips();
	if (retCode != AV_OK)
	{
		m_viewLocalVideoRender.m_identifier = "";
	}
}

void DialogQAVSDKDemo::OpenMic()
{
	int retCode = m_sdkWrapper.OpenMic();
	if (retCode == AV_OK)
	{
		uint32 micVolume = m_sdkWrapper.GetMicVolume();
		CString strMicVolume = _T("");
		strMicVolume.Format(_T("%d"), micVolume);
		m_sliderAudioMicVolume.SetPos(micVolume);
		m_staticMicCurVolume.SetWindowTextW(strMicVolume);
	}
	else
	{

	}
	//ShowRetCode("OpenMic", retCode);
}

void DialogQAVSDKDemo::OpenPlayer()
{
	int retCode = m_sdkWrapper.OpenPlayer();
	if (retCode == AV_OK)
	{
		uint32 playerVolume = m_sdkWrapper.GetPlayerVolume();
		CString strPlayerVolume = _T("");
		strPlayerVolume.Format(_T("%d"), playerVolume);
		m_sliderAudioPlayerVolume.SetPos(playerVolume);
		m_staticPlayerCurVolume.SetWindowTextW(strPlayerVolume);
	}
	else
	{

	}
	//ShowRetCode("OpenPlayer", retCode);
}

void DialogQAVSDKDemo::Login()
{
	//����crash�ϱ�
	m_sdkWrapper.EnableCrashReport(true);

	if (m_isLogin)
	{
		//ShowMessageBox(_T("�Ѿ���¼��"));
		return;
	}

	if (m_relationId== 0)
	{
		//ShowMessageBox(_T("Ⱥ��Id����Ϊ�գ�"));
		return;
	}

	//�л���̨����, 1��ʾ���Ի�����0��ʾ��ʽ����
	TIMManager::get().set_env(0);

	//����ģʽ�����Ҫ֧����־�ϱ����ܣ���Ҫ��ģʽ����Ϊ"1"��
	TIMManager::get().set_mode(1);

	//�ȳ�ʼ��ͨ��SDK
	TIMManager::get().Init();

	//TIMManager::get().SetMessageCallBack(&m_msgWrapper);
	//m_msgWrapper.SetListner(this);

	//����reciver�ļ���
	CcReciver *reciver = CcReciver::GetInstance();

	//����observer�Ļص�
	CcTestObserver  * observer = new CcTestObserver();
	observer->setObserverCallBack((CcObserverBaseCallBack *)this);
	reciver->AddObserver(observer);

	SetMainHWnd(this->m_hWnd);

	//��ѶΪÿ�����뷽������˺�����
	CString accountType = m_accountType;

	//Appʹ�õ�OAuth��Ȩ��ϵ�����AppId
	CString appIdAt3rd = m_appIdAt3rd;

	//�û��˺�
	CString identifier = m_identifier;

	//�û�����
	CString userSig = ConfigInfoMgr::GetInst()->GetConfigInfo().userSig;

	//��ѶΪÿ��ʹ��SDK��App�����AppId
	CString sdkAppId = m_SdkAppId;

	if (identifier.GetLength() > 0)
	{
		userSig = ConfigInfoMgr::GetInst()->GetConfigInfo().userSig;
	}
	else
	{
		return;
	}

	if (accountType.GetLength() > 0 && appIdAt3rd.GetLength() > 0
		&& identifier.GetLength() > 0 && userSig.GetLength() > 0
		&& sdkAppId.GetLength() > 0)
	{

		m_contextStartParam.account_type = StrWToStrA(accountType);
		m_contextStartParam.app_id_at3rd = StrWToStrA(appIdAt3rd);
		m_contextStartParam.sdk_app_id = _ttoi(sdkAppId);
		m_contextStartParam.identifier = StrWToStrA(identifier);
		m_userSig = StrWToStrA(userSig);


		//�˺ŵ�¼����
		//��д��¼ʹ�õ��û���Ϣ
		TIMUser user;
		user.set_account_type(m_contextStartParam.account_type);
		user.set_app_id_at_3rd(m_contextStartParam.app_id_at3rd);
		user.set_identifier(m_contextStartParam.identifier);
		//��ʼ����ͨ��SDK�󣬿�ʼ��¼
		ShowOperationTips(m_operationTipsLogining, 1000);

		//�������ͣ���ѶΪÿ��ʹ��SDK��App�����AppId���û���Ϣ���û����룬��¼�ɹ���ʧ�ܵĻص�������
		TIMManager::get().Login(m_contextStartParam.sdk_app_id, user, m_userSig, &m_loginCallBack);
	}
	else
	{
		//ShowMessageBox(_T("APP������Ϣ����Ϊ�գ�"));
		return;
	}
}
void DialogQAVSDKDemo::EnterRoom()
{
	UpdateData();
	if (!m_isLogin)
	{
		return;
	}

	if (m_relationId < 1 || m_relationId > 4294967295)
	{
		return;
	}

	AVRoomMulti::EnterParam enterRoomParam;
	enterRoomParam.relation_id = m_relationId;
	enterRoomParam.control_role = "";

	enterRoomParam.auth_bits = AUTH_BITS_DEFAULT;
	enterRoomParam.audio_category = AUDIO_CATEGORY_MEDIA_PLAY_AND_RECORD;

	enterRoomParam.create_room = true;

	//�Զ�����������Ƶ
	enterRoomParam.video_recv_mode = VIDEO_RECV_MODE_SEMI_AUTO_RECV_CAMERA_VIDEO;

	int retCode = m_sdkWrapper.EnterRoom(enterRoomParam);
//	if (retCode != AV_OK)ClearOperationTips();
//#ifdef ENABLE_UI_OPERATION_SAFETY
//	if (retCode != AV_OK)m_btnEnterRoom.EnableWindow(TRUE);
//#endif
//	ShowRetCode("EnterRoom", retCode);
}
void DialogQAVSDKDemo::PostMsgToMain(int message,WPARAM wParam, LPARAM lParam)
{
	HWND xp = ::FindWindow(NULL, _T("ħ��СƸ"));
	::PostMessage(xp, XPVIDEO_LOGIN, wParam, lParam);
}