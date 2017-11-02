#include "stdafx.h"
#include "effect.hpp"
#include "effects.hpp"
#include "rgb.hpp"

CEffectFactory CEffectFactory::instance_;

CEffect * CEffectFactory::CreateEffectExecutor(int type)
{
	if (!type)
	{
		return 0;
	}

	if (type & GrayScale)
	{
		return new CGrayScaleEffect();
	}

	if (type & Binarization)
	{
		return new CBlackAndWhiteEffect();
	}

	if (type & Sepia)
	{
		return new CSepiaEffect();
	}

	if (type & EdgeDetection)
	{
		return new CEdgeDetectorEffect();
	}

	if (type & CropOval)
	{
		return new CCropOvalEffect();
	}

	return 0;
}

void CBlackAndWhiteEffect::Apply(void * outPtr, void * grayScale, int width, int height)
{
	int widthM1 = width - 1;
	int heightM1 = height - 1;
	int stride = width;
	double ex, ey, weight, weightTotal = 0, total = 0;

	rgb24 * src = static_cast<rgb24 *>(grayScale);
	unsigned char * dst = static_cast<unsigned char *>(outPtr);

	// skip the first line for the first pass
	src += stride;

	// for each line
	for ( int y = 1; y < heightM1; y++ )
	{
		src++;
		// for each pixels
		for ( int x = 1; x < widthM1; x++, src++ )
		{
					
			ex = src[1].r - src[-1].r;
			ey = src[stride].r - src[-stride].r;
			weight = ( ex > ey ) ? ex : ey;
			weightTotal += weight;
			total += weight * ( (*src).r );
		}
		src++;
	}

	unsigned char threshold = ( weightTotal == 0 ) ? 0 : static_cast<unsigned char>( total / weightTotal );
	unsigned char * src2 = static_cast<unsigned char *>(grayScale);

	for (int i = 0; i < 3 * width * height; i++, dst++, src2++)
	{
		*dst = (*src2 <= threshold) ? 0 : 255;
	}
}

void CSepiaEffect::Apply(void * outPtr, void * grayScale, int width, int height)
{
	unsigned char * src = static_cast<unsigned char *>(grayScale);
	unsigned char * dst = static_cast<unsigned char *>(outPtr);

	for (int i = 0; i < height; i ++)
	{
		for (int j = 0; j < width; j++, dst += 3, src += 3)
		{
			*(dst + 2) = clip(*src + 49);
			*(dst + 1) = clip(*src - 14);
			*dst = clip(*src - 56);
		}
	}
}

void CEdgeDetectorEffect::Apply(void * outPtr, void * grayScale, int width, int height)
{
	int widthM1 = width - 1;
	int heightM1 = height - 1;
	int stride = width;

	int d = 0;
	int max = 0;

	rgb24 * src = static_cast<rgb24 *>(grayScale);
	rgb24 * dst = static_cast<rgb24 * >(outPtr);

	// skip one stride
	src += stride;
	dst += stride;

	// for each line
	for ( int y = 1; y < heightM1; y++ )
	{
		src++;
		dst++;
		// for each pixel
		for ( int x = 1; x < widthM1; x++, src++, dst++ )
		{
			max = 0;

			// left diagonal
			d = src[-stride - 1].r - src[stride + 1].r;
			if ( d < 0 )
				d = -d;
			if ( d > max )
				max = d;

			// right diagonal
			d = src[-stride + 1].r - src[stride - 1].r;
			if ( d < 0 )
				d = -d;
			if ( d > max )
				max = d;

			// vertical
			d = src[-stride].r - src[stride].r;
			if ( d < 0 )
				d = -d;
			if ( d > max )
				max = d;

			// horizontal
			d = src[-1].r - src[1].r;
			if ( d < 0 )
				d = -d;
			if ( d > max )
				max = d;

			dst -> r = static_cast<unsigned char>(max);
			dst -> g = static_cast<unsigned char>(max);
			dst -> b = static_cast<unsigned char>(max);
		}
		++src;
		++dst;
	}
}


void CCropOvalEffect::Apply(void * outPtr, void * grayScale, int width, int height)
{
	int a = width >> 1;
	int b = height >> 1;
	int a2 = a * a;
	int b2 = b * b;
	int a2b2 = a2 * b2;
	
	rgb24 * image = static_cast<rgb24 *>(outPtr);
	for (int i = 0; i < height; ++i)
	{
		for (int j = 0; j < width; ++j)
		{
			if (a2 * (i - b) * (i - b) + b2 * (j - a) * (j - a) > a2b2)
			{
				image[i * width + j].r = 0;
				image[i * width + j].g = 0;
				image[i * width + j].b = 0;
			}
		}
	}
}

void CCropRectangleEffect::Apply(void * outPtr, void * grayScale, int width, int height)
{
	int a = width >> 1;
	int b = height >> 1;

	rgb24 * image = static_cast<rgb24 *>(outPtr);
	for (int i = 0; i < height; ++i)
	{
		for (int j = 0; j < width; ++j)
		{
		}
	}
}