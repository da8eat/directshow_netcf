#pragma once

struct rgb
{
	int r : 5;
	int g : 6;
	int b : 5;
	int dummy : 16;
};

struct rgb24
{
	unsigned char r;
	unsigned char g;
	unsigned char b;
};