#pragma once

#include "effect.hpp"
#include "rgb.hpp"

enum EffectType
{
	None = 0,
	GrayScale = 1,
	Binarization = 2,
	Sepia = 4,
	EdgeDetection = 8,
	CropOval = 16
};

class CEffects
{
public:
	CEffects(bool YV12, int width, int height, bool rotate);
	~CEffects();
	void Execute(void * inPtr, void * outPtr, void * preBuff);
	void SetType(int type)
	{
		if (type_ != type)
		{
			type_ = type;
			if (effect_)
			{
				delete effect_;
				effect_ = 0;
			}

			effect_ = CEffectFactory::Instance().CreateEffectExecutor(type);
		}
	}

	int GetType() const
	{
		return type_;
	}

	void setOmniaFlag()
	{
		omnia_ = true;
	}

	void putBitmap(unsigned char * dst, const unsigned char * drawBuff, int drawWidth, const int &drawHeight, const int &x, const int &y, const rgb24 * transparentColor, const int &blend);
	void FixPreview(bool fix)
	{
		fix_ = fix;
	}
private:
	int type_;
	unsigned char * grayScale;
	bool isYV12;
	bool rotate_;
	bool omnia_;
	bool fix_;
	int width_;
	int height_;
	CEffect * effect_;

	void RGB565toRGB24(void * src, void * dst, void * preBuff);
	void YV12toRGB24(void * src, void * dst, void * preBuff);
	void apply(void * buffer);
};
