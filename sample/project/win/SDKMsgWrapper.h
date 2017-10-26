#pragma once
#include "tim.h"
#include "tim_comm.h"
#include "tim_int.h"
#include <tim_msg.h>

using namespace imcore;

class MsgListener{
public:
	virtual void OnIMNewMessage(int n) = 0;
};


class SDKMsgWrapper:public TIMMessageCallBack
{
public:
	void SetListner(MsgListener*lis){this->listener_ = lis;}
	SDKMsgWrapper(void);
	~SDKMsgWrapper(void);

protected:
	virtual void OnNewMessage(const std::vector<TIMMessage> &msgs);

	MsgListener*listener_;
};

