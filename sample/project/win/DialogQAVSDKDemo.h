
// DialogQAVSDKDemo.h : header file
//

#pragma once
#include "afxwin.h"
#include "resource.h"	
#include "VideoRender.h"
#include "SdkWrapper.h"
#include "CustomWinMsg.h"
#include "tim.h"
#include "tim_comm.h"
#include "tim_int.h"
#include "Util.h"
#include  "SDKMsgWrapper.h"
#include "AVCcReciver.h"
#include "AVCcObserver.h"
#include "AVCcTask.h"
using namespace imcore;
using namespace tencent::av;

class LoginCallBack : public TIMCallBack 
{
	virtual void OnSuccess();
	virtual void OnError(int retCode, const std::string &desc);
};

class LogoutCallBack : public TIMCallBack 
{
	virtual void OnSuccess();
	virtual void OnError(int retCode, const std::string &desc);
};


// DialogQAVSDKDemo dialog
class DialogQAVSDKDemo : public CDialogEx, public MsgListener,public CcObserverTestCallBack
{
	friend class SdkWrapper;
// Construction
public:
	DialogQAVSDKDemo(CWnd* pParent = NULL);	// standard constructor
	~DialogQAVSDKDemo();
// Dialog Data
	enum { IDD = IDD_DIALOG_DEMO };
	void OnIMNewMessage(int n);
	void OnReceive(CcDataBase *data);
	void ccCompleteCallBack(int result,std::string error_info);
protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	virtual BOOL OnInitDialog();
	virtual BOOL PreTranslateMessage(MSG* pMsg);

	// msg or command
  //系统部分
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg LONG OnGetDefId(WPARAM wp, LPARAM lp);
	afx_msg void OnClose();
	afx_msg void OnDestroy();
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg void OnTimer(UINT_PTR eventId);
	afx_msg void TrackMenu(CWnd*pWnd);
	afx_msg void OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);


  //用户自定义部分	
	afx_msg LONG OnLogin(WPARAM wParam, LPARAM lParam); 
	afx_msg LONG OnLogout(WPARAM wParam, LPARAM lParam); 
	afx_msg LONG OnStartContext(WPARAM wParam, LPARAM lParam); 
	afx_msg LONG OnStopContext(WPARAM wParam, LPARAM lParam); 
	afx_msg LONG OnEnterRoom(WPARAM wParam, LPARAM lParam); 
	afx_msg LONG OnExitRoom(WPARAM wParam, LPARAM lParam); 	
	afx_msg LONG OnRequestViewList(WPARAM wParam, LPARAM lParam); 
	afx_msg LONG OnCancelAllView(WPARAM wParam, LPARAM lParam); 
	afx_msg LONG OnCameraOperation(WPARAM wParam, LPARAM lParam); 
    afx_msg LONG OnCameraChange(WPARAM wParam, LPARAM lParam); 
	afx_msg LONG OnMicOperation(WPARAM wParam, LPARAM lParam); 
	afx_msg LONG OnPlayerOperation(WPARAM wParam, LPARAM lParam); 	
	afx_msg LONG OnEnpointsUpdateInfo(WPARAM wParam, LPARAM lParam); 
	afx_msg LONG OnAudioDeviceDetect(WPARAM wParam, LPARAM lParam); 
	afx_msg LONG OnLButtonDBClick(WPARAM wParam, LPARAM lParam); 

  
	DECLARE_MESSAGE_MAP()

	void OnVideoPreTreatment(VideoFrame* pFrame);

private:
	void UpadateVideoRenderInfo();	
	void UpdateEndpointList();
	void ShowRetCode(std::string tipsStr, int retCode);
	void ShowOperationTips(CString tips, int timeLen);
	void ClearOperationTips();
	void PrepareEncParam();

private:
	HICON m_hIcon;

	CListBox m_listEndpointList;	  

	CEdit m_editAccountType;
	CEdit m_editAppIdAt3rd;
	CEdit m_editSdkAppId;
    UINT m_relationId;
    CEdit m_editRelationId; // asam todo 统一命名为relation_id
	CEdit m_editAccountID;

	CStatic m_staticOperationTips;
	CStatic m_staticMicCurVolume;	
	CStatic m_staticPlayerCurVolume;		
	
	CSliderCtrl m_sliderAudioMicVolume;
	CSliderCtrl m_sliderAudioPlayerVolume;

	CString m_identifier;
	CString m_accountType;
	CString m_appIdAt3rd;
	CString m_SdkAppId;
	CString m_accountID;
public:
	SdkWrapper m_sdkWrapper;	

private:
    AVContext::StartParam m_contextStartParam;
	std::string m_userSig;

	VideoRender m_viewLocalVideoRender;
	VideoRender m_viewBigVideoRender;
  
	bool m_isExitDemo;
	bool m_isLogin;
	bool m_isEnterRoom;	

	LoginCallBack m_loginCallBack;
	LogoutCallBack m_logoutCallBack;


	int m_mainDialogLeft;
	int m_mainDialogTop;
	int m_mainDialogWidth;
	int m_mainDialogHeight;

	//一些比较耗时的操作提示
	CString m_operationTipsLogining;
	CString m_operationTipsLogouting;
	CString m_operationTipsEnterRooming;
	CString m_operationTipsExitRooming;
	CString m_operationTipsOpenCameraing;
	CString m_operationTipsCloseCameraing;
	CString m_operationTipsReqeustViewing;
	CString m_operationTipsCancelViewing;

	std::vector<std::pair<std::string/*id*/, std::string/*name*/> > m_micList;
	std::vector<std::pair<std::string/*id*/, std::string/*name*/> > m_playerList;
	std::vector<std::pair<std::string/*id*/, std::string/*name*/> > m_cameraList;

private:
	SDKMsgWrapper m_msgWrapper;

public:
	void OpenCamera();
	void OpenMic();
	void OpenPlayer();
	void Login();
	void EnterRoom();
	void PostMsgToMain(int message, WPARAM wParam, LPARAM lParam);
};
