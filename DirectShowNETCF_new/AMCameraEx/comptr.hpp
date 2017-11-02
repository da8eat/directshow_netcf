#pragma once

template <typename Type>
class ComPtr
{
public:
	ComPtr() : foo_(0)
	{
	}
	ComPtr(Type * v)
	{
		foo_ = v;
	}

	~ComPtr()
	{
		if (foo_)
		{		
			foo_ -> Release();
			foo_ = 0;
		}
	}

	ComPtr(const ComPtr &v)
	{
		foo_ = v.foo_;
		foo_ -> AddRef();
	}

	ComPtr &operator = (const ComPtr &v)
	{
		if (foo_)
			foo_ -> Release();

		foo_ = v.foo_;
		foo_ -> AddRef();

		return *this;
	}

	ComPtr &operator = (Type * v)
	{
		if (foo_)
			foo_ -> Release();

		foo_ = v;
		return *this;
	}
public:
	Type * operator -> () const
	{
		return foo_;
	}
public:
	void ** getVoidRef()
	{
		return reinterpret_cast<void **>(&foo_);
	}

	Type ** getRef()
	{
		return &foo_;
	}

	Type * get() const
	{
		return foo_;
	}

	bool isValid() const
	{
		return foo_ != 0;
	}

	void clear()
	{
		if (foo_)
		{
			foo_ -> Release();
			foo_ = 0;
		}
	}
private:
	Type * foo_;
};