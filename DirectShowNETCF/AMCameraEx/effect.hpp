#pragma once

class CEffect
{
public:
	virtual ~CEffect()
	{
	}

	virtual void Apply(void * outPtr, void * grayScale, int width, int height) = 0;
};

class CEffectFactory
{
public:
	static CEffectFactory &Instance()
	{
		return instance_;
	}
	CEffect * CreateEffectExecutor(int type);
private:
	CEffectFactory()
	{
	}

	CEffectFactory(const CEffectFactory &factory);

	CEffectFactory &operator = (const CEffectFactory &factory);


	static CEffectFactory instance_;
};

class CGrayScaleEffect : public CEffect
{
public:
	CGrayScaleEffect()
	{
	}

	~CGrayScaleEffect()
	{
	}

	void Apply(void * outPtr, void * grayScale, int width, int height)
	{
		memcpy(outPtr, grayScale, 3 * width * height);
	}
};

class CBlackAndWhiteEffect : public CEffect
{
public:
	CBlackAndWhiteEffect()
	{
	}

	~CBlackAndWhiteEffect()
	{
	}

	void Apply(void * outPtr, void * grayScale, int width, int height);
};

class CSepiaEffect : public CEffect
{
public:
	CSepiaEffect()
	{
	}

	~CSepiaEffect()
	{
	}

	void Apply(void * outPtr, void * grayScale, int width, int height);
private:
	unsigned char clip(int value)
	{
		if (value > 255)
		{
			value = 255;
		}

		if (value < 0)
		{
			value = 0;
		}

		return value;
	}
};

class CEdgeDetectorEffect : public CEffect
{
public:
	CEdgeDetectorEffect()
	{
	}

	~CEdgeDetectorEffect()
	{
	}

	void Apply(void * outPtr, void * grayScale, int width, int height);
};
class CCropOvalEffect : public CEffect
{
public:
	CCropOvalEffect()
	{
	}

	~CCropOvalEffect()
	{
	}

	void Apply(void * outPtr, void * grayScale, int width, int height);
};
class CCropRectangleEffect : public CEffect
{
public:
	CCropRectangleEffect()
	{
	}

	~CCropRectangleEffect()
	{
	}

	void Apply(void * outPtr, void * grayScale, int width, int height);
};