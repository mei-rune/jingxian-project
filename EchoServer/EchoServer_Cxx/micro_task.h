#pragma once

class micro_task
{
public:

	micro_task( micro_task& task, int headsize = 1024 * 1000)
		: _task( &task )
		, _is_convert( false )
	{
		_cothread = CreateFiber(headsize, run_main, this );
	}

	virtual ~micro_task()
	{
		if( _is_convert )
			::ConvertFiberToThread( );
		else
			::DeleteFiber( _cothread );
	}

	void switch_to( )
	{
	  SwitchToFiber( _cothread );
	}

	void yield()
	{	
		if( NULL != _task)
			_task->switch_to();
	}

	virtual void run()
	{
	}

	virtual void on_exit()
	{
	}
	
	static void WINAPI run_main( void* ptr)
	{
		try
		{
			((micro_task*)ptr)->run();
		}
		catch( ... )
		{}

		((micro_task*)ptr)->on_exit();
		((micro_task*)ptr)->yield();
	}
protected:
	
	micro_task(  )
		: _task( NULL )
		, _is_convert( false )
	{
		_cothread = GetCurrentFiber();
		if ((LPVOID) 0x1e00 == _cothread)
		{
			ConvertThreadToFiber( 0 );
			_is_convert = true;
			_cothread = GetCurrentFiber();
		}
	}

private:
	micro_task* _task;
	bool _is_convert;
	void* _cothread;
};

class main_task : public micro_task
{
public:
	main_task(  )
	{
	}

	void runForever()
	{
		try
		{
			run();
		}
		catch( ... )
		{}

		on_exit();
	}
};