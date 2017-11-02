#pragma once

class CDrawHelper
{
public:
	virtual ~CDrawHelper() 
	{
	}

	virtual void DrawText(const void * textPtr, void * pBuff, int textWidth, int textHeight) = 0;
	virtual void DrawTarget(int target, void * pBuff, RECT rect) = 0;
protected:
	enum TargetType
	{
		RECTANGLE = 0x00000002,
		TARGET = 0x00000004,
		CROSS = 0x00000008
	};
};

class CRGB565DrawHelper : public CDrawHelper
{
public:
	CRGB565DrawHelper(int width, int height) : width_(width), height_(height)
	{
	}

	~CRGB565DrawHelper()
	{
	}

	void DrawText(const void * textPtr, void * pBuff, int textWidth, int textHeight);
	void DrawTarget(int target, void * pBuff, RECT rect);
private:
	int width_;
	int height_;
};

class CYV12DrawHelper : public CDrawHelper
{
public:
	CYV12DrawHelper(int width, int height) : width_(width), height_(height)
	{
	}

	~CYV12DrawHelper()
	{
	}

	void DrawText(const void * textPtr, void * pBuff, int textWidth, int textHeight);
	void DrawTarget(int target, void * pBuff, RECT rect);
private:
	int width_;
	int height_;
};