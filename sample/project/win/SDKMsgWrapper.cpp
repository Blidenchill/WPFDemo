#include "StdAfx.h"
#include "SDKMsgWrapper.h"
#include "Util.h"
#include "CustomWinMsg.h"

SDKMsgWrapper::SDKMsgWrapper(void)
{
    listener_ = NULL;
}


SDKMsgWrapper::~SDKMsgWrapper(void)
{

}

void SDKMsgWrapper::OnNewMessage(const std::vector<TIMMessage> &msgs)
{
	for (size_t n = 0; n < msgs.size(); n++)
	{
		for (size_t t = 0; t < msgs[n].GetElemCount(); t++)
		{
			TIMElem * elem = msgs[n].GetElem(t);
			
			if (elem && elem->type() == kElemCustom)
			{
				TIMCustomElem*costomEle = (TIMCustomElem*)elem;
				int op = *(int*)(costomEle->data().c_str());

 			//	if (listener_){
 			//		listener_->OnIMNewMessage(op);
 			//	}
			}
		}
	

	}
}

