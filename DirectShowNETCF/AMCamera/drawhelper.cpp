#include "stdafx.h"
#include "drawhelper.hpp"

//============================ RGB565 =============================//
void CRGB565DrawHelper::DrawText(const void * textPtr, void * pBuff, int textWidth, int textHeight)
{
	if (textPtr)
	{
		int drawWidth = (textWidth <= width_) ? textWidth : width_;

		for (int i = 0; i < textHeight; i ++)
		{
			unsigned short * dst = static_cast<unsigned short * >(pBuff) + i * width_;
			unsigned char * src = static_cast<unsigned char * >(const_cast<void *>(textPtr)) + i * textWidth;
			for (int j = 0; j < drawWidth; j++)
			{
				if (*(src + j))
				{
					*(dst + j) = 0;
				}
			}
		}
	}
}
	
void CRGB565DrawHelper::DrawTarget(int target, void * pBuff, RECT rect)
{
	if (target)
	{
		if (target & RECTANGLE)
		{
			unsigned short *dst = reinterpret_cast<unsigned short *>(pBuff);
			
			int i = 0;
			for (i = rect.left; i < rect.right; ++i)
			{
				dst[(rect.top - 1) * width_ + i] = 0xF800;
				dst[(rect.top) * width_ + i] = 0xF800;
				dst[(rect.top + 1) * width_ + i] = 0xF800;
				dst[(rect.bottom - 1) * width_ + i] = 0xF800;
				dst[(rect.bottom) * width_ + i] = 0xF800;
				dst[(rect.bottom + 1) * width_ + i] = 0xF800;
			}

			for (i = rect.top; i < rect.bottom; ++i)
			{
				dst[i * width_ + rect.left - 1] = 0xF800;
				dst[i * width_ + rect.left] = 0xF800;
				dst[i * width_ + rect.left + 1] = 0xF800;	

				dst[i * width_ + rect.right - 1] = 0xF800;
				dst[i * width_ + rect.right] = 0xF800;
				dst[i * width_ + rect.right + 1] = 0xF800;
			}
		}
		else if (target & TARGET)
		{
			int x = rect.left;
			int y = rect.top;

			int QUATER_WIDTH = (rect.right - x) >> 2;
			int QUATER_HEIGHT = (rect.bottom - y) >> 2;

			unsigned short *dst = reinterpret_cast<unsigned short *>(pBuff);
		
			int i = 0;
			for (i = x - 1; i < x + QUATER_WIDTH; ++i)
			{
				dst[(y - 1) * width_ + i] = 0xF800;
				dst[y * width_ + i] = 0xF800;
				dst[(y + 1) * width_ + i] = 0xF800;
				dst[(y - 1) * width_ + i + 3 * QUATER_WIDTH] = 0xF800;
				dst[y * width_ + i + 3 * QUATER_WIDTH] = 0xF800;
				dst[(y + 1) * width_ + i + 3 * QUATER_WIDTH] = 0xF800;

				dst[(y + 4 * QUATER_HEIGHT - 1) * width_ + i] = 0xF800;
				dst[(y + 4 * QUATER_HEIGHT) * width_ + i] = 0xF800;
				dst[(y + 4 * QUATER_HEIGHT + 1) * width_ + i] = 0xF800;
				dst[(y + 4 * QUATER_HEIGHT - 1) * width_ + i + 3 * QUATER_WIDTH] = 0xF800;
				dst[(y + 4 * QUATER_HEIGHT) * width_ + i + 3 * QUATER_WIDTH] = 0xF800;
				dst[(y + 4 * QUATER_HEIGHT + 1) * width_ + i + 3 * QUATER_WIDTH] = 0xF800;
			}

			for (i = y - 1; i < y + QUATER_HEIGHT; ++i)
			{
				dst[i * width_ + x - 1] = 0xF800;
				dst[i * width_ + x] = 0xF800;
				dst[i * width_ + x + 1] = 0xF800;
				dst[(i + 3 * QUATER_HEIGHT) * width_ + x - 1] = 0xF800;
				dst[(i + 3 * QUATER_HEIGHT) * width_ + x] = 0xF800;
				dst[(i + 3 * QUATER_HEIGHT) * width_ + x + 1] = 0xF800;

				dst[i * width_ + x + 4 * QUATER_WIDTH - 1] = 0xF800;
				dst[i * width_ + x + 4 * QUATER_WIDTH] = 0xF800;
				dst[i * width_ + x + 4 * QUATER_WIDTH + 1] = 0xF800;
				dst[(i + 3 * QUATER_HEIGHT) * width_ + x + 4 * QUATER_WIDTH - 1] = 0xF800;
				dst[(i + 3 * QUATER_HEIGHT) * width_ + x + 4 * QUATER_WIDTH] = 0xF800;
				dst[(i + 3 * QUATER_HEIGHT) * width_ + x + 4 * QUATER_WIDTH + 1] = 0xF800;
			}
		}
	}
}

//============================= YV12 ==============================//

void CYV12DrawHelper::DrawText(const void * textPtr, void * pBuff, int textWidth, int textHeight)
{
	if (textPtr)
	{
		int drawWidth = (textWidth <= width_) ? textWidth : width_;

		for (int i = 0; i < textHeight; ++i)
		{
			unsigned char * dst = static_cast<unsigned char * >(pBuff) + i * width_;
			unsigned char * src = static_cast<unsigned char * >(const_cast<void *>(textPtr)) + i * textWidth;
			for (int j = 0; j < drawWidth; ++j)
			{
				if (*(src + j))
				{
					*(dst + j) = 0;
				}
			}
		}
	}
}
	
void CYV12DrawHelper::DrawTarget(int target, void * pBuff, RECT rect)
{
	if (target)
	{
		if (target & RECTANGLE)
		{
			unsigned char * dst = static_cast<unsigned char *>(pBuff);
			
			int i = 0;
			for (i = rect.left; i < rect.right; ++i)
			{
				dst[(rect.top - 1) * width_ + i] = 0xFF;
				dst[(rect.top) * width_ + i] = 0xFF;
				dst[(rect.top + 1) * width_ + i] = 0xFF;
				dst[(rect.bottom - 1) * width_ + i] = 0xFF;
				dst[(rect.bottom) * width_ + i] = 0xFF;
				dst[(rect.bottom + 1) * width_ + i] = 0xFF;
			}

			for (i = rect.top; i < rect.bottom; ++i)
			{
				dst[i * width_ + rect.left - 1] = 0xFF;
				dst[i * width_ + rect.left] = 0xFF;
				dst[i * width_ + rect.left + 1] = 0xFF;	

				dst[i * width_ + rect.right - 1] = 0xFF;
				dst[i * width_ + rect.right] = 0xFF;
				dst[i * width_ + rect.right + 1] = 0xFF;
			}
		}
		else if (target & TARGET)
		{
			int x = rect.left;
			int y = rect.top;

			int QUATER_WIDTH = (rect.right - x) >> 2;
			int QUATER_HEIGHT = (rect.bottom - y) >> 2;
		
			unsigned char * dst = static_cast<unsigned char *>(pBuff);

			int i = 0;
			for (i = x - 1; i < x + QUATER_WIDTH; ++i)
			{
				dst[(y - 1) * width_ + i] = 0xFF;
				dst[y * width_ + i] = 0xFF;
				dst[(y + 1) * width_ + i] = 0xFF;
				dst[(y - 1) * width_ + i + 3 * QUATER_WIDTH] = 0xFF;
				dst[y * width_ + i + 3 * QUATER_WIDTH] = 0xFF;
				dst[(y + 1) * width_ + i + 3 * QUATER_WIDTH] = 0xFF;

				dst[(y + 4 * QUATER_HEIGHT - 1) * width_ + i] = 0xFF;
				dst[(y + 4 * QUATER_HEIGHT) * width_ + i] = 0xFF;
				dst[(y + 4 * QUATER_HEIGHT + 1) * width_ + i] = 0xFF;
				dst[(y + 4 * QUATER_HEIGHT - 1) * width_ + i + 3 * QUATER_WIDTH] = 0xFF;
				dst[(y + 4 * QUATER_HEIGHT) * width_ + i + 3 * QUATER_WIDTH] = 0xFF;
				dst[(y + 4 * QUATER_HEIGHT + 1) * width_ + i + 3 * QUATER_WIDTH] = 0xFF;
			}	

			for (i = y - 1; i < y + QUATER_HEIGHT; ++i)
			{
				dst[i * width_ + x - 1] = 0xFF;
				dst[i * width_ + x] = 0xFF;
				dst[i * width_ + x + 1] = 0xFF;
				dst[(i + 3 * QUATER_HEIGHT) * width_ + x - 1] = 0xFF;
				dst[(i + 3 * QUATER_HEIGHT) * width_ + x] = 0xFF;
				dst[(i + 3 * QUATER_HEIGHT) * width_ + x + 1] = 0xFF;

				dst[i * width_ + x + 4 * QUATER_WIDTH - 1] = 0xFF;
				dst[i * width_ + x + 4 * QUATER_WIDTH] = 0xFF;
				dst[i * width_ + x + 4 * QUATER_WIDTH + 1] = 0xFF;
				dst[(i + 3 * QUATER_HEIGHT) * width_ + x + 4 * QUATER_WIDTH - 1] = 0xFF;
				dst[(i + 3 * QUATER_HEIGHT) * width_ + x + 4 * QUATER_WIDTH] = 0xFF;
				dst[(i + 3 * QUATER_HEIGHT) * width_ + x + 4 * QUATER_WIDTH + 1] = 0xFF;
			}
		}
	}
}