#pragma once

struct rgb
{
	__int32 r : 5;
	__int32 g : 6;
	__int32 b : 5;
	__int32 dummy : 16;
};

struct rgb24
{
	unsigned char r;
	unsigned char g;
	unsigned char b;
};