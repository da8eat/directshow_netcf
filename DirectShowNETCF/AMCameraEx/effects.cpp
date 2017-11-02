#include "stdafx.h"
#include "effects.hpp"

CEffects::CEffects(bool YV12, int width, int height, bool rotate) : 
	isYV12(YV12), type_(None), width_(width), height_(height), rotate_(rotate), 
		omnia_(false), fix_(false)
{
	grayScale = static_cast<unsigned char *>(malloc(3 * width * height));
}

CEffects::~CEffects() 
{
	if (grayScale)
	{
		free(grayScale);
	}

	if (effect_)
	{
		delete effect_;
	}
}

void CEffects::Execute(void * inPtr, void * outPtr, void * preBuff)
{
	if (isYV12)
	{
		YV12toRGB24(inPtr, outPtr, preBuff);
	}
	else
	{
		RGB565toRGB24(inPtr, outPtr, preBuff);
	}

	apply(outPtr);
}

void CEffects::putBitmap(unsigned char * dst, const unsigned char * drawBuff, 
						 int drawWidth, const int &drawHeight, const int &x, 
						 const int &y, const rgb24 * transparentColor, const int &blend)
{
	int width = rotate_ ? height_ : width_;
	int height = rotate_ ? width_ : height_;

	if (drawBuff)
	{
		int drawWidth_ = drawWidth;
		int drawHeight_ = drawHeight;
		if (x + drawWidth > width)
		{
			drawWidth_ = width - x;
		}

		if (y + drawHeight > height)
		{
			drawHeight_ = height - y;
		}

		// this is definitely bug :( will fix it one day
		// we should not increase width (this is probably even UB. we should just count correct stride
		drawWidth = (drawWidth % 4) ? ((drawWidth >> 2) + 1) << 2 : drawWidth;
		unsigned stride = (drawWidth % 4) ? (((3 * drawWidth) >> 2) + 1) << 2 : 3 * drawWidth;

		rgb24 * dst_ = reinterpret_cast<rgb24 *>(dst);
		const rgb24 * src_ = reinterpret_cast<const rgb24 *>(drawBuff);

		for (int i = y; i < drawHeight_ + y; ++i)
		{
			if (!transparentColor)
			{
				if (blend)
				{
					for (int j = x; j < drawWidth_ + x; ++j)
					{
						src_ =  reinterpret_cast<const rgb24 *>(&drawBuff[(i - y) * stride + 3 * (j - x)]);
						unsigned pixel = 
							dst_[(height - i) * width + j].b * (255 - blend) * 257 + src_ -> b * blend * 257;
						
						dst_[(height - i) * width + j].b = static_cast<unsigned char>(pixel >> 16);

						pixel = 
							dst_[(height - i) * width + j].g * (255 - blend) * 257 + src_ -> g * blend * 257;

						dst_[(height - i) * width + j].g = static_cast<unsigned char>(pixel >> 16);

						pixel = 
							dst_[(height - i) * width + j].r * (255 - blend) * 257 + src_ -> r * blend * 257;

						dst_[(height - i) * width + j].r = static_cast<unsigned char>(pixel >> 16);
					}
				}
				else
				{
					memcpy(dst + (height - i) * width * 3 + x * 3, drawBuff + (i - y) * stride, drawWidth_ * 3);
				}
			}
			else
			{
				for (int j = x; j < drawWidth_ + x; ++j)
				{
					if ((src_[(i - y) * drawWidth + (j - x)].b != transparentColor -> b) ||
						(src_[(i - y) * drawWidth + (j - x)].g != transparentColor -> g) ||
						(src_[(i - y) * drawWidth + (j - x)].r != transparentColor -> r))
					{
						dst_[(height - i) * width + j].b = src_[(i - y) * drawWidth + (j - x)].b;
						dst_[(height - i) * width + j].g = src_[(i - y) * drawWidth + (j - x)].g;
						dst_[(height - i) * width + j].r = src_[(i - y) * drawWidth + (j - x)].r;
					}
				}
			}
		}
	}
}

void CEffects::apply(void * buffer)
{
	if (effect_)
	{
		effect_ -> Apply(buffer, grayScale, rotate_ ? height_ : width_, rotate_ ? width_ : height_);
	}
}

void CEffects::YV12toRGB24(void * src, void * dst, void * preBuff)
{
	unsigned char * Y = static_cast<unsigned char *>(src);
	unsigned char * V = static_cast<unsigned char *>(src) + width_ * height_;
	unsigned char * U = V;
	if (!fix_)
	{
		U += (width_ * height_ >> 2);
	}

	unsigned char * rgb24_ = static_cast<unsigned char *>(dst);

	for (int i = 0; i < width_; i++)
	{
		for (int j = 0; j < height_; j++)
		{
			int C = Y[j * width_ + i] - 16;
			int D = int();
			int E = int();
			if (fix_)
			{
				D = U[2*((j / 2) * (width_ / 2) + (i / 2)) + 1] - 128;
				E = V[2*((j / 2) * (width_ / 2) + (i / 2))] - 128;
			}
			else
			{
				D = U[(j / 2) * (width_ / 2) + (i / 2)] - 128;
				E = V[(j / 2) * (width_ / 2) + (i / 2)] - 128;
			}

			int r = (298 * C           + 409 * E + 128) >> 8;
			int g = (298 * C - 100 * D - 208 * E + 128) >> 8;
			int b = (298 * C + 516 * D           + 128) >> 8;

			if (r > 255)
				r = 255;

			if (g > 255)
				g = 255;

			if (b > 255)
				b = 255;

			if (r < 0)
				r = 0;

			if (g < 0)
				g = 0;

			if (b < 0)
				b = 0;

			if (!rotate_)
			{
				rgb24_[3 * width_ * (height_ - j - 1) + 3 * i + 0] = b;
				rgb24_[3 * width_ * (height_ - j - 1) + 3 * i + 1] = g;
				rgb24_[3 * width_ * (height_ - j - 1) + 3 * i + 2] = r;
			
				grayScale[3 * width_ * (height_ - j - 1) + 3 * i + 0] = Y[j * width_ + i];
				grayScale[3 * width_ * (height_ - j - 1) + 3 * i + 1] = Y[j * width_ + i];
				grayScale[3 * width_ * (height_ - j - 1) + 3 * i + 2] = Y[j * width_ + i];
			}
			else
			{
				if (!omnia_)
				{
					rgb24_[3 * j + 3 * (width_ - i - 1) * height_ + 0] = b;
					rgb24_[3 * j + 3 * (width_ - i - 1) * height_ + 1] = g;
					rgb24_[3 * j + 3 * (width_ - i - 1) * height_ + 2] = r;
			
					grayScale[3 * j + 3 * (width_ - i - 1) * height_ + 0] = Y[j * width_ + i];
					grayScale[3 * j + 3 * (width_ - i - 1) * height_ + 1] = Y[j * width_ + i];
					grayScale[3 * j + 3 * (width_ - i - 1) * height_ + 2] = Y[j * width_ + i];
				}
				else
				{
					rgb24_[3 * (height_ - j - 1) + 3 * (width_ - i - 1) * height_ + 0] = b;
					rgb24_[3 * (height_ - j - 1) + 3 * (width_ - i - 1) * height_ + 1] = g;
					rgb24_[3 * (height_ - j - 1) + 3 * (width_ - i - 1) * height_ + 2] = r;
			
					grayScale[3 * (height_ - j - 1) + 3 * (width_ - i - 1) * height_ + 0] = Y[j * width_ + i];
					grayScale[3 * (height_ - j - 1) + 3 * (width_ - i - 1) * height_ + 1] = Y[j * width_ + i];
					grayScale[3 * (height_ - j - 1) + 3 * (width_ - i - 1) * height_ + 2] = Y[j * width_ + i];
				}
			}
		}
	}

	memcpy(preBuff, dst, width_ * height_ * 3);
}

void CEffects::RGB565toRGB24(void * src, void * dst, void * preBuff)
{
	unsigned char * rgb24_ = static_cast<unsigned char *>(dst);
	unsigned char * rgb565_ = static_cast<unsigned char *>(src);
	for (int i = 0; i < width_; i++)
	{
		for (int j = 0; j < height_; j++)
		{
			unsigned short rgb_ = static_cast<unsigned short>(rgb565_[2 * j * width_ + 2 * i] + rgb565_[2 * j * width_ + 2 * i + 1] * 256);
			unsigned char r = (rgb_ >> 11) << 3;
			unsigned char g = ((rgb_ << 5) >> 10) << 2;
			unsigned char b = (rgb_ & 31) << 3;

			

			if (!rotate_)
			{
				rgb24_[3 * j * width_ + i * 3 + 0] = b;
				rgb24_[3 * j * width_ + i * 3 + 1] = g;
				rgb24_[3 * j * width_ + i * 3 + 2] = r;
				//rgb24_[3 * (height_ - j - 1) * width_ + i * 3 + 0] = b;
				//rgb24_[3 * (height_ - j - 1) * width_ + i * 3 + 1] = g;
				//rgb24_[3 * (height_ - j - 1) * width_ + i * 3 + 2] = r;

				int gr = ((66 * r + 129 * g +  25 * b + 128) >> 8) +  16;

				grayScale[3 * j * width_ + i * 3 + 0] = gr;
				grayScale[3 * j * width_ + i * 3 + 1] = gr;
				grayScale[3 * j * width_ + i * 3 + 2] = gr;
			}
			else
			{
				rgb24_[3 * j + 3 * (width_ - i - 1) * height_ + 0] = b;
				rgb24_[3 * j + 3 * (width_ - i - 1) * height_ + 1] = g;
				rgb24_[3 * j + 3 * (width_ - i - 1) * height_ + 2] = r;
				//rgb24_[3 * j + 3 * i * height_ + 0] = b;
				//rgb24_[3 * j + 3 * i * height_ + 1] = g;
				//rgb24_[3 * j + 3 * i * height_ + 2] = r;

				int gr = ((66 * r + 129 * g +  25 * b + 128) >> 8) +  16;

				grayScale[3 * j + 3 * (width_ - i - 1) * height_ + 0] = gr;
				grayScale[3 * j + 3 * (width_ - i - 1) * height_ + 1] = gr;
				grayScale[3 * j + 3 * (width_ - i - 1) * height_ + 2] = gr;
			}
			


		}
	}

	memcpy(preBuff, dst, width_ * height_ * 3);
}