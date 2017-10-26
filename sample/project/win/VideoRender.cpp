// VideoRender.cpp : implementation file
//

#include "stdafx.h"
#include "QAVSDKApp.h"
#include "VideoRender.h"
#include "gdiplus.h"
#include "CustomWinMsg.h"
#include "Util.h"

#define VIEW_BKG_COLOR 0x60

using namespace Gdiplus;

IMPLEMENT_DYNAMIC(VideoRender, CWnd)

VideoRender::VideoRender()
{
	m_bRenderHasData = false;
	m_bRenderPaint = false;
	m_pFrameDataBuf = NULL;
	m_pBkgDataBuf = NULL;
	m_frameDataBufLen = 0;
	m_frameWidth = 0;
	m_frameHeight = 0;
	m_colorFormat = COLOR_FORMAT_RGB24;

	m_pRenderDataBuf = NULL;
	m_nRenderDataBufLen = 0;

	m_identifier = "";
	m_videoSrcType = VIDEO_SRC_TYPE_NONE;
}

VideoRender::~VideoRender()
{	
	if(m_pFrameDataBuf != NULL)
	{
		delete[]m_pFrameDataBuf;
		m_pFrameDataBuf = NULL;
	}

	if(m_pBkgDataBuf != NULL)
	{
		delete[]m_pBkgDataBuf;
		m_pBkgDataBuf = NULL;
	}

	if(m_pRenderDataBuf != NULL)
	{
		delete[]m_pRenderDataBuf;
		m_pRenderDataBuf = NULL;
	}
}

BEGIN_MESSAGE_MAP(VideoRender, CWnd)
	ON_WM_PAINT()
	ON_WM_TIMER()
END_MESSAGE_MAP()

void VideoRender::OnPaint()
{
	uint8 *pData = m_bRenderPaint ? m_pFrameDataBuf : m_pBkgDataBuf;

	CPaintDC dc(this);
	CWindowDC wdc(this);
	HDC hDc = wdc.m_hDC;

	//过来的图像一定是4：3比例
    RECT winRect = {0};
	GetClientRect(&winRect);

    int winWidth = winRect.right - winRect.left;
	int winHeight = winRect.bottom - winRect.top;	

	int srcWidth = m_frameWidth;
	int srcHeight = m_frameHeight;

	int dstWidth = 0;
	int dstHeight = 0;

	//宽小于高，加黑边
    if(winWidth > winHeight)
    {
	    if(m_bRenderPaint && srcWidth < srcHeight)
	    {
		    //int tempHeight = srcHeight;
		    //if(tempHeight % 3)tempHeight = tempHeight - (tempHeight % 3) + 3;
		    dstWidth = srcHeight * srcHeight / srcWidth;		
		    dstHeight = srcHeight;
	    }
	    else
	    {
		    dstWidth = srcWidth;
		    dstHeight = srcHeight;
	    }
    }
    else
    {
        dstWidth = srcWidth;
		dstHeight = srcHeight;
    }

	dstWidth = (dstWidth + 3) / 4 * 4;//保证宽度是4的倍数。

	if(m_nRenderDataBufLen != dstWidth * dstHeight * 3)
	{
		if(m_pRenderDataBuf != NULL)
		{
			delete [] m_pRenderDataBuf;
			m_pRenderDataBuf = NULL;
		}

		m_nRenderDataBufLen = dstWidth * dstHeight * 3;//RGB24
		m_pRenderDataBuf = new uint8[m_nRenderDataBufLen];
		memset(m_pRenderDataBuf, 0 , m_nRenderDataBufLen);
	}
	
	memset(m_pRenderDataBuf, 0 , m_nRenderDataBufLen);//清除上一次渲染。

	SIZE srcSize = {srcWidth, srcHeight};
	SIZE dstSize = {dstWidth, dstHeight};
	_ResizeWithMendBlack(m_pRenderDataBuf, pData, m_nRenderDataBufLen, m_frameDataBufLen, dstSize, srcSize, 3);

	BITMAPINFO Bitmap;
	memset(&Bitmap,0,sizeof(BITMAPINFO));
	Bitmap.bmiHeader.biSize = sizeof(BITMAPINFOHEADER);		
	Bitmap.bmiHeader.biWidth = dstWidth;
	Bitmap.bmiHeader.biHeight = -dstHeight;

	Bitmap.bmiHeader.biBitCount = 3 * 8;//COLOR_FORMAT_RGB24	
	Bitmap.bmiHeader.biPlanes = 1;	
	Bitmap.bmiHeader.biCompression = BI_RGB;//COLOR_FORMAT_RGB24	
	Bitmap.bmiHeader.biSizeImage = 0;	
	Bitmap.bmiHeader.biClrUsed = 0;
	Bitmap.bmiHeader.biXPelsPerMeter = 0;
	Bitmap.bmiHeader.biYPelsPerMeter = 0;	
	Bitmap.bmiHeader.biClrImportant = 0;
    
	/*
	add by: zhuojun
	改为GDI双缓冲,解决绘制有可能导致的闪烁。
	*/

	HDC hMemDC = CreateCompatibleDC(hDc);
	HBITMAP hMemBitmap= CreateCompatibleBitmap(hDc, winWidth, winHeight);
	SelectObject(hMemDC, hMemBitmap);

	SetStretchBltMode(hMemDC, HALFTONE);
	SetBrushOrgEx(hMemDC, 0, 0, NULL);
	StretchDIBits(hMemDC, 0, 0, winWidth, winHeight, 0, 0, dstWidth, dstHeight,		
		m_pRenderDataBuf, &Bitmap, DIB_RGB_COLORS, SRCCOPY);
  
	BitBlt(hDc, 0, 0, winWidth, winHeight, hMemDC, 0, 0, SRCCOPY);

	/*if(m_identifier != "")
	{
		CString identifier = CString(m_identifier.c_str());
		CString str;
		str.Format(_T("%s, %u*%u"), identifier, m_frameWidth, m_frameHeight);
		CSize sz = dc.GetTextExtent(str);

		CRect rect(0, 0, sz.cx, 200 + sz.cy);
		dc.SetTextColor(RGB(0xff,0,0));
		dc.DrawText(str, &rect, DT_LEFT);
	}*/	
	
	DeleteObject(hMemBitmap);
	DeleteObject(hMemDC);
}

void VideoRender::Init(int width, int height, ColorFormat colorFormat)
{
	ASSERT(colorFormat == COLOR_FORMAT_RGB24);//SDK1.3版本，demo渲染模块只支持渲染COLOR_FORMAT_RGB24格式的图像。

	m_frameWidth = width;
	m_frameHeight = height;
	m_colorFormat = colorFormat;
	m_frameDataBufLen = m_frameWidth * m_frameHeight * 3;//COLOR_FORMAT_RGB24
	m_pFrameDataBuf = new uint8[m_frameDataBufLen];
	m_pBkgDataBuf = new uint8[m_frameDataBufLen];
	memset(m_pFrameDataBuf, 0, m_frameDataBufLen);
	memset(m_pBkgDataBuf, VIEW_BKG_COLOR, m_frameDataBufLen);

	Clear();
}

void VideoRender::DoRender(VideoFrame* frame)
{
	if (frame == NULL || frame->data_size == 0)
	{		
		return;
	}

	ASSERT(frame->desc.color_format == COLOR_FORMAT_RGB24);//SDK1.3版本，demo渲染模块只支持渲染COLOR_FORMAT_RGB24格式的图像。

	if(m_frameDataBufLen != frame->data_size)
	{
		if(m_pFrameDataBuf != NULL)
		{
			delete[]m_pFrameDataBuf;
			m_pFrameDataBuf = NULL;
		}

		if(m_pBkgDataBuf != NULL)
		{
			delete[]m_pBkgDataBuf;
			m_pBkgDataBuf = NULL;
		}

		m_colorFormat = frame->desc.color_format;
		m_frameDataBufLen = frame->data_size;
		m_pFrameDataBuf = new uint8[m_frameDataBufLen];
		m_pBkgDataBuf = new uint8[m_frameDataBufLen];
		memset(m_pFrameDataBuf, 0, m_frameDataBufLen);
		memset(m_pBkgDataBuf, VIEW_BKG_COLOR, m_frameDataBufLen);
	}

	static void (* fn_table[4])(uint8*, uint8*, UINT, const SIZE &) = {&_CopyBits2Tex_None_0, &_CopyBits2Tex_None_90, &_CopyBits2Tex_None_180, &_CopyBits2Tex_None_270};

	if(frame->desc.rotate == 0)//0
	{
		m_frameWidth = frame->desc.width;
		m_frameHeight = frame->desc.height;
	}
	else if(frame->desc.rotate == 1)//90
	{
		m_frameWidth = frame->desc.height;
		m_frameHeight = frame->desc.width;
	}
	else if(frame->desc.rotate == 2)//180
	{
		m_frameWidth = frame->desc.width;
		m_frameHeight = frame->desc.height;
	}
	else if(frame->desc.rotate == 3)//270
	{
		m_frameWidth = frame->desc.height;
		m_frameHeight = frame->desc.width;
	}

	SIZE size = {frame->desc.width, frame->desc.height};
	memset(m_pFrameDataBuf, 0, m_frameDataBufLen);
	fn_table[frame->desc.rotate](m_pFrameDataBuf, frame->data, m_frameDataBufLen, size);
	/*
    add by: zhuojun
    这里要说明的是： InvalidateRect & UpdateWindow 相当于同步发送UI线程WM_PAINT消息，
    线程上下文是UI线程，当前调用线程将会被阻塞。(阻塞不是问题)
	*/
	m_bRenderHasData = true;
	m_bRenderPaint = true;
	InvalidateRect(NULL);
	UpdateWindow();
	m_bRenderPaint = false;
}

void VideoRender::Clear()
{
	/*
    add by: zhuojun
    这里要说明的是： InvalidateRect & UpdateWindow 相当于同步发送UI线程WM_PAINT消息，
    线程上下文是UI线程，当前调用线程将会被阻塞。(阻塞不是问题)
    最好不要加锁，小心死锁的问题。
	*/
	m_bRenderHasData = false;
	m_bRenderPaint = false;
	m_identifier = "";
	m_videoSrcType = VIDEO_SRC_TYPE_NONE;

	if(m_pFrameDataBuf != NULL)
	{
		delete[]m_pFrameDataBuf;
		m_pFrameDataBuf = NULL;
	}

	if(m_pBkgDataBuf != NULL)
	{
		delete[]m_pBkgDataBuf;
		m_pBkgDataBuf = NULL;
	}

	m_frameDataBufLen = m_frameWidth * m_frameHeight * 3;//COLOR_FORMAT_RGB24
	m_pFrameDataBuf = new uint8[m_frameDataBufLen];
	m_pBkgDataBuf = new uint8[m_frameDataBufLen];
	memset(m_pFrameDataBuf, 0, m_frameDataBufLen);
	memset(m_pBkgDataBuf, VIEW_BKG_COLOR, m_frameDataBufLen);

	InvalidateRect(NULL);
	UpdateWindow();
}

BOOL VideoRender::Create(LPCTSTR lpszClassName, LPCTSTR lpszWindowName, DWORD dwStyle, const RECT& rect, CWnd* pParentWnd, UINT nID, CCreateContext* pContext)
{
	// TODO: Add your specialized code here and/or call the base class
	BOOL bRet = CWnd::Create(lpszClassName, lpszWindowName, dwStyle, rect, pParentWnd, nID, pContext);
	return bRet;
}

BOOL VideoRender::SnapShot(const char* szBmpName)
{
  if(!m_bRenderHasData)
    return FALSE;

  char szBmpPath[MAX_PATH] = {0};

  GetModuleFileNameA(NULL, szBmpPath, MAX_PATH);
  PathAppendA(szBmpPath, "..\\");
  PathAppendA(szBmpPath, szBmpName);

  FILE* file = fopen(szBmpPath, "wb+");

  //bmp file header
  BITMAPFILEHEADER bmpHeader;
  memset(&bmpHeader,0,sizeof(BITMAPFILEHEADER));
  bmpHeader.bfSize = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) +  (m_frameWidth * m_frameHeight * 3);
  bmpHeader.bfType = 0x4D42;
  bmpHeader.bfOffBits = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER);
  bmpHeader.bfReserved1 = 0;
  bmpHeader.bfReserved2 = 0;
  fwrite(&bmpHeader, sizeof(bmpHeader), 1, file);

  //bmp data header
  BITMAPINFOHEADER bmiHeader;
  memset(&bmiHeader,0,sizeof(BITMAPINFOHEADER));
	bmiHeader.biSize = sizeof(BITMAPINFOHEADER);		
	bmiHeader.biWidth = m_frameWidth;
	bmiHeader.biHeight = -m_frameHeight;
	bmiHeader.biBitCount = 3 * 8;//COLOR_FORMAT_RGB24	
	bmiHeader.biPlanes = 1;	
	bmiHeader.biCompression = BI_RGB;//COLOR_FORMAT_RGB24	
	bmiHeader.biSizeImage = 0;	
	bmiHeader.biClrUsed = 0;
	bmiHeader.biXPelsPerMeter = 0;
	bmiHeader.biYPelsPerMeter = 0;	
	bmiHeader.biClrImportant = 0;
  fwrite(&bmiHeader, sizeof(bmiHeader), 1, file);

  //bmp data data
  fwrite(m_pFrameDataBuf, sizeof(uint8), (m_frameWidth * m_frameHeight * 3), file);
  fclose(file);

  return TRUE;
}