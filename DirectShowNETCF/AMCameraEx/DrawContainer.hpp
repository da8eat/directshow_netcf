#pragma once

#include "rgb.hpp"

class CDrawContainer
{
public:
	CDrawContainer(const int id, const int &x, const int &y, const int &drawWidth, const int &drawHeight, const rgb24 * color, const void * buff) :
		x_(x), y_(y), drawWidth_(drawWidth), drawHeight_(drawHeight), transparentColor(0), id_(id), blend_(0), doBlend_(false)
	{
		unsigned stride = (drawWidth % 4) ? (((3 * drawWidth) >> 2) + 1) << 2 : 3 * drawWidth;
		drawBuff = static_cast<unsigned char *>(malloc(stride * drawHeight));
		memcpy(drawBuff, buff, stride * drawHeight);
		if (color)
		{
			transparentColor = static_cast<rgb24 *>(malloc(sizeof(rgb24)));
			memcpy(transparentColor, color, sizeof(rgb24));
		}
	}

	CDrawContainer(const CDrawContainer &v) : x_(v.x_), y_(v.y_), drawWidth_(v.drawWidth_), 
		drawHeight_(v.drawHeight_), transparentColor(0), id_(v.id_), blend_(v.blend_), doBlend_(v.doBlend_)
	{
		drawBuff = static_cast<unsigned char *>(malloc(drawWidth_ * drawHeight_ * 3));
		memcpy(drawBuff, v.drawBuff, drawWidth_ * drawHeight_ * 3);
		if (v.transparentColor)
		{
			transparentColor = static_cast<rgb24 *>(malloc(sizeof(rgb24)));
			memcpy(transparentColor, v.transparentColor, sizeof(rgb24));
		}
	}


	~CDrawContainer()
	{
		free(transparentColor);
		free(drawBuff);
		transparentColor = 0;
		drawBuff = 0;
	}

	int getX() const
	{
		return x_;
	}

	int getY() const
	{
		return y_;
	}

	int getWidth() const
	{
		return drawWidth_;
	}

	int getHeight() const
	{
		return drawHeight_;
	}

	rgb24 * getColor() const
	{
		return transparentColor;
	}

	unsigned char * getBuff() const
	{
		return drawBuff;
	}

	int getBlend() const
	{
		if (doBlend_)
			return blend_;

		return 0;
	}

	bool isEqualId(int id)
	{
		return id == id_;
	}

	void setBlendValue(const int &value)
	{
		blend_ = value;
		doBlend_ = true;
	}
private:
	int x_, y_, drawWidth_, drawHeight_;
	int blend_;
	bool doBlend_;
	rgb24 * transparentColor;
	unsigned char * drawBuff;
	int id_;
};